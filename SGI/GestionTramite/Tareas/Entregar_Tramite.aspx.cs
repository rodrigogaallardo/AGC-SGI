using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.GestionTramite.Controls;
using SGI.Model;
using System.Data;
using System.Threading.Tasks;

namespace SGI.GestionTramite.Tareas
{
    public partial class Entregar_Tramite : System.Web.UI.Page
    {
        #region cargar inicial

        protected async void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(),
                    "script_inicial", "inicializar_controles();", true);

            if (!IsPostBack)
            {
                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    await CargarDatosTramite(id_tramitetarea);
            }
            
            if (!ucProcesosSADE.isEmptyGrid())
                ucSGI_DocumentoAdjunto.ocultarDocAdjColumnaEliminar(true);

        }

        private async Task CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            IniciarEntity();
            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                FinalizarEntity();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;
            WebUtil.EstadoControles(pnl_area_tarea.Controls, IsEditable);

            this.id_tramitetarea = id_tramitetarea;
            this.id_tarea = tramite_tarea.id_tarea;
            int id_grupotramite = Shared.GetGruposDeTramite(id_tramitetarea);

            SGI_Tarea_Entregar_Tramite entregar_tramite = Buscar_Tarea(id_tramitetarea);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                this.id_solicitud = ttHAB.id_solicitud;
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                this.id_solicitud = ttTR.id_solicitud;
            }

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, false);
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                this.id_paquete = (from p in db.SGI_Tarea_Generar_Expediente_Procesos
                                   join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                   join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                   where tt_hab.id_solicitud == this.id_solicitud
                                   select p.id_paquete).FirstOrDefault();
                if(this.id_paquete==0)
                    this.id_paquete = (from p in db.SGI_SADE_Procesos
                                       join tt_hab in db.SGI_Tramites_Tareas_HAB on p.id_tramitetarea equals tt_hab.id_tramitetarea
                                       where tt_hab.id_solicitud == this.id_solicitud
                                       select p.id_paquete).FirstOrDefault();


                ucListaRubros.LoadData(this.id_solicitud);
                ucTramitesRelacionados.LoadData(this.id_solicitud);

                ucListaRubros.Visible = true;
                ucTramitesRelacionados.Visible = true;

                SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);

                ucProcesosSADE.id_grupo_tramite = (int) Constants.GruposDeTramite.HAB;
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                this.id_paquete = (from p in db.SGI_SADE_Procesos
                                  join tt_hab in db.SGI_Tramites_Tareas_TRANSF on p.id_tramitetarea equals tt_hab.id_tramitetarea
                                  where tt_hab.id_solicitud == this.id_solicitud
                                  select p.id_paquete).FirstOrDefault();

                ucListaRubros.Visible = false;
                ucTramitesRelacionados.Visible = false;

                Transf_Solicitudes sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);
                ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
            }

            cargar_personas_habilitadas(this.id_solicitud, id_grupotramite);
            cargar_expediente(userid, tramite_tarea);

            cargarCombo();
            if (entregar_tramite != null)
            {
                txtFechaEntregaTramite.Text = string.Format("{0:dd/MM/yyyy}", entregar_tramite.fecha_entrega_tramite);
                ucObservacionesTarea.Text = entregar_tramite.Observaciones;
                if (entregar_tramite.enviar_a != null)
                    ddlEnviar.SelectedValue = entregar_tramite.enviar_a;
            }
            else
            {
                //txtFechaEntregaTramite.Text = "";
                ucObservacionesTarea.Text = "";
            }

            
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = IsEditable && (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea));

            FinalizarEntity();

        }

        private void cargarCombo()
        {
            ddlEnviar.Items.Insert(0, new ListItem("DGFyC", Functions.GetParametroChar("SGI.DGFYC.SectorDestino")));
            ddlEnviar.Items.Insert(1, new ListItem("GUARDA", Functions.GetParametroChar("SGI.GUARDA.SectorDestino")));
        }

        private void cargar_expediente(Guid userid, SGI_Tramites_Tareas tramite_tarea)
        {
            
            txtNroExpediente.Text = Shared.GetNroExpediente(id_tramitetarea);
            
        }

        private int _id_tramitetarea = 0;
        public int id_tramitetarea
        {
            get
            {
                if (_id_tramitetarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _id_tramitetarea);
                }
                return _id_tramitetarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _id_tramitetarea = value;
            }
        }

        private int id_paquete
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_paquete"] != null ? Convert.ToInt32(ViewState["_id_paquete"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_paquete"] = value;
            }

        }

        private int id_tarea
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_tarea"] != null ? Convert.ToInt32(ViewState["_id_tarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_tarea"] = value;
            }

        }
        private int _id_solicitud = 0;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        private SGI_Tarea_Entregar_Tramite Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Entregar_Tramite entregar_tramite =
                (
                    from ent_tra in db.SGI_Tarea_Entregar_Tramite
                    where ent_tra.id_tramitetarea == id_tramitetarea
                    orderby ent_tra.id_entregar_tramite descending
                    select ent_tra
                ).ToList().FirstOrDefault();

            return entregar_tramite;
        }

        #endregion

        #region acciones

        private Nullable<DateTime> fechaEntrega = null;
        private Nullable<int> nroExpediente = null;
        private Nullable<int> anioExpediente = null;


        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
            string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }


        private void Validar_fecha_entrega()
        {
            this.fechaEntrega = null;
            DateTime fecha_aux;

            if (txtFechaEntregaTramite.Text != null && txtFechaEntregaTramite.Text.Trim().Length > 0)
            {
                if (!DateTime.TryParse(txtFechaEntregaTramite.Text.Trim(), out fecha_aux))
                    throw new Exception("Fecha de entrega inválida."); 
                if (fecha_aux > DateTime.Today)
                    throw new Exception("Fecha de entrega inválida. No puede ser superior al día actual.");
                this.fechaEntrega = fecha_aux;
            }

        }

        private void Validar_expediente()
        {
            this.nroExpediente = null;
            this.anioExpediente = null;

            if (string.IsNullOrEmpty(txtNroExpediente.Text))
            {
                throw new Exception("Expediente inválido.");
            }

        }

        private void Validar_Guardar()
        {
            if (!string.IsNullOrEmpty( txtFechaEntregaTramite.Text))
                Validar_fecha_entrega();

            Validar_expediente();
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, int? nro_expediente, int? anio_expendinte, DateTime? fecha_entrega, string enviar_a, Guid userId)
        {

            SGI_Tarea_Entregar_Tramite entregar_tramite = Buscar_Tarea(id_tramite_tarea);

            int id_entregar_tramite = 0;
            if (entregar_tramite != null)
                id_entregar_tramite = entregar_tramite.id_entregar_tramite;

            db.SGI_Tarea_Entregar_Tramite_Actualizar(id_entregar_tramite, id_tramite_tarea,
                        observacion, nro_expediente, anioExpediente, enviar_a, fecha_entrega,  userId);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Validar_Guardar();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, this.nroExpediente,
                                        this.anioExpediente, this.fechaEntrega, ddlEnviar.SelectedValue, userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. entregar_tramite-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

                }
                db.Dispose();

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        private void Validar_Finalizar()
        {
            Validar_fecha_entrega();

            Validar_expediente();

            if ( this.nroExpediente <= 0 )
                throw new Exception("Número de expediente inválido.");

            if (this.anioExpediente <= 0)
                throw new Exception("Año de expediente inválido.");

            IniciarEntityFiles();

            int cantFileCount = db.SGI_Tarea_Documentos_Adjuntos
                .Where(x => x.id_tramitetarea == this.id_tramitetarea
                && x.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Comparando).ToList().Count();

            FinalizarEntityFiles();
            if (cantFileCount == 0)
                throw new Exception("Tiene que subir el Comparendo.");

        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                IniciarEntity();

                Validar_Finalizar();

                Guid userid = Functions.GetUserId();

                Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, this.nroExpediente,
                        this.anioExpediente, this.fechaEntrega, ddlEnviar.SelectedValue, userid);
                db.SaveChanges();

                int id_tramitetarea_nuevo = 0;
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;

                if (!hayProcesosGenerados)
                {
                    if (id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Entregar_Tramite ||
                        id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Entregar_Tramite)
                        db.SGI_Tarea_Entrega_Tramite_GenerarProcesos_v4(this.id_tramitetarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    else if (this.id_tarea == (int)Constants.ENG_Tareas.SSP_Entrega_Tramite_v3)
                        db.SGI_Tarea_Entrega_Tramite_GenerarProcesos_v3(this.id_tramitetarea, this.id_paquete, userid);
                    else
                        db.SGI_Tarea_Entrega_Tramite_GenerarProcesos(this.id_tramitetarea, this.id_paquete, userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, true);
                }
                else if ( Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {

                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. entregar_tramite-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }
                    FinalizarEntity();

                    Enviar_Mensaje("Se ha finalizado la tarea.", "");

                Redireccionar_VisorTramite();
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Entregar trámite");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }
 
        #endregion

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
                this.db.Dispose();
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
                this.dbFiles.Dispose();
        }

        #endregion


        #region personas habilitadas para retirar tramite
        private void cargar_personas_habilitadas(int id_solicitud, int id_grupotramite)
        {
            List<PersonasHabilitadas> lstPersonasHabilitada = PersonasHabilitadas.CargarPersonasHabilitadas(id_solicitud, id_grupotramite);

            rptPersonasHabilitadas.DataSource = lstPersonasHabilitada;
            rptPersonasHabilitadas.DataBind();
        }

        protected void grdPersonasHabilitados_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].ColumnSpan = 3;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;

                GridView grdFirmantesHabilitados = (GridView)e.Row.FindControl("grdFirmantesHabilitados");
                grdFirmantesHabilitados.DataSource = this.personasHabilitadas.firmantes;
                grdFirmantesHabilitados.DataBind();

                e.Row.Visible = (this.personasHabilitadas.firmantes.Count > 0);
            }


        }

        PersonasHabilitadas personasHabilitadas;
        protected void rptPersonasHabilitadas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GridView grdPersonasHabilitados = (GridView)e.Item.FindControl("grdPersonasHabilitados");
               // GridView grdFirmantesHabilitados = (GridView)e.Item.FindControl("grdFirmantesHabilitados");

                this.personasHabilitadas = (PersonasHabilitadas)e.Item.DataItem;

                List<PersonasHabilitadas> lstpersonasHabilitadas = new List<PersonasHabilitadas>();
                lstpersonasHabilitadas.Add(this.personasHabilitadas);

                grdPersonasHabilitados.DataSource = lstpersonasHabilitadas;
                grdPersonasHabilitados.DataBind();

            }

        }

        public class PersonasHabilitadas
        {
            public string TipoPersona { get; set; }
            public string TipoPersonaDesc { get; set; }
            public int id { get; set; }
            public string ApellidoNomRazon { get; set; }
            public string cuit { get; set; }
            public string Domicilio { get; set; }
            public List<FirmantesHabilitados> firmantes { get; set; }

            public void SetDomicilio(string calle, int puerta, string piso, string depto)
            {
                string piso_aux = "";
                string depto_aux = "";

                this.Domicilio = calle;

                if (puerta > 0)
                {
                    this.Domicilio = string.IsNullOrEmpty(this.Domicilio) ? puerta.ToString() : this.Domicilio + " " + puerta.ToString();
                }

                if (!string.IsNullOrEmpty(piso))
                    piso_aux = " Piso: " + piso;

                if (!string.IsNullOrEmpty(depto))
                    depto_aux = " Depto: " + depto;


                if (!string.IsNullOrEmpty(depto))
                    depto_aux = " Depto: " + depto;

                this.Domicilio = this.Domicilio + piso_aux + depto_aux;

            }


            public static List<PersonasHabilitadas> CargarPersonasHabilitadas(int id_solicitud, int id_grupotramite)
            {
                List<PersonasHabilitadas> lstPersonasHabilitadas = new List<PersonasHabilitadas>();
                PersonasHabilitadas personas;

                DGHP_Entities db = new DGHP_Entities();

                if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    // cargar titulares fisicas 
                    var fisica =
                    (
                        from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                        where pf.id_solicitud == id_solicitud
                        select new
                        {
                            pf.id_personafisica,
                            pf.Apellido,
                            pf.Nombres,
                            pf.Cuit,
                            pf.Calle,
                            pf.Nro_Puerta,
                            pf.Piso,
                            pf.Depto
                        }
                    ).ToList();

                    foreach (var item in fisica)
                    {
                        personas = new PersonasHabilitadas();

                        personas.TipoPersona = "PF";
                        personas.TipoPersonaDesc = "Persona Física";
                        personas.id = item.id_personafisica;
                        personas.ApellidoNomRazon = item.Apellido + " " + item.Nombres;
                        personas.cuit = item.Cuit;
                        personas.SetDomicilio(item.Calle, item.Nro_Puerta, item.Piso, item.Depto);
                        personas.firmantes = FirmantesHabilitados.CargarFirmantesHabilitadas(id_solicitud, item.id_personafisica, id_grupotramite);

                        lstPersonasHabilitadas.Add(personas);
                    }

                    // cargar titulares juridicas
                    var juridica =
                        (
                            from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                            where pj.id_solicitud == id_solicitud
                            select new
                            {
                                pj.id_personajuridica,
                                pj.Razon_Social,
                                pj.CUIT,
                                pj.Calle,
                                pj.NroPuerta,
                                pj.Piso,
                                pj.Depto
                            }
                        ).ToList();

                    foreach (var item in juridica)
                    {
                        personas = new PersonasHabilitadas();

                        personas.TipoPersona = "PJ";
                        personas.TipoPersonaDesc = "Persona Jurídica";
                        personas.id = item.id_personajuridica;
                        personas.ApellidoNomRazon = item.Razon_Social;
                        personas.cuit = item.CUIT;
                        personas.SetDomicilio(item.Calle, (int)item.NroPuerta, item.Piso, item.Depto);

                        personas.firmantes = FirmantesHabilitados.CargarFirmantesHabilitadas(id_solicitud, item.id_personajuridica, id_grupotramite);

                        lstPersonasHabilitadas.Add(personas);
                    }
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    // cargar titulares fisicas 
                    var fisica =
                    (
                        from pf in db.Transf_Titulares_PersonasFisicas
                        where pf.id_solicitud == id_solicitud
                        select new
                        {
                            pf.id_personafisica,
                            pf.Apellido,
                            pf.Nombres,
                            pf.Cuit,
                            pf.Calle,
                            pf.Nro_Puerta,
                            pf.Piso,
                            pf.Depto
                        }
                    ).ToList();

                    foreach (var item in fisica)
                    {
                        personas = new PersonasHabilitadas();

                        personas.TipoPersona = "PF";
                        personas.TipoPersonaDesc = "Persona Física";
                        personas.id = item.id_personafisica;
                        personas.ApellidoNomRazon = item.Apellido + " " + item.Nombres;
                        personas.cuit = item.Cuit;
                        personas.SetDomicilio(item.Calle, item.Nro_Puerta, item.Piso, item.Depto);
                        personas.firmantes = FirmantesHabilitados.CargarFirmantesHabilitadas(id_solicitud, item.id_personafisica, id_grupotramite);

                        lstPersonasHabilitadas.Add(personas);
                    }

                    // cargar titulares juridicas
                    var juridica =
                        (
                            from pj in db.Transf_Titulares_PersonasJuridicas
                            where pj.id_solicitud == id_solicitud
                            select new
                            {
                                pj.id_personajuridica,
                                pj.Razon_Social,
                                pj.CUIT,
                                pj.Calle,
                                pj.NroPuerta,
                                pj.Piso,
                                pj.Depto
                            }
                        ).ToList();

                    foreach (var item in juridica)
                    {
                        personas = new PersonasHabilitadas();

                        personas.TipoPersona = "PJ";
                        personas.TipoPersonaDesc = "Persona Jurídica";
                        personas.id = item.id_personajuridica;
                        personas.ApellidoNomRazon = item.Razon_Social;
                        personas.cuit = item.CUIT;
                        personas.SetDomicilio(item.Calle, (int)item.NroPuerta, item.Piso, item.Depto);

                        personas.firmantes = FirmantesHabilitados.CargarFirmantesHabilitadas(id_solicitud, item.id_personajuridica, id_grupotramite);

                        lstPersonasHabilitadas.Add(personas);
                    }
                }

                db.Dispose();

                return lstPersonasHabilitadas;
            }


        }

        public class FirmantesHabilitados
        {

            public string TipoPersona { get; set; }
            public string Titular { get; set; }
            public string ApellidoNombres { get; set; }
            public string DescTipoDocPersonal { get; set; }
            public string Nro_Documento { get; set; }
            public string nom_tipocaracter { get; set; }
            public string cargo_firmante_pj { get; set; }

            public static List<FirmantesHabilitados> CargarFirmantesHabilitadas(int id_solicitud, int id_persona, int id_grupotramite)
            {

                List<FirmantesHabilitados> lstFirmantesHabilitados = new List<FirmantesHabilitados>();

                FirmantesHabilitados firmante;

                DGHP_Entities db = new DGHP_Entities();
                if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    // cargar titulares juridicos 
                    var juridica =
                    (
                        from pj in db.SSIT_Solicitudes_Firmantes_PersonasJuridicas
                        join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                        join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                        join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                        where pj.id_solicitud == id_solicitud && pj.id_personajuridica == id_persona
                        select new
                        {
                            pj.id_firmante_pj,
                            titpj.Razon_Social,
                            pj.Apellido,
                            pj.Nombres,
                            DescTipoDocPersonal = tdoc.Nombre,
                            pj.Nro_Documento,
                            tcl.nom_tipocaracter,
                            pj.cargo_firmante_pj
                        }
                    ).ToList();


                    foreach (var item in juridica)
                    {
                        firmante = new FirmantesHabilitados();

                        firmante.TipoPersona = "PJ";
                        firmante.Titular = item.Razon_Social;
                        firmante.ApellidoNombres = item.Apellido + " " + item.Nombres;
                        firmante.DescTipoDocPersonal = item.DescTipoDocPersonal;
                        firmante.Nro_Documento = item.Nro_Documento;
                        firmante.nom_tipocaracter = item.nom_tipocaracter;
                        firmante.cargo_firmante_pj = item.cargo_firmante_pj;

                        lstFirmantesHabilitados.Add(firmante);
                    }

                    // cargar titulares fisicas 
                    var fisica =
                        (
                            from pf in db.SSIT_Solicitudes_Firmantes_PersonasFisicas
                            join titpf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on pf.id_personafisica equals titpf.id_personafisica
                            join tcl in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tcl.id_tipocaracter
                            join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                            where pf.id_solicitud == id_solicitud && pf.id_personafisica == id_persona
                            select new
                            {
                                pf.id_firmante_pf,
                                Titular = titpf.Apellido + ", " + titpf.Nombres,
                                pf.Apellido,
                                pf.Nombres,
                                DescTipoDocPersonal = tdoc.Nombre,
                                pf.Nro_Documento,
                                tcl.nom_tipocaracter
                            }
                        ).ToList();


                    foreach (var item in fisica)
                    {
                        firmante = new FirmantesHabilitados();
                        firmante.TipoPersona = "PF";
                        firmante.Titular = item.Titular;
                        firmante.ApellidoNombres = item.Apellido + " " + item.Nombres;
                        firmante.DescTipoDocPersonal = item.DescTipoDocPersonal;
                        firmante.Nro_Documento = item.Nro_Documento;
                        firmante.nom_tipocaracter = item.nom_tipocaracter;
                        firmante.cargo_firmante_pj = "";

                        lstFirmantesHabilitados.Add(firmante);
                    }
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    // cargar titulares juridicos 
                    var juridica =
                    (
                        from pj in db.Transf_Firmantes_PersonasJuridicas
                        join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                        join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                        join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                        where pj.id_solicitud == id_solicitud && pj.id_personajuridica == id_persona
                        select new
                        {
                            pj.id_firmante_pj,
                            titpj.Razon_Social,
                            pj.Apellido,
                            pj.Nombres,
                            DescTipoDocPersonal = tdoc.Nombre,
                            pj.Nro_Documento,
                            tcl.nom_tipocaracter,
                            pj.cargo_firmante_pj
                        }
                    ).ToList();


                    foreach (var item in juridica)
                    {
                        firmante = new FirmantesHabilitados();

                        firmante.TipoPersona = "PJ";
                        firmante.Titular = item.Razon_Social;
                        firmante.ApellidoNombres = item.Apellido + " " + item.Nombres;
                        firmante.DescTipoDocPersonal = item.DescTipoDocPersonal;
                        firmante.Nro_Documento = item.Nro_Documento;
                        firmante.nom_tipocaracter = item.nom_tipocaracter;
                        firmante.cargo_firmante_pj = item.cargo_firmante_pj;

                        lstFirmantesHabilitados.Add(firmante);
                    }

                    // cargar titulares fisicas 
                    var fisica =
                        (
                            from pf in db.Transf_Firmantes_PersonasFisicas
                            join titpf in db.Transf_Titulares_PersonasFisicas on pf.id_personafisica equals titpf.id_personafisica
                            join tcl in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tcl.id_tipocaracter
                            join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                            where pf.id_solicitud == id_solicitud && pf.id_personafisica == id_persona
                            select new
                            {
                                pf.id_firmante_pf,
                                Titular = titpf.Apellido + ", " + titpf.Nombres,
                                pf.Apellido,
                                pf.Nombres,
                                DescTipoDocPersonal = tdoc.Nombre,
                                pf.Nro_Documento,
                                tcl.nom_tipocaracter
                            }
                        ).ToList();


                    foreach (var item in fisica)
                    {
                        firmante = new FirmantesHabilitados();
                        firmante.TipoPersona = "PF";
                        firmante.Titular = item.Titular;
                        firmante.ApellidoNombres = item.Apellido + " " + item.Nombres;
                        firmante.DescTipoDocPersonal = item.DescTipoDocPersonal;
                        firmante.Nro_Documento = item.Nro_Documento;
                        firmante.nom_tipocaracter = item.nom_tipocaracter;
                        firmante.cargo_firmante_pj = "";

                        lstFirmantesHabilitados.Add(firmante);
                    }
                }
                db.Dispose();

                return lstFirmantesHabilitados;
            }

        }


        #endregion

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            int id_resultado = e.id_resultado.Value;
            var resultado = db.ENG_Resultados.FirstOrDefault(x => x.id_resultado == id_resultado);
            if (resultado.nombre_resultado == "Devolver a Aprobados")
            {
                rfv_txtFechaEntregaTramite.Enabled = false;
                rev_txtFechaEntregaTramite.Enabled = false;
            }
            else
            {
                rfv_txtFechaEntregaTramite.Enabled = true;
                rev_txtFechaEntregaTramite.Enabled = true;
            }
            FinalizarEntity();
        }

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }
    }
}

public class Items
{
    public string Texto { get; set; }
    public string Codigo { get; set; }


    public Items()
    {
    }


    public Items(string texto, string codigo)
    {
        this.Texto = texto;
        this.Codigo = codigo;
    }

    public Items(string texto, int codigo)
    {
        this.Texto = texto;
        this.Codigo = codigo.ToString();
    }

    public override bool Equals(object obj)
    {
        try
        {
            if (!(obj is Items))
                return false;

            if (obj != null && this.Codigo == ((Items)obj).Codigo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return this.Texto;
    }

    public string FullName()
    {
        return this.Texto + " - " + this.Codigo;
    }
}

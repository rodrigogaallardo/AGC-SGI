using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas
{
    public partial class Verificacion_IFCI : System.Web.UI.Page
    {
        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    CargarDatosTramite(id_tramitetarea);

            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        private void CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            IniciarEntity();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;
            var id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

            int cod_tarea_enviar = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Enviar_a_DGFyC);
            var id_tarea_enviar = db.ENG_Tareas.First(x => x.cod_tarea == cod_tarea_enviar).id_tarea;
            if (this.id_tarea== id_tarea_enviar)
            {
                int cod_tarea_revision = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP2);
                var t= db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_revision);
                if (t != null)
                    ucResultadoTarea.id_proximatarea_default = t.id_tarea;
            }


            SGI_Tarea_Verificacion_IFCI pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";

            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            
            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
            ucProcesosSADE.id_tarea = (int)Constants.ENG_Tareas.ESP_Verificacion_AVH;
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);
        }


        private int _tramiteTarea = 0;
        public int TramiteTarea
        {
            get
            {
                if (_tramiteTarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _tramiteTarea);
                }
                return _tramiteTarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _tramiteTarea = value;
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

        public int id_tarea
        {
            get
            {
                return (ViewState["_id_tarea"] != null ? Convert.ToInt32(ViewState["_id_tarea"]) : 0);
            }
            set
            {
                ViewState["_id_tarea"] = value.ToString();
            }
        }
        private SGI_Tarea_Verificacion_IFCI Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Verificacion_IFCI pvh =
                (
                    from env_phv in db.SGI_Tarea_Verificacion_IFCI
                    where env_phv.id_tramitetarea == id_tramitetarea
                    orderby env_phv.id_verificacion_IFCI descending
                    select env_phv
                ).FirstOrDefault();

            return pvh;
        }

        #endregion


        #region acciones

        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }


        private void Validar_Tarea()
        {
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Verificacion_IFCI pvh = Buscar_Tarea(id_tramite_tarea);

            int id_verificacion_IFCI = 0;
            if (pvh != null)
                id_verificacion_IFCI = pvh.id_verificacion_IFCI;

            db.SGI_Tarea_Verificacion_IFCI_Actualizar(id_verificacion_IFCI, id_tramite_tarea, observacion, userId);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                IniciarEntity();

                Validar_Tarea();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "error en transaccion. pvh-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }
                }
                FinalizarEntity();

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }

        }



        private void Validar_Finalizar()
        {
            var list_doc_adj =
                (
                    from adj in db.SGI_Tarea_Documentos_Adjuntos
                    where adj.id_tramitetarea == this.TramiteTarea
                    && adj.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Informe_IFCI
                    select new
                    {
                        adj
                    }
                );
            if (list_doc_adj.Count() == 0)
                throw new Exception("Debe subir el Informe de IFCI.");
        }

    
        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                if (!hayProcesosGenerados)
                {
                    if (this.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Verificacion_IFCI ||
                        this.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Verificacion_IFCI )
                        this.db.SGI_HAB_GenerarProcesos_SADE_v4(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid); 
                    else
                        this.db.SGI_Tarea_Verificacion_IFCI_GenerarProcesos(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    TransactionScope Tran = new TransactionScope();

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        if (Tran != null)
                            Tran.Dispose();
                        LogError.Write(ex, "error en transaccion. pvh-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
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
                string mensaje = Functions.GetErrorMessage(ex);
                Enviar_Mensaje(mensaje, "");
            }

        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
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
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
            ucResultadoTarea.btnFinalizar_Enabled = false;
        }
    }
}
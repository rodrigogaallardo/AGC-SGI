using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas
{
    public partial class Enviar_Procuracion : BasePage
    {
        #region "Propiedades"


        private int _tramiteTarea = 0;
        public int id_tramitetarea
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

        private int id_grupotramite
        {
            get
            {

                int ret = (ViewState["_id_grupotramite"] != null ? Convert.ToInt32(ViewState["_id_grupotramite"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_grupotramite"] = value.ToString();
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


        private SGI_Tarea_Enviar_Procuracion Buscar_Tarea(int id_tramitetarea)
        {
            var pvh = (from env_phv in db.SGI_Tarea_Enviar_Procuracion
                       where env_phv.id_tramitetarea == id_tramitetarea
                       orderby env_phv.id_enviar_procuracion descending
                       select env_phv
                      ).FirstOrDefault();

            return pvh;
        }
        private string _url_servicio_EE;
        private string url_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_url_servicio_EE))
                {
                    _url_servicio_EE = Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                }
                return _url_servicio_EE;
            }
        }
        private string _username_servicio_EE;
        private string username_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_username_servicio_EE))
                {
                    _username_servicio_EE = Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
                }
                return _username_servicio_EE;
            }
        }
        private string _pass_servicio_EE;
        private string pass_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_pass_servicio_EE))
                {
                    _pass_servicio_EE = Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
                }
                return _pass_servicio_EE;
            }
        }

        private string _sector_inicial_1;
        private string sector_inicial_1
        {
            get
            {
                if (string.IsNullOrEmpty(_sector_inicial_1))
                {
                    _sector_inicial_1 = Parametros.GetParam_ValorChar("SGI.EnviarProcuracion.SectorOrigen1");
                }
                return _sector_inicial_1;
            }
        }
        private string _sector_inicial_2;
        private string sector_inicial_2
        {
            get
            {
                if (string.IsNullOrEmpty(_sector_inicial_2))
                {
                    _sector_inicial_2 = Parametros.GetParam_ValorChar("SGI.EnviarProcuracion.SectorOrigen2");
                }
                return _sector_inicial_2;
            }
        }
        #endregion

        #region cargar inicial

        protected async Task Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    await CargarDatosTramite(id_tramitetarea);

            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        private async Task CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            IniciarEntity();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            bool puedeTrabajar = false;
            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            this.id_grupotramite = id_grupotramite;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.id_tramitetarea = id_tramitetarea;
            this.id_paquete = (from p in db.SGI_SADE_Procesos
                               join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                               join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                               where tt_hab.id_solicitud == this.id_solicitud
                               select p.id_paquete
                               ).FirstOrDefault();

            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            if (IsEditable && !hayProcesosGenerados)
            {
                string nroExp = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).Select(y => y.NroExpedienteSade).FirstOrDefault();
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                try
                {
                    ws_ExpedienteElectronico.consultaExpedienteResponseDetallado consultaExpedienteResponseDetallado = serviceEE.consultarExpedienteDetallado(this.username_servicio_EE, this.pass_servicio_EE, nroExp);
                    if (consultaExpedienteResponseDetallado.sectorDestino != sector_inicial_1 &&
                        consultaExpedienteResponseDetallado.sectorDestino != sector_inicial_2)
                    {
                        lblError.Text = string.Format("El expediente no se encuentra en el sector {0} o {1}" , sector_inicial_1, sector_inicial_2);
                        this.EjecutarScript(updFinalizarTarea, "showfrmError();");
                    }
                    else
                    {
                        puedeTrabajar = true;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updFinalizarTarea, "showfrmError();");
                }
            }else if (IsEditable && hayProcesosGenerados)
                puedeTrabajar = true;

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----

            ucResultadoTarea.Enabled = IsEditable && puedeTrabajar;
            ucObservacionesTarea.Enabled = IsEditable && puedeTrabajar;



            SGI_Tarea_Enviar_Procuracion pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";

            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            if (IsEditable)
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

        }

        private bool exist(string numeroGEDO, int id_paquete)
        {
            SGI_SADE_Procesos proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_paquete == id_paquete && x.resultado_ee==numeroGEDO);
            return proc != null;
        }
        #endregion


        #region acciones
        private void Enviar_Mensaje(UpdatePanel upd, string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            this.EjecutarScript(upd, "mostrarMensaje('" + mensaje + "','" + titulo + "')");

        }

        private void Redireccionar_VisorTramite()
        {
            //string url = string.Format("~/GestionTramite/VisorTramite.aspx?id={0}", this.id_solicitud);
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0); string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }


        private void Validar_Tarea()
        {
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId, int? id_resultado, int? id_proxima_tarea)
        {
            int id_enviar_procuracion = 0;
            SGI_Tarea_Enviar_Procuracion pvh = Buscar_Tarea(id_tramitetarea);

            if (pvh != null)
                id_enviar_procuracion = pvh.id_enviar_procuracion;

            db.SGI_Tarea_Enviar_Procuracion_Actualizar(id_enviar_procuracion, id_tramite_tarea, observacion, userId);

            if(id_resultado.HasValue || id_proxima_tarea.HasValue)
            {
                var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == this.id_tramitetarea);
                if(id_resultado.HasValue)
                    tt.id_resultado = id_resultado.Value;

                if (id_proxima_tarea.HasValue)
                    tt.id_proxima_tarea = id_proxima_tarea;

                db.SGI_Tramites_Tareas.Attach(tt);
                db.Entry(tt).State = System.Data.Entity.EntityState.Modified;
            }
            
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
                        
                        Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid,e.id_resultado,e.id_proxima_tarea);
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
        }


        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            try
            {

                Guid userid = Functions.GetUserId();
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == this.id_tramitetarea) > 0;

                if (!hayProcesosGenerados)
                {

                    string nroExp = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).Select(y => y.NroExpedienteSade).FirstOrDefault();
                    ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                    serviceEE.Url = this.url_servicio_EE;
                    try
                    { 
                        ws_ExpedienteElectronico.consultaExpedienteResponseDetallado consultaExpedienteResponseDetallado = serviceEE.consultarExpedienteDetallado(this.username_servicio_EE, this.pass_servicio_EE, nroExp);
                        if (consultaExpedienteResponseDetallado.sectorDestino != sector_inicial_1 &&
                            consultaExpedienteResponseDetallado.sectorDestino != sector_inicial_2)
                        {
                            lblError.Text = string.Format("El expediente no se encuentra en el sector {0} o {1}", sector_inicial_1, sector_inicial_2);
                            this.EjecutarScript(updFinalizarTarea, "showfrmError();");
                        }
                        else
                        {
                            string documentos = "";
                            string AcronimoProvidencia = Functions.GetParametroChar("EE.acronimo.pv");
                            foreach (string numeroGEDO in consultaExpedienteResponseDetallado.listDocumentosOficiales)
                            {
                                if (!exist(numeroGEDO, id_paquete) && !numeroGEDO.StartsWith(AcronimoProvidencia))    // no se deben evaluar las providencias
                                {
                                    if (string.IsNullOrEmpty(documentos))
                                        documentos = numeroGEDO;
                                    else
                                        documentos += "/" + numeroGEDO;
                                }
                            }

                            //Actualizar el resultado y la proxima tarea.
                            var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == this.id_tramitetarea);
                            if(e.id_resultado.HasValue)
                                tt.id_resultado = e.id_resultado.Value;
                            if(e.id_proxima_tarea.HasValue)
                                tt.id_proxima_tarea = e.id_proxima_tarea.Value;
                            db.SGI_Tramites_Tareas.Attach(tt);
                            db.Entry(tt).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            //genera los procesos
                            db.SGI_Tarea_Enviar_Procuracion_GenerarProcesos(this.id_tramitetarea, this.id_paquete, documentos, userid);

                            ucResultadoTarea.btnFinalizar_Enabled = false;
                            //Ejecuta los procesos en SADE
                            ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = Functions.GetErrorMessage(ex);
                        this.EjecutarScript(updFinalizarTarea, "showfrmError();");
                    }
                }
                else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {
                    TransactionScope Tran = new TransactionScope();

                    try
                    {
                        Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid,e.id_resultado,e.id_proxima_tarea);
                        db.SaveChanges();

                        ucResultadoTarea.FinalizarTarea();

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

                    Enviar_Mensaje(updFinalizarTarea, "Se ha finalizado la tarea.", "");

                    Redireccionar_VisorTramite();
                }
                else
                {
                    Enviar_Mensaje(updFinalizarTarea, "No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updFinalizarTarea, "showfrmError();");
            }
            finally
            {
                FinalizarEntity();
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
            ucResultadoTarea.btnFinalizar_Enabled = true;
            ucListaDocumentos.LoadData(this.id_grupotramite, this.id_solicitud);
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }

    }

}
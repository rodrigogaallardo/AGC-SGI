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
    public partial class Revision_Firma_Disposicion2 : BasePage
    {

        #region cargar inicial
        private int id_solicitud
        {
            get
            {

                int ret = (ViewState["_id_solicitud"] != null ? Convert.ToInt32(ViewState["_id_solicitud"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_solicitud"] = value.ToString();
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

        private int id_tramitetarea
        {
            get
            {

                int ret = (ViewState["_id_tramitetarea"] != null ? Convert.ToInt32(ViewState["_id_tramitetarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_tramitetarea"] = value.ToString();
            }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
            }

            if (!IsPostBack)
            {
                this.id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
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

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.id_tramitetarea = id_tramitetarea;
            this.id_grupotramite = id_grupotramite;
            int id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);

            ucListaObservacionesAnt.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnt.Visible = true;
            ucSGI_ListaDocumentoAdjuntoAnt.LoadData(id_grupotramite, this.id_solicitud, this.id_tramitetarea);
            ucSGI_ListaDocumentoAdjuntoAnt.Visible = true;

            ucResultadoTarea.btnGuardar_Visible = false;

            SGI_Tarea_Entregar_Tramite rev = Buscar_Tarea(id_tramitetarea);
            if (rev != null)
                ucObservacionesTarea.Text = rev.Observaciones;
            else
                ucObservacionesTarea.Text = "";

            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            if (IsEditable && !hayProcesosGenerados)
            {
                try
                {
                    if(id_tarea== (int)Constants.ENG_Tareas.ESCU_HP_Revision_Firma_Disposicion ||
                        id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Revision_Firma_Disposicion)
                        db.SGI_HAB_GenerarProcesos_SADE_Revision_Firma_Disposicion_v4(this.id_tramitetarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    else
                        db.SGI2_Cargar_Procesos_Revision_Firma_Disposicion(this.id_tramitetarea, userid);
                }
                catch (Exception ex)
                {

                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updCargarProcesos, "showfrmError();");
                    IsEditable = false;
                }
            }
            
            ucListaObservacionesAnterioresv1.Visible = true;
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);

            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
            ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, false);
            ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
            if (IsEditable)
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

            updControlesCabecera.Update();

            FinalizarEntity();
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
        private void Enviar_Mensaje(UpdatePanel upd, string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            this.EjecutarScript(upd, "mostrarMensaje('" + mensaje + "','" + titulo + "')");

        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            try
            {

                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;

                if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {

                    //Busco el numero especial de la dispo
                    ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                    ws_ExpedienteElectronico.documentosVinculadosResponse detalleDoc = new ws_ExpedienteElectronico.documentosVinculadosResponse();
                    serviceEE.Url = this.url_servicio_EE;
                    string nroExp = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).Select(y => y.NroExpedienteSade).FirstOrDefault();

                    var nroDispo = (from tt in db.SGI_Tramites_Tareas
                                    join tth in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tth.id_tramitetarea
                                    join sp in db.SGI_SADE_Procesos on tt.id_tramitetarea equals sp.id_tramitetarea
                                    where sp.realizado_en_SADE && sp.id_proceso == (int)Constants.EE_Procesos.FirmarDocumento_RevisarFirma && tth.id_solicitud == this.id_solicitud
                                    select new
                                    {
                                        sp.resultado_ee,
                                        sp.UpdateDate
                                    }).Union(from tt in db.SGI_Tramites_Tareas
                                             join tth in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tth.id_tramitetarea
                                             join tge in db.SGI_Tarea_Generar_Expediente_Procesos on tt.id_tramitetarea equals tge.id_tramitetarea
                                             where tge.realizado && tge.id_proceso == (int)Constants.EE_Procesos.FirmarDocumento_RevisarFirma && tth.id_solicitud == this.id_solicitud
                                             select new
                                             {
                                                 tge.resultado_ee,
                                                 tge.UpdateDate
                                             }).OrderByDescending(x => x.UpdateDate).FirstOrDefault();

                    if (nroDispo != null && nroExp != null)
                        detalleDoc = serviceEE.obtenerDetalleDocumento(this.username_servicio_EE, this.pass_servicio_EE, nroExp, nroDispo.resultado_ee);

                    this.db.SSIT_Actualizar_NroDisposicionSADE(id_solicitud, detalleDoc!= null ? detalleDoc.numeroEspecialDocumento : "");

                    id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                    SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);

                    if (sol.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad || sol.id_estado == (int)Constants.Solicitud_Estados.Caduco)
                    {
                        db.ActualizarEstadoSolicitud((int)Constants.Solicitud_Estados.Caduco, sol.id_tipotramite, sol.id_solicitud);
                        Mailer.MailMessages.SendMail_Caducidad_v2(id_solicitud);
                    }
                    else
                    {

                        var TipoResolucionTramite = Functions.GetTipoResolucionHAB(this.id_solicitud);

                        if (sol.id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.HabilitacionPrevia && TipoResolucionTramite == Constants.TipoResolucionHAB.Aprobado)
                        {
                            //Generro el documento de inicio
                            if (!sol.SSIT_DocumentosAdjuntos.Where(x => x.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.OBLEA_SOLICITUD).Any())
                                Documentos.generarDocumentoInicio(this.id_solicitud);
                        }

                        if (TipoResolucionTramite == Constants.TipoResolucionHAB.Aprobado)
                        {
                            db.ActualizarEstadoSolicitud((int)Constants.Solicitud_Estados.Aprobada, sol.id_tipotramite, sol.id_solicitud);
                            Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(id_solicitud);
                        }
                        else if (TipoResolucionTramite == Constants.TipoResolucionHAB.Rechazado)
                        {
                            db.ActualizarEstadoSolicitud((int)Constants.Solicitud_Estados.Rechazada, sol.id_tipotramite, sol.id_solicitud);
                            Mailer.MailMessages.SendMail_RechazoSolicitud_v2(id_solicitud);
                        }
                        else if (TipoResolucionTramite == Constants.TipoResolucionHAB.Observado)
                        {
                            db.ActualizarEstadoSolicitud((int)Constants.Solicitud_Estados.En_trámite, sol.id_tipotramite, sol.id_solicitud);
                            Mailer.MailMessages.SendMail_LevantamientoRechazo(id_solicitud);
                        }
                    }

                    try
                    {
                        Encuestas.enviarEncuesta(id_solicitud);
                    }
                    catch (Exception ex)
                    {
                        LogError.Write(ex, "Error en ws encuesta");
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

        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }

        private void Validar_Guardar()
        {
            // agregar aca validaciones
        }


        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {

        }

        private void Validar_Finalizar()
        {
            // agregar aca validaciones
        }

        private int id_tipotramite = 0;
        private int id_tipoexpediente = 0;
        private int id_subtipoexpediente = 0;

        private void buscar_datos_solicitud()
        {
            // buscar datos del tipo de tramite 
            var datos_sol =
                (
                    from sol in this.db.SSIT_Solicitudes
                    where sol.id_solicitud == this.id_solicitud
                    select new
                    {
                        sol.id_tipotramite,
                        sol.id_tipoexpediente,
                        sol.id_subtipoexpediente,
                        sol.id_solicitud
                    }
                ).FirstOrDefault();

            this.id_tipotramite = datos_sol.id_tipotramite;
            this.id_tipoexpediente = datos_sol.id_tipoexpediente;
            this.id_subtipoexpediente = datos_sol.id_subtipoexpediente;
            this.id_solicitud = datos_sol.id_solicitud;
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
            ucResultadoTarea.btnFinalizar_Enabled = true;
            ucListaDocumentos.LoadData(this.id_grupotramite, this.id_solicitud);
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                CargarDatosTramite(this.id_tramitetarea);
                this.EjecutarScript(updCargaInicial, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                lblErrorCargaInicial.Text = ex.Message;
                pnlErrorCargaInicial.Visible = true;
            }
        }

        protected void btnCargarProcesos_Click(object sender, EventArgs e)
        {
            try
            {

                IniciarEntity();
                Guid userid = Functions.GetUserId();
                bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);

                ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, IsEditable);

                FinalizarEntity();

            }
            catch (Exception ex)
            {

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updCargarProcesos, "showfrmError();");
            }

        }
    }

}
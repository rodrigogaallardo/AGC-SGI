using SGI.GestionTramite.Controls;
using SGI.Model;
using SGI.WebServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;

namespace SGI.GestionTramite.Tareas
{
    public partial class Revision_Firma_Disposicion : System.Web.UI.Page
    {


        #region cargar inicial

        protected async void Page_Load(object sender, EventArgs e)
        {

            IniciarEntity();

            if (!IsPostBack)
            {
                try
                {
                    int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                    if (id_tramitetarea > 0)
                        await CargarDatosTramite(id_tramitetarea);
                }
                catch (Exception ex)
                {
                    Enviar_Mensaje(ex.Message, "");
                }
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

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucListaObservacionesAnt.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucSGI_ListaDocumentoAdjuntoAnt.LoadData(id_grupotramite, this.id_solicitud, this._tramiteTarea);
            ucResultadoTarea.btnGuardar_Visible = false;
            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);
            ucProcesosExpediente.editable = IsEditable;
            ucProcesosExpediente.GenerarProcesos_Revision_Firma_Disposicion(tramite_tarea.id_tramitetarea);
            ucProcesosExpediente.LoadData(tramite_tarea.id_tramitetarea);

            ucListaRubros.Visible = true;
            ucTramitesRelacionados.Visible = true;
            SGI_Tarea_Entregar_Tramite rev = Buscar_Tarea(id_tramitetarea);
            if (rev != null)
                ucObservacionesTarea.Text = rev.Observaciones;
            else
                ucObservacionesTarea.Text = "";


            ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
            if ( IsEditable )
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !hayProcesosPendientes();
           

        }

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

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Entregar_Tramite entregar_tramite = Buscar_Tarea(id_tramite_tarea);

            int id_entregar_tramite = 0;
            if (entregar_tramite != null)
                id_entregar_tramite = entregar_tramite.id_entregar_tramite;

            // guardar en la tabla que corresponda
            db.SGI_Tarea_Entregar_Tramite_Actualizar(id_entregar_tramite, id_tramite_tarea, observacion, null, null, null, null, userId);

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
                    Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid);

                    db.SaveChanges();

                    Tran.Complete();
                    Tran.Dispose();
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

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea = this.id_tramitetarea;
                int id_tramitetarea_nuevo = 0;
                int id_resultado = (int)e.id_resultado;

                Validar_Finalizar();

                #region identificar si hay que generar tarea en paralelo a la defacult

                buscar_datos_solicitud();

                #endregion

                SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);
                //Actualizo el estado de la solicitud a En Tramitre
                bool aprobada = Functions.isAprobado(this.id_solicitud);

                //Busco el numero especial de la dispo
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                ws_ExpedienteElectronico.documentosVinculadosResponse detalleDoc = new ws_ExpedienteElectronico.documentosVinculadosResponse();
                serviceEE.Url = this.url_servicio_EE;
                string username_SADE = Functions.GetUsernameSADE(userid);
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

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text.Trim(), userid);
                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                        this.db.SSIT_Actualizar_NroDisposicionSADE(id_solicitud, detalleDoc.numeroEspecialDocumento);
                        this.db.SSIT_Solicitudes_ActualizarEstado(this.id_solicitud, (aprobada ? (int)Constants.Solicitud_Estados.Aprobada : (int)Constants.Solicitud_Estados.Rechazada), userid, string.Empty, sol.telefono);

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. revision_firma_dispo-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }

                }

                //Generro el documento de inicio
                if (sol.id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.HabilitacionPrevia && aprobada)
                    Documentos.generarDocumentoInicio(this.id_solicitud);
                try
                {
                    Encuestas.enviarEncuesta(id_solicitud);
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Error en ws encuesta");
                }
                Enviar_Mensaje("Se ha finalizado la tarea.", "");
                
                Mailer.MailMessages.SendMail_EnviarCaratula_v2(id_solicitud);

                string mensaje_envio_mail = "";
                try
                {
                    if (e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Fin_Tramite_Nuevo)
                        Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(id_solicitud);
                }
                catch (Exception ex)
                {
                    mensaje_envio_mail = ex.Message;
                }

                Redireccionar_VisorTramite();

            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
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
                titulo = System.Web.HttpUtility.HtmlEncode( this.Title);
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

        protected void ucProcesosExpediente_Proceso_Finalizado(object sender, ucProcesosExpediente.ResultadoProcesoExpediente e)
        {

            if (!hayProcesosPendientes())
            {
                ucResultadoTarea.btnFinalizar_Enabled = true;
            }
           
        }

        protected void ucProcesosExpediente_Error(object sender, ucProcesosExpediente.ErrorProcesoExpediente_EventArgs e)
        {
            Enviar_Mensaje(e.mensaje, "");
        }
        private bool hayProcesosPendientes()
        {
            DGHP_Entities db = new DGHP_Entities();
            bool procesosPendientes = (from tp in db.SGI_Tarea_Generar_Expediente_Procesos
                                       where tp.id_tramitetarea == id_tramitetarea && !tp.realizado
                                       select tp).Count() > 0;

            db.Dispose();
            return procesosPendientes;

        }
    }

}
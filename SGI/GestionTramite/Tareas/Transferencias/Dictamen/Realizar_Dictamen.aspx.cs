using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas.Transferencias.Dictamen
{
    public partial class Realizar_Dictamen : System.Web.UI.Page
    {
        #region cargar inicial
        protected async void Page_Load(object sender, EventArgs e)
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

            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;
            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = ttTR.SGI_Tramites_Tareas.id_tarea;

            SGI_Tarea_Dictamen_Realizar_Dictamen pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucSGI_ListaDocumentoAdjuntoAnt.LoadData(id_grupotramite, this.id_solicitud, this.TramiteTarea);

            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
            ucProcesosSADE.cargarDatosProcesos(id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
            if (IsEditable)
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

            if (this.id_solicitud > nroTrReferencia)
                ucListaObservacionesAnteriores.Visible = false;
            else
                ucListaObservacionesAnterioresv1.Visible = false;
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

        private SGI_Tarea_Dictamen_Realizar_Dictamen Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Dictamen_Realizar_Dictamen pvh =
                (
                    from env_phv in db.SGI_Tarea_Dictamen_Realizar_Dictamen
                    where env_phv.id_tramitetarea == id_tramitetarea
                    orderby env_phv.id_dictamen_realizar_dictamen descending
                    select env_phv
                ).FirstOrDefault();

            return pvh;
        }

        #endregion

        #region acciones

        private void Redireccionar_VisorTramite()
        {
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

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Dictamen_Realizar_Dictamen pvh = Buscar_Tarea(id_tramite_tarea);

            int id_enviar_avh = 0;
            if (pvh != null)
                id_enviar_avh = pvh.id_dictamen_realizar_dictamen;

            db.SGI_Tarea_Dictamen_Realizar_Dictamen_Actualizar(id_enviar_avh, id_tramite_tarea, observacion, userId);

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
            int cantFileCount = db.SGI_Tarea_Documentos_Adjuntos
                .Where(x => x.id_tramitetarea == this.TramiteTarea
                 && x.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Calificacion_Tecnica_y_Legal).ToList().Count();

            if (cantFileCount == 0)
                throw new Exception("Tiene que subir la Calificación Técnica y Legal.");
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                IniciarEntity();

                Validar_Finalizar();

                Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                db.SaveChanges();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                
                if (!hayProcesosGenerados)
                {
                    db.SGI_Transmisiones_GenerarProcesos_SADE_Dictamen(this.TramiteTarea, userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
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
                    }
                    Enviar_Mensaje("Se ha finalizado la tarea.", "");

                    Redireccionar_VisorTramite();
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                string message = ex.Message;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;
                Enviar_Mensaje(message, "");
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

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
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
    }
}
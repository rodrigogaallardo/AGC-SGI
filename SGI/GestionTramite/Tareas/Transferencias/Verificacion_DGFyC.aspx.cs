using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Verificacion_DGFyC : System.Web.UI.Page
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
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
            //Se debe establecer siempre el estado de controles antes del load de los controles
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;
            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttHTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = ttHTR.SGI_Tramites_Tareas.id_tarea;
            SGI_Tarea_Verificacion_DGFyC pvd = Buscar_Tarea(id_tramitetarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvd != null) ? pvd.Observaciones : "";
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
            ucProcesosSADE.id_tarea = (int)Constants.ENG_Tareas.ESP_Verificacion_AVH;
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);
            if (this.id_solicitud > nroTrReferencia)
                ucListaObservacionesAnteriores.Visible = false;
            else
                ucListaObservacionesAnterioresv1.Visible = false;
        }

        private int _tramiteTarea = 0;

        private int _id_solicitud = 0;

        public int TramiteTarea
        {
            get
            {
                if (_tramiteTarea == 0)
                    int.TryParse(hid_id_tramitetarea.Value, out _tramiteTarea);
                return _tramiteTarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _tramiteTarea = value;
            }
        }

        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
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
        private SGI_Tarea_Verificacion_DGFyC Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Verificacion_DGFyC pvd = (from env_pvd in db.SGI_Tarea_Verificacion_DGFyC
                                                where env_pvd.id_tramitetarea == id_tramitetarea
                                                orderby env_pvd.id_verificacion_DGFyC descending
                                                select env_pvd).FirstOrDefault();
            return pvd;
        }
        #endregion

        #region acciones
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

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {
            SGI_Tarea_Verificacion_DGFyC pvd = Buscar_Tarea(id_tramite_tarea);
            int id_verificacion_DGFyC = 0;
            if (pvd != null)
                id_verificacion_DGFyC = pvd.id_verificacion_DGFyC;
            db.SGI_Tarea_Verificacion_DGFyC_Actualizar(id_verificacion_DGFyC, id_tramite_tarea, observacion, userId);
        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                IniciarEntity();
                using (DbContextTransaction Tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                        db.SaveChanges();
                        Tran.Commit();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "error en transaccion. pvd-ucResultadoTarea_GuardarClick");
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
            var list_doc_adj = (from adj in db.SGI_Tarea_Documentos_Adjuntos
                                where adj.id_tramitetarea == this.TramiteTarea
                                && adj.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Informe_DGFyC
                                select new { adj });
            if (list_doc_adj.Count() == 0)
                throw new Exception("Debe subir el Informe de DGFyC.");
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;
                IniciarEntity();
                Validar_Finalizar();
                Transf_Solicitudes sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;
                if (!hayProcesosGenerados)
                {
                    this.db.SGI_Tarea_Verificacion_DGFyC_GenerarProcesos(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    if (db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0)
                    {
                        ucResultadoTarea.btnFinalizar_Enabled = false;
                        ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                    }
                    else
                    {
                        using (DbContextTransaction Tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                                db.SaveChanges();
                                id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                                Tran.Commit();
                                Tran.Dispose();
                            }
                            catch (Exception ex)
                            {
                                if (Tran != null)
                                    Tran.Dispose();
                                LogError.Write(ex, "error en transaccion. pvd-ucResultadoTarea_FinalizarTareaClick");
                                throw ex;
                            }
                            FinalizarEntity();
                            Enviar_Mensaje("Se ha finalizado la tarea.", "");
                            Redireccionar_VisorTramite();
                        }
                    }
                }
                else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    using (DbContextTransaction Tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                            db.SaveChanges();
                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                            Tran.Commit();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            if (Tran != null)
                                Tran.Dispose();
                            LogError.Write(ex, "error en transaccion. pvd-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }
                    }
                    FinalizarEntity();
                    Enviar_Mensaje("Se ha finalizado la tarea.", "");
                    Redireccionar_VisorTramite();
                }
                else
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
            }
            catch (Exception ex)
            {
                string mensaje = Functions.GetErrorMessage(ex);
                Enviar_Mensaje(mensaje, "");
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }
        #endregion

        #region entity
        private DGHP_Entities db = null;

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
        #endregion

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
        }
    }
}
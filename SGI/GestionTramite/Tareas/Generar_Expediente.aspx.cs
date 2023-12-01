using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.UI;
using SGI.GestionTramite.Controls;
using SGI.Model;
using System.Web.Security;
using System.Threading.Tasks;

namespace SGI.GestionTramite.Tareas
{
    public partial class Generar_Expediente : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Generar_Expediente;

        protected async void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea == 0)
                    id_tramitetarea = (Page.RouteData.Values["id"] != null ? Convert.ToInt32(Page.RouteData.Values["id"]) : 0);

                if (id_tramitetarea > 0)
                    await CargarDatosTramite(id_tramitetarea);

            }

        }


        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            FinalizarEntityFiles();
            base.OnUnload(e);
        }


        private async Task CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            IniciarEntity();

            SGI_Tramites_Tareas tramite_tarea = this.db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

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

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            int id_subtipoexpediente = ttHAB.SSIT_Solicitudes.id_subtipoexpediente;
            IniciarEntityFiles();


            //generar expediente y los item a procesar para expediente sade.
            int id_generar_expediente = Guardar_tarea(id_tramitetarea, "", userid);

            SGI_Tarea_Generar_Expediente expediente = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                ucListaRubros.LoadData(this.id_solicitud);
                ucListaRubros.Visible = true;
                ucTramitesRelacionados.Visible = true;

                ucProcesosExpediente.editable = IsEditable;
                try
                {
                    ucProcesosExpediente.GenerarProcesos_Expediente(tramite_tarea.id_tramitetarea);
                    ucProcesosExpediente.LoadData(tramite_tarea.id_tramitetarea);
                }
                catch (Exception ex)
                {
                    if(ex.InnerException.Message != null)
                        Enviar_Mensaje(ex.InnerException.Message, "");
                    else
                        Enviar_Mensaje(ex.Message, "");
                }
            }
            else
            {
                ucListaRubros.Visible = false;
                ucTramitesRelacionados.Visible = false;
            }

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, false);
            ucObservacionesTarea.Text = (expediente != null) ? expediente.Observaciones : "";

            ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
            if (IsEditable)
            {
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosExpediente.Existen_procesos_pendientes;
                ucResultadoTarea.ddlProximaTarea_Enabled = Functions.EsForzarTarasSade() || !ucProcesosExpediente.Existen_procesos_pendientes;
                ucResultadoTarea.ddlResultado_Enabled = Functions.EsForzarTarasSade() || !ucProcesosExpediente.Existen_procesos_pendientes;
            }
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            if (id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.ConPlanos && !ucProcesosExpediente.Existen_procesos_pendientes)
            {
                ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
                bool hayProcesosPendientes = ucProcesosSADE.hayProcesosPendientesSADE(tramite_tarea.id_tramitetarea);
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !hayProcesosGenerados || !hayProcesosPendientes;
                ucResultadoTarea.ddlProximaTarea_Enabled = Functions.EsForzarTarasSade() || !hayProcesosGenerados;
                ucResultadoTarea.ddlResultado_Enabled = Functions.EsForzarTarasSade() || !hayProcesosGenerados;
            }
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

        private SGI_Tarea_Generar_Expediente Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Generar_Expediente expediente =
                (
                    from gen_exp in db.SGI_Tarea_Generar_Expediente
                    where gen_exp.id_tramitetarea == id_tramitetarea
                    orderby gen_exp.id_generar_expediente descending
                    select gen_exp
                ).ToList().FirstOrDefault();

            return expediente;
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

        private int Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Generar_Expediente expediente = Buscar_Tarea(id_tramite_tarea);

            int id_generar_expediente = 0;
            if (expediente != null)
                id_generar_expediente = expediente.id_generar_expediente;

            ObjectResult<int?> objResult = db.SGI_Tarea_Generar_Expediente_Actualizar(id_generar_expediente, id_tramite_tarea, observacion, userId);

            foreach (int? q in objResult.ToList())
            {
                id_generar_expediente = (int)q;
            }

            return id_generar_expediente;
        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                IniciarEntity();

                Validar_Tarea();

                
                try
                {
                    Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);

                }
                catch (Exception ex)
                {
                    string mensaje = Functions.GetErrorMessage(ex);
                    LogError.Write(ex, "ucResultadoTarea_GuardarClick: " + mensaje);
                    throw ex;
                }

                FinalizarEntity();

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje(ex.Message, "");
            }

        }

       
        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                //Guardo los datos del tramite_tarea
                db.SGI_Tramites_Tareas_Actualizar(e.id_tramitetarea_actual, e.id_resultado, e.id_proxima_tarea);

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                int id_subtipoexpediente = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud).id_subtipoexpediente;

                if (!Functions.EsForzarTarasSade() && !hayProcesosGenerados && id_subtipoexpediente==(int)Constants.SubtipoDeExpediente.ConPlanos)
                {
                    int id_paquete = (from p in db.SGI_Tarea_Generar_Expediente_Procesos
                                       join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                       join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                       where tt_hab.id_solicitud == this.id_solicitud
                                       select p.id_paquete).FirstOrDefault();

                    db.SGI_Tarea_Generar_Expediente_GenerarProcesos(TramiteTarea, id_paquete, userid, ucResultadoTarea.getIdProximaTarea());
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucResultadoTarea.ddlProximaTarea_Enabled = false;
                    ucResultadoTarea.ddlResultado_Enabled = false;

                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(TramiteTarea) || id_subtipoexpediente != (int)Constants.SubtipoDeExpediente.ConPlanos)
                {

                    Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);

                    if (ActualizarNroExpediente())
                    {

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                        Enviar_Mensaje("Se ha finalizado la tarea.", "");

                        Redireccionar_VisorTramite();
                    }
                    else
                        Enviar_Mensaje("No es posible avanzar la tarea, no se puedo obtener el numero de expediente.", "");
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }
            }
            catch (Exception ex)
            {
                string mensaje = Functions.GetErrorMessage(ex);
                LogError.Write(ex, "ucResultadoTarea_FinalizarTareaClick: " + mensaje);
                Enviar_Mensaje(mensaje, "");
            }
            finally
            {
                FinalizarEntity();
            }
        }

        private bool ActualizarNroExpediente()
        {
            var sol= db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
            if (string.IsNullOrEmpty(sol.NroExpedienteSade))
            {
                int id_paquete = (from p in db.SGI_Tarea_Generar_Expediente_Procesos
                                  join tt_hab in db.SGI_Tramites_Tareas_HAB on p.id_tramitetarea equals tt_hab.id_tramitetarea
                                  where tt_hab.id_solicitud == this.id_solicitud
                                  && p.id_proceso == (int)Constants.EE_Procesos.GeneracionCaratula
                                  && p.realizado == true
                                  select p.id_paquete).FirstOrDefault();
                EE_Entities dbEE = new EE_Entities();
                var car = dbEE.wsEE_Caratulas.FirstOrDefault(x => x.id_paquete == id_paquete);
                dbEE.Dispose();

                if (car==null || string.IsNullOrEmpty(car.resultado_SADE))
                    return false;

                this.db.SGI_Nro_Expediente_Sade_Actualizar(id_solicitud, car.resultado_SADE);
            }
            return true;
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Generar Expediente");

            //updPnlGrillaProcesos
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }


        #endregion

        protected void ucProcesosExpediente_Proceso_Finalizado(object sender, ucProcesosExpediente.ResultadoProcesoExpediente e)
        {
            if (!e.hay_procesos_pendientes)
            {
                ucResultadoTarea.btnFinalizar_Enabled = true;
                ucResultadoTarea.ddlProximaTarea_Enabled = true;
                ucResultadoTarea.ddlResultado_Enabled = true;
            }
        }

        protected async Task ucProcesosExpediente_ProcesoItem_Finalizado(object sender, ucProcesosExpediente.ResultadoProcesoExpediente e)
        {

            if (e.id_proceso_ejecutado == (int)Constants.EE_Procesos.FirmarDocumento ||
                e.id_proceso_ejecutado == (int)Constants.EE_Procesos.SubirDocumento)
            {
                // cuando termina procesar la firma del documento se vuelven a cargar
                // los documentos adjuntos para que muestre el pdf de caratula
                await ucListaDocumentos.LoadData(this.id_solicitud);
            }

        }

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }

    }
}
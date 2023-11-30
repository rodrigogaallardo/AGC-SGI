using SGI.GestionTramite.Controls;
using SGI.Model;
using SGI.WebServices.LIZA;
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
    public partial class Obtener_Ticket_Liza : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Enviar_PVH;

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

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Obtener_Ticket_Liza pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";

            bool hayProcesosGenerados = db.SGI_LIZA_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            if (!hayProcesosGenerados)
                generarProcesos();
            ucProcesosLIZA.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            bool hayProcesosPendientes = ucProcesosLIZA.hayProcesosPendientes(tramite_tarea.id_tramitetarea);
            ucResultadoTarea.btnFinalizar_Enabled = !hayProcesosGenerados || !hayProcesosPendientes;
            ucResultadoTarea.ddlProximaTarea_Enabled = !hayProcesosGenerados;
            ucResultadoTarea.ddlResultado_Enabled = !hayProcesosGenerados;

        }

        private void generarProcesos()
        {
            SGI_LIZA_Ticket ticket =
                (
                    from ti in db.SGI_LIZA_Ticket
                    join tl in db.SGI_Tarea_Generar_Ticket_Liza on ti.id_tramitetarea equals tl.id_tramitetarea
                    join tt in db.SGI_Tramites_Tareas_HAB on tl.id_tramitetarea equals tt.id_tramitetarea
                    where tl.id_tramitetarea < TramiteTarea && tt.id_solicitud == id_solicitud
                    orderby tl.id_generar_ticket_liza descending
                    select ti
                ).FirstOrDefault();
            WS_Response_Ticket respTick =WS_LizaRest.obtencionTicket(ticket.Alias);

            //Chequear estado
            if (respTick.Closed)
            {
                Guid userid = Functions.GetUserId();
                ObjectParameter id_liza_proceso;
                foreach (WS_Archivo ar in respTick.Archivos)
                {
                    id_liza_proceso = new ObjectParameter("id_liza_proceso", typeof(int));
                    db.SGI_LIZA_Procesos_Agregar(ticket.id_ticket, TramiteTarea, ar.Nombre, ar.UrlAdjunto, userid, id_liza_proceso);
                }
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

        private SGI_Tarea_Obtener_Ticket_Liza Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Obtener_Ticket_Liza pvh =
                (
                    from env_phv in db.SGI_Tarea_Obtener_Ticket_Liza
                    where env_phv.id_tramitetarea == id_tramitetarea
                    orderby env_phv.id_obtener_ticket_liza descending
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

            SGI_Tarea_Obtener_Ticket_Liza pvh = Buscar_Tarea(id_tramite_tarea);

            int id_enviar_avh = 0;
            if (pvh != null)
                id_enviar_avh = pvh.id_obtener_ticket_liza;

            db.SGI_Tarea_Obtener_Ticket_Liza_Actualizar(id_enviar_avh, id_tramite_tarea, observacion, userId);

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
        }


        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                bool hayProcesosGenerados = db.SGI_LIZA_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;
                bool hayProcesosPendientes = ucProcesosLIZA.hayProcesosPendientes(TramiteTarea);
                if (hayProcesosGenerados && !hayProcesosPendientes)
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

                    Enviar_Mensaje("Se ha finalizado la tarea.", "");

                    Redireccionar_VisorTramite();
                }
                else
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en LIZA.", "");

            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
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

        protected void ucProcesosLIZA_FinalizadoEnLIZA(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }

    }
}

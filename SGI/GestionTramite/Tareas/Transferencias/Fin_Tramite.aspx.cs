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

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Fin_Tramite : System.Web.UI.Page
    {
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

        private async Task CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            this.db = new DGHP_Entities();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }


            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Fin_Tramite director = Buscar_Tarea(id_tramitetarea);

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, false);
            ucListaResultadoTareasAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);

            ucObservacionesTarea.Text = (director != null) ? director.Observaciones : "";

            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            
            ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

            this.db.Dispose();

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

        private SGI_Tarea_Fin_Tramite Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Fin_Tramite director =
                (
                    from dir in db.SGI_Tarea_Fin_Tramite
                    where dir.id_tramitetarea == id_tramitetarea
                    orderby dir.id_Fin_Tramite descending
                    select dir
                ).ToList().FirstOrDefault();

            return director;
        }

        #endregion


        #region acciones

        private DGHP_Entities db = null;

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

            SGI_Tarea_Fin_Tramite director = Buscar_Tarea(id_tramite_tarea);

            int id_Fin_Tramite = 0;
            if (director != null)
                id_Fin_Tramite = director.id_Fin_Tramite;

            db.SGI_Tarea_Fin_Tramite_Actualizar(id_Fin_Tramite, id_tramite_tarea, observacion, userId);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

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
                        LogError.Write(ex, "Error en transaccion. fin_tramite-ucResultadoTarea_GuardarClick");
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
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                this.db = new DGHP_Entities();
                Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);

                Validar_Finalizar();
                if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {
                            db.SaveChanges();

                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();
                            Enviar_Mensaje("Se ha finalizado la tarea.", "");

                            Redireccionar_VisorTramite();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. fin_tramiteucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }
                    }
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Fin Tramite");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());

        }
    }
}
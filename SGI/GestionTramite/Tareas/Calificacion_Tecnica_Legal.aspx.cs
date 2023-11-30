using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.UI;
using SGI.GestionTramite.Controls;
using SGI.Model;

namespace SGI.GestionTramite.Tareas
{
    public partial class Calificacion_Tecnica_Legal : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal;

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
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Calificacion_Tecnica_Legal tecnica_legal = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucResultadoTarea.LoadData(id_tramitetarea, false);
            ucObservacionesTarea.Text = (tecnica_legal != null) ? tecnica_legal.Observaciones : "";

            db.Dispose();

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

        private SGI_Tarea_Calificacion_Tecnica_Legal Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Calificacion_Tecnica_Legal tecnica_legal =
                (
                    from calif in db.SGI_Tarea_Calificacion_Tecnica_Legal
                    where calif.id_tramitetarea == id_tramitetarea
                    orderby calif.id_cal_tec_leg descending
                    select calif
                ).ToList().FirstOrDefault();

            return tecnica_legal;
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

            SGI_Tarea_Calificacion_Tecnica_Legal tecnica_legal = Buscar_Tarea(id_tramite_tarea);

            int id_cal_tec_leg = 0;
            if (tecnica_legal != null)
                id_cal_tec_leg = tecnica_legal.id_cal_tec_leg;

            db.SGI_Tarea_Calificacion_Tecnica_Actualizar(id_cal_tec_leg, id_tramite_tarea, observacion, userId);

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
                        LogError.Write(ex, "Error en transaccion. calificacion_tecnica_legal-ucResultadoTarea_GuardarClick");
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

                int id_resultado = (int)e.id_resultado;

                this.db = new DGHP_Entities();

                Validar_Finalizar();

                using (TransactionScope Tran = new TransactionScope())
                {

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
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. calificacion_tecnica_legal-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }
	

                }
                db.Dispose();

                Enviar_Mensaje("Se ha finalizado la tarea.", "");

                Redireccionar_VisorTramite();

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
                titulo = System.Web.HttpUtility.HtmlEncode("Calificación Técnica y Legal");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion

    }
}


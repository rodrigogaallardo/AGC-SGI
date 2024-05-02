using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Calificar : BasePage
    {

        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Calificar;

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

            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);

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
            ucObservacionContribuyente.Enabled = IsEditable;
            ucObservacionesInternas.Enabled = IsEditable;
            ucDocumentoAdjunto.Enabled = IsEditable;
            ucObservaciones.Enabled = IsEditable;
            chbGuardarProvidenciaHTML.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Calificar calificar = Buscar_Tarea(id_tramitetarea);

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucTitulares.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, this.TramiteTarea);
            ucDocumentoAdjunto.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucObservaciones.Visible = false;
            ucPreviewDocumentos.LoadData(this.id_solicitud, (int)Constants.TipoDeTramite.Transferencia);
            if (this.id_solicitud > nroTrReferencia)
            {
                ucObservaciones.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea, false);
                ucObservaciones.Visible = true;
                ucObservacionContribuyente.Visible = false;
                ucListaObservacionesAnteriores.Visible = false;
            }
            else
            {
                ucListaObservacionesAnterioresv1.Visible = false;
                ucPreviewDocumentos.Visible = true;                
            }
            

            if (calificar != null)
            {
                ucObservacionesTarea.Text = calificar.Observaciones.Trim();
                ucObservacionContribuyente.Text = calificar.Observaciones_contribuyente.Trim();
                ucObservacionesInternas.Text = ucObservacionesTarea.LoadObservacionesInternasCalificar(id_tramitetarea, id_solicitud);
                ucObservacionProvidencia.Text = calificar.Observaciones_Providencia;
            }
            else
            {
                ucObservacionesTarea.Text = ObservacionAnteriores.Buscar_ObservacionPlancheta((int)Constants.GruposDeTramite.TR, id_solicitud, id_tramitetarea);
                ucObservacionContribuyente.Text = "";
                ucObservacionesInternas.Text = ucObservacionesTarea.LoadObservacionesInternasCalificar(id_tramitetarea, id_solicitud); ;
                if (this.id_solicitud <= nroTrReferencia)
                    ucObservacionProvidencia.Text = "SUBGERENTE OPERATIVO TRANSFERENCIA\n\n\n" +
                                                    "Atento lo actuado y teniendo en cuenta que se ha dado cumplimiento con todos los requisitos normativos " +
                                                    "y reunido todos los elementos necesarios para la aprobación del presente trámite, se eleva a los efectos " +
                                                    "de su consideración.";
                else
                    ucObservacionProvidencia.Text = "";
            }

            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                ucObservacionesTarea.Text = Parametros.GetParam_ValorChar("SGI.Notas.Adicionales.Dispo") + "\n\r" + ucObservacionesTarea.Text;
                ucObservacionesTarea.Update();
            }

            FinalizarEntity();


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

        private SGI_Tarea_Calificar Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Calificar calificar =
                (
                    from calif in db.SGI_Tarea_Calificar
                    where calif.id_tramitetarea == id_tramitetarea
                    orderby calif.id_calificar descending
                    select calif
                ).ToList().FirstOrDefault();

            return calificar;
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

        private void Guardar_tarea(bool finalizar, int id_solicitud, int id_tramite_tarea, string observacion, string observContribuyente,
            string observInternas, string observProvidencia, Guid userId)
        {

            SGI_Tarea_Calificar calificar = Buscar_Tarea(id_tramite_tarea);

            int id_calificar = 0;
            if (calificar != null)
                id_calificar = calificar.id_calificar;

            int id = db.SGI_Tarea_Calificar_Actualizar(id_calificar, id_tramite_tarea, observacion, observContribuyente, observInternas, observProvidencia, null, false, userId);

            if (finalizar && !string.IsNullOrEmpty(observContribuyente))
                db.Transf_Solicitudes_AgregarObservaciones(id_solicitud, observContribuyente, userId);
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

                        Guardar_tarea(false, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text, ucObservacionContribuyente.Text,
                            ucObservacionesInternas.Text, ucObservacionProvidencia.Text, userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. calificar-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

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



        private void Validar_Finalizar()
        {

            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);

            if (this.id_solicitud > nroTrReferencia)
            {
                if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion
                        && ucObservaciones.countObservaciones == 0)
                    throw new Exception("Debe especificar la Documentacion a Presentar.");
                else if (ucResultadoTarea.getIdResultadoTarea() != (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion
                    && ucObservaciones.countObservaciones != 0)
                    throw new Exception("No se debe especificar la Documentacion a Presentar.");
            }
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(true, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text, ucObservacionContribuyente.Text,
                            ucObservacionesInternas.Text, ucObservacionProvidencia.Text, userid);
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                        
                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. calificar-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }

                }

                if (chbGuardarProvidenciaHTML.Checked)
                    Functions.GuardarFileHTMLProvidencia(this.TramiteTarea, File.ReadAllText(Server.MapPath(@"~\Reportes\Providencia.html")), ucObservacionProvidencia.Text, id_tramitetarea_nuevo);

                FinalizarEntity();

                string mensaje_envio_mail = "";
                try
                {
                    if (e.id_proxima_tarea == (int)Constants.ENG_Tareas.TRM_Correccion_Solicitud)
                        Mailer.MailMessages.SendMail_CorreccionSolicitud_v2(id_solicitud);
                }
                catch (Exception ex)
                {
                    mensaje_envio_mail = ex.Message;
                }

                Enviar_Mensaje("Se ha finalizado la tarea.", "");

                Redireccionar_VisorTramite();

            }
            catch (Exception ex)
            {
                FinalizarEntity();
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

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                ucObservacionesTarea.Text = Parametros.GetParam_ValorChar("SGI.Notas.Adicionales.Dispo") + "\n\r" + ucObservacionesTarea.Text;
                ucObservacionesTarea.Update();
            }
            else
            {
                ucObservacionesTarea.Text = ucObservacionesTarea.Text.Replace(Parametros.GetParam_ValorChar("SGI.Notas.Adicionales.Dispo"), "");
                ucObservacionesTarea.Update();
            }
            FinalizarEntity();
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
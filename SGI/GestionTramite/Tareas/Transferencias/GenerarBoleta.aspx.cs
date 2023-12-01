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
    public partial class GenerarBoleta : System.Web.UI.Page
    {
        #region cargar inicial

        protected async void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script_inicial", "inicializar_controles();", true);

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
                FinalizarEntity();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;

            WebUtil.EstadoControles(pnl_area_tarea.Controls, IsEditable);

            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);

            ucResultadoTarea.btnGuardar_Visible = false;
            ucMediosDePago.LoadData(id_tramitetarea, id_grupotramite, true);
            bool existen_pagos_pendientes = ucMediosDePago.ExistenPagosPendientes;

            if (ucResultadoTarea.btnFinalizar_Enabled)
                ucResultadoTarea.btnFinalizar_Enabled = existen_pagos_pendientes;

            ucMediosDePago.GenerarBoleta_Visible = IsEditable && !existen_pagos_pendientes;


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
            // db.SGI_Tarea_Entregar_Tramite_Actualizar(id_entregar_tramite, id_tramite_tarea, observacion, userId);

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

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, "", userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. generar_boleta-ucResultadoTarea_GuardarClick");
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
            // agregar aca validaciones
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
                        Guardar_tarea(this.TramiteTarea, "", userid);
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. generar_boleta-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }

                }
                db.Dispose();

                string mensaje_envio_mail = "";
                try
                {
                    Mailer.MailMessages.SendMail_BoletaGenerada_v2(id_solicitud);
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
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Generar Boleta");
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
                this.dbFiles.Dispose();
        }

        #endregion


        #region generacion de pagos

        private bool HabilitarFinalizar()
        {
            int id_tramite_tarea = int.Parse(hid_id_tramitetarea.Value);

            IniciarEntity();

            List<SGI_Solicitudes_Pagos> lstPagos =
                            (
                    from p in db.SGI_Solicitudes_Pagos
                    where p.id_tramitetarea == id_tramite_tarea
                    select p
                ).ToList();

            FinalizarEntity();

            if (lstPagos.Count > 0)
                return true;
            else
                return false;
        }

        protected void GenerarBoletaUnica_Click(object sender, ucMediosPagos_EventArgs e)
        {
            try
            {
                ucMediosDePago.GenerarBoletaUnica(e.id_tramite_tarea, (int)Constants.GruposDeTramite.TR);

                // cuando se genera la boleta única se debe cambiar el estado de la solicitud a pendiente de pago.
                Guid userid = Functions.GetUserId();

                /*IniciarEntity();

                this.db.Transf_Solicitudes_ActualizarEstado(this.id_solicitud, (int)Constants.Solicitud_Estados.Pendiente_de_pago, userid);

                FinalizarEntity();*/

                ucCabecera.LoadData(this.id_solicitud, (int)Constants.GruposDeTramite.TR);
                ucResultadoTarea.btnFinalizar_Enabled = HabilitarFinalizar();
                updResultadoTarea.Update();


            }
            catch (Exception ex)
            {
                LogError.Write(ex, ex.StackTrace);
                Enviar_Mensaje(ex.Message, "");

            }

        }

        #endregion
    }
}
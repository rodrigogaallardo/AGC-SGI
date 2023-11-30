using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.GestionTramite.Controls;
using SGI.Model;

namespace SGI.GestionTramite.Tareas
{
    public partial class Revision_Pagos : System.Web.UI.Page
    {

        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Revision_Pagos;

        protected async Task Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                {
                    IniciarEntity();
                    await CargarDatosTramite(id_tramitetarea);
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
            ucResultadoTarea.btnFinalizar_Enabled = false;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Revision_Pagos pagos = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pagos != null) ? pagos.Observaciones : "";
            cargarBoletas(id_solicitud);


        }

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

        private SGI_Tarea_Revision_Pagos Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Revision_Pagos pagos =
                (
                    from pag in db.SGI_Tarea_Revision_Pagos
                    where pag.id_tramitetarea == id_tramitetarea
                    orderby pag.id_revision_pagos descending
                    select pag
                ).ToList().FirstOrDefault();

            return pagos;
        }

        public void cargarBoletas(int id_solicitud)
        {


            DGHP_Entities db = new DGHP_Entities();
            var lstPagos = (
                                from p in db.SGI_Solicitudes_Pagos
                                join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                where tt_hab.id_solicitud == id_solicitud
                                select new
                                {
                                    p.id_pago,
                                    p.monto_pago,
                                    p.CreateDate,
                                }
                            ).ToList();

            var lstPagosSSIT = (
                                from p in db.SSIT_Solicitudes_Pagos
                                where p.id_solicitud == id_solicitud
                                select new
                                {
                                    p.id_pago,
                                    p.monto_pago,
                                    p.CreateDate
                                }
                            ).ToList();

            lstPagos.AddRange(lstPagosSSIT);
            db.Dispose();

            grdPagosGeneradosBU.DataSource = lstPagos;
            grdPagosGeneradosBU.DataBind();


        }

        public string ConsultarEstadoPago(int id_pago)
        {
            string strEstadoPago = "";

            if (id_pago <= 0)
                return strEstadoPago;

            string WSPagos_url = Parametros.GetParam_ValorChar("Pagos.Url");
            string WSPagos_Usuario = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string WSPagos_Password = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            SGI.Webservices.Pagos.ws_pagos servicePagos = new SGI.Webservices.Pagos.ws_pagos();
            servicePagos.Url = WSPagos_url;

            SGI.Webservices.Pagos.wsResultado wsPagos_resultado = new SGI.Webservices.Pagos.wsResultado();


            try
            {
                strEstadoPago = servicePagos.GetEstadoPago(WSPagos_Usuario, WSPagos_Password, id_pago, ref wsPagos_resultado);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error en ws GetEstadoPago de ws_pagos");
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }

            if (wsPagos_resultado.ErrorCode != 0)
            {
                LogError.Write(new Exception("Error en ws GetEstadoPago de ws_pagos"), wsPagos_resultado.ErrorDescription + " - Código Error: " + wsPagos_resultado.ErrorCode);
                throw new Exception("El servicio de generación de boletas no esta disponible. Intente en otro momento.");
            }

            return strEstadoPago;

        }

        protected void grdPagosGeneradosBU_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id_pago = int.Parse(grdPagosGeneradosBU.DataKeys[e.Row.RowIndex].Values["id_pago"].ToString());

                Label lblDescripicionEstadoPago = (Label)e.Row.FindControl("lblDescripicionEstadoPago");
                string estado = "";
                try
                {
                    estado = ConsultarEstadoPago(id_pago);
                }
                catch (Exception ex)
                {
                    Enviar_Mensaje(ex.Message, "");
                }

                // Si alguna de las boletas está paga se habilita la finalización de la tarea.
                if (estado == Constants.Pago_EstadoPago.Pagado.ToString())
                    ucResultadoTarea.btnFinalizar_Enabled = true;

                lblDescripicionEstadoPago.Text = estado;
            }
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

            SGI_Tarea_Revision_Pagos pagos = Buscar_Tarea(id_tramite_tarea);

            int id_revision_pagos = 0;
            if (pagos != null)
                id_revision_pagos = pagos.id_revision_pagos;

            db.SGI_Tarea_Revision_Pagos_Actualizar(id_revision_pagos, id_tramite_tarea, observacion, userId);

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
                        LogError.Write(ex, "Error en transaccion. revision_pago-ucResultadoTarea_GuardarClick");
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

                Validar_Finalizar();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                        SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);

                        //Actualizo el estado de la solicitud a En Tramitre


                        this.db.SSIT_Solicitudes_ActualizarEstado(this.id_solicitud, (int)Constants.Solicitud_Estados.En_trámite, userid, string.Empty, sol.telefono);

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. revision_pagos-ucResultadoTarea_FinalizarTareaClick");
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
                titulo = System.Web.HttpUtility.HtmlEncode("Revisión Pagos");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.BusinessLogicLayer;
using System.Drawing;
using SGI.Model;
using SGI.GestionTramite.Controls;


namespace SGI
{

    public partial class NotificacionGenerica : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            #endregion

            if (!IsPostBack)
            {
                //Campos Hidden
                DGHP_Entities entities = new DGHP_Entities();
                string idNotificacionMotivoStr = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();
                if (String.IsNullOrEmpty(idNotificacionMotivoStr))
                {
                    Response.Redirect("NotificacionesCaducidad.aspx");
                }

                int idNotificacionMotivo = int.Parse(idNotificacionMotivoStr);
                hdIdNotificacionMotivo.Value = idNotificacionMotivoStr;

                string nroSolicitudStr = (Request.QueryString["nroSolicitud"] == null) ? "" : Request.QueryString["nroSolicitud"].ToString();
                if (String.IsNullOrEmpty(nroSolicitudStr))
                {
                    nroSolicitudStr = "0";
                }

                int nroSolicitud = int.Parse(nroSolicitudStr);
                hdNroSolicitud.Value = nroSolicitudStr;

                string fechaNotificacionStr = (Request.QueryString["fechaNotificacion"] == null) ? "" : Request.QueryString["fechaNotificacion"].ToString();

                DateTime fechaNotificacion = DateTime.Parse(fechaNotificacionStr);
                hdFechaNotificacion.Value = fechaNotificacionStr;



                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtAsunto.Text = string.Empty;
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        protected void btnNotificar_OnClick(Object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtAsunto.Text) && string.IsNullOrEmpty(txtMensaje.Value))
            {
                lblError.Text = "No se ha ingresado 'Asunto' y/o 'Mensaje'";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError()");
                return;
            }
            else
            {
                Notificar(int.Parse(hdNroSolicitud.Value), int.Parse(hdIdNotificacionMotivo.Value), DateTime.Parse(hdFechaNotificacion.Value), txtAsunto.Text, txtMensaje.Value);
            }

        }

        public void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("NotificacionesCaducidad.aspx?idSolicitud=" + hdNroSolicitud.Value);
        }

        private void Notificar(int nroSolicitud, int idNotificacionMotivo, DateTime fechaNotificacion, string asunto, string mensaje)
        { 
            try
            {
                bool pudo = TramitesBLL.NotificarTramite(nroSolicitud, idNotificacionMotivo, fechaNotificacion, out string errorMessage, asunto, mensaje);

                if (!pudo)
                {
                    lblError.Text = errorMessage;
                    lblError.ForeColor = Color.Red;
                    this.EjecutarScript(updResultados, "showfrmError();");
                }
                else
                {
                    lblSuccess.Text = errorMessage;
                    lblSuccess.ForeColor = Color.Black;
                    this.EjecutarScript(updResultados, "showfrmSuccess();");
                    Response.Redirect("NotificacionesCaducidad.aspx?idSolicitud=" + hdNroSolicitud.Value);
                }
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }
            //Response.Redirect("~/Operaciones/ViewLayer/NotificacionesCaducidad.aspx");
        }
    }























}
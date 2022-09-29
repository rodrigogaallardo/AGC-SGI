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

namespace SGI
{
    public partial class NotificacionesCaducidad : BasePage
    {
        #region CargaInicial
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager sm = ScriptManager.GetCurrent(this);

            //if ( sm.IsInAsyncPostBack )
            //{
            //    ScriptManager.RegisterStartupScript(updPnlFiltroCaducar, updPnlFiltroCaducar.GetType(),"inicializar_controles", "inicializar_controles();", true);
            //}

            if ( ! IsPostBack )
            {
                LimpiarCampos();
            }
        }
        #endregion

        #region CaducarTramite

        private void LimpiarCampos()
        {
            txtNroSolicitud.Text = string.Empty;
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        protected void btnNotificarCaducidad_OnClick(object sender, EventArgs e)
        {
            bool pudo = int.TryParse(txtNroSolicitud.Text, out int id_solicitud);

            if (string.IsNullOrEmpty(txtFechaNotificacion.Text.Trim()))
                throw new Exception("No se ha ingresado 'Fecha Notificación'");

            if (pudo)
            { 
                CaducarTramite(id_solicitud, Convert.ToDateTime(txtFechaNotificacion.Text.Trim()));
            }
            else
            {
                lblError.Text = "El campo solo acepta valores numéricos.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }

        private void CaducarTramite(int nroSolicitud,DateTime fechaNotificacion)
        {
            try
            {
                bool pudo = TramitesBLL.NotificarCaducaTramite(nroSolicitud,fechaNotificacion,out string errorMessage);
                
                if( ! pudo)
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
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                LogError.Write(ex, "Error al intentar caducar trámite-btnCaducarNotificaciones_OnClick");
            }
        }
        #endregion
    }
}
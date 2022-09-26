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

                if (pudo)
                { 
                    CaducarTramite(id_solicitud);
                }
                else
                {
                    lblError.Text = "El campo solo acepta valores numéricos.";
                    lblError.ForeColor = Color.Red;
                    this.EjecutarScript(updResultados, "showfrmError();");
                }
        }

        private void CaducarTramite(int nroSolicitud)
        {
            try
            {
                TramitesBLL.NotificarCaducaTramite(nroSolicitud,out string errorMessage);

                if(errorMessage != string.Empty)
                {
                    lblError.Text = errorMessage;
                    lblError.ForeColor = Color.Red;
                    this.EjecutarScript(updResultados, "showfrmError();");
                }
                else
                {
                    lblError.Text = errorMessage;
                    lblError.ForeColor = Color.Green;
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
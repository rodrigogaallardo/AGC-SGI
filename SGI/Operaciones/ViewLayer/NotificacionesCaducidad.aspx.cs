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
using SGI.DataLayer.Models;
using SGI.BusinessLogicLayer.Constants;

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
                List<SSIT_Solicitudes_Notificaciones_motivos> Notificaciones_motivosList = TramitesBLL.TraerNotificaciones_motivos( out string errorMessage);
                ddlNotificaciones_motivos.DataSource = Notificaciones_motivosList;
                ddlNotificaciones_motivos.DataTextField = "NotificacionMotivo";
                ddlNotificaciones_motivos.DataValueField = "IdNotificacionMotivo";
                ddlNotificaciones_motivos.DataBind();
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

        protected void btnNotificar_OnClick(object sender, EventArgs e)
        {
            bool pudo = int.TryParse(txtNroSolicitud.Text, out int id_solicitud);

            if (string.IsNullOrEmpty(txtFechaNotificacion.Text.Trim()))
            {
                lblError.Text = "No se ha ingresado 'Fecha Notificación.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }
               

            if (ddlNotificaciones_motivos.SelectedIndex < 0)
            {
                lblError.Text = "No se ha ingresado 'Motivo de Notificación.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (pudo)
            { 
                Notificar(id_solicitud, int.Parse(ddlNotificaciones_motivos.SelectedValue), Convert.ToDateTime(txtFechaNotificacion.Text.Trim()));
            }
            else
            {
                lblError.Text = "El campo solo acepta valores numéricos.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }

        private void Notificar(int nroSolicitud,int IdNotificacionMotivo,DateTime fechaNotificacion)
        {

            try
            {

                if (IdNotificacionMotivo == 13)
                {
                    using (DGHP_Entities db = new DGHP_Entities())
                    { 
                        var solicitudesNotificadas = (from st in db.SSIT_Solicitudes
                                                  join SSN in db.SSIT_Solicitudes_Notificaciones on st.id_solicitud equals SSN.id_solicitud
                                                  where st.id_solicitud == nroSolicitud
                                                  && SSN.Id_NotificacionMotivo == IdNotificacionMotivo
                                                  && SSN.createDate.Year == DateTime.Now.Year
                                                  && SSN.createDate.Month == DateTime.Now.Month
                                                  && SSN.createDate.Day == DateTime.Now.Day
                                                  select new SSIT_Solicitudes_Model
                                                  {
                                                      id_solicitud = nroSolicitud,
                                                      id_estado = st.id_estado
                                                  }).Union(from tr in db.Transf_Solicitudes
                                                           join trn in db.Transf_Solicitudes_Notificaciones on tr.id_solicitud equals trn.id_solicitud
                                                           where tr.id_solicitud == nroSolicitud
                                                           && trn.Id_NotificacionMotivo == IdNotificacionMotivo
                                                           && trn.createDate.Year == DateTime.Now.Year
                                                           && trn.createDate.Month == DateTime.Now.Month
                                                           && trn.createDate.Day == DateTime.Now.Day
                                                           select new SSIT_Solicitudes_Model
                                                           {
                                                               id_solicitud = nroSolicitud,
                                                               id_estado = tr.id_estado
                                                           }).ToList<SSIT_Solicitudes_Model>();

                        if (solicitudesNotificadas.Count != 0)
                        {
                            throw new Exception(ErrorConstants.ERROR_SOLICITUD_NOTIFICADA);
                        }
                    }
  
                    Response.Redirect("~/Operaciones/ViewLayer/NotificacionGenerica.aspx?id=" + IdNotificacionMotivo + "&nroSolicitud=" + nroSolicitud + "&fechaNotificacion=" + fechaNotificacion.ToShortDateString());
                }
                else
                {
                    bool pudo = TramitesBLL.NotificarTramite(nroSolicitud, IdNotificacionMotivo, fechaNotificacion, out string errorMessage, null, null);

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
                    }
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
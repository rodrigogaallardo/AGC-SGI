using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class VisorTramite_TR : BasePage
    {
        private int id_solicitud
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_solicitud.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ComprobarSolicitud();
                CargarDatosTramite(id_solicitud);
            }
        }

        private void ComprobarSolicitud()
        {
            if (Page.RouteData.Values["id"] != null)
            {
                this.id_solicitud = Convert.ToInt32(Page.RouteData.Values["id"].ToString());

                DGHP_Entities db = new DGHP_Entities();
                db.Database.CommandTimeout = 120;
                var transf = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);
                if(transf == null)
                    Server.Transfer("~/Errores/Error3051.aspx");

                db.Dispose();
            }
            else
            {
                Server.Transfer("~/Errores/Error3040.aspx");
            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {               
                this.EjecutarScript(updCargaTramite, "finalizarCarga();");
            }
            catch (Exception ex)
            {

                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updCargaTramite, "finalizarCarga();showfrmError();");
            }
        }

        private void CargarDatosTramite(int id_solicitud)
        {
            try
            {
                ucCabecera.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);
                ucListaDocumentos.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);               
                ucListaTareas.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);
                ucListaRubros.LoadData(id_solicitud);
                ucTramitesRelacionados.LoadData(id_solicitud);
                ucNotificaciones.LoadData(id_solicitud);
                ucPagos.LoadData(id_solicitud);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatosTramite en VisorTramite_TR.aspx");
                Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.TargetSite +
                    Environment.NewLine + Environment.NewLine +
                    ex.InnerException.Message + Environment.NewLine + ex.InnerException.Source + Environment.NewLine + ex.InnerException.TargetSite));
            }
        }

    }
}
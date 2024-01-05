using SGI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SGI.Constants;

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

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ComprobarSolicitud();
                await CargarDatosTramite(id_solicitud);
                #region ASOSA BOLETA 0
                using (var db = new DGHP_Entities())
                {
                    var sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    int id_tipotramite = sol.id_tipotramite;
                    DateTime BOLETA_0_FECHADESDE = DateTime.Parse(ConfigurationManager.AppSettings["BOLETA_0_FECHADESDE"]);

                  
                    bool flagAGC = true;
                    bool flagAPRA = true;

                    if (DateTime.Now >= BOLETA_0_FECHADESDE)
                    {
                            #region AGC
                            List<SGI.GestionTramite.Controls.ucPagos.clsItemGrillaPagos> lstPagosAGC = ucPagos.PagosAGCList(id_solicitud);
                            if (lstPagosAGC.Count > 0)
                                flagAGC = true;
                            else
                                flagAGC = false;
                            #endregion

                            #region APRA
                            List<SGI.GestionTramite.Controls.ucPagos.clsItemGrillaPagos> lstPagosAPRA = await ucPagos.PagosAPRAList(id_solicitud);
                            if (lstPagosAPRA != null && lstPagosAPRA.Count > 0)
                                flagAPRA = true;
                            else
                                flagAPRA = false;
                            #endregion


                            if (!flagAGC & !flagAPRA)
                            {
                                ucPagos.Visible = false;
                            }
                            else
                            {
                                if (!flagAGC)
                                {
                                    ucPagos.CargarPagosAGCVisibility(false);//ESCONDO AGC
                                }
                                if (!flagAPRA)
                                {
                                    ucPagos.CargarPagosAPRAVisibility(false);//ESCONDO APRA
                                }
                            }
                    }
                }
                #endregion
            }
        }

        private void ComprobarSolicitud()
        {
            if (Page.RouteData.Values["id"] != null)
            {
                this.id_solicitud = Convert.ToInt32(Page.RouteData.Values["id"].ToString());

                DGHP_Entities db = new DGHP_Entities();
                db.Database.CommandTimeout = 300;
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

        private async Task CargarDatosTramite(int id_solicitud)
        {
            try
            {
                ucCabecera.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);
                await ucListaDocumentos.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);          
                ucListaTareas.LoadData((int)Constants.GruposDeTramite.TR, id_solicitud);
                ucListaRubros.LoadData(id_solicitud);
                ucTramitesRelacionados.LoadData(id_solicitud);
                ucNotificaciones.LoadData(id_solicitud);
                await ucPagos.LoadData(id_solicitud);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatosTramite en VisorTramite_TR.aspx");
                if (ex.InnerException != null)
                    Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.InnerException.Message + Environment.NewLine + ex.InnerException.Source + Environment.NewLine + ex.InnerException.TargetSite));
                else
                    Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.TargetSite));
            }
        }

    }
}
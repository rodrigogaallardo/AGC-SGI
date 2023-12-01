using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class VisorTramite_CP : BasePage
    {
        public class itemCertificado
        {
            public int TipoTramite { get; set; }
            public int NroTramite { get; set; }
            public string usuario { get; set; }
            public string nom_docadjunto { get; set; }
            public string url { get; set; }
            public int id { get; set; }

            public Guid UserId
            {
                set
                {
                    if (string.IsNullOrEmpty(this.usuario))
                        this.usuario = value.ToString();
                }
            }

            public itemCertificado Clone()
            {
                itemCertificado cert = new itemCertificado();

                cert.url = this.url;
                cert.usuario = this.usuario;
                cert.nom_docadjunto = this.nom_docadjunto;
                cert.TipoTramite = this.TipoTramite;
                cert.NroTramite = this.NroTramite;
                cert.id = this.id;

                return cert;
            }

        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

                if (string.IsNullOrEmpty(strID))
                {
                    strID = Page.RouteData.Values["id"].ToString(); // ejemplo route
                }

                if (!string.IsNullOrEmpty(strID))
                {
                    int id_cpadron = Convert.ToInt32(strID);
                    await CargarDatosTramite(id_cpadron);
                }
            }

        }

        private async Task CargarDatosTramite(int id_cpadron)
        {
            ucCabecera.LoadData((int)Constants.GruposDeTramite.CP, id_cpadron);
            await ucListaDocumentos.LoadData((int)Constants.GruposDeTramite.CP, id_cpadron);
            ucListaTareas.LoadData((int)Constants.GruposDeTramite.CP, id_cpadron);
            //cargo las observaciones
            DGHP_Entities db = new DGHP_Entities();

            var objsol = (from sol in db.CPadron_Solicitudes
                          where sol.id_cpadron.Equals(id_cpadron)
                          select new
                          {
                              sol.observaciones,
                          }).FirstOrDefault();
            lblObservaciones.Text = objsol.observaciones;

            
            db.Dispose();
            updObservaciones.Update();
        }
    }
}
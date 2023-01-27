using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.CPadron.Reportes
{
    public partial class DescargarPlanos : System.Web.UI.Page
    {
        DGHP_Entities db;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    this.db = new DGHP_Entities();
                    ImprimirPlano();
                    this.db.Dispose();

                }
                catch (Exception ex)
                {
                    if (this.db != null)
                        this.db.Dispose();
                }

                Response.End();
            }
        }

        private void ImprimirPlano()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id"].ToString(); // ejemplo route
            }
            
            int id_cpadron_plano = 0;

            int.TryParse(strID, out id_cpadron_plano);

            var cpPlanos = db.CPadron_Planos.Where(x => x.id_cpadron_plano == id_cpadron_plano).FirstOrDefault();

            if (cpPlanos!= null)
            {
                var arc = ws_FilesRest.DownloadFile(cpPlanos.id_file);
                if (arc != null)
                {
                    string nombre_archivo = Convert.ToString(cpPlanos.nombre_archivo);
                    
                    string arch = "plano-cp" + cpPlanos.id_cpadron.ToString() + "-" + cpPlanos.id_cpadron_plano.ToString() + "-" + nombre_archivo;

                    MemoryStream msDocumento = new MemoryStream(arc);

                    Response.Clear();
                    Response.Buffer = true;
                    //Response.ContentType = "application/octet-stream";
                    Response.ContentType = Functions.GetMimeTypeByFileName(arch);
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + arch);
                    Response.AddHeader("Content-Length", msDocumento.Length.ToString());
                    Response.AddHeader("Transfer-Encoding", "identity");
                    Response.BinaryWrite(msDocumento.ToArray());
                }
                else
                    Response.Write("El documento indicado no pertence a la está visualizando.");
            }
            else
                Response.Write("No se ha encontrado el documento.");

        }

    }
}
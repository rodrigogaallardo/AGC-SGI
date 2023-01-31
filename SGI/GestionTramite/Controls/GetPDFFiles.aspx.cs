using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class GetPDFFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param_id = "";
                int id_file = 0;

                try
                {
                    param_id = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

                    if (string.IsNullOrEmpty(param_id))
                    {
                        param_id = Page.RouteData.Values["id"].ToString(); // ejemplo route
                    }

                    byte[] bidfile = Convert.FromBase64String(HttpUtility.UrlDecode(param_id));
                    id_file = int.Parse(System.Text.Encoding.ASCII.GetString(bidfile));

                }
                catch (Exception)
                {
                    LogError.Write(new Exception("Error al convertir el id_file, parametro enviado: " + param_id));
                }

                DescargarFile(id_file);
            }
        }
        private void DescargarFile(int id_file)
        {
            try
            {
                byte[] Pdf = new byte[0];
                string FileName = string.Empty;
                if (id_file > 0)
                {
                    try
                    {
                        Pdf = ws_FilesRest.descargarArchivo(id_file, out FileName);
                    }
                    catch { }
                }

                if (Pdf != null && Pdf.Length > 0)
                {
                    Response.Clear();
                    //Response.ContentType = "application/octet-stream";
                    Response.ContentType = Functions.GetMimeTypeByFileName(FileName);
                    Response.Headers.Add("Content-Length", $"{Pdf.Length}");
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", FileName));
                    //Response.AddHeader("Transfer-Encoding", "identity");
                    Response.OutputStream.Write(Pdf, 0, Pdf.Length);
                    Response.Flush();
                    //Response.Clear();
                    //Response.Buffer = true;//false;
                    //Response.ContentType = "application/octet-stream";
                    //Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
                    //Response.AddHeader("Content-Length", Pdf.Length.ToString());
                    //Response.BinaryWrite(Pdf);
                    //Response.Flush();
                }
                else
                {
                    Response.Clear();
                    Response.Write("No es posible encontrar el archivo");
                }

            }
            catch (Exception)
            {
                throw new Exception("Se produjo un error al recuperar el pdf.");
            }
        }
    }
}

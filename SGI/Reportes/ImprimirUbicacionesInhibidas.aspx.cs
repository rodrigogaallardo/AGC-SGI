using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ImprimirUbicacionesInhibidas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ImprimirArchivo();
                }
                catch (Exception ex)
                {
                    EnviarError(ex.Message);
                }

                Response.End();
            }


        }

        private void ImprimirArchivo()
        {

            try
            {
                string datos = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();
                
                byte[] bidfile = Convert.FromBase64String(HttpUtility.UrlDecode(datos));

                string valor = System.Text.Encoding.ASCII.GetString(bidfile);

                int indx = valor.IndexOf(",");
                string Tipo = valor.Substring(indx + 1);
                int id_ubicinhibida = Convert.ToInt32(valor.Substring(0, indx));

                byte[] documento = PDFInhibicion.GetUbicacionesInhibidas(Tipo, id_ubicinhibida);

                string nombArch = string.Format("UbicacionInhibida{0}.pdf", id_ubicinhibida);

                //mostrar archivo
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombArch);
                Response.AddHeader("Content-Length", documento.Length.ToString());
                Response.BinaryWrite(documento);
                Response.Flush();

            }
            catch (Exception)
            {
                throw new Exception("Se produjo un error al enviar pdf.");
            }

        }
        private void EnviarError(string mensaje)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/HTML";
            Response.AddHeader("Content-Disposition", "inline;filename=error.html");
            Response.Write(mensaje);
            Response.Flush();
        }
    }
}
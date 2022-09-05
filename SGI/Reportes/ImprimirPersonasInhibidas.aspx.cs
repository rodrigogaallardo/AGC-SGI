using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ImprimirPersonasInhibidas : System.Web.UI.Page
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

                int id_personainhibida = int.Parse(System.Text.Encoding.ASCII.GetString(bidfile));
                

                byte[] documento = PDFInhibicion.GetPersonasInhibidas(id_personainhibida);

                string nombArch = string.Format("PersonaInhibida{0}.pdf", id_personainhibida);

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
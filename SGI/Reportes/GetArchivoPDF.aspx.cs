using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class GetArchivoPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IniciarEntity();
                    ImprimirArchivo();
                    FinalizarEntity();
                }
                catch (Exception ex)
                {
                    FinalizarEntity();
                    EnviarError(ex.Message);
                }

                Response.End();
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

        private void ImprimirArchivo()
        {

            try
            {
                string strID = (Request.QueryString["id_file"] == null) ? "" : Request.QueryString["id_file"].ToString();

                if (string.IsNullOrEmpty(strID))
                {
                    strID = Page.RouteData.Values["id_file"].ToString(); // ejemplo route
                }


                int id_file = Convert.ToInt32(strID);

                Files archivo = db.Files.FirstOrDefault(x => x.id_file == id_file);


                byte[] documento = archivo.content_file;


                string nombArch = archivo.FileName;

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

        #region entity

        private AGC_FilesEntities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new AGC_FilesEntities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
                this.db.Dispose();
        }

        #endregion
    }
}
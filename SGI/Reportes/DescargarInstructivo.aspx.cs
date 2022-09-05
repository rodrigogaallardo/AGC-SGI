using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGI.Reportes
{
    public partial class DescargarInstructivo : System.Web.UI.Page
    {
        private DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enviarFlujo();
            }
            catch (Exception ex)
            {
                EnviarError(ex.Message);
            }

            Response.End();

        }

        private void enviarFlujo()
        {
            int id_Instructivo = (Request.QueryString["id"] == null) ? 0 : Convert.ToInt32(Request.QueryString["id"]);
            if (id_Instructivo == 0)
                id_Instructivo = (Page.RouteData.Values["id"] != null) ? Convert.ToInt32(Page.RouteData.Values["id"]) : 0;

            if (id_Instructivo == 0)
                id_Instructivo = 4;//profesionales elevadores
            //throw new Exception("Debe enviar el plano.");

            string arch = "";
            this.db = new DGHP_Entities();
            var obj =
                (
                    from instructivosRel in db.Instructivos
                    where instructivosRel.id_instructivo == id_Instructivo
                    select new
                    {
                        instructivosRel.id_file
                    }).FirstOrDefault();

            db.Dispose();

            if (obj == null)
                throw new Exception("El plano no existe. ");

            AGC_FilesEntities dbFile = new AGC_FilesEntities();
            var f =
                (
                    from file in dbFile.Files
                    where file.id_file == obj.id_file
                    select new
                    {
                        file.id_file,
                        file.content_file
                    }).FirstOrDefault();

            dbFile.Dispose();

            if (f == null)
                throw new Exception("El plano no existe. ");

            //arch = obj.nombre_archivo.Replace(' ', '_');

            arch = "Instructivo_" + obj.id_file + "." + "pdf";
            Response.Clear();
            Response.Buffer = true;//false;
            Response.ContentType = "application/pdf";

            Response.AddHeader("Content-Disposition", "attachment;filename=" + arch);
            Response.BinaryWrite(f.content_file);
            Response.Flush();




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
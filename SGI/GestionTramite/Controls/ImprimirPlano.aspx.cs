using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ImprimirPlano : System.Web.UI.Page
    {
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
            int id_encomienda_plano = (Request.QueryString["id"] == null) ? 0 : Convert.ToInt32(Request.QueryString["id"]);
            if (id_encomienda_plano == 0)
                id_encomienda_plano = (Page.RouteData.Values["id"] != null) ? Convert.ToInt32(Page.RouteData.Values["id"]) : 0;

            if (id_encomienda_plano == 0)
                throw new Exception("Debe enviar el plano.");

            string arch = "";
            DGHP_Entities db = new DGHP_Entities();
            var obj =
                (
                    from plano in db.Encomienda_Planos
                    where plano.id_encomienda_plano == id_encomienda_plano
                    select new
                    {
                        plano.id_encomienda_plano,
                        plano.id_file,
                        plano.nombre_archivo,
                        plano.id_encomienda
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
            string ext = obj.nombre_archivo.Substring(obj.nombre_archivo.Length-3);
            arch = "plano_habilitacion_"+obj.id_file+"." + ext;
            Response.Clear();
            Response.Buffer = true;
            //Response.ContentType = "application/octet-stream";
            Response.ContentType = Functions.GetMimeTypeByFileName(arch);
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + arch + "\"");
            //Response.AddHeader("Transfer-Encoding", "identity");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    public partial class ImprimirCertificado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                try
                {
                    ExportarPDF();
                }
                catch (Exception ex)
                {
                    EnviarError(ex.Message);
                }

                Response.End();
            }

        }
        private void ExportarPDF()
        {
            int id_certificado = (Page.RouteData.Values["id"] != null ) ? Convert.ToInt32(  Page.RouteData.Values["id"] ) :0;

            if (id_certificado == 0 )
                throw new Exception("Debe enviar el certificado");

            string arch = "";
            AGC_FilesEntities db = new AGC_FilesEntities();
            var obj =
                (from cert in db.Certificados
                 where cert.id_certificado == id_certificado
                 select new
                 {
                     cert.Certificado,
                     cert.TipoTramiteCertificados.Descripcion,
                     cert.NroTramite,
                     cert.TipoTramite,
                     cert.item
                 }).FirstOrDefault();
    
            db.Dispose();

            if (obj == null)
                throw new Exception("El certificado no existe. ");

            string desc_aux = "";
            if (obj.TipoTramite == (int)Constants.TipoTramiteCertificados.CAA && obj.item.HasValue && obj.item == 2)
            {
                desc_aux = "_reverso";
            }

            arch = obj.Descripcion.Replace(" ", "-") + "-" + obj.NroTramite.ToString() + desc_aux + ".pdf";
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + arch + "\"");
            Response.BinaryWrite(obj.Certificado);
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
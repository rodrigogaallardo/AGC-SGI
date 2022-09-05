using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Controls
{
    public partial class DescargarArchivoTemporal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DescargarArchivo();
                }
                catch (Exception ex)
                {
                    Enviar_Mensaje(ex.Message, "");
                }
                
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        private void DescargarArchivo()
        {
            try
            {
                string filename = (Request.QueryString["fname"] != null ? Request.QueryString["fname"].ToString() : "");
                filename = HttpUtility.UrlDecode(filename);
                string filepath = Constants.Path_Temporal + filename;


                if (filename.Length > 0)
                {

                    Response.Clear();
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename));

                    if (filename.EndsWith(".xls"))
                        Response.ContentType = "application/vnd.ms-excel";
                    if (filename.EndsWith(".xlsx"))
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    Response.Charset = "utf-8";

                    Response.WriteFile(filepath);

                    Response.End();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error de exportacion: Intente nuevamente.");
            }            
        }
    }
}
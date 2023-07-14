using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace SGI
{
    /// <summary>
    /// Descripción breve de Download
    /// </summary>
    public class Download : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            if (Membership.GetUser() == null)
            {
                context.Response.Clear();
                context.Response.Write("Debe estar logueado para estar en esta seccion");
            }
            else
            {
                if (context.Request.QueryString["filename"] != null)
                {
                    byte[] Pdf = null;

                    string FileName = context.Request.QueryString["filename"];

                    string FilePath = Constants.Path_Temporal + FileName;
                    if (System.IO.File.Exists(FilePath))                                            
                        //Pdf = System.IO.File.ReadAllBytes(FilePath);
                        Pdf = Encoding.UTF8.GetBytes(FilePath);
                                            
                    if (Pdf != null)
                    {
                        context.Response.Clear();
                        context.Response.Buffer = true;
                        context.Response.ContentType = Functions.GetMimeTypeByFileName(FileName);
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.AddHeader("Content-Disposition", "inline;filename=\"" + FileName + "\"");
                        context.Response.AddHeader("Content-Length", Pdf.Length.ToString());
                        context.Response.AddHeader("Connection", "keep-alive");
                        context.Response.AddHeader("Accept-Encoding", "identity");
                        context.Response.BinaryWrite(Pdf);
                        context.Response.Flush();

                    }
                    else
                    {
                        context.Response.Clear();
                        context.Response.Write("No es posible encontrar el archivo");
                    }
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
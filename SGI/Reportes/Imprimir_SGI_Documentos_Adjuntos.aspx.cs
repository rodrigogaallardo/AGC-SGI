using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using SGI.WebServices;

namespace SGI.Reportes
{
    public partial class Imprimir_SGI_Documentos_Adjuntos : System.Web.UI.Page
    {
        private AGC_FilesEntities dbFiles = null;
        private DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            {
                try
                {
                    this.dbFiles = new AGC_FilesEntities();
                    this.db = new DGHP_Entities();
                    ImprimirDocumentoAdjunto();
                    this.dbFiles.Dispose();
                }
                catch (Exception ex)
                {
                    if (this.dbFiles != null)
                        this.dbFiles.Dispose();
                    if (this.db != null)
                        this.db.Dispose();
                    EnviarError(ex.Message);
                }
                Response.End();
            }
        }

        private void ImprimirDocumentoAdjunto()
        {

            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            int id_doc_adjunto = Convert.ToInt32(strID);

            Guid userKey = (Guid)Membership.GetUser().ProviderUserKey;

            SGI_Tarea_Documentos_Adjuntos doc_adj = this.db.SGI_Tarea_Documentos_Adjuntos.FirstOrDefault(x => x.id_doc_adj == id_doc_adjunto);


            if (doc_adj == null)
            {
                throw new Exception("No se pudo encontarar pdf.");
            }

            TiposDeDocumentosRequeridos tipo = db.TiposDeDocumentosRequeridos.FirstOrDefault(x => x.id_tdocreq == doc_adj.id_tdocreq);

            if (tipo == null)
            {
                throw new Exception("No se pudo encontarar el tipo.");
            }
            byte[] file = new byte[0];
            string FileName = string.Empty;
            file = ws_FilesRest.descargarArchivo(doc_adj.id_file, out FileName);

            if (file == null)
                throw new Exception("El file no existe. ");

            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = Functions.GetMimeTypeByFileName(FileName);
                Response.ContentEncoding = Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "inline;filename=\"" + FileName + "\"");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.AddHeader("Connection", "keep-alive");
                Response.AddHeader("Accept-Encoding", "identity");
                Response.BinaryWrite(file);
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
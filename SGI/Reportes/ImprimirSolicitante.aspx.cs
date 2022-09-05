using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ImprimirSolicitante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IniciarEntity();
            ImprimirSolicitanteDeSolicitud();
            FinalizarEntity();
        }

        private void ImprimirSolicitanteDeSolicitud()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id"].ToString();
            }
            int id_tramite = Convert.ToInt32(strID);       

            ws_Reporting reportingService = new ws_Reporting();
            var ReportingEntity = reportingService.GetPDFSolicitantePorIdSolicitud(id_tramite);
            var documento = ReportingEntity.Reporte;

            Guid userid = Functions.GetUserId();

            if (documento.Length == 0)
            {
                throw new Exception("El documento está en blanco.");
            }
            else
            {
                try
                {
                    string nombArch = "Solicitante-" + strID.ToString() + ".pdf";

                    //mostrar archivo
                    Response.Clear();
                    Response.Buffer = true;
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

        #region entity

        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
                this.db.Dispose();
        }
        #endregion
    }
}
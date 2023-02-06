using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using SGI.WebServices;

namespace SGI.Reportes
{
    public partial class Imprimir_SolicitudNuevoCur : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IniciarEntity();
                    ImprimirCertificado();
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

        private void ImprimirCertificado()
        {
            int id_solicitud = 0;
            if (Page.RouteData.Values["id_solicitud"] != null)
            {
                String param_id = Page.RouteData.Values["id_solicitud"].ToString();
                byte[] bidfile = Convert.FromBase64String(HttpUtility.UrlDecode(param_id));
                id_solicitud = int.Parse(System.Text.Encoding.ASCII.GetString(bidfile));
            }
            else
            {
                id_solicitud = Convert.ToInt32(Request.QueryString["id"]);
            }
            byte[] pdfSolicitud = new byte[0];

            ws_Reporting reportingService = new ws_Reporting();
            var ReportingEntity = reportingService.GetPDFSolicitudNueva(id_solicitud, false);

            string arch = ReportingEntity.FileName;

            pdfSolicitud = ReportingEntity.Reporte;

            Response.Clear();
            Response.Buffer = true;//false;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + arch + "\"");

            Response.BinaryWrite(pdfSolicitud);
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
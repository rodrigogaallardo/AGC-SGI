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
    public partial class ImprimirDisposicion : System.Web.UI.Page
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

        private void EnviarError(string mensaje)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/HTML";
            Response.AddHeader("Content-Disposition", "inline;filename=error.html");
            Response.Write(mensaje);
            Response.Flush();
        }

        private void ImprimirCertificado()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id"].ToString(); // ejemplo route
            }

            int id_tramitetarea = Convert.ToInt32(strID);
            var q_datos =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join ssit in db.SSIT_Solicitudes on tt_hab.id_solicitud equals ssit.id_solicitud
                    where tt.id_tramitetarea == id_tramitetarea
                    select new
                    {
                        tt_hab.id_solicitud

                    }
                ).FirstOrDefault();

            int id_solicitud = q_datos.id_solicitud;

            Stream msPdfDisposicion = null;

            string expediente_actuacion = "XXXXXXXXXXXXX";// this.datos_caratula_nro_expediente; // GetExpediente();

            PdfDisposicion pdf = new PdfDisposicion();

            App_Data.dsImpresionDisposicion dsDispo = pdf.GenerarDataSetDisposicion(id_solicitud, id_tramitetarea, expediente_actuacion, true);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();

            try
            {

                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/Disposicion.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsDispo);
                msPdfDisposicion = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                string nombArch = "Disposicion-" + strID.ToString() + ".pdf";

                //mostrar archivo
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombArch);
                Response.AddHeader("Content-Length", msPdfDisposicion.Length.ToString());
                Response.BinaryWrite(Functions.StreamToArray(msPdfDisposicion));
                Response.Flush();

            }
            catch (Exception)
            {
                try
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }
                catch { }
                throw new Exception("Se produjo un error al enviar pdf.");
            }

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
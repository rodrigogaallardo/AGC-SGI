using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SGI.Reportes
{
    public partial class ImprimirProfesional : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IniciarEntity();
            ImprimirCertificado();
            FinalizarEntity();
        }

        private void ImprimirCertificado()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id"].ToString(); // ejemplo route
            }

            int id_profesional = Convert.ToInt32(strID);
            var q_datos =
                (
                    from pro in db.Profesional
                    where pro.Id == id_profesional
                    select new
                    {
                        
                    }
                ).FirstOrDefault();

            Stream msPdfDisposicion = null;

            string expediente_actuacion = "XXXXXXXXXXXXX";// this.datos_caratula_nro_expediente; // GetExpediente();

            Expediente exp = new Expediente();

            App_Data.dsImpresionProfesional dsProfesional = exp.GenerarDataSetProfesional(id_profesional);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();

            try
            {

                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/Profesional.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsProfesional);
                msPdfDisposicion = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                string nombArch = "Profesional-" + strID.ToString() + ".pdf";

                //mostrar archivo
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + nombArch + "\"");
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
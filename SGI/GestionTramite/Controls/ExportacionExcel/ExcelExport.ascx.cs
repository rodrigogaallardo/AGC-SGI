using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.ExportacionExcel
{
    public partial class ExcelExport : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void EjecutarExportacion(List<IEnumerable<object>> consultas, string FileName, string[] NombreHojas)
        {

            QueryExcel queryExcel = new QueryExcel();
            queryExcel.Page = this.Page as BasePage;
            queryExcel.queryList = consultas;
            queryExcel.NameColumns = NombreHojas;
            queryExcel.cant_registros_x_vez = 10000;

            mostrarTimer(FileName);
            
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(queryExcel.ExportarSolicitudesAExcel));
            thread.Start();
            (this.Page as BasePage).EjecutarScript(updExportaExcel, "showfrmExportarExcel();");

        }
        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlExportacionError.Style["display"] = "none";
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            (this.Page as BasePage).EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");

        }
        protected void mostrarTimer(string name)
        {
            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            Random random = new Random((int)DateTime.Now.Ticks);
            int NroAleatorio = random.Next(0, 100);
            NroAleatorio = NroAleatorio * random.Next(0, 100);
            name = name + "-{0}.xlsx";
            string fileName = string.Format(name, NroAleatorio);

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            Timer1.Enabled = true;
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                {
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                    lblExportarError.Text = Convert.ToString(Session["progress_data"]);
                }
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    filename = HttpUtility.UrlEncode(filename);
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
                //Cuando falla la exportacion
                if (Session["progress_data"].ToString().StartsWith("Error:"))
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlExportandoExcel.Style["display"] = "none";
                    pnlExportacionError.Style["display"] = "block";
                }
            }
            catch
            {
                Timer1.Enabled = false;
            }

        }
    }
}
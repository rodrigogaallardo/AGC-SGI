using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ExportarElevadoresPagosxEmpresaxDiasPendientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Exportar();

            }
        }


        private void Exportar()
        {

            //Guid userid = Functions.GetUserid();
            DGHP_Entities db = new DGHP_Entities();

            ObjectResult<ASRExportarElevadoresPagosxEmpresaxDiasPendientes_Result> result = db.ASRExportarElevadoresPagosxEmpresaxDiasPendientes();

            // Ubicacion, Razon_Social, Anio, Estado_Pago, Estado_Aceptacion, Cant_elevadores

            List<ASRExportarElevadoresPagosxEmpresaxDiasPendientes_Result> resultados = result.ToList();

            grdEmpresas.DataSource = resultados.ToList();
            grdEmpresas.DataBind();
            //Response.Clear();
            Response.Buffer = true;

            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "inline; filename=ElevadorespagosxEmpresaxDiasPendiente.xls");
            Response.Charset = "iso-8859-1";
            Response.ContentEncoding = System.Text.Encoding.Default;
            this.EnableViewState = false;

            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

            // Get the HTML for the control.

            //grdAscensores.RenderControl(hw);
            grdEmpresas.RenderBeginTag(hw);
            grdEmpresas.HeaderRow.RenderControl(hw);

            foreach (GridViewRow row in grdEmpresas.Rows)
            {
                //row.Cells.RemoveAt(5);
                row.RenderControl(hw);
            }
            grdEmpresas.FooterRow.RenderControl(hw);

            // Write the HTML back to the browser.
            Response.Write(tw.ToString());

        }
    }
}
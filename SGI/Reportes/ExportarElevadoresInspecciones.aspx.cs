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
    public partial class ExportarElevadoresInspecciones : System.Web.UI.Page
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

            ObjectResult<ASRExportarElevadoresInspecciones_Result> result = db.ASRExportarElevadoresInspecciones();

            // Ubicacion, Razon_Social, Anio, Estado_Pago, Estado_Aceptacion, Cant_elevadores

            List<ASRExportarElevadoresInspecciones_Result> resultados = result.ToList();

            grdEmpresas.DataSource = resultados.OrderBy(x => x.RazonSocial_empasc).ToList();
            grdEmpresas.DataBind();
            //Response.Clear();
            Response.Buffer = true;

            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "inline; filename=ElevadoresInspecciones.xls");
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

        private string GetEstado(int? id_estado_pago)
        {
            if (id_estado_pago == 0)
                return "Pendiente de Oblea";
            else if (id_estado_pago == 1)
                return "Oblea Obtenida";
            else if (id_estado_pago == 2)
                return "Oblea Vencida";
            return "Desconocido";
        }
    }
}
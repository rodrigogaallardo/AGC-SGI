using SGI.WebServices;
using System;
using System.Web.UI;

namespace SGI.GestionTramite.Controls
{
    public partial class ImprimirBoleta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ExportarPDF();
                }
                catch (Exception ex)
                {
                    EnviarError(ex.Message);
                }
                Response.End();
                
            }

        }
        
        private void ExportarPDF()
        {
            if (Page.RouteData.Values["id_pago"] != null)
            {
                int id_pago = Convert.ToInt32(Page.RouteData.Values["id_pago"]);

                ws_PagosRest servicePagos = new ws_PagosRest();

                byte[] PdfBoletaUnica = servicePagos.GetPDFBoletaUnica(id_pago);

                if (PdfBoletaUnica == null || PdfBoletaUnica.Length == 0)
                    throw new Exception("hubo un problema al conectarse al servicios de pagos, GetPDFBoletaUnica. No se pudo recuperar los datos del documento. ");

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + id_pago.ToString() + ".pdf");
                Response.AddHeader("Content-Length", PdfBoletaUnica.Length.ToString());
                Response.BinaryWrite(PdfBoletaUnica);
                Response.Flush();
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
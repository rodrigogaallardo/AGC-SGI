using System;

namespace SGI.Reportes
{
    public partial class ImprimirBoletaUnica : System.Web.UI.Page
    {

       protected void Page_Load(object sender, EventArgs e)
       {
           if (!IsPostBack)
           {
               try
               {
                   ImprimirCertificado();
               }
               catch (Exception ex)
               {
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
            string strIdPago = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();
            string strIsApra = (Request.QueryString["isApra"] == null) ? "" : Request.QueryString["isApra"].ToString();
            int id_pago = Convert.ToInt32(strIdPago);
            SGI.Webservices.Pagos.wsResultado result = new SGI.Webservices.Pagos.wsResultado();
            string Usuario_WSPagos = "";
            string Password_WSPagos = "";

            if (strIsApra == "1")
            {
                Usuario_WSPagos = Functions.GetParametroChar("Apra.Pagos.User");
                Password_WSPagos = Functions.GetParametroChar("Apra.Pagos.Password");
            }
            else
            {
                Usuario_WSPagos = Functions.GetParametroChar("SSIT.WebService.Pagos.User");
                Password_WSPagos = Functions.GetParametroChar("SSIT.WebService.Pagos.Password");
            }

            SGI.Webservices.Pagos.ws_pagos servicePagos = new SGI.Webservices.Pagos.ws_pagos();
            servicePagos.Url = Functions.GetParametroChar("Pagos.Url");

            byte[] PdfBoletaUnica = servicePagos.GetPDFBoletaUnica(Usuario_WSPagos, Password_WSPagos, id_pago, ref result);

            if (PdfBoletaUnica != null && PdfBoletaUnica.Length > 0)
            {
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline;filename=" + id_pago.ToString() + ".pdf");
                Response.AddHeader("Content-Length", PdfBoletaUnica.Length.ToString());
                Response.BinaryWrite(PdfBoletaUnica);
                Response.Flush();
            }
            else
            {
                Response.Clear();
                Response.Write("El servicio de pagos no ha podido recuperar el pdf de la boleta. ");
            }

        }

    }
}
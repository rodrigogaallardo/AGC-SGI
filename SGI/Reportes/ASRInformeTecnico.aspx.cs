using DGFYCO_Prof;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ASRInformeTecnico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    if (Page.RouteData.Values["id_inspeccion"] != null)
                    {
                        int id_inspeccion = int.Parse(Page.RouteData.Values["id_inspeccion"].ToString());
                        Imprimir(id_inspeccion);
                    }
                    else
                    {
                        Server.Transfer("~/Errores/error3001.aspx");
                    }



                }
                catch (Exception ex)
                {
                }

                Response.End();
            }


        }

        private void Imprimir(int id_inspeccion)
        {
            ws_DGFYCOInformesTecnicos ws = new ws_DGFYCOInformesTecnicos();
            ws.Url = Parametros.GetParam_ValorChar("Elevadores.Empresas.url") + "/ws_DGFYCOInformesTecnicos.asmx";
            byte[] msArchivo = ws.getInformeInspeccionASR(id_inspeccion);
            try
            {
                string filename = "";
                if (id_inspeccion <= 0)
                    filename = string.Format("inspeccion-{0}.pdf", id_inspeccion);
                else
                    filename = string.Format("inspeccion-{0}.pdf", id_inspeccion);


                //mostrar archivo
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
                Response.AddHeader("Content-Length", msArchivo.Length.ToString());
                Response.BinaryWrite(msArchivo.ToArray());
                Response.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
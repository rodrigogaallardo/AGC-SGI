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
    public partial class ImprimirCertificado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IniciarEntity();
                    Imprimir();
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

        private void Imprimir()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id"].ToString(); // ejemplo route
            }

            int id_solicitud = Convert.ToInt32(strID);

            string expediente_actuacion = "XXXXXXXXXXXXX";// this.datos_caratula_nro_expediente; // GetExpediente();

            byte[] documento = Certificado.GenerarPdf(id_solicitud, expediente_actuacion, true);

            try
            {
                string nombArch = "Certificado-" + strID.ToString() + ".pdf";

                //mostrar archivo
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + nombArch + "\"");
                Response.AddHeader("Content-Length", documento.Length.ToString());
                Response.BinaryWrite(documento);
                Response.Flush();

            }
            catch (Exception)
            {
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
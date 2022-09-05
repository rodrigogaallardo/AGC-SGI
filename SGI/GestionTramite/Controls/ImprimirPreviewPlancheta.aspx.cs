using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ImprimirPreviewPlancheta : System.Web.UI.Page
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
            string strID = (Request.QueryString["id_solicitud"] == null) ? "" : Request.QueryString["id_solicitud"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id_solicitud"].ToString(); // ejemplo route
            }

            int id_solicitud = Convert.ToInt32(strID);
            var q_datos =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join ssit in db.SSIT_Solicitudes on tt_hab.id_solicitud equals ssit.id_solicitud
                    where tt_hab.id_solicitud == id_solicitud
                    orderby tt.id_tramitetarea descending
                    select new
                    {
                        tt.id_tramitetarea,
                    }
                ).FirstOrDefault();
            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault()  == id_solicitud
                                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
            int id_tramitetarea = q_datos.id_tramitetarea;

            string expediente_actuacion = "XXXXXXXXXXXXX";// this.datos_caratula_nro_expediente; // GetExpediente();

            byte[] documento = Plancheta.GenerarPdfPlanchetahabilitacion(id_solicitud, id_tramitetarea, enc.id_encomienda, expediente_actuacion, true);

            try
            {
                string nombArch = "Plancheta-" + strID.ToString() + ".pdf";

                //mostrar archivo
                Response.Clear();
                Response.Buffer = true;//false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombArch);
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
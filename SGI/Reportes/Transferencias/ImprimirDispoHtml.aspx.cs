using Microsoft.Ajax.Utilities;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes.Transferencias
{
    public partial class ImprimirDispoHtml : System.Web.UI.Page
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
                    LogError.Write(ex, "ImprimirDispoHTML - Page_Load");
                    FinalizarEntity();
                }

                Response.End();
            }

        }

        private void Imprimir()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();
            string NroExpediente = (Request.QueryString["nro_expediente"] == null) ? "" : Request.QueryString["nro_expediente"].ToString();

            if (string.IsNullOrEmpty(strID))
            {
                strID = Page.RouteData.Values["id"].ToString(); // ejemplo route
            }

            if (string.IsNullOrEmpty(NroExpediente))
            {
                NroExpediente = Page.RouteData.Values["nro_expediente"].ToString();
                if(string.IsNullOrEmpty(NroExpediente))
                {
                    NroExpediente = "";
                }
            }

            int id_solicitud = Convert.ToInt32(strID);
            var tt = db.SGI_Tramites_Tareas_TRANSF.Where(x => x.id_solicitud == id_solicitud).OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();

            int id_tramitetarea = tt.id_tramitetarea;
            
            string str_archivo = string.Empty;
            string html_dispo = string.Empty;
            if (tt.SGI_Tramites_Tareas.ENG_Tareas.id_circuito != (int)Constants.ENG_Circuitos.TRANSF_NUEVO)
            {
                str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\Disposicion.html"));
                html_dispo = PdfDisposicion.Transf_GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, NroExpediente, str_archivo);
            }
            else
            {
                Transf_Solicitudes transf = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_solicitud).FirstOrDefault();
                if (transf.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_Transferencia)
                {
                    str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionTransferencia.html"));
                    if (Functions.isTransmisionCambioActividad(id_solicitud))
                        str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionTransferencia.CambioActividad.html"));
                    if (Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Rechazado
                        || Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Rechazo_In_Limine)
                        str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionTransferencia.Rechazo.html"));
                }
                else if (transf.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_nominacion)
                {
                    str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionCambioDenominacion.html"));
                    if (Functions.isTransmisionCambioActividad(id_solicitud))
                        str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionCambioDenominacion.CambioActividad.html"));
                    if (Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Rechazado
                        || Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Rechazo_In_Limine)
                        str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionCambioDenominacion.Rechazo.html"));
                }
                else if (transf.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_oficio_judicial)
                {
                    str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionOficioJudicial.html"));
                    if (Functions.isTransmisionCambioActividad(id_solicitud))
                        str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionOficioJudicial.CambioActividad.html"));
                    if (Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Rechazado
                        || Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Rechazo_In_Limine)
                        str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmisionOficioJudicial.Rechazo.html"));
                }
                else
                {
                    str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmision.html"));
                }
                html_dispo = PdfDisposicion.Transmision_GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, NroExpediente, str_archivo);
            }
            //mostrar archivo
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/HTML; charset=UTF8";
            Response.AddHeader("Content-Disposition", "inline;filename=\"Disposicion.html\"");
            Response.AddHeader("Content-Length", Encoding.UTF8.GetBytes(html_dispo).Length.ToString());
            Response.AddHeader("Connection", "keep-alive");
            Response.AddHeader("Accept-Encoding", "identity");
            Response.BinaryWrite(Encoding.UTF8.GetBytes(html_dispo));
            Response.Flush();        
        }

        #region entity

        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
                this.db.Dispose();
        }

        #endregion

    }
}
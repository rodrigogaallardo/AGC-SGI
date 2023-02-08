using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes.CPadron
{
    public partial class ImprimirInformeTipo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            {
                try
                {
                    IniciarEntity();
                    IniciarEntityFiles();
                    ImprimirDocumento();
                    FinalizarEntity();
                    FinalizarEntityFiles();
                }
                catch (Exception ex)
                {
                    FinalizarEntity();
                    FinalizarEntityFiles();
                    EnviarError(ex.Message);
                }
                Response.End();
            }
        }

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

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

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
                this.dbFiles.Dispose();
        }

        #endregion

        private void ImprimirDocumento()
        {
            string strID = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            string stringTT = (Request.QueryString["tt"] == null) ? "" : Request.QueryString["tt"].ToString();

            int ttramite = Convert.ToInt32(stringTT);

            int id_cpadrontarea = Convert.ToInt32(strID);

            int idCpadron = 0;
            SGI_Tarea_Carga_Tramite tramiteatarea = this.db.SGI_Tarea_Carga_Tramite.Where(x => x.id_tramitetarea == id_cpadrontarea).FirstOrDefault();

            SGI_Tramites_Tareas_CPADRON tt = this.db.SGI_Tramites_Tareas_CPADRON.Where(x => x.id_tramitetarea == id_cpadrontarea).FirstOrDefault();

            ws_Reporting reportingService = new ws_Reporting();
            ExternalService.Class.ReportingEntity ReportingEntity = new ExternalService.Class.ReportingEntity();
            if (tt != null)
            {
                idCpadron = tt.id_cpadron;
                ReportingEntity = reportingService.GetPDFInformeCPadron(idCpadron, ttramite, true);
            }
            else
            {
                idCpadron = this.db.SGI_Tramites_Tareas_TRANSF.Where(x => x.id_tramitetarea == id_cpadrontarea).FirstOrDefault().Transf_Solicitudes.id_cpadron;
                ReportingEntity = reportingService.GetPDFTransmisionesInformeCPadron(idCpadron, ttramite, true);                
            }            
                        
            var documento = ReportingEntity.Reporte;

            Guid userid = Functions.GetUserId();

            if (documento.Length == 0)
            {
                throw new Exception("El documento está en blanco.");
            }
            else
            {
                try
                {
                    string nombArch = "Informe - Tipo "+ ttramite +"-"+ strID.ToString() + ".pdf";

                    //mostrar archivo
                    Response.Clear();
                    Response.Buffer = true;//false;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + nombArch + "\"");
                    Response.AddHeader("Content-Length", documento.Length.ToString());
                    Response.BinaryWrite(documento);
                    Response.Flush();
                }
                catch (Exception ex)
                {
                    throw new Exception("Se produjo un error al enviar pdf.");
                }
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
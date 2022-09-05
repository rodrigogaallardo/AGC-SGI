using SGI.App_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ThoughtWorks.QRCode.Codec;

namespace SGI.Model
{
    public class Observaciones_SADE
    {
        #region habilitaciones
        public static byte[] GenerarPdf(int id_solicitud)
        {
            byte[] documento = null;

            DGHP_Entities db = new DGHP_Entities();

            Stream msPdfDisposicion = null;

            dsObservaciones dsPlancheta = GenerarDataSet(id_solicitud);


            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/ObservacionesSADE.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsPlancheta);
                msPdfDisposicion = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                documento = Functions.StreamToArray(msPdfDisposicion);

            }
            catch (Exception ex)
            {
                try
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }
                catch { }

                msPdfDisposicion = null;
                throw ex;
            }
            return documento;
        }

        public static dsObservaciones GenerarDataSet(int id_solicitud)
        {
            dsObservaciones ds = new dsObservaciones();

            DataRow row;
            DGHP_Entities db = new DGHP_Entities();

            var lstObservaciones = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                    join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                    join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                    join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                    where tt_h.id_solicitud == id_solicitud && obs.id_file_sade==null
                                    select new 
                                    {
                                        id_ObsDocs = obs.id_ObsDocs,
                                        nombre_tdocreq = tdocreq.nombre_tdocreq,
                                        Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                        Respaldo_ObsDocs = obs.Respaldo_ObsDocs
                                    }).ToList();

            DataTable observaciones = ds.Tables["Observaciones"];

            foreach (var item in lstObservaciones)
            {
                row = observaciones.NewRow();
                row["id_ObsDocs"] = item.id_ObsDocs;
                row["nombre_tdocreq"] = item.nombre_tdocreq;
                row["Observacion_ObsDocs"] = item.Observacion_ObsDocs;
                row["Respaldo_ObsDocs"] = item.Respaldo_ObsDocs;
                observaciones.Rows.Add(row);
            }

            db.Dispose();
            return ds;
        }

        #endregion

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SGI.GestionTramite.Controls.ExportacionExcel
{
    public class QueryExcel
    {
        public BasePage Page { get; set; }
        public IList<IEnumerable<object>> queryList { get; set; }
        public decimal cant_registros_x_vez { get; set; }
        public IList<string> NameColumns { get; set; }
        public void ExportarSolicitudesAExcel()
        {
            int totalRowCount = 0;
            int startRowIndex = 0;
            Model.DGHP_Entities db = new Model.DGHP_Entities();

            try
            {
                List<List<object>> resultados = new List<List<object>>();
                int Index = 0;

                foreach (var query in queryList)
                {
                    Index++;
                    // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                    totalRowCount = query.Count();

                    int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                    List<object> resultadosParcial = new List<object>();
                    for (int i = 1; i <= cantidad_veces; i++)
                    {
                        resultadosParcial.AddRange(query.Skip(startRowIndex).Take((int)cant_registros_x_vez));
                        Page.Session["progress_data"] = string.Format("Hoja {0} </br> {1} / {2} registros exportados.", Index, resultadosParcial.Count, totalRowCount);
                        startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                    }
                    Page.Session["progress_data"] = string.Format("Hoja {0} </br> {1} / {2} registros exportados.", Index, resultadosParcial.Count, totalRowCount);

                    resultados.Add(resultadosParcial);

                    startRowIndex = 0;
                }

                string savedFileName = Constants.Path_Temporal + Page.Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();
                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Model.CreateExcelFile.CreateExcelDocument(resultados, savedFileName, NameColumns);
                // quita la variable de session.
                Page.Session.Remove("progress_data");
                Page.Session.Remove("exportacion_en_proceso");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogError.Write(ex.InnerException);
                    Page.Session["progress_data"] = "Error: " + ex.InnerException.Message;
                }
                else
                {
                    LogError.Write(ex);
                    Page.Session["progress_data"] = "Error: " + ex.Message;
                }
            }
            db.Dispose();
        }        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGI.App_Data;
using System.Data;
using SGI.WebServices;

namespace SGI.Model
{
    public class Solicitante
    {
        public static byte[] SolicitantePorIdSolicitud_GenerarPdf(int id_tramite)
        {
            byte[] documento = null;

            try
            {
                ws_Reporting reportingService = new ws_Reporting();
                var ReportingEntity = reportingService.GetPDFSolicitantePorIdSolicitud(id_tramite);
                documento = ReportingEntity.Reporte;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return documento;         

        }
    }
}
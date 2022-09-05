using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{

    public partial class ucPreviewDocumentos : System.Web.UI.UserControl
    {
        public void LoadData(int id_solicitud)
        {
            LoadData(id_solicitud, (int)Constants.TipoDeTramite.Habilitacion);
        }

        public void LoadData(int id_solicitud, int tipoTramite)
        {
            DGHP_Entities db = new DGHP_Entities();

            if (tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
            {
                List<itemDocumentov1> objCertificado = new List<itemDocumentov1>();

                itemDocumentov1 itemNuevo = new itemDocumentov1();
                itemNuevo.id_solicitud = 1;
                itemNuevo.nombre = "Plancheta Habilitación";
                itemNuevo.url = string.Format("~/ImprimirPreviewPlancheta/{0}", id_solicitud);
                objCertificado.Add(itemNuevo);

                itemNuevo = new itemDocumentov1();
                itemNuevo.id_solicitud = 1;
                itemNuevo.nombre = "Disposicion HTML";

                SGI_Tramites_Tareas_HAB tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == id_solicitud).OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();
                string nro_expediente = "XXXXXXXXXXXXXXXXX";

                int id_resultado;
                id_resultado = Functions.isResultadoDispo(id_solicitud);
                int nroSolReferencia = 0;
                int.TryParse(Functions.GetParametroChar("NroSolicitudReferencia"), out nroSolReferencia);

                if ((id_solicitud > nroSolReferencia 
                    && (id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado 
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Requiere_Rechazo 
                    || tarea.SSIT_Solicitudes.id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion 
                    || tarea.SSIT_Solicitudes.id_tipotramite == (int)Constants.TipoDeTramite.RedistribucionDeUso
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado_Reconsideracion
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Observado_Reconsideracion
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado_Reconsideracion)) 
                    || Functions.caduco(id_solicitud))
                    itemNuevo.url = string.Format("~/ImprimirDispoHtmlNuevoCur/{0}/{1}/{2}", id_solicitud, tarea.id_tramitetarea, nro_expediente);
                else
                    itemNuevo.url = string.Format("~/ImprimirPreviewDispoHtml/{0}/{1}/{2}", id_solicitud, tarea.id_tramitetarea, nro_expediente);
                objCertificado.Add(itemNuevo);

                repeater_certificados.DataSource = objCertificado;
                repeater_certificados.DataBind();
            }
            else if (tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
            {
                List<itemDocumentov1> objCertificado = new List<itemDocumentov1>();

                itemDocumentov1 itemNuevo = new itemDocumentov1();

                itemNuevo.id_solicitud = 1;
                itemNuevo.nombre = "Plancheta Transferencia";
                itemNuevo.url = string.Format("~/ImprimirPreviewPlanchetaTransf/{0}", id_solicitud);
                objCertificado.Add(itemNuevo);

                itemNuevo = new itemDocumentov1();
                itemNuevo.id_solicitud = 1;
                itemNuevo.nombre = "Disposicion HTML";

                SGI_Tramites_Tareas_TRANSF tarea = db.SGI_Tramites_Tareas_TRANSF.Where(x => x.id_solicitud == id_solicitud).OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();
                string nro_expediente = "XXXXXXXXXXXXXXXXX";
                itemNuevo.url = string.Format("~/ImprimirDispoHtmlTransf/{0}/{1}", id_solicitud, nro_expediente);
                objCertificado.Add(itemNuevo);

                repeater_certificados.DataSource = objCertificado;
                repeater_certificados.DataBind();
            }
        }
    }
}
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace SGI
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute("MenuItems_Route", "Menu/Items/{id}", "~/Menuitems.aspx");
            routes.MapPageRoute("ImprimirCertificadoRoute", "ImprimirCertificado/{id}", "~/GestionTramite/Controls/ImprimirCertificado.aspx",false);
            routes.MapPageRoute("ImprimirDocumentoAdjuntoRoute", "ImprimirDocumentoAdjunto/{id_docadjunto}", "~/GestionTramite/Controls/ImprimirDocumentoAdjunto.aspx",false);
            routes.MapPageRoute("ImprimirPreviewPlanchetaRoute", "ImprimirPreviewPlancheta/{id_solicitud}", "~/GestionTramite/Controls/ImprimirPreviewPlancheta.aspx");
            routes.MapPageRoute("ImprimirPreviewDispoHtmlRoute", "ImprimirPreviewDispoHtml/{id_solicitud}/{id_tramitetarea}/{nro_expediente}", "~/Reportes/ImprimirDispoHtml.aspx");
            routes.MapPageRoute("ImprimirPreviewDispoNuevaHtmlRoute", "ImprimirDispoHtmlNuevoCur/{id_solicitud}/{id_tramitetarea}/{nro_expediente}", "~/Reportes/ImprimirDispoHtmlNuevoCur.aspx");
            routes.MapPageRoute("ImprimirBoletaRoute", "ImprimirBoleta/{id_pago}/{Numero_BU}", "~/GestionTramite/Controls/ImprimirBoleta.aspx",false);
            routes.MapPageRoute("GetTramiteQrRoute", "GetTramiteQr/{id_encomienda}", "~/Tramite/TramiteQr.aspx",false);
            routes.MapPageRoute("GetPDFRoute", "GetPDF/{id}", "~/GestionTramite/Controls/GetPDF.aspx", false);
            routes.MapPageRoute("GetPDFFilesRoute", "GetPDFFiles/{id}", "~/GestionTramite/Controls/GetPDFFiles.aspx", false);
            
            //Consulta al padron
            routes.MapPageRoute("GestionTramiteCPRoute", "VisorTramiteCP/{id}", "~/GestionTramite/VisorTramite_CP.aspx");
            routes.MapPageRoute("DescargarPlanosCPRoute", "DescargarPlanosCP/{id}", "~/Reportes/CPadron/DescargarPlanos.aspx");
            routes.MapPageRoute("BuscarTramite", "GestionTramite/BusquedaTramite/{guidJason}", "~/GestionTramite/BuscarTramite.aspx");
            routes.MapPageRoute("ConsultarTramite", "GestionTramite/ConsultarTramite/{guidJason}", "~/GestionTramite/ConsultaTramite.aspx");
            routes.MapPageRoute("BuscarTramiteSSIT", "GestionTramite/Consulta_SSIT/BusquedaTramiteSSIT/{guidJason}", "~/GestionTramite/Consulta_SSIT/BuscarTramiteSSIT.aspx");

            //Transferencias
            routes.MapPageRoute("GestionTramiteTRRoute", "VisorTramiteTR/{id}", "~/GestionTramite/VisorTramite_TR.aspx");
            routes.MapPageRoute("ImprimirDocumentoAdjuntoTRRoute", "ImprimirDocumentoAdjuntoTR/{id}", "~/GestionTramite/Transferencia/Reportes/ImprimirDocumentoAdjunto.aspx");
            routes.MapPageRoute("ImprimirPreviewPlanchetaTransfRoute", "ImprimirPreviewPlanchetaTransf/{id_solicitud}", "~/GestionTramite/Controls/ImprimirPreviewPlanchetaTransf.aspx");
            //routes.MapPageRoute("ImprimirPreviewDispoHtmlTransfRoute", "ImprimirDispoHtmlTransf/{id}", "~/Reportes/Transferencias/ImprimirDispoHtml.aspx");
            routes.MapPageRoute("ImprimirPreviewDispoHtmlTransfRoute", "ImprimirDispoHtmlTransf/{id}/{nro_expediente}", "~/Reportes/Transferencias/ImprimirDispoHtml.aspx");

            //Informes Tecnicos
            routes.MapPageRoute("ImprimirInspeccion_Route", "DescargarInforme/{id_inspeccion}", "~/Reportes/ASRInformeTecnico.aspx");
            routes.MapPageRoute("ImprimirInspeccionIFCI_Route", "DescargarInformeIFCI/{id_inspeccion}", "~/Reportes/IFCIInformeTecnico.aspx");
            routes.MapPageRoute("ImprimirInspeccionRIT_Route", "DescargarInformeRIT/{id_inspeccion}", "~/Reportes/RITInformeTecnico.aspx");
            
        }
    }
}

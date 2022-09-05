<%@ Page Title="Tarea: Revisión Firma Disposición" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Revision_Firma_Disposicion.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Revision_Firma_Disposicion" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosExpediente.ascx" TagPrefix="uc1" TagName="ucProcesosExpediente" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaDocumentoAdjuntoAnteriores.ascx" TagPrefix="uc1" TagName="ucSGI_ListaDocumentoAdjuntoAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucPreviewDocumentos.ascx" TagPrefix="uc1" TagName="ucPreviewDocumentos" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>

    <uc1:uccabecera runat="server" id="ucCabecera" />
    <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
    <uc1:ucTramitesRelacionados runat="server" id="ucTramitesRelacionados" />
    <uc1:uclistadocumentos runat="server" id="ucListaDocumentos" />
    <uc1:ucPreviewDocumentos runat="server" id="ucPreviewDocumentos" />

    <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
    <asp:HiddenField ID="hid_id_tramitetarea" runat="server"  Value="0"/>

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
             <h5><%: Page.Title %></h5>
        </div>
        <div class="widget-content">
            <div style="padding: 20px">
                <div style="width: 100%;">

                    <div>

                        <uc1:ucListaObservacionesAnteriores ID="ucListaObservacionesAnt" runat="server" />

                        <uc1:ucSGI_ListaDocumentoAdjuntoAnteriores ID="ucSGI_ListaDocumentoAdjuntoAnt" runat="server" />

                        <uc1:ucProcesosExpediente runat="server" ID="ucProcesosExpediente" 
                             OnProceso_Finalizado="ucProcesosExpediente_Proceso_Finalizado" 
                              OnProcesoError="ucProcesosExpediente_Error" />

                    </div>
                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" 
                        btnGuardar_Visible ="false"
                        OnCerrarClick="ucResultadoTarea_CerrarClick" 
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick"  />

               </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">


        // el script para mostrar errores debe estar arriba porque en caso de que de error en 
        // load page debe tener el script cargado desde el inicio
        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
    </script>  
</asp:Content>





<%@ Page Title="Tarea: Calificar Trámite" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Calificar.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Transferencias.Calificar" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnterioresv1.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnterioresv1" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaDocumentoAdjuntoAnteriores.ascx" TagPrefix="uc1" TagName="ucSGI_ListaDocumentoAdjuntoAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucDocumentoAdjunto.ascx" TagPrefix="uc1" TagName="ucDocumentoAdjunto" %>
<%@ Register Src="~/GestionTramite/Controls/ucPreviewDocumentos.ascx" TagPrefix="uc1" TagName="ucPreviewDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/Transferencias/ucTitulares.ascx" TagPrefix="uc1" TagName="ucTitulares" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTareav1.ascx" TagPrefix="uc1" TagName="ucObservacionesTareav1" %>

<asp:content id="Content1" contentplaceholderid="HeadContent" runat="server">
    <script type="text/javascript">
        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
</script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="FeaturedContent" runat="server">
</asp:content>
<asp:content id="Content3" contentplaceholderid="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>

    <uc1:uccabecera runat="server" id="ucCabecera" />
    <uc1:ucTitulares runat="server" id="ucTitulares" />
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

                    <uc1:ucSGI_ListaDocumentoAdjuntoAnteriores runat="server" ID="ucSGI_ListaDocumentoAdjuntoAnteriores" Collapse="true" />
                    <uc1:ucDocumentoAdjunto runat="server" ID="ucDocumentoAdjunto" />

                    <uc1:ucListaObservacionesAnteriores ID="ucListaObservacionesAnteriores" runat="server" Collapse="true" />
                    <uc1:ucListaObservacionesAnterioresv1 ID="ucListaObservacionesAnterioresv1" runat="server" Collapse="true" />

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionContribuyente" 
                        LabelObservacion="Observaciones para Contribuyente"/>

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionesTarea" UpdateMode="Conditional"
                        LabelObservacion="Observaciones para plancheta" />

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionesInternas" 
                        LabelObservacion="Observaciones Internas" />

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionProvidencia"
                        LabelObservacion="Providencia" />

                    <uc1:ucObservacionesTareav1 runat="server" id="ucObservaciones"/>

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" 
                        OnGuardarClick="ucResultadoTarea_GuardarClick" 
                        OnCerrarClick="ucResultadoTarea_CerrarClick" 
                        OnResultadoSelectedIndexChanged="ucResultadoTarea_ResultadoSelectedIndexChanged" 
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick"  />


               </div>
            </div>
        </div>
    </div>
                
</asp:content>

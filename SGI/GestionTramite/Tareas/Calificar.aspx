﻿<%@ Page Title="Tarea: Calificar Trámite" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Calificar.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Calificar" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTareav1.ascx" TagPrefix="uc1" TagName="ucObservacionesTareav1" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnterioresv1.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnterioresv1" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaDocumentoAdjuntoAnteriores.ascx" TagPrefix="uc1" TagName="ucSGI_ListaDocumentoAdjuntoAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_DocumentoAdjunto.ascx" TagPrefix="uc1" TagName="ucSGI_DocumentoAdjunto" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>
<%@ Register Src="~/GestionTramite/Controls/ucPreviewDocumentos.ascx" TagPrefix="uc1" TagName="ucPreviewDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaPlanoVisado.ascx" TagPrefix="uc1" TagName="ucSGI_ListaPlanoVisado"%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>

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

                    <uc1:ucProcesosSADEv1 runat="server" ID="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />

                    <uc1:ucSGI_ListaDocumentoAdjuntoAnteriores runat="server" ID="ucSGI_ListaDocumentoAdjuntoAnteriores" Collapse="true" />
                    <uc1:ucSGI_ListaPlanoVisado runat="server" ID="ucSGI_ListaPlanoVisado" />
                    <uc1:ucSGI_DocumentoAdjunto runat="server" ID="ucSGI_DocumentoAdjunto" />

                    <uc1:ucListaObservacionesAnteriores ID="ucListaObservacionesAnteriores" runat="server" Collapse="true" />
                    <uc1:ucListaObservacionesAnterioresv1 ID="ucListaObservacionesAnterioresv1" runat="server" Collapse="true" />

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionContribuyente" 
                        LabelObservacion="Observaciones para Contribuyente"/>


                    
                <uc1:ucObservacionesTarea runat="server" id="ucObservacionesTarea" UpdateMode="Conditional"
                        LabelObservacion="Notas adicionales para la disposición" />

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionesInternas" 
                        LabelObservacion="Observaciones Internas" />

                    <uc1:ucObservacionesTareav1 runat="server" id="ucObservaciones"/>
                    
                    <asp:UpdatePanel ID="pnl_Librar_Uso" runat="server" Visible="false">
                        <ContentTemplate>
                            <div class="control-group">
                                <label class="control-label">Librar Uso:</label>
                                <asp:CheckBox ID="chbLibrarUso" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="ChbLibrarUso_CheckedChanged" />
                            </div>
                            <uc1:ucObservacionesTarea runat="server" ID="UcObservacionesLibrarUso" UpdateMode="Conditional"
                                LabelObservacion="Observaciones adicionales al Librado al Uso para la Oblea:" Enabled="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" 
                        OnGuardarClick="ucResultadoTarea_GuardarClick" 
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnResultadoSelectedIndexChanged="ucResultadoTarea_ResultadoSelectedIndexChanged" 
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick"  />


               </div>
            </div>
        </div>
    </div>
                
</asp:Content>

﻿<%@ Page Title="Tarea: Revisión Director" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Revision_Director.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Revision_Director" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaResultadoTareasAnteriores.ascx" TagPrefix="uc1" TagName="ucListaResultadoTareasAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
    <uc1:ucTramitesRelacionados runat="server" id="ucTramitesRelacionados" />
    <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
    
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
                    </div>

                    <uc1:ucListaObservacionesAnteriores ID="ucListaObservacionesAnteriores" runat="server" />
                    
                    <uc1:ucListaResultadoTareasAnteriores ID="ucListaResultadoTareasAnteriores" runat="server" Collapse="true" Titulo="Resultado Tarea Anterior" />

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />

                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function mostratMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
    </script>

</asp:Content>

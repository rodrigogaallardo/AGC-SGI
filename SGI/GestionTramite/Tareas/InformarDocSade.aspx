<%@ Page Title="Tarea: Informar Documentos a SADE" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="InformarDocSade.aspx.cs" Inherits="SGI.GestionTramite.Tareas.InformarDocSade" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>

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

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
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

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucProcesosSADEv1 runat="server" ID="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />


                    <asp:UpdatePanel ID="updFinalizarTarea" runat="server">
                        <ContentTemplate>
                            <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                                OnGuardarClick="ucResultadoTarea_GuardarClick"
                                OnCerrarClick="ucResultadoTarea_CerrarClick"
                                OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

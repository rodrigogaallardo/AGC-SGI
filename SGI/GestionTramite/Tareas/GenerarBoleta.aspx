<%@ Page Title="Tarea: Generar Boleta" MasterPageFile="~/Site.Master"  Language="C#" AutoEventWireup="true" CodeBehind="GenerarBoleta.aspx.cs" Inherits="SGI.GestionTramite.Tareas.GenerarBoleta" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucMediosPagos.ascx" TagPrefix="uc1" TagName="ucMediosPagos" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>

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
    <%: Styles.Render("~/bundles/iconMoonCss") %>

    <uc1:uccabecera runat="server" id="ucCabecera" />
    <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
    <uc1:ucTramitesRelacionados runat="server" id="ucTramitesRelacionados" />
    <uc1:uclistadocumentos runat="server" id="ucListaDocumentos" />

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

                    <asp:Panel ID="pnl_area_tarea" runat="server">
                    
                    </asp:Panel>

                    <uc1:ucMediosPagos runat="server" id="ucMediosDePago" 
                       OnClick_GenerarBoletaUnica="GenerarBoletaUnica_Click"
                        GenerarBoleta_Visible="false" />

                    <%--<uc1:ucObservacionesTarea runat="server" id="ucObservacionesTarea" />--%>
                    <asp:UpdatePanel ID="updResultadoTarea" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" 
                                OnGuardarClick="ucResultadoTarea_GuardarClick" 
                                OnCerrarClick="ucResultadoTarea_CerrarClick" 
                                OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick"  />

                        </ContentTemplate>
                    </asp:UpdatePanel>


                </div>
            </div>
        </div>
    </div>
                




</asp:Content>





<%@ Page Title="Tarea: Generar Expediente" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Generar_Expediente.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Transferencias.Generar_Expediente" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <div>
            <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
        </div>
        <div>
            <label style="font-size: 24px">Cargando...</label>
        </div>
    </div>

     <asp:UpdatePanel ID="updCargaInicial" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />

            <asp:Panel ID="pnlErrorCargaInicial" runat="server" CssClass="alert alert-danger" Visible="false">
                <asp:Label ID="lblErrorCargaInicial" runat="server"></asp:Label>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updCargarProcesos" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnCargarProcesos" runat="server" OnClick="btnCargarProcesos_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="page_content" style="display: none">
        <asp:UpdatePanel ID="updControlesCabecera" runat="server">
        <ContentTemplate>
                <uc1:ucCabecera runat="server" ID="ucCabecera" />
                <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5><%: Page.Title %></h5>
        </div>
        <div class="widget-content">
            <div style="padding: 20px">
                <div style="width: 100%;">

                        <uc1:ucProcesosSADEv1 runat="server" id="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />

                        <asp:UpdatePanel ID="updFinalizarTarea" runat="server">
                            <ContentTemplate>

                                <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" btnGuardar_Visible="false"
                                    OnCerrarClick="ucResultadoTarea_CerrarClick"
                                    OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--Modal mensajes de error--%>
    <div id="frmError" class="modal fade" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Error</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->


    <script type="text/javascript">

        $(document).ready(function () {
            $("#page_content").hide();
            $("#Loading").show();

            $("#<%: btnCargarDatos.ClientID %>").click();

        });
        function finalizarCarga() {

            $("#<%: btnCargarProcesos.ClientID %>").click();

            $("#Loading").hide();
            $("#page_content").show();


            return false;
        }

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

    </script>

</asp:Content>

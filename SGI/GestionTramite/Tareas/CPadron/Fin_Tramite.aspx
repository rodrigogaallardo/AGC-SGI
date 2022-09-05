<%@ Page Title="Tarea: Fin de Trámite" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Fin_Tramite.aspx.cs" Inherits="SGI.GestionTramite.Tareas.CPadron.Fin_Tramite" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tabs_Tramite.ascx" TagPrefix="uc1" TagName="Tabs_Tramite" %>

<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="page_content" style="display: none">

        <asp:UpdatePanel ID="updCargaTramite" runat="server">
            <ContentTemplate>
                <uc1:uccabecera runat="server" id="ucCabecera" />
                <uc1:uclistadocumentos runat="server" id="ucListaDocumentos" />
                <uc1:tabs_tramite runat="server" id="Tabs_Tramite" />
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

                                <uc1:uclistaobservacionesanteriores id="ucListaObservacionesAnteriores" runat="server" collapse="true" />

                                <uc1:ucobservacionestarea runat="server" id="ucObservacionContribuyente"
                                    labelobservacion="Observaciones para Contribuyente" />

                                <uc1:ucobservacionestarea runat="server" id="ucObservacionesTarea"
                                    labelobservacion="Observaciones Internas" />

                                <uc1:ucresultadotarea runat="server" id="ucResultadoTarea"
                                    onguardarclick="ucResultadoTarea_GuardarClick"
                                    oncerrarclick="ucResultadoTarea_CerrarClick"
                                    onfinalizartareaclick="ucResultadoTarea_FinalizarTareaClick" />

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--Modal mensajes de error--%>
    <div id="frmError_Carga" class="modal fade" style="display: none;">
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
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showfrmError_Carga() {
            $("#frmError_Carga").modal("show");
            return false;
        }

    </script>
</asp:Content>

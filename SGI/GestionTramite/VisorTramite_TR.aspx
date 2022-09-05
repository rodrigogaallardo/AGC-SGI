<%@ Page Title="Visualización del trámite" Language="C#" AutoEventWireup="true" CodeBehind="VisorTramite_TR.aspx.cs" MasterPageFile="~/Site.Master" Inherits="SGI.GestionTramite.VisorTramite_TR" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>
<%@ Register Src="~/GestionTramite/Controls/ucPagos.ascx" TagPrefix="uc1" TagName="ucPagos" %>
<%@ Register Src="~/GestionTramite/Controls/ucNotificaciones.ascx" TagPrefix="uc1" TagName="ucNotificaciones" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
        
    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                <td>
                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>

    <asp:UpdatePanel ID="updCargaTramite" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hid_id_solicitud" runat="server" />
            
            <asp:Button ID="btnFinalizar" runat="server" OnClick="btnFinalizar_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="page_content" style="display: none">

      
        <asp:UpdatePanel ID="updPagina" runat="server">
            <ContentTemplate>


                <uc1:ucCabecera runat="server" ID="ucCabecera" />
                <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
                <uc1:ucTramitesRelacionados runat="server" ID="ucTramitesRelacionados" />
                <uc1:ucPagos runat="server" ID="ucPagos" />
                <uc1:ucNotificaciones runat="server" ID="ucNotificaciones" />
                <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
                <uc1:ucListaTareas runat="server" ID="ucListaTareas" />
            <%--    <div id="box_observaciones" class="accordion-group widget-box">

                    <div class="accordion-heading">
                        <a id="observaciones_btnUpDown" data-parent="#collapse-group" href="#collapse_observaciones"
                            data-toggle="collapse" onclick="observaciones_btnUpDown_collapse_click(this)">

                            <asp:HiddenField ID="hid_observaciones_collapse" runat="server" Value="true" />
                            <asp:HiddenField ID="hid_observaciones_visible" runat="server" Value="false" />

                            <div class="widget-title">
                                <span class="icon"><i class="icon icon-list-alt"></i></span>
                                <h5>
                                    <asp:Label ID="lbl_observaciones_tituloControl" runat="server" Text="Observaciones"></asp:Label></h5>
                                <span class="btn-right"><i class="icon icon-chevron-down"></i></span>
                            </div>
                        </a>
                    </div>

                    <div class="accordion-body collapse" id="collapse_observaciones">
                        <div class="widget-content">
                            <asp:UpdatePanel ID="updObservaciones" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table border="0" style="border-collapse: separate; border-spacing: 5px;">
                                        <tr>
                                            <td>Observaciones: </td>
                                            <td>
                                                <asp:Label ID="lblObservaciones" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>--%>
                
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    

    <%--modal de Errores--%>
    <div id="frmError" class="modal fade">
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
                                        <asp:Label ID="lblError" runat="server" Style="color: Black"></asp:Label>
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
    <script type="text/javascript">
        $(document).ready(function () {

            $("#page_content").hide();
            $("#Loading").show();

          $("#<%: btnFinalizar.ClientID %>").click();
        });

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function showfrmError() {

            $("#frmError").modal("show");

            return false;
        }
        function observaciones_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(obj).find("i.icon-chevron-down").length > 0) {
                    $(obj).find("i.icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
                else {
                    $(obj).find("i.icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }

    </script>
</asp:Content>

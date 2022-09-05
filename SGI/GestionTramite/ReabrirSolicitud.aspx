<%@ Page Title="Reabrir Circuito" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReabrirSolicitud.aspx.cs" Inherits="SGI.GestionTramite.ReabrirSolicitud" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script type="text/javascript">

            // el script para mostrar errores debe estar arriba porque en caso de que de error en 
            // load page debe tener el script cargado desde el inicio
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
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <asp:Panel ID="pnlBotonDefault" runat="server">

        <div class="">

            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- Titulo collapsible Parametros Superficie --%>
                <div class="accordion-heading">
                    <a id="bt_Reabrir_Circuito_btnUpDown" data-parent="#collapse-group" href="#bt_Reabrir_Circuito_Bandeja" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_Reabrir_Circuito_tituloControl" runat="server" Text="Desvencer Solicitud"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="bt_Reabrir_Circuito_Bandeja">
                    <div class="widget-content">
                        <%-- Campos de Texto --%>
                        <asp:UpdatePanel ID="updPnlReabrir" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <div class="row-fluid">
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="ReqSolicitud" runat="server" ValidationGroup="Reabrir"
                                                            ControlToValidate="txtSolicitud" Display="Dynamic" CssClass="field-validation-error"
                                                            ErrorMessage="Ingrese la Solicitud."></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- Boton para Actualizar --%>
                        <asp:UpdatePanel ID="updPnlBotonReabrir" runat="server">
                            <ContentTemplate>
                                <div class="pull-right">
                                    <asp:LinkButton ID="btnReabrir" runat="server" CssClass="btn btn-inverse" ValidationGroup="Reabrir" OnClick="btnReabrir_Click">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Desvencer Solicitud</span>
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    
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

    <div id="frmMsj" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Mensaje</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon imoon-info fs32" style="color: darkcyan"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updMsj" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMsj" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('.modal-backdrop').remove();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            inicializar_controles();
        });

        function bt_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(href_collapse).css("height") == "0px") {
                    $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
                else {
                    $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }
        function inicializar_controles() {
            $('#<%=txtSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        }

        function showfrmMsj() {
            $('.modal-backdrop').remove();
            $("#frmMsj").modal("show");
            return false;
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }
    </script>

</asp:Content>

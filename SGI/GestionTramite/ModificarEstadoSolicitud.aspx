<%@ Page Title="Actualizar estado solicitud" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModificarEstadoSolicitud.aspx.cs" Inherits="SGI.GestionTramite.ModificarEstadoSolicitud" %>

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

    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

        <%-- Titulo collapsible Parametros Superficie --%>
        <div class="accordion-heading">
            <a id="bt_ActEstado_btnUpDown" data-parent="#collapse-group" href="#bt_ActEstado_Bandeja" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="bt_ActEstado_tituloControl" runat="server" Text="Actualizar estado solicitud"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                </div>
            </a>
        </div>

        <div class="accordion-body collapse in" id="bt_ActEstado_Bandeja">
            <div class="widget-content">

                <asp:UpdatePanel ID="updPnlEstados" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ReqSolicitud" runat="server" ValidationGroup="Estados"
                                                ControlToValidate="txtSolicitud" Display="Dynamic" CssClass="field-validation-error"
                                                ErrorMessage="Ingrese la Solicitud."></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblCombo" runat="server" AssociatedControlID="ddlCombo" Text="Categoria:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlCombo" runat="server" style="width: auto;"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updPnlBotonReabrir" runat="server">
                    <ContentTemplate>
                        <div class="pull-right">
                            <asp:LinkButton ID="btnReabrir" runat="server" CssClass="btn btn-inverse" ValidationGroup="Estados" OnClick="btnReabrir_Click">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Cambiar estado</span>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
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

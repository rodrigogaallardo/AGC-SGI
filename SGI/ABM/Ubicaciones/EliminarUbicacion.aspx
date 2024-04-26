<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EliminarUbicacion.aspx.cs" Inherits="SGI.ABM.Ubicaciones.EliminarUbicacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <%--ajax cargando ...--%>

    <asp:HiddenField ID="hid_valor_boton" runat="server" />
    <asp:HiddenField ID="hid_observaciones" runat="server" />

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
    <div id="page_content" style="display: none">
        <hgroup class="title">
            <h1>ABM de Ubicaciones</h1>
            <h1><%: Title %>.</h1>
        </hgroup>
        <div id="box_datos">
            <div class="col-sm-12 col-md-12">
                <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_id_ubicacion" runat="server" />
                        <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                        <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                            <div>
                                <%-- Observaciones --%>
                                <div class="widget-box">
                                    <div class="widget-title">
                                        <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                        <h5>Observaciones</h5>
                                    </div>
                                    <div class="widget-content">
                                        <fieldset>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label class="control-label">Observaciones:</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div id="Req_Observaciones" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Por favor ingrese un número una Observación, el campo no puede ser vacio.
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                                <%-- Fin Observaciones --%>
                                <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                                    <ContentTemplate>
                                        <div class="pull-right" style="margin-bottom: 20px">
                                            <div class="control-group">
                                                <div id="Val_PartidaNueva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Revise las validaciones en pantalla.
                                                </div>
                                            </div>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger" OnClientClick="return validarBorrado();" OnClick="btnEliminar_Click">
                                                            <i class="imoon imoon-remove fs16"></i>
                                                            <span class="text">Borrar</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClick="btnCancelar_Click">
                                                            <i class="imoon imoon-blocked"></i>
                                                            <span class="text">Cancelar</span>
                                            </asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="updfrmerror" runat="server">
                        <ContentTemplate>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="frmerrortitle" runat="server" Text="Error"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <i class="imoon imoon-remove-circle fs64" style="color: #f00"></i>
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

    <%--Modal Eliminar Log--%>
    <div id="frmEliminarLog" class="modal fade" style="max-width: 400px;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Eliminar</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="control-label">Observaciones del Solicitante:</label>
                        <div class="controls">
                            <asp:TextBox ID="txtObservacionesSolicitante" runat="server" CssClass="form-control" Columns="10" Width="95%" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Botones --%>
                <div class="modal-footer" style="text-align: left;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptar_Click" />
                    <asp:Button ID="btnCancelarObservacion" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelarObservacion_Click" />
                </div>
            </div>
        </div>
    </div>

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

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function validarBorrado() {
            var ret = true;

            if ($.trim($("#<%: txtObservaciones.ClientID %> ").val()).length == 0) {
                $("#Req_Observaciones").css("display", "inline-block");
                ret = false;
                return ret;
            }
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }
    </script>
</asp:Content>

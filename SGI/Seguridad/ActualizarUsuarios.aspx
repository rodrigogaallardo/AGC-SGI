<%@ Page Title="Actualización de datos SADE" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActualizarUsuarios.aspx.cs" Inherits="SGI.Seguridad.ActualizarUsuarios" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <link href="../Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />


    <hgroup class="title">
        <h1>Usuarios.</h1>
    </hgroup>


    <div id="page_content">

        <%-- Muestra Busqueda--%>
        <div id="box_busqueda">

            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar">



                <div class="widget-content">
                    <div class="form-horizontal">
                        <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <div style="display: inline-block">
                                        <h5>Total de Usuarios para Actualizar:</h5>
                                    </div>

                                    <div style="display: inline-block">
                                        <span class="badge">
                                            <asp:Label ID="lblCantidadUsuarios" runat="server"></asp:Label></span>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="pull-right">

                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>

                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClick="btnActualizar_Click">
                                <span class="icon" ><i class="imoon imoon-search"></i></span>
                                <span class="text">Actualizar Usuarios SADE</span>
                        </asp:LinkButton>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <br />
            <br />

            <%-- Muestra Resultados--%>
            <div id="box_resultado" style="display: none;">
                <div class="widget-box">
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-horizontal" style="margin-left: 15px; margin-right: 15px">
                                <div style="display: inline-block">
                                    <h5>Cantidad de registros actualizados:</h5>
                                </div>
                                <div style="display: inline-block">
                                    <span class="badge">
                                        <asp:Label ID="lblCantidadRegistros" runat="server"></asp:Label></span>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>

    </div>

    <%--modal Aviso OK--%>
    <div id="frmAviso" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Informaci&oacute;n</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-ok fs64" style="color: green"></label>
                            </td>
                            <td>
                                <div class="pad20">

                                    <asp:UpdatePanel ID="updLabelAviso" runat="server" class="form-group">
                                        <ContentTemplate>
                                            <asp:Label ID="lblAviso" runat="server" Style="color: Black"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>


                                </div>

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
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="control-group">
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

    <!-- /.modal -->

    <script type="text/javascript">

        $(document).ready(function () {


            init_Js_updpnlBuscar();
            init_Js_updResultados();
            init_Js_updDatosUsuario();

        });


        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }
        function showfrmError() {

            $("#frmError").modal("show");

            return false;
        }


        function init_Js_updResultados() {
            toolTips();
        }



        function hidefrmConfirmarEliminar() {
            $("#frmConfirmarEliminar").modal("hide");
            return false;
        }
        function ocultarBotonesConfirmacion() {
            $("#pnlBotonesConfirmacionEliminar").hide();
            return false;
        }

        function showDatosUsuario() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }
        function GetoutDatos() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
            //$("#box_resultado").show("slow");

        }
        function showResultado() {
            $("#box_resultado").show("slow");
        }


        function showfrmAviso() {

            $("#frmAviso").modal("show");
            GetoutDatos();
            return false;
        }

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }


    </script>
</asp:Content>

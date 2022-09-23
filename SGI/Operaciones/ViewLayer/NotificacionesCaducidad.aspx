﻿<%@  Title="Notificaciones de Caducidad" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="NotificacionesCaducidad.aspx.cs" Inherits="SGI.NotificacionesCaducidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function redirect(obj) {
            location.href = obj.data - href;
        }
    </script>

    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnNotificarCaducidad">
        <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Notificaciones de Caducidad</h5>
            </div>
            <div class="widget-content">
                <asp:UpdatePanel ID="updPnlFiltroCaducar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <fieldset>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group" style="vertical-align:top;">
                                            <asp:Label ID="lblNroSolicitud" runat="server" Text="Solicitud:" class="control-label" AssociatedControlID="txtNroSolicitud" style="vertical-align:bottom"></asp:Label>
                                            <asp:TextBox ID="txtNroSolicitud" runat="server" Width="80px" style="vertical-align:bottom"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                ControlToValidate="txtNroSolicitud" runat="server"
                                                ErrorMessage="Solo se admiten números."
                                                ValidationExpression="\d+" style="vertical-align:bottom">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                             </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <br/>
        <asp:UpdatePanel ID="updPnlNotificarCaducidad" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hid_puede_modificar" runat="server" Value="false" />
                <div class="pull-right">
                    <div class="control-group inline-block">
                        <asp:UpdateProgress ID="updPrgss_NotificarCaducidad" AssociatedUpdatePanelID="updPnlNotificarCaducidad"
                            runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <asp:Button ID="btnNotificarCaducidad" runat="server" CssClass="btn  btn-inverse" ValidationGroup="caducar" OnClick="btnNotificarCaducidad_OnClick" Text="Notificar Caducidad">
                    </asp:Button>
                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick" OnClientClick="LimpiarFormulario();">
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
                    </asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
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
                                <asp:UpdatePanel ID="updResultados" runat="server" class="form-group">
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


    <script>
        function LimpiarFormulario()
        {
            document.getElementById("MainContent_txtNroSolicitud").value = "";
        }

        function showfrmError()
        {
            $("#frmError").modal("show");
            return false;
        }
    </script>
</asp:Content>
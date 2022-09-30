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

    <div class="control-group">
        <label class="control-label" for="txtNroSolicitud">Solicitud</label>
        <div class="controls">
            <asp:TextBox id="txtNroSolicitud" Width="80px" runat="server" CssClass="controls"/>
        </div>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
            ControlToValidate="txtNroSolicitud" runat="server"
            ErrorMessage="Solo se admiten números."
            ValidationExpression="\d+">
        </asp:RegularExpressionValidator>
    </div>
    <div class="control-group" >
        <label for="txtFechaNotificacion" class="control-label">Fecha Notificación:</label>
            <div class="controls">
                <asp:TextBox ID="txtFechaNotificacion" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                <div class="req">
                    <asp:RegularExpressionValidator
                        ID="rev_txtFechaNotificacion" runat="server" 
                        ValidationGroup="buscar"
                        ControlToValidate="txtFechaNotificacion" CssClass="field-validation-error" 
                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                        Display="Dynamic">
                    </asp:RegularExpressionValidator>  
                </div>

            </div>

            </div>

    <div class="control-group pull-right">
        <asp:Button ID="btnNotificarCaducidad" runat="server" CssClass="btn btn-primary" ValidationGroup="caducar" OnClick="btnNotificarCaducidad_OnClick" Text="Notificar Caducidad"></asp:Button>
        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClick="btnLimpiar_OnClick" OnClientClick="LimpiarFormulario();">
            <i class="icon-refresh"></i>
            <span class="text">Limpiar</span>
        </asp:LinkButton>
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

    <%--modal de Success--%>
    <div id="frmSuccess" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Éxito.</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-ok-sign fs64" style="color: #67eb34"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updResultados2" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblSuccess" runat="server" Style="color: Black"></asp:Label>
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

        $(document).ready(function () {
            inicializar_controles();
        });

        function inicializar_fecha()
        {

        var fechaDesde = $('#<%=txtFechaNotificacion.ClientID%>');
        var es_readonly = $(fechaDesde).attr("readonly");
        if (!($(fechaDesde).is('[disabled]') || $(fechaDesde).is('[readonly]'))) {
           $(fechaDesde).datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                                'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                                    'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 0,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''

                }
                );
        }
    }

        function inicializar_controles()
        {
            inicializar_fecha();
        }

        function LimpiarFormulario()
        {
            document.getElementById("MainContent_txtNroSolicitud").value = "";
        }

        function showfrmError()
        {
            $("#frmError").modal("show");
            return false;
        }

        function showfrmSuccess()
        {
            $("#frmSuccess").modal("show");
            return false;
        }


    </script>
</asp:Content>
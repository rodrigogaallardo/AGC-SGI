<%@  Title="Notificaciones de una Solicitud" Language="C#" MasterPageFile="~/Site.Master"
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

    
    <asp:HiddenField ID="hid_valor_boton" runat="server" />
    <asp:HiddenField ID="hid_observaciones" runat="server" />

    <%--Nro de Solicitud--%>
    <div style="display: flex;">
        <div class="control-group">
            <label class="control-label" for="txtNroSolicitud">Solicitud</label>
            <div class="controls">
                <asp:TextBox ID="txtNroSolicitud" Width="80px" runat="server" CssClass="controls" />
            </div>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                ControlToValidate="txtNroSolicitud" runat="server"
                ErrorMessage="Solo se admiten números."
                ValidationExpression="\d+">
            </asp:RegularExpressionValidator>
        </div>

        <%--Motivo de Notificacion--%>
        <div class="control-group" style="padding-right: 30px">
            <label class="control-label" for="txtNroSolicitud">Motivo de Notificación</label>
            <div class="controls">
                <asp:DropDownList ID="ddlNotificaciones_motivos" runat="server" AutoPostBack="false" CssClass="controls"
                    DataTextField="NotificacionMotivo" DataValueField="IdNotificacionMotivo">
                </asp:DropDownList>
            </div>

        </div>


        <%--Fecha Notificacion--%>
        <div class="control-group" style="padding-left: 30px">
            <label for="txtFechaNotificacion" class="control-label">Fecha Notificación</label>
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
    </div>


        <%--Botones--%>
        <div class="control-group" style="float: right">
            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_OnClick" Text="Buscar" />
            <asp:Button ID="btnNotificar" runat="server" CssClass="btn btn-primary" ValidationGroup="caducar" OnClick="btnNotificar_OnClick" Text="Notificar Solicitud"></asp:Button>
            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClick="btnLimpiar_OnClick" OnClientClick="LimpiarFormulario();">
            <i class="icon-refresh"></i>
            <span class="text">Limpiar</span>
            </asp:LinkButton>
        </div>

    <div class="accordion-group widget-box">
    <div class="accordion-heading">
        <a id="btnUpDownNot" data-parent="#collapse-group" href="#collapseNoti" 
            data-toggle="collapse" onclick="btnUpDownNot_click(this)">

            <div class="widget-title">
                <span class="icon"><i class="icon-th-list"></i></span>
                <h5>Notificaciones</h5>       
            </div>
        </a>

    </div>


    <div class="accordion-body collapse in" id="collapseNoti">
        <div class="widget-content">
            <asp:Panel runat="server" ID="pnlResultadoBuscar">
                <asp:UpdatePanel ID="updPnlNotificaciones" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hfMailID" runat="server" />
                        <asp:GridView ID="grdBuscarNotis"
                            runat="server"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            CssClass="table table-bordered table-striped table-hover with-check"
                            DataKeyNames="Mail_ID, id_solicitud"
                            ItemType="SGI.Model.clsItemGrillaBuscarMails"
                            OnRowDataBound="grdBuscarMails_RowDataBound">
                            <Columns>
                                <asp:BoundField Visible="false" DataField="Mail_ID" HeaderText="ID" />
                                <asp:BoundField Visible="false" DataField="id_solicitud" HeaderText="ID Solicitud" />
                                <%--<asp:BoundField DataField="Mail_Estado" HeaderText="Estado" />--%>
                                <%--<asp:BoundField DataField="Mail_Proceso" HeaderText="Proceso" />--%>
                                <asp:BoundField DataField="Mail_Asunto" HeaderText="Asunto" ItemStyle-Width="300px" />
                                <asp:BoundField DataField="Mail_Email" HeaderText="E-Mail" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Mail_Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                                <asp:BoundField DataField="MailFechaNot_FechaSSIT" HeaderText="F. Notificación" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                                <asp:TemplateField ItemStyle-Width="15px" HeaderText="Ver" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDetalles" runat="server" ToolTip="Ver Detalle" CssClass="link-local" CommandArgument='<%#Eval("Mail_ID")%>' OnClick="lnkDetalles_Click">
                                <i class="icon-eye-open" style="transform: scale(1.1);"></i>
                                        </asp:LinkButton>
                                        <asp:Panel ID="pnlDetalle" runat="server" class="modal fade" data-backdrop="static" Style="display: none; min-width: 90%; left: 350px; top: 0px; height: 100%">
                                            <div class="modal-dialog" role="document" style="height: 100%">
                                                <div class="modal-content" style="height: 100%">
                                                    <div class="modal-header">
                                                        <a class="close" data-dismiss="modal">×</a>
                                                        <h3>Detalle del E-Mail</h3>
                                                    </div>
                                                    <div class="modal-body" style="height: 100%">
                                                        <asp:Table ID="Table1" runat="server" HorizontalAlign="Center" Font-Size="8px" Width="100%">
                                                            <asp:TableHeaderRow VerticalAlign="Middle" HorizontalAlign="Center">
                                                                <asp:TableHeaderCell Visible="false">ID</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Email</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Asunto</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Proceso</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Fecha de Alta</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Fecha de Envio</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Prioridad</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Font-Bold="true">Cant. de Intentos</asp:TableHeaderCell>
                                                            </asp:TableHeaderRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell Visible="false" ID="IDCorreo"></asp:TableCell>
                                                                <asp:TableCell ID="Email"></asp:TableCell>
                                                                <asp:TableCell ID="Asunto"></asp:TableCell>
                                                                <asp:TableCell ID="Proceso"></asp:TableCell>
                                                                <asp:TableCell ID="FecAlta"></asp:TableCell>
                                                                <asp:TableCell ID="FecEnvio"></asp:TableCell>
                                                                <asp:TableCell ID="Prioridad"></asp:TableCell>
                                                                <asp:TableCell ID="CantInt"></asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableHeaderRow VerticalAlign="Middle" HorizontalAlign="Center">
                                                                <asp:TableHeaderCell ColumnSpan="8" Font-Bold="true">Mensaje</asp:TableHeaderCell>
                                                            </asp:TableHeaderRow>

                                                        </asp:Table>

                                                        <iframe style="width: 100%; height: 70%; border-style: none;" id="Message" runat="server"></iframe>

                                                    </div>
                                                </div>
                                            </div>

                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15px" HeaderText="Eliminar" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEliminar"
                                            runat="server" ToolTip="Eliminar"
                                            CssClass="link-local"
                                            CommandArgument='<%#Eval ("Mail_Id")%>'
                                            CommandName='<%#Eval ("id_solicitud")%>'
                                            OnClientClick="javascript:return tda_confirm_del();"
                                            OnClick="lnkEliminar_Click">
                                <i class="icon-trash" style="transform: scale(1.1);"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                    No se encontraron registros.
                                </asp:Panel>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>
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

    <%--modal de Rectificada--%>
    <div id="frmRectificada" class="modal fade">
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
                                <asp:UpdatePanel ID="updResultados3" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRectificada" runat="server" Style="color: Black"></asp:Label>
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
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
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
            document.getElementById("MainContent_txtFechaNotificacion").value = "";
        }

        function btnUpDownNot_click(obj) {
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

        function showfrmRectificada() {
            $("#frmRectificada").modal("show");
            return false;
        }
        function tda_confirm_del() {

            return confirm('¿Esta seguro que desea eliminar este Registro?');
        }

        function showResultado() {
            $("#box_resultado").show();
        }
    </script>
</asp:Content>
<%@ Page Title="Buscar Mail" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Buscar_mail.aspx.cs"
    Inherits="SGI.GestionTramite.Buscar_mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <link href="../Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            inicializar_controles();
            toolTips();
        });

        function inicializar_controles() {
            inicializar_fechas();
            $('#<%=txtNroSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
        }

        function inicializar_fechas() {
            var fechaDesde = $('#<%=txtEnvioFechaDesde.ClientID%>');
            var es_readonly = $(fechaDesde).attr("readonly");
            if (!($(fechaDesde).is('[disabled]') || $(fechaDesde).is('[readonly]'))) {
                $(fechaDesde).datepicker(
                    {
                        closeText: 'Cerrar', prevText: '&#x3c;Ant', nextText: 'Sig&#x3e;', currentText: 'Hoy',
                        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                        weekHeader: 'Sm', dateFormat: 'dd/mm/yy', firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: ''
                    }
                );
            }

            var fechaHasta = $('#<%=txtEnvioFechaHasta.ClientID%>');
            var es_readonly = $(fechaHasta).attr("readonly");

            if (!($(fechaHasta).is('[disabled]') || $(fechaHasta).is('[readonly]'))) {
                $(fechaHasta).datepicker({
                    maxDate: "0", closeText: 'Cerrar', prevText: '&#x3c;Ant', nextText: 'Sig&#x3e;', currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm', dateFormat: 'dd/mm/yy', firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: ''
                });
            }

            var fechaAltaDesde = $('#<%=txtFechaAltaDesde.ClientID%>');
            var es_readonly = $(fechaAltaDesde).attr("readonly");
            if (!($(fechaAltaDesde).is('[disabled]') || $(fechaAltaDesde).is('[readonly]'))) {
                $(fechaAltaDesde).datepicker({
                    maxDate: "0", closeText: 'Cerrar', prevText: '&#x3c;Ant', nextText: 'Sig&#x3e;', currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm', dateFormat: 'dd/mm/yy', firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: ''
                });
            };

            var fechaAltaHasta = $('#<%=txtFechaAltaHasta.ClientID%>');
            var es_readonly = $(fechaAltaHasta).attr("readonly");
            if (!($(fechaAltaHasta).is('[disabled]') || $(fechaAltaHasta).is('[readonly]'))) {
                $(fechaAltaHasta).datepicker({
                    maxDate: "0", closeText: 'Cerrar', prevText: '&#x3c;Ant', nextText: 'Sig&#x3e;', currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm', dateFormat: 'dd/mm/yy', firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: ''
                });
            };
        }
    </script>

    <%-- filtros de busqueda--%>
    <div class="accordion-group widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5>Buscar</h5>
        </div>

        <div class="widget-content">

            <asp:UpdatePanel ID="updPnlFiltroBuscar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblMail" runat="server" AssociatedControlID="txtMail" Text="E-mail:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtMail" runat="server" MaxLength="100" Width="185px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblAsunto" runat="server" AssociatedControlID="txtAsunto" Text="Asunto:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAsunto" runat="server" MaxLength="100" Width="185px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblEstadoMail" runat="server" AssociatedControlID="ddlEstadoMail" Text="Estado:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlEstadoMail" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoPago_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <%--                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblProceso" runat="server" AssociatedControlID="ddlProceso" Text="Origen:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlProceso" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlProceso_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>

                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtFechaAltaDesde" Text="Fecha Alta Desde:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaAltaDesde" runat="server" MaxLength="10" Width="185px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="RegularExpressionValidator1" runat="server" ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaAltaDesde" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="Label3" runat="server" AssociatedControlID="txtFechaAltaHasta" Text="Fecha Alta Hasta:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaAltaHasta" runat="server" MaxLength="10" Width="185px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="RegularExpressionValidator2" runat="server" ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaAltaHasta" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtEnvioFechaDesde" Text="Fecha Envío Desde:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtEnvioFechaDesde" runat="server" MaxLength="10" Width="185px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaDesde" runat="server" ValidationGroup="buscar"
                                                    ControlToValidate="txtEnvioFechaDesde" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblFechaHasta" runat="server" AssociatedControlID="txtEnvioFechaHasta" Text="Fecha Envío Hasta:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtEnvioFechaHasta" runat="server" MaxLength="10" Width="185px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaHasta" runat="server" ValidationGroup="buscar"
                                                    ControlToValidate="txtEnvioFechaHasta" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtNroSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtNroSolicitud" runat="server" MaxLength="100" Width="185px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div id="frmCola" class="modal fade">
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
                                <asp:UpdatePanel ID="updMsjCola" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMsjCola" runat="server" Style="color: Black"></asp:Label>
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
        function showfrmCola() {
            $('.modal-backdrop').remove();
            $("#frmCola").modal("show");
            return false;
        }
    </script>

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
        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }
    </script>

    <asp:UpdatePanel ID="updPnlBuscarMails" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                function endRequestHandler() {
                    inicializar_controles();
                }
            </script>

            <asp:HiddenField ID="hid_puede_modificar" runat="server" Value="false" />
            <div class="pull-right">
                <div class="control-group inline-block">
                    <asp:UpdateProgress ID="updPrgss_BuscarPagos" AssociatedUpdatePanelID="updPnlBuscarMails" runat="server" DisplayAfter="0">
                        <ProgressTemplate>
                            <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnBuscar_OnClick">
                <i class="icon-white icon-search"></i>
                <span class="text">Buscar</span>
                </asp:LinkButton>
                <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick">
                <i class="icon-refresh"></i>
                <span class="text">Limpiar</span>
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <div id="box_resultado" class="widget-box" style="display: none;">

        <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div style="margin-left: 10px; margin-right: 10px">
                    <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">
                        <div style="display: inline-block">
                            <h5>Resultado</h5>
                        </div>
                        <div style="display: inline-block">
                            (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>)
                        </div>
                    </asp:Panel>
                    <asp:HiddenField ID="hfMailID" runat="server" />
                    <%--                <div>
                    <asp:Label ID="lblEnCola" Visible="false" runat="server" >
                        Mensaje en cola.
                    </asp:Label>
                </div>--%>
                    <asp:GridView ID="grdBuscarMails" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                        DataKeyNames="Mail_ID" ItemType="SGI.Model.clsItemGrillaBuscarMails" AllowPaging="true" PageSize="30"
                        OnDataBound="grdBuscarMails_DataBound" OnRowDataBound="grdBuscarMails_RowDataBound">
                        <Columns>
                            <asp:BoundField Visible="false" DataField="Mail_ID" HeaderText="ID" />
                            <asp:BoundField DataField="Mail_Estado" HeaderText="Estado" />
                            <asp:BoundField DataField="Mail_Proceso" HeaderText="Proceso" />
                            <asp:BoundField DataField="Mail_Asunto" HeaderText="Asunto" />
                            <asp:BoundField DataField="Mail_Email" HeaderText="E-Mail" />
                            <asp:BoundField DataField="Mail_FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="Mail_FechaEnvio" HeaderText="Fecha Envio" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                            <asp:TemplateField ItemStyle-Width="15px" HeaderText="Acción" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDetalles" runat="server" ToolTip="Ver Detalle" CssClass="link-local" CommandArgument='<%#Eval("Mail_ID")%>' OnClick="lnkDetalles_Click">
                                <i class="icon-eye-open" style="transform: scale(1.1);"></i>
                                    </asp:LinkButton>
                                    <asp:Panel ID="pnlDetalle" runat="server" class="modal hide fade in" data-backdrop="static" Style="display: none; /*max-height: 700px; */max-width: 700px; width: auto">
                                        <div class="modal-header">
                                            <a class="close" data-dismiss="modal">×</a>
                                            <h3>Detalle del E-Mail</h3>
                                        </div>
                                        <div class="modal-body">
                                            <asp:Table ID="Table1" runat="server" HorizontalAlign="Center">
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
                                                <asp:TableRow>
                                                    <asp:TableCell ID="CuerpoHTML" ColumnSpan="8" Width="500px">
                                                        <iframe style="width: 100%; border-style: none" id="Message" runat="server"></iframe>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </div>
                                        <div class="modal-footer pull-right">
                                            <asp:LinkButton ID="btnReenviar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnReenviar_OnClick">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Reenviar</span>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="modal-footer">
                                            <a href="#" class="btn" data-dismiss="modal">Cancelar</a>
                                        </div>


                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                <img src="../Content/img/app/NoRecords.png" />No se encontraron registros con los filtros ingresados.
                            </asp:Panel>
                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                <%--      <div style="display: inline-table">
                            <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlResultadoBuscar" runat="server" DisplayAfter="0">
                                <ProgressTemplate>
                                    <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>--%>
                                <asp:LinkButton ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click" CssClass="btn" />
                                <asp:LinkButton ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click" CssClass="btn" />
                            </asp:Panel>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

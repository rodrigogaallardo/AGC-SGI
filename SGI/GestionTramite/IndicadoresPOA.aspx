<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndicadoresPOA.aspx.cs" Inherits="SGI.GestionTramite.IndicadoresPOA" %>

<%@ Register Src="~/GestionTramite/Controls/Charts/Chart.ascx" TagPrefix="uc1" TagName="Chart" %>


<%@ Register src="Controls/ExportacionExcel/ExcelExport.ascx" tagname="ExcelExport" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <p>
        89
    </p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />


    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>


    <script type="text/javascript">


        $(document).ready(function () {
            inicializar_controles();
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#<%: btnCargarDatos.ClientID %>").click();


        });

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function inicializar_controles() {
            inicializar_popover();
            inicializar_autocomplete();
            inicializar_fechas();
        }
        function init_Js_updpnlBuscar() {


        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function bt_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(href_collapse).css("height") == "0px") {
                    $(obj).find(".imoon-chevron-down").switchClass("imoon-chevron-down", "imoon-chevron-up", 0);
                }
                else {
                    $(obj).find(".imoon-chevron-up").switchClass("imoon-chevron-up", "imoon-chevron-down", 0);
                }
            }
        }

        function inicializar_popover() {

            $('[rel=popover]').popover({
                html: 'true',
                placement: 'right'
            })

        }
        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function validarBuscar() {
            var ret = true;
            hideSummary();

            if (ret) {

            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }

        function validarGuardar() {
            var ret = true;
            hideSummary();
            $("#Val_BotonGuardar").hide();

            if (ret) {
                ocultarBotonesGuardar();
            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }


        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }

        function ConfirmDel() {
            return confirm('¿Esta seguro que desea quitar esta condición?');
        }

        function inicializar_autocomplete() {

        }


    </script>

    <div id="box_busqueda">
        <asp:Panel ID="pnlBotonDefault" runat="server">


            <div class="accordion-group widget-box">

                <%-- titulo collapsible buscar ubicacion --%>
                <div class="accordion-heading">
                    <a id="bt_ubicacion_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_ubicacion"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                        <div class="widget-title">
                            <span class="icon" style="margin-left: 4px"><i class="imoon-library"></i></span>
                            <h5>
                                <asp:Label ID="Label1" runat="server" Text="Consulta Tramite y Circuito por Solicitud"></asp:Label></h5>
                            <span class="btn-right"><i class="imoon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible buscar por ubicacion --%>
                <div class="accordion-body collapse in" id="collapse_bt_ubicacion">
                    <div class="widget-content">
                        <%-- Busqueda1 --%>
                        <div class="form-horizontal">
                            <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnDescargarBloque1">
                                        <div class="control-group">
                                            <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtFechaDesde"
                                                Text="Fecha Desde:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                <div class="req">
                                                    <asp:RegularExpressionValidator
                                                        ID="rev_txtFechaDesde" runat="server"
                                                        ValidationGroup="buscar"
                                                        ControlToValidate="txtFechaDesde" CssClass="field-validation-error"
                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                        Display="Dynamic">
                                                    </asp:RegularExpressionValidator>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblFechaCierreDesde" runat="server" AssociatedControlID="txtFechaHasta"
                                                Text="Fecha Hasta:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                <div class="req">
                                                    <asp:RegularExpressionValidator
                                                        ID="rev_txtFechaHasta" runat="server"
                                                        ValidationGroup="buscar"
                                                        ControlToValidate="txtFechaHasta" CssClass="field-validation-error"
                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                        Display="Dynamic">
                                                    </asp:RegularExpressionValidator>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="control-group">


                                            <div class="row">
                                                <div class="span7">
                                                    <asp:Label ID="lblBusTipoTramite" runat="server" CssClass="control-label" Text="Tipo de Tramite" AssociatedControlID="ddlBusTipoTramite" />
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlBusTipoTramite" runat="server" Style="width: 40%; min-width: 100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusTipoTramite_SelectedIndexChanged" />
                                                        <asp:DropDownList ID="ddlBusTipoExpediente" runat="server" Style="width: 25%; min-width: 100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusTipoExpediente_SelectedIndexChanged" />
                                                        <asp:DropDownList ID="ddlBusSubtipoExpediente" runat="server" Style="width: 30%; min-width: 100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusSubtipoExpediente_SelectedIndexChanged" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="span7">
                                                <asp:Label ID="lblBusCircuito" runat="server" CssClass="control-label" Text="Circuito" AssociatedControlID="ddlBusCircuito" />
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlBusCircuito" runat="server" Style="width: 40%; min-width: 100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusCircuito_SelectedIndexChanged" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">

                                            <div class="pull-right">

                                                <div class="control-group inline-block">

                                                    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="updpnlBuscar"
                                                        runat="server" DisplayAfter="0">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>

                                                </div>
                                                <asp:LinkButton ID="btnDescargarBloque1" runat="server" CssClass="btn btn-success btn-large" OnClick="btnDescargarBloque1_Click" ValidationGroup="validarBuscar">
                                                    <i class="imoon-file-excel"></i>
                                                    <span class="text">Detalle</span>
                                                </asp:LinkButton>

                                                <asp:LinkButton ID="lnkMostrarGrafico" runat="server" CssClass="btn btn-success btn-large" OnClick="btnMostrarGrafico_Click" ValidationGroup="validarBuscar">
                                                    <i class="imoon-pie"></i>
                                                    <span class="text">Indicadores</span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <uc1:Chart runat="server" ID="Chart" Visible="false" />
                                            <uc2:ExcelExport ID="ExcelExport1" runat="server" />
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

        </asp:Panel>
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
                                <label class="imoon imoon-info color-blue fs64"></label>
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
    <!-- /.modal -->



    <script>


        function showfrmExportarExcel() {
            $("#frmExportarExcel").modal({
                backdrop: "static",
                show: true
            });
            return true;
        }
        function hidefrmExportarExcel() {
            $("#frmExportarExcel").modal("hide");
            return false;
        }
        function inicializar_fechas() {

            var fechaDesde = $('#<%=txtFechaDesde.ClientID%>');
            var es_readonly = $(fechaDesde).attr("readonly");

            $("#<%: txtFechaDesde.ClientID %>").datepicker({
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                    $("#Req_FechaDesde").hide();
                    $("#Val_Formato_FechaDesde").hide();
                    $("#Val_FechaDesdeMenor").hide();
                }
            });


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

            };
            var fechaCierreDesde = $('#<%=txtFechaHasta.ClientID%>');
            var es_readonlyCierre = $(fechaCierreDesde).attr("readonly");

            $("#<%: txtFechaHasta.ClientID %>").datepicker({
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                    $("#Req_FechaCierreDesde").hide();
                    $("#Val_Formato_FechaCierreDesde").hide();
                    $("#Val_FechaCierreDesdeMenor").hide();
                }
            });


            if (!($(fechaCierreDesde).is('[disabled]') || $(fechaCierreDesde).is('[readonly]'))) {
                $(fechaCierreDesde).datepicker(
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

            };
        }



    </script>
</asp:Content>

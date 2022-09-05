<%@  Title="Buscador de Pagos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarPagos.aspx.cs" Inherits="SGI.BuscarPagos" %>

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

    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_controles();
            toolTips();
            $("[id*='lnkHistorial']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });
            // Popovers rubros de la bandeja propia
            $("[id*='MainContent_grdBandeja_lnkHistorial_']").each(function () {

                var id_pnlRubros = $(this).attr("id").replace("MainContent_grdBandeja_lnkHistorial_", "MainContent_grdBandeja_pnlHistorial_");
                var objHistorial = $("#" + id_pnlHistorial).html();
                $(this).popover({ title: 'Historial', content: objHistorial, html: 'true' });

            });

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
        inicializar_fechas();
        camposAutonumericos();
        //$(".lnkCertificado").tooltip({ 'delay': { show: 500, hide: 0 } });

    }
    function inicializar_popover() {

        $('[rel=popover]').popover({
            html: 'true',
            placement: 'right'
        })

    }

    function toolTips() {
        $("[data-toggle='tooltip']").tooltip();
        return false;

    }

    function camposAutonumericos() {
        $('#<%=txtNroBU.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtId.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
    }

    function inicializar_fechas() {

        var fechaDesde = $('#<%=txtFechaDesde.ClientID%>');
        var es_readonly = $(fechaDesde).attr("readonly");
        if (!($(fechaDesde).is('[disabled]') || $(fechaDesde).is('[readonly]'))) {
            $(fechaDesde).datepicker(
                {
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

        var fechaHasta = $('#<%=txtFechaHasta.ClientID%>');
        var es_readonly = $(fechaHasta).attr("readonly");
        if (!($(fechaHasta).is('[disabled]') || $(fechaHasta).is('[readonly]'))) {
            $(fechaHasta).datepicker({
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

            });
        }

        var txtFechaPagoDesde = $('#<%=txtFechaPagoDesde.ClientID%>');
        var es_readonly = $(txtFechaPagoDesde).attr("readonly");
        if (!($(txtFechaPagoDesde).is('[disabled]') || $(txtFechaPagoDesde).is('[readonly]'))) {
            $(txtFechaPagoDesde).datepicker({
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

            });
        }
        var txtFechaPagoHasta = $('#<%=txtFechaPagoHasta.ClientID%>');
        var es_readonly = $(txtFechaPagoHasta).attr("readonly");
        if (!($(txtFechaPagoHasta).is('[disabled]') || $(txtFechaPagoHasta).is('[readonly]'))) {
            $(txtFechaPagoHasta).datepicker({
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
            });
        }

    }

    function showResultado() {
        $("#box_resultado").show("slow");
    }

    function hideResultado() {
        $("#box_resultado").hide("slow");
    }

    function inicializarControlesModal() {

        $("[id*='MainContent_grdPagos_pnlActualizarPago']").each(function (indice, elemento) {

            // dentro de cada panel busco los controles combo y textbox para inicializarlos
            var obj_ddlEstadoPagoEdit = $(elemento).find("[id*='ddlEstadoPagoEdit']");
            var obj_txtFechaPago = $(elemento).find("[id*='txtFechaPago']");

            // buscar valor del combo
            var optionSelected = $(this).find("option:selected");
            var valueSelected = optionSelected.val();
            var textSelected = optionSelected.text();

            //cuando el combo dice pagado se permite ingresar fecha
            //en cualquier otro caso se blanquea el campo fecha
            if (valueSelected != 1) {
                $(obj_txtFechaPago).prop("value", "");
                $(obj_txtFechaPago).prop("disabled", true);
            } else {
                $(obj_txtFechaPago).prop("disabled", false);
            }

            $(obj_ddlEstadoPagoEdit).change(function () {
                var itemSelected = $(this).find("option:selected");
                var ddlValor = itemSelected.val();
                var ddlTexto = itemSelected.text();
                // 0 pendiente de pago, 1 pagado, 2 vencida
                if (ddlValor != 1) {
                    $(obj_txtFechaPago).prop("value", "");
                    $(obj_txtFechaPago).prop("disabled", true);
                    //$(obj_txtFechaPago).show();
                } else {
                    $(obj_txtFechaPago).prop("disabled", false);
                }

            });

            $(obj_txtFechaPago).datepicker({
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

            });


        });

    }


    function cerrarModal(btn_confirmar) {
        var buscar_boton = $(btn_confirmar)[0].parentElement.parentElement;
        $(buscar_boton).modal('hide');
    }


    function validarActualizar(btn_confirmar) {

        var cerrarModal = false;

        // panel model
        var obj_pnlActualizarPago = $(btn_confirmar)[0].parentElement.parentElement.parentElement;

        // input dentro del model
        var ddlEstadoPagoEdit = $(obj_pnlActualizarPago).find("[id*='MainContent_grdPagos_ddlEstadoPagoEdit']");
        var txtFechaPago = $(obj_pnlActualizarPago).find("[id*='MainContent_grdPagos_txtFechaPago']");

        // buscar div con el texto de error
        var val_ddlEstadoPagoEdit = $(obj_pnlActualizarPago).find("[id*='val_ddlEstadoPagoEdit']");
        var val_txtFechaPago = $(obj_pnlActualizarPago).find("[id*='val_txtFechaPago']");


        // ocultar mensajes de error
        var error_id_pago = "";
        var error_fecha_pago = "";
        $(val_ddlEstadoPagoEdit).hide();
        $(val_txtFechaPago).hide();

        // validar combo estado
        var id_estado_pago = $(ddlEstadoPagoEdit).prop('value');
        var text_estado_pago = $(ddlEstadoPagoEdit).prop('text');

        if (id_estado_pago == "-1") {
            error_id_pago = "Debe seleccionar estado de pago.";
        }

        if (error_id_pago != "") {
            $(val_ddlEstadoPagoEdit).html(error_id_pago);
            $(val_ddlEstadoPagoEdit).show();
        }

        //validar fecha de pago
        var fecha_pago = $(txtFechaPago).prop('value');

        if (id_estado_pago == 1 && fecha_pago == "") {
            error_fecha_pago = "Debe indicar la fecha de pago para cambiar a estado pagado.";
        }

        if (id_estado_pago != 1 && fecha_pago != "") {
            error_fecha_pago = "No corresponde ingresar fecha de pago para cambiar a estado " + text_estado_pago + ".";
        }

        if (id_estado_pago == 1 && fecha_pago != "") {
            //verificar formato de fecha
            var dateDDMMYYYRegex = /(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$/;

            if (!fecha_pago.match(dateDDMMYYYRegex)) {
                error_fecha_pago = "Fecha inv&aacute;lida, ingrese con formato dd/mm/aaaa";
            }

        }

        if (error_fecha_pago != "") {
            $(val_txtFechaPago).html(error_fecha_pago);
            $(val_txtFechaPago).show();
        }

        if (error_id_pago == "" && error_fecha_pago == "") {
            cerrarModal = true;
        }

        if (cerrarModal) { // cerrar model
            var buscar_boton = $(btn_confirmar)[0].parentElement.parentElement;
            $(buscar_boton).modal('hide');
            return true;  // validaciones en js con error
        }
        else {
            return false; // validaciones en js ok
        }

    }


    </script>


    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">

        <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Buscar Pagos</h5>
            </div>

            <div class="widget-content">
                <asp:UpdatePanel ID="updPnlFiltroBuscar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="form-horizontal">
                            <fieldset>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <asp:Label ID="lblNroBU" runat="server" AssociatedControlID="txtNroBU"
                                                Text="Número de Boleta única" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtNroBU" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <asp:Label ID="lblId" runat="server" AssociatedControlID="txtId"
                                                Text="Nro Identificación" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtId" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtFechaDesde"
                                                Text="Fecha Creación Desde:" class="control-label"></asp:Label>
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
                                    </div>

                                    <div class="span6">
                                        <div class="control-group">
                                            <asp:Label ID="lblFechaHasta" runat="server" AssociatedControlID="txtFechaHasta"
                                                Text="Fecha Creación Hasta:" class="control-label"></asp:Label>
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
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="lblEstadoPago" runat="server" AssociatedControlID="ddlEstadoPago"
                                        Text="Estado" class="control-label"></asp:Label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlEstadoPago" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlEstadoPago_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <asp:Label ID="lblFechaPagoDesde" runat="server" AssociatedControlID="txtFechaPagoDesde"
                                                Text="Fecha Pago Desde:" class="control-label" Visible="false"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFechaPagoDesde" runat="server" MaxLength="10" Width="80px" Visible="false"></asp:TextBox>
                                                <div class="req">
                                                    <asp:RegularExpressionValidator
                                                        ID="RegularExpressionValidator1" runat="server"
                                                        ValidationGroup="buscar"
                                                        ControlToValidate="txtFechaPagoDesde" CssClass="field-validation-error"
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
                                            <asp:Label ID="lblFechaPagoHasta" runat="server" AssociatedControlID="txtFechaPagoHasta"
                                                Text="Fecha Pago Hasta:" class="control-label" Visible="false"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFechaPagoHasta" runat="server" MaxLength="10" Width="80px" Visible="false"></asp:TextBox>
                                                <div class="req">
                                                    <asp:RegularExpressionValidator
                                                        ID="RegularExpressionValidator2" runat="server"
                                                        ValidationGroup="buscar"
                                                        ControlToValidate="txtFechaPagoHasta" CssClass="field-validation-error"
                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                        Display="Dynamic">
                                                    </asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <br />

        <asp:UpdatePanel ID="updPnlBuscarPagos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:HiddenField ID="hid_puede_modificar" runat="server" Value="false" />

                <div class="pull-right">

                    <div class="control-group inline-block">

                        <asp:UpdateProgress ID="updPrgss_BuscarPagos" AssociatedUpdatePanelID="updPnlBuscarPagos"
                            runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                    </div>
                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse"
                        ValidationGroup="buscar" OnClick="btnBuscar_OnClick">
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

    </asp:Panel>



    <div id="box_resultado" class="widget-box" style="display: none;">

        <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div style="margin-left: 10px; margin-right: 10px">
                    <script type="text/javascript">
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                        function endRequestHandler() {
                            inicializarControlesModal();
                        }
                    </script>

                    <script type="text/javascript">
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                        function endRequestHandler() {
                            inicializar_popover();
                        }
                    </script>
                    <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">

                        <div style="display: inline-block">
                            <h5>Lista de Boletas</h5>
                        </div>
                        <div style="display: inline-block">
                            (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>
                            )
                        </div>

                    </asp:Panel>

                    <asp:GridView ID="grdPagos" runat="server" AutoGenerateColumns="false"
                        GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                        DataKeyNames="id_pago_BU,id_pago" ItemType="SGI.Model.clsItemPagos"
                        AllowPaging="true" PageSize="30" OnPageIndexChanging="grdPagos_PageIndexChanging"
                        OnDataBound="grdPagos_DataBound" OnRowDataBound="grdPagos_RowDataBound">

                        <Columns>

                            <asp:BoundField DataField="Numero_BU" HeaderText="Nro. Boleta" ItemStyle-Width="60px" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="CreateDate" HeaderText="Fecha de creación" DataFormatString="{0:d}" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="loginModif" HeaderText="Usuario" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ApellidoyNombre" HeaderText="Apellido y Nombre / Razón Social" />
                            <asp:BoundField DataField="Monto_BU" HeaderText="Monto" DataFormatString="{0:$ #,##0.00 }" ItemStyle-Width="65px" ItemStyle-CssClass="align-right" />
                            <asp:BoundField DataField="estado_desc" HeaderText="Estado" ItemStyle-Width="120px" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="FechaPago_BU" HeaderText="Fecha Pago" DataFormatString="{0:d}" ItemStyle-Width="70px" ItemStyle-CssClass="align-center" /> 
                            <asp:TemplateField HeaderText="Acción" ItemStyle-Width="80px" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="updPnlActualizar" runat="server">
                                        <ContentTemplate>
                                            <asp:HyperLink ID="lnkCertificado" runat="server"
                                                ToolTip="Imprimir boleta"
                                                data-toggle="tooltip"
                                                class="lnkCertificado"
                                                NavigateUrl='<%# string.Format("~/ImprimirBoleta/{0}/{1}", Eval("id_pago"),Eval("Numero_BU")) %>'>
                                <i  class="icon-file" style="transform: scale(1.3);margin-right:5px;margin-left:5px"></i> 
                                            </asp:HyperLink>

                                            <%--<asp:LinkButton ID="btnEnviarMail" runat="server" title="Reenviar mail con datos de la cuenta de usuario" data-toggle="tooltip"
                                                CssClass="link-local" CommandArgument='<%# Eval("userid") %>' OnClick="btnEnviarMail_Click">
                                                    <span class="icon azulmarino" > <i class="icon-envelope"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminarUsuario" runat="server" title="Eliminar" data-toggle="tooltip"
                                                CssClass="link-local" data-userid='<%# Eval("userid") %>' OnClientClick="return showfrmConfirmarEliminar(this);">
                                                     <span class="icon azulmarino" ><i class="icon-remove"></i></span>
                                            </asp:LinkButton>--%>

                                            <asp:HyperLink ID="lnkActualizar" runat="server"
                                                ToolTip="Actualizar" CssClass="link-local"
                                                data-toggle="modal">
                                <i class="icon-refresh" style="transform: scale(1.3);margin-right:5px;margin-left:5px"></i>
                                            </asp:HyperLink>

                                            <asp:Panel ID="pnlActualizarPago" runat="server"
                                                class="modal hide fade in" data-backdrop="static"
                                                Style="display: none; max-height: 600px; max-width: 700px">

                                                <div class="modal-header">
                                                    <a class="close" data-dismiss="modal">×</a>
                                                    <h3>Actualizaci&oacute;n de pago</h3>
                                                </div>

                                                <div class="modal-body" style="max-height: 250px">

                                                    <div class="row-fluid" style="margin-top: 0">
                                                        <div class="row-fluid" style="margin-top: 0">
                                                            <div class="span5">N&uacute;mero Boleta:</div>
                                                            <div class="span7"><%#Eval("Numero_BU")%>  </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span5">Fecha Creaci&oacute;n:</div>
                                                            <div class="span7"><%#Eval("CreateDate")%>  </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span5">Apellido y Nombre / R&aacute;zon Social:</div>
                                                            <div class="span7"><%#Eval("ApellidoyNombre")%>  </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span5">Estado:</div>
                                                            <div class="span7">
                                                                <asp:DropDownList ID="ddlEstadoPagoEdit" runat="server" Width="170px">
                                                                </asp:DropDownList>
                                                                <div id="val_ddlEstadoPagoEdit" class="req field-validation-error" style="display: none">
                                                                    Debe seleccionar estado de pago.
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="row-fluid">
                                                            <div class="span5">Fecha de Pago:</div>
                                                            <div class="span7">
                                                                <asp:TextBox ID="txtFechaPago" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div id="val_txtFechaPago" class="req field-validation-error" style="display: none">
                                                                    Ingrese la fecha de pago.
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                </div>

                                                <div class="modal-footer">
                                                    <a href="#" class="btn" data-dismiss="modal">Cancelar</a>
                                                    <asp:Button ID="btnGuardarFechaPago" runat="server"
                                                        ValidationGroup="val_actualizar_"
                                                        Text="Guardar" CssClass="btn btn-success"
                                                        OnClientClick="return validarActualizar(this);"
                                                        CommandArgument='<%# Eval("id_pago_BU") %>'
                                                        OnCommand="btnActualizar_Command" />


                                                </div>

                                            </asp:Panel>


                                            <asp:LinkButton ID="lnkHistorial" runat="server"
                                                ToolTip="Historial" CssClass="link-local"
                                                data-toggle="modal" CommandArgument='<%#Eval("id_pago_BU")%>' OnClientClick='redirect(this)'>
                                    <i class="icon-time" style="transform: scale(1.3);margin-right:5px;margin-left:5px"></i>
                                
                                            </asp:LinkButton>

                                            <%--Popover con la lista de Rubros--%>
                                            <asp:Panel ID="pnlHistorial" runat="server" class="modal hide fade in" data-backdrop="static" Style="display: none; max-height: 600px; max-width: 700px">
                                                <div class="modal-header">
                                                    <a class="close" data-dismiss="modal">×</a>
                                                    <h3>Historial de estados</h3>
                                                </div>

                                                <div class="modal-body" style="max-height: 250px">

                                                    <asp:Table ID="tblHistorial" runat="server" HorizontalAlign="Center">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Estado anterior</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Estado nuevo</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Realizado por</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                    </asp:Table>
                                                </div>

                                                <div class="modal-footer">
                                                    <a href="#" class="btn" data-dismiss="modal">Cancelar</a>
                                                </div>
                                            </asp:Panel>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>


                            <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                <img src="../Content/img/app/NoRecords.png" />

                                No se encontraron boletas con los filtros ingresados.
                       
                            </asp:Panel>
                        </EmptyDataTemplate>

                        <PagerTemplate>

                            <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                <div style="display: inline-table">
                                    <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlResultadoBuscar" runat="server"
                                        DisplayAfter="0">
                                        <ProgressTemplate>
                                            <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>

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
                    <br />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

<%@  Title="Consulta del trámite" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaTramite.aspx.cs" Inherits="SGI.GestionTramite.ConsultaTramite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/gritter") %>

    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_controles();
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

            //debugger;
            //inicializar tootip del popOver
            inicializar_popover();
            inicializar_fechas();
            camposAutonumericos();
            inicializar_autocomplete();
            $("#<%: ddlTipoExpediente.ClientID %>").select2({ allowClear: true });
                $("#<%: ddlSubTipoTramite.ClientID %>").select2({ allowClear: true });
                $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });

                /// Inicializar select2 de busqueda
                var tags_selecionados = "";
                if ($("#<%: hid_estados_selected.ClientID %>").val().length > 0) {
                    tags_selecionados = $("#<%: hid_estados_selected.ClientID %>").val().split(",");
                }

                $("#<%: ddlEstado.ClientID %>").select2({
                    tags: true,
                    tokenSeparators: [","],
                    placeholder: "Ingrese las etiquetas de búsqueda",
                    language: "es",
                    data: tags_selecionados
                });
                $("#<%: ddlEstado.ClientID %>").val(tags_selecionados);
                $("#<%: ddlEstado.ClientID %>").trigger("change.select2");

                $("#<%: ddlEstado.ClientID %>").on("change", function () {
                    $("#<%: hid_estados_selected.ClientID %>").val($("#<%: ddlEstado.ClientID %>").val());
            });

                var tags_selecionados = "";
                if ($("#<%: hid_grupocircuito_selected.ClientID %>").val().length > 0) {
                    tags_selecionados = $("#<%: hid_grupocircuito_selected.ClientID %>").val().split(",");
                }

                $("#<%: ddlGrupoCircuito.ClientID %>").select2({
                    tags: true,
                    tokenSeparators: [","],
                    placeholder: "Ingrese las etiquetas de búsqueda",
                    language: "es",
                    data: tags_selecionados
                });
                $("#<%: ddlGrupoCircuito.ClientID %>").val(tags_selecionados);
                $("#<%: ddlGrupoCircuito.ClientID %>").trigger("change.select2");

                $("#<%: ddlGrupoCircuito.ClientID %>").on("change", function () {
                    $("#<%: hid_grupocircuito_selected.ClientID %>").val($("#<%: ddlGrupoCircuito.ClientID %>").val());
            });


                var tags_selecionados = "";
                if ($("#<%: hid_tipotramite_selected.ClientID %>").val().length > 0) {
                    tags_selecionados = $("#<%: hid_tipotramite_selected.ClientID %>").val().split(",");
                }

                $("#<%: ddlTipoTramite.ClientID %>").select2({
                    tags: true,
                    tokenSeparators: [","],
                    placeholder: "Ingrese las etiquetas de búsqueda",
                    language: "es",
                    data: tags_selecionados
                });
                $("#<%: ddlTipoTramite.ClientID %>").val(tags_selecionados);
                $("#<%: ddlTipoTramite.ClientID %>").trigger("change.select2");

                $("#<%: ddlTipoTramite.ClientID %>").on("change", function () {
                    $("#<%: hid_tipotramite_selected.ClientID %>").val($("#<%: ddlTipoTramite.ClientID %>").val());
            });
        }

        function camposAutonumericos() {
            $('#<%=txtNroSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtUbiSeccion.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtUbiNroPuertaDesde.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '999999' });
        $('#<%=txtUbiNroPuertaHasta.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '999999' });
        vSeparadorDecimal = $("#<%: hid_DecimalSeparator.ClientID %>").attr("value");
        eval("$('#<%: txtSuperficieDesde.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtSuperficieHasta.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");

        }

        function inicializar_fechas() {
            $("#<%: txtFechaInicioDesde.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaInicioDesde").hide();
            }
        });
        if (!($('#<%=txtFechaInicioDesde.ClientID%>').is('[disabled]') || $('#<%=txtFechaInicioDesde.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaInicioDesde.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        $("#<%: txtFechaInicioHasta.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaInicioHasta").hide();
            }
        });
        if (!($('#<%=txtFechaInicioHasta.ClientID%>').is('[disabled]') || $('#<%=txtFechaInicioHasta.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaInicioHasta.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        $("#<%: txtFechaIngresoDesde.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaIngresoeDesde").hide();
            }
        });
        if (!($('#<%=txtFechaIngresoDesde.ClientID%>').is('[disabled]') || $('#<%=txtFechaIngresoDesde.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaIngresoDesde.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        $("#<%: txtFechaIngresoHasta.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaIngresoHasta").hide();
            }
        });
        if (!($('#<%=txtFechaIngresoHasta.ClientID%>').is('[disabled]') || $('#<%=txtFechaIngresoHasta.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaIngresoHasta.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        $("#<%: txtFechaLibradoUsoDesde.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaLibradoUsoDesde").hide();
            }
        });
        if (!($('#<%=txtFechaLibradoUsoDesde.ClientID%>').is('[disabled]') || $('#<%=txtFechaLibradoUsoDesde.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaLibradoUsoDesde.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        $("#<%: txtFechaLibradoUsoHasta.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaLibradoUsoHasta").hide();
            }
        });
        if (!($('#<%=txtFechaLibradoUsoHasta.ClientID%>').is('[disabled]') || $('#<%=txtFechaLibradoUsoHasta.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaLibradoUsoHasta.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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
        $("#<%: txtFechaHabilitacionDesde.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaHabilitacionDesde").hide();
            }
        });
        if (!($('#<%=txtFechaHabilitacionDesde.ClientID%>').is('[disabled]') || $('#<%=txtFechaHabilitacionDesde.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaHabilitacionDesde.ClientID%>').datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        $("#<%: txtFechaHabilitacionHasta.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                $("#rev_txtFechaHabilitacionHasta").hide();
            }
        });
        if (!($('#<%=txtFechaHabilitacionHasta.ClientID%>').is('[disabled]') || $('#<%=txtFechaHabilitacionHasta.ClientID%>').is('[readonly]'))) {
            $('#<%=txtFechaHabilitacionHasta.ClientID%>').datepicker(
                    {
                        maxDate: "0",
                        closeText: 'Cerrar',
                        prevText: '&#x3c;Ant',
                        nextText: 'Sig&#x3e;',
                        currentText: 'Hoy',
                        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
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

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
        }


        function inicializar_autocomplete() {
            $("#<%: ddlCalles.ClientID %>").select2({
                minimumInputLength: 3
            });
        }


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

        function switchear_buscar_ubicacion(btn) {

            if (btn == 1) {
                $("#buscar_ubi_por_dom").show();
                $("#buscar_ubi_por_smp").hide();

                $("#btnBuscarPorDom").addClass("active");
                $("#btnBuscarPorSMP").removeClass("active");
            }
            else if (btn == 2) {
                $("#buscar_ubi_por_dom").hide();
                $("#buscar_ubi_por_smp").show();
                $("#btnBuscarPorDom").removeClass("active");
                $("#btnBuscarPorSMP").addClass("active");
            }
        }


        function inicializar_popover() {

            $("[id*='lnkUbicaciones']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

            $("[id*='MainContent_grdTramites_lnkUbicaciones_']").each(function () {
                //para cada fila de la grilla, se busca el link y se lo vincula al panel de la misma fila
                //para que con el clikc del link habra el popOver de un html
                var id_pnlTareas = $(this).attr("id").replace("MainContent_grdTramites_lnkUbicaciones_", "MainContent_grdTramites_pnlUbicaciones_");
                var objTareas = $("#" + id_pnlTareas).html();
                $(this).popover({
                    title: 'Ubicaciones',
                    content: objTareas,
                    html: 'true'
                });
            });

            $("[id*='lnkRubros']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

            $("[id*='MainContent_grdTramites_lnkRubros_']").each(function () {
                //para cada fila de la grilla, se busca el link y se lo vincula al panel de la misma fila
                //para que con el clikc del link habra el popOver de un html
                var id_pnlTareas = $(this).attr("id").replace("MainContent_grdTramites_lnkRubros_", "MainContent_grdTramites_pnlRubros_");
                var objTareas = $("#" + id_pnlTareas).html();
                $(this).popover({
                    title: 'Rubros',
                    content: objTareas,
                    html: 'true'
                });
            });
        }

        function popOver(obj) {
            if ($(obj).attr("data-visible") == "true") {
                $(obj).attr("data-visible", "false");
            }
            else {
                $("[data-visible='true']").popover("toggle");
                $("[data-visible='true']").attr("data-visible", "false");
                $(obj).attr("data-visible", "true");
            }
            return false;
        }

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

        /////////////////////Rubros
        function showfrmAgregarRubros_Rubros() {

            $("#<%: txtBuscar.ClientID %>").val("");
        $("#<%: pnlResultadoBusquedaRubros.ClientID %>").hide();
        $("#<%: pnlBuscarRubros.ClientID %>").show();
        $("#<%: pnlResultadoBusquedaRubros.ClientID %>").hide();
        $("#<%: pnlBotonesAgregarRubros.ClientID %>").hide();
        $("#<%: pnlBotonesBuscarRubros.ClientID %>").show();

        $("#<%: BotonesBuscarRubros.ClientID %>").show();


        $("#frmAgregarRubros_Rubros").on("shown.bs.modal", function (e) {
            $("#<%: txtBuscar.ClientID %>").focus();
        });

            $("#frmAgregarRubros_Rubros").modal({
                "show": true,
                "backdrop": "static"
            });

            return false;
        }

        function validarBuscar_Rubros() {
            var ret = true;
            $("#Req_txtBuscar_Rubros").hide();

            if ($("#<%: txtBuscar.ClientID %>").val().length < 3) {
                $("#Req_txtBuscar_Rubros").css("display", "inline-block");
                ret = false;
            }

            if (ret) {
                ocultarBotonesBusquedaRubros_Rubros();
                $("#<%: pnlGrupoAgregarRubros.ClientID %>").css("display", "block");
            }
            return ret;
        }

        function hidefrmAgregarRubros_Rubros() {

            $("#frmAgregarRubros_Rubros").modal("hide");
            return false;
        }

        function ocultarBotonesBusquedaRubros_Rubros() {
            $("#<%: BotonesBuscarRubros.ClientID %>").hide();
            return false;
        }

    </script>

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">

        <%-- filtros de busqueda--%>
        <div class="">

            <%--buscar tramite --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />

                <%-- titulo collapsible buscar por tramite --%>
                <div class="accordion-heading">
                    <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                        <%--            <asp:HiddenField ID="hid_bt_tramite_collapse" runat="server" Value="true"/>
                    <asp:HiddenField ID="hid_bt_tramite_visible" runat="server" Value="false"/>--%>

                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_tramite_tituloControl" runat="server" Text="Trámites"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible buscar por tramite --%>
                <div class="accordion-body collapse in" id="collapse_bt_tramite">
                    <div class="widget-content">
                        <asp:UpdatePanel ID="updPnlFiltroBuscar_tramite" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblNroSolicitud" runat="server" AssociatedControlID="txtNroSolicitud"
                                                    Text="Solicitud:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtNroSolicitud" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblFechaInicioDesde" runat="server" AssociatedControlID="txtFechaInicioDesde"
                                                    Text="Fecha de Inicio Desde:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaInicioDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaInicioDesde" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaInicioDesde" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblFechaInicioHasta" runat="server" AssociatedControlID="txtFechaInicioHasta"
                                                    Text="Fecha de Inicio Hasta:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaInicioHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaInicioHasta" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaInicioHasta" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblFechaIngresoDesde" runat="server" AssociatedControlID="txtFechaIngresoDesde"
                                                    Text="Fecha de Ingreso Desde:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaIngresoDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaIngresoeDesde" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaIngresoDesde" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblFechaIngresoHasta" runat="server" AssociatedControlID="txtFechaIngresoHasta"
                                                    Text="Fecha de Ingreso Hasta:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaIngresoHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaIngresoHasta" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaIngresoHasta" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblFechaLibradoUsoDesde" runat="server" AssociatedControlID="txtFechaLibradoUsoDesde"
                                                    Text="Fecha de Librado al Uso Desde:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaLibradoUsoDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaLibradoUsoDesde" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaLibradoUsoDesde" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblFechaLibradoUsoHasta" runat="server" AssociatedControlID="txtFechaLibradoUsoHasta"
                                                    Text="Fecha de Librado al Uso Hasta:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaLibradoUsoHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaLibradoUsoHasta" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaLibradoUsoHasta" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblFechaHabilitacionDesde" runat="server" AssociatedControlID="txtFechaHabilitacionDesde"
                                                    Text="Fecha de Habilitación Desde:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaHabilitacionDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaHabilitacionDesde" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaHabilitacionDesde" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblFechaHabilitacionHasta" runat="server" AssociatedControlID="txtFechaHabilitacionHasta"
                                                    Text="Fecha de Habilitación Hasta:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtFechaHabilitacionHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                    <div class="req">
                                                        <asp:RegularExpressionValidator
                                                            ID="rev_txtFechaHabilitacionHasta" runat="server"
                                                            ValidationGroup="buscar"
                                                            ControlToValidate="txtFechaHabilitacionDesde" CssClass="field-validation-error"
                                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                            Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblTipoTramite" runat="server" AssociatedControlID="ddlTipoTramite"
                                                    Text="Tipo Trámite:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlTipoTramite" runat="server" multiple="true" Width="300px"></asp:DropDownList>
                                                    <asp:HiddenField ID="hid_tipotramite_selected" runat="server"></asp:HiddenField>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblGrupoCircuito" runat="server" AssociatedControlID="ddlGrupoCircuito"
                                                    Text="Grupo Circuito:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlGrupoCircuito" runat="server" multiple="true" Width="300px"></asp:DropDownList>
                                                    <asp:HiddenField ID="hid_grupocircuito_selected" runat="server"></asp:HiddenField>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblTipoExpediente" runat="server" AssociatedControlID="ddlTipoExpediente"
                                                    Text="Tipo Expediente:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlTipoExpediente" runat="server" Width="200px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlTipoExpediente_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblSubTipoTramite" runat="server" AssociatedControlID="ddlSubTipoTramite"
                                                    Text="Subtipo Trámite:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlSubTipoTramite" runat="server" Width="200px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlSubTipoTramite_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado"
                                                    Text="Estado del Trámite:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlEstado" runat="server" Width="300px" multiple="true"></asp:DropDownList>
                                                    <asp:HiddenField ID="hid_estados_selected" runat="server"></asp:HiddenField>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="lblTarea" runat="server" AssociatedControlID="ddlTarea"
                                                    Text="Tarea abierta:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlTarea" runat="server" Width="300px"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="Label3" runat="server" AssociatedControlID="txtSuperficieDesde"
                                                    Text="Superficie Desde:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtSuperficieDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <asp:Label ID="Label4" runat="server" AssociatedControlID="txtSuperficieHasta"
                                                    Text="Superficie Hasta:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtSuperficieHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="span5">
                                                <asp:Label ID="Label7" runat="server" AssociatedControlID="txtSuperficieDesde"
                                                    Text="Plano de Incendio:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlPlanoIncendio" AutoPostBack="true" OnSelectedIndexChanged="ddlPlanoIncendio_SelectedIndexChanged" runat="server" Width="200px">
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                            <div class="span5">
                                            </div>

                                        </div>

                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>

            <%--buscar ubicacion --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- titulo collapsible buscar ubicacion --%>
                <div class="accordion-heading">
                    <a id="bt_ubicacion_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_ubicacion"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                        <%--                <asp:HiddenField ID="hid_bt_ubicacion_collapse" runat="server" Value="false"/>
                    <asp:HiddenField ID="hid_bt_ubicacion_visible" runat="server" Value="false"/>--%>

                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="Label1" runat="server" Text="Ubicación"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-down"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible buscar por ubicacion --%>
                <div class="accordion-body collapse" id="collapse_bt_ubicacion">
                    <div class="widget-content">

                        <%--tipos de busquedad por ubicacion--%>
                        <div class="widget-content">

                            <div class="btn-group" data-toggle="buttons-radio" style="display: table-cell;">
                                <button id="btnBuscarPorDom" type="button" class="btn active" onclick="switchear_buscar_ubicacion(1);">Por Domicilio</button>
                                <button id="btnBuscarPorSMP" type="button" class="btn" onclick="switchear_buscar_ubicacion(2);">Por SMP</button>
                            </div>

                        </div>

                        <%--buscar por nombre de calle--%>
                        <div id="buscar_ubi_por_dom" class="widget-content">

                            <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_dom" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-horizontal">

                                        <fieldset>

                                            <div class="control-group">
                                                <label for="ddlZona" class="control-label">Zona:</label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlZona" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label for="ddlBarrio" class="control-label">Barrio:</label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlBarrio" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label for="ddlComuna" class="control-label">Comuna:</label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlComuna" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:Label ID="lblUbiCalle" runat="server" AssociatedControlID="ddlCalles"
                                                    CssClass="control-label">Búsqueda de Calle:</asp:Label>
                                                <div class="controls">
                                                    <div class="clearfix">
                                                        <div class="pull-left">
                                                            <asp:DropDownList ID="ddlCalles" runat="server" Width="500px"></asp:DropDownList>
                                                            <span style="font-size: 8pt">Debe ingresar un mínimo de 3 letras y el sistema le mostrará
                                                                las calles posibles.</span>
                                                            <asp:RequiredFieldValidator ID="ReqCalle" runat="server" ErrorMessage="Debe seleccionar una de las calles de la lista desplegable."
                                                                Display="Dynamic" ControlToValidate="ddlCalles" ValidationGroup="Buscar2"
                                                                CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <asp:Label ID="lblUbiNroPuerta" runat="server" AssociatedControlID="txtUbiNroPuertaDesde"
                                                    CssClass="control-label">Nro. Puerta Desde:</asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtUbiNroPuertaDesde" runat="server" MaxLength="10" Width="50px"
                                                        CssClass="input-xlarge"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:Label ID="Label5" runat="server" AssociatedControlID="txtUbiNroPuertaHasta"
                                                    CssClass="control-label">Nro. Puerta Hasta:</asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtUbiNroPuertaHasta" runat="server" MaxLength="10" Width="50px"
                                                        CssClass="input-xlarge"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <asp:Label ID="Label6" runat="server" AssociatedControlID="ddlVereda"
                                                    CssClass="control-label">Vereda:</asp:Label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlVereda" runat="server" AutoPostBack="true" Width="350px">
                                                        <asp:ListItem Value="0">Todos</asp:ListItem>
                                                        <asp:ListItem Value="1">Par</asp:ListItem>
                                                        <asp:ListItem Value="2">Impar</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>

                        <%--buscar por seccion, manzana, parcela--%>
                        <div id="buscar_ubi_por_smp" class="widget-content" style="display: none">

                            <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_smp" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-horizontal">

                                        <fieldset>

                                            <div class="control-group">

                                                <asp:Label ID="lblUbiSeccion" runat="server" AssociatedControlID="txtUbiManzana"
                                                    Text="Sección:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                <div class="control-label" style="margin-left: -75px; margin-top: -20px">
                                                    <asp:TextBox ID="txtUbiSeccion" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                </div>


                                                <asp:Label ID="lblUbiManzana" runat="server" AssociatedControlID="txtUbiManzana"
                                                    Text="Manzana:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                    <asp:TextBox ID="txtUbiManzana" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                </div>


                                                <asp:Label ID="lblUbiParcela" runat="server" AssociatedControlID="txtUbiParcela"
                                                    Text="Parcela:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                    <asp:TextBox ID="txtUbiParcela" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </fieldset>



                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>

                    </div>
                </div>

            </div>

            <%--buscar rubros --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- titulo collapsible buscar rubros --%>
                <div class="accordion-heading">
                    <a id="bt_rubro_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_rubro"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="Label2" runat="server" Text="Rubros o Actividades"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-down"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible buscar por rubros --%>
                <div class="accordion-body collapse" id="collapse_bt_rubro">
                    <div class="widget-content">

                        <div class="row mbottom10">
                            <div class="col-sm-12 text-right pright15">
                                <button id="btnAgregarRubros_Rubros" class="btn btn-success pbottom5" onclick="return showfrmAgregarRubros_Rubros();" data-group="controles-accion">
                                    <i class="imoon imoon-plus"></i>
                                    <span class="text">Agregar Rubro</span>
                                </button>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="updRubros" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <asp:GridView ID="grdRubrosIngresados" runat="server" AutoGenerateColumns="false"
                                    AllowPaging="false" Style="border: none;" CssClass="table table-bordered mtop5"
                                    GridLines="None" Width="100% "
                                    DataKeyNames="id_rubro,cod_rubro,nom_rubro,tipo_actividad">
                                    <HeaderStyle CssClass="grid-header" />
                                    <RowStyle CssClass="grid-row" />
                                    <AlternatingRowStyle BackColor="#efefef" />
                                    <Columns>
                                        <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Descripción">
                                            <ItemTemplate>
                                                <%# Eval("nom_rubro") %> <%# (Eval("id_subrubro").ToString().Length > 0 
                                                                                ?  "<div style='color:#0e7939; font-size:8pt'>" + Eval("id_subrubro") + " - " + Eval("nom_subrubro") + "</div>"
                                                                                : "")%>
                                            </ItemTemplate>

                                        </asp:TemplateField>


                                        <asp:BoundField DataField="tipo_actividad" HeaderText="Actividad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px" />
                                        <%--<asp:BoundField DataField="TipoTamite" HeaderText="Tipo Trámite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />--%>
                                        <%--<asp:BoundField DataField="EsAnterior" HeaderText="Anterior" Visible="false" />--%>
                                        <asp:TemplateField ItemStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEliminarRubro" runat="server" CommandArgument='<%# Eval("id_rubro") %>' CssClass="link-local"
                                                    OnClick="btnEliminarRubro_Click" data-group="controles-accion">
                                                    <i class="imoon imoon-close"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="mtop10">
                                            <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                            <span class="mleft10">No se encontraron registros.</span>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <asp:HiddenField ID="hid_id_caarubro_eliminar" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>

            </div>

        </div>
        <%--Modal form Agregar Rubros--%>
        <div id="frmAgregarRubros_Rubros" class="modal fade" style="margin-left: -25%; width: 50%; display: none; max-height: 90%; overflow: auto">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Agregar Rubros</h4>
                    </div>
                    <div class="modal-body pbottom20">
                        <asp:UpdatePanel ID="updBuscarRubros" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlBuscarRubros" runat="server" CssClass="form-horizontal" DefaultButton="btnBuscar">

                                    <div class="form-group">
                                        <h3 class="pleft20 col-sm-12">B&uacute;squeda de Rubros</h3>
                                    </div>
                                    <div class="row-fluid">
                                        <label class="span3" style="margin-top: -1px">Ingrese el código o parte de la descripción del rubro a buscar:</label>
                                        <div class="span9">
                                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-offset-3 col-sm-9 ptop5">
                                            <div id="Req_txtBuscar_Rubros" class="alert alert-danger mbottom0" style="display: none">
                                                Debe ingresar al menos 3 caracteres para iniciar la b&uacute;squeda.
                                            </div>
                                        </div>
                                    </div>

                                    <hr class="mbottom0 mtop0" />


                                    <asp:UpdatePanel ID="updBotonesBuscarRubros" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <asp:Panel ID="pnlBotonesBuscarRubros" runat="server" CssClass="form-inline text-right">
                                                <div class="form-group">
                                                    <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="updBotonesBuscarRubros"
                                                        runat="server" DisplayAfter="200">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                            Buscando...
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>

                                                <asp:Panel ID="BotonesBuscarRubros" runat="server" CssClas="form-group">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" Style="color: white" OnClick="btnBuscar_Click" OnClientClick="return validarBuscar_Rubros();">
                                                        <i class="imoon imoon-search"></i>
                                                        <span class="text">Buscar</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-default" data-dismiss="modal">
                                                        <i class="imoon imoon-close"></i>
                                                        <span class="text">Cerrar</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </asp:Panel>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </asp:Panel>

                                <asp:Panel ID="pnlResultadoBusquedaRubros" runat="server" CssClass="form-horizontal" Style="display: none">

                                    <div style="max-height: 500px; overflow-y: auto">

                                        <asp:GridView ID="grdRubros" runat="server" AutoGenerateColumns="false" DataKeyNames="id_rubro,cod_rubro,nom_rubro,tipo_actividad,id_subrubro,nom_subrubro"
                                            AllowPaging="false" Style="border: none;" CssClass="table table-bordered mtop5"
                                            GridLines="None" Width="100% ">
                                            <HeaderStyle CssClass="grid-header" />
                                            <RowStyle CssClass="grid-row" />
                                            <AlternatingRowStyle BackColor="#efefef" />
                                            <Columns>
                                                <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />

                                                <asp:TemplateField HeaderText="Descripción">
                                                    <ItemTemplate>
                                                        <%# Eval("nom_rubro") %> <%#  (Eval("id_subrubro").ToString().Length > 0 
                                                                                         ?  "<div style='color:#0e7939; font-size:8pt'>" + Eval("id_subrubro") + " - " + Eval("nom_subrubro") + "</div>"
                                                                                         : "")
                                                        %>
                                                    </ItemTemplate>

                                                </asp:TemplateField>

                                                <asp:BoundField DataField="tipo_actividad" HeaderText="Actividad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px" />
                                                <asp:TemplateField ItemStyle-Width="55px" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRubroElegido" runat="server" Enabled="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="mtop10">
                                                    <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                                    <span class="mleft10">No se encontraron registros.</span>
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>

                                    <asp:UpdatePanel ID="updBotonesAgregarRubros" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <asp:Panel ID="pnlGrupoAgregarRubros" runat="server" CssClass="row ptop10 pleft10 pright10" Style="display: none">
                                                <div class="span2">

                                                    <asp:LinkButton ID="btnnuevaBusqueda" runat="server" CssClass="btn btn-default" OnClick="btnnuevaBusqueda_Click">
                                                        <i class="imoon imoon-search"></i>
                                                        <span class="text">Nueva B&uacute;squeda</span>
                                                    </asp:LinkButton>
                                                </div>

                                                <div class="span7 pleft20">

                                                    <asp:UpdatePanel ID="updValidadorAgregarRubros" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="ValidadorAgregarRubros" runat="server" CssClass="alert alert-danger mbottom0" Style="display: none">
                                                                <asp:Label ID="lblValidadorAgregarRubros" runat="server"></asp:Label>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>



                                                <asp:Panel ID="pnlBotonesAgregarRubros" runat="server" CssClass="col-sm-3 text-right">

                                                    <asp:UpdateProgress ID="UpdateProgress4" AssociatedUpdatePanelID="updBotonesAgregarRubros"
                                                        runat="server" DisplayAfter="200">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                            Procesando...
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>


                                                    <div id="BotonesAgregarRubros_Rubros">
                                                        <asp:LinkButton ID="btnIngresarRubros" runat="server" CssClass="btn btn-primary" OnClick="btnIngresarRubros_Click" OnClientClick="ocultarBotonesAgregarRubros_Rubros();">
                                                            <i class="imoon imoon-plus"></i>
                                                            <span class="text">Agregar</span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-default" data-dismiss="modal">
                                                            <i class="imoon imoon-close"></i>
                                                            <span class="text">Cerrar</span>
                                                        </asp:LinkButton>
                                                    </div>

                                                </asp:Panel>
                                            </asp:Panel>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </asp:Panel>


                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>

                </div>
            </div>
        </div>
        <!-- /.modal -->
        <br />

        <asp:UpdatePanel ID="btn_BuscarTramite" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="pull-right">

                    <div class="control-group inline-block">

                        <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="btn_BuscarTramite"
                            runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
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

    </asp:Panel>

    <br />
    <br />

    <div id="box_resultado" class="widget-box" style="display: none;">
        <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                    function endRequestHandler() {
                        inicializar_popover();
                    }
                </script>
                <div style="margin-left: 10px; margin-right: 10px; overflow-x: scroll;">
                    <asp:Panel ID="pnlResultadoBuscar" runat="server">

                        <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">
                            <div class="col-sm-6">
                                <strong>Resultado de la b&uacute;squeda:</strong>
                            </div>
                            <div class="col-sm-6 text-right">
                                <asp:RadioButton ID="rbtRubro" runat="server" Text="Por Rubro" Checked="true" GroupName="tipoExp" />
                                <asp:RadioButton ID="rbtDomicilio" runat="server" Text="Por Domicilio" GroupName="tipoExp" />
                                <asp:RadioButton ID="rbtRubroDomicilio" runat="server" Text="Por Rubro y Domicilio." GroupName="tipoExp" CssClass="pright20" />

                                <strong>
                                    <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click" OnClientClick="return showfrmExportarExcel();" CssClass="link-local pright20" Style="font-size: 14px;">
                                        <i class="imoon imoon-file-excel color-green"></i>
                                        <span>Exportar a Excel</span>
                                    </asp:LinkButton>

                                    Cantidad de registros:
                                </strong>
                                <asp:Label ID="lblCantRegistros" runat="server" CssClass="badge">0</asp:Label>
                            </div>
                        </asp:Panel>

                        <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false"
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            DataKeyNames="id_solicitud"
                            SelectMethod="GetTramites" ItemType="SGI.Model.clsItemConsultaTramite"
                            AllowPaging="true" AllowSorting="true" PageSize="30" OnPageIndexChanging="grdTramites_PageIndexChanging"
                            OnDataBound="grdTramites_DataBound" >
                            <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                            <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                            <Columns>
                                <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud" />
                                <asp:BoundField DataField="id_solicitud_ref" HeaderText="Solicitud Referencia" />
                                <asp:TemplateField HeaderText="Ubicaciones" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkUbicaciones" runat="server" OnClientClick="return popOver(this);" data-toggle="popover" data-visible="false" data-placement="right" title="Lista de Ubicaciones">
                                            <i class="icon-share"></i>
                                        </asp:LinkButton>
                                        <%--Popover con la lista de tareas--%>
                                        <asp:Panel ID="pnlUbicaciones" runat="server" Style="display: none; padding: 10px; max-height: 600px; max-width: 1000px">
                                            <asp:GridView ID="grdUbicaciones" runat="server"
                                                AutoGenerateColumns="false"
                                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                                DataSource='<%# Item.Ubicaciones%>' ItemType="SGI.Model.clsItemConsultaUbicacion">
                                                <Columns>
                                                    <asp:BoundField DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="Barrio" HeaderText="Barrio" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="UnidadFuncional" HeaderText="U.F." ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="Seccion" HeaderText="Sección" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="Manzana" HeaderText="Manzana" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="Parcela" HeaderText="Parcela" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="PartidaMatriz" HeaderText="Partida Matriz" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="PartidaHorizontal" HeaderText="Partida Horizontal" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="SubTipoUbicacion" HeaderText="Subtipo" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="LocalSubTipoUbicacion" HeaderText="Local Subtipo" ItemStyle-CssClass="align-center" />
                                                    <asp:TemplateField HeaderText="Calles" ItemStyle-CssClass="align-center">
                                                        <ItemTemplate>
                                                            <asp:GridView ID="grdCalles" runat="server"
                                                                AutoGenerateColumns="false"
                                                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                                                DataSource='<%# Item.Calles%>' ItemType="SGI.Model.clsItemConsultaPuerta">
                                                                <Columns>
                                                                    <asp:BoundField DataField="calle" HeaderText="Calle" ItemStyle-CssClass="align-center" />
                                                                    <asp:BoundField DataField="puerta" HeaderText="Puerta" ItemStyle-CssClass="align-center" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Apertura" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="TipoTramite" HeaderText="Tipo Trámite" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="TipoExpediente" HeaderText="Tipo Expediente" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="SubTipoExpediente" HeaderText="Sub Tipo Expediente" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="TipoCAA" HeaderText="Tipo CAA" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="TareaActual" HeaderText="Tarea Actual" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Superficie" HeaderText="Superficie" />

                                <asp:TemplateField HeaderText="Tiene Plano Incendio" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                           <asp:CheckBox  runat="server" Enabled="false" Checked='<%# Eval("TienePlanoIncendio") %>'  ToolTip='<%# Eval("TienePlanoIncendio") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Rubros" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkRubros" runat="server" OnClientClick="return popOver(this);" data-toggle="popover" data-visible="false" data-placement="right" title="Lista de Rubros">
                                            <i class="icon-share"></i>
                                        </asp:LinkButton>
                                        <%--Popover con la lista de tareas--%>
                                        <asp:Panel ID="pnlRubros" runat="server" Style="display: none; padding: 10px; max-height: 300px; max-width: 500px">
                                            <asp:GridView ID="grdRubros" runat="server"
                                                AutoGenerateColumns="false"
                                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                                DataSource='<%# Item.Rubros%>' ItemType="SGI.Model.clsItemddlRubro">
                                                <Columns>
                                                    <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="nom_rubro" HeaderText="Descripción" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="id_subrubro" HeaderText="Código Sub Rubro" ItemStyle-CssClass="align-center" />
                                                    <asp:BoundField DataField="nom_subrubro" HeaderText="Descripción Subrubro" ItemStyle-CssClass="align-center" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Calificador" HeaderText="Calificador" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="ProfesionalAnexoTecnico" HeaderText="ProfesionalAnexoTecnico" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="ProfesionalAnexoNotarial" HeaderText="ProfesionalAnexoNotarial" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaLibrado" HeaderText="Fecha Librado al Uso" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaHabilitacion" HeaderText="Fecha Habilitación" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaBaja" HeaderText="Fecha Baja" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaCaducidad" HeaderText="Fecha Caducidad" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="NumeroExp" HeaderText="Número Expediente" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:TemplateField HeaderText="Titulares" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        <asp:Repeater ID="repeater_certificados" runat="server" DataSource='<%# Item.Titulares%>'>
                                            <ItemTemplate>
                                                <%# Eval("value").ToString() %>;
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cuit" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        <asp:Repeater ID="repeater_certificados2" runat="server" DataSource='<%# Item.Cuits%>'>
                                            <ItemTemplate>
                                                <%# Eval("value").ToString() %>;
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idEncomienda" HeaderText="Anexo Técnico" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="idCAA" HeaderText="Número CAA" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="idSIPSA" HeaderText="Número SIPSA" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="TipoNormativa" HeaderText="Normativa" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Organismo" HeaderText="Organismo" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="NroNormativa" HeaderText="Número Normativa" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="DiasEnCorreccion" HeaderText="Días en Corrección" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="MailTitulares" HeaderText="Mail de los Titulares" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="MailFirmantes" HeaderText="Mail de los Firmantes" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="MailUsuarioSSIT" HeaderText="Mail del Usuario SSIT" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="MailUsuarioTAD" HeaderText="Mail del Usuario TAD" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="PlantasHabilitar" HeaderText="Plantas a Habilitar" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="NombreyApellido" HeaderText="Nombre y Apellido" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaInicioAT" HeaderText="Fecha Inicio AT" DataFormatString="{0:d}" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="FechaAprobadoAT" HeaderText="Fecha Aprobado AT" DataFormatString="{0:d}" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                                

                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                    <p>
                                        No se encontraron trámites con los filtros ingresados.
                                    </p>
                                </asp:Panel>
                            </EmptyDataTemplate>
                            <PagerTemplate>
                                <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                    <div style="display: inline-table">
                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlResultadoBuscar" runat="server"
                                            DisplayAfter="0">
                                            <ProgressTemplate>
                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
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

                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--Exportación a Excel--%>
    <div id="frmExportarExcel" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updExportaExcel" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h4 class="modal-title">Exportar a Excel</h4>
                        </div>
                        <div class="modal-body">


                            <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" Enabled="false">
                            </asp:Timer>

                            <asp:Panel ID="pnlExportandoExcel" runat="server">
                                <div class="row text-center">
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                                </div>
                                <div class="row text-center">
                                    <h2>
                                        <asp:Label ID="lblRegistrosExportados" runat="server"></asp:Label></h2>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDescargarExcel" runat="server" Style="display: none">
                                <div class="row text-center">
                                    <asp:HyperLink ID="btnDescargarExcel" runat="server" Target="_blank" CssClass="btn btn-link">
                                        <i class="imoon imoon-file-excel color-green fs48"></i>
                                        <br />
                                        <span class="text">Descargar archivo</span>
                                    </asp:HyperLink>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrarExportacion" runat="server" CssClass="btn btn-default" OnClick="btnCerrarExportacion_Click" Text="Cerrar" Visible="false" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

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




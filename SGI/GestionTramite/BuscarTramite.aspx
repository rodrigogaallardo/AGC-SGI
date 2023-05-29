<%@  Title="Búsqueda del trámite" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarTramite.aspx.cs"
    Inherits="SGI.BuscarTramite" %>

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
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <script type="text/javascript">

        //TOMAS -- visibilizar el filtro del acordeon
        $(document).ready(function () {
            var myValue = $('#ultBTN input[type=hidden]').val();
            if (myValue == '') {
                inicializar_controles0();
            }
            {
                switch (myvalue) {
                    case ("porTramite"):
                        inicializar_controles0();
                        break;
                    case ("porPartida"):
                        inicializar_controles1();
                        break;
                    case ("porDomicilio"):
                        inicializar_controles2();
                        break;
                    case ("porSMP"):
                        inicializar_controles3();
                        break;
                    case ("porUbi"):
                        inicializar_controles4();
                        break;
                }
            }
        });

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function inicializar_controles0() {

            //debugger;
            //inicializar tootip del popOver
            inicializar_popover();
            inicializar_fechas();
            camposAutonumericos();
           // inicializar_autocomplete();
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTipoExpediente.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlSubTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTareaCerrada.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlLibradoUso.ClientID %>").select2({ allowClear: true });

<%--            var tags_selecionados = "";
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
                });--%>
            }

            function inicializar_controles1() {

                //debugger;
                //inicializar tootip del popOver
                inicializar_popover();
                inicializar_fechas();
                camposAutonumericos();
               // inicializar_autocomplete();
                $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
                $("#<%: ddlTipoExpediente.ClientID %>").select2({ allowClear: true });
                $("#<%: ddlSubTipoTramite.ClientID %>").select2({ allowClear: true });
                $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });
                $("#<%: ddlTareaCerrada.ClientID %>").select2({ allowClear: true });

                switchear_buscar_ubicacion(1);
                $("div.accordion-body").collapse().show();
                //$('div.accordion-body').collapse().slideDown();
                //$('div.accordion-body').collapse().slideUp();
            }

            function inicializar_controles2() {
                inicializar_popover();
                inicializar_fechas();
                camposAutonumericos();
                //inicializar_autocomplete();
                $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTipoExpediente.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlSubTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTareaCerrada.ClientID %>").select2({ allowClear: true });

            switchear_buscar_ubicacion(2);
            $("div.accordion-body").collapse().show();
        }

        function inicializar_controles3() {
            inicializar_popover();
            inicializar_fechas();
            camposAutonumericos();
           // inicializar_autocomplete();
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
        $("#<%: ddlTipoExpediente.ClientID %>").select2({ allowClear: true });
        $("#<%: ddlSubTipoTramite.ClientID %>").select2({ allowClear: true });
        $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });
        $("#<%: ddlTareaCerrada.ClientID %>").select2({ allowClear: true });

        switchear_buscar_ubicacion(3);
        $("div.accordion-body").collapse().show();
    }

    function inicializar_controles4() {
        inicializar_popover();
        inicializar_fechas();
        camposAutonumericos();
        //inicializar_autocomplete();
        $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTipoExpediente.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlSubTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTareaCerrada.ClientID %>").select2({ allowClear: true });

            switchear_buscar_ubicacion(4);
            $("div.accordion-body").collapse().show();
        }

        function camposAutonumericos() {
            $('#<%=txtNroSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtNroEncomienda.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtUbiSeccion.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtUbiNroPartida.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtUbiNroPuerta.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '999999' });
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
            var fechaCierreDesde = $('#<%=txtFechaCierreDesde.ClientID%>');
            var es_readonlyCierre = $(fechaCierreDesde).attr("readonly");

            $("#<%: txtFechaCierreDesde.ClientID %>").datepicker({
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

            var fechaHasta = $('#<%=txtFechaHasta.ClientID%>');
            var es_readonly = $(fechaHasta).attr("readonly");
            if (!($(fechaHasta).is('[disabled]') || $(fechaHasta).is('[readonly]'))) {
                $(fechaHasta).datepicker({
                    minDate: "-100Y",
                    maxDate: "0Y",
                    yearRange: "-100:-0",
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
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
            };
            var fechaCierreHasta = $('#<%=txtFechaCierreHasta.ClientID%>');
        var es_readonlyCierre = $(fechaCierreHasta).attr("readonly");
        if (!($(fechaCierreHasta).is('[disabled]') || $(fechaCierreHasta).is('[readonly]'))) {
            $(fechaCierreHasta).datepicker({
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
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

        };

    }
    function showResultado() {
        $("#box_resultado").show("slow");
    }

    function hideResultado() {
        $("#box_resultado").hide("slow");
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
            $("#buscar_ubi_por_partida").show();
            $("#buscar_ubi_por_dom").hide();
            $("#buscar_ubi_por_smp").hide();
            $("#buscar_ubi_por_especial").hide();

            $("#btnBuscarPorPartida").addClass("active");
            $("#btnBuscarPorDom").removeClass("active");
            $("#btnBuscarPorSMP").removeClass("active");
            $("#btnBuscarPorUbiEspecial").removeClass("active");

            $('#ultBTN input[type=hidden]').val("porPartida");
        }
        else if (btn == 2) {
            $("#buscar_ubi_por_partida").hide();
            $("#buscar_ubi_por_dom").show();
            $("#buscar_ubi_por_smp").hide();
            $("#buscar_ubi_por_especial").hide();

            $("#btnBuscarPorPartida").removeClass("active");
            $("#btnBuscarPorDom").addClass("active");
            $("#btnBuscarPorSMP").removeClass("active");
            $("#btnBuscarPorUbiEspecial").removeClass("active");

            $('#ultBTN input[type=hidden]').val("porDomicilio");
        }
        else if (btn == 3) {
            $("#buscar_ubi_por_partida").hide();
            $("#buscar_ubi_por_dom").hide();
            $("#buscar_ubi_por_smp").show();
            $("#buscar_ubi_por_especial").hide();

            $("#btnBuscarPorPartida").removeClass("active");
            $("#btnBuscarPorDom").removeClass("active");
            $("#btnBuscarPorSMP").addClass("active");
            $("#btnBuscarPorUbiEspecial").removeClass("active");

            $('#ultBTN input[type=hidden]').val("porSMP");
        }
        else if (btn == 4) {
            $("#buscar_ubi_por_partida").hide();
            $("#buscar_ubi_por_dom").hide();
            $("#buscar_ubi_por_smp").hide();
            $("#buscar_ubi_por_especial").show();

            $("#btnBuscarPorPartida").removeClass("active");
            $("#btnBuscarPorDom").removeClass("active");
            $("#btnBuscarPorSMP").removeClass("active");
            $("#btnBuscarPorUbiEspecial").addClass("active");

            $('#ultBTN input[type=hidden]').val("porUbi");
        }


    }


    function inicializar_popover() {

        $("[id*='lnkTareasSolicitud']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

        $("[id*='MainContent_grdTramites_lnkTareasSolicitud_']").each(function () {
            //para cada fila de la grilla, se busca el link y se lo vincula al panel de la misma fila
            //para que con el clikc del link habra el popOver de un html
            var id_pnlTareas = $(this).attr("id").replace("MainContent_grdTramites_lnkTareasSolicitud_", "MainContent_grdTramites_pnlTareas_");
            var objTareas = $("#" + id_pnlTareas).html();
            $(this).popover({
                title: 'Tareas',
                content: objTareas,
                html: 'true'
            });
        });
    }

    function popOverTareas(obj) {

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

    </script>

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">

        <%-- filtros de busqueda--%>
        <div class="">

            <%--buscar tramite --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

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


                                <%--                <div id="myContainer">
                    <asp:HiddenField ID="hdMyControl" runat="server" /></div>--%>

                                <div id="ultBTN">
                                    <asp:HiddenField ID="hdUltBtn" runat="server" />
                                </div>

                                <table>
                                    <tr>

                                        <td style="vertical-align: top">
                                            <div class="form-horizontal" style="width: 50%">
                                                <fieldset>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblNroSolicitud" runat="server" AssociatedControlID="txtNroSolicitud"
                                                            Text="Solicitud:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtNroSolicitud" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtFechaDesde"
                                                            Text="Fecha Apertura Desde:" class="control-label"></asp:Label>
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
                                                        <asp:Label ID="lblFechaCierreDesde" runat="server" AssociatedControlID="txtFechaCierreDesde"
                                                            Text="Fecha Cierre Desde:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaCierreDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <asp:RegularExpressionValidator
                                                                    ID="rev_txtFechaCierreDesde" runat="server"
                                                                    ValidationGroup="buscar"
                                                                    ControlToValidate="txtFechaCierreDesde" CssClass="field-validation-error"
                                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                            </div>

                                                        </div>
                                                    </div>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblTipoTramite" runat="server" AssociatedControlID="ddlTipoTramite"
                                                            Text="Tipo Trámite:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="200px" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlTipoTramite_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblSubTipoTramite" runat="server" AssociatedControlID="ddlSubTipoTramite"
                                                            Text="Subtipo Trámite:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlSubTipoTramite" runat="server" Width="200px" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlSubTipoTramite_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblNroExpediente" runat="server" AssociatedControlID="txtNroExp"
                                                            Text="Nro. Expediente:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtNroExp" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblLibradoUso" runat="server" AssociatedControlID="ddlLibradoUso"
                                                            Text="Librado al Uso:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlLibradoUso" runat="server" Width="200px"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </td>

                                        <td style="vertical-align: top;">
                                            <div class="form-horizontal" style="width: 300px">
                                                <fieldset>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblNroEncomienda" runat="server" AssociatedControlID="txtNroEncomienda"
                                                            Text="Encomienda:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtNroEncomienda" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblFechaHasta" runat="server" AssociatedControlID="txtFechaHasta"
                                                            Text="Fecha Apertura Hasta:" class="control-label"></asp:Label>
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
                                                        <asp:Label ID="lblFechaCierreHasta" runat="server" AssociatedControlID="txtFechaCierreHasta"
                                                            Text="Fecha Cierre Hasta:" class="control-label"></asp:Label>
                                                        <div class="controls">

                                                            <asp:TextBox ID="txtFechaCierreHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <asp:RegularExpressionValidator
                                                                    ID="rev_txtFechaCierreHasta" runat="server"
                                                                    ValidationGroup="buscar"
                                                                    ControlToValidate="txtFechaCierreHasta" CssClass="field-validation-error"
                                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                            </div>

                                                        </div>
                                                    </div>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblTipoExpediente" runat="server" AssociatedControlID="ddlTipoExpediente"
                                                            Text="Tipo Expediente:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTipoExpediente" runat="server" Width="200px" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlTipoExpediente_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblTarea" runat="server" AssociatedControlID="ddlTarea"
                                                            Text="Tarea abierta:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTarea" runat="server" Width="300px"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblTareaCerrada" runat="server" AssociatedControlID="ddlTareaCerrada"
                                                            Text="Tarea cerrada:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTareaCerrada" runat="server" Width="300px"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </fieldset>
                                            </div>
                                        </td>

                                    </tr>

                                </table>

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

                <%-- controles collapsible buscar por ubicacion TOMAS // collapse o no, depende del filtro--%>
                <div class="accordion-body collapse" id="collapse_bt_ubicacion">
                    <div class="widget-content">

                        <%--tipos de busquedad por ubicacion--%>
                        <div class="widget-content">

                            <div class="btn-group" data-toggle="buttons-radio" style="display: table-cell;">
                                <button id="btnBuscarPorPartida" type="button" class="btn active" onclick="switchear_buscar_ubicacion(1);">Por Partida</button>
                                <button id="btnBuscarPorDom" type="button" class="btn" onclick="switchear_buscar_ubicacion(2);">Por Domicilio</button>
                                <button id="btnBuscarPorSMP" type="button" class="btn" onclick="switchear_buscar_ubicacion(3);">Por SMP</button>
                                <button id="btnBuscarPorUbiEspecial" type="button" class="btn" onclick="switchear_buscar_ubicacion(4);">Por Ubicaciones Especiales</button>
                            </div>

                        </div>


                        <%--buscar por numero de partida--%>
                        <div id="buscar_ubi_por_partida" class="widget-content">

                            <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_partida" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-horizontal">

                                        <fieldset>

                                            <div class="control-group">
                                                <asp:Label ID="lblUbiPartidaMatriz" runat="server" AssociatedControlID="rbtnUbiPartidaMatriz"
                                                    CssClass="control-label">Tipo de Partida:</asp:Label>
                                                <div class="controls">
                                                    <div class="form-inline">
                                                        <asp:RadioButton ID="rbtnUbiPartidaMatriz" runat="server"
                                                            Text="Matriz" GroupName="TipoDePartida" Checked="true" />
                                                        <asp:RadioButton ID="rbtnUbiPartidaHoriz" runat="server" Style="padding-left: 5px"
                                                            Text="Horizontal" GroupName="TipoDePartida" />
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="control-group">
                                                <asp:Label ID="lblUbiNroPartida" runat="server" AssociatedControlID="txtUbiNroPartida"
                                                    CssClass="control-label">Nro. Partida:</asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtUbiNroPartida" runat="server" MaxLength="10" Width="100px"
                                                        CssClass="input-xlarge"></asp:TextBox>
                                                </div>
                                            </div>


                                        </fieldset>

                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>

                        <%--buscar por nombre de calle--%>
                        <div id="buscar_ubi_por_dom" class="widget-content" style="display: none">

                            <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_dom" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-horizontal">

                                        <fieldset>

                                            <div class="control-group">
                                                <asp:Label ID="lblUbiCalle" runat="server" AssociatedControlID="AutocompleteCalles"
                                                    CssClass="control-label">Búsqueda de Calle:</asp:Label>
                                                <div class="controls">
                                                    <div class="clearfix">
                                                        <div class="pull-left">
                                                            <ej:Autocomplete ID="AutocompleteCalles" MinCharacter="3" DataTextField="NombreOficial_calle" DataUniqueKeyField="id_calle" Width="500px" runat="server" FilterType="Contains" EnablePersistence="false"  OnValueSelect="AutocompleteCalles_ValueSelect"/>
                                                            <span style="font-size: 8pt">Debe ingresar un mínimo de 3 letras y el sistema le mostrará
                                                                las calles posibles.</span>
                                                            <asp:RequiredFieldValidator ID="ReqCalle" runat="server" ErrorMessage="Debe seleccionar una de las calles de la lista desplegable."
                                                                Display="Dynamic" ControlToValidate="AutocompleteCalles" ValidationGroup="Buscar2"
                                                                CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblUbiNroPuerta" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">Nro. Puerta:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtUbiNroPuerta" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblUbiUF" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">UF:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtUF" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblUbiTorre" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">Torre:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtTorre" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblUbiDpto" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">Dpto:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtDpto" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblUbiLocal" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">Local:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtLocal" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblNroPuertaDesde" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">Nro. puerta desde:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtNroPuertaDesde" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblNroPuertaHasta" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                                CssClass="control-label">Hasta:</asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtNroPuertaHasta" runat="server" MaxLength="10" Width="50px"
                                                                    CssClass="input-xlarge"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="control-group">
                                                            <asp:Label ID="lblNroPuertaRadio" runat="server" AssociatedControlID="rbtnUbiPartidaMatriz"
                                                                CssClass="control-label">Buscar rango por:</asp:Label>
                                                            <div class="controls">
                                                                <div class="form-inline" style="padding-top: 5px">
                                                                    <asp:RadioButton ID="rbtnNroPuertaPar" runat="server"
                                                                        Text="&nbspPar" GroupName="NroPuerta" />
                                                                    <asp:RadioButton ID="rbtnNroPuertaImpar" runat="server" Style="padding-left: 5px"
                                                                        Text="&nbspImpar" GroupName="NroPuerta" />
                                                                    <asp:RadioButton ID="rbtnNroPuertaAmbas" runat="server" Style="padding-left: 5px"
                                                                        Text="&nbspAmbas" GroupName="NroPuerta" Checked="true" />
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

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

                        <%--buscar por tipo subtipo ubicacion--%>
                        <div id="buscar_ubi_por_especial" class="widget-content" style="display: none">

                            <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_especial" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-horizontal">

                                        <div class="control-group">
                                            <label for="ddlbiTipoUbicacion" class="control-label">Tipo de Ubicación:</label>
                                            <div class="controls">

                                                <asp:DropDownList ID="ddlbiTipoUbicacion" runat="server"
                                                    OnSelectedIndexChanged="ddlbiTipoUbicacion_SelectedIndexChanged"
                                                    AutoPostBack="true" Width="350px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label for="ddlUbiSubTipoUbicacion" class="control-label">Subtipo de Ubicación:</label>
                                            <div class="controls">

                                                <asp:DropDownList ID="ddlUbiSubTipoUbicacion" runat="server"
                                                    Width="350px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

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

                        <%--            <asp:HiddenField ID="hid_bt_rubro_collapse" runat="server" Value="false"/>
                    <asp:HiddenField ID="hid_bt_rubro_visible" runat="server" Value="false"/>--%>

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

                        <asp:UpdatePanel ID="updPnlFiltroBuscar_rubros" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">

                                    <fieldset>

                                        <div class="control-group">
                                            <asp:Label ID="lblRubroCodDesc" runat="server" AssociatedControlID="txtRubroCodDesc"
                                                Text="Código o Descripción del Rubro:" class="control-label" Width="210px"></asp:Label>
                                            <div class="controls" style="margin-left: 225px">
                                                <asp:TextBox ID="txtRubroCodDesc" runat="server" MaxLength="70" Width="700px"></asp:TextBox>
                                                <div class="req">
                                                    <asp:RegularExpressionValidator ID="rev_txtRubroCodDesc" runat="server"
                                                        ControlToValidate="txtRubroCodDesc"
                                                        ErrorMessage="Debe ingresar al menos tres letras."
                                                        CssClass="field-validation-error" Display="Dynamic"
                                                        ValidationExpression=".{3,70}" ValidationGroup="buscar">
                                                    </asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>


                                    </fieldset>

                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>

            </div>

            <%--buscar titulares --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 0px">

                <%-- titulo collapsible buscar titulares --%>
                <div class="accordion-heading">
                    <a id="bt_titular_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_titular"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                        <%--                    <asp:HiddenField ID="hid_bt_titular_collapse" runat="server" Value="false"/>
                    <asp:HiddenField ID="hid_bt_titular_visible" runat="server" Value="false"/>--%>

                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="Label3" runat="server" Text="Titulares"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-down"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible buscar por titulares --%>
                <div class="accordion-body collapse" id="collapse_bt_titular">
                    <div class="widget-content">

                        <asp:UpdatePanel ID="updPnlFiltroBuscar_titulares" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">

                                    <fieldset>

                                        <div class="control-group">
                                            <asp:Label ID="lblTitApellido" runat="server" AssociatedControlID="txtTitApellido"
                                                Text="Titular o Razón Social:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtTitApellido" runat="server" MaxLength="40" Width="400px"></asp:TextBox>
                                                <div class="req">
                                                    <asp:RegularExpressionValidator ID="rev_txtTitApellido" runat="server"
                                                        ControlToValidate="txtTitApellido"
                                                        ErrorMessage="Debe ingresar al menos tres letras."
                                                        CssClass="field-validation-error" Display="Dynamic"
                                                        ValidationExpression=".{3,40}" ValidationGroup="buscar">
                                                    </asp:RegularExpressionValidator>
                                                </div>
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

        <br />

        <asp:UpdatePanel ID="btn_BuscarTramite" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="pull-right">

                    <div class="control-group inline-block">

                        <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="btn_BuscarTramite"
                            runat="server" DisplayAfter="0">
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
                <div style="margin-left: 10px; margin-right: 10px">

                    <asp:Panel ID="pnlResultadoBuscar" runat="server">

                        <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">

                            <div style="display: inline-block">
                                <h5>Lista de Tr&aacute;mites</h5>
                            </div>
                            <div style="display: inline-block">
                                (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>
                                )
                            </div>

                        </asp:Panel>

                        <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false"
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            DataKeyNames="id_solicitud,formulario_tarea"
                            SelectMethod="GetTramites" ItemType="SGI.Model.clsItemBuscarTramite"
                            AllowPaging="true" AllowSorting="true" PageSize="30"
                            OnPageIndexChanging="grdTramites_PageIndexChanging"
                            OnDataBound="grdTramites_DataBound"
                            OnRowDataBound="grdTramites_RowDataBound">
                            <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                            <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                            <Columns>

                                <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="id_solicitud">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# Item.url_visorTramite%>'><%# Item.id_solicitud %></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="15px">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="lnkTareasSolicitud" runat="server" title="Lista de Tareas"
                                            CommandArgument='<%# Eval("id_solicitud") %>'
                                            OnClientClick="return popOverTareas(this);"
                                            data-toggle="popover" data-visible="false" data-placement="right">
                                    <i class="icon-share"></i>
                                        </asp:LinkButton>

                                        <%--Popover con la lista de tareas--%>
                                        <asp:Panel ID="pnlTareas" runat="server" Style="display: none; padding: 10px; max-height: 300px; max-width: 500px">

                                            <asp:GridView ID="grdTareas" runat="server" AutoGenerateColumns="false"
                                                CssClass="table table-bordered table-striped table-hover with-check"
                                                GridLines="None">

                                                <Columns>

                                                    <asp:HyperLinkField DataNavigateUrlFormatString="{0}"
                                                        DataNavigateUrlFields="url_tareaTramite"
                                                        DataTextField="nombre_tarea" HeaderText="Tarea"
                                                        ItemStyle-Width="200px" SortExpression="nombre_tarea" />

                                                    <asp:BoundField DataField="FechaInicio_tramitetarea"
                                                        HeaderText="Tarea creada el" DataFormatString="{0:d}"
                                                        ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />

                                                </Columns>
                                            </asp:GridView>


                                        </asp:Panel>


                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="estado" HeaderText="Estado" />
                                <asp:BoundField DataField="direccion" HeaderText="Ubicación" />

                                <asp:TemplateField HeaderText="Tarea" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" SortExpression="nombre_tarea">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkid_tarea" runat="server" NavigateUrl='<%# Item.url_tareaTramite%>'><%# Item.nombre_tarea %></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="desc_circuito" HeaderText="Circuito" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" SortExpression="desc_circuito" />

                                <asp:BoundField DataField="FechaInicio_tarea" HeaderText="Tarea creada el" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaInicio_tarea" />

                                <asp:BoundField DataField="LibradoUso" HeaderText ="Librado al uso el" DataFormatString="{0:g}" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" SortExpression="LibradoUso" />
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
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>




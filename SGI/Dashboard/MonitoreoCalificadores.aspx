<%@ Page Title="Seguimiento Equipo de Trabajo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MonitoreoCalificadores.aspx.cs"
    Inherits="SGI.Dashboard.MonitoreoCalificadores" %>

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
    <%: Scripts.Render("~/bundles/chartjs") %>

    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdntxtFechaHasta" runat="server" />
            <asp:HiddenField ID="hdntxtFechaDesde" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <hgroup class="title">
        <h1>Seguimiento Equipo de Trabajo</h1>
    </hgroup>
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnBuscar">
        <div>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                <div class="accordion-heading">
                    <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_tramite_tituloControl" runat="server" Text="Desempeño de los calificadores de sus equipos de trabajo"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="collapse_bt_tramite">
                    <div class="widget-content">
                        <asp:UpdatePanel ID="updPnlMonitoreo" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="vertical-align: top">
                                            <div class="form-horizontal" style="width: 100%">
                                                <fieldset>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblCalificador" runat="server" AssociatedControlID="ddlCalificador" Text="Calificador:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlCalificador" runat="server" Width="300px"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtFechaDesde" Text="Fecha Desde:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <div id="req_txtFechaDesde" class="field-validation-error" style="display: none;">
                                                                    Campo requerido
                                                                </div>
                                                                <asp:RegularExpressionValidator ID="rev_txtFechaDesde" runat="server" ValidationGroup="buscar" ControlToValidate="txtFechaDesde"
                                                                    CssClass="field-validation-error" ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblFechaHasta" runat="server" AssociatedControlID="txtFechaHasta" Text="Fecha Hasta:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <div id="req_txtFechaHasta" class="field-validation-error" style="display: none;">
                                                                    Campo requerido
                                                                </div>
                                                                <asp:RegularExpressionValidator ID="rev_txtFechaHasta" runat="server" ValidationGroup="buscar" ControlToValidate="txtFechaHasta"
                                                                    CssClass="field-validation-error" ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
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
            <asp:UpdatePanel ID="upd_Botones" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="pull-right">
                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="upd_Botones"
                                runat="server" DisplayAfter="0">
                                <ProgressTemplate>
                                    <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnBuscar_Click" OnClientClick="return validarDatos();">
                    <i class="icon-white icon-search"></i>
                    <span class="text">Buscar</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_Click">
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
        </div>
    </asp:Panel>
    <div id="box_resultados" class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px; display: none" runat="server">
        <%-- titulo collapsible --%>
        <div class="accordion-heading">
            <a id="btn_Tramites_btnUpDown" data-parent="#collapse-group" runat="server">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-stats" style="color: #344882;"></i></span>
                    <h5>
                        <asp:Label ID="lbl_Tramites" runat="server" Text="Tramites"></asp:Label></h5>
                    <span class="btn-right"><i class="imoon imoon-chevron-up" style="color: #344882;"></i></span>
                </div>
            </a>
        </div>
        <%-- contenido del collapsible --%>
        <div class="accordion-body" id="collapse_tramites">
            <div class="widget-content">
                <div class="row">
                    <div style="width: 500px; padding-left: 400px;" id="DivCanvas" runat="server">
                        <canvas id="myChart" height="600" width="600" style="display: none"></canvas>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="updResultado">
                    <ContentTemplate>

                        <script type="text/javascript">
                            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                            function endRequestHandler() {
                                inicializar_controles();
                            }
                        </script>

                        <div class="form-horizontal">
                            <asp:GridView ID="grdCalificadores" runat="server" AutoGenerateColumns="false" Visible="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="Calificador" HeaderText="Calificador" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="fechaDesde" HeaderText="Fecha Desde" ItemStyle-Width="100px" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="fechaHasta" HeaderText="Fecha Hasta" ItemStyle-Width="100px" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                    <asp:TemplateField HeaderText="Cantidad de Solicitudes Trabajadas" ItemStyle-Width="75px" ItemStyle-CssClass="align-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="id_solicitud" runat="server" NavigateUrl="#" Text='<%# Eval("cantidad") %>' OnClick="id_solicitud_Click"
                                                CommandArgument='<%# Eval("UsuarioAsignado_tramitetarea") + "," + Eval("Calificador")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UsuarioAsignado_tramitetarea" HeaderText="UsuarioAsignado_tramitetarea" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" Visible="false" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                        <p>
                                            No se encontraron Datos
                                        </p>
                                    </asp:Panel>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="box_Detalles" class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px; display: none" runat="server">
        <%-- titulo collapsible --%>
        <div class="accordion-heading">
            <a id="btn_Detalle_btnUpDown" data-parent="#collapse-group" runat="server">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-stats" style="color: #344882;"></i></span>
                    <h5>
                        <asp:Label ID="Label1" runat="server" Text="Detalle"></asp:Label></h5>
                    <span class="btn-right"><i class="imoon imoon-chevron-up" style="color: #344882;"></i></span>
                </div>
            </a>
        </div>
        <%-- contenido del collapsible --%>
        <div class="accordion-body" id="collapse_detalles">
            <div class="widget-content">
                <asp:UpdatePanel runat="server" ID="updDetalles">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <fieldset>
                                <div class="control-group">
                                    <asp:Label ID="DetalleCalificador" Style="font-weight: bold; font-size: 16px; color: #555555" runat="server">Calificador:</asp:Label>
                                    <br />
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="DetalleFechaDesde" Style="font-size: 13px; color: #555555" runat="server">Fecha desde:</asp:Label>
                                    <br />
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="DetalleFechaHasta" Style="font-size: 13px; color: #555555" runat="server">Fecha hasta:</asp:Label>
                                    <br />
                                </div>
                            </fieldset>
                            <asp:GridView ID="grdSolicitudes" runat="server" AutoGenerateColumns="false"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# "~/GestionTramite/VisorTramite.aspx?id=" + Eval("id_solicitud") %>'><%# Eval("id_solicitud") %></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="cod_circuito" HeaderText="Circuito" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="FechaCierre_tramitetarea" HeaderText="Fecha de Última calificación" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Cantidad_de_Calificaciones" HeaderText="Cantidad de Calificaciones" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                        <p>
                                            No se encontraron Datos
                                        </p>
                                    </asp:Panel>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%--Modal mensajes de error--%>
    <div id="frmError" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" style="margin-top: -8px">Atención</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <asp:Label runat="server" class="imoon imoon-info fs64" Style="color: #377bb5"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="udpError" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer mleft20 mright20">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->
    <script type="text/javascript">
        $(document).ready(function () {
            inicializar_controles();            
        });

        function inicializar_controles() {
            inicializar_fechas();
            $("#<%: ddlCalificador.ClientID %>").select2({ allowClear: true });

        }

        ///Fechas
        function validarDatos() {
            $("#req_txtFechaDesde").hide();
            $("#req_txtFechaHasta").hide();

            var ret = true;

            if ($.trim($("#<%: txtFechaDesde.ClientID %>").val()).length == 0) {
                $("#req_txtFechaDesde").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtFechaHasta.ClientID %>").val()).length == 0) {
                $("#req_txtFechaHasta").css("display", "inline-block");
                ret = false;
            }
            return ret;
        }

        function showfrmError() {
            $("#frmError").modal("show");
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
                firstDay: 1,
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
                        firstDay: 1,
                        isRTL: false,
                        showMonthAfterYear: false,
                        yearSuffix: ''
                    });
            };


            var fechaHasta = $('#<%=txtFechaHasta.ClientID%>');
            var es_readonly = $(fechaHasta).attr("readonly");
            $("#<%: txtFechaHasta.ClientID %>").datepicker({
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                firstDay: 1,
                onSelect: function () {
                    $("#Req_FechaHasta").hide();
                    $("#Val_Formato_FechaHasta").hide();
                    $("#Val_FechaCierreDesdeMenor").hide();
                }
            });

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
                    firstDay: 1,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''
                });
            };
        }
        ///Fin Fechas

        function bt_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(obj).find("i.imoon-chevron-down").length > 0) {
                    $(obj).find("i.imoon-chevron-down").switchClass("imoon-chevron-down", "imoon-chevron-up", 0);
                }
                else {
                    $(obj).find("i.imoon-chevron-up").switchClass("imoon-chevron-up", "imoon-chevron-down", 0);
                }
            }
        }

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        var myChart = null;

        function Graficar(datos, label, colorChart) {

            var strArr = datos.split(',');
            var intArr = [];
            for (i = 0; i < strArr.length; i++)
                intArr.push(parseInt(strArr[i]));

            var strColor = colorChart.split(',');

            var ctx = document.getElementById("myChart").getContext("2d");

            document.getElementById("myChart").style.display = "block";

            if (myChart != null) {
                myChart.destroy();
            }

            myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    datasets: [{
                        borderWidth: 2,
                        hoverBorderWidth: 10,
                        backgroundColor: strColor,
                        hoverBackgroundColor: strColor,
                        hoverBorderColor: strColor,
                        borderColor: strColor,
                        data: intArr
                    }],
                    labels: label.split(',')
                }
            });
        }

        function clearChart() {
            if (myChart != null) {
                document.getElementById('myChart').style.display = "none";
            }
        }

        function showDetalles() {
            $('#<%: box_Detalles.ClientID%>').show("slow");
        }

        function hideDetalles() {
            $('#<%: box_Detalles.ClientID%>').hide();
        }

        function showResultados() {
            $('#<%: box_resultados.ClientID%>').show("slow");   
        }

        function hideResultados() {
            $('#<%: box_resultados.ClientID%>').hide();
        }
    </script>

</asp:Content>

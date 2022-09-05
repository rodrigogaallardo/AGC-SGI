<%@ Page Title="Seguimiento Equipo de gerencias" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MonitoreoGerenciasDetalle.aspx.cs" Inherits="SGI.Dashboard.MonitoreoGerenciasDetalle" %>

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
    <%: Scripts.Render("~/bundles/gritter") %>

    <asp:UpdatePanel runat="server" ID="updHids" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hidTramites" runat="server" />
            <asp:HiddenField ID="hidIdTipo" runat="server" />
            <asp:HiddenField ID="hidDescTipo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <hgroup class="title">
        <h1>Seguimiento de gerencias</h1>
    </hgroup>
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnRegresar">
        <div>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                <div class="accordion-heading">
                    <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_tramite_tituloControl" runat="server" Text="Grafíco de desempeño de la gerencia"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="collapse in" id="collapse_bt_tramite">
                    <table style="border: 0px solid DarkGray; width: 100%; padding-left: 0px; padding-right: 0px">
                        <tr>
                            <td style="border: 1px solid LightGray; width: 49%">
                                <div style="width: 100%; display: inline-table;">
                                    <h6 class="my-0 font-weight-normal">Grafico por tareas</h6>
                                    <div style="width: 98%; padding-left: 2%;" id="Div1" runat="server">
                                        <canvas id="myChartTareas" height="500" width="500" style="display: none"></canvas>
                                    </div>
                                </div>
                            </td>
                            <td style="border: 1px solid LightGray; width: 49%">
                                <div style="width: 100%; display: inline-table;">
                                    <h6 class="my-0 font-weight-normal">Grafico por persona asignada</h6>
                                    <div style="width: 98%; padding-left: 2%;" id="DivCanvasPersonas" runat="server">
                                        <canvas id="myChartPersonas" height="500" width="500" style="display: none"></canvas>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>


            </div>
        </div>

        <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
            <div class="accordion-heading">
                <a id="bt_Datos_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_Datos"
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                    <div class="widget-title">
                        <span class="icon"><i class="icon-list-alt"></i></span>
                        <h5>
                            <asp:Label ID="Label1" runat="server" Text="Datos de desempeño las gerencias por "></asp:Label></h5>
                        <span class="btn-right"><i class="icon-chevron-up"></i></span>
                    </div>
                </a>
            </div>
            <div class="accordion-body collapse in" id="collapse_bt_Datos">

                <div style="border: 0px solid black; width: 50%; display: inline-table;">
                    <h6 class="my-0 font-weight-normal">Detalle por tareas</h6>
                    <div style="width: 99%">
                        <asp:UpdatePanel runat="server" ID="updResultado">
                            <ContentTemplate>

                                <script type="text/javascript">
                                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                                    function endRequestHandler() {
                                        inicializar_controles();
                                    }
                                </script>

                                <div class="form-horizontal">
                                    <asp:GridView
                                        ID="grdPorTareas"
                                        OnRowDataBound="grdPorTareas_RowDataBound"
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        Visible="true"
                                        GridLines="None"
                                        CssClass="table table-bordered table-striped table-hover with-check"
                                        ShowFooter="True">
                                        <Columns>
                                            <asp:BoundField DataField="cod_tarea" HeaderText="Codigo" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" visible="false" />
                                            <asp:BoundField DataField="nombre_tarea" HeaderText="Tarea" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                            <asp:BoundField DataField="fechaDesde" HeaderText="Fecha Desde" ItemStyle-Width="100px" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                            <asp:BoundField DataField="fechaHasta" HeaderText="Fecha Hasta" ItemStyle-Width="100px" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                            <asp:TemplateField HeaderText="Cantidad de Solicitudes" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" FooterStyle-CssClass="align-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="DetalleGrafTarea" runat="server" NavigateUrl="#" Text='<%# Eval("cantidad") %>' OnClick="DetalleGrafTarea_Click" CommandArgument='<%# Eval("cod_tarea") + "," + Eval("nombre_tarea")%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <strong>
                                                        <asp:Label ID="lbltotalTareas" runat="server" Text="Label"></asp:Label></strong>
                                                </FooterTemplate>
                                            </asp:TemplateField>
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
                <div style="border: 0px solid black; width: 49%; display: inline-table;">
                    <h6 class="my-0 font-weight-normal">Detalle por persona asignada</h6>
                    <div style="width: 99%">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>

                                <script type="text/javascript">
                                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                                    function endRequestHandler() {
                                        inicializar_controles();
                                    }
                                </script>

                                <div class="form-horizontal">
                                    <asp:GridView
                                        ID="grdPorPersonas"
                                        OnRowDataBound="grdPorPersonas_RowDataBound"
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        Visible="true"
                                        GridLines="None"
                                        CssClass="table table-bordered table-striped table-hover with-check"
                                        ShowFooter="True">
                                        <Columns>
                                            <asp:BoundField DataField="userid" HeaderText="Id" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" Visible="false" />
                                            <asp:BoundField DataField="Apellido_nombre" HeaderText="Persona" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                            <asp:BoundField DataField="fechaDesde" HeaderText="Fecha Desde" ItemStyle-Width="100px" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                            <asp:BoundField DataField="fechaHasta" HeaderText="Fecha Hasta" ItemStyle-Width="100px" DataFormatString="{0:d}" ItemStyle-CssClass="align-center" />
                                            <asp:TemplateField HeaderText="Cantidad de Solicitudes" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" FooterStyle-CssClass="align-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="DetalleGrafPersona" runat="server" NavigateUrl="#" Text='<%# Eval("cantidad") %>' OnClick="DetalleGrafPersona_Click" 
                                                        CommandArgument='<%# String.Format("{0},{1}", Eval("userid") != null ? Eval("userid").ToString() : "", Eval("Apellido_nombre"))%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <strong>
                                                        <asp:Label ID="lbltotalPersonas" runat="server" Text="Label"></asp:Label></strong>
                                                </FooterTemplate>
                                            </asp:TemplateField>
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
        </div>

        <div id="box_Detalles" class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px; display: none;">
            <%-- titulo collapsible --%>
            <div class="accordion-heading">
                <a id="bt_detalle_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_detalle"
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                    <div class="widget-title">
                        <span class="icon"><i class="imoon imoon-stats" style="color: #344882;"></i></span>
                        <h5>
                            <asp:Label ID="Label2" runat="server" Text="Detalle"></asp:Label></h5>
                        <span class="btn-right"><i class="imoon imoon-chevron-up" style="color: #344882;"></i></span>
                    </div>
                </a>
            </div>
            <%-- contenido del collapsible --%>
            <div class="accordion-body collapse in" id="collapse_bt_detalle">
                <div class="widget-content">
<%--                    <asp:UpdatePanel ID="updExcel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="pull-right">
                                <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click" OnClientClick="return showfrmExportarExcel();" CssClass="link-local pright20" Style="font-size: 14px;">
                            <i class="imoon imoon-file-excel color-green"></i>
                            <span>Exportar a Excel</span>
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <asp:UpdatePanel runat="server" ID="updDetalles">
                        <ContentTemplate>
                            <div class="form-horizontal">
                                <fieldset>
                                    <div class="control-group">
                                        <asp:Label ID="lblDescTipo" Style="font-weight: bold; font-size: 16px; color: #555555" runat="server"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </fieldset>
                                <asp:GridView ID="grdSolicitudes" runat="server" AutoGenerateColumns="false" OnSorting="grdSolicitudes_Sorting"
                                    GridLines="None" CssClass="table table-bordered table-striped table-hover with-check" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="Solicitud">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkid_solicitud" runat="server" NavigateUrl="#" Text='<%# Eval("Solicitud") %>'
                                                    OnClientClick='<%# "VerTramite(\"" + Eval("Circuito") + "," + Eval("Solicitud") + "\"); return false;" %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Tipo" />
                                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Tarea" />
                                        <asp:BoundField DataField="Circuito" HeaderText="Circuito" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Circuito" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Fecha" />
                                        <asp:BoundField DataField="Dias" HeaderText="Dias" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Dias" />
                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Usuario" />
                                        <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="Observaciones" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="pad10">
                                            <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                            <span class="mleft20">No se encontraron Datos</span>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
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
                    <asp:LinkButton ID="btnRegresar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClientClick="return Regresar();">
                            <i class="icon-white icon-search"></i>
                            <span class="text">Regresar</span>
                    </asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

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


        }

        function showDetalles() {
            $("#box_Detalles").show("slow");
        }

        function hideDetalles() {
            $("#box_Detalles").hide();
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }


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

        var myChartTareas = null;
        var myChartPersonas = null;

        function GraficarTareas(datos, label, colorChart) {
            var strArr = datos.split(',');
            var intArr = [];
            try {
                for (i = 0; i < strArr.length; i++)
                    intArr.push(parseInt(strArr[i]));

                var strColor = colorChart.split(',');

                var ctx = document.getElementById("myChartTareas").getContext("2d");

                document.getElementById("myChartTareas").style.display = "block";

                if (myChartTareas != null) {
                    myChartTareas.destroy();
                }
                myChartTareas = new Chart(ctx, {
                    type: 'pie',
                    data: {
                        datasets: [{
                            borderWidth: 0,
                            hoverBorderWidth: 10,
                            backgroundColor: strColor,
                            hoverBackgroundColor: strColor,
                            hoverBorderColor: strColor,
                            borderColor: strColor,
                            data: intArr
                        }],
                        labels: label.split(',')
                    },
                    options: {
                        legend: {
                            display: true,
                            labels: {
                                fontSize: 8
                            }
                        }
                    }
                }
                );
            }
            catch (err) {
                alert("Graficar error: " + err.message);
            }
        }

        function GraficarPersonas(datos, label, colorChart) {

            var strArr = datos.split(',');
            var intArr = [];
            try {
                for (i = 0; i < strArr.length; i++)
                    intArr.push(parseInt(strArr[i]));

                var strColor = colorChart.split(',');

                var ctx = document.getElementById("myChartPersonas").getContext("2d");

                document.getElementById("myChartPersonas").style.display = "block";

                if (myChartPersonas != null) {
                    myChartPersonas.destroy();
                }
                myChartPersonas = new Chart(ctx, {
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
                    },
                    options: {
                        legend: {
                            display: true,
                            labels: {
                                fontSize: 8
                            }
                        }
                    }
                }
                );
            }
            catch (err) {
                alert("Graficar error: " + err.message);
            }
        }

        function VerTramite(args) {

            var circuito = args.split(',')[0];
            var solicitud = args.split(',')[1];

            //alert(circuito);
            //alert(solicitud);

            if (circuito != "TRANSF")
                window.open("../GestionTramite/VisorTramite.aspx?id=" + solicitud);
            else
                window.open("../GestionTramite/VisorTramite_TR.aspx?id=" + solicitud);

        }

        function clearChartTareas() {
            if (myChart != null) {
                document.getElementById('myChartTareas').style.display = "none";
            }
        }

        function clearChartPersonas() {
            if (myChart != null) {
                document.getElementById('myChartPersonas').style.display = "none";
            }
        }
        function Regresar() {
            window.history.back();
        }
    </script>

</asp:Content>

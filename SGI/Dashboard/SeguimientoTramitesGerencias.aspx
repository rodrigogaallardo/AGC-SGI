<%@ Page Title="Seguimiento de Trámites Por Gerencias" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SeguimientoTramitesGerencias.aspx.cs" Inherits="SGI.Dashboard.SeguimientoTramitesGerencias" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>
    <%: Scripts.Render("~/bundles/chartjs") %>
    <%: Scripts.Render("~/bundles/gritter") %>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <asp:UpdatePanel runat="server" ID="updHids" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hidTramites" runat="server" />
            <asp:HiddenField ID="hidIdGerencia" runat="server" />
            <asp:HiddenField ID="hidDescGerencia" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <hgroup class="title">
        <h1>Seguimiento de Trámites Por Gerencias</h1>
    </hgroup>

    <div id="divBotones">
        <div class="pull-right">
            <asp:UpdatePanel runat="server" ID="updbtnDetalle" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="btnDetalle" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnDetalle_Click" Style="display: none;">
                    <i class="icon-white icon-search"></i>
                    <span class="text">Detalle</span>
                    </asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn" OnClick="btnVolver_Click">
                <span class="text">Regresar</span>
            </asp:LinkButton>
            <asp:HiddenField ID="hidUrlAnteriror" runat="server" />
            <br />
            <br />
        </div>
    </div>

    <div id="box_resultados" class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px; display: none;">

        <div class="accordion-heading">
            <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-stats" style="color: #344882;"></i></span>
                    <h5>
                        <asp:Label ID="lblTramites" runat="server" Text=""></asp:Label></h5>
                    <span class="btn-right"><i class="imoon imoon-chevron-up" style="color: #344882;"></i></span>
                </div>
            </a>
        </div>
        <div class="accordion-body collapse in" id="collapse_bt_tramite">
            <div class="widget-content">
                <asp:UpdatePanel runat="server" ID="updResultado">
                    <ContentTemplate>
                        <div class="control-group pleft20 ptop10 pright25">
                            <div style="position: relative; width: 40%; padding-left: 30%; text-align: justify;" id="DivCanvas" runat="server">
                                <canvas id="myChart" style="display: block"></canvas>
                            </div>
                        </div>

                        <div class="control-group pleft20 ptop10 pright25">
                            <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false" Visible="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="id_gerencia" HeaderText="Id" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" Visible="false" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Gerencia" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                    <asp:TemplateField HeaderText="Cantidad" ItemStyle-Width="75px" ItemStyle-CssClass="align-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DetalleGraf" runat="server" NavigateUrl="#" Text='<%# Eval("Cantidad") %>' OnClick="DetalleGraf_Click"
                                                CommandArgument='<%# Eval("id_gerencia") + "," + Eval("Descripcion") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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

    <div id="box_Detalles" class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px; display: none;">
        <%-- titulo collapsible --%>
        <div class="accordion-heading">
            <a id="bt_detalle_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_detalle"
                data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-stats" style="color: #344882;"></i></span>
                    <h5>
                        <asp:Label ID="Label1" runat="server" Text="Detalle"></asp:Label></h5>
                    <span class="btn-right"><i class="imoon imoon-chevron-up" style="color: #344882;"></i></span>
                </div>
            </a>
        </div>
        <%-- contenido del collapsible --%>
        <div class="accordion-body collapse in" id="collapse_bt_detalle">
            <div class="widget-content">
                <asp:UpdatePanel ID="updExcel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="pull-right">
                            <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click" OnClientClick="return showfrmExportarExcel();" CssClass="link-local pright20" Style="font-size: 14px;">
                            <i class="imoon imoon-file-excel color-green"></i>
                            <span>Exportar a Excel</span>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="updDetalles">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <fieldset>
                                <div class="control-group">
                                    <asp:Label ID="lblDescGerencia" Style="font-weight: bold; font-size: 16px; color: #555555" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                            </fieldset>
                            <asp:GridView ID="grdSolicitudes" runat="server" AutoGenerateColumns="false" OnSorting="grdSolicitudes_Sorting"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check" AllowSorting="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="Solicitud" >
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

    <%--Modal Errores--%>
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="updfrmerror" runat="server">
                        <ContentTemplate>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="frmerrortitle" runat="server" Text="Error"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <i class="imoon imoon-remove-circle fs64" style="color: #f00"></i>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" Style="color: black"></asp:Label>
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
    <%--Fin Modal Errores--%>

    <script type="text/javascript">

        $(document).ready(function () {

        });

        function showfrmError() {
            $("#frmError").modal("show");
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
            try {
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
                    },
                    options: {
                        responsive: true,
                        onClick: graphClickEvent,
                        legend: {
                            display: true,
                            labels: {
                                fontSize: 14
                            }
                        },
                        tooltips: {
                            bodyFontSize: 14
                        }
                    }
                });
            }
            catch (err) {
                alert("Graficar error: " + err.message);
            }
        }

        function clearChart() {
            //alert("clearChart");
            if (myChart != null) {
                document.getElementById('myChart').style.display = "none";
            }
        }

        function showResultados() {
            $("#box_resultados").show("slow");
        }

        function hideResultados() {
            $("#box_resultados").hide();
        }

        function showDetalles() {
            $("#box_Detalles").show("slow");
        }

        function hideDetalles() {
            $("#box_Detalles").hide();
        }

        function graphClickEvent(event, array) {
            try {
                var activePoints = myChart.getElementsAtEvent(event);
                var firstPoint = activePoints[0];

                var id_ger = myChart.data.labels[firstPoint._index].split("-")[0];
                document.getElementById("MainContent_hidIdGerencia").value = id_ger;

                var desc = myChart.data.labels[firstPoint._index].split("-")[1];
                document.getElementById("MainContent_hidDescGerencia").value = desc;

                document.getElementById("<%=btnDetalle.ClientID %>").click();
            }
            catch (err) {
                //alert("graphClickEvent error: " + err.message);
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

    </script>


</asp:Content>

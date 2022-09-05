<%@ Page Title="Seguimiento de Trámites – DGHP" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeguimientoTramites.aspx.cs" Inherits="SGI.Dashboard.SeguimientoTramites" %>

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


    <hgroup class="title">
        <h1>Seguimiento de Trámites – DGHP</h1>
    </hgroup>

    <div id="divBotones">
        <div class="pull-right">
            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnBuscar_Click" Visible="false">
                    <i class="icon-white icon-search"></i>
                    <span class="text">Buscar</span>
            </asp:LinkButton>
            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_Click">
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
            </asp:LinkButton>
        </div>
        <br />
        <br />
    </div>

    <div id="box_resultados" class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px; display: none;">

        <div class="accordion-heading">
            <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-stats" style="color: #344882;"></i></span>
                    <h5>Trámites</h5>
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
                            <div style="text-align: center;" id="DivBotonesCanvas">
                                <asp:HiddenField ID="hidUrlGrfGerencias" runat="server" />
                                <asp:HiddenField ID="hidUrlGrfTipos" runat="server" />
                                <asp:Button ID="btnGraficoGerencias" runat="server" CssClass="btn btn-primary" Text="Gráfico Por Gerencias" OnClick="btnGraficoGerencias_Click" />
                                <asp:Button ID="btnGraficoTipos" runat="server" CssClass="btn btn-primary" Text="Gráfico Por Tipos de Trámite" OnClick="btnGraficoTipos_Click" />
                            </div>
                        </div>

                        <div class="control-group pleft20 ptop10 pright25">
                            <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false" Visible="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="Tramites" HeaderText="Tramites" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                    <asp:TemplateField HeaderText="Cantidad" ItemStyle-Width="75px" ItemStyle-CssClass="align-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DetalleGraf" runat="server" NavigateUrl="#" Text='<%# Eval("Cantidad") %>' OnClick="DetalleGraf_Click" CommandArgument='<%# Eval("Tramites")%>'></asp:LinkButton>
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

        function finalizarCarga() {

            return false;
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
                                fontSize: 18
                            }
                        },
                        tooltips: {
                            bodyFontSize: 18
                        }
                    }
                });
            }
            catch (err) {
                alert("Graficar error: " + err.message);
            }
        }

        function clearChart() {
            alert("clearChart");
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

        function showBotonesCanvas() {
            $("#DivBotonesCanvas").show("slow");
            $("#divBotones").show("slow");
        }

        function hideBotonesCanvas() {
            $("#DivBotonesCanvas").hide();
            $("#divBotones").hide();
        }

        function graphClickEvent(event, array) {
            try {
                var activePoints = myChart.getElementsAtEvent(event);
                var firstPoint = activePoints[0];
                var tramites = btoa(myChart.data.labels[firstPoint._index].split("-")[0]);
                var value = myChart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];

                var urlGerencias = "../Dashboard/SeguimientoTramitesGerencias.aspx?trm=" + tramites;
                document.getElementById("MainContent_hidUrlGrfGerencias").value = urlGerencias;

                var urlTipos = "../Dashboard/SeguimientoTramitesTipos.aspx?trm=" + tramites;
                document.getElementById("MainContent_hidUrlGrfTipos").value = urlTipos;

                showBotonesCanvas();
            }
            catch (err) {
                //alert("graphClickEvent error: " + err.message);
            }
        }

    </script>
</asp:Content>

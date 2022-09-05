<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoTorta.ascx.cs" Inherits="SGI.Dashboard.Controls.ucGraficoTorta" %>

    <%: Scripts.Render("~/bundles/flot") %>
    <%: Styles.Render("~/bundles/flotCss") %>

    <style type="text/css">
    .plot_pie {
	    width: 450px;
	    height: 250px;
    }
	</style>

<table>
    <tr>
        <td>
            <div id="ph_sol_pend" class="plot_pie"></div>
        </td>
        <td> 
            <div id="ph_sol_pend_legend"></div>
        </td>
    </tr>
    <tr>
        <td style="padding-top:10px" colspan="2">
           
        </td>
    </tr>
</table>


<asp:Panel ID="SolicitudesPendienetes_detalle" runat="server">
    <asp:HiddenField ID="tarea_id" runat="server" />

    <div id="ph_sol_pend_memo"></div>

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5><asp:Label ID="tituloDetalle" runat="server"></asp:Label></h5>
        </div>

        <div class="widget-content">
    
            <asp:UpdatePanel ID="updPnlSolicitudesPendienetes_detalle" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                <asp:Button ID="btnCargarDetalle" runat="server" Text="cargar" OnClick="btnCargarDetalle_OnClick" style="display:none" />

                <asp:GridView ID="grdSolPend" runat="server" AutoGenerateColumns="false"
                    GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                   DataKeyNames="id_tramitetarea"  >

                    <Columns>

                        <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="sol.id_solicitud">
                            <ItemTemplate> 
                                <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# "~/GestionTramite/VisorTramite.aspx?id=" + Eval("id_solicitud") %>'><%# Eval("id_solicitud") %></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="FechaInicio_tramitetarea" HeaderText="Tarea creada el" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />

                        <asp:BoundField DataField="dias" HeaderText="Días transcurridos"  ItemStyle-Width="100px" ItemStyle-CssClass="align-center"/>

                    </Columns>

                    <EmptyDataTemplate>
                        <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                            <p>
                                No se encontraron trámites pendientes de asignación
                            </p>
                        </asp:Panel>                    
                    </EmptyDataTemplate>


                </asp:GridView>


                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

    </div>

</asp:Panel>


<script type="text/javascript">

    function ph_sol_pend_labelFormatter(label, series) {
        var html = "<div style='font-size:8pt; padding:2px; color:black;'>" +
                 +Math.round(series.percent) + "%</div>";
        return html;
    }


    function ph_sol_pend_legendFormatter(label, series) {
        //legenda de las series
        var cant = series.data[0][1];
        var html = "<div style='font-size:8pt;text-align:left; padding-left:5px; color:black;'>";
        if (series.link_detalle != "")
            html = "<div style='font-size:8pt;text-align:left; padding-left:5px; color:black; cursor: pointer;' onclick='visibleTorta2();'>";

        html = html + series.label + ": " + cant + " (" + Math.round(series.percent) + "%)" + "</div>";
        return html;
    }

    function cargar_ph_sol_pend() {

        var opciones = {
            legend: {
                show: true
                , container: '#ph_sol_pend_legend'
                , labelFormatter: ph_sol_pend_legendFormatter
            },
            series: {
                pie: {
                    show: true,
                    label: {
                        show: true,
                        radius: 1
                        , formatter: ph_sol_pend_labelFormatter
                        //, threshold: 0.1
                        , background: {
                            opacity: 0.5
                        }
                    }

                }
            },
            grid: {
                //hoverable: true,
                clickable: true
            },

        };

        var sol_asignadas = $("#ph_sol_pend");

        var ws_url = "<%: ResolveUrl("~/Dashboard/Dashboard.aspx") %>" + "/getDatos_solicitudes_pendientes"
        var tarea = $("#<%=tarea_id.ClientID%>").val();

        $.ajax({
            url: ws_url,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,            
            data: "{ tareaId: " + tarea +"}",
            success: function (data) {
                var placeholder_data = [];
                $.map(data.d, function (item) {
                    placeholder_data.push({ label: item.label, data: item.value, link_detalle: item.link_detalle });
                });


                $.plot(sol_asignadas, placeholder_data, opciones);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }

        });


        sol_asignadas.bind("plotclick", function (event, pos, item) {

            if (!item) {
                return;
            }

            //debugger;
            var link = item.series.link_detalle;

            if (link != "") {
                visibleTorta2();
                //window.open(link);
            }

        });
    }

    $(document).ready(function () {
        cargar_ph_sol_pend();
    });


</script>
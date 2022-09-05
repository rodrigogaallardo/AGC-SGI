<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoTorta2.ascx.cs" Inherits="SGI.Dashboard.Controls.ucGraficoTorta2" %>

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
            <div id="ph_sol_asig" class="plot_pie"></div>
        </td>
        <td> 
            <div id="ph_sol_asig_legend"></div>
        </td>
    </tr>
    <tr>
        <td style="padding-top:10px" colspan="2">
           
        </td>
    </tr>
</table>


<asp:Panel ID="SolicitudesAsignadas_detalle" runat="server" >
    <asp:HiddenField ID="tarea_id" runat="server" />

    <div id="ph_sol_asig_memo"></div>

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5>Solicitudes Asignadas</h5>
        </div>

        <div class="widget-content">

            <asp:UpdatePanel ID="updPnlSolicitudesAsignadas_detalle" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                <asp:HiddenField ID="hid_userid_detalle" runat="server" Value="fcc0df15-ee44-49ab-972a-c62f7e273dda" />
                <asp:Button ID="btnCargarDetalle" runat="server" Text="cargar" OnClick="btnCargarDetalle_OnClick" style="display:none" />

                <asp:GridView ID="grdSolAsig" runat="server" AutoGenerateColumns="false"
                    GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                   DataKeyNames="id_tramitetarea,userid"  >

                    <Columns>

                        <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="sol.id_solicitud">
                            <ItemTemplate> 
                                <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# "~/GestionTramite/VisorTramite.aspx?id=" + Eval("id_solicitud") %>'><%# Eval("id_solicitud") %></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField >
                            <HeaderTemplate>
                                <div id="id_div_calificador_Header">
                                    Calificador
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate> 
                                <div id="id_div_calificador">
                                    <%# Eval("calificador") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:BoundField DataField="FechaInicio_tramitetarea" HeaderText="Tarea creada el" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />

                        <asp:BoundField DataField="dias" HeaderText="Días transcurridos"  ItemStyle-Width="100px" ItemStyle-CssClass="align-center"/>

                    </Columns>

                    <EmptyDataTemplate>
                        <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                            <p>
                                El calificador no posee solicitudes asignadas
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

    function ph_sol_asig_labelFormatter(label, series) {
        var html = "<div style='font-size:8pt; padding:2px; color:black;'>" +
                 +Math.round(series.percent) + "%</div>";
        return html;
    }


    function ph_sol_asig_legendFormatter(label, series) {
        //legenda de las series
        var cant = series.data[0][1];
        var usuario = series.userid;
        var c = "onclick=\"clickUsuario('" + usuario + "');\"";

        var html = "<div style='font-size:8pt;text-align:left; padding-left:5px; color:black; cursor: pointer;' "+ c + ">" +
                series.label + ": " + cant + " (" + Math.round(series.percent) + "%)" +
                "</div>";
        return html;
    }

    function cargar_ph_sol_asig() {

        //debugger;
        var opciones = {
            legend: {
                show: true
                , container: '#ph_sol_asig_legend'
                , labelFormatter: ph_sol_asig_legendFormatter
            },
            series: {
                pie: {
                    show: true,
                    label: {
                        show: true,
                        radius: 1
                        , formatter: ph_sol_asig_labelFormatter
                        //, threshold: 0.1
                        , background: {
                            opacity: 0.5
                        }
                    }

                }
            },
            grid: {
                clickable: true
            },

        };

        var sol_asignadas = $("#ph_sol_asig");

        var ws_url = "<%: ResolveUrl("~/Dashboard/Dashboard.aspx") %>" + "/getDatos_solicitudes_asignadas"

        var tarea = $("#<%=tarea_id.ClientID%>").val();

        $.ajax({
            url: ws_url,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: "{ tareaId: " + tarea + "}",
            success: function (data) {
                var placeholder_data = [];
                $.map(data.d, function (item) {
                    placeholder_data.push({ label: item.label, data: item.value, userid: item.userid });
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

            var usuario = item.series.userid;
            clickUsuario(usuario);
        });

    }

    function clickUsuario(usuario) {
        $('#<%=hid_userid_detalle.ClientID%>').prop("value", usuario);
        $('#<%=btnCargarDetalle.ClientID%>').click();

        $('#<%=SolicitudesAsignadas_detalle.ClientID%>').show();
        //$('#<%=grdSolAsig.ClientID%>').find("[id*='id_div_calificador']").hide();
        //$('#<%=grdSolAsig.ClientID%>').find("[id*='id_div_calificador_Header']").hide();

        //$('#<%=grdSolAsig.ClientID%> table th:eq(1)').hide();


    }

    $(document).ready(function () {
        cargar_ph_sol_asig();
    });


</script>
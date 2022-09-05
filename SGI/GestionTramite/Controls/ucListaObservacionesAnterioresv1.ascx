<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListaObservacionesAnterioresv1.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucListaObservacionesAnterioresv1" %>

<asp:HiddenField ID="hid_loa_collapse_v1" runat="server" Value="false"/>

<asp:Panel ID="pnlObservAnterior" runat="server" Visible="false">

    <div class="accordion-group widget-box">

        <div class="accordion-heading">
            <a id="loa_btnUpDownv1" data-parent="#collapse-group" href="#collapse_obs_ant_v1" 
                data-toggle="collapse" onclick="loa_btnUpDown_collapsev1_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5><asp:Label ID="tituloControl" runat="server" Text="Observaciones Tareas Anteriores"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>        
                </div>
            </a>
        </div>


        <div  class="accordion-body collapse" id="collapse_obs_ant_v1" >

            <div class="widget-content">
                <div>
                    <div style="width: 100%;">
              <%--         CssClass="table table-bordered table-striped"--%>
                        <asp:GridView ID="grdObservTareasAnterioresv1" runat="server" 
                            AutoGenerateColumns="false" GridLines="None" 
                            CssClass="table table-bordered"
                            ItemType="ObservacionAnterioresv1" Width="100%" DataKeyNames="ID"
                            OnRowDataBound="grdObservTareasAnteriores_RowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="widget-content" HeaderText="Tarea" HeaderStyle-Width="200">
                                    <ItemTemplate>
                                        
                                        <div class="inline">
                                            <div style="text-align:center">
                                                <span><%# Item.Nombre_tarea %></span><br />
                                            </div>
                                            <div style="text-align:center">
                                                <span><%# Item.UsuarioApeNom %></span><br />
                                            </div>
                                            <div style="text-align:center">
                                                <span><%# Item.Fecha %></span>
                                            </div>
                                        </div>
    
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="widget-content" HeaderText="Observaciones">
                                    <ItemTemplate>
                                        <div>
                                            <%--CssClass="table table-striped"--%>
                                            <asp:DataList ID="dl_observ" runat="server" ItemType="Items"
                                                 RepeatDirection="Vertical" RepeatColumns="0" 
                                                 GridLines="None" RepeatLayout="Flow" Width="100%">
                                                <ItemTemplate>
                                                    <span style="color:#4f4ab8"><%# Item.Codigo %></span>
                                                    <span  class="control-label"><%# Item.Texto %></span>
                                                    <hr />
                                                </ItemTemplate>

                                            </asp:DataList>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate> 
                                <div style="width: 100%;">
                                    <i class="icon-remove"></i>
                                    <span class="text">No se encontraron observaciones de tareas anteriores.</span>
                                </div>
                            </EmptyDataTemplate>

                        </asp:GridView>  

                    </div>
                </div>
            </div>


        </div>

    </div>

</asp:Panel>
<script type="text/javascript">

    $(document).ready(function () {
        inicializar_loa_btnUpDown_collapse_v1();
    });


    function inicializar_loa_btnUpDown_collapse_v1() {
        //cuando cargua por primera vez, se muestra expandido o no segun el seteo de los atributos del control
        var colapsar = $('#<%=hid_loa_collapse_v1.ClientID%>').attr("value");

        var obj = $("#loa_btnUpDownv1")[0];
        var href_collapse = $(obj).attr("href");

        if ($(href_collapse).attr("id") != undefined) {
            if ($(href_collapse).css("height") == "0px") {
                if (colapsar == "true") {
                    $(href_collapse).collapse();
                    $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
            }
            else {
                if (colapsar == "false") {
                    $(href_collapse).collapse();
                    $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }
    }

    function loa_btnUpDown_collapsev1_click(obj) {
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


</script>



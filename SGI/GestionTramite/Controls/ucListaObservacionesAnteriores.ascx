<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListaObservacionesAnteriores.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucListaObservacionesAnteriores" %>

<script type="text/javascript">

    $(document).ready(function () {
        inicializar_loa_btnUpDown_collapse();
    });


    function inicializar_loa_btnUpDown_collapse() {
        //cuando cargua por primera vez, se muestra expandido o no segun el seteo de los atributos del control
        var colapsar = $('#<%=hid_loa_collapse.ClientID%>').attr("value");

        var obj = $("#loa_btnUpDown")[0];
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

    function loa_btnUpDown_collapse_click(obj) {
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


<asp:HiddenField ID="hid_loa_collapse" runat="server" Value="false"/>


<asp:Panel ID="pnlObservAnterior" runat="server" Visible="false">

    <div class="accordion-group widget-box">

        <div class="accordion-heading">
            <a id="loa_btnUpDown" data-parent="#collapse-group" href="#collapse_obs_ant" 
                data-toggle="collapse" onclick="loa_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5><asp:Label ID="tituloControl" runat="server" Text="Observaciones Tareas Anteriores"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>        
                </div>
            </a>
        </div>


        <div  class="accordion-body collapse" id="collapse_obs_ant" >

            <div class="widget-content">
                <div>
                    <div style="width: 100%;">
              <%--         CssClass="table table-bordered table-striped"--%>
                        <asp:GridView ID="grdObservTareasAnteriores" runat="server" 
                            AutoGenerateColumns="false" GridLines="None" 
                            CssClass="table table-bordered"
                            ItemType="ObservacionAnteriores" Width="100%" DataKeyNames="ID"
                            OnRowDataBound="grdObservTareasAnteriores_RowDataBound">
                            <Columns>
                
                                <asp:BoundField DataField="Nombre_tarea" HeaderText="Tarea" 
                                    ItemStyle-CssClass="align-center" ItemStyle-Width="150px" />

                                <asp:TemplateField ItemStyle-CssClass="widget-content" HeaderText="Observaciones">
                                    <ItemTemplate>
                                        
                                        <div>
                                        <%--CssClass="table table-striped"--%>
                                        <asp:DataList ID="dl_observ" runat="server" ItemType="Items"
                                             RepeatDirection="Vertical" RepeatColumns="0" 
                                             GridLines="None"
                                             
                                             RepeatLayout="Flow" Width="100%" >
                                             
                                            <ItemTemplate>
                                                <span style="color:#4f4ab8"><%# Item.Codigo %></span>
                                                <span  class="control-label"><%# Item.Texto %></span>
                                            </ItemTemplate>

                                        </asp:DataList>
                                        </div>
    
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" 
                                    ItemStyle-CssClass="align-center" ItemStyle-Width="70px" 
                                    DataFormatString="{0:dd/MM/yyyy}"  />
                                <asp:BoundField DataField="UsuarioApeNom" HeaderText="Usuario" 
                                    ItemStyle-CssClass="align-center" ItemStyle-Width="200px" />

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



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSGI_ListaResultadoTareasAnteriores.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucSGI_ListaResultadoTareasAnteriores" %>


<script type="text/javascript">

    $(document).ready(function () {
        inicializar_btnUpDown_collapse();
    });

    function inicializar_btnUpDown_collapse() {
        //cuando cargua por primera vez, se muestra expandido o no segun el seteo de los atributos del control
        var colapsar = $('#<%=hid_lta_collapse.ClientID%>').attr("value");
        var colapsar2 = $('#<%=hid_lta_collapse2.ClientID%>').attr("value");

        var obj = $("#lta_btnUpDown")[0];
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

        var obj2 = $("#lta_btnUpDown2")[0];
        var href_collapse2 = $(obj2).attr("href");

        if ($(href_collapse2).attr("id") != undefined) {
            if ($(href_collapse2).css("height") == "0px") {
                if (colapsar2 == "true") {
                    $(href_collapse2).collapse();
                    $(obj2).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
            }
            else {
                if (colapsar2 == "false") {
                    $(href_collapse2).collapse();
                    $(obj2).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }
    }

    function lta_btnUpDown_collapse_click(obj) {
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

    function lta_btnUpDown_collapse_click2(obj) {
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

<asp:HiddenField ID="hid_lta_collapse" runat="server" Value="false"/>

<asp:Panel ID="pnllistResulTareaAnterior" runat="server" Visible="false">

    <div class="accordion-group widget-box">

        <div class="accordion-heading">
            <a id="lta_btnUpDown" data-parent="#collapse-group" href="#collapse_resul_ant" 
                data-toggle="collapse" onclick="lta_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5><asp:Label ID="tituloControl" runat="server" Text="Resultado Tareas anteriores"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>        
                </div>
            </a>
        </div>

        <div  class="accordion-body collapse" id="collapse_resul_ant" >
             <div class="widget-content">
                <div>

                    <asp:GridView ID="grd_doc_adj_anteriores" runat="server" AutoGenerateColumns="false" 
                        DataKeyNames="id_tramitetarea"
                        GridLines="none" CssClass="table table-striped table-bordered">
                        <Columns>

                            <asp:BoundField DataField="nombre_tarea" HeaderText="Tarea" ItemStyle-Width="200px"/>

                            <asp:TemplateField HeaderText="Resultado" ItemStyle-Width="200px" >
                                <ItemTemplate>
                                    <span style="color:#4f4ab8"><b><%# Eval("nombre_resultado")%></b></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FechaCierre_tramitetarea" HeaderText="Fecha Resolución"  
                                DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="110px"/>
                            <asp:BoundField DataField="usuario" HeaderText="Usuario" />

                        </Columns>
                    </asp:GridView>

                </div>


             </div>
        </div>


    </div>

</asp:Panel>

<asp:HiddenField ID="hid_lta_collapse2" runat="server" Value="true"/>
<asp:Panel ID="pnllistaResulUltimaTarea" runat="server" Visible="false">

    <div class="accordion-group widget-box">

        <div class="accordion-heading">
            <a id="lta_btnUpDown2" data-parent="#collapse-group" href="#collapse_resul_ant2" 
                data-toggle="collapse" onclick="lta_btnUpDown_collapse_click2(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5><asp:Label ID="titulo" runat="server" Text="Resultado Última Tarea"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>        
                </div>
            </a>
        </div>

        <div  class="accordion-body collapse" id="collapse_resul_ant2" >
             <div class="widget-content">
                <div>

                    <asp:GridView ID="grdTareaAnterior" runat="server" AutoGenerateColumns="false" 
                        DataKeyNames="id_tramitetarea"
                        GridLines="none" CssClass="table table-striped table-bordered">
                        <Columns>

                            <asp:BoundField DataField="nombre_tarea" HeaderText="Tarea" ItemStyle-Width="200px"/>

                            <asp:TemplateField HeaderText="Resultado" ItemStyle-Width="200px" >
                                <ItemTemplate>
                                    <span style="color:#4f4ab8"><b><%# Eval("nombre_resultado")%></b></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FechaCierre_tramitetarea" HeaderText="Fecha Resolución"  
                                DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="110px"/>
                            <asp:BoundField DataField="usuario" HeaderText="Usuario" />

                        </Columns>
                    </asp:GridView>

                </div>


             </div>
        </div>


    </div>

</asp:Panel>


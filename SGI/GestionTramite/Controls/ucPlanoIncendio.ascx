<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPlanoIncendio.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucPlanoIncendio" %>


<asp:HiddenField ID="hid_loa_collapse" runat="server" Value="false"/>

<asp:Panel ID="pnlPlanoIncendio" runat="server" Visible="true">

    <div class="accordion-group widget-box">
        <div class="accordion-heading">

            <a id="loa_btnUpDown" data-parent="#collapse-group" href="#collapse_pl_incen"
                data-toggle="collapse" onclick="loa_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="tituloControl" runat="server" Text="Planos de Incendio"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>
                </div>
            </a>
        </div>

        <div class="accordion-body collapse" id="collapse_pl_incen">
            <div class="widget-content">
                <div>
                    <div style="width: 100%;">
                        <asp:GridView ID="grdPlanoIncendio" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                            <Columns>
                                <asp:BoundField DataField="id_planoincendio" Visible = "false" />
                                <asp:BoundField DataField="nombre_tarea" HeaderText="Tarea" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="nombre_plano" HeaderText="Tipo de Plano" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="fechaPlano" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="UsuarioApeNom" HeaderText="Usuario" ItemStyle-CssClass="align-center" />
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
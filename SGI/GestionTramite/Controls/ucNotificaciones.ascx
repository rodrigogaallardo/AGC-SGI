<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNotificaciones.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucNotificaciones" %>

<asp:HiddenField ID="hid_lNot_collapse" runat="server" Value="false"/>

<div class="accordion-group widget-box">
    <div class="accordion-heading">
        <a id="btnUpDownNot" data-parent="#collapse-group" href="#collapseNoti" 
            data-toggle="collapse" onclick="btnUpDownNot_click(this)">

            <div class="widget-title">
                <span class="icon"><i class="icon-th-list"></i></span>
                <h5>Notificaciones</h5>
                <span class="btn-right"><i class="icon-chevron-up"></i></span>        
            </div>
        </a>

    </div>
    <div  class="accordion-body collapse in" id="collapseNoti" >
        <div class="widget-content">
            <asp:UpdatePanel ID="updPnlNotificaciones" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfMailID" runat="server" />
                <asp:GridView ID="grdBuscarMails" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                DataKeyNames="Mail_ID" ItemType="SGI.Model.clsItemGrillaBuscarMails" OnRowDataBound="grdBuscarMails_RowDataBound">
                <Columns>
                    <asp:BoundField Visible="false" DataField="Mail_ID" HeaderText="ID" />
                    <%--<asp:BoundField DataField="Mail_Estado" HeaderText="Estado" />--%>
                    <%--<asp:BoundField DataField="Mail_Proceso" HeaderText="Proceso" />--%>
                    <asp:BoundField DataField="Mail_Asunto" HeaderText="Asunto" ItemStyle-Width="300px"/>
                    <asp:BoundField DataField="Mail_Email" HeaderText="E-Mail" ItemStyle-Width="100px"/>
                    <asp:BoundField DataField="Mail_Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                    <asp:BoundField DataField="MailFechaNot_FechaSSIT" HeaderText="F. Notificación" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                    <asp:TemplateField ItemStyle-Width="15px" HeaderText="Ver" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetalles" runat="server" ToolTip="Ver Detalle" CssClass="link-local" CommandArgument='<%#Eval("Mail_ID")%>' OnClick="lnkDetalles_Click">
                                <i class="icon-eye-open" style="transform: scale(1.1);"></i>
                            </asp:LinkButton>
                            <asp:Panel ID="pnlDetalle" runat="server" class="modal fade" data-backdrop="static" Style="display: none;min-width: 90%; left: 350px; top:0px;height:100%">
                                <div class="modal-dialog" role="document" style="height: 100%">
                                    <div class="modal-content" style="height: 100%">
                                        <div class="modal-header">
                                            <a class="close" data-dismiss="modal">×</a>
                                            <h3>Detalle del E-Mail</h3>
                                        </div>
                                        <div class="modal-body" style="height: 100%">
                                            <asp:Table ID="Table1" runat="server" HorizontalAlign="Center" Font-Size="8px" Width="100%"  >
                                                <asp:TableHeaderRow VerticalAlign="Middle" HorizontalAlign="Center">
                                                    <asp:TableHeaderCell Visible="false">ID</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Email</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Asunto</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Proceso</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Fecha de Alta</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Fecha de Envio</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Prioridad</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Font-Bold="true">Cant. de Intentos</asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Visible="false" ID="IDCorreo"></asp:TableCell>
                                                    <asp:TableCell ID="Email"></asp:TableCell>
                                                    <asp:TableCell ID="Asunto"></asp:TableCell>
                                                    <asp:TableCell ID="Proceso"></asp:TableCell>
                                                    <asp:TableCell ID="FecAlta"></asp:TableCell>
                                                    <asp:TableCell ID="FecEnvio"></asp:TableCell>
                                                    <asp:TableCell ID="Prioridad"></asp:TableCell>
                                                    <asp:TableCell ID="CantInt"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableHeaderRow VerticalAlign="Middle" HorizontalAlign="Center">
                                                    <asp:TableHeaderCell ColumnSpan="8" Font-Bold="true">Mensaje</asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                        
                                            </asp:Table>
                                    
                                            <iframe style="width: 100%; height: 70%; border-style: none;" id="Message" runat="server"></iframe>

                                        </div>
                                    </div>
                                </div>
                                      
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                        <img src="../Content/img/app/NoRecords.png" />No se encontraron registros.
                    </asp:Panel>
                </EmptyDataTemplate>
            </asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {
        //inicializar_lNot_btnUpDown_collapse();
    });


<%--    function inicializar_lNot_btnUpDown_collapse() {
        //cuando cargua por primera vez, se muestra expandido o no segun el seteo de los atributos del control
        var colapsar = $('#<%=hid_lNot_collapse.ClientID%>').attr("value");

        var obj = $("#btnUpDownNot")[0];
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
    }--%>

    function btnUpDownNot_click(obj) {
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSGI_ListaDocumentoAdjuntoAnteriores.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucSGI_ListaDocumentoAdjuntoAnteriores" %>

<script type="text/javascript">

    $(document).ready(function () {
        inicializar_ldaa_btnUpDown_collapse();
    });

    function tda_confirm_del() {
        return confirm('¿Esta seguro que desea eliminar este Registro?');
    }

    function inicializar_ldaa_btnUpDown_collapse() {
        //cuando cargua por primera vez, se muestra expandido o no segun el seteo de los atributos del control
        var colapsar = $('#<%=hid_ldaa_collapse.ClientID%>').attr("value");

        var obj = $("#ldaa_btnUpDown")[0];
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

    function ldaa_btnUpDown_collapse_click(obj) {
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

<asp:HiddenField ID="hid_ldaa_collapse" runat="server" Value="false" />

<asp:Panel ID="pnlDosAdjAnterior" runat="server" Visible="false">


    <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
    <asp:HiddenField ID="hid_id_grupotramite" runat="server" Value="0" />
    <asp:HiddenField ID="hid_id_tramitetarea" runat="server" Value="0" />
    <asp:HiddenField ID="hid_editable" runat="server" Value="true" />

    <div class="accordion-group widget-box">

        <div class="accordion-heading">
            <a id="ldaa_btnUpDown" data-parent="#collapse-group" href="#collapse_doc_adj_ant"
                data-toggle="collapse" onclick="ldaa_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="tituloControl" runat="server" Text="Lista de Documentos Adjuntados Anteriormente"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>
                </div>
            </a>
        </div>

        <div class="accordion-body collapse" id="collapse_doc_adj_ant">

            <div class="widget-content">
                <div>
                    <asp:UpdatePanel ID="updPnlDosAdjAnterior" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grd_doc_adj_anteriores" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="id_doc_adj, id_tdocreq"
                                GridLines="none" CssClass="table table-striped table-bordered"
                                OnRowDataBound="grd_doc_adj_anteriores_RowDataBound">
                                <Columns>

                                    <asp:BoundField DataField="nombre_tarea" HeaderText="Tarea"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />

                                    <asp:BoundField DataField="tdoc_adj_detalle" HeaderText="Tipo de Documento"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="CreateDate" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkVerDoc" runat="server"
                                                CssClass="btnVerPdf20" Target="_blank"
                                                NavigateUrl='<%# ResolveUrl("~/Reportes/Imprimir_SGI_Documentos_Adjuntos.aspx?id=" + Eval("id_doc_adj")) %>'
                                                Text="Ver" Width="40px">
                                            </asp:HyperLink>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminarDocAdj" runat="server"
                                                CommandArgument='<%# Eval("id_doc_adj") %>'
                                                OnClientClick="javascript:return tda_confirm_del();"
                                                OnCommand="lnkEliminarDocAdj_Command"
                                                Width="70px">
                                            <i class="icon icon-trash"></i> 
                                            <span class="text">Eliminar</span></a>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>

        </div>

    </div>


</asp:Panel>




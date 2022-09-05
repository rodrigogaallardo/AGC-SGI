<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSGI_ListaPlanoVisado.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucSGI_ListaPlanoVisado" %>


<div class="accordion-group widget-box">

    <div class="accordion-heading">
        <a id="btnUpDown" data-parent="#collapse-group" href="#collapse_doc_adj"
            data-toggle="collapse" onclick="tda_btnUpDown_collapse_click(this)">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>
                    <asp:Label ID="tituloControl" runat="server" Text="Lista de Plano/s Visado/s"></asp:Label></h5>
                <span class="btn-right"><i class="icon-chevron-down"></i></span>
            </div>
        </a>
    </div>

    <div class="accordion-body collapse in" id="collapse_doc_adj">

        <div class="widget-content">


            <div>

                <asp:Panel ID="pnlPlanVisado" runat="server" Visible="true">

                    <asp:UpdatePanel ID="updPnlPlanVisado" runat="server">
                        <ContentTemplate>

                            <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
                            <asp:HiddenField ID="hid_id_tramitetarea" runat="server" Value="0" />
                            <asp:HiddenField ID="hid_id_grupotramite" runat="server" Value="0" />
                            <asp:HiddenField ID="hid_editable" runat="server" Value="true" />

                            <asp:GridView ID="grd_plan_visado" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="id_doc_adj,id_tdocreq,id_solicitud,id_file"
                                GridLines="none" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:TemplateField HeaderText="Archivo">
                                        <ItemTemplate>

                                            <asp:HyperLink ID="lnkArchivoPdf" runat="server" CssClass="btn-link" Target="_blank" NavigateUrl='<%#Eval("url")%>'>
                                        <i class="imoon imoon-file-pdf color-red"></i>
                                        <span class="text"><%#Eval("nombre") %></span>
                                            </asp:HyperLink>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="id_file" HeaderText="Nº Archivo" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Plano Visado" ItemStyle-CssClass="text-center" ItemStyle-Width="70px">
                                        <ItemTemplate>

                                            <asp:CheckBox ID="chkPlanoVisado" runat="server" OnCheckedChanged="chkPlanoVisado_CheckedChanged"
                                                AutoPostBack="true" Checked='<%# Eval("esPlanoVisado") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-block">
                                        <span class="text">No existen documentos.</span>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>                    
                </asp:Panel>
                <asp:UpdatePanel ID="updGuardarTarea" runat="server" RenderMode="Inline" class="pull-left">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" CssClass="btn pull-rightt" Text="Guardar"
                            Style="margin-left: 5px; margin-top: 5px" />

                        <asp:UpdateProgress ID="progGuardarTarea" runat="server" AssociatedUpdatePanelID="updGuardarTarea" class="pull-rightt">
                            <ProgressTemplate>
                                <asp:Image ID="imgProgGuardarTarea" runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif" />Procesando...
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div style="border-top: solid 1px #dddddd; padding-top: 15px; padding-bottom: 15px">
                </div>

            </div>

        </div>



    </div>

</div>
    <%--Modal mensajes de error--%>
<div id="frmError_AnexoTecnico" class="modal fade" style="display: none;">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Error</h4>
            </div>
            <div class="modal-body">
                <table style="border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="text-align: center; vertical-align: text-top">
                            <label class="imoon imoon-remove-circle fs64" style="color: #f00"></label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
                                <ContentTemplate>
                                    <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
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
<!-- /.modal -->



<script type="text/javascript">

    $(document).ready(function () {



    });


    function showfrmError_AnexoTecnico() {
        
        $("#frmError_AnexoTecnico").modal("show");
        return false;

    }


</script>
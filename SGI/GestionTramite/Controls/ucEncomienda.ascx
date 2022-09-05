<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEncomienda.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucEncomienda" %>

<%: Scripts.Render("~/bundles/autoNumeric") %>

<asp:UpdatePanel ID="Hiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_solicitud" runat="server" />       
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>


<div id="page_content">
    
   <div class="accordion-group widget-box">

        <%-- titulo box Rubros--%>
        <div class="accordion-heading">
            <a id="A1" data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-file"></i></span>
                    <h5>Anexo T&eacutecnico</h5>
                </div>
            </a>
        </div>
       
        <%-- contenido del box Rubros --%>
        <div id="box_AnexoTecnico" class="accordion-body collapse in">
            <div class="widget-content">


                <asp:GridView ID="grdAnexoTecnico" runat="server" AutoGenerateColumns="false"
                    AllowPaging="false" CssClass="table table-bordered mtop5"
                    GridLines="None" Width="100% ">
                    <HeaderStyle CssClass="grid-header" />
                    <RowStyle CssClass="grid-row" />
                    <AlternatingRowStyle BackColor="#efefef" />

                    <Columns>
                        <asp:BoundField DataField="IdEncomienda" HeaderText="Anexo" ItemStyle-Width="70px" ItemStyle-CssClass="align-center" />
                        <asp:BoundField DataField="tipoAnexo" HeaderText="Tipo" ItemStyle-CssClass="align-center" ItemStyle-Width="90px"/>
                        <asp:BoundField DataField="CreateDate" DataFormatString="{0:d}" HeaderText="Fecha" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="DescripcionTipoTramite" HeaderText="Tipo Trámite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="NomEstado" HeaderText="Estado" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                        <asp:BoundField DataField="Profesional" HeaderText="Profesional" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                        
                    </Columns>
                    <EmptyDataTemplate>

                        <div class="mtop10">

                            <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                            <span class="mleft10">No se encontraron registros.</span>

                        </div>

                    </EmptyDataTemplate>
                </asp:GridView>

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

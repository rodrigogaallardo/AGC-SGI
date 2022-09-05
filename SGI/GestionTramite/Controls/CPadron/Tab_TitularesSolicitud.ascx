<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_TitularesSolicitud.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tab_TitularesSolicitud" %>

<asp:UpdatePanel ID="updHiddens" runat="server">
    <ContentTemplate>

        <asp:HiddenField ID="hid_id_cpadron" runat="server" />
        <asp:HiddenField ID="hid_id_encomienda" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_CargosFirPJ" runat="server" />
        <asp:HiddenField ID="hid_CargosFirSH" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
        <asp:HiddenField ID="hid_editar" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>


<div id="page_content_Titulares">

    <div id="titulo" runat="server" class="accordion-group widget-box">
        <h3 style="line-height: 20px;">Titulares de Solicitud</h3>
    </div>

    <div id="box_titularesSol" class="accordion-group widget-box">

        <div class="accordion-heading">
            <a data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-users"></i></span>
                    <h5>
                        <asp:Label ID="Label1" runat="server" Text="Titulares"></asp:Label></h5>

                </div>
            </a>
        </div>

        <div class="accordion-body collapse in" id="collapse_titulares">
            <div class="widget-content">

                <asp:UpdatePanel ID="updGrillaTitularesSol" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <%--Grilla de Titulares--%>
                        <div>
                            <strong>Titulares</strong>
                        </div>
                        <div>
                            <asp:GridView ID="grdTitularesSol" runat="server" AutoGenerateColumns="false" DataKeyNames="id_persona"
                                AllowPaging="false" GridLines="None" Width="100%" CssClass="table table-bordered mtop5"
                                CellPadding="3">
                                <HeaderStyle CssClass="grid-header" />
                                <AlternatingRowStyle BackColor="#efefef" />
                                <Columns>

                                    <asp:BoundField DataField="TipoPersonaDesc" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ApellidoNomRazon" HeaderText="Apellido y Nombre / Razon Social"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Cuit" HeaderText="CUIT" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" HeaderStyle-HorizontalAlign="Left" />

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="mtop10">

                                        <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                        <span class="mleft10">No se encontraron registros.</span>

                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>

                        <asp:HiddenField ID="hid_tipopersona_eliminar" runat="server" />
                        <asp:HiddenField ID="hid_id_persona_eliminar" runat="server" />

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</div>

<%--Modal mensajes de error--%>
<div id="frmError_TitularesSol" class="modal fade" style="display: none;">
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

    var id_tipodoc_pasaporte = "<%: (int) SGI.Constants.TipoDocumentoPersonal.PASAPORTE %>";

    $(document).ready(function () {
        toolTips_TitularesSol();
        init_JS_updGrillaTitulares_TitularesSol();
    });


    function toolTips_TitularesSol() {
        $("[data-toggle='tooltip']").tooltip();
        return false;

    }

    function showfrmError_TitularesSol() {
        $("#frmError_TitularesSol").modal("show");
        return false;
    }

    function init_JS_updGrillaTitulares_TitularesSol() {
    }


</script>



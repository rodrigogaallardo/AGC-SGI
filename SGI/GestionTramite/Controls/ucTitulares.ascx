<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTitulares.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucTitulares" %>

<asp:UpdatePanel ID="updHiddens" runat="server">
    <ContentTemplate>

        <asp:HiddenField ID="hid_id_solicitud" runat="server" />
        <asp:HiddenField ID="hid_id_encomienda" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_CargosFirPJ" runat="server" />
        <asp:HiddenField ID="hid_CargosFirSH" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
        <asp:HiddenField ID="hid_editar" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<div id="page_content_Titulares">


    <div id="box_MostrarTitulares" class="accordion-group widget-box" runat="server">

        <div class="accordion-heading">
            <a data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-users"></i></span>
                    <h5>
                        <asp:Label ID="Label2" runat="server" Text="Titulares"></asp:Label></h5>

                </div>
            </a>
        </div>


        <div class="accordion-body collapse in" id="titulares">
            <div class="widget-content">


                <div>
                    <asp:GridView ID="grdTitulares" runat="server" AutoGenerateColumns="false" DataKeyNames="id_persona"
                        AllowPaging="false" GridLines="None" Width="100%"
                        CssClass="table table-bordered table-striped table-hover with-check"
                        CellPadding="3">
                        <HeaderStyle CssClass="grid-header" />
                        <AlternatingRowStyle BackColor="#efefef" />
                        <Columns>

                            <asp:TemplateField HeaderText="Personería" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <i class='<%#Eval("TipoPersona").ToString() == "PF" ? ("imoon imoon-user") : ("imoon imoon-office") %>' style="font-size: medium; margin-left: 5px"></i>
                                    <span>'<%#Eval("TipoPersonaDesc") %>'</span>
                                </ItemTemplate>
                            </asp:TemplateField>
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


            </div>
        </div>


    </div>

</div>




<%--Modal mensajes de error--%>
<div id="frmError_Titulares" class="modal fade" style="margin-left: -25%; width: 50%; display: none;" role="dialog">
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
        toolTips_Titulares();
        init_JS_updGrillaTitulares_Titulares();
        //init_JS_updGrillaTitulares();
        //init_JS_upd_ddlTipoIngresosBrutosPF();
        //init_JS_upd_txtIngresosBrutosPF();
        //init_JS_updLocalidadPF();
        //init_JS_updProvinciasPF();
        //init_JS_updAgregarPersonaJuridica();
        //init_JS_updAgregarPersonaFisica();
        //init_JS_upd_ddlTipoIngresosBrutosPJ();
        //init_JS_upd_txtIngresosBrutosPJ();
        //init_JS_updLocalidadPJ();
        //init_JS_updProvinciasPJ();
        //init_JS_upd_ddlTipoSociedadPJ();
        //init_JS_upd_txtRazonSocialPJ();
        //Falta cargarDatos
    });

    function init_JS_updGrillaTitulares() {
    }


    function toolTips_Titulares() {
        $("[data-toggle='tooltip']").tooltip();

        return false;
    }

    function showfrmError_Titulares() {
        $("#frmError_Titulares").modal("show");
        return false;
    }

    function init_JS_updGrillaTitulares_Titulares() {
        //$("#<: optMismaPersona.ClientID %>").on("click", function () {
        //    $("#<: pnlOtraPersona.ClientID %>").hide("slow");
        //});

        //$("#<: optOtraPersona.ClientID %>").on("click", function () {
        //    $("#<: pnlOtraPersona.ClientID %>").show("slow");
        //});
    }


    function showfrmAgregarPersonaFisica_Titulares() {
        $("#frmAgregarPersonaFisica").modal({
            "show": true,
            "backdrop": "static"
        });
        return false;
    }

    function hidefrmAgregarPersonaFisica() {
        $("#frmAgregarPersonaFisica").modal("hide");
        return false;
    }

    function showfrmAgregarPersonaJuridica() {

        $("#frmAgregarPersonaJuridica").modal({
            "show": true,
            "backdrop": "static"

        });
        return false;
    }
    function hidefrmAgregarPersonaJuridica() {
        $("#frmAgregarPersonaJuridica").modal("hide");
        return false;
    }


    function hideTitulares() {
        $("#box_titulares").hide("slow");
    }

    function hideMostrarTitulares() {
        $("#box_MostrarTitulares").hide("slow");
    }

</script>


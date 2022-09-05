<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_Ubicaciones.ascx.cs" Inherits="SGI.GestionTramite.Controls.Transferencias.Tab_Ubicaciones" %>

<%@ Register Src="~/GestionTramite/Controls/Transferencia/BuscarUbicacion.ascx" TagPrefix="uc" TagName="BuscarUbicacion" %>
<%@ Register Src="~/GestionTramite/Controls/Transferencia/Ubicacion.ascx" TagPrefix="uc" TagName="Ubicacion" %>

<asp:UpdatePanel ID="updUbicaciones" runat="server">
    <ContentTemplate>

        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_id_solicitud" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />

        <h3 style="line-height: 20px;">Ubicaci&oacute;n</h3>
       <asp:UpdatePanel ID="updAgregarUbicacion" runat="server">
        <ContentTemplate>
        <div class="flow-row">
            <div class="">
                <span class="btn btn-success mtop30" onclick="showfrmAgregarUbicacion();" data-group="controles-accion">
                    <i class="imoon imoon-plus"></i>
                    <span class="text">Agregar Ubicaci&oacute;n</span>
                </span>
            </div>
        </div>
        </ContentTemplate>
       </asp:UpdatePanel>

        <div id="box_ubicacion" class="accordion-group widget-box">

            <%-- titulo collapsible ubicaciones--%>
            <div class="accordion-heading">
                <a id="ubicacion_btnUpDown" data-parent="#collapse-group">
                    <div class="widget-title">
                        <span class="icon"><i class="imoon imoon-map-marker"></i></span>
                        <h5>
                            <asp:Label ID="lbl_ubicacion_tituloControl" runat="server" Text="Ubicaciones ingresadas"></asp:Label></h5>
                    </div>
                </a>
            </div>
            <%-- contenido del collapsible ubicaciones --%>
            <div class="accordion-body collapse in" id="collapse_ubicacion">
                <div class="widget-content">
                    <uc:Ubicacion runat="server" ID="visUbicaciones" OnEliminarClick="visUbicaciones_EliminarClick" OnEditarClick="visUbicaciones_EditarClick" />
                </div>
            </div>

        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="updPlantas" runat="server" UpdateMode="Conditional">
    <ContentTemplate>


        <div class="accordion-group widget-box">

            <%-- Titulo del panel de plantas a habilitar --%>
            <div class="accordion-heading">
                <a id="A1" data-parent="#collapse-group">

                    <div class="widget-title">
                        <span class="icon"><i class="imoon imoon-stackexchange"></i></span>
                        <h5>
                            <asp:Label ID="Label1" runat="server" Text="Plantas"></asp:Label></h5>
                    </div>
                </a>
            </div>

            <%-- Contenido del panel de plantas a habilitar --%>
            <div class="accordion-body collapse in">
                <div class="widget-content">

                    <asp:Panel ID="pnlPlantasHabilitar" runat="server" CssClass="mtop5">

                        <strong class="pleft10">Seleccione las plantas:</strong>


                        <div class="row pleft40 mtop10">

                            <div class="control-group span8">

                                <asp:GridView ID="grdPlantasHabilitar" runat="server" AutoGenerateColumns="false"
                                    GridLines="None" CellPadding="3" ShowHeader="false" OnRowDataBound="grdPlantasHabilitar_OnRowDataBound"
                                    DataKeyNames="id_transftiposector" Enabled="true"  >
                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <div class="checkbox" style="margin-top: 1px">
                                                    <label style="width: 100px">
                                                        <asp:CheckBox ID="chkSeleccionado" runat="server" Checked='<% #Eval("Seleccionado") %>'
                                                            OnCheckedChanged="chkSeleccionado_CheckedChanged" AutoPostBack="true" />
                                                        <label><%# Eval("Descripcion") %></label>
                                                        <asp:HiddenField ID="hid_id_tiposector" runat="server" Value='<% #Eval("id_tiposector") %>' />
                                                        <asp:HiddenField ID="hid_descripcion" runat="server" Value='<% #Eval("Descripcion") %>' />
                                                    </label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="form-inline">
                                                    <div class="control-group">
                                                        <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control mtop5 mbottom5" MaxLength='<% #Eval("TamanoCampoAdicional") %>'
                                                            Visible='<% #Eval("MuestraCampoAdicional") %>' Width="100px" Text='<% #Eval("detalle") %>'></asp:TextBox>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Panel ID="ReqtxtDetalle" runat="server" CssClass="alert alert-danger pad5 mbottom5 mtop5 mleft5" Style="display: none">
                                                            Debe ingresar la aclaración del item.
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                            <div class="control-group span4">

                                <div>Opciones con información adicional:</div>
                                <div style="padding-top: 10px"><b>Piso:</b> En esta opción deberá indicar la aclaración del piso. Ej: Piso 2, Piso 3</div>
                                <div style="padding-top: 10px"><b>Otro:</b> En esta opción deberá indicar la descripcón deaseada, alguna no incluída en la lista ofrecida.</div>
                                <div style="padding-top: 10px">
                                    <b>Nota:</b> En los campos "Otro" <b>NO</b> deberá ingresar información de otra cosa que no sea una planta a habilitar. <b>NO</b> indicar unidades funcionales, departamentos, locales o cualquier referencia
                                    a la ubicación.
                                </div>

                            </div>

                        </div>


                    </asp:Panel>

                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<%--Botones de Guardado--%>
<asp:UpdatePanel ID="updBotonesGuardar" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div class="form-inline text-center mtop20">
            <div id="pnlBotonesGuardar" class="form-group">

                <asp:LinkButton ID="btnGuardar2" runat="server" OnClick="btnGuardar_Click" CssClass="btn btn-primary" style="color:white" 
                    OnClientClick="return validarGuardar()" data-group="controles-accion">
                    <i class="imoon imoon-disk"></i>
                    <span class="text">Guardar Plantas</span>
                </asp:LinkButton>

            </div>
            <div class="form-group">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updBotonesGuardar">
                    <ProgressTemplate>
                        <img src='<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>' style="margin-left: 10px" alt="loading" />Guardando...
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<%--Modal mensajes de error--%>
<div id="frmError" class="modal fade" style="display: none;">
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

<%--Modal Confirmar Eliminación--%>
<div id="frmConfirmarEliminar" class="modal fade" style="display: none;">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Eliminar ubicaci&oacute;n</h4>
            </div>
            <div class="modal-body">
                <table style="border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="text-align: center; vertical-align: text-top">
                            <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                        </td>
                        <td style="vertical-align: middle">
                            <label class="mleft10">¿ Est&aacute; seguro de eliminar esta ubicaci&oacute;n ?</label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="modal-footer">

                <asp:UpdatePanel ID="updConfirmarEliminar" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updConfirmarEliminar">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesConfirmacion" class="form-group">
                                <asp:Button ID="btnEliminar_Si" runat="server" CssClass="btn btn-primary" Text="Sí" OnClick="btnEliminar_Si_Click" OnClientClick="ocultarBotonesConfirmacion();" />
                                <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>
<!-- /.modal -->


<%-- Modal Agregar Ubicación --%>
<div id="frmAgregarUbicacion" class="modal fade" style="display: none; width: 950px;">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Buscar Ubicaci&oacute;n</h4>
            </div>
            <div class="modal-body">

                <uc:BuscarUbicacion runat="server" id="BuscarUbicacion"
                    oncerrarclientclick="hidefrmAgregarUbicacion();" onagregarubicacionclick="BuscarUbicacion_AgregarUbicacionClick" />
            </div>
        </div>
    </div>
</div>
<!-- /.modal -->

<script type="text/javascript">

    function toolTips() {
        $("[data-toggle='tooltip']").tooltip();
        return false;

    }

    function showfrmError() {

        $("#frmError").modal("show");
        return false;
    }

    function init_Js_updPagina() {

        toolTips();
        return false;
    }
    function showfrmConfirmarEliminar() {
        debugger;
        $("#pnlBotonesConfirmacion").show();
        $("#frmConfirmarEliminar").modal("show");
        return false;
    }

    function hidefrmConfirmarEliminar() {

        $("#frmConfirmarEliminar").modal("hide");
        return false;
    }

    function ocultarBotonesConfirmacion() {

        $("#pnlBotonesConfirmacion").hide();
        return false;
    }

    function showfrmAgregarUbicacion() {

        $("#frmAgregarUbicacion").modal({
            "show": true,
            "backdrop": "static",
            "keyboard": false
        });
        return false;
    }

    function hidefrmAgregarUbicacion() {

        $("#frmAgregarUbicacion").modal("hide");
        return false;
    }

    function ocultarBotonesGuardado() {

        $("#pnlBotonesGuardar").hide();
        return true;
    }

    function validarGuardar() {
        var ret = true;

        $("#<%: grdPlantasHabilitar.ClientID %> [id*='_ReqtxtDetalle']").hide();

        $("#<%: grdPlantasHabilitar.ClientID %> [id*='_txtDetalle']").each(function (index, element) {

            if ($(element).val().length == 0) {
                var txtDetalle_id = $(element).prop("id");
                var chkSeleccionado_id = txtDetalle_id.replace("_txtDetalle", "_chkSeleccionado");
                var ReqtxtDetalle_id = txtDetalle_id.replace("_txtDetalle", "_ReqtxtDetalle");

                if ($("#" + chkSeleccionado_id).prop("checked")) {

                    $("#" + ReqtxtDetalle_id).show();
                    ret = false;

                }
            }

        })

        return ret;
    }
</script>
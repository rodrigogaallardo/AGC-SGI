<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucObservacionesTareav1.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucObservacionesTareav1" %>

<div class="accordion-group widget-box">

    <div class="accordion-heading">
        <a id="btnUpDown" data-parent="#collapse-group" href="#collapse_observ" 
            data-toggle="collapse" onclick="tda_btnUpDown_collapse_click(this)">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5><asp:Label ID="tituloControl" runat="server" Text="Documentación a Presentar"></asp:Label></h5>
                <span class="btn-right"><i class="icon-chevron-down"></i></span>        
            </div>
        </a>
    </div>

    <div  class="accordion-body collapse in" id="collapse_observ" >

        <div class="widget-content" >

            <div>

                <asp:Panel ID="pnlObser" runat="server" Visible="true">

                    <asp:UpdatePanel ID="updPnlObser" runat="server">
                    <ContentTemplate>

                        <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
                        <asp:HiddenField ID="hid_id_tramitetarea" runat="server"  Value="0"/>
                        <asp:HiddenField ID="hid_editable" runat="server" Value="true" />

                        <asp:UpdatePanel ID="updShowAgregar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row ptop10 pright15">
                                    <div class="cols-sm-12 text-right">
                                        <asp:LinkButton ID="btnShowAgregar" runat="server" CssClass="btn btn-default pbottom5" OnClick="btnShowAgregar_Click"
                                            data-group="controles-accion">
                                                <i class="imoon imoon-plus"></i>
                                                <span class="text">Agregar Documento</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Panel ID="pnlObservaciones" runat="server" Visible="true">
                            <asp:GridView ID="grdObser" runat="server" AutoGenerateColumns="false" 
                                GridLines="none" CssClass="table table-striped table-bordered" 
                                OnRowDataBound="grdObser_RowDataBound">
                                <Columns>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditarObser" runat="server" 
                                                    CommandArgument='<%# Eval("id_ObsDocs") %>' 
                                                    OnCommand="lnkEditarObser_Command" 
                                                    Width="15px" >
                                                <i class="icon icon-pencil"></i> 
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="nombre_tdocreq" HeaderText="Documento" />
                                    <asp:BoundField DataField="Observacion_ObsDocs" HeaderText="Observaciones" />
                                    <asp:BoundField DataField="Respaldo_ObsDocs" HeaderText="Respaldo Normativo" />

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminarObser" runat="server" 
                                                    CommandArgument='<%# Eval("id_ObsDocs") %>' 
                                                    OnClientClick="javascript:return tda_confirm_del();"
                                                    OnCommand="lnkEliminarObser_Command" 
                                                    Width="15px" >
                                                <i class="icon icon-remove"></i> 
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-block" >
                                        <span class="text">No posee Documentacion a Presentar en la tarea actual.</span>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>

                </asp:Panel>

            </div>

        </div>

    </div>

</div>


<%--Modal agregar--%>
<div id="frmDatosObser" class="modal fade" style="display: none; width: 750px">
    <asp:UpdatePanel ID="updAgregar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Agregar Documentación</h4>
                    </div>
                    <div class="modal-body pbottom0" >
                        <asp:HiddenField ID="hid_id_ObsDocs" runat="server" />


                        <div class="form-horizontal pright10">
                            <div class="row-fluid">
                                <label class="span2">Documento (*):</label>
                                <div class="span10">
                                    <asp:DropDownList ID="ddlDocumento" runat="server" Width="100%">
                                    </asp:DropDownList>
                                    <div id="Req_Documento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el Documento.
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-horizontal pright10">
                            <div class="row-fluid">
                                <label class="span2">Observación(*):</label>
                                <div class="span10">
                                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode ="MultiLine" Width="100%" Height="80px" MaxLength="8000" ></asp:TextBox>
                                    <div id="Req_txtObservaciones" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar Observación.
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlRespaldoNormativo" runat="server">
                            <div class="form-horizontal pright10">
                                <div class="row-fluid">
                                    <label class="span2">Respaldo Normativo :</label>
                                    <div class="span10">
                                        <asp:TextBox ID="txtRespaldo" runat="server" TextMode ="MultiLine" Width="100%" Height="80px" MaxLength="2000" ></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modal-footer mtop0">

                        <asp:UpdatePanel ID="updBotonesAgregar" runat="server">
                            <ContentTemplate>

                                <div class="form-inline">
                                    <div class="form-group">
                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="updBotonesAgregar">
                                            <ProgressTemplate>
                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />Guardando...
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div id="pnlBotonesAgregar" class="form-group">

                                        <asp:LinkButton ID="btnAceptar" runat="server" CssClass="btn btn-primary" OnClientClick="return validarAgregar();" OnClick="btnAceptar_Click">
                                            <i class="imoon imoon-ok"></i>
                                            <span class="text">Aceptar</span>
                                        </asp:LinkButton>
                                        <button type="button" class="btn btn-default" data-dismiss="modal">
                                            <i class="imoon imoon-close"></i>
                                            <span class="text">Cancelar</span>
                                        </button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</div>
<!-- /.modal -->


<%--Modal mensajes de error--%>
<div id="frmError_ucObser" class="modal fade" style="display: none;">
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
                            <asp:UpdatePanel ID="updmpeInfoObser" runat="server" class="form-group">
                                <ContentTemplate>
                                    <asp:Label ID="lblErrorObs" runat="server" class="pad10"></asp:Label>
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
        init_Js_upd_ddlDocumento();
    });

    function init_Js_upd_ddlDocumento() {
        $("#<%: ddlDocumento.ClientID %>").select2({ allowClear: true });
        return false;
    }

    function showfrmError_ucObser() {
        $("#frmError_ucObser").modal("show");
        return false;
    }

    function tda_confirm_del() {
        return confirm('¿Esta seguro que desea eliminar este Registro?');
    }

    function tda_mostrar_mensaje(texto, titulo) {

        if (titulo == "") {
            titulo = "Documentacion a presentar";
        }

        $.gritter.add({
            title: titulo,
            text: texto,
            image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
            sticky: false
        });

    }

    function tda_btnUpDown_collapse_click(obj) {
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

    function validarAgregar() {
        var ret = true;

        $("#Req_Documento").hide();
        $("#Req_txtObservaciones").hide();
        if ($.trim($("#<%: ddlDocumento.ClientID %>").val()) == 0) {
            $("#Req_Documento").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtObservaciones.ClientID %>").val()).length == 0) {
            $("#Req_txtObservaciones").css("display", "inline-block");
            ret = false;
        }

        if (ret) {
            $("#pnlBotonesAgregar").hide();
        }
        return ret;
    }

    function showfrmDatosObser() {
        $("#frmDatosObser").modal({
            "show": true,
            "backdrop": "static"
        });
        return false;
    }

    function hidefrmDatosObser() {
        $("#frmDatosObser").modal("hide");
        return false;
    }
</script>



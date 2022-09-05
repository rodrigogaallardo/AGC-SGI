<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_DatosLocal.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tab_DatosLocal" %>

<asp:UpdatePanel ID="updHiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_id_cpadron" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
        <asp:HiddenField ID="hid_editar" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<div id="titulo" runat="server" class="accordion-heading">
    <h3 style="line-height: 20px;">Datos del Local</h3>
</div>
<%--Box--%>

<div id="box_datoslocal" class="accordion-group widget-box mtop20">

    <%-- titulo Mapas--%>
    <div class="accordion-heading">
        <a id="ubicacion_btnUpDown" data-parent="#collapse-group">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-office"></i></span>
                <h5>Datos del Local</h5>
            </div>
        </a>
    </div>
    <%-- contenido --%>
    <div class="accordion-body collapse in" id="collapse_ubicacion">
        <div class="widget-content">
            <asp:UpdatePanel ID="updDatosLocal" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="flow-row">
                        <div class="span5">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label><b>Características del Local</b></label>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Superficie Cubierta:</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtSuperficieCubierta" runat="server" Text="0,00" Width="70px" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Superficie Descubierta:</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtSuperficieDescubierta" runat="server" Text="0,00" Width="70px" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Superficie Total:</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtSuperficieTotal" runat="server" Text="0,00" Width="70px" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqSuperficies" class="alert alert-danger" style="display: none">
                                            Los campos de superficie son obligatorios (al menos uno).
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Dimensión del Frente:</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtDimensionFrente" runat="server" Text="0,00" Width="70px" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Posee lugar de carga y descarga:</label>
                                    <div class="col-sm-6">
                                        <asp:RadioButton ID="opt1_si" runat="server" GroupName="LugarCargaDescarga" Text="Sí" Enabled="false" />
                                        <asp:RadioButton ID="opt1_no" runat="server" GroupName="LugarCargaDescarga" Text="No" Enabled="false" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Posee estacionamiento:</label>
                                    <div class="col-sm-6">
                                        <asp:RadioButton ID="opt2_si" runat="server" GroupName="Estacionamiento" Text="Sí" Enabled="false" />
                                        <asp:RadioButton ID="opt2_no" runat="server" GroupName="Estacionamiento" Text="No" Enabled="false" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Red de tránsito pesado:</label>
                                    <div class="col-sm-6">
                                        <asp:RadioButton ID="opt3_si" runat="server" GroupName="RedTansitoPesado" Text="Sí" Enabled="false" />
                                        <asp:RadioButton ID="opt3_no" runat="server" GroupName="RedTansitoPesado" Text="No" Enabled="false" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Sobre Avenida:</label>
                                    <div class="col-sm-6">
                                        <asp:RadioButton ID="opt4_si" runat="server" GroupName="SobreAvenida" Text="Sí" Enabled="false" />
                                        <asp:RadioButton ID="opt4_no" runat="server" GroupName="SobreAvenida" Text="No" Enabled="false" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">

                                        <div id="ReqRadioButton" class="alert alert-danger" style="display: none">
                                            Las respuestas son obligatorias..
                                        </div>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label" style="width: 250px;">Cantidad de operarios:</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtCantOperarios" runat="server" Text="0,00" Width="50px" MaxLength="5" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqCantOperarios" class="alert alert-danger" style="display: none">
                                            El campo es obligatorio.
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label><b>Materiales expresados en...</b></label>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Pisos:</label>
                                    <div>
                                        <asp:TextBox ID="txtPisos" runat="server" TextMode="MultiLine" Height="63px"
                                            Width="250px" onkeypress="return textMaxLength(this,200);" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqPisos" class="alert alert-danger" style="display: none">
                                            El campo es obligatorio.
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Paredes:</label>
                                    <div>
                                        <asp:TextBox ID="txtParedes" runat="server" TextMode="MultiLine"
                                            Height="63px" Width="250px" onkeypress="return textMaxLength(this,200);" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqParedes" class="alert alert-danger" style="display: none">
                                            El campo es obligatorio.
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Techos:</label>
                                    <div>
                                        <asp:TextBox ID="txtTechos" runat="server" TextMode="MultiLine" Height="63px"
                                            Width="250px" onkeypress="return textMaxLength(this,200);" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqTechos" class="alert alert-danger" style="display: none">
                                            El campo es obligatorio.
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Revestimientos:</label>
                                    <div>
                                        <asp:TextBox ID="txtRevestimientos" runat="server" TextMode="MultiLine"
                                            Height="63px" Width="250px" onkeypress="return textMaxLength(this,200);" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqRevestimientos" class="alert alert-danger" style="display: none">
                                            El campo es obligatorio.
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span7">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <div class="control-group">
                                        <label><b>Mapa de Ubicación</b></label>
                                    </div>
                                    <div class="text-center ">
                                        <asp:Image ID="imgMapa1" runat="server" CssClass="img-thumbnail" />
                                    </div>
                                </div>
                                <hr />
                                <div class="control-group">
                                    <div class="control-group">
                                        <label><b>Croquis de Ubicación</b></label>
                                    </div>
                                    <div class="text-center ">
                                        <asp:Image ID="imgMapa2" runat="server" CssClass="img-thumbnail" />
                                    </div>
                                </div>
                                <div>
                                    <div class="control-group span1" style="width: 100px;">
                                        <label>Frente:</label>
                                        <div>
                                            <asp:TextBox ID="txtFrente" runat="server" MaxLength="10" Width="100px" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span1" style="width: 100px;">
                                        <label>Fondo:</label>
                                        <div>
                                            <asp:TextBox ID="txtFondo" runat="server" MaxLength="10" Width="100px" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span1" style="width: 130px;">
                                        <label>Lateral Izquierdo:</label>
                                        <div>
                                            <asp:TextBox ID="txtLatIzq" runat="server" MaxLength="10" Width="100px" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span1" style="width: 130px;">
                                        <label>Lateral Derecho:</label>
                                        <div>
                                            <asp:TextBox ID="txtLatDer" runat="server" MaxLength="10" Width="100px" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqMedidasParcela" class="alert alert-danger" style="display: none">
                                            Debe ingresar todos los valores (Frente/Fondo/Lateral Izquierdo y Lateral Derecho.
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <%--Servicios Sanitarios--%>
                                <div class="control-group">
                                    <label><b>Servicios sanitarios:</b></label>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px; text-align: left; margin-left: 10px">Los mismos se encuentran:</label>
                                </div>
                                <div class="control-group">
                                    <div class="control-group span3">
                                        <asp:RadioButton ID="opt5_dentro" runat="server" GroupName="Sanitarios"
                                            onclick="objVisibility('tblDistanciaSanitarios_dl','hide');"
                                            Text="Dentro del Local" Enabled="false" />
                                        <asp:RadioButton ID="opt5_fuera" runat="server" GroupName="Sanitarios"
                                            onclick="objVisibility('tblDistanciaSanitarios_dl','show');"
                                            Text="Fuera del Local" Enabled="false" />
                                    </div>
                                    <div class="control-group span3" id="tblDistanciaSanitarios_dl" style="display: none;">
                                        <label>¿a que distancia? (metros):</label>
                                        <asp:TextBox ID="txtDistanciaSanitarios_dl" runat="server" MaxLength="4"
                                            onfocus="this.select();" Width="40px" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="pleft20 pright20">
                                        <div id="ReqIngresoRespuestasSanitarios" class="alert alert-danger" style="display: none">
                                            Las respuestas son obligatorias y de selecciona 'Fuera del local', la distancia es obligatoria.
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px">Cantidad de artefactos sanitarios:</label>
                                    <div>
                                        <asp:TextBox ID="txtCantidadArtefactosSanitarios" runat="server" MaxLength="4" Width="70px" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" style="width: 250px">Superficie de Sanitarios:</label>
                                    <div>
                                        <asp:TextBox ID="txtSuperficieSanitarios" runat="server" MaxLength="8" Width="70px" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <hr />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="clear: both"></div>
        </div>
    </div>
</div>

<%--Botones de Guardado--%>
<asp:UpdatePanel ID="updBotonesGuardar" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div class="form-inline text-center mtop20">
            <div id="pnlBotonesGuardar_DatosLocal" class="form-group">

                <asp:LinkButton ID="btnGuardar" runat="server" Style="color: white" CssClass="btn btn-primary" data-group="controles-accion"
                    OnClick="btnGuardar_Click" OnClientClick="return validarGuardar_DatosLocal();">
                        <i class="imoon imoon-disk"></i>
                        <span class="text">Guardar Datos del Local</span>
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
<div id="frmError_DatosLocal" class="modal fade" style="display: none;">
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

    var vSeparadorDecimal;
    $(document).ready(function () {
        init_JS_updDatosLocal();
        return false;
    });

    function textMaxLength(txt, maxLen) {
        if (txt.value.length > maxLen - 1)
            return false;
        return true;
    }

    function init_JS_updDatosLocal() {
        vSeparadorDecimal = $("#<%: hid_DecimalSeparator.ClientID %>").attr("value");
        eval("$('#<%: txtSuperficieCubierta.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtSuperficieDescubierta.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");

        eval("$('#<%: txtDimensionFrente.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999.99'})");
        eval("$('#<%: txtDistanciaSanitarios_dl.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '99.99'})");

        eval("$('#<%: txtCantidadArtefactosSanitarios.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '0',vMax: '99999'})");
        eval("$('#<%: txtSuperficieSanitarios.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");

        eval("$('#<%: txtFrente.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtFondo.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtLatIzq.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtLatDer.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtCantOperarios.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '0',vMax: '99999'})");

        /*$('#%: txtPisos.ClientID %>').on("keyup", function () {
            $("#ReqPisos").hide();
        });
        $('#%: txtParedes.ClientID %>').on("keyup", function () {
            $("#ReqParedes").hide();
        });
        $('#%: txtTechos.ClientID %>').on("keyup", function () {
            $("#ReqTechos").hide();
        });
        $('#%: txtRevestimientos.ClientID %>').on("keyup", function () {
            $("#ReqRevestimientos").hide();
        });
        */
        $('#<%: txtSuperficieCubierta.ClientID %>').on("keyup", function () {
            //$("#ReqSuperficies").hide();
            TotalizarSuperficie_DatosLocal();
        });

        $('#<%: txtSuperficieDescubierta.ClientID %>').on("keyup", function () {
            //$("#ReqSuperficies").hide();
            TotalizarSuperficie_DatosLocal();
        });
        RefrescarDatos();
    }

    function frmError_DatosLocal() {
        $("#pnlBotonesGuardar_DatosLocal").show();
        $("#frmError_DatosLocal").modal("show");
        return false;
    }

    function TotalizarSuperficie_DatosLocal() {
        var value1 = $("#<%: txtSuperficieCubierta.ClientID %>").val();
        var value2 = $("#<%: txtSuperficieDescubierta.ClientID %>").val();

        val1 = stringToFloat(value1, vSeparadorDecimal);
        val2 = stringToFloat(value2, vSeparadorDecimal);
        var total = val1 + val2;
        $("#<%: txtSuperficieTotal.ClientID %>").val(total.toFixed(2).toString().replace(".", vSeparadorDecimal));

        return false;
    }

    function ocultarBotonesGuardado_DatosLocal() {
        $("#pnlBotonesGuardar_DatosLocal").hide("slow");
        return true;
    }

    function validarGuardar_DatosLocal() {
        var ret = true;
        /*$("#ReqSuperficies").hide();
        $("#ReqMedidasParcela").hide();
        $("#ReqRadioButton").hide();
        $("#ReqIngresoRespuestasSanitarios").hide();
        $("#ReqIngresoRequisitos").hide();
        $("#ReqIngresoArt813").hide();
        $("#ReqIngresoSobrecargas").hide();
        $("#ReqPisos").hide();
        $("#ReqParedes").hide();
        $("#ReqTechos").hide();
        $("#ReqRevestimientos").hide();

        var value1 = $("#%: txtSuperficieTotal.ClientID %>").val();
        var total = stringToFloat(value1);
        if (total <= 0) {
            $("#ReqSuperficies").show();
            ret = false;
        }
        valido = validarMedidasParcela();
        if (ret)
            ret = valido;
        valido = validarMateriales();
        if (ret)
            ret = valido;
        valido = validarIngresoRespuestasCL();
        if (ret)
            ret = valido;
        var valido;
        valido = validarIngresoRespuestasSanitarios();
        if (ret)
            ret = valido;
        var valido;
        valido = validarIngresoSobrecargas();
        if (ret)
            ret = valido;
        valido = validarIngresoArt813();
        if (ret)
            ret = valido;
        valido = validarIngresoRequisitos();
        if (ret)
            ret = valido;

        if (ret)
            ocultarBotonesGuardado_DatosLocal();*/
        return ret;
    }

    function validarMateriales() {
        var IsValid = true;
        var txtPisos = $("#<%: txtPisos.ClientID %>").val();
        var txtParedes = $("#<%: txtParedes.ClientID %>").val();
        var txtTechos = $("#<%: txtTechos.ClientID %>").val();
        var txtRevestimientos = $("#<%: txtRevestimientos.ClientID %>").val();
        if (txtPisos.trim().length == 0) {
            $("#ReqPisos").show();
            IsValid = false;
        }

        if (txtParedes.trim().length == 0) {
            $("#ReqParedes").show();
            IsValid = false;
        }

        if (txtTechos.trim().length == 0) {
            $("#ReqTechos").show();
            IsValid = false;
        }

        if (txtRevestimientos.trim().length == 0) {
            $("#ReqRevestimientos").show();
            IsValid = false;
        }
        return IsValid;
    }

    function validarMedidasParcela() {
        var IsValid = true;
        var txtFrente = $("#<%: txtFrente.ClientID %>").val();
        var txtFondo = $("#<%: txtFondo.ClientID %>").val();
        var txtLatIzq = $("#<%: txtLatIzq.ClientID %>").val();
        var txtLatDer = $("#<%: txtLatDer.ClientID %>").val();
        if (stringToFloat(txtFrente, vSeparadorDecimal) == 0 || stringToFloat(txtFondo, vSeparadorDecimal) == 0 ||
            stringToFloat(txtLatIzq, vSeparadorDecimal) == 0 || stringToFloat(txtLatDer, vSeparadorDecimal) == 0) {
            $("#ReqMedidasParcela").show();
            IsValid = false;
        }
        return IsValid;
    }

    function validarIngresoRespuestasCL() {
        ret = true;
        if (!$("#<%: opt1_si.ClientID %>").prop("checked") && !$("#<%: opt1_no.ClientID %>").prop("checked"))
            ret = false;
        if (!$("#<%: opt2_si.ClientID %>").prop("checked") && !$("#<%: opt2_no.ClientID %>").prop("checked"))
            ret = false;
        if (!$("#<%: opt3_si.ClientID %>").prop("checked") && !$("#<%: opt3_no.ClientID %>").prop("checked"))
            ret = false;
        if (!$("#<%: opt4_si.ClientID %>").prop("checked") && !$("#<%: opt4_no.ClientID %>").prop("checked"))
            ret = false;
        if (!ret)
            $("#ReqRadioButton").show();
        return ret;
    }

    function validarIngresoRespuestasSanitarios() {
        ret = true;
        var value1;
        var val = 0.00;
        if (!$("#<%: opt5_dentro.ClientID %>").prop("checked") && !$("#<%: opt5_fuera.ClientID %>").prop("checked")) {
            ret = false;
            $("#ReqIngresoRespuestasSanitarios").show();
        }
        if (ret) {
            if ($("#<%: opt5_fuera.ClientID %>").prop("checked")) {
                value1 = $("#<%: txtDistanciaSanitarios_dl.ClientID %>").val();
                val = stringToFloat(value1);
                if (val <= 0 || isNaN(val)) {
                    ret = false;
                    $("#ReqIngresoRespuestasSanitarios").show();
                }
            }
        }
        return ret;
    }

    function objVisibility(id, accion) {
        //recibe el id del objeto a mostrar u ocultar y la accion 'show'-> mostrar 'hide' o nada->ocultar
        debugger;
        if (accion == 'show')
            $("#" + id).css("display", "block");
        else
            $("#" + id).css("display", "none");
        return false;
    }

    function RefrescarDatos() {
        TotalizarSuperficie_DatosLocal();
        if ($("#<%: opt5_dentro.ClientID %>").prop("checked")) {
            $("#tblDistanciaSanitarios_dl").css("display", "none");
        }
        else {
            $("#tblDistanciaSanitarios_dl").css("display", "block");
        }
        return false;
    }

    function habilitarControlesDatosLocal(habilitar) {
        debugger;
        if (habilitar) {
            $("#<%: txtPisos.ClientID %>").removeAttr('disabled');
            $("#<%: txtParedes.ClientID %>").removeAttr('disabled');
            $("#<%: txtTechos.ClientID %>").removeAttr('disabled');
            $("#<%: txtRevestimientos.ClientID %>").removeAttr('disabled');
        }
        else {
            $("#<%: txtPisos.ClientID %>").attr('disabled', 'disabled');
            $("#<%: txtParedes.ClientID %>").attr('disabled', 'disabled');
            $("#<%: txtTechos.ClientID %>").attr('disabled', 'disabled');
            $("#<%: txtRevestimientos.ClientID %>").attr('disabled', 'disabled');
        }

        return false;
    }
</script>

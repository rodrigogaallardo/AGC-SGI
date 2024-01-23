<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualizarRubro.ascx.cs" Inherits="SGI.Controls.VisualizarRubro" %>
<style type="text/css">
    
    .col1
    { 
        width:150px;
        text-align:right;
        vertical-align:top;
        padding-top: 7px;
    }
    .col2
    { 
        padding-top:5px;
    }

 
  
    
</style>

<%: Styles.Render("~/Content/themes/base/css") %>
<%: Scripts.Render("~/bundles/autoNumeric") %>

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
<link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function init_updDatos() {

        var fechaVencimientoReq = $('#<%=txtFechaVigenciaHasta.ClientID%>');
        var es_readonly = $('#<%=txtFechaVigenciaHasta.ClientID%>').attr("readonly");
        $("#<%: txtFechaVigenciaHasta.ClientID %>").datepicker({
            minDate: "-100Y",
            maxDate: "0Y",
            yearRange: "-100:-0",
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            onSelect: function () {
                //$("#Req_FechaDesde").hide();
                //$("#Val_Formato_FechaDesde").hide();
                //$("#Val_FechaDesdeMenor").hide();
            }
        });
        vSeparadorDecimal = $("#<%: hid_DecimalSeparator.ClientID %>").attr("value");
        eval("$('#<%: txtSupMinCargaDescarga.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtSupMinCargaDescargaRefII.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtSupMinCargaDescargaRefV.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtSalonVentas.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtDesde.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        eval("$('#<%: txtHasta.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
    }

    function AgregarZonaCondicion(mostrar) {

        if (mostrar) {
            $('#<%=ddlZona.ClientID%>')[0].selectedIndex = 0;
            $('#<%=ddlCondicion.ClientID%>')[0].selectedIndex = 0;
            $('#<%=pnlAgregarZonaCondicion1.ClientID%>').slideUp(400);
            $('#<%=pnlAgregarZonaCondicion2.ClientID%>').slideDown(400);

        }
        else {
            $('#<%=pnlAgregarZonaCondicion2.ClientID%>').slideUp(400);
            $('#<%=pnlAgregarZonaCondicion1.ClientID%>').slideDown(400);
        }

        return false;
    }    

    function AgregarInfoRelevante(mostrar) {

        if (mostrar) {
            $('#<%=hid_id_rubinf.ClientID%>').attr("value", "0");
            $('#<%=txtEditDescripInfoRelevante.ClientID%>').attr("value", "");
            $('#<%=pnlAgregarInfoRelevanteEdit.ClientID%>').slideDown(400);
            $('#<%=pnlAccionesAgregarInfoRelevante.ClientID%>').slideUp(400);
        }
        else {
            $('#<%=pnlAgregarInfoRelevanteEdit.ClientID%>').slideUp(400);
            $('#<%=pnlAccionesAgregarInfoRelevante.ClientID%>').slideDown(400);
        }

        return false;
    }

    function AgregarConfIncendio(mostrar) {

        if (mostrar) {
            $('#<%=hid_id_rubinf.ClientID%>').attr("value", "0");
            $('#<%=txtEditDescripInfoRelevante.ClientID%>').attr("value", "");
            $('#<%=pnlAgregarConfIncendioEdit.ClientID%>').slideDown(400);
            $('#<%=pnlAccionesAgregarConfIncendio.ClientID%>').slideUp(400);
        }
        else {
            $('#<%=pnlAgregarConfIncendioEdit.ClientID%>').slideUp(400);
            $('#<%=pnlAccionesAgregarConfIncendio.ClientID%>').slideDown(400);
        }

        return false;
    }

    function AgregarDocumentoReq(mostrar) {

        if (mostrar) {
            $('#<%=hid_id_rubtdocreq.ClientID%>').attr("value", "0");
            $('#<%=ddlEditTipoDocReq.ClientID%>').attr("value", "0");
            $('#<%=ddlEditObligatorio.ClientID%>').attr("value", "0");

            $('#<%=pnlAgregarDocReqEdit.ClientID%>').slideDown(400);
            $('#<%=pnlAccionesAgregarDocReq.ClientID%>').slideUp(400);
        }
        else {
            $('#<%=pnlAgregarDocReqEdit.ClientID%>').slideUp(400);
            $('#<%=pnlAccionesAgregarDocReq.ClientID%>').slideDown(400);
        }

        return false;
    }

    function AgregarZonaAuto(mostrar) {

        if (mostrar) {
            $('#<%=hid_id_rubcircauto.ClientID%>').attr("value", "0");
            $('#<%=ddlEditZonaAuto.ClientID%>').attr("value", "0");

            $('#<%=pnlAgregarAutoEdit.ClientID%>').slideDown(400);
            $('#<%=pnlAccionesAgregarAuto.ClientID%>').slideUp(400);
        }
        else {
            $('#<%=pnlAgregarAutoEdit.ClientID%>').slideUp(400);
            $('#<%=pnlAccionesAgregarAuto.ClientID%>').slideDown(400);
        }

        return false;
    }

    function AgregarDocumentoReqZona(mostrar) {
        if (mostrar) {
            $('#<%=hid_id_rubtdocreq.ClientID%>').attr("value", "0");
            $('#<%=ddlEditTipoDocReqZona.ClientID%>').attr("value", "0");
            $('#<%=ddlEditObligatorioZona.ClientID%>').attr("value", "0");
            $('#<%=ddlEditZona.ClientID%>').attr("value", "0");

            $('#<%=pnlAgregarDocReqEditZona.ClientID%>').slideDown(400);
            $('#<%=pnlAccionesAgregarDocReqZona.ClientID%>').slideUp(400);
        }
        else {
            $('#<%=pnlAgregarDocReqEditZona.ClientID%>').slideUp(400);
            $('#<%=pnlAccionesAgregarDocReqZona.ClientID%>').slideDown(400);
        }

        return false;
    }

    function grdMouseOver() {
        grdMouseInf();
        grdMouseDocReq();
        grdMouseDocReqZona();
        grdMouseConfIncendio();
        grdMouseOverAuto();
        grdMouseOverZonasCondiciones();
    }

    function grdMouseOverZonasCondiciones() {
        debugger;
        
        var permiteActualizar = $('#<%=hid_rol_edicion.ClientID%>').val();
        
        $('#<%=grdZonasCondiciones.ClientID%> tbody tr').mouseover(function () {
            
            var id_estado_modif = $('#<%=hid_id_estado_modif.ClientID%>').attr("value");
            var rol_edicion = $('#<%=hid_rol_edicion.ClientID%>').attr("value");

            // En proceso
            if (permiteActualizar == "true" && id_estado_modif == "0" && rol_edicion == "true") 
            {
                $(this).find(".btnEliminarZonaCondicion").css("display", "block");
            }

        }

        );
        
        $('#<%=grdZonasCondiciones.ClientID%> tbody tr').mouseout(function () {

             $(this).find(".btnEliminarZonaCondicion").css("display", "none");

         });


    }

    function grdMouseInf() {
        var permiteActualizar = $('#<%=hid_rol_edicion.ClientID%>').val();

        $('#<%=grdInfoRelevante.ClientID%> tbody tr').mouseover(function () {
            var id_estado_modif = $('#<%=hid_id_estado_modif.ClientID%>').attr("value");
            var rol_edicion = $('#<%=hid_rol_edicion.ClientID%>').attr("value");
            // En proceso
            if (permiteActualizar == "true" && id_estado_modif == "0" && rol_edicion == "true") {
                $(this).find(".btnEliminarInfoRelevante").css("display", "block");
            }
        });

        $('#<%=grdInfoRelevante.ClientID%> tbody tr').mouseout(function () {
            $(this).find(".btnEliminarInfoRelevante").css("display", "none");
        });
    }
    function grdMouseDocReq() {
        var permiteActualizar = $('#<%=hid_rol_edicion.ClientID%>').val();
        $('#<%=grdDocReq.ClientID%> tbody tr').mouseover(function () {
            var id_estado_modif = $('#<%=hid_id_estado_modif.ClientID%>').attr("value");
            var rol_edicion = $('#<%=hid_rol_edicion.ClientID%>').attr("value");
            // En proceso
            if (permiteActualizar == "true" && id_estado_modif == "0" && rol_edicion == "true") {
                $(this).find(".btnEliminarDocReq").css("display", "block");
            }
        });

        $('#<%=grdDocReq.ClientID%> tbody tr').mouseout(function () {
            $(this).find(".btnEliminarDocReq").css("display", "none");
        });
    }
    function grdMouseDocReqZona() {
        var permiteActualizar = $('#<%=hid_rol_edicion.ClientID%>').val();
        $('#<%=grdDocReqZona.ClientID%> tbody tr').mouseover(function () {
            var id_estado_modif = $('#<%=hid_id_estado_modif.ClientID%>').attr("value");
            var rol_edicion = $('#<%=hid_rol_edicion.ClientID%>').attr("value");
            // En proceso
            if (permiteActualizar == "true" && id_estado_modif == "0" && rol_edicion == "true") {
                $(this).find(".btnEliminarDocReqZona").css("display", "block");
            }
        });

        $('#<%=grdDocReqZona.ClientID%> tbody tr').mouseout(function () {
            $(this).find(".btnEliminarDocReqZona").css("display", "none");
        });
    }
    function grdMouseConfIncendio() {
        var permiteActualizar = $('#<%=hid_rol_edicion.ClientID%>').val();
        $('#<%=grdConfIncendio.ClientID%> tbody tr').mouseover(function () {
            var id_estado_modif = $('#<%=hid_id_estado_modif.ClientID%>').attr("value");
            var rol_edicion = $('#<%=hid_rol_edicion.ClientID%>').attr("value");
            // En proceso
            if (permiteActualizar == "true" && id_estado_modif == "0" && rol_edicion == "true") {
                $(this).find(".btnEliminarConfIncendio").css("display", "block");
            }
        });

        $('#<%=grdDocReqZona.ClientID%> tbody tr').mouseout(function () {
            $(this).find(".btnEliminarConfIncendio").css("display", "none");
        });
    }
    function grdMouseOverAuto() {
        var permiteActualizar = $('#<%=hid_rol_edicion.ClientID%>').val();
        $('#<%=grdAuto.ClientID%> tbody tr').mouseover(function () {
            var id_estado_modif = $('#<%=hid_id_estado_modif.ClientID%>').attr("value");
            var rol_edicion = $('#<%=hid_rol_edicion.ClientID%>').attr("value");
            // En proceso
            if (permiteActualizar == "true" && id_estado_modif == "0" && rol_edicion == "true") {
                $(this).find(".btnEliminarAutoZona").css("display", "block");
            }
        });

        $('#<%=grdAuto.ClientID%> tbody tr').mouseout(function () {
            $(this).find(".btnEliminarAutoZona").css("display", "none");
        });
    }

    function showfrmErrorVisual() {
        $("#frmErrorVisual").modal("show");
        return false;
    }
</script>

<asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
<asp:HiddenField ID="hid_id_rubhistcam" runat="server" />
<asp:HiddenField ID="hid_id_rubro" runat="server" />
<asp:HiddenField ID="hid_tipo_solicitudcambio" runat="server" />
<asp:HiddenField ID="hid_id_estado_modif" runat="server" />
<asp:HiddenField ID="hid_rol_edicion" runat="server" />


<%--extracto de la solicitud de cambio--%>
<div class="widget-box">
<div class="widget-title ">
			    <span class="icon"><i class="imoon imoon-hammer"></i></span>
			    <h5>Extracto de la solicitud</h5>
            </div>
            <div class="widget-content">
<asp:Panel ID="pnlDatosSolicitudCambio" runat="server" Width="90%" Visible="false">
    
    <table border="0" style="width:100%">
    <tr>
        <td class="col1" style="width:200px" >
            <div style="padding-top: 7px;">
                <label>Nº de Solicitud de Cambio:</label>
            </div>
            <div style="padding-top: 7px;">
                <label>Tipo de solicitud:</label>
            </div>
        </td>
        <td style="vertical-align: top;  ">
            <div style="padding-top: 14px;">
                <asp:Label ID="lblNroSolicitudCambio" runat="server" Text="0" Font-Bold="true" ></asp:Label>
            </div>
            <div style="padding-top: 10px;">
                <asp:Label ID="lblTipoSolicitud" runat="server" Font-Bold="true" ForeColor="#c2510f"></asp:Label>
            </div>
                
            
        </td>
        <td style="text-align: right" rowspan="2">
            <div>
                <label>Usuario/s interviniente/s en el/los cambio/s:</label>
            </div>
            
            <asp:GridView ID="grdUsuariosIntervinientes" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
            <Columns>
            
                <asp:BoundField DataField="UserName" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Usuario" />
                <asp:BoundField DataField="Apenom" ItemStyle-Width="120px" HeaderText="Apellido y Nombres" />
                <asp:BoundField DataField="LastUpdateDate" ItemStyle-Width="70px" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                    HeaderText="Fecha de modif." ItemStyle-HorizontalAlign="Center" />
                
            </Columns>
            </asp:GridView>
           
        </td>
    </tr>

    </table>


</asp:Panel>
</div>
    </div>
<%--Datos del rubro y tablas relacionadas--%>

<asp:Panel ID="pnlDatosRubro" runat="server">
        
    <div class="widget-box">
        <div class="widget-title ">
		    <span class="icon"><i class="imoon imoon-hammer"></i></span>
			<h5>Datos del Rubro</h5>
        </div>
        <div class="widget-content">
            <table border="0" style="width: 100%">
                <tr>
                    <td class="col1">
                        <asp:Label ID="lblCodRubro" runat="server" Text="Código del Rubro:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtCodRubro" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="ReqtxtCodRubro" runat="server" ControlToValidate="txtCodRubro"  CssClass="error"
                             ErrorMessage="El código de rubro es obligatorio." Display="Dynamic" ValidationGroup="Guardar" >
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="lblDescRubro" runat="server" Text="Descripción del Rubro:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtDescRubro" runat="server" MaxLength="300" 
                            TextMode="MultiLine" Width="90%"
                           ></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="ReqtxtDescRubro" runat="server" ControlToValidate="txtDescRubro"  CssClass="error"
                             ErrorMessage="La descripción del rubro es obligatorio." Display="Dynamic" ValidationGroup="Guardar">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="lblBusqueda" runat="server" Text="Palabras Claves:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtBusqueda" runat="server" MaxLength="400" TextMode="MultiLine"
                            Width="90%"></asp:TextBox>
                        <div class="info2">
                            Palabras separadas por (/), las mismas sirven para que el rubro sea encontrado por
                            otra palabra habitual.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="lblTooTip" runat="server" Text="Información descriptiva:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtToolTip" runat="server" MaxLength="2000" TextMode="MultiLine"
                            Width="90%"></asp:TextBox>
                        <div class="info2">
                            Lo escrito en este campo saldrá como ayuda en el Asesor Virtual de Habilitaciones.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="lblTipoActividad" runat="server" Text="Tipo de Actividad:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Panel ID="pnlTipoActividad" runat="server" Width="350px" >
                            <asp:DropDownList ID="ddlTipoActividad" runat="server" Width="300px">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="lblTipoDocReq" runat="server" Text="Tipo de Trámite:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Panel ID="pnlTipoDocReq" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlTipoDocReq" runat="server" Width="300px" OnSelectedIndexChanged="ddlTipoDocReq_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label19" runat="server" Text="Registro de alcohol:"></asp:Label>
                    </td>
                    <td class="col3">
                        <asp:Panel ID="Panel1" runat="server" Width="350px" >
                            <asp:DropDownList ID="ddlRegistroAlc" runat="server" Width="300px">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label25" runat="server" Text="Superficie Salón de Ventas:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtSalonVentas" runat="server" MaxLength="6" Width="80px" AutoPostBack ="true"
                            OnTextChanged ="txtSalonVentas_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label20" runat="server" Text="Grupo Circuito:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Panel ID="pnlCircuito" runat="server" Width="350px" >
                            <asp:DropDownList ID="ddlCircuito" runat="server" Width="500px">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label22" runat="server" Text="Nomenclador ClaNaE:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Panel ID="pnlClanae" runat="server" Width="350px" >
                            <asp:DropDownList ID="ddlClanae" runat="server" Width="500px">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label1" runat="server" Text="Tipo de Rubro:"></asp:Label>
                    </td>
                    <td class="col2">
            
                        <asp:Panel ID="pnlRubroHistorico" runat="server" style="margin-top: -3px;" Width="220px" Height="23px">
                            <asp:RadioButton ID="optRubroActual" runat="server" Text="Rubro Actual" GroupName="TipoDeRubro"
                                Checked="true" />
                            <asp:RadioButton ID="optRubroHistorico" runat="server" Text="Rubro Histórico" GroupName="TipoDeRubro" />
                        </asp:Panel>
                        <div class="info2">
                            <b>Rubro Actual:</b> Son los rubros que actualmente se encuentran en el Nomenclador
                            vigente.<br />
                            <b>Rubro histórico:</b> Son los anteriores a los que están en el Nomenclador vigente,
                            y se utilizan para los tipos de trámite (Transferencias,Ampliaciones,Unificaciones
                            y Redistribuciones de uso).
                        </div>

                    </td>
                </tr>
            </table>
            <table border="0" style="width: 100%">
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label2" runat="server" Text="Circuito hab. automático:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:CheckBox ID="chkCircuitoAutomatico" runat="server" OnCheckedChanged="chkCircuitoAutomatico_CheckedChanged" AutoPostBack="true" />            
                    </td>
                    <td class="col1">
                        <asp:Label ID="Label8" runat="server" Text="Uso condicionado (UCDI):"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:CheckBox ID="chkUsoCondicionado" runat="server" />            
                    </td>
                    <td class="col1">
                        <asp:Label ID="Label18" runat="server" Text="Valida carga/descarga:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:CheckBox ID="chkValidaCargaDescarga" runat="server" />            
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label12" runat="server" Text="Oficina Comercial:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:CheckBox ID="chkOficinaComercial" runat="server" />            
                    </td>
                    <td class="col1">
                        <asp:Label ID="Label13" runat="server" Text="Con Depósito:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:CheckBox ID="chkTieneDeposito" runat="server" />            
                    </td>
                    <td class="col1">
                        <asp:Label ID="Label21" runat="server" Text="Librado al Uso:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:CheckBox ID="ChkLibrado" runat="server" />    
                    </td>
                </tr>

                <tr>
                    <td class="col1">
                        <asp:Label ID="Label9" runat="server" Text="Superficie Minima de carga/descarga:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtSupMinCargaDescarga" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                    </td>
                    <td class="col1">
                        <asp:Label ID="Label10" runat="server" Text="Superficie Minima de carga/descarga segun Ref II:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtSupMinCargaDescargaRefII" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                    </td>
                    <td class="col1">
                        <asp:Label ID="Label11" runat="server" Text="% Superficie Minima de carga/descarga segun Ref V :"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtSupMinCargaDescargaRefV" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <table border="0" style="width: 100%">
                <%--circuitos automaticos--%>
                <tr>
                    <td colspan="2">
                        <div style="padding: 15px 5px 0px 5px">
                            <asp:Label ID="Label16" runat="server" Text="Configuración de circuito hab. automático por zona:" Font-Bold="true"></asp:Label>
                            <%--grilla con datos de documentos requeridos--%>
                            <asp:UpdatePanel ID="pnlGrdAuto" runat="server" Width="90%" style="text-align:right;padding-top:5px"  > 
                                <ContentTemplate>
                                    <asp:GridView ID="grdAuto" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="id_rubcircauto_histcam,id_rubcircauto,codZonaHab" Style="margin-top: 5px" GridLines="None" 
                                        CssClass="table table-bordered table-striped table-hover with-check">
                                        <Columns>   
                                            <asp:BoundField DataField="zonaDesc" HeaderText="Zona Habilitación" HeaderStyle-Width="60px"/>
                                            <asp:TemplateField ItemStyle-Width="80px">
                                            <ItemTemplate>
                        
                                                <asp:LinkButton ID="btnEliminarAutoZona" runat="server" Text="Eliminar" 
                                                    CssClass="btnEliminarAutoZona" style="display:none"
                                                    CommandArgument='<%# Eval("id_rubcircauto_histcam") %>' 
                                                    OnClick="btnEliminarAutoZona_Click"
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                </asp:LinkButton>

                                            </ItemTemplate>
                        
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No se han encontrado circuitos automáticos por zona para este rubro.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <%--Agregar nueva documentos requeridos--%>
                            <asp:Panel ID="pnlAccionesAgregarAuto" runat="server" Width="90%" style="text-align:right;padding-top:5px" Visible="false"   > 
                                <asp:LinkButton ID="lnkBtnAccionesAgregarAuto" runat="server" CssClass="btn btn-primary" 
                                    OnClientClick="return AgregarZonaAuto(true);">
                                    <i class="icon-white icon-plus"></i>
                                    <span class="text">Agregar Circuito Automático por zona</span>
                                </asp:LinkButton>
                            </asp:Panel>

                            <%--panel para editar documentos requeridos--%>
                            <asp:Panel ID="pnlAgregarAutoEdit" runat="server" style="display:none" >

                                <div style="padding: 10px 5px 10px 5px" class="info2">
                                    <label>
                                        Para Agregar una zona que corresponda al circuito de habilitaci&oacute;n automática para el rubro ingrese la zona y luego presione el bot&oacute;n "Agregar"
                                    </label>
                                    <div style="margin-top: 10px">

                                        <asp:HiddenField ID="hid_id_rubcircauto" runat="server" />

                                        <table>
                                            <tr>
                                                <td>Zona Habilitación
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditZonaAuto" runat="server" Width="200px"/>
                                                    <asp:CompareValidator ID="CompareValidator3" runat="server" 
                                                            ErrorMessage="Debe seleccionar la zona."
                                                            ControlToValidate="ddlEditZonaAuto" 
                                                            ValidationGroup="guardarAuto"            
                                                            ValueToCompare="0"
                                                            display="Dynamic"
                                                            Type="String" Operator="NotEqual">
                                                    </asp:CompareValidator> 
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <asp:UpdatePanel ID="updPnlAgregarAuto" runat="server" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnGuardarAuto" runat="server"  ValidationGroup="guardarAuto"
                                                                 Style="margin-left: 10px" OnClick="btnGuardarAuto_Click" CssClass="btn btn-inverse">
                                                                <i class="icon-white icon-plus"></i>
                                                                <span class="text">Agregar</span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnCancelarAuto" runat="server" 
                                                                 OnClientClick="return AgregarZonaAuto(false);" CssClass="btn btn-default">
                                                                <i class="imoon imoon-blocked"></i>
                                                                <span class="text">Cancelar</span>
                                                            </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                            </asp:Panel>

                            <%--grilla con las zonas que seran eliminados--%>
                            <asp:Panel ID="pnlAutoEliminada" runat="server" Visible="false">
                    
                                <asp:Label ID="Label17" runat="server" Text="Zonas que ha sido eliminada:" Font-Bold="true"></asp:Label>

                                <asp:GridView ID="grdAutoEliminada" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="id_rubcircauto_histcam,id_rubcircauto,codZonaHab" Style="margin-top: 5px" GridLines="None" 
                                    CssClass="table table-bordered table-striped table-hover with-check">
                                    <Columns>
                                        <asp:BoundField DataField="zonaDesc" HeaderText="Zona Habilitación" HeaderStyle-Width="60px"/>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        NO HAY DATOS ...
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
        
    <div class="widget-box">
        <div class="widget-title ">
			<span class="icon"><i class="imoon imoon-hammer"></i></span>
			<h5>Condiciones por Zona</h5>
        </div>
        <div class="widget-content">
                
    <table border="0" style="width: 100%">
        <tr>
            <td class="col1">
                <asp:Label ID="lblFechaVigenciaHasta" runat="server" Text="Rubro vigente hasta el:"></asp:Label>
            </td>
            <td class="col2">
                <asp:TextBox ID="txtFechaVigenciaHasta" runat="server" Width="80px" ></asp:TextBox>
                <div class="req">
                  <asp:RegularExpressionValidator
                        ID="rev_txtFechaVigenciaHasta" runat="server" 
                        ValidationGroup="buscar"
                        ControlToValidate="txtFechaVigenciaHasta" CssClass="field-validation-error" 
                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                        Display="Dynamic">
                  </asp:RegularExpressionValidator>  
                </div>
  
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <div style="padding: 15px 5px 0px 5px">
                
<%--                    <asp:Label ID="Label2" runat="server" Text="Configuración de Condiciones por Zona:"
                        Font-Bold="true"></asp:Label>--%>
                
                    <asp:UpdatePanel ID="pnlGrdZonasCondiciones" runat="server" Width="90%" style="text-align:right;padding-top:5px"  > 
                        <ContentTemplate>
                        <asp:GridView ID="grdZonasCondiciones" runat="server" AutoGenerateColumns="false"
                            DataKeyNames="id_rubzonhabhistcam,cod_ZonaHab,cod_condicion" Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                            <Columns>
                                <asp:BoundField DataField="Zona" HeaderText="Zona" ItemStyle-Width="350px" />
                                <asp:BoundField DataField="Condicion" HeaderText="Condición" />
                                <asp:BoundField DataField="SupMin_condicion" HeaderText="Sup. min." ItemStyle-Width="80px" />
                                <asp:BoundField DataField="SupMax_condicion" HeaderText="Sup. max." ItemStyle-Width="80px" />
                                <asp:TemplateField ItemStyle-Width="80px">
                                <ItemTemplate>
                        
                                    <asp:LinkButton ID="btnEliminarZonaCondicion" runat="server" Text="Eliminar" CssClass="btnEliminarZonaCondicion" style="display:none"
                                        OnClick="btnEliminarZonaCondicion_Click" OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');"></asp:LinkButton>

                                </ItemTemplate>
                        
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                       </ContentTemplate>
                    </asp:UpdatePanel>
                
                    <asp:Panel ID="pnlAgregarZonaCondicion1" runat="server" Width="90%" style="text-align:right;padding-top:5px" Visible="false"   > 
                        <asp:LinkButton ID="lnkAgregarZonaCondicion" runat="server"
                                                    CssClass="btn btn-primary" 
                                                    OnClientClick="return AgregarZonaCondicion(true);">
                                                <i class="icon-white icon-plus"></i>
                                                <span class="text">Agregar nueva zona y condición</span>
                                            </asp:LinkButton>
                    </asp:Panel>
                
                    <asp:Panel ID="pnlAgregarZonaCondicion2" runat="server" style="display:none" >

                        <div style="padding: 10px 5px 10px 5px" class="info2">
                            <label>
                                Para Agregar una nueva restricc&oacute;n ingrese la zona, condici&oacute;n y el rango de superficie deaseado y luego presione el bot&oacute;n "Agregar"
                            </label>
                            <div style="margin-top: 10px">
                                <label>Zona:</label>
                                <asp:DropDownList ID="ddlZona" runat="server" Width="300px"></asp:DropDownList>
                                <label>Condición:</label>
                                <asp:DropDownList ID="ddlCondicion" runat="server" Width="300px"></asp:DropDownList>
                            
                                <asp:UpdatePanel ID="updAgregarZonacondicion" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnAgregarZonaCondicion" runat="server" Style="margin-left: 10px"
                                        OnClick="btnAgregarZonaCondicion_Click" CssClass="btn btn-inverse">
                                        <i class="icon-white icon-plus"></i>
                                        <span class="text">Agregar</span>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnCancelarZonaCondicion" runat="server" OnClientClick="return AgregarZonaCondicion(false);" CssClass="btn btn-default">
                                        <i class="imoon imoon-blocked"></i>
                                        <span class="text">Cancelar</span>
                                    </asp:LinkButton>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnlZonasCondicionesEliminadas" runat="server" Visible="false">
                    
                        <asp:Label ID="Label3" runat="server" Text="Condiciones que han sido eliminadas:"
                            Font-Bold="true"></asp:Label>

                        <asp:GridView ID="grdZonasCondicionesEliminadas" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                        <Columns>
                            <asp:BoundField DataField="Zona" HeaderText="Zona" ItemStyle-Width="350px" />
                            <asp:BoundField DataField="Condicion" HeaderText="Condición" />
                            <asp:BoundField DataField="SupMin_condicion" HeaderText="Sup. min." ItemStyle-Width="80px" />
                            <asp:BoundField DataField="SupMax_condicion" HeaderText="Sup. max." ItemStyle-Width="80px" />
                    
                        </Columns>
                        </asp:GridView>



                    </asp:Panel>

                </div>
            </td>
        </tr>
        </table>
                </div>
            </div>
    
    <div class="widget-box">
        <div class="widget-title ">
		    <span class="icon"><i class="imoon imoon-hammer"></i></span>
			<h5>Impacto Ambiental</h5>
        </div>
        <div class="widget-content">
            <table border="0" style="width: 100%">
                <tr>
                    <td colspan="2">
                        <div style="padding: 15px 5px 0px 5px">
                      <%--      <asp:Label ID="lblTituloGrillaImpacto" runat="server" Text="Configuración de Impacto Ambiental:"
                                Font-Bold="true"></asp:Label>--%>
                            <asp:GridView ID="grdImpactoAmbiental" runat="server" AutoGenerateColumns="false"
                                Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="nom_impactoAmbiental" HeaderText="Impacto Ambiental" />
                                    <asp:BoundField DataField="DesdeM2" HeaderText="Desde m2" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="HastaM2" HeaderText="Hasta m2" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="LetraAnexo" HeaderText="Letra" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>

                <%--datos documentos requeridos por todas las zona--%>
                <tr>
                    <td colspan="2">
                        <div style="padding: 15px 5px 0px 5px">
                            <asp:Label ID="Label6" runat="server" Text="Configuración de Documentación Requerida para todas las zonas:" Font-Bold="true"></asp:Label>
                            <%--grilla con datos de documentos requeridos--%>
                            <asp:UpdatePanel ID="pnlGrdDocReq" runat="server" Width="90%" style="text-align:right;padding-top:5px"  > 
                                <ContentTemplate>
                                    <asp:GridView ID="grdDocReq" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="id_rubDocReq_histcam,id_rubtdocreq" Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                        <Columns>   
                                            <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación"  ItemStyle-HorizontalAlign="Left"/>
                                            <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="60px"/>
                            
                                            <asp:TemplateField ItemStyle-Width="80px">
                                            <ItemTemplate>
                        
                                                <asp:LinkButton ID="btnEliminarDocReq" runat="server" Text="Eliminar" 
                                                    CssClass="btnEliminarDocReq" style="display:none"
                                                    CommandArgument='<%# Eval("id_rubDocReq_histcam") %>' 
                                                    OnClick="btnEliminarDocReq_Click"
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                </asp:LinkButton>

                                            </ItemTemplate>
                        
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No se han encontrado Documentos Requeridos para este rubro.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <%--Agregar nueva documentos requeridos--%>
                            <asp:Panel ID="pnlAccionesAgregarDocReq" runat="server" Width="90%" style="text-align:right;padding-top:5px" Visible="false"   > 
                                <asp:LinkButton ID="lnkBtnAccionesAgregarDocReq" runat="server" CssClass="btn btn-primary" 
                                    OnClientClick="return AgregarDocumentoReq(true);">
                                    <i class="icon-white icon-plus"></i>
                                    <span class="text">Agregar Documento Requerido</span>
                                </asp:LinkButton>
                            </asp:Panel>

                            <%--panel para editar documentos requeridos--%>
                            <asp:Panel ID="pnlAgregarDocReqEdit" runat="server" style="display:none" >

                                <div style="padding: 10px 5px 10px 5px" class="info2">
                                    <label>
                                        Para Agregar documentaci&oacute;n requerida al rubro ingrese los datos y luego presione el bot&oacute;n "Agregar"
                                    </label>
                                    <div style="margin-top: 10px">

                                        <asp:HiddenField ID="hid_id_rubtdocreq" runat="server" />

                                        <table>
                                            <tr>
                                                <td>Tipo Documento: 
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditTipoDocReq" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="cv_ddlEditTipoDocReq" runat="server" 
                                                            ErrorMessage="Debe seleccionar el tipo de documento."
                                                            ControlToValidate="ddlEditTipoDocReq" 
                                                            ValidationGroup="guardarTipoDocReq"            
                                                            ValueToCompare="0"
                                                            display="Dynamic"
                                                            Type="Integer" Operator="NotEqual">
                                                    </asp:CompareValidator>  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Documentación Obligatoria
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditObligatorio" runat="server" Width="80px">
                                                        <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                        <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="cv_ddlEditObligatorio" runat="server" 
                                                            ErrorMessage="Debe seleccionar si el documento es obligatorio."
                                                            ControlToValidate="ddlEditObligatorio" 
                                                            ValidationGroup="guardarTipoDocReq"            
                                                            ValueToCompare="0"
                                                            display="Dynamic"
                                                            Type="String" Operator="NotEqual">
                                                    </asp:CompareValidator> 
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <asp:UpdatePanel ID="updPnlAgregarDocReq" runat="server" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnGuardarDocReq" runat="server"  ValidationGroup="guardarTipoDocReq"
                                                                 Style="margin-left: 10px" OnClick="btnGuardarDocReq_Click" CssClass="btn btn-inverse">
                                                                <i class="icon-white icon-plus"></i>
                                                                <span class="text">Agregar</span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnCancelarDocReq" runat="server" 
                                                                 OnClientClick="return AgregarDocumentoReq(false);" CssClass="btn btn-default">
                                                                <i class="imoon imoon-blocked"></i>
                                                                <span class="text">Cancelar</span>
                                                            </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                            </asp:Panel>

                            <%--grilla con de documentos requeridos que seran eliminados--%>
                            <asp:Panel ID="pnlDocReqEliminada" runat="server" Visible="false">
                    
                                <asp:Label ID="Label7" runat="server" Text="Documentación Requerida que ha sido eliminada:"
                                    Font-Bold="true"></asp:Label>

                                <asp:GridView ID="grdDocReqEliminada" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="id_rubDocReq_histcam,id_rubtdocreq" Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación"  ItemStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="60px"/>
                                </Columns>
                                <EmptyDataTemplate>
                                    NO HAY DATOS ...
                                </EmptyDataTemplate>
                                </asp:GridView>



                            </asp:Panel>

                        </div>
                    </td>
                </tr>

                <%--datos documentos requeridos por zona--%>
                <tr>
                    <td colspan="2">
                        <div style="padding: 15px 5px 0px 5px">
                            <asp:Label ID="Label14" runat="server" Text="Configuración de Documentación Requerida por zona:" Font-Bold="true"></asp:Label>
                            <%--grilla con datos de documentos requeridos--%>
                            <asp:UpdatePanel ID="pnlGrdDocReqZona" runat="server" Width="90%" style="text-align:right;padding-top:5px"  > 
                                <ContentTemplate>
                                    <asp:GridView ID="grdDocReqZona" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="id_rubDocReqZona_histcam,id_rubtdocreqzona,codZonaHab" Style="margin-top: 5px" GridLines="None" 
                                        CssClass="table table-bordered table-striped table-hover with-check">
                                        <Columns>   
                                            <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación"  ItemStyle-HorizontalAlign="Left"/>
                                            <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="zonaDesc" HeaderText="Zona Habilitación" HeaderStyle-Width="60px"/>
                            
                                            <asp:TemplateField ItemStyle-Width="80px">
                                            <ItemTemplate>
                        
                                                <asp:LinkButton ID="btnEliminarDocReqZona" runat="server" Text="Eliminar" 
                                                    CssClass="btnEliminarDocReqZona" style="display:none"
                                                    CommandArgument='<%# Eval("id_rubDocReqZona_histcam") %>' 
                                                    OnClick="btnEliminarDocReqZona_Click"
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                </asp:LinkButton>

                                            </ItemTemplate>
                        
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No se han encontrado Documentos Requeridos para este rubro.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <%--Agregar nueva documentos requeridos--%>
                            <asp:Panel ID="pnlAccionesAgregarDocReqZona" runat="server" Width="90%" style="text-align:right;padding-top:5px" Visible="false"   > 
                                <asp:LinkButton ID="lnkBtnAccionesAgregarDocReqZona" runat="server" CssClass="btn btn-primary" 
                                    OnClientClick="return AgregarDocumentoReqZona(true);">
                                    <i class="icon-white icon-plus"></i>
                                    <span class="text">Agregar Documento Requerido por zona</span>
                                </asp:LinkButton>
                            </asp:Panel>

                            <%--panel para editar documentos requeridos--%>
                            <asp:Panel ID="pnlAgregarDocReqEditZona" runat="server" style="display:none" >

                                <div style="padding: 10px 5px 10px 5px" class="info2">
                                    <label>
                                        Para Agregar documentaci&oacute;n requerida al rubro ingrese los datos y luego presione el bot&oacute;n "Agregar"
                                    </label>
                                    <div style="margin-top: 10px">

                                        <asp:HiddenField ID="hid_id_rubtdocreqZona" runat="server" />

                                        <table>
                                            <tr>
                                                <td>Tipo Documento: 
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditTipoDocReqZona" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="cv_ddlEditTipoDocReqZona" runat="server" 
                                                            ErrorMessage="Debe seleccionar el tipo de documento."
                                                            ControlToValidate="ddlEditTipoDocReqZona" 
                                                            ValidationGroup="guardarTipoDocReqZona"            
                                                            ValueToCompare="0"
                                                            display="Dynamic"
                                                            Type="Integer" Operator="NotEqual">
                                                    </asp:CompareValidator>  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Documentación Obligatoria
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditObligatorioZona" runat="server" Width="80px">
                                                        <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                        <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="cv_ddlEditObligatorioZona" runat="server" 
                                                            ErrorMessage="Debe seleccionar si el documento es obligatorio."
                                                            ControlToValidate="ddlEditObligatorioZona" 
                                                            ValidationGroup="guardarTipoDocReqZona"            
                                                            ValueToCompare="0"
                                                            display="Dynamic"
                                                            Type="String" Operator="NotEqual">
                                                    </asp:CompareValidator> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Zona Habilitación
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditZona" runat="server" Width="200px"/>
                                                    <asp:CompareValidator ID="cv_ddlEditZona" runat="server" 
                                                            ErrorMessage="Debe seleccionar la zona."
                                                            ControlToValidate="ddlEditZona" 
                                                            ValidationGroup="guardarTipoDocReqZona"            
                                                            ValueToCompare="0"
                                                            display="Dynamic"
                                                            Type="String" Operator="NotEqual">
                                                    </asp:CompareValidator> 
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <asp:UpdatePanel ID="updPnlAgregarDocReqZona" runat="server" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnGuardarDocReqZona" runat="server"  ValidationGroup="guardarTipoDocReqZona"
                                                                 Style="margin-left: 10px" OnClick="btnGuardarDocReqZona_Click" CssClass="btn btn-inverse">
                                                                <i class="icon-white icon-plus"></i>
                                                                <span class="text">Agregar</span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnCancelarDocReqZona" runat="server" 
                                                                 OnClientClick="return AgregarDocumentoReqZona(false);" CssClass="btn btn-default">
                                                                <i class="imoon imoon-blocked"></i>
                                                                <span class="text">Cancelar</span>
                                                            </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                            </asp:Panel>

                            <%--grilla con de documentos requeridos que seran eliminados--%>
                            <asp:Panel ID="pnlDocReqZonaEliminada" runat="server" Visible="false">
                    
                                <asp:Label ID="Label15" runat="server" Text="Documentación Requerida que ha sido eliminada:"
                                    Font-Bold="true"></asp:Label>

                                <asp:GridView ID="grdDocReqZonaEliminada" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="id_rubDocReqZona_histcam,id_rubtdocreqzona,codZonaHab" Style="margin-top: 5px" GridLines="None" 
                                    CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación"  ItemStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="60px"/>
                                    <asp:BoundField DataField="zonaDesc" HeaderText="Zona Habilitación" HeaderStyle-Width="60px"/>
                                </Columns>
                                <EmptyDataTemplate>
                                    NO HAY DATOS ...
                                </EmptyDataTemplate>
                                </asp:GridView>



                            </asp:Panel>

                        </div>
                    </td>
                </tr>

                <%--datos informacion relevante--%>
                <tr>
                    <td colspan="2">
                        <div style="padding: 15px 5px 0px 5px">
                            <asp:Label ID="Label4" runat="server" Text="Configuración de Información Relevante:"
                                Font-Bold="true"></asp:Label>
                            <%--grilla con datos de info relevante--%>
                            <asp:UpdatePanel ID="pnlGrdInfoRelevante" runat="server" Width="90%" style="text-align:right;padding-top:5px"  > 
                                <ContentTemplate>
                            <asp:GridView ID="grdInfoRelevante" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="id_rubInfRel_histcam, id_rubinf" Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="descripcion_rubinf" HeaderText="Observación" />
                                    <asp:TemplateField ItemStyle-Width="80px">
                                    <ItemTemplate>
                        
                                        <asp:LinkButton ID="btnEliminarInfoRelevante" runat="server" Text="Eliminar" 
                                            CssClass="btnEliminarInfoRelevante" style="display:none"
                                            CommandArgument='<%# Eval("id_rubInfRel_histcam") %>' 
                                            OnClick="btnEliminarInfoRelevante_Click"
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                        </asp:LinkButton>

                                    </ItemTemplate>
                        
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No se han encontrado datos sobre información relevante para este rubro.
                                </EmptyDataTemplate>
                            </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            <%--Agregar nueva info relevante--%>
                            <asp:Panel ID="pnlAccionesAgregarInfoRelevante" runat="server" Width="90%" style="text-align:right;padding-top:5px" Visible="false"   > 
                                <asp:LinkButton ID="lnkBtnAccionesAgregarInfoRelevante" runat="server"
                                                            CssClass="btn btn-primary" 
                                                            OnClientClick="return AgregarInfoRelevante(true);">
                                                        <i class="icon-white icon-plus"></i>
                                                        <span class="text">Agregar Información Relevante</span>
                                                    </asp:LinkButton>
                            </asp:Panel>
                    
                            <%--panel para editar info relevante--%>
                            <asp:Panel ID="pnlAgregarInfoRelevanteEdit" runat="server" style="display:none" >

                                <div style="padding: 10px 5px 10px 5px" class="info2">
                                    <label>
                                        Para Agregar informaci&oacute;n relevante al rubro ingrese el dato y luego presione el bot&oacute;n "Agregar"
                                    </label>
                                    <div style="margin-top: 10px">
                                        <asp:HiddenField ID="hid_id_rubinf" runat="server" />
                                        <table>
                                            <tr>
                                                <td valign="middle">Informaci&oacute;n:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEditDescripInfoRelevante" runat="server" 
                                                        TextMode="MultiLine" MaxLength="500" Height="100px" Width="650px"
                                                       >
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updPnlAgregarInfoRelevante" runat="server" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnGuardarInfoRelevante" runat="server" 
                                                                 Style="margin-left: 10px" OnClick="btnGuardarInfoRelevante_Click" CssClass="btn btn-inverse">
                                                                 <i class="icon-white icon-plus"></i>
                                                                <span class="text">Agregar</span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnCancelarInfoRelevante" runat="server" 
                                                                 OnClientClick="return AgregarInfoRelevante(false);" CssClass="btn btn-default">
                                                                 <i class="imoon imoon-blocked"></i>
                                                                <span class="text">Cancelar</span>
                                                            </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>

                            </asp:Panel>

                            <%--grilla con info relevante que sera eliminada--%>
                            <asp:Panel ID="pnlInfoRelevanteEliminada" runat="server" Visible="false">
                    
                                <asp:Label ID="Label5" runat="server" Text="Información relevante que ha sido eliminada:"
                                    Font-Bold="true"></asp:Label>

                                <asp:GridView ID="grdInfoRelevanteEliminada" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="id_rubInfRel_histcam,id_rubinf" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="descripcion_rubinf" HeaderText="Observación" />
                                </Columns>
                                <EmptyDataTemplate>
                                    NO HAY DATOS ...
                                </EmptyDataTemplate>
                                </asp:GridView>



                            </asp:Panel>

                        </div>
                    </td>
                </tr>

                <%--datos conf incendio--%>
                <tr>
                    <td colspan="2">
                        <div style="padding: 15px 5px 0px 5px">
                            <asp:Label ID="Label23" runat="server" Text="Configuración de Incendio:"
                                Font-Bold="true"></asp:Label>
                            <%--grilla con datos de info relevante--%>
                            <asp:UpdatePanel ID="pnlGrdConfIncendio" runat="server" Width="90%" style="text-align:right;padding-top:5px"> 
                                <ContentTemplate>
                                    <asp:GridView ID="grdConfIncendio" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="id_rubro_incendio_histcam, id_rubro_incendio, id_tdocreq" Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                        <Columns>
                                            <asp:BoundField DataField="riesgo" HeaderText="Riesgo" />
                                            <asp:BoundField DataField="DesdeM2" HeaderText="Sup. Desde" />
                                            <asp:BoundField DataField="HastaM2" HeaderText="Sup. Hasta" />
                                            <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField ItemStyle-Width="80px">
                                            <ItemTemplate>
                        
                                                <asp:LinkButton ID="btnEliminarConfIncendio" runat="server" Text="Eliminar" 
                                                    CssClass="btnEliminarConfIncendio" style="display:none"
                                                    CommandArgument='<%# Eval("id_rubro_incendio_histcam") %>' 
                                                    OnClick="btnEliminarConfIncendio_Click"
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                </asp:LinkButton>

                                            </ItemTemplate>
                        
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No se han encontrado datos sobre configuracion de incendio para este rubro.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <%--Agregar nueva info relevante--%>
                            <asp:Panel ID="pnlAccionesAgregarConfIncendio" runat="server" Width="90%" style="text-align:right;padding-top:5px" Visible="false"   > 
                                <asp:LinkButton ID="lnkBtnAccionesAgregarConfIncendio" runat="server"
                                    CssClass="btn btn-primary" 
                                    OnClientClick="return AgregarConfIncendio(true);">
                                    <i class="icon-white icon-plus"></i>
                                    <span class="text">Agregar Configuración de Incendio</span>
                                </asp:LinkButton>
                            </asp:Panel>
                    
                            <%--panel para editar info relevante--%>
                            <asp:Panel ID="pnlAgregarConfIncendioEdit" runat="server" style="display:none" >

                                <div style="padding: 10px 5px 10px 5px" class="info2">
                                    <label>
                                        Para Agregar Configuración Incendio al rubro ingrese el dato y luego presione el bot&oacute;n "Agregar"
                                    </label>
                                    <div style="margin-top: 10px">
                                        <asp:HiddenField ID="hid_id_rubro_incendio" runat="server" />

                                        <table>
                                            <tr>
                                                <td>Riesgo:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlRiesgo" runat="server" Width="80px">
                                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Sup. Desde:</td>
                                                <td>
                                                    <asp:TextBox ID="txtDesde" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Sup. Hasta:</td>
                                                <td>
                                                    <asp:TextBox ID="txtHasta" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tipo Documento:</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEditTipoDocReqConfInc" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="updPnlAgregarConfIncendio" runat="server" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnGuardarConfIncendio" runat="server" 
                                                                 Style="margin-left: 10px" OnClick="btnGuardarConfIncendio_Click" CssClass="btn btn-inverse">
                                                                 <i class="icon-white icon-plus"></i>
                                                                <span class="text">Agregar</span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnCancelarConfIncendio" runat="server" 
                                                                 OnClientClick="return AgregarConfIncendio(false);" CssClass="btn btn-default">
                                                                 <i class="imoon imoon-blocked"></i>
                                                                <span class="text">Cancelar</span>
                                                            </asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>

                            </asp:Panel>

                            <%--grilla con info relevante que sera eliminada--%>
                            <asp:Panel ID="pnlConfIncendioEliminada" runat="server" Visible="false">
                    
                                <asp:Label ID="Label24" runat="server" Text="Configuracion de incendio que ha sido eliminada:"
                                    Font-Bold="true"></asp:Label>

                                <asp:GridView ID="grdConfIncendioEliminada" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="id_rubro_incendio_histcam, id_rubro_incendio, id_tdocreq" GridLines="None" 
                                    CssClass="table table-bordered table-striped table-hover with-check">
                                <Columns>
                                    <asp:BoundField DataField="riesgo" HeaderText="Riesgo" />
                                    <asp:BoundField DataField="DesdeM2" HeaderText="Sup. Desde" />
                                    <asp:BoundField DataField="HastaM2" HeaderText="Sup. Hasta" />
                                    <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                                <EmptyDataTemplate>
                                    NO HAY DATOS ...
                                </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Panel>

        <div class="widget-box">
            <div class="widget-content">
<%--edit datos de la solicitud de cambio--%>
<asp:UpdatePanel ID="updEstadosSolicitudCambio" runat="server">
<ContentTemplate>

    <asp:Panel ID="pnlEstados" runat="server" Visible="false" Style="margin-top: 10px">
        <label>Estado de la solicitud de cambio:</label>
        <asp:DropDownList ID="ddlEstado" runat="server" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" >
        </asp:DropDownList>
    
        <table border="0" style="width:100%; margin-top:10px">
            <tr>
                <td style="width:100px;vertical-align:text-top">
                    <label>Observaciones:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtObservacionesSolicitudCambio" runat="server" TextMode="MultiLine" 
                        MaxLength="1000" Height="100px" Width="90%"
                        >
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width:100px;vertical-align:text-top">
                    <label>Observaciones del Solicitante:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtObservacionesSolicitante" runat="server" TextMode="MultiLine" 
                        MaxLength="1000" Height="100px" Width="90%"
                        >
                    </asp:TextBox>
                </td>
            </tr>
    
        </table>
    </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>


<div style="text-align: center; width: 100%; margin-top: 10px">

    <asp:UpdatePanel ID="updGuardar" runat="server">
    <ContentTemplate>
        <center>
            <table border="0">
            <tr>
            <td>
                <asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnGuardar_Click"
                    Visible="false" ValidationGroup="Guardar" CssClass="btn btn-inverse">
                     <i class="imoon-white imoon-save"></i>
                     <span class="text">Guardar</span>
               </asp:LinkButton>

                <asp:LinkButton ID="btnNuevabusqueda" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();">
                       <i class="imoon imoon-blocked"></i>
                       <span class="text">Nueva Búsqueda</span>
                </asp:LinkButton>
            </td>
            <td>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="updGuardar" runat="server" >
                    <ProgressTemplate>
                        <asp:Image ID="imgProgress1" runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
            </tr>
            </table>
        </center>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>

                </div>
            </div>
    <%--modal de Errores--%>
    <div id="frmErrorVisual" class="modal fade">
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
                                        <asp:Label ID="lblError" runat="server" Style="color: Black"></asp:Label>
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
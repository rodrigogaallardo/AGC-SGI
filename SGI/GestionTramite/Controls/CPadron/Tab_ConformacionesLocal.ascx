<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_ConformacionesLocal.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tab_ConformacionesLocal" %>

<%: Scripts.Render("~/bundles/autoNumeric") %>

<asp:UpdatePanel ID="Hiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_cpadron" runat="server" />
        <asp:HiddenField ID="hid_id_encomienda" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_DecimalSeparator1" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<h3 style="line-height: 20px;">Conformacion del Local</h3>

<%--Box--%>
<div id="box_datoslocal" class="accordion-group widget-box mtop20">
    <asp:Panel ID="pnlPage" runat="server" CssClass="PageContainer">

        <%--Contenido--%>
        <asp:Panel ID="pnlContenido" runat="server" CssClass="box-contenido" BackColor="White">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:HiddenField ID="HiddenField2" runat="server" />

            <asp:Panel ID="pnlInfoPaso" runat="server">
                <div class="titulo-5" style="margin-top: 20px;">
                    <asp:Label ID="lblTituloInfoIngresada" runat="server" Style="padding-left: 15px">
                            Información ingresada en el trámite</asp:Label>
                </div>
                <asp:UpdatePanel ID="updConformacionIngresada" runat="server">
                    <ContentTemplate>
                        <div style="padding: 0px 20px 20px 20px; width: 100%">

                            <asp:GridView ID="grdConformacionLocal" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="id_cpadronconflocal" AllowPaging="false" ShowHeader="false"
                                Style="border: none; margin-top: 10px" GridLines="None" Width="900px"
                                OnRowCommand="grdConformacionLocal_RowCommand" CellPadding="3">


                                <HeaderStyle CssClass="grid-header" HorizontalAlign="Left" />
                                <PagerStyle CssClass="grid-pager" HorizontalAlign="Center" />
                                <FooterStyle CssClass="grid-footer" HorizontalAlign="Left" />
                                <RowStyle CssClass="grid-row" HorizontalAlign="Left" />
                                <AlternatingRowStyle CssClass="grid-alternating-row" HorizontalAlign="Left" />
                                <EditRowStyle CssClass="grid-edit-row" HorizontalAlign="Left" />
                                <SelectedRowStyle CssClass="grid-selected-row" HorizontalAlign="Left" />


                                <Columns>

                                    <asp:TemplateField ItemStyle-Width="100%">
                                        <ItemTemplate>

                                            <table>

                                                <tr>

                                                    <td style="text-align: right">Destino: 
                                                    </td>
                                                    <td style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtGrillaDestino" runat="server" Text='<% #Eval("desc_tipodestino") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right">Planta: 
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaPlanta" runat="server" Text='<% #Eval("desc_planta") %>'
                                                            Enabled="false" Width="100%" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                    <td style="padding-left: 15px">Observaciones:
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 70px; text-align: right">Largo: 
                                                    </td>

                                                    <td style="width: 40px; padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaLargo" runat="server" Text='<% #Eval("largo_conflocal") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>

                                                    <td style="width: 60px; text-align: right">Paredes:
                                                    </td>

                                                    <td style="width: 400px; padding-left: 5px">
                                                        <asp:TextBox ID="txtlGrillaPared" runat="server" Text='<% #Eval("Paredes_conflocal") %>'
                                                            Enabled="false" Width="100%" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                    <td rowspan="4" style="padding-left: 15px; text-align: left; vertical-align: top;">
                                                        <asp:TextBox ID="txtlGrillaObserv" runat="server" Text='<% #Eval("Observaciones_conflocal") %>'
                                                            Enabled="false" Width="200px" Height="100px"
                                                            TextMode="MultiLine" Style="font-size: 11px; overflow: auto;"></asp:TextBox>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="text-align: right">Ancho: 
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaAncho" runat="server" Text='<% #Eval("ancho_conflocal") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>

                                                    <td style="text-align: right">Techos:
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaTecho" runat="server" Text='<% #Eval("Techos_conflocal") %>'
                                                            Enabled="false" Width="100%" Style="font-size: 11px"></asp:TextBox>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td style="text-align: right">Alto: 
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaAlto" runat="server" Text='<% #Eval("alto_conflocal") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>

                                                    <td style="text-align: right">Pisos:
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaPiso" runat="server" Text='<% #Eval("Pisos_conflocal") %>'
                                                            Enabled="false" Width="100%" Style="font-size: 11px"></asp:TextBox>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td style="text-align: right">Tipo Superficie: 
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="TextBox1" runat="server"
                                                            Text='<% # Eval("desc_tiposuperficie") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right">Sup Estimada: 
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaSupEstimada" runat="server"
                                                            Text='<% # decimal.Parse( Eval("superficie_conflocal").ToString() ).ToString("N2")  %>'
                                                            Enabled="false" Width="100%" Style="font-size: 11px"></asp:TextBox>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td style="text-align: right">Frisos:
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaFriso" runat="server"
                                                            Text='<% # Eval("Frisos_conflocal") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right"></td>
                                                    <td style="padding-left: 5px"></td>

                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Ventilación: 
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="txtGrillaVentilacion" runat="server" Text='<% #Eval("desc_ventilacion") %>'
                                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right">Iluminación:
                                                    </td>

                                                    <td style="padding-left: 5px">
                                                        <asp:TextBox ID="TextBox3" runat="server" Text='<% #Eval("desc_iluminacion") %>'
                                                            Enabled="false" Width="100%" Style="font-size: 11px"></asp:TextBox>
                                                    </td>
                                                </tr>


                                            </table>


                                            <div style="padding: 10px">
                                                <center>
                                                        <asp:Button ID="btnEditarDetalleConformacion" runat="server" CssClass="btnEdit" Width="70px"
                                                            CommandName="EditarDetalle" CommandArgument='<% #Eval("id_cpadronconflocal") %>' 
                                                            Text="Editar" data-group="controles-accion"/>
                    
                                                        <asp:Button ID="btnEliminarDetalleConformacion" runat="server" CssClass="btnClose" Width="90px"
                                                            CommandName="EliminarDetalle" CommandArgument='<%# Eval("id_cpadronconflocal") %>'
                                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este detalle de la conformación del local?');"
                                                            Text="Eliminar" data-group="controles-accion"/>
                                                    </center>

                                            </div>

                                            <hr />

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                                <EmptyDataTemplate>
                                    <div class="titulo-4">
                                        No hay datos aún...
                                    </div>
                                </EmptyDataTemplate>

                            </asp:GridView>

                        </div>

                        <div style="padding: 10px">
                            <table>
                                <tr>
                                    <td style="text-align: right">Superficie Total Estimada:
                                    </td>

                                    <td style="padding-left: 5px">
                                        <asp:TextBox ID="txtSupTotal" runat="server" Text=''
                                            Enabled="false" Width="150px" Style="font-size: 11px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="updIngresarDetalleConformacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row ptop10 pright15">
                            <div class="cols-sm-12 text-right">
                                <asp:LinkButton ID="lnkIngresarDetalleConformacion" runat="server" CssClass="btn btn-default" OnClick="lnkIngresarDetalleConformacion_Click"
                                    data-group="controles-accion">
                                        <i class="imoon imoon-user"></i>
                                        <span class="text">Ingresar Detalle</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </asp:Panel>

        </asp:Panel>

        <%--Cierre contenido--%>
        <div class="footer">
        </div>

    </asp:Panel>
    <div class="footer-sombra">
    </div>
</div>



<%--Popup Agregar Detalle Conformacion CssClass="modalPopup"--%>
<div id="frmAgregarDetalleConformacion" class="modal fade" style="display: none; overflow-y: auto; width: 650px">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Agregar detalle de conformación de local</h4>
            </div>
            <div class="modal-body pbottom0">
                <asp:UpdatePanel ID="updDetalleConformacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_conflocal" runat="server" />
                        <div style="padding: 10px 0px 10px 0px">

                            <%--combo destino--%>
                            <div class="fila1">
                                <asp:Label ID="lblDestino" runat="server" Text="Destino:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:DropDownList ID="ddlTipoDestino" runat="server" Width="350px"
                                    AutoPostBack="false" onchange="cambioTipoDestino();">
                                </asp:DropDownList>
                            </div>

                            <%--campo detalle--%>
                            <div id="pnlDetalle" runat="server" class="fila1" style="display: none;">
                                <asp:Label ID="Label1" runat="server" Text="Detalle:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:TextBox ID="txtDetalle" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                <div>
                                    <asp:Label ID="Label14" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqtxtDetalle" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el detalle de manera obligatoria.
                                    </div>
                                </div>

                            </div>

                            <%--campo largo, ancho y alto--%>
                            <div class="fila1">
                                <div style="display: table; margin-top: 6px; margin-bottom: 5px;">
                                    <div style="display: table-row;">
                                        <div style="display: table-cell; width: 100px;">
                                            <asp:Label ID="Label2" runat="server" Text="Largo:" Width="100px" CssClass="label1"></asp:Label>
                                        </div>
                                        <div style="display: table-cell; padding-left: 3px; width: 100px">
                                            <asp:TextBox ID="txtLargo" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                        </div>
                                        <div style="display: table-cell; width: 50px">
                                            <asp:Label ID="Label3" runat="server" Text="Ancho:" Width="50px" CssClass="label1" Style="padding-left: 5px"></asp:Label>
                                        </div>
                                        <div style="display: table-cell; width: 100px">
                                            <asp:TextBox ID="txtAncho" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                        </div>
                                        <div style="display: table-cell; width: 50px">
                                            <asp:Label ID="Label4" runat="server" Text="Alto:" Width="50px" CssClass="label1" Style="padding-left: 5px"></asp:Label>
                                        </div>
                                        <div style="display: table-cell; width: 100px">
                                            <asp:TextBox ID="txtAlto" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div style="display: table-row;">
                                        <div style="display: table-cell; width: 100px">
                                        </div>
                                        <div style="display: table-cell; padding-left: 3px; padding-top: 3px; width: 100px">
                                            <div id="ReqtxtLargo" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Dato obligatorio.
                                            </div>
                                        </div>
                                        <div style="display: table-cell; width: 50px">
                                        </div>
                                        <%--  style="margin-left:90px"--%>
                                        <div style="display: table-cell; width: 100px">
                                            <div id="ReqtxtAncho" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Dato obligatorio.
                                            </div>
                                        </div>
                                        <div style="display: table-cell; width: 50px">
                                        </div>
                                        <%--  style="margin-left:90px"--%>
                                        <div style="display: table-cell; width: 100px">
                                            <div id="ReqtxtAlto" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Dato obligatorio.
                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>

                            <%--campo tipo superficie--%>
                            <div class="fila1">
                                <div style="display: table; margin-top: 6px; margin-bottom: 5px;">

                                    <div style="display: table-row;">
                                        <div style="display: table-cell; width: 100px;">
                                            <asp:Label ID="Label19" runat="server" Text="Tipo de Superficie:" Width="100px" CssClass="label1"></asp:Label>
                                        </div>
                                        <div style="display: table-cell; padding-left: 3px; width: 100px;">
                                            <asp:DropDownList ID="ddlTipoSuperficie" runat="server" Width="250px"
                                                AutoPostBack="false" onchange="cambioTipoSuperficie();">
                                            </asp:DropDownList>
                                        </div>
                                        <div style="display: table-cell; width: 100px;">
                                            <asp:Label ID="Label20" runat="server" Text="Superficie:" Width="100px" CssClass="label1"></asp:Label>
                                        </div>
                                        <div style="display: table-cell; width: 100px;">
                                            <asp:TextBox ID="txtSuperficie" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div style="display: table-row;">
                                        <div style="display: table-cell; width: 100px;">
                                        </div>
                                        <div style="display: table-cell; padding-left: 3px; width: 100px;">
                                            <div id="ReqTipoSuperficie" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe indicar el Tipo de Superficie.
                                            </div>
                                        </div>
                                        <div style="display: table-cell; width: 100px;">
                                        </div>
                                        <div style="display: table-cell; width: 100px;">
                                            <div id="ReqSuperficie" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Dato obligatorio.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--campo planta--%>
                            <div class="fila1">
                                <asp:Label ID="lblPlanta" runat="server" Text="Planta:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:DropDownList ID="ddlPlanta" runat="server" Width="350px"></asp:DropDownList>
                                <div>
                                    <asp:Label ID="Label15" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqPlanta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe indicar la planta.
                                    </div>
                                </div>
                            </div>

                            <%--campo paredes--%>
                            <div class="fila1">
                                <asp:Label ID="Label5" runat="server" Text="Paredes:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:TextBox ID="txtParedes" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                <div>
                                    <asp:Label ID="Label10" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqtxtParedes" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el material de las paredes de manera obligatoria.
                                    </div>
                                </div>
                            </div>

                            <%--campo techo--%>
                            <div class="fila1">
                                <asp:Label ID="Label6" runat="server" Text="Techos:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:TextBox ID="txtTechos" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                <div>
                                    <asp:Label ID="Label18" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqtxtTechos" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el material de los techos de manera obligatoria.
                                    </div>
                                </div>
                            </div>

                            <%--campo piso--%>
                            <div class="fila1">
                                <asp:Label ID="Label7" runat="server" Text="Pisos:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:TextBox ID="txtPisos" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                <div>
                                    <asp:Label ID="Label12" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqtxtPisos" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el material del piso de manera obligatoria.
                                    </div>
                                </div>
                            </div>

                            <%--campo ventilacion--%>
                            <div class="fila1">
                                <asp:Label ID="lblVentilacion" runat="server" Text="Ventilación:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:DropDownList ID="ddlVentilacion" runat="server" Width="350px"></asp:DropDownList>
                                <div>
                                    <asp:Label ID="Label17" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqVentilacion" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe indicar el tipo de ventilación.
                                    </div>
                                </div>
                            </div>

                            <%--campo iluminacion--%>
                            <div class="fila1">
                                <asp:Label ID="lblIluminacion" runat="server" Text="Iluminaciòn:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:DropDownList ID="ddlIluminacion" runat="server" Width="350px"></asp:DropDownList>
                                <div>
                                    <asp:Label ID="Label16" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqIluminacion" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe indicar el tipo de iluminación.
                                    </div>
                                </div>
                            </div>

                            <%--campo Friso--%>
                            <div class="fila1">
                                <asp:Label ID="Label8" runat="server" Text="Frisos:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:TextBox ID="txtFrisos" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                <div>
                                    <asp:Label ID="Label11" runat="server" Text="" Width="100px" CssClass="label1"></asp:Label>
                                    <div id="ReqtxtFrisos" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el material de los revestimientos de manera obligatoria.
                                    </div>
                                </div>
                            </div>

                            <%--campo observaciones--%>
                            <div class="fila1" id="pnlObservaciones">
                                <asp:Label ID="Label9" runat="server" Text="Observaciones:" Width="100px" CssClass="label1"></asp:Label>
                                <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                            </div>

                        </div>
                        <div class="modal-footer mtop10">
                            <asp:UpdatePanel ID="updBotonesIngresarDetalleConformacion" runat="server">
                                <ContentTemplate>

                                    <div class="form-inline">
                                        <div class="form-group">
                                            <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="updBotonesIngresarDetalleConformacion">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />Guardando...
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                        <div id="pnlBotonesAgregarConfirmacion" class="form-group">
                                            <asp:LinkButton ID="btnAceptarConformacion" runat="server" CssClass="btn btn-primary"
                                                OnClientClick="return validarAgregarDetalleConfirmacion();"
                                                OnClick="btnAceptarConformacion_Click">
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


                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:UpdatePanel ID="updTblTipoDestino" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table ID="tblTipoDestino" runat="server" Style="display: none">
                    <%--Desde js se usa esta tabla pasa saber cuando mostrar o no el campo detalle --%>
                </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<%--Modal mensajes de error--%>
<div id="frmError_Conformacion" class="modal fade" style="display: none;">
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
    var vSeparadorDecimal1;
    $(document).ready(function () {
        camposAutonumericos();
    });

    function showfrmAgregarDetalleConformacion() {
        $("#frmAgregarDetalleConformacion").modal({
            "show": true,
            "backdrop": "static"
        });
        return false;
    }

    function hidefrmAgregarDetalleConformacion() {
        $("#frmAgregarDetalleConformacion").modal("hide");
        return false;
    }

    function showfrmError_DetalleConformacion() {
        $("#frmError_Conformacion").modal("show");
        return false;
    }

    function camposAutonumericos() {
        vSeparadorDecimal1 = $("#<%: hid_DecimalSeparator1.ClientID %>").attr("value");

            eval("$('#<%: txtLargo.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal1 + "', mDec: '2',vMax: '9999.99'})");
            eval("$('#<%: txtAncho.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal1 + "', mDec: '2',vMax: '9999.99'})");
            eval("$('#<%: txtAlto.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal1 + "', mDec: '2',vMax: '9999.99'})");
            eval("$('#<%: txtSuperficie.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal1 + "', mDec: '2',vMax: '999999.99'})");

            $("#<%: txtLargo.ClientID %>").on("keyup", function () {
                cambioTipoSuperficie();
            });

            $('#<%: txtAncho.ClientID %>').on("keyup", function () {
                cambioTipoSuperficie();
            });
            return false;
        }

        function cambioTipoDestino() {
            var combo = $("#<%: ddlTipoDestino.ClientID %>");
            var detalleVisible = false;
            var valor = $(combo).val();
            $("#<%: tblTipoDestino.ClientID %> tr").each(function (indexFila) {
                var celdaCodigo = 0;
                var celdaDetalle = false;
                //buscar el codigo de tipo destino y campo que indica visible o no el detalle
                $(this).children("td").each(function (indexColumna) {
                    switch (indexColumna) {
                        case 0:
                            celdaCodigo = $(this).text();
                            break;
                        case 1:
                            celdaDetalle = $(this).text();
                            break;
                    }
                });
                if (celdaCodigo == valor) {
                    detalleVisible = celdaDetalle;
                }
            });

            if (detalleVisible == "true") {
                $("#<%: pnlDetalle.ClientID %>").slideDown("slow");
            }
            else {
                $("#<%: pnlDetalle.ClientID %>").hide("slow");
            }

            return false;
        }

        function cambioTipoSuperficie() {
            var combo = $("#<%: ddlTipoSuperficie.ClientID %>");
            var detalleVisible = false;
            var valor = $(combo).val();
            if (valor == 1) {
                var sup = stringToFloat($("#<%: txtAncho.ClientID %>").val(), vSeparadorDecimal1) * stringToFloat($("#<%: txtLargo.ClientID %>").val(), vSeparadorDecimal1);
                $("#<%: txtSuperficie.ClientID %>").val(sup.toFixed(2).toString().replace(".", vSeparadorDecimal1));
                $("#<%: txtSuperficie.ClientID %>").prop("disabled", true);
            } else
                $("#<%: txtSuperficie.ClientID %>").prop("disabled", false);
            return false;
        }

        function validarAgregarDetalleConfirmacion() {

            var ret = true;
            $("#ReqtxtDetalle").hide();
            $("#ReqtxtLargo").hide();
            $("#ReqtxtAncho").hide();
            $("#ReqtxtAlto").hide();
            $("#ReqTipoSuperficie").hide();
            $("#ReqSuperficie").hide();
            $("#ReqPlanta").hide();
            $("#ReqtxtParedes").hide();
            $("#ReqtxtTechos").hide();
            $("#ReqtxtPisos").hide();
            $("#ReqVentilacion").hide();
            $("#ReqIluminacion").hide();
            $("#ReqtxtFrisos").hide();

            if ($("#<%: pnlDetalle.ClientID %>").is(":visible") &&
                $.trim($("#<%: txtDetalle.ClientID %>").val()).length == 0) {
                $("#ReqtxtDetalle").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtLargo.ClientID %>").val()).length == 0) {
                $("#ReqtxtLargo").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtAncho.ClientID %>").val()).length == 0) {
                $("#ReqtxtAncho").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtAlto.ClientID %>").val()).length == 0) {
                $("#ReqtxtAlto").css("display", "inline-block");
                ret = false;
            }
            if ($("#<%: ddlTipoSuperficie.ClientID %>").val() == 0) {
                $("#ReqTipoSuperficie").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtSuperficie.ClientID %>").val()).length == 0) {
                $("#ReqSuperficie").css("display", "inline-block");
                ret = false;
            }
            if ($("#<%: ddlPlanta.ClientID %>").val() == 0) {
                $("#ReqPlanta").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtParedes.ClientID %>").val()).length == 0) {
                $("#ReqtxtParedes").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtTechos.ClientID %>").val()).length == 0) {
                $("#ReqtxtTechos").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtPisos.ClientID %>").val()).length == 0) {
                $("#ReqtxtPisos").css("display", "inline-block");
                ret = false;
            }
            if ($("#<%: ddlVentilacion.ClientID %>").val() == -1) {
                $("#ReqVentilacion").css("display", "inline-block");
                ret = false;
            }
            if ($("#<%: ddlIluminacion.ClientID %>").val() == 0) {
                $("#ReqIluminacion").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtFrisos.ClientID %>").val()).length == 0) {
                $("#ReqtxtFrisos").css("display", "inline-block");
                ret = false;
            }
            if (ret) {
                $("#pnlBotonesAgregarConfirmacion").hide();
            }
            return ret;
        }

</script>

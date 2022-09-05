<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_Titulares.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tab_Titulares" %>

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
        <h3 style="line-height: 20px;">Titulares y Firmantes</h3>
    </div>
    <div id="box_titulares" class="accordion-group widget-box" runat="server">

        <div class="accordion-heading">
            <a data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-users"></i></span>
                    <h5>vali
                        <asp:Label ID="Label1" runat="server" Text="Titulares"></asp:Label></h5>

                </div>
            </a>
        </div>

        <asp:UpdatePanel ID="updShowAgregarPersonas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row ptop10 pright15">
                    <div class="cols-sm-12 text-right">
                        <asp:LinkButton ID="btnShowAgregarPF" runat="server" CssClass="btn btn-default" OnClick="btnShowAgregarTitular_Click" data-group="controles-accion">
                            <i class="imoon imoon-user" style="color:#377bb5"></i>
                            <span class="text">Agregar Persona F&iacute;sica</span>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnShowAgregarPJ" runat="server" CssClass="btn btn-default" OnClick="btnShowAgregarTitular_Click" data-group="controles-accion">
                            <i class="imoon imoon-office" style="color:#377bb5"></i>
                            <span class="text">Agregar Persona Jur&iacute;dica</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="accordion-body collapse in" id="collapse_titulares">
            <div class="widget-content">

                <asp:UpdatePanel ID="updGrillaTitulares" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <%--Grilla de Titulares--%>
                        <div>
                            <strong>Titulares</strong>
                        </div>

                        <div>
                            <asp:GridView ID="grdTitularesHab" runat="server" AutoGenerateColumns="false" DataKeyNames="id_persona"
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

                                    <asp:TemplateField HeaderText="Acción" ItemStyle-Width="50px">
                                        <ItemTemplate>

                                            <asp:LinkButton ID="btnEditarTitular" runat="server" CommandName='<%#Eval("TipoPersona") %>' CommandArgument='<%# Eval("id_persona") %>'
                                                title="Editar" CssClass="link-local" data-toggle="tooltip" OnClick="btnEditarTitular_Click" data-group="controles-accion">
                                                        <i class="imoon imoon-pencil2" style="color:#377bb5;font-size:medium;margin-left:5px" ></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnEliminarTitular" runat="server" CommandName='<%#Eval("TipoPersona") %>' CommandArgument='<%# Eval("id_persona") %>'
                                                title="Eliminar" CssClass="link-local" data-toggle="tooltip" OnClick="btnEliminarTitular_Click" data-group="controles-accion">
                                                        <i class="imoon imoon-close" style="color:#377bb5;font-size:medium;margin-left:5px"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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



                <%--Grilla de Titulares--%>
                <div>
                    <strong>Titulares</strong>
                </div>

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

    <%--Botones de Guardado--%>
    <asp:UpdatePanel ID="updBotonesGuardar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="form-inline text-right mtop20">
                <div id="pnlBotonesGuardar_Titulares" class="form-group">
                </div>
                <div class="form-group">
                    <asp:UpdateProgress ID="UpdateProgress9" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updBotonesGuardar">
                        <ProgressTemplate>
                            <img src='<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>' style="margin-left: 10px" alt="loading" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</div>

<div class="modal hide fade">
    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h3>Modal header</h3>
    </div>
    <div class="modal-body">
        <p>One fine body…</p>
    </div>
    <div class="modal-footer">
        <a href="#" class="btn">Close</a>
        <a href="#" class="btn btn-primary">Save changes</a>
    </div>
</div>

<%--Modal Agregar Persona Física--%>
<div id="frmAgregarPersonaFisica" class="modal fade" style="margin-left: -25%; width: 50%; display: none; max-height: 90%; overflow: auto">
    <asp:UpdatePanel ID="updAgregarPersonaFisica" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Agregar Persona F&iacute;sica</h4>
                    </div>
                    <div class="modal-body pbottom0">
                        <asp:HiddenField ID="hid_id_titular_pf" runat="server" />
                        <div style="float: left; width: 50%;">

                            <div class="form-inline pright10 pleft10">

                                <div class="form-group">
                                    <label class="control-label col-sm-2">Apellido/s (*):</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtApellidosPF" Width="80%" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                        <div id="Req_ApellidoPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el/los Apellido/s.
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group ">
                                    <label class="control-label col-sm-2">Nombre/s (*):</label>
                                    <div class="col-sm-4">

                                        <asp:TextBox ID="txtNombresPF" Width="80%" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                        <div id="Req_NombresPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el/los Nombres/s.
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-2">Tipo y Nro de doc.:</label>
                                    <div class="col-sm-4">
                                        <div class="form-inline">
                                            <asp:DropDownList ID="ddlTipoDocumentoPF" runat="server" Width="35%" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtNroDocumentoPF" runat="server" MaxLength="15" Width="30%" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <%--                                        <div id="Req_TipoNroDocPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el Tipo y Nro. de doc.
                                        </div>--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-2">Cuit:</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtCuitPF" runat="server" MaxLength="13" Width="60%" CssClass="form-control"></asp:TextBox>
                                        <div id="Req_CuitPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el CUIT.
                                        </div>
                                        <%--                                        <div id="ValFormato_CuitPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            formato inv&aacute;lido. Ej: 20012345673
                                        </div>
                                        <div id="ValDV_CuitPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            El CUIT ingresado es inv&aacute;lido.
                                        </div>
                                        <div id="ValDNI_CuitPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            El CUIT ingresado es distinto al DNI.
                                        </div>--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label">Tipo Ing. Brutos:</label>
                                    <div class="col-sm-4">
                                        <asp:UpdatePanel ID="upd_ddlTipoIngresosBrutosPF" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlTipoIngresosBrutosPF" runat="server" Width="65%" CssClass="form-control"
                                                    OnSelectedIndexChanged="ddlTipoIngresosBrutosPF_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>

                                                <asp:HiddenField ID="hid_IngresosBrutosPF_expresion" runat="server" />
                                                <asp:HiddenField ID="hid_IngresosBrutosPF_formato" runat="server" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--                                        <div id="Req_TipoIngresosBrutosPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe seleccionar el tipo de Ingresos Brutos.
                                        </div>--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-2">Nº Ing. Brutos:</label>
                                    <div class="col-sm-4">
                                        <asp:UpdatePanel ID="upd_txtIngresosBrutosPF" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtIngresosBrutosPF" runat="server" MaxLength="20" Width="60%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--                                        <div id="Req_IngresosBrutosPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el Nro de Ing. Brutos.
                                        </div>
                                        <div id="ValFormato_IngresosBrutosPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Solo se permiten Nros. y guiones.
                                        </div>--%>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div style="float: left; width: 50%">

                            <div class="form-inline pright10">

                                <div class="form-group">
                                    <label class="control-label col-sm-2">Calle (*):</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtCallePF" runat="server" MaxLength="50" Width="80%" CssClass="form-control"></asp:TextBox>
                                        <div id="Req_CallePF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar la Calle.
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-2">Nro de Puerta (*):</label>
                                    <div class="col-sm-4">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtNroPuertaPF" runat="server" MaxLength="5" Width="20%" CssClass="form-control"></asp:TextBox>
                                            <label class="pleft5 pright5">Piso:</label>
                                            <asp:TextBox ID="txtPisoPF" runat="server" MaxLength="5" Width="10%" CssClass="form-control"></asp:TextBox>
                                            <label class="pleft5 pright5">Depto:</label>
                                            <asp:TextBox ID="txtDeptoPF" runat="server" MaxLength="5" Width="10%" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div id="Req_NroPuertaPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el Nro de Puerta.
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-2">C&oacute;digo Postal:</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtCPPF" runat="server" MaxLength="8" Style="text-transform: uppercase" Width="52%" CssClass="form-control"></asp:TextBox>

                                        <%--                                        <div id="Req_CPPF" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el c&oacute;digo postal.
                                        </div>
                                        <div id="Val_Formato_CPPF" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            El formato es inv&aacute;lido, el mismo debe se por ej: 1093 o C1093AAC.
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2">Provincia:</label>
                                    <div class="col-sm-4">
                                        <asp:UpdatePanel ID="updProvinciasPF" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlProvinciaPF" runat="server" Width="65%" AutoPostBack="true" CssClass="form-control"
                                                    OnSelectedIndexChanged="ddlProvinciaPF_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <%--                                        <div id="Req_ProvinciaPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar la Provincia.
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2">Localidad:</label>
                                    <div class="col-sm-4">
                                        <asp:UpdatePanel ID="updLocalidadPF" runat="server">
                                            <ContentTemplate>
                                                <div class="form-inline">

                                                    <asp:DropDownList ID="ddlLocalidadPF" runat="server" Width="65%" CssClass="form-control">
                                                    </asp:DropDownList>

                                                    <div class="pull-right">

                                                        <asp:UpdateProgress ID="UpdateProgress7" AssociatedUpdatePanelID="updProvinciasPF" runat="server" DisplayAfter="0">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>

                                                    </div>

                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--                                        <div id="Req_LocalidadPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar la Localidad.
                                        </div>--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-2">E-mail:</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtEmailPF" runat="server" Width="60%" CssClass="form-control"></asp:TextBox>

                                        <%--                                        <div id="Req_EmailPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el E-mail.
                                        </div>
                                        <div id="ValFormato_EmailPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            formato inv&aacute;lido. Ej: nombre@servidor.com
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="form-group">

                                    <label class="control-label col-sm-2">Tel&eacute;fono M&oacute;vil:</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtTelefonoMovilPF" runat="server" Width="60%" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">

                                    <label class="control-label col-sm-2">Tel&eacute;fono:</label>
                                    <div class="col-sm-4">
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtTelefonoPF" runat="server" Width="46%" MaxLength="18" CssClass="form-control"></asp:TextBox>
                                            <div style="font-size: 9pt">
                                                <label>(Area + Prefijo + Sufijo)</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <%--                <div style="float:left;width: 100%;display:none">
                            <asp:Panel ID="pnlMismoTitular" runat="server" CssClass="form-inline pright10">

                                <label class="radio-inline mleft20">
                                    <asp:RadioButton ID="optMismaPersona" runat="server"
                                        GroupName="MismoFirmante" Checked="true" /><strong>El firmante es la misma persona</strong>
                                </label>
                                <label class="radio-inline">
                                    <asp:RadioButton ID="optOtraPersona" runat="server" GroupName="MismoFirmante"  /><strong>El firmante es otra persona (Apoderado).</strong>
                                </label>

                                <asp:Panel ID="pnlOtraPersona" runat="server" Style="display: none">
                                    <asp:UpdatePanel ID="updFirmantePF" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <div class="form-horizontal pright10 ptop15">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-2">Apellido/s (*):</label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtApellidoFirPF" runat="server" MaxLength="50" Width="200px" CssClass="form-control"></asp:TextBox>
                                                        <div id="Req_ApellidoFirPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                            Debe ingresar el/los Apellido/s.
                                                        </div>
                                                    </div>
                                                    <label class="control-label col-sm-2">Nombre/s (*):</label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtNombresFirPF" runat="server" MaxLength="50" Width="250px" CssClass="form-control"></asp:TextBox>
                                                        <div id="Req_NombresFirPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                            Debe ingresar el/los Nombres/s.
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-2">Tipo y Nº de Doc (*):</label>
                                                    <div class="col-sm-4">
                                                        <div class="form-inline">
                                                            <asp:DropDownList ID="ddlTipoDocumentoFirPF" runat="server" Width="120px" CssClass="form-control">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtNroDocumentoFirPF" runat="server" MaxLength="15" Width="140px" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div id="Req_TipoNroDocFirPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                            Debe ingresar el Tipo y Nro. de doc.
                                                        </div>
                                                    </div>
                                                    <label class="control-label col-sm-2">Car&aacute;cter Legal (*):</label>
                                                    <div class="col-sm-4">
                                                        <div class="form-inline">

                                                            <asp:DropDownList ID="ddlTipoCaracterLegalFirPF" runat="server" Width="250px" CssClass="form-control">
                                                            </asp:DropDownList>
                                                            <div id="Req_TipoCaracterLegalFirPF_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                Debe seleccionar Car&aacute;cter Legal.
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </asp:Panel>
                        </div>--%>
                    </div>
                    <div class="modal-footer mtop0">

                        <asp:UpdatePanel ID="updBotonesAgregarPF" runat="server">
                            <ContentTemplate>

                                <div class="form-inline">

                                    <div class="form-group pull-left">
                                        <asp:Panel ID="ValExiste_TitularPF" runat="server" CssClass="alert alert-small alert-danger mbottom0" Style="display: none;">
                                            Ya existe un titular con el mismo número de CUIT.
                                        </asp:Panel>
                                    </div>
                                    <div class="form-group">
                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="updBotonesAgregarPF">
                                            <ProgressTemplate>
                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />Guardando...
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div id="pnlBotonesAgregarPF" class="form-group">

                                        <asp:LinkButton ID="btnAceptarTitPF" runat="server" Style="color: white" CssClass="btn btn-primary" OnClientClick="return validarAgregarPF_Titulares();" OnClick="btnAceptarTitPF_Click">
                                    <i class="imoon imoon-ok" ></i>
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


<%--Modal Agregar Persona Jurídica--%>
<div id="frmAgregarPersonaJuridica" class="modal fade" style="margin-left: -30%; width: 60%; display: none;" role="dialog">

    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h4 class="modal-title">Agregar Persona Jur&iacute;dica</h4>
    </div>
    <div class="modal-body pbottom0">
        <asp:UpdatePanel ID="updAgregarPersonaJuridica" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:HiddenField ID="hid_id_titular_pj" runat="server" />
                <div style="float: left; width: 50%;">
                    <div class="form-inline pright10 pleft10">
                        <div class="form-group">
                            <label class="control-label">Tipo de Sociedad (*):</label>
                            <div class="col-sm-4">
                                <asp:UpdatePanel ID="upd_ddlTipoSociedadPJ" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTipoSociedadPJ" CssClass="form-control" runat="server" Width="90%"
                                            OnSelectedIndexChanged="ddlTipoSociedadPJ_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div id="Req_TipoSociedadPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Debe ingresar el Tipo de Sociedad
                                </div>

                                <div class="alert alert-warning" style="width: 78%">
                                    <small>La razón social debe coincidir exactamente con la de la escritura de constitución
                                            de la sociedad. En el Certificado saldrá exactamente lo que se escriba en el campo Razón Social.
                                    </small>
                                </div>
                            </div>
                            <div class="pleft30">
                            </div>
                        </div>
                        <div class="form-group ">
                            <asp:Label ID="lblRazonSocialPJ" runat="server" CssClass="control-label col-sm-2" Text="Razon Social (*):"></asp:Label>
                            <div class="col-sm-4">
                                <asp:UpdatePanel ID="upd_txtRazonSocialPJ" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtRazonSocialPJ" runat="server" MaxLength="100" Width="90%" CssClass="form-control"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div id="Req_RazonSocialPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Debe ingresar la Raz&oacute;n Social
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="control-label col-sm-2">C.U.I.T.:</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCuitPJ" runat="server" MaxLength="11" Width="60%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">Tipo Ing. Brutos:</label>
                            <div class="col-sm-4">
                                <asp:UpdatePanel ID="upd_ddlTipoIngresosBrutosPJ" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTipoIngresosBrutosPJ" runat="server" Width="60%" AutoPostBack="true" CssClass="form-control"
                                            OnSelectedIndexChanged="ddlTipoIngresosBrutosPJ_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hid_IngresosBrutosPJ_expresion" runat="server" />
                                        <asp:HiddenField ID="hid_IngresosBrutosPJ_formato" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2" style="width: 110px;">Nº Ing. Brutos:</label>
                            <div class="col-sm-4">
                                <asp:UpdatePanel ID="upd_txtIngresosBrutosPJ" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtIngresosBrutosPJ" runat="server" MaxLength="20" Width="55%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="float: left; width: 50%;">
                    <div class="form-inline pright10">
                        <div class="form-group">
                            <label class="control-label col-sm-2">Calle: (*)</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCallePJ" runat="server" MaxLength="50" Width="80%" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div id="Req_CallePJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                Debe ingresar la Calle.
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">Nro de Puerta (*):</label>
                            <div class="col-sm-4">
                                <div class="form-inline">
                                    <asp:TextBox ID="txtNroPuertaPJ" runat="server" MaxLength="5" Width="26%" CssClass="form-control"></asp:TextBox>
                                    <label class="pleft5 pright5">Piso:</label>
                                    <asp:TextBox ID="txtPisoPJ" runat="server" MaxLength="5" Width="10%" CssClass="form-control"></asp:TextBox>
                                    <label class="pleft5 pright5">Depto:</label>
                                    <asp:TextBox ID="txtDeptoPJ" runat="server" MaxLength="5" Width="10%" CssClass="form-control"></asp:TextBox>
                                    <div id="Req_NroPuertaPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el Nro de Puerta.
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">C&oacute;digo Postal:</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtCPPJ" runat="server" MaxLength="8" Style="text-transform: uppercase" CssClass="form-control" Width="52%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">Provincia:</label>
                            <div class="col-sm-4">
                                <asp:UpdatePanel ID="updProvinciasPJ" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlProvinciaPJ" runat="server" Width="55%" AutoPostBack="true" CssClass="form-control"
                                            OnSelectedIndexChanged="ddlProvinciaPJ_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">Localidad:</label>
                            <div class="col-sm-4">
                                <asp:UpdatePanel ID="updLocalidadPJ" runat="server">
                                    <ContentTemplate>
                                        <div class="form-inline">
                                            <asp:DropDownList ID="ddlLocalidadPJ" runat="server" Width="55%" CssClass="form-control">
                                            </asp:DropDownList>
                                            <div class="pull-right">
                                                <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="updProvinciasPJ" runat="server" DisplayAfter="0">
                                                    <ProgressTemplate>
                                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">Tel&eacute;fono:</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtTelefonoPJ" runat="server" Width="60%" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">E-mail:</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtEmailPJ" runat="server" Width="60%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <%--Grilla de los titulares de las Sociedades de hecho --%>
                <%--             <asp:UpdatePanel ID="updgrillaTitularesSH" runat="server" UpdateMode="Conditional" style="display:none">
                            <ContentTemplate>

                                <asp:Panel ID="pnlAgregarTitularSH" runat="server" Style="display: none">

                                    <asp:UpdatePanel ID="updBotonesAgregarTitularSH" runat="server">
                                        <ContentTemplate>
                                            <table border="0" style="width: 100%">
                                                <tr>
                                                    <td style="padding-left: 10px">
                                                        <b>Datos de los Titulares</b>
                                                    </td>
                                                    <td>

                                                        <div style="width: 100%; text-align: right;">

                                                            <table border="0" style="float: right">
                                                                <tr>
                                                                    <td style="width: 30px">

                                                                        <asp:UpdateProgress ID="UpdateProgress15" AssociatedUpdatePanelID="updBotonesAgregarTitularSH"
                                                                            runat="server" DisplayAfter="0">
                                                                            <ProgressTemplate>
                                                                                <img src='<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>' alt="" />
                                                                            </ProgressTemplate>
                                                                        </asp:UpdateProgress>

                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton ID="btnAgregarTitularSH" runat="server" CssClass="btn btn-default" OnClick="btnAgregarTitularSH_Click">
                                                                                <i class="imoon imoon-plus"></i>
                                                                                <span class="text">Agregar Titular / Firmante</span>
                                                                        </asp:LinkButton>
                                                                    </td>


                                                                </tr>
                                                            </table>

                                                        </div>

                                                    </td>
                                                </tr>
                                            </table>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <asp:GridView ID="grdTitularesSH" runat="server" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-bordered mtop5"
                                        GridLines="None" Width="860px" CellPadding="3">


                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditarTitularSH" runat="server" OnClick="btnEditarTitularSH_Click"
                                                        CommandName="Editar" title="Editar" data-toggle="tooltip">
                                                            <i class="imoon imoon-pencil color-black"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:BoundField DataField="Apellidos" HeaderText="Apellido/s" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Nombres" HeaderText="Nombre/s" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="TipoDoc" HeaderText="Tipo Doc." HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NroDoc" HeaderText="Nro de Doc." HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />


                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>

                                                    <asp:LinkButton ID="btnEliminarTitularSH" runat="server" OnClick="btnEliminarTitularSH_Click" title="Eliminar" data-toggle="tooltip"
                                                        CommandName="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este titular?');">
                                                            <i class="imoon imoon-remove color-black"></i>
                                                            
                                                    </asp:LinkButton>

                                                    <asp:HiddenField ID="hid_rowid_grdTitularesSH" runat="server" Value='<% #Eval("rowid") %>' />
                                                    <asp:HiddenField ID="hid_id_tipodoc_grdTitularesSH" runat="server" Value='<% #Eval("id_tipodoc_personal") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="mtop10">

                                                <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                                <span class="mleft10">No se encontraron registros.</span>

                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>


                                    <div style="padding-left: 10px; margin-top: 10px">
                                        <b>Datos de los Firmantes</b>
                                    </div>

                                    <asp:GridView ID="grdFirmantesSH" runat="server" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-bordered mtop5"
                                        Style="border: none;" GridLines="None" Width="860px" CellPadding="3">
                                        <HeaderStyle CssClass="grid-header" />
                                        <AlternatingRowStyle BackColor="#efefef" />

                                        <Columns>

                                            <asp:BoundField DataField="FirmanteDe" HeaderText="Firmante de" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Apellidos" HeaderText="Apellido/s" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Nombres" HeaderText="Nombre/s" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="TipoDoc" HeaderText="Tipo Doc." HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NroDoc" HeaderText="Nro de Doc." HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="nom_tipocaracter" HeaderText="Carácter Legal" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cargo_firmante" HeaderText="Cargo" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />

                                            <asp:TemplateField ItemStyle-Width="1px">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hid_rowid_grdFirmantesSH" runat="server" Value='<% #Eval("rowid") %>' />
                                                    <asp:HiddenField ID="hid_rowid_titularSH_grdFirmantesSH" runat="server" Value='<% #Eval("rowid_titular") %>' />
                                                    <asp:HiddenField ID="hid_id_tipodoc_grdFirmantesSH" runat="server" Value='<% #Eval("id_tipodoc_personal") %>' />
                                                    <asp:HiddenField ID="hid_id_caracter_grdFirmantesSH" runat="server" Value='<% #Eval("id_tipocaracter") %>' />
                                                    <asp:HiddenField ID="hid_misma_persona_grdFirmantesSH" runat="server" Value='<% #Eval("misma_persona") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="mtop10">

                                                <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                                <span class="mleft10">No se encontraron registros.</span>

                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                    <div id="Req_TitularesSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los Titulares de la Sociedad de Hecho, el m&iacute;nimo son 2 titulares.
                                    </div>


                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%--Panel de Firmantes de las personas juridicas  (todas menos las sociedades de hecho)--%>
                <%--      <asp:Panel ID="pnlFirmantesPJ" runat="server" style="display:none">

                            <asp:UpdatePanel ID="updbtnShowAgregarFirPJ" runat="server">
                                <ContentTemplate>

                                    <div class="row pleft15 pright10">
                                        <div class="col-sm-6 ptop15">
                                            <strong>Firmantes:</strong>
                                        </div>
                                        <div class="col-sm-6 text-right">
                                            <asp:LinkButton ID="btnShowAgregarFirPJ" runat="server" CssClass="btn btn-default" OnClick="btnShowAgregarFirPJ_Click">
                                                    <i class="imoon imoon-plus"></i>
                                                    <span class="text">Agregar Firmante</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row pleft20 pright10">
                                <asp:UpdatePanel ID="updgrdFirmantesPJ" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:GridView ID="grdFirmantesPJ" runat="server" AutoGenerateColumns="false" AllowPaging="false" CssClass="table table-bordered mtop10 mbottom0"
                                            GridLines="None">
                                            <HeaderStyle CssClass="grid-header" />
                                            <AlternatingRowStyle BackColor="#efefef" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="30px" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEditarFirPJ" runat="server" CssClass="link-local" data-toggle="tooltip"
                                                            title="Editar" OnClick="btnEditarFirPJ_Click">
                                                                <i class="imoon imoon-pencil"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Apellidos" HeaderText="Apellido/s" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Nombres" HeaderText="Nombre/s" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TipoDoc" HeaderText="Tipo Doc." HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="NroDoc" HeaderText="Nro de Doc." HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="nom_tipocaracter" HeaderText="Carácter Legal" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="cargo_firmante_pj" HeaderText="Cargo" HeaderStyle-HorizontalAlign="Left" />

                                                <asp:TemplateField ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEliminarFirPJ" runat="server" CssClass="link-local" Width="30px" data-rowindex='<% #Eval("rowindex") %>'
                                                            OnClientClick="return showfrmConfirmarEliminarFirPJ_Titulares(this);" data-toggle="tooltip" title="Eliminar">
                                                                <i class="imoon imoon-close"></i>    
                                                        </asp:LinkButton>

                                                        <asp:HiddenField ID="hid_id_tipodoc_grdFirmantes" runat="server" Value='<% #Eval("id_tipodoc_personal") %>' />
                                                        <asp:HiddenField ID="hid_id_caracter_grdFirmantes" runat="server" Value='<% #Eval("id_tipocaracter") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="mtop10">

                                                    <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                                    <span class="mleft10">No se encontraron registros.</span>

                                                </div>

                                            </EmptyDataTemplate>
                                        </asp:GridView>

                                        <asp:HiddenField ID="hid_rowindex_eliminar" runat="server" />

                                        <div id="Req_FirmantesPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el/los firmante/s.
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </asp:Panel>
                --%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="modal-footer mtop10">
        <asp:UpdatePanel ID="updBotonesAgregarPJ" runat="server">
            <ContentTemplate>

                <div class="form-inline">

                    <div class="form-group pull-left">
                        <asp:Panel ID="ValExiste_TitularPJ" runat="server" CssClass="alert alert-small alert-danger mbottom0" Style="display: none;">
                            Ya existe un titular con el mismo número de CUIT.
                        </asp:Panel>
                    </div>

                    <div class="form-group">
                        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="updBotonesAgregarPJ">
                            <ProgressTemplate>
                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />Guardando...
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <div id="pnlBotonesAgregarPJ" class="form-group">

                        <asp:LinkButton ID="btnAceptarTitPJ" runat="server" CssClass="btn btn-primary" Style="color: white" OnClientClick="return validarAgregarPJ_Titulares();" OnClick="btnAceptarTitPJ_Click">
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
<!-- /.modal -->


<%--Modal Agregar Firmante Persona Juridica--%>
<%--<div id="frmAgregarFirmantePJ_Titulares" class="modal fade" style="margin-left:-25%; width: 50%; display:none;" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Agregar Firmante (Persona Jur&iacute;dica)</h4>
            </div>
            <div class="modal-body pbottom5">
                <asp:UpdatePanel ID="updFirmantePJ" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:HiddenField ID="hid_rowindex_fir" runat="server" />

                        <div class="form-horizontal">
                            <div class="span6">
                                <label class="control-label">Apellido/s (*):</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtApellidosFirPJ" runat="server" MaxLength="50" Width="90%" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_ApellidosFirPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los Apellido/s.
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">Nombre/s (*):</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtNombresFirPJ" runat="server" MaxLength="50" Width="90%" CssClass="form-control"></asp:TextBox>
                                    <div id="Req_NombresFirPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los nombre/s.
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">Tipo y Nro doc.(*):</label>
                                <div class="controls">
                                    <div class="form-inline">
                                        <asp:DropDownList ID="ddlTipoDocumentoFirPJ" runat="server" Width="50%" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtNroDocumentoFirPJ" runat="server" MaxLength="15" Width="40%" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div id="Req_TipoNroDocFirPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el Tipo y Nro. de doc.
                                    </div>
                                    <asp:Panel ID="ValExiste_TipoNroDocFirPJ" runat="server" CssClass="alert alert-small alert-danger mbottom0 mtop5" Style="display: none;">
                                        Ya hay un firmante con el mismo Tipo y Nro de Documento.
                                    </asp:Panel>
                                </div>
                            </div>

                            <div class="span6">
                                <label class="control-label">E-mail (*):</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEmailFirPJ" runat="server" MaxLength="40" Width="90%" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_EmailFirPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el e-mail.
                                    </div>
                                    <div id="Val_Formato_EmailFirPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        formato inv&aacute;lido. Ej: nombre@servidor.com
                                    </div>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="upd_ddlTipoCaracterLegalFirPJ" runat="server">
                                <ContentTemplate>

                                    <div class="span6">
                                        <asp:Label ID="lblCaracterLegalFirPJ" runat="server" Text="Carácter Legal (*):" CssClass="control-label"></asp:Label>

                                        <div class="controls">
                                            <div class="form-inline">

                                                <asp:DropDownList ID="ddlTipoCaracterLegalFirPJ" runat="server" Width="90%" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCaracterLegalFirPJ_SelectedIndexChanged">
                                                </asp:DropDownList>

                                                <div class="form-group pleft20">

                                                    <asp:UpdateProgress ID="UpdateProgress4" AssociatedUpdatePanelID="upd_ddlTipoCaracterLegalFirPJ" runat="server" DisplayAfter="0">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>

                                                </div>
                                            </div>

                                            <div id="Req_TipoCaracterLegalFirPJ_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe seleccionar el car&aacute;cter legal.
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel ID="rowCargoFirmantePJ" runat="server" CssClass="form-group" Style="display: none; margin-left: -5px">
                                        <label class="control-label col-sm-3">Cargo (*):</label>
                                        <div class="col-sm-9 mleft5">
                                            <asp:HiddenField ID="hid_CargosFir_seleccionado" runat="server" />
                                            <asp:TextBox ID="txtCargoFirPJ" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                            <i class="imoon imoon-question mleft5" title="Selecciona el cargo. Si el mismo no se encuentra, escribilo y luego presiona <TAB> o <ENTER>" data-toggle="tooltip"></i>
                                            <asp:Panel ID="Req_CargoFirPJ" runat="server" CssClass="alert alert-small alert-danger mbottom0 mtop5" Style="display: none;">
                                                Debe ingresar el cargo que ocupa.
                                            </asp:Panel>
                                        </div>


                                    </asp:Panel>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <asp:UpdatePanel ID="updBotonesAgregarFirPj" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="updBotonesAgregarFirPj">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />Guardando...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesAgregarFirPJ_Titulares" class="form-group">
                                <asp:LinkButton ID="btnAceptarFirPJ" runat="server" CssClass="btn btn-primary" OnClientClick="return validarAgregarFirPJ_Titulares();" OnClick="btnAceptarFirPJ_Click">
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
</div>--%>
<!-- /.modal -->

<%--Confirmar Eliminar Persona--%>
<div id="frmConfirmarEliminar_Titulares" class="modal fade" style="margin-left: -25%; width: 50%; display: none;" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Eliminar Persona</h4>
            </div>
            <div class="modal-body">
                <table style="border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="text-align: center; vertical-align: text-top">
                            <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                        </td>
                        <td style="vertical-align: middle">
                            <label class="mleft10">¿ Est&aacute; seguro de eliminar el registro ?</label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="modal-footer">

                <asp:UpdatePanel ID="updConfirmarEliminar" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updConfirmarEliminar">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesConfirmacionEliminar_Titulares" class="form-group">
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-primary" Text="Sí" OnClick="btnEliminar_Click" />
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

<%--Confirmar Eliminar Firmante Persona Juridica--%>

<%--<div id="frmConfirmarEliminarFirPJ_Titulares" class="modal fade" style="margin-left:-25%; width: 50%; display:none;" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Eliminar Firmante (Persona Jur&iacute;dica)</h4>
            </div>
            <div class="modal-body">
                <table style="border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="text-align: center; vertical-align: text-top">
                            <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                        </td>
                        <td style="vertical-align: middle">
                            <label class="mleft10">¿ Est&aacute; seguro de eliminar este firmante ?</label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="modal-footer">

                <asp:UpdatePanel ID="updConfirmarEliminarFirPJ" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="updConfirmarEliminarFirPJ">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesRliminarFirmantePJ_Titulares" class="form-group">
                                <asp:Button ID="btnEliminarFirmantePJ" runat="server" CssClass="btn btn-primary" Text="Sí" OnClick="btnEliminarFirmantePJ_Click" />
                                <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>--%>


<%-- Agregar Titular y Firmante (Sociedad de Hecho)--%>
<%--<div id="frmAgregarTitularesSH_Titulares" class="modal fade" style="margin-left:-25%; width: 50%; display:none;" role="dialog">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Agregar Titulares / Firmantes (Sociedad de Hecho)</h4>
            </div>
            <div class="modal-body">

                <asp:UpdatePanel ID="updABMTitularesSH" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:HiddenField ID="hid_rowindex_titSH" runat="server" />
                        <strong>Datos del Titular</strong>
                        <div class="form-horizontal pright10 mtop5">

                            <div class="form-group">
                                <label class="control-label col-sm-3">Apellido/s (*):</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtApellidosTitSH" runat="server" MaxLength="50" Width="300px" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_ApellidosTitSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los Apellido/s.
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3">Nombre/s (*):</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtNombresTitSH" runat="server" MaxLength="50" Width="400px" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_NombresTitSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los Nombre/s.
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3">Tipo y Nro. de doc.(*):</label>
                                <div class="col-sm-9">
                                    <div class="form-inline ">
                                        <asp:DropDownList ID="ddlTipoDocumentoTitSH" runat="server" Width="150px" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtNroDocumentoTitSH" runat="server" MaxLength="15" Width="140px" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div id="Req_TipoNroDocumentoTitSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el Tipo y Nro. de doc.
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3">E-mail (*):</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtEmailTitSH" runat="server" MaxLength="40" Width="400px" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_EmailTitSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el e-mail.
                                    </div>
                                    <div id="Val_Formato_EmailTitSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        formato inv&aacute;lido. Ej: nombre@servidor.com
                                    </div>
                                </div>
                            </div>


                            <strong>Datos del Firmante</strong>

                            <div class="pad10">
                                <label class="radio-inline">

                                    <asp:RadioButton ID="optMismaPersonaSH" runat="server" Text="El firmante es la misma persona"
                                        GroupName="MismoFirmanteSH" ForeColor="#645fc5" Checked="true" onclick="return hideDatosFirmanteSH_Titulares();" />

                                </label>
                                <label class="radio-inline">
                                    <asp:RadioButton ID="optOtraPersonaSH" runat="server" Text="El firmante es otra persona."
                                        GroupName="MismoFirmanteSH" ForeColor="#645fc5" Font-Bold="true" onclick="return showDatosFirmanteSH_Titulares();" />
                                </label>
                            </div>

                        </div>

                        <asp:Panel ID="pnlFirSH" runat="server" CssClass="form-horizontal pright10" Style="display: none">

                            <div class="form-group">
                                <label class="control-label col-sm-3">Apellido/s (*):</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtApellidosFirSH" runat="server" MaxLength="50" Width="300px" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_ApellidosFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los Apellido/s.
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3">Nombre/s (*):</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtNombresFirSH" runat="server" MaxLength="50" Width="400px" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_NombresFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el/los Nombre/s.
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3">Tipo y Nro. de doc.(*):</label>
                                <div class="col-sm-9">
                                    <div class="form-inline">
                                        <asp:DropDownList ID="ddlTipoDocumentoFirSH" runat="server" Width="150px" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtNroDocumentoFirSH" runat="server" MaxLength="15" Width="140px" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div id="Req_TipoNroDocumentoFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el Tipo y Nro. de doc.
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3">E-mail (*):</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtEmailFirSH" runat="server" MaxLength="40" Width="400px" CssClass="form-control"></asp:TextBox>

                                    <div id="Req_EmailFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        Debe ingresar el e-mail.
                                    </div>
                                    <div id="Val_Formato_EmailFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        formato inv&aacute;lido. Ej: nombre@servidor.com
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="upd_ddlTipoCaracterLegalFirSH" runat="server">
                                <ContentTemplate>

                                    <div class="form-group">
                                        <label class="control-label col-sm-3">Car&aacute;cter Legal (*):</label>
                                        <div class="col-sm-9">
                                            <div class="form-inline">

                                                <asp:DropDownList ID="ddlTipoCaracterLegalFirSH" runat="server" Width="350px" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCaracterLegalFirSH_SelectedIndexChanged">
                                                </asp:DropDownList>

                                                <div class="form-group">
                                                    <asp:UpdateProgress ID="UpdateProgress16" AssociatedUpdatePanelID="updABMTitularesSH" runat="server" DisplayAfter="0">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" class="mleft15" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                            </div>
                                            <div id="Req_TipoCaracterLegalFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe seleccionar Carácter Legal.
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel ID="pnlCargoFirmanteSH" runat="server" CssClass="form-group" Style="display: none; margin-left: -5px">
                                        <label class="control-label col-sm-3" style="margin-left: -5px">Cargo (*):</label>
                                        <div class="col-sm-9">

                                            <asp:HiddenField ID="hid_CargosFirSH_seleccionado" runat="server" />
                                            <asp:TextBox ID="txtCargoFirSH" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                                            <i class="imoon imoon-question mleft5" title="Selecciona el cargo. Si el mismo no se encuentra, escribilo y luego presiona <TAB> o <ENTER>" data-toggle="tooltip"></i>
                                            <br />
                                            <div id="Req_CargoFirSH_Titulares" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el cargo que ocupa.
                                            </div>

                                        </div>
                                    </asp:Panel>

                                </ContentTemplate>
                            </asp:UpdatePanel>


                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">

                <asp:UpdatePanel ID="updBotonesIngresarTitularesSH" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="updBotonesIngresarTitularesSH">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesIngresarTitularesSH_Titulares" class="form-group">
                                <asp:Button ID="btnAceptarTitSH" runat="server" CssClass="btn btn-primary" Text="Aceptar"
                                    OnClientClick="return validarAgregarTitSH_Titulares();" OnClick="btnAceptarTitSH_Click" />
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>--%>

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
        //toolTips_Titulares();
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


    //function toolTips_Titulares() {
    //    $("[data-toggle='tooltip']").tooltip();

    //    return false;
    //}

    //function showfrmError_Titulares() {
    //    $("#frmError_Titulares").modal("show");
    //    return false;
    //}

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

    function init_JS_updAgregarPersonaFisica() {



        // Se realiza aqui y en el change para que ete correcto al editar y levanta por primera vez
        if ($("#<%: ddlTipoDocumentoPF.ClientID %>").val() != id_tipodoc_pasaporte) {
            $("#<%: txtNroDocumentoPF.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $("#<%: txtNroDocumentoPF.ClientID %>").autoNumeric("update");
        }
        else {
            $("#<%: txtNroDocumentoPF.ClientID %>").autoNumeric("destroy");
        }

        $("#<%: txtCuitPF.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999999999' });
        $("#<%: txtNroPuertaPF.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });
        $("#<%: txtTelefonoPF.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999999' });

        $("#<%: txtApellidosPF.ClientID %>").on("keyup", function (e) {
            $("#Req_ApellidoPF_Titulares").hide();
        });

        $("#<%: txtNombresPF.ClientID %>").on("keyup", function (e) {
            $("#Req_NombresPF_Titulares").hide();
        });

        $("#<%: ddlTipoDocumentoPF.ClientID %>").on("change", function (e) {
            $("#Req_TipoNroDocPF_Titulares").hide();


            if ($("#<%: ddlTipoDocumentoPF.ClientID %>").val() != id_tipodoc_pasaporte) {
                $("#<%: txtNroDocumentoPF.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
                $("#<%: txtNroDocumentoPF.ClientID %>").autoNumeric("update");
            }
            else {
                $("#<%: txtNroDocumentoPF.ClientID %>").autoNumeric("destroy");
            }

        });

        $("#<%: txtNroDocumentoPF.ClientID %>").on("keyup", function (e) {
            $("#Req_TipoNroDocPF_Titulares").hide();
            $("#ValDNI_CuitPF_Titulares").hide();
        });

        $("#<%: txtCuitPF.ClientID %>").on("keyup", function (e) {
            $("#Req_CuitPF_Titulares").hide();
            $("#ValFormato_CuitPF_Titulares").hide();
            $("#ValDV_CuitPF_Titulares").hide();
            $("#ValDNI_CuitPF_Titulares").hide();
            $("#<%: ValExiste_TitularPF.ClientID %>").hide();

        });

        $("#<%: txtCallePF.ClientID %>").on("keyup", function (e) {
            $("#Req_CallePF_Titulares").hide();
        });

        $("#<%: txtNroPuertaPF.ClientID %>").on("keyup", function (e) {
            $("#Req_NroPuertaPF_Titulares").hide();
        });

        $("#<%: txtCallePJ.ClientID %>").on("keyup", function (e) {
            $("#Req_CallePJ_Titulares").hide();
        });

        $("#<%: txtNroPuertaPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_NroPuertaPJ_Titulares").hide();
        });

        $("#<%: txtEmailPF.ClientID %>").on("keyup", function (e) {
            $("#Req_EmailPF_Titulares").hide();
            $("#ValFormato_EmailPF_Titulares").hide();
        });

        return false;
    }

    function init_JS_updAgregarPersonaJuridica() {

        $("#<%: txtNroPuertaPF.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });
        $("#<%: txtNroPuertaPJ.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });

<%--        $("#<%: txtCuitPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_CuitPJ_Titulares").hide();
            $("#ValFormato_CuitPJ_Titulares").hide();
            $("#ValDV_CuitPJ_Titulares").hide();
            $("#<%: ValExiste_TitularPJ.ClientID %>").hide();
        });

        $("#<%: txtCallePJ.ClientID %>").on("keyup", function (e) {
            $("#Req_CallePJ_Titulares").hide();
        });

        $("#<%: txtEmailPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_EmailPJ_Titulares").hide();
            $("#ValFormato_EmailPJ_Titulares").hide();
        });

        $("#<%: txtNroPuertaPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_NroPuertaPJ_Titulares").hide();
        });

        $("#<%: txtCPPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_CPPJ").hide();
            $("#Val_Formato_CPPJ").hide();
        });--%>

        return false;
    }
    function init_JS_upd_ddlTipoIngresosBrutosPF() {

        $("#<%: ddlTipoIngresosBrutosPF.ClientID %>").on("change", function (e) {
            $("#Req_TipoIngresosBrutosPF_Titulares").hide();
            $("#Req_IngresosBrutosPF_Titulares").hide();
            $("#ValFormato_IngresosBrutosPF_Titulares").hide();
        });

        return false;

    }

    function init_JS_upd_txtIngresosBrutosPF() {
        $("#<%: txtIngresosBrutosPF.ClientID %>").on("keyup", function (e) {
            $("#Req_IngresosBrutosPF_Titulares").hide();
            $("#ValFormato_IngresosBrutosPF_Titulares").hide();
        });

        return false;
    }

    function init_JS_updLocalidadPF() {

        $("#<%: ddlLocalidadPF.ClientID %>").on("change", function (e) {
            $("#Req_LocalidadPF_Titulares").hide();
        });
        return false;
    }

    function init_JS_updProvinciasPF() {
        $("#<%: ddlProvinciaPF.ClientID %>").on("change", function (e) {
            $("#Req_ProvinciaPF_Titulares").hide();
            $("#Req_LocalidadPF_Titulares").hide();
        });
        return false;
    }


    function init_JS_upd_ddlTipoIngresosBrutosPJ() {

        $("#<%: ddlTipoIngresosBrutosPJ.ClientID %>").on("change", function (e) {
            $("#Req_TipoIngresosBrutosPJ_Titulares").hide();
            $("#Req_IngresosBrutosPJ_Titulares").hide();
            $("#ValFormato_IngresosBrutosPJ_Titulares").hide();
        });

        return false;
    }


    function showConfirmarEliminar_Titulares(obj) {

        var tipopersona_eliminar = $(obj).attr("data-tipopersona-eliminar");
        var id_persona_eliminar = $(obj).attr("data-id-persona-eliminar");

        $("#<%: hid_tipopersona_eliminar.ClientID %>").val(tipopersona_eliminar);
        $("#<%: hid_id_persona_eliminar.ClientID %>").val(id_persona_eliminar);

        $("#frmConfirmarEliminar_Titulares").modal("show");
        return false;
    }

<%--    function init_JS_upd_txtIngresosBrutosPJ() {
        $("#<%: txtIngresosBrutosPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_IngresosBrutosPJ_Titulares").hide();
            $("#ValFormato_IngresosBrutosPJ_Titulares").hide();
        });
        return false;
    }--%>

    function validarAgregarPJ_Titulares() {

        var ret = true;


        var strmsgFormatoIIBB = "formato incorrecto. Ej: " + $("#<%: hid_IngresosBrutosPJ_formato.ClientID %>").val();
        var formatoIIBB = $("#<%: hid_IngresosBrutosPJ_expresion.ClientID %>").val(); ///^([0-9]|-)*$/;


        if (formatoIIBB.length > 0) {
            formatoIIBB = eval("/^" + formatoIIBB + "$/");
        }

        //var formatoCUIT = /^\d{2}-\d{7}-\d{1}|\d{2}-\d{8}-\d{1}|\d{10}$/;
        var formatoCUIT = /[3]\d{10}$/;
        var formatoEmail = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;


        $("#Req_TipoSociedadPJ_Titulares").hide();
        $("#Req_RazonSocialPJ_Titulares").hide();
        //$("#Req_CuitPJ_Titulares").hide();
        //$("#ValDV_CuitPJ_Titulares").hide();
        $//("#ValFormato_CuitPJ_Titulares").hide();
        $//("#Req_TipoIngresosBrutosPJ_Titulares").hide();
        //$("#Req_IngresosBrutosPJ_Titulares").hide();
        //$("#ValFormato_IngresosBrutosPJ_Titulares").hide();
        $("#Req_CallePJ_Titulares").hide();
        $("#Req_NroPuertaPJ_Titulares").hide();
        $("#Req_CallePJ_Titulares").hide();
        $("#Req_NroPuertaPJ_Titulares").hide();
        //$("#Req_ProvinciaPJ_Titulares").hide();
        //$("#Req_LocalidadPJ_Titulares").hide();
        $//("#Req_EmailPJ_Titulares").hide();
        //$("#ValFormato_EmailPJ_Titulares").hide();
        //$("#<%: ValExiste_TitularPJ.ClientID %>").hide();


        var id_tiposociedad = $.trim($("#<%: ddlTipoSociedadPJ.ClientID %>").val());

        if ($.trim($("#<%: ddlTipoSociedadPJ.ClientID %>").val()).length == 0) {
            $("#Req_TipoSociedadPJ_Titulares").css("display", "inline-block");
            $("#rowRazonSocial_Titulares").css("margin-top", "-50px");
            ret = false;
        }

        if ($.trim($("#<%: txtRazonSocialPJ.ClientID %>").val()).length == 0) {
            $("#Req_RazonSocialPJ_Titulares").css("display", "inline-block");
            ret = false;
        }

        <%--if ($.trim($("#<%: txtCuitPJ.ClientID %>").val()).length == 0) {
            $("#Req_CuitPJ_Titulares").css("display", "inline-block");
            ret = false;
        }
        else {
            if (!formatoCUIT.test($.trim($("#<%: txtCuitPJ.ClientID %>").val()))) {
                $("#ValFormato_CuitPJ_Titulares").css("display", "inline-block");
                ret = false;
            }
            else if (!ValidarDniCuit($("#<%: txtCuitPJ.ClientID %>")[0])) {
                $("#ValDV_CuitPJ_Titulares").css("display", "inline-block");
                ret = false;
            }
        }

        if ($.trim($("#<%: ddlTipoIngresosBrutosPJ.ClientID %>").val()).length == 0) {
            $("#Req_TipoIngresosBrutosPJ_Titulares").css("display", "inline-block");
            ret = false;
        }
        else {
            if (!$("#<%: txtIngresosBrutosPJ.ClientID %>").prop("disabled")) {
                if ($.trim($("#<%: txtIngresosBrutosPJ.ClientID %>").val()).length == 0) {
                    $("#Req_IngresosBrutosPJ_Titulares").css("display", "inline-block");
                    ret = false;
                }
                else {
                    if (!formatoIIBB.test($.trim($("#<%: txtIngresosBrutosPJ.ClientID %>").val()))) {
                        $("#ValFormato_IngresosBrutosPJ_Titulares").text(strmsgFormatoIIBB);
                        $("#ValFormato_IngresosBrutosPJ_Titulares").css("display", "inline-block");
                        ret = false;
                    }
                }
            }
        }--%>
        if ($.trim($("#<%: txtCallePJ.ClientID %>").val()).length == 0) {
            $("#Req_CallePJ_Titulares").css("display", "inline-block");
            ret = false;
        }
        if ($.trim($("#<%: txtNroPuertaPJ.ClientID %>").val()).length == 0) {
            $("#Req_NroPuertaPJ_Titulares").css("display", "inline-block");
            ret = false;
        }

        <%--if ($.trim($("#<%: ddlProvinciaPJ.ClientID %>").val()).length == 0) {
            $("#Req_ProvinciaPJ_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: ddlLocalidadPJ.ClientID %>").val()).length == 0) {
            $("#Req_LocalidadPJ_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtEmailPJ.ClientID %>").val()).length > 0) {
            if (!formatoEmail.test($.trim($("#<%: txtEmailPJ.ClientID %>").val()))) {
                $("#ValFormato_EmailPJ_Titulares").css("display", "inline-block");
                ret = false;
            }
        }
        else {
            $("#Req_EmailPJ_Titulares").css("display", "inline-block");
            ret = false;
        }

        if (ret) {
            $("#pnlBotonesAgregarPJ_Titulares").hide();
        }--%>

        return ret;
    }

    function ValidarDniCuit() {
        debugger;
        var dni = $("#<%: txtNroDocumentoPF.ClientID %>").val();
        var cuit = $("#<%: txtCuitPF.ClientID %>").val();
        if (cuit.indexOf(dni) != -1) {
            return true;
        } else {
            return false;
        }
    }

    function validarAgregarPF_Titulares() {

        var ret = true;

        <%--var strmsgFormatoIIBB = "formato incorrecto. Ej: " + $("#<%: hid_IngresosBrutosPF_formato.ClientID %>").val();
        var formatoIIBB = $("#<%: hid_IngresosBrutosPF_expresion.ClientID %>").val(); ///^([0-9]|-)*$/;

        if (formatoIIBB.length > 0) {
            formatoIIBB = eval("/^" + formatoIIBB + "$/");
        }
        var formatoCUIT = /^\d{2}[-.]?\d{7,8}[-.]?\d{1}$/;
        var formatoEmail = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;--%>


        $("#Req_ApellidoPF_Titulares").hide();
        $("#Req_NombresPF_Titulares").hide();
        $("#Req_CallePF_Titulares").hide();
        $("#Req_NroPuertaPF_Titulares").hide();

        //$("#Req_TipoNroDocPF_Titulares").hide();
        //$("#Req_CuitPF_Titulares").hide();
        //$("#ValDV_CuitPF_Titulares").hide();
        //$("#ValFormato_CuitPF_Titulares").hide();
        //$("#ValDNI_CuitPF_Titulares").hide();
        //$("#Req_TipoIngresosBrutosPF_Titulares").hide();
        //$("#Req_IngresosBrutosPF_Titulares").hide();
        //$("#ValFormato_IngresosBrutosPF_Titulares").hide();
        //$("#Req_ProvinciaPF_Titulares").hide();
        //$("#Req_LocalidadPF_Titulares").hide();
        //$("#Req_EmailPF_Titulares").hide();
        //$("#ValFormato_EmailPF_Titulares").hide();
        //$("#<%: ValExiste_TitularPF.ClientID %>").hide();


        if ($.trim($("#<%: txtApellidosPF.ClientID %>").val()).length == 0) {
            $("#Req_ApellidoPF_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtNombresPF.ClientID %>").val()).length == 0) {
            $("#Req_NombresPF_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtCallePF.ClientID %>").val()).length == 0) {
            $("#Req_CallePF_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtNroPuertaPF.ClientID %>").val()).length == 0) {
            $("#Req_NroPuertaPF_Titulares").css("display", "inline-block");
            ret = false;
        }


        <%--if ($.trim($("#<%: ddlTipoDocumentoPF.ClientID %>").val()).length == 0 ||
            $.trim($("#<%: txtNroDocumentoPF.ClientID %>").val()).length == 0) {
            $("#Req_TipoNroDocPF_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtCuitPF.ClientID %>").val()).length == 0) {
            $("#Req_CuitPF_Titulares").css("display", "inline-block");
            ret = false;
        }
        else {
            if (!formatoCUIT.test($.trim($("#<%: txtCuitPF.ClientID %>").val()))) {
                $("#ValFormato_CuitPF_Titulares").css("display", "inline-block");
                ret = false;
            }
                //else if (!ValidarCuit($("#<%: txtCuitPF.ClientID %>")[0])) {
                //    $("#ValDV_CuitPF_Titulares").css("display", "inline-block");
                //    ret = false;
                //}
            else if (!ValidarDniCuit()) {
                $("#ValDNI_CuitPF_Titulares").css("display", "inline-block");
                ret = false;
            }

        }

        if ($.trim($("#<%: ddlTipoIngresosBrutosPF.ClientID %>").val()).length == 0) {
            $("#Req_TipoIngresosBrutosPF_Titulares").css("display", "inline-block");
            ret = false;
        }

        else {
            if (!$("#<%: txtIngresosBrutosPF.ClientID %>").prop("disabled")) {

                if ($.trim($("#<%: txtIngresosBrutosPF.ClientID %>").val()).length == 0) {
                    $("#Req_IngresosBrutosPF_Titulares").css("display", "inline-block");
                    ret = false;
                }
                else {
                    if (!formatoIIBB.test($.trim($("#<%: txtIngresosBrutosPF.ClientID %>").val()))) {
                        $("#ValFormato_IngresosBrutosPF_Titulares").text(strmsgFormatoIIBB);
                        $("#ValFormato_IngresosBrutosPF_Titulares").css("display", "inline-block");
                        ret = false;
                    }
                }
            }
        }--%>       

        <%--if ($.trim($("#<%: ddlProvinciaPF.ClientID %>").val()).length == 0) {
            $("#Req_ProvinciaPF_Titulares").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: ddlLocalidadPF.ClientID %>").val()).length == 0) {
            $("#Req_LocalidadPF_Titulares").css("display", "inline-block");
            ret = false;
        }


        if ($.trim($("#<%: txtEmailPF.ClientID %>").val()).length > 0) {
            if (!formatoEmail.test($.trim($("#<%: txtEmailPF.ClientID %>").val()))) {
                $("#ValFormato_EmailPF_Titulares").css("display", "inline-block");
                ret = false;
            }
        }
        else {
            $("#Req_EmailPF_Titulares").css("display", "inline-block");
            ret = false;
        }


        if (ret) {
            $("#pnlBotonesAgregarPF_Titulares").hide();
        }--%>

        return ret;

    }

<%--    function init_JS_updLocalidadPJ() {

        $("#<%: ddlLocalidadPJ.ClientID %>").on("change", function (e) {
            $("#Req_LocalidadPJ").hide();
        });
        return false;
    }

    function init_JS_updProvinciasPJ() {
        $("#<%: ddlProvinciaPJ.ClientID %>").on("change", function (e) {
            $("#Req_ProvinciaPJ").hide();
            $("#Req_LocalidadPJ").hide();
        });
        return false;
    }


    function init_JS_upd_ddlTipoSociedadPJ() {

        $("#<%: ddlTipoSociedadPJ.ClientID %>").on("change", function (e) {
                $("#Req_TipoSociedadPJ").hide();
                $("#rowRazonSocial").css("margin-top", "-70px");

            });

            $("#<%: ddlTipoSociedadPJ.ClientID %>").select2({
                placeholder: "Seleccionar",
                allowClear: true,
                minimumInputLength: 3,
            });

            return false;
        }

        function init_JS_upd_txtRazonSocialPJ() {

            $("#<%: txtRazonSocialPJ.ClientID %>").on("keyup", function (e) {
            $("#Req_RazonSocialPJ").hide();
        });

        return false;
    }--%>

    function hideTitulares() {
        $("#box_titulares").hide("slow");
    }

    function hideMostrarTitulares() {
        $("#box_MostrarTitulares").hide("slow");
    }

</script>


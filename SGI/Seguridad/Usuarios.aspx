<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="SGI.Seguridad.Usuarios" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <%: Styles.Render("~/Content/themes/base/css")%>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <link href="../Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />


    <hgroup class="title">
        <h1>Usuarios.</h1>
    </hgroup>

    <div id="page_content">

        <%-- Muestra Busqueda--%>
        <div id="box_busqueda">

            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar">

                <div class="widget-title">
                    <span class="icon azulmarino"><i class="imoon-search"></i></span>
                    <h5>B&uacute;squeda de Usuarios</h5>
                </div>

                <div class="widget-content">
                    <div class="form-horizontal">
                        <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="row">
                                        <div class="span5">
                                            <label class="control-label">Apellido:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBusApellido" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="span5">

                                            <label class="control-label">Nombre:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBusNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="span5">

                                            <label class="control-label">Usuario:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBusUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="span5">

                                            <label class="control-label">Bloqueado:</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlBloqueado" runat="server" CssClass="form-control" Style="width: 85%">
                                                    <asp:ListItem Value="null" Text="Todos"></asp:ListItem>
                                                    <asp:ListItem Value="true" Text="Sí"></asp:ListItem>
                                                    <asp:ListItem Value="false" Text="No"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="span5">

                                            <label class="control-label">Perfil:</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlBusPerfil" runat="server" CssClass="form-control" Style="width: 100%"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="pull-right">

                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>

                        <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-success" OnClick="btnExportar_Click" OnClientClick="showfrmExportarExcel();" Visible="false">
	                        <i class="icon-white imoon-file-excel"></i>
                            <span class="text">Exportar usuarios</span>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClick="btnBuscar_Click">
                                <span class="icon" ><i class="imoon imoon-search"></i></span>
                                <span class="text">Buscar</span>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnNuevoUsuario" runat="server" CssClass="btn btn-default" OnClick="btnNuevoUsuario_Click">
                            <span class="icon" ><i class="imoon imoon-plus"></i></span>
                            <span class="text">Nuevo Usuario</span>
                        </asp:LinkButton>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <br />
            <br />

            <%-- Muestra Resultados--%>
            <div id="box_resultado" style="display: none;">
                <div class="widget-box">
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-horizontal" style="margin-left: 15px; margin-right: 15px">
                                <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false">

                                    <div style="display: inline-block">
                                        <h5>Lista de Usuarios</h5>
                                    </div>
                                    <div style="display: inline-block">
                                        <span class="badge">
                                            <asp:Label ID="lblCantidadRegistros" runat="server"></asp:Label></span>
                                    </div>


                                </asp:Panel>
                                <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                    GridLines="None" CssClass="table table-bordered table-striped table-hover with-check" OnPageIndexChanging="grdResultados_PageIndexChanging"
                                    PageSize="30" OnSorting="grdResultados_Sorting" AllowSorting="True" OnDataBound="grdResultados_DataBound">
                                    <Columns>

                                        <asp:BoundField DataField="UserName" ItemStyle-Width="150px" HeaderText="Usuario" SortExpression="username" />
                                        <asp:BoundField DataField="Apellido" HeaderText="Apellido/s" SortExpression="apellido" />
                                        <asp:BoundField DataField="Nombres" HeaderText="Nombre/s" SortExpression="nombres" />
                                        <asp:BoundField DataField="UserName_SADE" HeaderText="Usuario SADE" SortExpression="username_SADE" />
                                        <asp:BoundField DataField="Reparticion_SADE" HeaderText="Repartición SADE" SortExpression="reparticion_SADE" />
                                        <asp:BoundField DataField="Sector_SADE" HeaderText="Sector SADE" SortExpression="sector_SADE" />
                                        <asp:TemplateField HeaderText="Bloqueado" ItemStyle-Width="75px" ItemStyle-CssClass="text-center" SortExpression="Bloqueado">
                                            <ItemTemplate>
                                                <label><%# (Convert.ToBoolean(Eval("Bloqueado"))? "Sí" : "No")  %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Perfiles_1Linea" HeaderText="Perfiles" SortExpression="Perfiles_1Linea" />

                                        <asp:TemplateField HeaderText="Acción" ItemStyle-Width="80px" HeaderStyle-ForeColor="#0088cc" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditarUsuario" runat="server" title="Editar" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%# Eval("userid") %>' OnClick="btnEditarUsuario_Click">
                                                 <span class="icon " ><i class="imoon-edit" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEnviarMail" runat="server" title="Reenviar mail con datos de la cuenta de usuario" data-toggle="tooltip"
                                                    CssClass="link-local" CommandArgument='<%# Eval("userid") %>' OnClick="btnEnviarMail_Click">
                                                    <span class="icon " > <i class="imoon-envelope" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEliminarUsuario" runat="server" title="Eliminar" data-toggle="tooltip"
                                                    CssClass="link-local" data-userid='<%# Eval("userid") %>' OnClientClick="return showfrmConfirmarEliminar(this);">
                                                     <span class="icon" ><i class="imoon-remove" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:Panel ID="pnlPager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                            <asp:Button ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click"
                                                CssClass="btn btn-default" />
                                            <asp:Button ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn" />
                                            <asp:Button ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn" />
                                            <asp:Button ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn btn-primary" />
                                            <asp:Button ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click"
                                                CssClass="btn btn-default" />
                                        </asp:Panel>
                                    </PagerTemplate>
                                    <EmptyDataTemplate>
                                        <div>
                                            <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                            <span class="mleft20">No se encontraron registros.</span>

                                        </div>

                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>

        <%-- Muestra Datos--%>
        <div id="box_datos" style="display: none">

            <div class="widget-box">

                <div class="widget-title">
                    <span class="icon azulmarino"><i class="imoon imoon-user-md"></i></span>
                    <h5>Datos del Usuario</h5>
                </div>

                <div class="widget-content">
                    <div class="row">
                        <div class="col-sm-12 col-md-12">
                            <asp:UpdatePanel ID="updDatosUsuario" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:HiddenField ID="hid_userid" runat="server" />

                                    <asp:Panel ID="pnlDatosUsuario" runat="server" CssClass="form-horizontal">


                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Usuario (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Width="350px" MaxLength="20"></asp:TextBox>
                                                <div id="Req_Username" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el Nombre de Usuario.
                                                </div>
                                                <div id="Valformato_User" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    El nombre de usuario debe tener entre 6 y 20 caracteres y solo se permiten letras, números y los siguientes caracteres especiales:  _-.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Apellido/s (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_Apellido" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el/los Apellido/s de la persona.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Nombre/s (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_Nombre" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el/los Nombre/s de la persona.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Email (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_Email" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el e-mail de la persona.
                                                </div>
                                                <div id="ValFormato_Email" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    formato inv&aacute;lido. Ej: nombre@servidor.com
                                                </div>
                                            </div>

                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Cuit/Cuil (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtCuit" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_Cuit" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el Cuit/Cuil de la persona.
                                                </div>
                                                <div id="ValFormato_Cuit" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    formato inv&aacute;lido. Ej: 20296987452
                                                </div>
                                                <asp:LinkButton ID="consultaUsuario" runat="server" CssClass="btn btn-default" OnClick="btnConsultarUsuario_Click" OnClientClick="return validarCuit();">
                                                                    <i class="imoon imoon-search"></i></span>
                                                                    <span class="text">Consultar</span> 
                                                </asp:LinkButton>
                                            </div>

                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Usuario SADE:</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtUsernameSADE" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_UsernameSADE" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el Usuario SADE de la persona.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Repartición SADE:</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtReparticionSADE" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_ReparticionSADE" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar la Repartición SADE de la persona.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Sector SADE:</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtSectorSADE" runat="server" CssClass="form-control" Width="350px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_SectorSADE" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el Sector SADE de la persona.
                                                </div>
                                                <div class="control">
                                                    <p class="alert alert-small alert-info mbottom0 mtop5" style="display: inline-block">
                                                        <span class="icon azulmarino"><i class="imoon imoon-info-sign"></i></span>Si va a realizar tareas que interact&uacute;en con SADE,
                                                        <br />
                                                        debe ingresar el nombre de usuario, la repartición y el sector respetando las may&uacute;sculas y min&uacute;sculas.
                                                    </p>
                                                </div>
                                            </div>
                                        </div>

                                        <asp:Panel ID="pnlBloqueado" runat="server" CssClass="form-group">
                                            <label class="col-sm-2 control-label">Bloqueado:</label>
                                            <div class="col-sm-10 controls">
                                                <asp:DropDownList ID="ddlEditBloqueado" CssClass="form-control" runat="server" Width="250px">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="true" Text="Si"></asp:ListItem>
                                                    <asp:ListItem Value="false" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </asp:Panel>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Perfiles:</label>
                                            <div class="col-sm-10 controls">
                                                <asp:DropDownList ID="ddlPerfiles" runat="server" Width="650px" multiple="true"></asp:DropDownList>
                                                <asp:HiddenField ID="hid_perfiles_selected" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="form-horizontal">
                        <div class="controls">
                            <asp:LinkButton ID="btnReenviar" runat="server" CssClass="btn btn-default" OnClick="btnReenviar_Click" OnClientClick="return validarGuardar();">
                                                                    <span><i class="imoon imoon-mail"></i></span> 
                                                                    <span class="text">Reenviar contraseña</span>
                            </asp:LinkButton>
                        </div>
                    </div>


                    <div class="form-horizontal">
                        <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                            <ContentTemplate>

                                <div id="pnlBotonesGuardar" class="control">

                                    <div class="controls">

                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-default" OnClick="btnGuardar_Click" OnClientClick="return validarGuardar();">
                                                                    <i class="imoon imoon-save"></i></span>
                                                                    <span class="text">Guardar</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-inverse" OnClientClick="return GetoutDatos();">
                                                                    <i class="imoon imoon-blocked"></i>
                                                                    <span class="text">Cancelar</span></span>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                            Guardando...
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <%--Confirmar Eliminar Usuario--%>
    <div id="frmConfirmarEliminar" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Eliminar Usuario</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64"></label>
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

                            <asp:HiddenField ID="hid_userid_eliminar" runat="server" />

                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="updConfirmarEliminar">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div id="pnlBotonesConfirmacionEliminar" class="control-group">
                                    <asp:Button ID="btnEliminarUsuario" runat="server" CssClass="btn btn-default" Text="Sí" OnClick="btnEliminarUsuario_Click"
                                        OnClientClick="ocultarBotonesConfirmacion();" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>

    <%--modal Aviso OK--%>
    <div id="frmAviso" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Informaci&oacute;n</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-ok fs64" style="color: green"></label>
                            </td>
                            <td>
                                <div class="pad20">

                                    <asp:UpdatePanel ID="updLabelAviso" runat="server" class="form-group">
                                        <ContentTemplate>
                                            <asp:Label ID="lblAviso" runat="server" Style="color: Black"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>


                                </div>

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
    <%--modal de Errores--%>
    <div id="frmError" class="modal fade">
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
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="control-group">
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


    <%--Exportación a Excel--%>
    <div id="frmExportarExcel" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updExportaExcel" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h4 class="modal-title">Exportar a Excel</h4>
                        </div>
                        <div class="modal-body">


                            <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" Enabled="false">
                            </asp:Timer>

                            <asp:Panel ID="pnlExportandoExcel" runat="server">
                                <div class="row text-center">
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                                </div>
                                <div class="row text-center">
                                    <h2>
                                        <asp:Label ID="lblRegistrosExportados" runat="server"></asp:Label></h2>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDescargarExcel" runat="server" Style="display: none">
                                <div class="row text-center">
                                    <asp:HyperLink ID="btnDescargarExcel" runat="server" Target="_blank" CssClass="btn btn-link">
                                        <i class="imoon imoon-file-excel color-green fs48"></i>
                                        <br />
                                        <span class="text">Descargar archivo</span>
                                    </asp:HyperLink>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrarExportacion" runat="server" CssClass="btn btn-default" data-dismiss="modal" Text="Cerrar" Visible="false" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>    

    <script type="text/javascript">
        $(document).ready(function () {


            init_Js_updpnlBuscar();
            init_Js_updResultados();
            init_Js_updDatosUsuario();

        });

        function showfrmExportarExcel(obj) {
            $(obj).hide();
            $("#frmExportarExcel").modal({
                backdrop: "static",
                show: true
            });
            return true;
        }

        function hidefrmExportarExcel() {
            $("#frmExportarExcel").modal("hide");
            return false;
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function showfrmError() {

            $("#frmError").modal("show");

            return false;
        }

        function init_Js_updpnlBuscar() {
            toolTips();

            $("#<%: ddlBloqueado.ClientID %>").select2({ allowClear: true, placeholder: "Todos" });

            $("#<%: ddlBusPerfil.ClientID %>").select2({ allowClear: true, placeholder: "Todos" });


            return false;
        }
        function init_Js_updResultados() {
            toolTips();
        }

        function init_Js_updDatosUsuario() {

            $("#<%: txtUsername.ClientID %>").on("keyup", function (e) {
                $("#Req_Username").hide();
                $("#Valformato_User").hide();

            });

            $("#<%: txtApellido.ClientID %>").on("keyup", function (e) {
                $("#Req_Apellido").hide();
            });

            $("#<%: txtNombre.ClientID %>").on("keyup", function (e) {
                $("#Req_Nombre").hide();
            });

            $("#<%: txtEmail.ClientID %>").on("keyup", function (e) {
                $("#Req_Email").hide();
                $("#ValFormato_Email").hide();
            });

            $("#<%: txtCuit.ClientID %>").on("keyup", function (e) {
                $("#Req_Cuit").hide();
                $("#ValFormato_Cuit").hide();
            });

            $("#<%: txtUsernameSADE.ClientID %>").on("keyup", function (e) {
                $("#Req_UsernameSADE").hide();
                $("#<%: txtUsernameSADE.ClientID %>").val($("#<%: txtUsernameSADE.ClientID %>").val().toUpperCase());
            });

            $("#<%: txtReparticionSADE.ClientID %>").on("keyup", function (e) {
                $("#Req_ReparticionSADE").hide();
            });

            $("#<%: txtSectorSADE.ClientID %>").on("keyup", function (e) {
                $("#Req_SectorSADE").hide();
            });

            //



            /// Inicializar select2 de busqueda
            var tags_selecionados = "";
            if ($("#<%: hid_perfiles_selected.ClientID %>").val().length > 0) {
                tags_selecionados = $("#<%: hid_perfiles_selected.ClientID %>").val().split(",");
            }

            $("#<%: ddlPerfiles.ClientID %>").select2({
                tags: true,
                tokenSeparators: [","],
                placeholder: "Ingrese las etiquetas de búsqueda",
                language: "es",
                data: tags_selecionados
            });
            $("#<%: ddlPerfiles.ClientID %>").val(tags_selecionados);
            $("#<%: ddlPerfiles.ClientID %>").trigger("change.select2");

            $("#<%: ddlPerfiles.ClientID %>").on("change", function () {
                $("#<%: hid_perfiles_selected.ClientID %>").val($("#<%: ddlPerfiles.ClientID %>").val());
            });


            return false;

        }

        function showfrmConfirmarEliminar(obj) {

            var userid_eliminar = $(obj).attr("data-userid");

            $("#<%: hid_userid_eliminar.ClientID %>").val(userid_eliminar);

            $("#frmConfirmarEliminar").modal("show");
            return false;
        }

        function hidefrmConfirmarEliminar() {
            $("#frmConfirmarEliminar").modal("hide");
            return false;
        }
        function ocultarBotonesConfirmacion() {
            $("#pnlBotonesConfirmacionEliminar").hide();
            return false;
        }

        function showDatosUsuario() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }
        function GetoutDatos() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
            //$("#box_resultado").show("slow");

        }
        function showResultado() {
            $("#box_resultado").show("slow");
        }


        function showfrmAviso() {

            $("#frmAviso").modal("show");
            GetoutDatos();
            return false;
        }

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }
        function validarGuardar() {

            var ret = true;
            var formatoEmail = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            var formatoCuit = /^[\d+$]{11}$/;

            /*
            Following chars = letter OR number OR _ (underscore) . (point) - (dash)
            Minimum length 5 chars
            Maximum length 20 chars
            */
            var formatoUser = /^[A-Za-z0-9_-]{6,20}$/;

            $("#Req_Username").hide();
            $("#Req_Apellido").hide();
            $("#Req_Nombre").hide();
            $("#Req_Email").hide();
            $("#ValFormato_Email").hide();
            $("#Valformato_User").hide();
            $("#Req_UsernameSADE").hide();
            $("#Req_ReparticionSADE").hide();
            $("#Req_SectorSADE").hide();
            $("#Req_Cuit").hide();
            $("#ValFormato_Cuit").hide();


            if ($("#<%: hid_userid.ClientID%>").val().length < 1) {
                if ($.trim($("#<%: txtUsername.ClientID %>").val()).length > 0) {
                    if (!formatoUser.test($.trim($("#<%: txtUsername.ClientID %>").val()))) {
                        $("#Valformato_User").css("display", "inline-block");
                        ret = false;
                    }
                }
                else {
                    $("#Req_Username").css("display", "inline-block");
                    ret = false;
                }
            }

            if ($.trim($("#<%: txtApellido.ClientID %>").val()).length == 0) {
                $("#Req_Apellido").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtNombre.ClientID %>").val()).length == 0) {
                $("#Req_Nombre").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtEmail.ClientID %>").val()).length > 0) {
                if (!formatoEmail.test($.trim($("#<%: txtEmail.ClientID %>").val()))) {
                    $("#ValFormato_Email").css("display", "inline-block");
                    ret = false;
                }
            }
            else {
                $("#Req_Email").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtCuit.ClientID %>").val()).length > 0) {
                if (!formatoCuit.test($.trim($("#<%: txtCuit.ClientID %>").val()))) {
                    $("#ValFormato_Cuit").css("display", "inline-block");
                    ret = false;
                }
            }
            else {
                $("#Req_Cuit").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtUsernameSADE.ClientID %>").val()).length > 0 ||
                $.trim($("#<%: txtReparticionSADE.ClientID %>").val()).length > 0 ||
                $.trim($("#<%: txtSectorSADE.ClientID %>").val()).length > 0) {

                if ($.trim($("#<%: txtUsernameSADE.ClientID %>").val()).length == 0) {
                    $("#Req_UsernameSADE").css("display", "inline-block");
                    ret = false;
                }

                if ($.trim($("#<%: txtReparticionSADE.ClientID %>").val()).length == 0) {
                    $("#Req_ReparticionSADE").css("display", "inline-block");
                    ret = false;
                }

                if ($.trim($("#<%: txtSectorSADE.ClientID %>").val()).length == 0) {
                    $("#Req_SectorSADE").css("display", "inline-block");
                    ret = false;
                }
            }

            if (ret) {
                ocultarBotonesGuardar();
            }

            return ret;

        }
        function validarCuit() {

            var ret = true;

            var formatoCuit = /^[\d+$]{11}$/;


            $("#Req_Cuit").hide();
            $("#ValFormato_Cuit").hide();


            if ($.trim($("#<%: txtCuit.ClientID %>").val()).length > 0) {
                if (!formatoCuit.test($.trim($("#<%: txtCuit.ClientID %>").val()))) {
                    $("#ValFormato_Cuit").css("display", "inline-block");
                    ret = false;
                }
            }
            else {
                $("#Req_Cuit").css("display", "inline-block");
                ret = false;
            }

            //if (ret) {
            //    ocultarBotonesGuardar();
            //}

            return ret;

        }
    </script>


</asp:Content>

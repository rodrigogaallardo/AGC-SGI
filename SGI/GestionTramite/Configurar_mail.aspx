<%@ Page Title="Configurar Mail" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Configurar_mail.aspx.cs"
    Inherits="SGI.GestionTramite.Configurar_mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>


    <link rel="stylesheet" type="text/css" href="../Content/css/jquery-ui-timepicker-addon.css" />
    <script type="text/javascript" src="../Scripts/jquery-ui-timepicker-addon.js"></script>


    <asp:Panel ID="pnlBotonDefault" runat="server">

        <div class="">

            <%-- Datos del Servidror --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- titulo collapsible Datos del Servidor --%>
                <div class="accordion-heading">
                    <a id="bt_Datos_Server_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_Datos_Server" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_Datos_tituloControl" runat="server" Text="Datos del Servidor"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="collapse_bt_Datos_Server">
                    <div class="widget-content">

                        <%--GridView con los datos --%>
                        <asp:UpdatePanel ID="updPnlServer" runat="server" UpdateMode="Conditional" class="mleft30 mright30">
                            <ContentTemplate>
                                <asp:GridView
                                    ID="grdDatosServer"
                                    runat="server"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    CssClass="table table-bordered table-striped table-hover with-check"
                                    AllowPaging="true"
                                    PageSize="30">
                                    <Columns>
                                        <asp:BoundField DataField="id_profile" HeaderText="Id" />
                                        <asp:BoundField DataField="profile_name" HeaderText="Nombre del perfil" />
                                        <asp:BoundField DataField="is_default" HeaderText="Uso por defecto" />
                                        <asp:BoundField DataField="display_name" HeaderText="Nombre a Mostrar" />
                                        <asp:BoundField DataField="email_address" HeaderText="Email de respuesta" />
                                        <asp:BoundField DataField="smtp" HeaderText="SMTP" />
                                        <asp:BoundField DataField="port" HeaderText="Puerto" />
                                        <asp:BoundField DataField="username" HeaderText="Usuario" />
                                        <asp:BoundField DataField="password" HeaderText="Contraseña" />
                                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="Acciones" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditarConfig" runat="server" CommandArgument='<%# Eval("id_profile") %>'
                                                    data-toggle="tooltip" title="Editar" CssClass="link-local" OnClick="btnEditarConfig_Click">
                                                        <i class="icon-edit" style="transform: scale(1.2);"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnVerUsuarios" runat="server" CommandArgument='<%# Eval("id_profile") %>'
                                                    data-toggle="tooltip" title="Ver Usuarios" CssClass="link-local" OnClick="btnVerUsuarios_Click">
                                                        <i class="icon-eye-open" style="transform: scale(1.2);"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEliminarConfig" runat="server" CommandArgument='<%# Eval("id_profile") %>'
                                                    data-toggle="tooltip" title="Eliminar" CssClass="link-local" OnClick="btnEliminarConfig_Click">
                                                        <i class="icon-trash" style="transform: scale(1.2);"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                            <img src="../Content/img/app/NoRecords.png" />No se encontraron registros con los filtros ingresados.
                                        </asp:Panel>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%-- Boton para Actualizar --%>
                        <asp:UpdatePanel ID="updPnlGuardar" runat="server">
                            <ContentTemplate>
                                <div class="pull-right">
                                    <asp:LinkButton ID="btnAgregarCfg" runat="server" CssClass="btn  btn-inverse" OnClientClick="return AgregarNuevaConfig();" >
                                    <i class="icon-white icon-plus"></i>
                                    <span class="text">Agregar</span>
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                    </div>

                </div>
            </div>

            <%-- Prioridades --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                <%-- titulo collapsible Prioridades --%>
                <div class="accordion-heading">
                    <a id="bt_Prioridades" data-parent="#collapse-group" href="#collapse_bt_Prioridades" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="lblPrioridades" runat="server" Text="Prioridades"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="collapse_bt_Prioridades">
                    <%-- aqui va todo --%>
                    <div class="widget-content">
                        <asp:UpdatePanel ID="updPrior" runat="server" UpdateMode="Conditional" class="mleft10 mright10">
                            <ContentTemplate>
                                <asp:GridView ID="gvPrioridades" runat="server" OnRowCommand="gvPrioridades_RowCommand" AutoGenerateColumns="false" AllowPaging=" true" DataKeysName="Prior_ID" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                    <Columns>
                                        <asp:BoundField DataField="Prior_ID" HeaderText="Prioridad" />
                                        <asp:BoundField DataField="Prior_Desde" HeaderText="Hora Inicio" />
                                        <asp:BoundField DataField="Prior_Hasta" HeaderText="Hora Fin" />
                                        <asp:BoundField DataField="Prior_Reenvio" HeaderText="Reiterancia" />
                                        <asp:BoundField DataField="Prior_Observacion" HeaderText="Observación" />

                                        <asp:TemplateField ItemStyle-Width="15px" HeaderText="Acción" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="Editar" CssClass="link-local" OnClick="lnkEdit_Click">
                                                <i class="icon-edit" style="transform: scale(1.2);"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <%-- Estado Job --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                <%-- titulo collapsible Prioridades --%>
                <div class="accordion-heading">
                    <a id="btn_Job" data-parent="#collapse-group" href="#collapse_btn_Job" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="Label1" runat="server" Text="Estado Job"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="collapse_btn_Job">
                    <%-- aqui va todo --%>
                    <asp:UpdatePanel ID="updEstadoJob" runat="server" UpdateMode="Conditional" class="mleft30 mright30">
                        <ContentTemplate>
                            <div class="form-horizontal">
                                <fieldset>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">
                                                <asp:Label ID="lblEstadoJob" runat="server" AssociatedControlID="txtEstadoJob" Text="Estado Job:" class="control-label"></asp:Label>
                                                <asp:Label ID="txtEstadoJob" runat="server" Style="text-align: center" Font-Size="Medium" class="control-label" Text="Estado"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-- Boton para Actualizar --%>
                    <asp:UpdatePanel ID="updEstadoBtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="pull-right mright20 mbottom20">
                                <asp:LinkButton ID="btnGuardarJob" Text="Boton Estado" runat="server" CssClass="btn  btn-inverse" ValidationGroup="Guardar" OnClick="btnGuardarJob_OnClick">
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>

    </asp:Panel>

    <div id="editModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="editModalLabel">Editar Prioridad</h3>
        </div>

        <asp:Panel runat="server">
            <div class="modal-body">
                <table class="table">
                    <tr>
                        <td>Desde</td>
                        <td>
                            <asp:TextBox ID="txtDesde" runat="server" CssClass="ic"></asp:TextBox>
                        <td>
                    </tr>
                    <tr>
                        <td>Hasta</td>
                        <td>
                            <asp:TextBox ID="txtHasta" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Reenvio</td>
                        <td>
                            <asp:TextBox ID="txtReenvio" runat="server" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txtReenvio" ErrorMessage="Value must be a whole number" />
                        </td>
                    </tr>
                    <tr>
                        <td>Observacion</td>
                        <td>
                            <asp:TextBox ID="txtObservacion" TextMode="multiline" runat="server" Width="100%"></asp:TextBox></td>
                    </tr>
                </table>
            </div>

            <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional" class="mleft30 mright30">
                <ContentTemplate>
                    <asp:GridView ID="grdEditActual" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                        ItemType="SGI.Model.clsItemGrillaDatosServerMail" AllowPaging="true" PageSize="30">
                        <Columns>
                            <asp:BoundField DataField="grdEditActual_Prioridad" HeaderText="Prioridad" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="grdEditActual_Desde" HeaderText="Desde" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="grdEditActual_Hasta" HeaderText="Hasta" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="grdEditActual_Reenvio" HeaderText="Reenvio" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="grdEditActual_Observacion" HeaderText="Observacion" ItemStyle-Width="40%" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal-footer">
                <asp:LinkButton ID="btnGuardarPrior" runat="server" CssClass="btn btn-inverse" OnClick="btnGuardarPrior_Click">
                    <i class="icon-white icon-refresh"></i>
                    <span class="text">Actualizar</span>
                </asp:LinkButton>

                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-inverse" OnClientClick="LimpiarEditar();">
                    <span class="text">Cerrar</span>
                </asp:LinkButton>
            </div>

        </asp:Panel>
    </div>



    <div id="frmMsj" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Mensaje</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon imoon-info fs32" style="color: darkcyan"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updMsj" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMsj" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('.modal-backdrop').remove();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>





    <div id="frmConfigurarMail" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Configurar e-mail service</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updConfigurarMail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField id="hid_modificando" runat="server"/>
                            <div class="form-horizontal pright10">
                                <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtProfileName" class="control-label">Nombre del perfil (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtProfileName" runat="server" MaxLength="200" Width="185px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfv_txtProfileName" runat="server"
                                    ControlToValidate="txtProfileName"
                                    ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RequiredFieldValidator>

                                <asp:Label ID="lblDisplayName" runat="server" MaxLength="200" AssociatedControlID="txtDisplayName" class="control-label">Nombre a mostrar (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDisplayName" runat="server" Width="300px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfv_txtDisplayName" runat="server"
                                    ControlToValidate="txtDisplayName"
                                    ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RequiredFieldValidator>

                                <asp:Label ID="lblEsDefault" runat="server" AssociatedControlID="rbEsDefault" class="control-label">Es cuenta por defecto (*):</asp:Label>
                                <div class="controls">
                                    <asp:RadioButtonList ID="rbEsDefault" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Si" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>

                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" class="control-label">Email (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfv_txtEmail" runat="server"
                                    ControlToValidate="txtEmail"
                                    ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RequiredFieldValidator>

                                <asp:Label ID="lblSMTP" runat="server" AssociatedControlID="txtSMTP" class="control-label">SMTP (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtSMTP" runat="server" MaxLength="200" Width="120px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfv_txtSMTP" runat="server"
                                    ControlToValidate="txtSMTP"
                                    ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RequiredFieldValidator>

                                <asp:Label ID="lblPuerto" runat="server" AssociatedControlID="txtPuerto" class="control-label">Puerto (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtPuerto" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                </div>
                                <asp:RegularExpressionValidator ID="rev_txtPuerto" runat="server"
                                    ControlToValidate="txtPuerto"
                                    ErrorMessage="Solo se permite ingresar Números" CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationExpression="\d+"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfv_txtPuerto" runat="server"
                                    ControlToValidate="txtPuerto"
                                    ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RequiredFieldValidator>

                                <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" class="control-label">Usuario (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtUsuario" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfv_txtUsuario" runat="server"
                                    ControlToValidate="txtUsuario"
                                    ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                    ValidationGroup="GuardarConfigEmail">
                                </asp:RequiredFieldValidator>

                                <asp:Label ID="lblContrasena" runat="server" AssociatedControlID="txtContrasena" class="control-label">Contraseña (*):</asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="txtContrasena" runat="server" MaxLength="200" Width="185px"></asp:TextBox>
                                    <div class="control-label" style="text-align: left; float: left; margin-left: -10px">
                                        <asp:RequiredFieldValidator ID="rfv_txtContrasena" runat="server"
                                            ControlToValidate="txtContrasena"
                                            ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 col-sm-6" Display="Dynamic"
                                            ValidationGroup="GuardarConfigEmail">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="modal-footer">
                    <asp:UpdatePanel ID="updBotonesConfigurarMail" runat="server">
                        <ContentTemplate>
                            <div id="pnlBotonesConfigurarMail" class="form-group">
                                <asp:LinkButton ID="btnAceptarConfigurarMail" runat="server" CssClass="btn btn-primary" 
                                              OnClick="btnAceptarConfigurarMail_Click" ValidationGroup="GuardarConfigEmail" >
                                                    <i class="imoon imoon-ok"></i>
                                                    <span class="text">Aceptar</span>
                                </asp:LinkButton>
                                <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('.modal-backdrop').remove();">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    
    <%--Modal ver Usuarios--%>
    <div id="frmVerUsuario" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Usuarios relacionados al perfil</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updVerUsuario" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField runat="server" ID="hid_id_perfil" />

                            <asp:GridView
                                ID="grdVerUsuario"
                                runat="server"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                CssClass="table table-bordered table-striped table-hover with-check"
                                AllowPaging="true"
                                PageSize="30">
                                <Columns>
                                    <asp:BoundField DataField="id_rel" HeaderText="id" Visible="false" />
                                    <asp:BoundField DataField="ws_username" HeaderText="Usuario" />
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="10px" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEliminarRelUsuario" runat="server" CommandArgument='<%# Eval("id_rel") %>'
                                                data-toggle="tooltip" title="Eliminar" CssClass="link-local" OnClick="btnEliminarRelUsuario_Click">
                                                        <i class="icon-trash" style="transform: scale(1.2);"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                        <img src="../Content/img/app/NoRecords.png" />No se encontraron registros.
                                    </asp:Panel>
                                </EmptyDataTemplate>
                            </asp:GridView>

                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="updAgregarUsuarioPopOver" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <div class="col-sm-6">
                                                <asp:Label ID="lblUsuarioNuevo" runat="server" AssociatedControlID="txtUsuarioNuevo" class="control-label col-sm-6">Usuario (*):</asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtUsuarioNuevo" runat="server" MaxLength="200" Width="185px"></asp:TextBox>

                                                    <asp:LinkButton ID="lnkAgregarUsuario" runat="server"
                                                        CssClass="btn btn-primary"
                                                        ValidationGroup="GuardarUsuarioNuevo"
                                                        OnClick="lnkAgregarUsuarioNuevo_Click"
                                                        title="Agregar nuevo usuario al perfil">
                                                                <i class="imoon imoon-plus"></i>
                                                                    <span class="text">Agregar</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="form-horizontal pright10 mleft40">
                                                <div class="form-group mleft40">
                                                    <asp:RequiredFieldValidator ID="rfv_txtUsuarioNuevo" runat="server"
                                                        ControlToValidate="txtUsuarioNuevo"
                                                        ErrorMessage="El campo es obligatorio." CssClass="alert alert-small alert-danger mbottom0 mtop5 mleft40" Display="Dynamic"
                                                        ValidationGroup="GuardarUsuarioNuevo">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="hidePopOver(); $('.modal-backdrop').remove();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    
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
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('.modal-backdrop').remove();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $("[data-toggle=popover]").popover({
            html: true,
            content: function () {
                return $('#popover-content').html();
            }
        });
        function hidePopOver() {
            $("[data-toggle='popover']").popover('hide');
        }

        function showfrmMsj() {
            $('.modal-backdrop').remove();
            $("#frmMsj").modal("show");
            return false;
        }
        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function LimpiarEditar() {
            $('#<%=txtDesde.ClientID %>').val("");
            $('#<%=txtHasta.ClientID %>').val("");
            $('#<%=txtReenvio.ClientID %>').val("");
            $('#<%=txtObservacion.ClientID %>').val("");
            return false;
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function bt_btnUpDown_collapse_click(obj) {
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
        $(function () {
            $('#<%=txtDesde.ClientID %>').timepicker({
                timeOnlyTitle: 'Seleccionar Hora',
                timeText: 'Seleccion',
                hourText: 'Hora',
                minuteText: 'Minuto',
                secondText: 'Segundo',
                currentText: "Ahora",
                closeText: 'Salir'
            });
        })
            $(function () {
                $('#<%=txtHasta.ClientID %>').timepicker({
                    timeOnlyTitle: 'Seleccionar Hora',
                    timeText: 'Seleccion',
                    hourText: 'Hora',
                    minuteText: 'Minuto',
                    secondText: 'Segundo',
                    currentText: "Ahora",
                    closeText: 'Salir'
                });
            })

            $(document).ready(function () {
                $('#<%=txtReenvio.ClientID %>').keydown(function (e) {
                    // Allow: backspace, delete, tab, escape, enter and .
                    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                        // Allow: Ctrl+A
                        (e.keyCode == 65 && e.ctrlKey === true) ||
                        // Allow: home, end, left, right
                        (e.keyCode >= 35 && e.keyCode <= 39)) {
                        // let it happen, don't do anything
                        return;
                    }
                    // Ensure that it is a number and stop the keypress
                    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                        e.preventDefault();
                    }
                });
            });

        function AgregarNuevaConfig() {
            
            $("#<%: txtProfileName.ClientID %>").val("");
            $("#<%: txtProfileName.ClientID %>").removeAttr("disabled");
            $("#<%: txtDisplayName.ClientID %>").val("");
            $("#<%: txtEmail.ClientID %>").val("");
            $("#<%: txtSMTP.ClientID %>").val("");
            $("#<%: txtPuerto.ClientID %>").val("");
            $("#<%: txtUsuario.ClientID %>").val("");
            $("#<%: txtContrasena.ClientID %>").val("");
            showfrmConfigurarMail();
        }


        function validarAgregarConfigurarMail() {
            var ret = true;

            if ($.trim($("#<%: txtProfileName.ClientID %>").val()).length == 0) {
                ret = false;
            }

            if ($.trim($("#<%: txtDisplayName.ClientID %>").val()).length == 0) {
                ret = false;
            }

            if ($.trim($("#<%: txtEmail.ClientID %>").val()).length == 0) {
                ret = false;
            }

            if ($.trim($("#<%: txtSMTP.ClientID %>").val()).length == 0) {
                ret = false;
            }

            if ($.trim($("#<%: txtPuerto.ClientID %>").val()).length == 0) {
                ret = false;
            } 

            if ($.trim($("#<%: txtUsuario.ClientID %>").val()).length == 0) {
                ret = false;
            }

            if ($.trim($("#<%: txtContrasena.ClientID %>").val()).length == 0) {
                ret = false;
            }

        }

        function showfrmVerUsuario() {
            $("#frmVerUsuario").modal("show");
            return false;
        }

        function hidefrmVerUsuario() {
            debugger;
            $("#frmVerUsuario").modal("hide");
            return false;
        }

            function showfrmConfigurarMail() {

                $("#frmConfigurarMail").modal("show");
                return false;
            }
            function hidefrmConfigurarMail() {

                $("#frmConfigurarMail").modal("hide");
                return false;
            }


    </script>



</asp:Content>

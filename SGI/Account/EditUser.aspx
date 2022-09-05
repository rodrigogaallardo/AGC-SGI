<%@ Page Title="Edición de usuario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="SGI.Account.EditUser" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <hgroup class="title">
        <h1>Use el formulario siguiente para cambiar los datos de usuario.</h1>
    </hgroup>


    <asp:Panel ID="pnlEditarDatos" runat="server" CssClass="form-horizontal">
        
            <h3>Datos del usuario</h3>


            <div style="color: red; padding: 10px 0px 10px 0px">
                <div class="field-validation-error">
                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                </div>
            </div>

            <fieldset>
            
            
                <div class="control-group">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="control-label">Nombre de Usuario (*):</asp:Label>
                    <div class="controls">

                        <asp:TextBox ID="UserName" runat="server" MaxLength="50" Width="150px" CssClass="input-xlarge"  Enabled="false"></asp:TextBox>
                    </div>
                </div>
           

                <div class="control-group">
                    <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" CssClass="control-label">E-mail (*):</asp:Label>
                    <div class="controls">
                        <asp:TextBox ID="Email" runat="server" MaxLength="50" Width="250px"></asp:TextBox>

                        <div class="req">
                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                Display="Dynamic" ErrorMessage="E-mail es requerido." ValidationGroup="Guardar"
                                SetFocusOnError="True" CssClass="field-validation-error"></asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="EmailRegEx" runat="server" ControlToValidate="Email" Display="Dynamic"
                                CssClass="field-validation-error" ErrorMessage="E-mail no tiene un formato válido. Ej: nombre@servidor.com"
                                SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="Guardar">
                            </asp:RegularExpressionValidator>

                        </div>
                    </div>
                </div>

                <div class="control-group">
                    <asp:Label ID="ApellidoLabel" runat="server" AssociatedControlID="Apellido" CssClass="control-label">Apellido (*):</asp:Label>
                    <div class="controls">
                        <asp:TextBox ID="Apellido" runat="server" MaxLength="50" Width="150px"></asp:TextBox>

                        <div class="req">
                            <asp:RequiredFieldValidator ID="ApellidoRequired" runat="server" ControlToValidate="Apellido"
                                Display="Dynamic" ErrorMessage="Apellido es requerido." ValidationGroup="Guardar"
                                SetFocusOnError="True" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>


                <div class="control-group">
                    <asp:Label ID="NombreLabel" runat="server" AssociatedControlID="Nombre" CssClass="control-label">Nombre/s (*):</asp:Label>
                    <div class="controls">
                        <asp:TextBox ID="Nombre" runat="server" MaxLength="50" Width="250px"></asp:TextBox>

                        <div class="req">
                            <asp:RequiredFieldValidator ID="NombreRequired" runat="server" ControlToValidate="Nombre"
                                Display="Dynamic" ErrorMessage="Nombre es requerido." ValidationGroup="Guardar"
                                SetFocusOnError="True" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>


                <div style="margin-left:200px; margin-top:10px">
                
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary"  OnClick="ActualizarUsuario" ValidationGroup="Guardar" >
                        <i class="icon-white icon-ok"></i>
                        <span class="text">Guardar</span>

                    </asp:LinkButton>

                </div>

            </fieldset>

        

    </asp:Panel>

    <asp:Panel ID="pnlResultado" runat="server" Visible="false" Style="margin-top:20px"  >

        <div class="alert alert-info">
            <asp:Label ID="lblMensajeResultado" runat="server" 
                class="titulo-2"
                Text="Los cambios se han realizado exitosamente."></asp:Label>
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" Style="margin-top:20px"  >

        <div class="alert alert-error" >
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <asp:Label ID="lblMensajeError" runat="server" 
                class="titulo-2"
                Text="Ha ocurrido un error al realizr los cambios."></asp:Label>
        </div>


    </asp:Panel>


</asp:Content>

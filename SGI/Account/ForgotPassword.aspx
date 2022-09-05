<%@ Page Title="Recupero de Contraseña" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SGI.Account.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <hgroup class="title">
        <h1>Utilice este formulario para recuperar su contrase&ntilde;a.</h1>
    </hgroup>


    <asp:PasswordRecovery ID="PasswordRecovery" runat="server" 
        OnSendingMail="PasswordRecovery_SendingMail"
        OnSendMailError="PasswordRecovery1_SendMailError" 
        UserNameFailureText="No fue posible tener acceso a su información. Inténtelo nuevamente."
        SuccessText="Se le ha enviado la contraseña.">
        
        <UserNameTemplate>
            <h1>¿Olvido su contrase&ntilde;a?</h1>
            
            <label>Ingrese el nombre de usuario, presione el bot&oacute;n y recibir&aacute; su contrase&ntilde;a por correo electr&oacute;nico.</label>
            
            <asp:Panel ID="pnlContent" runat="server" CssClass="form-horizontal" DefaultButton="SubmitButton">

                <fieldset>

                    <div class="control-group">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="control-label" placeholder="Usuario">Nombre de usuario:</asp:Label>
                        <div class="controls">
                            <div class="input-prepend">
                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>

                                <asp:LinkButton ID="SubmitButton" runat="server" CssClass="btn btn-primary" CommandName="Submit" ValidationGroup="PasswordRecovery1" style="margin-left:10px">
                                    <i class="icon-white icon-envelope"></i>
                                    <span class="text">Enviar correo</span>
                                </asp:LinkButton>
                            </div>
                            <div>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" CssClass="field-validation-error"
                                ErrorMessage="El nombre de usuario es requerido." ValidationGroup="PasswordRecovery1"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        

                    </div>
                    
                                    
                    <p class="validation-summary-errors">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                    </p>

                   
                </fieldset>

            </asp:Panel>

    </UserNameTemplate>
        <SuccessTemplate>
            <div class="widget-box" style="padding: 15px;">

            
                <div class="titulo-1">Se le ha enviado la contraseña al e-mail <asp:Label ID="lblEmail" runat="server"  ForeColor="Blue" ></asp:Label></div>

           
            </div>
           
        </SuccessTemplate>
        

        
    </asp:PasswordRecovery>
</asp:Content>

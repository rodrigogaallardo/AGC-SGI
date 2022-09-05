<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SGI.Account.Login" %>
<%@ OutputCache Location="None"  %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1>Inicie sesi&oacute;n para utilizar el sistema.</h1>
    </hgroup>

      <%: Scripts.Render("~/bundles/browser") %>

    <section id="loginForm">
        
        <asp:Login id="LoginControl" runat="server" 
            ViewStateMode="Disabled" RenderOuterTable="false" 
            InstructionText="Escriba su nombre de usuario y contraseña." 
            PasswordLabelText="Contraseña:" PasswordRequiredErrorMessage="La contraseña es requerida."
            UserNameLabelText="Nombre de Usuario:" UserNameRequiredErrorMessage="El Nombre de usuario es requerido" 
            DestinationPageUrl="~/GestionTramite/BandejaEntrada.aspx"
            OnLoginError="LoginControl_OnLoginError" 
            FailureText="Nombre de usuario o contraseña incorrecta.<br />Por favor, intente nuevamente."
            RememberMeText="Recordarme en este equipo." TitleText=""
            PasswordRecoveryText="Recuperar contraseña"
            PasswordRecoveryUrl="~/Account/ForgotPassword"

            >
            <LayoutTemplate>
                
                <asp:Panel ID="pnl" runat="server" DefaultButton="LinkButton1">
                
                   <div class="form-horizontal">
                
                       
                <legend>Iniciar sesi&oacute;n</legend>
                <fieldset>
                    
                    <div class="control-group">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="UserName" CssClass="control-label">Nombre de usuario</asp:Label>
                        <div class="controls">
                            <div class="input-prepend">
                                <span class="add-on"><i class="icon-user"></i></span>
                                <asp:TextBox runat="server" ID="UserName"  Width="200" Placeholder="Usuario" />
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="UserName" CssClass="field-validation-error" 
                                ErrorMessage="El Nombre de usuario es obligatorio." />
                                
                        </div>
                    </div>
                    <div class="control-group">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="Password" 
                            CssClass="control-label">Contraseña</asp:Label>
                        
                        <div class="controls">
                            <div class="input-prepend">
                                <span class="add-on"><i class="icon-lock"></i></span>
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" Width="200" Text ="123456" 
                                    Placeholder="Contraseña" />
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="Password" CssClass="field-validation-error" 
                                ErrorMessage="La contraseña es obligatoria." />
                        </div>

                    </div>
                    <div class="control-group">
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="RememberMe" 
                            CssClass="control-label">¿Recordar contrase&ntilde;a?
                        </asp:Label>

                        <div class="controls">
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Login" CssClass="btn btn-primary" style="margin-left:10px">
                                <i class="icon-white icon-user"></i>
                                <span class="text">Iniciar Sesi&oacute;n</span>
                            </asp:LinkButton>

                          
                        </div>
                    </div>
                    <div class="control-group">
                        
                        <div class="controls">
                            <asp:HyperLink ID="lnkPasswordRecovery" runat="server" 
                                NavigateUrl="~/Account/ForgotPassword">¿Olvidaste tu contraseña?</asp:HyperLink>
                        </div>
                    </div>

                    <p class="validation-summary-errors" style="margin-left: 30px">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                    
                   
                </fieldset>
               </div>
               </asp:Panel>
            </LayoutTemplate>
        </asp:Login>
    </section>

</asp:Content>



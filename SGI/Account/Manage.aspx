<%@ Page Title="Administrar cuenta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="SGI.Account.Manage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <section id="passwordForm">
        
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="message-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="changePassword" >
            
            <p>Ha iniciado sesión como <strong><%: User.Identity.Name %></strong>.</p>

            <h3>Cambiar contraseña</h3>
            <asp:ChangePassword ID="ChangePassword1" runat="server" 
                CancelDestinationPageUrl="~/" ViewStateMode="Disabled" 
                RenderOuterTable="false" 
                SuccessPageUrl="Manage.aspx?m=ChangePwdSuccess">
                <ChangePasswordTemplate>
                    <p class="validation-summary-errors">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                    <fieldset class="changePassword">
                        
                        <ul>
                            <li>
                                <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword">Contraseña actual</asp:Label>
                                <asp:TextBox runat="server" ID="CurrentPassword" CssClass="passwordEntry" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CurrentPassword"
                                    CssClass="field-validation-error" ErrorMessage="El campo de contraseña actual es obligatorio."
                                    ValidationGroup="ChangePassword" />
                            </li>
                            <li>
                                <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword">Nueva contraseña</asp:Label>
                                <asp:TextBox runat="server" ID="NewPassword" CssClass="passwordEntry" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="NewPassword"
                                    CssClass="field-validation-error" ErrorMessage="La contraseña nueva es obligatoria."
                                    ValidationGroup="ChangePassword" />
                            </li>
                            <li>
                                <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword">Confirmar la nueva contraseña</asp:Label>
                                <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ConfirmNewPassword"
                                    CssClass="field-validation-error" Display="Dynamic" ErrorMessage="La confirmación de contraseña nueva es obligatoria."
                                    ValidationGroup="ChangePassword" />
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                    CssClass="field-validation-error" Display="Dynamic" ErrorMessage="La nueva contraseña y la contraseña de confirmación no coinciden."
                                    ValidationGroup="ChangePassword" />
                            </li>
                        </ul>
                        <div style="margin-left:25px">
                        <asp:Button ID="Button1" runat="server" CommandName="ChangePassword" Text="Cambiar contraseña" ValidationGroup="ChangePassword" CssClass="btn btn-primary" />
                        </div>
                    </fieldset>
                </ChangePasswordTemplate>
                
            </asp:ChangePassword>
        </asp:PlaceHolder>
    </section>


</asp:Content>

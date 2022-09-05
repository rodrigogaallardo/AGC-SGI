<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActivateUser.aspx.cs" Inherits="SGI.Account.ActivateUser" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <hgroup class="title">
        <h1>Activación de Usuario</h1>
    </hgroup>

    <asp:Panel ID="pnlActivacionOk" runat="server">
        <h4>El usuario <b>
            <asp:Label ID="UsuarioLabel" runat="server" /></b> ha sido activado correctamente.</h4>
        <p>
            Ahora puede ingresar al sistema haciendo click en el siguiente enlace e introduciendo su usuario y contraseña
            <asp:HyperLink ID="lnkIniciarSesion" runat="server" NavigateUrl="~/Account/Login">Iniciar Sesi&oacute;n</asp:HyperLink>.
        </p>
    </asp:Panel>

    <div id="ActivacionError">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="field-validation-error"></asp:Label>
    </div>

</asp:Content>

<%@ Page
    Title="Administrar Expedientes Electronicos"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="SadeForm.aspx.cs"
    Inherits="SGI.Operaciones.SadeForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
    
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div style="float: left;">
        <p class="lead mtop20">Solicitud</p>
        <asp:Repeater ID="txtBoxRepeater" runat="server">
            <ItemTemplate>
                <asp:TextBox ID="txtBoxSolicitud" runat="server"></asp:TextBox>
                <br />
            </ItemTemplate>
        </asp:Repeater>
        <asp:Button ID="AddTextBoxButton" runat="server" Text="Agregar" OnClick="AddTextBoxButton_Click" />
        <div style="display: flex; justify-content: space-between;">
        <div style="display: flex; justify-content: space-between;">
            <asp:RadioButton ID="rdoUser" runat="server" Text="Usuario" GroupName="RadialGroup"></asp:RadioButton>
            <asp:RadioButton ID="rdoGroup" runat="server" Text="Grupo" GroupName="RadialGroup"></asp:RadioButton>
            <asp:TextBox ID="textBoxDestino" runat="server"></asp:TextBox>
            <asp:TextBox ID="textBoxReparticionDestino" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="btnProcesar" runat="server" Text="Realizar Pase" OnClick="btnProcesar_Click" />
            <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" OnClick="btnDesbloquear_Click" />
            <asp:Button ID="btnBloquear" runat="server" Text="Bloquear" OnClick="btnBloquear_Click" />
            <asp:Button ID="btnConsultar_Bloqueo" runat="server" Text="Consultar Bloqueo" OnClick="btnConsultar_Bloqueo_Click" />
            <asp:Button ID="btnConsultar_EE" runat="server" Text="Consultar Expediente Electronico" OnClick="btnConsultar_EE_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" />
        </div>
    </div>
    </div>
    
    <div style="float: right;">
        <asp:Label ID="lblResult" runat="server" Text="Resultado"></asp:Label>
        <asp:Label ID="lblError" runat="server" Text="Errores" Visible="false"></asp:Label>
    </div>
</asp:Content>


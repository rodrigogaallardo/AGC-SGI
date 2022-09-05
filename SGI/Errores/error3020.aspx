<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="error3020.aspx.cs" Inherits="SGI.Errores.error3020" %>

<asp:content id="Content1" contentplaceholderid="HeadContent" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="MainContent" runat="server">


    <div class="text-center">
        <h2>Ha ocurrido un error al intentar cargar la p&aacute;gina.</h2>
        <h3>
            <asp:Label ID="lblPagina" runat="server" CssClass="text-danger"></asp:Label>
        </h3>
        <div class="mtop20">
            <asp:LinkButton ID="btnHome" runat="server" CssClass="btn btn-default" PostBackUrl="~/">
                <i  class="imoon imoon-home"></i>
                <span class="text">Inicio</span>
            </asp:LinkButton>
        </div>
    </div>

</asp:content>

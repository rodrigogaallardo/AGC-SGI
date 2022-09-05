<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs"
     Inherits="SGI.Dashboard.Dashboard" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
            function mostrarMensaje(texto, titulo) {
                $.gritter.add({
                    title: titulo,
                    text: texto,
                    image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                    sticky: false
                });
            }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/flot") %>
    <%: Styles.Render("~/bundles/flotCss") %>

    <hgroup class="title">
        <h1>Panel de control</h1>
    </hgroup>
    <div style="width: 100%; text-align: center; padding-top: 50px;">
        
        <div >
            <h1>No disponible a&uacute;n</h1>

        </div>

        <div style="padding-top: 30px">
            <a href="<%: ResolveUrl("~/") %>" class="btn">ir al Home</a>
        </div>
    </div>

</asp:Content>

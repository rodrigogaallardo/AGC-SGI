<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="error404.aspx.cs" Inherits="SGI.error404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <hgroup class="title">
        <h1>P&aacute;gina no encontrada</h1>
    </hgroup>
    <div style="width:100%; text-align:center;padding-top:50px;">
        <img src="Content/img/app/404.png" />

        <div style="padding-top:30px">
            <a href="<%: ResolveUrl("~/") %>" class="btn">ir al Home</a>
        </div>
    </div>
        
</asp:Content>

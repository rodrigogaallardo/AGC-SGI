﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error3051.aspx.cs" Inherits="SGI.Errores.Error3051" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="text-center">
        <h3>El Número de trámite indicado no existe</h3>
        <div class="mtop20">
            <asp:LinkButton ID="btnHome" runat="server" CssClass="btn btn-default" PostBackUrl="~/">
                <i  class="imoon imoon-home"></i>
                <span class="text">Inicio</span>
            </asp:LinkButton>
        </div>
    </div>
</asp:Content>

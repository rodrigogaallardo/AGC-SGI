<%@ Page Title="Visualización del trámite" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisorTramite.aspx.cs" Inherits="SGI.GestionTramite.VisorTramite" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>
<%@ Register Src="~/GestionTramite/Controls/ucPagos.ascx" TagPrefix="uc1" TagName="ucPagos" %>
<%@ Register Src="~/GestionTramite/Controls/ucNotificaciones.ascx" TagPrefix="uc1" TagName="ucNotificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/Unicorn.Tables") %>

    <uc1:ucCabecera runat="server" id="ucCabecera" />
    <uc1:ucListaRubros runat="server" id="ucListaRubros" />
    <uc1:ucTramitesRelacionados runat="server" id="ucTramitesRelacionados" />
    <uc1:ucPagos runat="server" ID="ucPagos" />
    <uc1:ucNotificaciones runat="server" ID="ucNotificaciones" />
    <uc1:ucListaDocumentos runat="server" id="ucListaDocumentos" />
    <uc1:ucListaTareas runat="server" id="ucListaTareas" />
    
            
</asp:Content>

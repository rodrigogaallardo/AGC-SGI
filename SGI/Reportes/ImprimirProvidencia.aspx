<%@ Page Title="Providencia" Language="C#" MasterPageFile="~/Reportes/Reporte.Master" AutoEventWireup="true" CodeBehind="ImprimirProvidencia.aspx.cs" Inherits="SGI.Reportes.ImprimirProvidencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <webopt:BundleReference ID="BundleReference1" runat="server" Path="~/Content/css" />
    <br />
    <div class="widget-box">
        <p><asp:Label ID="lblObservaciones" runat="server" ></asp:Label></p>
    </div>
    <br />

</asp:content>


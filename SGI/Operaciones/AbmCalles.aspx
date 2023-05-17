<%@  Title="ABM de Calles" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AbmCalles.aspx.cs" Inherits="SGI.AbmCalles"  %>

<%@ Register Src="~/GestionTramite/Controls/ucListaCalle.ascx" TagPrefix="uc1" TagName="ucListaCalle" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function redirect(obj) {
            location.href = obj.data - href;
        }
    </script>

    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>


    <%--Calle--%>
    <div style="display: flex">
        <div class="control-group" style="padding-right: 30px">
            <asp:Label ID="lblCalle" runat="server" AssociatedControlID="ddlCalles" Text="Búsqueda de Calle: "
                class="control-label" />
            <div class="controls">
                <asp:DropDownList ID="ddlCalles" runat="server" Width="300px"></asp:DropDownList>
            </div>
        </div>
    </div>

    <%--Botones--%>
        <div class="control-group" style="float: right">
            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_OnClick" Text="Buscar" />
            <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary" OnClick="btnAgregar_OnClick" Text="Agregar"></asp:Button>
            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClick="btnLimpiar_OnClick" OnClientClick="LimpiarFormulario();">
            <i class="icon-refresh"></i>
            <span class="text">Limpiar</span>
            </asp:LinkButton>
        </div>



    <uc1:ucListaCalle runat="server" id="ucListaCalle" />

    <script>
        $(document).ready(function () {
            inicializar_autocomplete();
        });


        function inicializar_autocomplete() {
            $("#<%: ddlCalles.ClientID %>").select2({
                minimumInputLength: 3
            });
        }


        function LimpiarFormulario() {
            document.getElementById("MainContent_ddlCalles").value = "";
        }
    </script>
</asp:Content>


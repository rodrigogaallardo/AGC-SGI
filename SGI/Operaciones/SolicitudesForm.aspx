<%@ Page
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="SolicitudesForm.aspx.cs"
    Inherits="SGI.Operaciones.SolicitudesForm" %>

<%@ Register Assembly="Syncfusion.EJ.Web" Namespace="Syncfusion.JavaScript.Web" TagPrefix="ej" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup>

    <div class="control-group">
        <label class="control-label" for="txtBuscarSolicitud">Buscar por numero de solicitud</label>

    </div>

    <%--    <ej:Grid ID="Grid1" runat="server">
    </ej:Grid>--%>



    <hr />
    <p class="lead mtop20">Tareas Tramites</p>

    <div class="control-group">
        <div style="width: 100px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecCierre" Text="TipoEstado" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlTipoEstado" runat="server" AutoPostBack="false"
                DataTextField="Descripcion" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>


    <div class="control-group">
        <div style="width: 100px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecLibrado" style=" position: relative; top: -100px;" Text="Fec.Librado" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaLibrado" OnSelectionChanged="calFechaLibrado_SelectionChanged" runat="server"></asp:Calendar>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:CheckBox ID="chkFecLibrado" Text="No Establecer" style=" position: relative; top: -100px;" AutoPostBack="false" runat="server" />
        </div>
    </div>




 

    <asp:HiddenField ID="hdidSolicitud" runat="server" />
    <asp:HiddenField ID="hdtipo" runat="server" />
    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


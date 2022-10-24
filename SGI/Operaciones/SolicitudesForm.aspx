<%@ Page
    Title="Actualizacion de Estado V2"
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
        <div style="width: 100px; display: inline-block; margin-top: 5px;">
            <p class="lead mtop20">Solicitud : </p>
        </div>
        <div style="width: 50px; display: inline-block; margin-top: 5px;">
            <asp:TextBox ID="txtSolicitudId" runat="server" ReadOnly="true" Width="50px"> </asp:TextBox>
        </div>
        <div style="width: 15px; display: inline-block; margin-top: 5px;">
            &nbsp;
        </div>
        <div style="width: 200px; display: inline-block; margin-top: 5px;">
            <asp:TextBox ID="txtdescripcion_tipotramite" runat="server" ReadOnly="true" Width="200px"> </asp:TextBox>
        </div>
    </div>



    <div class="control-group">
        <div style="width: 100px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecCierre" Text="Estado" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlTipoEstado" runat="server" AutoPostBack="false"
                DataTextField="Descripcion" DataValueField="Id">
            </asp:DropDownList>
        </div>
    </div>


    <div class="control-group">
        <div style="width: 100px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecLibrado" Style="position: relative; top: -100px;" Text="Fec.Librado" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaLibrado" OnSelectionChanged="calFechaLibrado_SelectionChanged" runat="server"></asp:Calendar>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:CheckBox ID="chkFecLibrado" Text="No Establecer" Style="position: relative; top: -100px;" AutoPostBack="false" runat="server" />
        </div>
    </div>






    <asp:HiddenField ID="hdidSolicitud" runat="server" />
    <asp:HiddenField ID="hdtipo" runat="server" />
    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


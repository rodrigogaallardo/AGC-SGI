<%@ Page
   Title="Administrar Catalogo Distritos"
     MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="CatalogoDistritosForm.aspx.cs"
    Inherits="SGI.Operaciones.CatalogoDistritosForm" %>

<%@ Register Assembly="Syncfusion.EJ.Web" Namespace="Syncfusion.JavaScript.Web" TagPrefix="ej" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup>
    <script type="text/javascript">
        function ConfirmaTransaccion() {
            <%--var ddlSFtarea = $('#<%: ddlSFtarea.ClientID %>').val();
            if (ddlSFtarea == "") {
                alert("Debe seleccionar una Tarea");
                return false;
            }--%>
            return confirm('¿Confirma la Transaccion?');
        }

    </script>
   
    <p class="lead mtop20">Grupos Distritos</p>


    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Grupo Distrito" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlGrupoDistricto" runat="server" AutoPostBack="false" 
                DataTextField="Nombre" DataValueField="IdGrupoDistricto">
            </asp:DropDownList>

        </div>
    </div>


    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Codigo" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:TextBox ID="txtCodigo" CssClass="input-xlarge" Width="150px" runat="server"> </asp:TextBox>
        </div>
    </div>


    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Descripcion" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:TextBox ID="txtDescripcion" CssClass="input-xlarge" Width="300px" TextMode="MultiLine" Height="100px" runat="server"> </asp:TextBox>
        </div>
    </div>



    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Orden" runat="server"></asp:Label>
        </div>
        <div style="width: 50px; display: inline-block; margin-top: 5px;">
            <asp:TextBox ID="txtOrden" CssClass="input-xlarge" Width="50px" runat="server"> </asp:TextBox>
        </div>
    </div>

    <div class="control-group" style="padding-top:20px;">
        <label class="control-label">Observaciones del Solicitante:</label>
        <div class="controls">
            <asp:TextBox ID="txtObservacionesSolicitante" runat="server" CssClass="form-control" Columns="10" Width="350px" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>


    <asp:HiddenField ID="hdIdDistrito" runat="server" />
    <asp:HiddenField ID="hdIdGrupoDistrito" runat="server" />
    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


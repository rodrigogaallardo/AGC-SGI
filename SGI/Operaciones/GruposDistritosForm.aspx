<%@ Page
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="GruposDistritosForm.aspx.cs"
    Inherits="SGI.Operaciones.GruposDistritosForm" %>

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
    <div class="control-group">
        <label class="control-label" for="txtBuscarSolicitud">Buscar por numero de solicitud</label>

    </div>

   

    <hr />
    <p class="lead mtop20">Grupos Distritos</p>

   <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
           <asp:Label class="control-label" Text="Codigo" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
        <asp:TextBox  ID="txtCodigo" CssClass="input-xlarge" Width="150px" runat="server"> </asp:TextBox>
        </div>
    </div>


    <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Nombre" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
       <asp:TextBox  ID="txtNombre" CssClass="input-xlarge" Width="300px" runat="server"> </asp:TextBox>
        </div>
    </div>
   
      <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Definicion" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
       <asp:TextBox  ID="txtDefinicion" CssClass="input-xlarge" Width="300px" TextMode="MultiLine" Height="100px"  runat="server"> </asp:TextBox>
        </div>
    </div>
    
     <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Referencia" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
       <asp:TextBox  ID="txtReferencia" CssClass="input-xlarge" Width="300px" TextMode="MultiLine" Height="100px" runat="server"> </asp:TextBox>
        </div>
    </div>
    
   
  

    <asp:HiddenField ID="hdIdGrupoDistrito" runat="server" />

    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


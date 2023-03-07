<%@ Page
    Title="Alta de Feriados "
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="FeriadosForm.aspx.cs"
    Inherits="SGI.Operaciones.FeriadosForm" %>

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
           
            return confirm('¿Confirma la Transaccion?');
        }

    </script>


    <%--    <ej:Grid ID="Grid1" runat="server">
    </ej:Grid>--%>






    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Style="font-family: sans-serif; font-size: 14px;" ID="lblFecAsignacion" Text="Fecha de Feriado" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Style="font-family: sans-serif; font-size: 14px;" ID="lblFecInicio" Text="Descripcion" runat="server"></asp:Label>
        </div>
       
    </div>
    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFecha" runat="server"></asp:Calendar>
        </div>
        <div style="width: 500px; display: inline-block; vertical-align:top; margin-top: 5px;" >
            <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Rows="3" MaxLength="100"   runat="server" style=" vertical-align:top;"></asp:TextBox>
        </div>
        
    </div>









   

    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


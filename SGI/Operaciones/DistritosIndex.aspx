<%@ Page 
    Title="Administrar Distritos"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="DistritosIndex.aspx.cs" 
    Inherits="SGI.Operaciones.DistritosIndex" 
%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup> 

        <p class="lead mtop20">Indice Distritos</p>


     <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
        <asp:Button id="btnGruposDistritos" Width="200px" Enabled="true" runat="server" Text="Grupo Distritos" OnClick="btnGruposDistritos_Click" CssClass="btn btn-primary"/>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
        <asp:Button id="btnCatalogoDistritos" Width="200px" Enabled="true" runat="server" Text="Catalogo Distritos" OnClick="btnCatalogoDistritos_Click" CssClass="btn btn-primary"/>
        </div>
         <div style="width: 250px; display: inline-block; margin-top: 5px;">
        <asp:Button id="btnCatalogoDistritos_Zonas" Width="200px" Enabled="true" runat="server" Text="Zonas" OnClick="btnCatalogoDistritos_Zonas_Click" CssClass="btn btn-primary"/>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
        <asp:Button id="btnCatalogoDistritos_Subzonas" Width="200px" Enabled="true" runat="server" Text="Subzonas" OnClick="btnCatalogoDistritos_Subzonas_Click" CssClass="btn btn-primary"/>
        </div>
    </div>
  

    
      <hr />
     
     


</asp:Content>


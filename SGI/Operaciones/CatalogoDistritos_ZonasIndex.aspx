﻿<%@ Page 
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="CatalogoDistritos_ZonasIndex.aspx.cs" 
    Inherits="SGI.Operaciones.CatalogoDistritos_ZonasIndex" 
%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup> 
    <script type="text/javascript">
    
        function ConfirmaEliminar(obj) {
            return confirm('¿Confirma que desea Eliminar este CatalogoDistritos_Zonas ' + obj + '?');
        }
    </script>

   

        <hr />
        <p class="lead mtop20">Catalogo Distritos Zonas</p>


    
     <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Grupo-Districto" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlGrupoDistricto" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlGrupoDistricto_SelectedIndexChanged"
                DataTextField="Nombre" DataValueField="IdGrupoDistrito">
            </asp:DropDownList>

        </div>
    </div>

    <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-lddlGrupoDistrictoabel" Text="Catalogo-Districto" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlCatalogoDistritos" runat="server" AutoPostBack="true"   OnSelectedIndexChanged="ddlCatalogoDistritos_SelectedIndexChanged"
                DataTextField="Descripcion" DataValueField="IdDistrito">
            </asp:DropDownList>

        </div>
    </div>
     <div class="control-group">
        <asp:Button id="btnBuscar" Enabled="true" runat="server" Text="Buscar Catalogo-Distritos-Zona" OnClick="btnBuscar_Click" CssClass="btn btn-primary"/>
    </div>

        <asp:GridView id="gridView" 
            runat="server"
            Width="100%" 
            AutoGenerateColumns="false">
            <Columns>
                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("IdZona") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

               <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("IdZona") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("IdDistrito")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="IdZona" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelIdDistrito" runat="server" Text='<%# Bind("IdZona") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="IdDistrito">
                    <ItemTemplate>
                        <asp:Label ID="labelCodigo" runat="server" Text='<%# Bind("IdDistrito") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Codigo Zona">
                    <ItemTemplate>
                        <asp:Label ID="labelDescripcion" runat="server" Text='<%# Bind("CodigoZona") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
          
            </Columns>
        </asp:GridView>
      <hr />
     
      <div class="control-group">
        <asp:Button id="btnNuevo" Enabled="true" runat="server" Text="Nueva Catalogo-Distritos-Zona" OnClick="btnNuevo_Click" CssClass="btn btn-primary"/>
    </div>
       <asp:HiddenField  ID ="hdIdDistrito" runat="server"/>

</asp:Content>


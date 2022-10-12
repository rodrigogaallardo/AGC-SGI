<%@ Page 
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="CatalogoDistritosIndex.aspx.cs" 
    Inherits="SGI.Operaciones.CatalogoDistritosIndex" 
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
            return confirm('¿Confirma que desea Eliminar este Grupos-Distritos ' + obj + '?');
        }
    </script>

   

        <hr />
        <p class="lead mtop20">Catalogo Distritos</p>


     <div class="control-group">
        <div style="width: 150px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Grupo-Districto" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlGrupoDistricto" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlGrupoDistricto_SelectedIndexChanged"
                DataTextField="Nombre" DataValueField="IdGrupoDistricto">
            </asp:DropDownList>

        </div>
    </div>


        <asp:GridView id="gridView" 
            runat="server"
            Width="100%" 
            AutoGenerateColumns="false">
            <Columns>
                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("IdDistrito") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

               <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("IdDistrito") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("IdGrupoDistrito")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Id" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelIdDistrito" runat="server" Text='<%# Bind("IdDistrito") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Codigo">
                    <ItemTemplate>
                        <asp:Label ID="labelCodigo" runat="server" Text='<%# Bind("Codigo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Descripcion">
                    <ItemTemplate>
                        <asp:Label ID="labelDescripcion" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Creado">
                    <ItemTemplate>
                        <asp:Label ID="labeldefinicion" runat="server" Text='<%# Bind("CreateDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Orden">
                    <ItemTemplate>
                        <asp:Label ID="labelReferencia" runat="server" Text='<%# Bind("orden") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
      <hr />
     
      <div class="control-group">
        <asp:Button id="btnNuevo" Enabled="true" runat="server" Text="Nueva Catalogo-Distritos" OnClick="btnNuevo_Click" CssClass="btn btn-primary"/>
    </div>


</asp:Content>


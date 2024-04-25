<%@ Page 
    Title="Administrar Grupo Distritos"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="GruposDistritosIndex.aspx.cs" 
    Inherits="SGI.Operaciones.GruposDistritosIndex" 
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

   

      
        <p class="lead mtop20">Grupo Distrito</p>

        <asp:GridView id="gridView" 
            runat="server"
            Width="100%" 
            AutoGenerateColumns="false">
            <Columns>
                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("IdGrupoDistrito") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

               <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("IdGrupoDistrito") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("IdGrupoDistrito")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

              <%--  <asp:TemplateField HeaderText="Id" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelIdGrupoDistrito" runat="server" Text='<%# Bind("IdGrupoDistrito") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="Codigo">
                    <ItemTemplate>
                        <asp:Label ID="labelCodigo" runat="server" Text='<%# Bind("Codigo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <asp:Label ID="labelNombre" runat="server" Text='<%# Bind("Nombre") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Definicion">
                    <ItemTemplate>
                        <asp:Label ID="labeldefinicion" runat="server" Text='<%# Bind("definicion") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Referencia">
                    <ItemTemplate>
                        <asp:Label ID="labelReferencia" runat="server" Text='<%# Bind("referencia") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
      <hr />
     
      <div class="control-group">
        <asp:Button id="btnNuevo" Enabled="true" runat="server" Text="Nuevo Grupo Distrito" OnClick="btnNuevo_Click" CssClass="btn btn-primary"/>
        <asp:Button ID="btnReturn" runat="server" Text="Volver al Indice" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>

    <asp:HiddenField ID="hid_valor_boton" runat="server" />
    <asp:HiddenField ID="hid_observaciones" runat="server" />

    <%-- Modal Observacion Solicitante--%>
    <div id="frmEliminarLog" class="modal fade" style="max-width: 400px;">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:HiddenField ID="hid_id_object" runat="server"/>
                <div class="modal-header">
                    <h4 class="modal-title">Eliminar</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="control-label">Observaciones del Solicitante:</label>
                        <div class="controls">
                            <asp:TextBox ID="txtObservacionesSolicitante" runat="server" CssClass="form-control" Columns="10" Width="95%" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Botones --%>
                <div class="modal-footer" style="text-align: left;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>


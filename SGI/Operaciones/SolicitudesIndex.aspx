<%@ Page 
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SolicitudesIndex.aspx.cs" 
    Inherits="SGI.Operaciones.SolicitudesIndex" 
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
            return confirm('¿Confirma que desea Eliminar est tramite-tarea ' + obj + '?');
        }
    </script>

    <div class="control-group">
        <label class="control-label" for="txtBuscarSolicitud">Buscar por numero de solicitud</label>
        <div class="controls">
            <asp:TextBox id="txtBuscarSolicitud" runat="server" CssClass="controls"/>
        </div>
    </div>

    <div class="control-group">
        <asp:Button id="btnBuscarSolicitud" runat="server" Text="Buscar" OnClick="btnBuscarSolicitud_Click" CssClass="btn btn-primary"/>
    </div>

        <hr />
        <p class="lead mtop20">Tareas de la solicitud</p>

        <asp:GridView id="gridViewSSIT_Solicitudes" 
            runat="server"
            Width="100%" 
            OnRowDataBound="gridView_RowDataBound"
            AutoGenerateColumns="false" Visible="false">
            <Columns>
                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("id_solicitud") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

             <%--  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("id_solicitud") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("id_solicitud")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>--%>


                <asp:TemplateField HeaderText="idSolicitud" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelidSolicitud" runat="server" Text='<%# Bind("id_solicitud") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Tarea">
                    <ItemTemplate>
                        <asp:Label ID="labelCreateDate" runat="server"><%# Eval("CreateDate") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Circuito">
                    <ItemTemplate><%# Eval("CodigoSeguridad") %></ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Usuario Asignado">
                    <ItemTemplate>
                        <asp:Label ID="labelUsuarioAsignado" runat="server" Text='<%# Bind("idTAD") %>'></asp:Label>
                    </ItemTemplate>
                 
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:GridView id="gridViewTransf_Solicitudes" 
            runat="server"
            Width="100%" 
            OnRowDataBound="gridView_RowDataBound"
            AutoGenerateColumns="false" Visible="false">
             <Columns>
                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("id_solicitud") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

             <%--  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("id_solicitud") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("id_solicitud")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>--%>


                <asp:TemplateField HeaderText="idSolicitud" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelidSolicitud" runat="server" Text='<%# Bind("id_solicitud") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Tipo Estado" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lblTipoEstado" runat="server" Text='<%# Bind("id_estado") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Create Date">
                    <ItemTemplate>
                        <asp:Label ID="labelCreateDate" runat="server"><%# Eval("CreateDate") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Codigo Seguridad">
                    <ItemTemplate><%# Eval("CodigoSeguridad") %></ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="idTAD">
                    <ItemTemplate>
                        <asp:Label ID="labelUsuarioAsignado" runat="server" Text='<%# Bind("idTAD") %>'></asp:Label>
                    </ItemTemplate>
                 
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
      <hr />
    <asp:Label ID="lblMsj" runat="server"  Style="color: Black"></asp:Label>

     <asp:HiddenField  ID ="hdidSolicitud" runat="server"/>
     <asp:HiddenField  ID ="hdSSIT_TRANSF" runat="server"/>
    
    

</asp:Content>


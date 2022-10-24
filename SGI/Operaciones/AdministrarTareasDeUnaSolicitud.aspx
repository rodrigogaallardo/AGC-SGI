<%@ Page 
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="AdministrarTareasDeUnaSolicitud.aspx.cs" 
    Inherits="SGI.Operaciones.AdministrarTareasDeUnaSolicitud" 
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

        <asp:GridView id="gridView" 
            runat="server"
            Width="100%" 
            OnRowDataBound="gridView_RowDataBound"
            AutoGenerateColumns="false">
            <Columns>
                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("id_tramitetarea") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

               <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("id_tramitetarea") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("id_tramitetarea")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="IdTramiteTarea" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelIdTramiteTarea" runat="server" Text='<%# Bind("id_tramitetarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Tarea">
                    <ItemTemplate>
                        <asp:Label ID="labelIdTarea" runat="server"><%# Eval("ENG_Tareas.nombre_tarea") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Circuito" SortExpression="ENG_Tareas.ENG_Circuitos.nombre_grupo">
                    <ItemTemplate><%# Eval("ENG_Tareas.ENG_Circuitos.nombre_grupo") %></ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Usuario Asignado">
                    <ItemTemplate>
                        <asp:Label ID="labelUsuarioAsignado" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                    </ItemTemplate>
                  <%--  <EditItemTemplate>
                        <asp:DropDownList ID="ddlUsuarioAsignado_tramitetarea" runat="server"
                            DataSource="<%# CargarTodosLosUsuarios() %>" DataTextField="UserName" DataValueField="UserId"></asp:DropDownList>
                    </EditItemTemplate>--%>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Fecha Asignacion">
                    <ItemTemplate>
                        <asp:Label ID="labelFechaAsignacion" runat="server" Text='<%# Bind("FechaAsignacion_tramtietarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Fecha Inicio">
                    <ItemTemplate>
                        <asp:Label ID="labelFechaInicio" runat="server" Text='<%# Bind("FechaInicio_tramitetarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha Cierre">
                    <ItemTemplate>
                        <asp:Label ID="labelFechaCierre" runat="server" Text='<%# Bind("FechaCierre_tramitetarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

               <%--  <asp:TemplateField HeaderText="id_solicitud" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelid_solicitud" runat="server" Text='<%# Bind("id_solicitud") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>

            </Columns>
        </asp:GridView>
      <hr />
     <asp:HiddenField  ID ="hdidSolicitud" runat="server"/>
     <asp:HiddenField  ID ="hdHAB_TRANSF" runat="server"/>
    
      <div class="control-group">
        <asp:Button id="btnNuevo" Enabled="false" runat="server" Text="Nueva Tarea-Tramite" OnClick="btnNuevo_Click" CssClass="btn btn-primary"/>
    </div>


</asp:Content>


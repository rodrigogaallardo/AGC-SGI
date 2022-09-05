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

        <asp:GridView id="gridViewTareas" 
            runat="server"
            Width="100%" 
            OnRowEditing="gridViewTareas_RowEditing" 
            OnRowCancelingEdit="gridViewTareas_RowCancelingEdit" 
            OnRowUpdating="gridViewTareas_RowUpdating" 
            AutoGenerateColumns="false">
            <Columns>
                <asp:CommandField ShowEditButton="true" />

                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="labelIdTramiteTarea" runat="server" Text='<%# Bind("id_tramitetarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Tarea">
                    <ItemTemplate>
                        <asp:Label ID="labelIdTarea" runat="server"><%# Eval("ENG_Tareas.nombre_tarea") %></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList id="dropDownListEditTarea" runat="server"
                            DataSource="<%# CargarTodasLasTareas() %>" DataTextField="DescripcionTareaCircuito" DataValueField="id_tarea" Width="100%"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Circuito" SortExpression="ENG_Tareas.ENG_Circuitos.nombre_grupo">
                    <ItemTemplate><%# Eval("ENG_Tareas.ENG_Circuitos.nombre_grupo") %></ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="labelUsuarioAsignado" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="dropDownListEditUsuario" runat="server"
                            DataSource="<%# CargarTodosLosUsuarios() %>" DataTextField="UserName" DataValueField="UserId"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="labelFechaAsignacion" runat="server" Text='<%# Bind("FechaAsignacion_tramtietarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="labelFechaCierre" runat="server" Text='<%# Bind("FechaCierre_tramitetarea") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Calendar ID="calendarEditFechaCierre" runat="server"></asp:Calendar>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
</asp:Content>


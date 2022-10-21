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
                        <asp:Button ID="btnEdit" runat="server" Text="Editar"  ToolTip ='<%# Eval("tipo") %>' OnClick="btnEdit_Click"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

             <%--  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("id_solicitud") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("id_solicitud")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                 <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <asp:Label  ID="lblTipoEstado" runat="server" Text='<%# Bind("tipo") %>'> ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="idTarea" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelidSolicitud" runat="server" Text='<%# Bind("id_solicitud") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Tipo Tramite">
                    <ItemTemplate>
                         <asp:Label ID="labelTipoTramite" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Tipo Expediente">
                    <ItemTemplate><%# Eval("descripcion_tipoexpediente") %></ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Sub Tipo Expediente">
                    <ItemTemplate><%# Eval("descripcion_subtipoexpediente") %></ItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate><%# Eval("estado") %></ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Fec.Creacion">
                    <ItemTemplate>
                        <asp:Label  runat="server"><%# Eval("CreateDate") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Usuario">
                    <ItemTemplate>
                        <asp:Label  runat="server"><%# Eval("CreateUser") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="C.Seguridad">
                    <ItemTemplate><%# Eval("CodigoSeguridad") %></ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Fec.Librado">
                    <ItemTemplate><%# Eval("FechaLibrado") %></ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField  Visible="false">
                    <ItemTemplate>
                         <asp:Label ID="labelid_estado" Text='<%# Bind("id_estado") %>' Visible="false" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
              
            </Columns>
        </asp:GridView>

     
      <hr />
    <asp:Label ID="lblMsj" runat="server"  Style="color: Black"></asp:Label>

     <asp:HiddenField  ID ="hdidSolicitud" runat="server"/>
     <asp:HiddenField  ID ="hdSSIT_TRANSF" runat="server"/>
    
    

</asp:Content>


<%@ Page 
    Title="Administrar Feriados"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="FeriadosIndex.aspx.cs" 
    Inherits="SGI.Operaciones.FeriadosIndex" 
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
            return confirm('¿Confirma que desea Eliminar el Feriado del  ' + obj + '?');
        }
    </script>

    <div class="control-group">
        <label class="control-label" for="txtBuscarSolicitud">Buscar Rango de Fechas</label>
    

         <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Style="font-family: sans-serif; font-size: 14px;" ID="lblFecAsignacion" Text="Fec.Desde" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Style="font-family: sans-serif; font-size: 14px;" ID="lblFecInicio" Text="Fec.Hasta" runat="server"></asp:Label>
        </div>
       
    </div>


    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaDesde"  runat="server"></asp:Calendar>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaHasta"  runat="server"></asp:Calendar>
        </div>
    </div>
    </div>

    <div class="control-group">
        <asp:Button id="btnBuscarSolicitud" runat="server" Text="Buscar" OnClick="btnBuscarSolicitud_Click" CssClass="btn btn-primary"/>
    </div>

        <hr />
        <p class="lead mtop20">Feriados</p>

        <asp:GridView id="gridView" 
            runat="server"
            Width="100%" 
            OnRowDataBound="gridView_RowDataBound"
            AutoGenerateColumns="false">
            <Columns>
                  

               <asp:TemplateField>
                    <ItemTemplate>
                       <asp:Button ID="btnRemove" runat="server" Text="Eliminar"  ToolTip='<%# Eval("idFeriado") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("idFeriado")) %>'></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="IdTramiteTarea" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="labelIdTramiteTarea" runat="server" Text='<%# Bind("idFeriado") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Fecha Feriado">
                    <ItemTemplate>
                        <asp:Label ID="labelFecha" runat="server" Text='<%# Bind("Fecha") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Descripcion">
                    <ItemTemplate>
                        <asp:Label ID="labelDescripcion" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Usuario Creador">
                    <ItemTemplate>
                        <asp:Label ID="labelUsuarioAsignado" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                    </ItemTemplate>
                
                </asp:TemplateField>

              
                <asp:TemplateField HeaderText="Fecha Creacion">
                    <ItemTemplate>
                        <asp:Label ID="labelFechaCierre" runat="server" Text='<%# Bind("CreateDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

             

            </Columns>
        </asp:GridView>
      <hr />
     <asp:HiddenField  ID ="hdidSolicitud" runat="server"/>
     <asp:HiddenField  ID ="hdHAB_TRANSF" runat="server"/>
    
      <div class="control-group">
        <asp:Button id="btnNuevo" Enabled="true" runat="server" Text="Nuevo Feriado" OnClick="btnNuevo_Click" CssClass="btn btn-primary"/>
    </div>


</asp:Content>


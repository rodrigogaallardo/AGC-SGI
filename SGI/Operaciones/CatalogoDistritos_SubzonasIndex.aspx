<%@ Page
    Title="Administrar SubZonas"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="CatalogoDistritos_SubzonasIndex.aspx.cs"
    Inherits="SGI.Operaciones.CatalogoDistritos_SubzonasIndex" %>

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
            return confirm('¿Confirma que desea Eliminar este CatalogoDistritos_Subzonas ' + obj + '?');
        }
    </script>



 
    <p class="lead mtop20">Sub Zonas</p>



    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Grupo Distrito" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlGrupoDistricto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupoDistricto_SelectedIndexChanged"
                DataTextField="Nombre" DataValueField="IdGrupoDistrito">
            </asp:DropDownList>

        </div>
    </div>

    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-lddlGrupoDistrictoabel" Text="Distrito" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlCatalogoDistritos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCatalogoDistritos_SelectedIndexChanged"
                DataTextField="Descripcion" DataValueField="IdDistrito">
            </asp:DropDownList>

        </div>
    </div>

    <div class="control-group">
        <div style="width: 80px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-lddlGrupoDistrictoabel" Text="Zonas" runat="server"></asp:Label>
        </div>
        <div style="width: 500px; display: inline-block; margin-top: 5px;">
            <asp:DropDownList ID="ddlCatalogoDistritos_Zonas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCatalogoDistritos_Zonas_SelectedIndexChanged"
                DataTextField="Descripcion" DataValueField="IdDistrito">
            </asp:DropDownList>

        </div>
    </div>


    <div class="control-group">
        <asp:Button ID="btnBuscar" Enabled="true" runat="server" Text="Buscar SubZona" OnClick="btnBuscar_Click" CssClass="btn btn-primary" />
    </div>

    <asp:GridView ID="gridView"
        runat="server"
        Width="50%"
        AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" Text="Editar" ToolTip='<%# Eval("IdSubZona") %>' OnClick="btnEdit_Click"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnRemove" runat="server" Text="Eliminar" ToolTip='<%# Eval("IdSubZona") %>' OnClick="btnRemove_Click" OnClientClick='<%# string.Format("return ConfirmaEliminar({0});", Eval("IdZona")) %>'></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>

   <%--         <asp:TemplateField HeaderText="IdZona">
                <ItemTemplate>
                    <asp:Label ID="labelIdZona" runat="server" Text='<%# Bind("IdZona") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>--%>

            <asp:TemplateField HeaderText="IdSubZona" Visible="true">
                <ItemTemplate>
                    <asp:Label ID="labelIdSubZona" runat="server" Text='<%# Bind("IdSubZona") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Codigo Sub Zona">
                <ItemTemplate>
                    <asp:Label ID="labelCodigoSubZona" runat="server" Text='<%# Bind("CodigoSubZona") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    <hr />

    <div class="control-group">
        <asp:Button ID="btnNuevo" Enabled="false" runat="server" Text="Nueva Subzonas" OnClick="btnNuevo_Click" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver al Indice" OnClick="btnReturn_Click" CssClass="btn btn-primary" />
    </div>
    <asp:HiddenField ID="hdIdZona" runat="server" />
    <asp:HiddenField ID="hdIdDistrito" runat="server" />
    <asp:HiddenField ID="hdIdGrupoDistrito" runat="server" />
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


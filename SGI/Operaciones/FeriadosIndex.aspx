<%@ Page
    Title="Administrar Feriados"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="FeriadosIndex.aspx.cs"
    Inherits="SGI.Operaciones.FeriadosIndex" %>

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
            console.log(obj.toString());
            return confirm('¿Confirma que desea Eliminar el Feriado del  ' + obj.toString() + '?');
        }
    </script>

    <div class="control-group">

        <div class="control-group">
            <div style="width: 250px; display: inline-block; margin-top: 5px;">
                <asp:Label class="control-label" Style="font-family: sans-serif; font-size: 14px;" ID="lblFecAsignacion" Text="Fecha Desde" runat="server"></asp:Label>
            </div>
            <div style="width: 250px; display: inline-block; margin-top: 5px;">
                <asp:Label class="control-label" Style="font-family: sans-serif; font-size: 14px;" ID="lblFecInicio" Text="Fecha Hasta" runat="server"></asp:Label>
            </div>

        </div>


        <div class="control-group">
            <div style="width: 250px; display: inline-block; margin-top: 5px;">
                <asp:Calendar ID="calFechaDesde" runat="server"></asp:Calendar>
            </div>
            <div style="width: 250px; display: inline-block; margin-top: 5px;">
                <asp:Calendar ID="calFechaHasta" runat="server"></asp:Calendar>
            </div>
        </div>
    </div>

    <div class="control-group">
        <asp:Button ID="btnBuscarSolicitud" runat="server" Text="Buscar" OnClick="btnBuscarSolicitud_Click" CssClass="btn btn-primary" />
    </div>

    <hr />
    <p class="lead mtop20">Listado de Feriados</p>

    <asp:GridView ID="gridView"
        runat="server"
        Width="100%"
        OnRowDataBound="gridView_RowDataBound"
        AutoGenerateColumns="false">
        <Columns>


            <asp:TemplateField ItemStyle-Width="90px">
                <ItemTemplate>
                    <asp:Button ID="btnRemove" runat="server" Text="Eliminar" ToolTip='<%# Eval("idFeriado") %>' 
                        OnClick="btnRemove_Click"
                      
                        ></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Id" Visible="true">
                <ItemTemplate>
                    <asp:Label ID="labelIdTramiteTarea" runat="server" Text='<%# Bind("idFeriado") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

                  <asp:BoundField DataField="Fecha"  HeaderText="Fecha Feriado" DataFormatString="{0:d}"
                            ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />


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

        <EmptyDataTemplate>
            <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                <p>
                    No se encontraron trámites con los filtros ingresados.
                </p>
            </asp:Panel>
        </EmptyDataTemplate>

    </asp:GridView>
    <hr />
    <asp:HiddenField ID="hdidSolicitud" runat="server" />
    <asp:HiddenField ID="hdHAB_TRANSF" runat="server" />

    <div class="control-group">
        <asp:Button ID="btnNuevo" Enabled="true" runat="server" Text="Nuevo Feriado" OnClick="btnNuevo_Click" CssClass="btn btn-primary" />
    </div>

    <asp:HiddenField ID="hid_valor_boton" runat="server" />
    <asp:HiddenField ID="hid_observaciones" runat="server" />

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


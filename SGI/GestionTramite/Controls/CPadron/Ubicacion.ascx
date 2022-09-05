<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ubicacion.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Ubicacion" %>

<%--Panel de Ubicacion--%>
<asp:HiddenField ID="hid_id_cpadron" runat="server" />

<asp:Panel ID="pnlUbicacion" runat="server" Width="900px">
    <div style="padding: 0px 10px 10px 10px; width: auto">

        <asp:GridView ID="gridubicacion_db" runat="server" AutoGenerateColumns="false" DataKeyNames="id_cpadronubicacion"
            OnRowDataBound="gridubicacion_db_OnRowDataBound" Style="border: none;" GridLines="None" ShowHeader="false"
            AllowPaging="false" Width="100%">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <table style="width: 900px; border-collapse: separate; border-spacing: 5px;">
                            <tr>
                                <td colspan="2">
                                    <strong>Datos de la Ubicaci&oacute;n</strong>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px">
                                    <asp:Image ID="imgFotoParcela_db" runat="server" class="img-polaroid" Style="outline: solid 2px #939393"
                                        onError="noExisteFotoParcela(this);" />
                                </td>
                                
        
                                <td style="width: 700px; padding-left: 7px; vertical-align: text-top">
                                     <asp:Panel ID="pnlbtnEliminar" runat="server" CssClass="text-center">
                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="pull-right btn btn-default mright20" title="Eliminar Ubicación" data-group="controles-accion"
                                            OnClick="btnEliminar_Click" CommandArgument='<%# Eval("id_cpadronubicacion") %>'>
                                            <i class="imoon imoon-close"></i>
                                        </asp:LinkButton>
                                    </asp:Panel>
                                     <asp:Panel ID="Panel1" runat="server" CssClass="text-center">
                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="pull-right btn btn-default" title="Editar Ubicación" data-group="controles-accion"
                                            OnClick="btnEditar_Click" CommandArgument='<%# Eval("id_cpadronubicacion") %>'>
                                            <i class="imoon imoon-pencil"></i>
                                        </asp:LinkButton>
                                    </asp:Panel>
     
                                    <asp:Panel ID="pnlSMPview" runat="server" Style="padding-top: 3px">
                                        <div>
                                            <b>Sección:</b>
                                            <asp:Label ID="grd_seccion_db" runat="server"></asp:Label>
                                            <b>Manzana:</b>
                                            <asp:Label ID="grd_manzana_db" runat="server"></asp:Label>
                                            <b>Parcela:</b>
                                            <asp:Label ID="grd_parcela_db" runat="server"></asp:Label>
                                        </div>
                                        <div style="padding-top: 3px">
                                            <b>Partida Matriz Nº:</b>
                                            <asp:Label ID="grd_NroPartidaMatriz_db" runat="server"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlTipoUbicacionview" runat="server" Style="padding-top: 3px" Visible="false">
                                        <div>
                                            <strong>Tipo de Ubicaci&oacute;n:</strong>
                                            <asp:Label ID="lblTipoUbicacionview" runat="server"></asp:Label>
                                        </div>
                                        <div>
                                            <strong>Subtipo de Ubicaci&oacute;n:</strong>
                                            <asp:Label ID="lblSubTipoUbicacionview" runat="server"></asp:Label>
                                        </div>
                                        <div>
                                            <strong>Local:</strong>
                                            <asp:Label ID="lblLocalview" runat="server"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPartidasHorizontalesview" runat="server">
                                        <div style="padding-top: 3px" class="titulo-4">
                                            <strong>Partida/s Horizontal/es:</strong>
                                        </div>
                                        <asp:DataList ID="dtlPartidaHorizontales_db" runat="server" RepeatDirection="Horizontal"
                                            RepeatColumns="3" CellSpacing="10">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartidahorizontal_db" runat="server" Text='<% #Bind("DescripcionCompleta") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <asp:Label ID="lblEmptyDataPartidasHorizontales_db" runat="server" Text="No posee"
                                            Visible="false"></asp:Label>
                                    </asp:Panel>
                                    <div style="padding-top: 10px">
                                        <div>
                                            <strong>Zonificación de la parcela</strong>
                                        </div>
                                        <asp:Label ID="grd_zonificacion_db" runat="server" Style="width: 100%"></asp:Label>
                                    </div>
                                    <asp:Panel ID="pnlPuertasview" runat="server">
                                        <div style="padding-top: 10px">
                                            <strong>Puertas</strong>
                                        </div>
                                        <asp:DataList ID="dtlPuertas_db" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
                                            CellSpacing="10">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPuertas_db" runat="server" Text='<% #Bind("Calle") %>'></asp:Label>
                                                <asp:Label ID="lnkNroPuerta_db" runat="server" Text='<% #Bind("NroPuerta") %>' CssClass="pleft5"></asp:Label>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDeptoLocalview" runat="server">
                                        <div style="padding-top: 10px">
                                            <asp:Label ID="lblDeptoLocalvis1" runat="server" Text="Depto/Local:" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lblDeptoLocalvis2" runat="server"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>


                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="mtop10">

                    <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' class="img-polaroid" />
                    <span class="mleft10">No se encontraron registros.</span>

                </div>
            </EmptyDataTemplate>
        </asp:GridView>

        <hr />
        <asp:Panel ID="pnlZona" runat="server" CssClass="back-panel-gris" Style="margin-top: 10px; padding-left: 10px">
            <strong>Zona Declarada:</strong>
            <asp:Label ID="lblZona" runat="server"></asp:Label>
        </asp:Panel>
        <%--Plantas a habilitar--%>
        <asp:Panel ID="pnlPlantasHabilitar" runat="server" CssClass="back-panel-gris" Style="margin-top: 20px; padding-left: 10px">
            <strong>Plantas a habilitar:</strong>
            <asp:Label ID="lblPlantasHabilitar" runat="server"></asp:Label>
        </asp:Panel>
    </div>
</asp:Panel>

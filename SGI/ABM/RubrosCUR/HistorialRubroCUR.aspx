<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HistorialRubroCUR.aspx.cs" Inherits="SGI.ABM.RubrosCUR.HistoriaRubroCUR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
            function showResultado() {
            $("#box_resultado").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
        }

    </script>
    <!-- Grilla -->
    <div id="box_resultado" class="widget-box">
        <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server">
            <ContentTemplate>

                <div style="margin-left: 10px; margin-right: 10px; overflow-x: scroll;">
                    <asp:Panel ID="pnlResultadoBuscar" runat="server">
                        <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false" Style="display: inline-block">
                            <div style="display: inline-block">
                                <h5>Detalle de Historial</h5>
                            </div>
                            <div style="display: inline-block">
                                (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>)
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="grdHistorial" runat="server" AutoGenerateColumns="false"
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            DataKeyNames="IdRubrosCN_historial"
                            SelectMethod="GetHistorial" ItemType="SGI.Model.clsItemHistorialRubroCur"
                            AllowPaging="true" AllowSorting="true" PageSize="30"
                            OnPageIndexChanging="grdHistorial_PageIndexChanging">
                            <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                            <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                            <Columns>
                                <asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operacion" />
                                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operacion" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Codigo" HeaderText="Codigo Rubro" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre Rubro" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="VigenciaHasta_rubro" HeaderText="Vigencia Hasta Rubro" ItemStyle-CssClass="align-center" />                                
                                <asp:BoundField DataField="TipoActividad" HeaderText="Actividad" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="TipoExpediente" HeaderText="Tipo Tramite" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="GrupoCircuito" HeaderText="Grupo Circuito" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="LibrarUso" HeaderText="Librado al Uso" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="CondicionExpress" HeaderText="Condicion Express" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="ZonaMixtura1" HeaderText="ZonaMixtura 1" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="ZonaMixtura2" HeaderText="ZonaMixtura 2" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="ZonaMixtura3" HeaderText="ZonaMixtura 3" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="ZonaMixtura4" HeaderText="ZonaMixtura 4" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Estacionamiento" HeaderText="Estacionamiento" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Bicicleta" HeaderText="Bicicleta" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="CyD" HeaderText="CyD" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Estacionamiento" HeaderText="Estacionamiento" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="LastUpdateDate" HeaderText="Fecha Ultima Actualización" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="Asistentes350" HeaderText="Asistentes 350" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="SinBanioPCD" HeaderText="Sin Baño PCD" ItemStyle-CssClass="align-center" />
                                <asp:BoundField DataField="LastUpdateUser" HeaderText="Usuario" ItemStyle-CssClass="align-center" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                    <p>
                                        No se encontraron trámites con los filtros ingresados.
                                    </p>
                                </asp:Panel>
                            </EmptyDataTemplate>
                            <PagerTemplate>
                                <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                    <div style="display: inline-table">
                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlResultadoBuscar" runat="server"
                                            DisplayAfter="0">
                                            <ProgressTemplate>
                                                <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <asp:LinkButton ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click" CssClass="btn" />
                                    <asp:LinkButton ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click" CssClass="btn" />
                                </asp:Panel>
                            </PagerTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Fin Grilla -->

</asp:Content>

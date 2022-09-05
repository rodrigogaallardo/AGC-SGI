<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPagos.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucPagos" %>

<asp:UpdatePanel ID="updBoxPagos" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_tipo_tramite" runat="server" />        
        <asp:HiddenField ID="hid_id_pago" runat="server" Value="0" />
        <asp:HiddenField ID="hid_estado_pago" runat="server" Value="" />
        <asp:Panel ID="pnlPagos" runat="server" CssClass="accordion-group widget-box" >

            <%-- titulo collapsible pagos--%>
            <div class="accordion-heading">
                <a id="A2" data-parent="#collapse-group" href="#collapse_pagos"
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                    <div class="widget-title">
                        <span class="icon"><i class="icon imoon-dollar"></i></span>
                        <h5>
                            <asp:Label ID="Label2" runat="server" Text="Pagos"></asp:Label></h5>
                        <span class="btn-right"><i class="icon icon-chevron-up"></i></span>
                    </div>
                </a>
            </div>

            <%-- contenido del collapsible pagos --%>
            <div class="accordion-body collapse in" id="collapse_pagos">
                <div class="widget-content">
                    <strong>Boletas generadas para AGC</strong>
                     <asp:GridView ID="grdPagosGeneradosBUI_AGC" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="id_sol_pago,id_solicitud,id_pago" CssClass="table table-bordered mtop5"
                        AllowPaging="false" Style="border: none;" ItemType="clsItemGrillaPagos"
                        GridLines="None" Width="100%" OnRowDataBound="grdPagosGeneradosBUI_AGC_RowDataBound">
                        <HeaderStyle CssClass="grid-header" />                
                        <Columns>
                            <asp:BoundField DataField="CreateDate" HeaderText="Fecha" ItemStyle-Width="80px"
                                HeaderStyle-HorizontalAlign="Left" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="id_pago" HeaderText="Nº identificación" ItemStyle-Width="200px" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="monto_pago" HeaderText="Monto Total" DataFormatString="{0:C}"  ItemStyle-CssClass="text-right"
                                ItemStyle-Width="200px" />
                            <asp:BoundField DataField="estado" HeaderText="Estado" ItemStyle-Width="200px" ItemStyle-CssClass="text-center" />

                            <asp:TemplateField ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkImprimirBoletaUnica" runat="server" 
                                        Text="Imprimir Boleta" Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="mtop10">
                                <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                <span class="mleft10">No existen boletas generadas.</span>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>

                    <strong>Boletas generadas para APRA</strong>
                    <asp:GridView ID="grdPagosGeneradosBUI_APRA" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="id_sol_pago,id_solicitud,id_pago" CssClass="table table-bordered mtop5"
                        AllowPaging="false" Style="border: none;" ItemType="clsItemGrillaPagos"
                        GridLines="None" Width="100%" OnRowDataBound="grdPagosGeneradosBUI_APRA_RowDataBound">
                        <HeaderStyle CssClass="grid-header" />                
                        <Columns>
                            <asp:BoundField DataField="CreateDate" HeaderText="Fecha" ItemStyle-Width="80px"
                                HeaderStyle-HorizontalAlign="Left" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="id_pago" HeaderText="Nº identificación" ItemStyle-Width="200px" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="monto_pago" HeaderText="Monto Total" DataFormatString="{0:C}"  ItemStyle-CssClass="text-right"
                                ItemStyle-Width="200px" />

                            <asp:BoundField DataField="estado" HeaderText="Estado" DataFormatString="{0:C}"  ItemStyle-CssClass="text-center"
                                ItemStyle-Width="200px" />

                            <asp:TemplateField ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkImprimirBoletaUnica" runat="server" 
                                        Text="Imprimir Boleta" Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="mtop10">
                                <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                <span class="mleft10">No existen boletas generadas.</span>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>

                </div>
            </div>
        </asp:Panel>
                        
    </ContentTemplate>
</asp:UpdatePanel>


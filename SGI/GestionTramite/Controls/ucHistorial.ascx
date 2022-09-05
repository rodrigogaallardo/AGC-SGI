<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHistorial.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucHistorial" %>

<%: Scripts.Render("~/bundles/autoNumeric") %>

<div id="page_content">

    <%--Box--%>
    <div id="box_datoslocal" class="accordion-group widget-box mtop20">

        <%-- titulo --%>
        <div class="accordion-heading">
            <a id="ubicacion_btnUpDown" data-parent="#collapse-group">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-file"></i></span>
                    <h5>Historial de Estado</h5>
                </div>
            </a>
        </div>
        <%-- contenido --%>
        <div id="box_Historial" class="accordion-body collapse in" runat="server">
            <div class="widget-content">
                <asp:GridView ID="grdHistorial" runat="server" AutoGenerateColumns="false"
                    AllowPaging="false" CssClass="table table-bordered mtop5"
                    GridLines="None" Width="100% ">
                    <HeaderStyle CssClass="grid-header" />
                    <RowStyle CssClass="grid-row" />
                    <AlternatingRowStyle BackColor="#efefef" />
                    <Columns>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="estado" HeaderText="Estado del Tramite" HeaderStyle-HorizontalAlign="Left" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="mtop10">
                            <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                            <span class="mleft10">No se encontraron registros.</span>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

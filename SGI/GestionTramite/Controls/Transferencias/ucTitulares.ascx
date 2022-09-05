<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTitulares.ascx.cs" Inherits="SGI.GestionTramite.Controls.Transferencias.ucTitulares" %>
<style type="text/css">

    .center
    {
        text-align:center !important;
    }
</style>

<div class="widget-box">
    <div class="widget-title">
        <span class="icon"><i class="icon-list-alt"></i></span>
        <h5>Lista de Titulares</h5>
    </div>
    <div class="widget-content">
        <div>
            <div style="width: 100%;">

                <asp:UpdatePanel ID="updGrillaTitulares" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <%--Grilla de Titulares--%>
                        <div>
                            <strong>Titulares</strong>
                        </div>
                        <div>
                            <asp:GridView ID="grdTitulares" runat="server" AutoGenerateColumns="false" DataKeyNames="id_persona"
                                AllowPaging="false" GridLines="None" Width="100%" 
                                CssClass="table table-bordered table-striped table-hover with-check"
                                CellPadding="3">
                                <HeaderStyle CssClass="grid-header" />
                                <AlternatingRowStyle BackColor="#efefef" />
                                <Columns>

                                    <asp:BoundField DataField="TipoPersonaDesc" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ApellidoNomRazon" HeaderText="Apellido y Nombre / Razon Social"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Cuit" HeaderText="CUIT" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" HeaderStyle-HorizontalAlign="Left" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="mtop10">

                                        <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                        <span class="mleft10">No se encontraron registros.</span>

                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>

                        <%--Grilla de Firmantes--%>
                        <div>
                            <strong>Firmantes</strong>
                        </div>
                        <div>
                            <asp:GridView ID="grdFirmantes" runat="server" AutoGenerateColumns="false" DataKeyNames="id_firmante"
                                AllowPaging="false" GridLines="None" Width="100%" 
                                CssClass="table table-bordered table-striped table-hover with-check"
                                CellPadding="3">
                                <HeaderStyle CssClass="grid-header" />
                                <AlternatingRowStyle BackColor="#efefef" />
                                <Columns>
                                    <asp:BoundField DataField="FirmanteDe" HeaderText="Firmante de..." HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ApellidoNombres" HeaderText="Apellido y Nombre/s" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="TipoDoc" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="NroDoc" HeaderText="Documento" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CaracterLegal" HeaderText="Carácter Legal" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="cargo_firmante_pj" HeaderText="Cargo" HeaderStyle-HorizontalAlign="Left" />

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="mtop10">

                                        <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                        <span class="mleft10">No se encontraron registros.</span>

                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>

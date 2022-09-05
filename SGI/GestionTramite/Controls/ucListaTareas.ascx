<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListaTareas.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucListaTareas" %>

<style type="text/css">

    .center
    {
        text-align:center !important;
    }
</style>

<div class="widget-box">
    <div class="widget-title">
        <span class="icon"><i class="icon-list-alt"></i></span>
        <h5>Lista de Tareas</h5>
    </div>
    <div class="widget-content">
        <div>
            <div style="width: 100%;">

                <asp:UpdatePanel ID="updTareas" runat="server">
                    <ContentTemplate>


                        <asp:GridView ID="grdTareas" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover"
                            ItemType="SGI.GestionTramite.Controls.clsRowitemTarea" SelectMethod="GetTareas" Width="800px"
                            OnRowDataBound="grdTareas_RowDataBound"
                            DataKeyNames="id_tramitetarea,UsuarioAsignado,FechaFinalizacion,form_aspx,id_tarea">
                            <Columns>
                        
                                <asp:TemplateField HeaderText="Descripción" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        
                                        <asp:Label ID="lblTramiteTarea" runat="server" Text="<%# Item.Descripcion %>" Visible="false">
                                        </asp:Label>

                                        <asp:HyperLink ID="lnkTramiteTarea" runat="server" 
                                            Text="<%# Item.Descripcion %>" Visible="false">
                                        </asp:HyperLink>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="codigo_circuito" HeaderText="Código Circuito" ItemStyle-CssClass="align-center" />

<%--                                <asp:HyperLinkField DataNavigateUrlFormatString="~/GestionTramite/Tareas/{0}?id={1}" 
                                    DataNavigateUrlFields="form_aspx,id_tramitetarea" 
                                    DataTextField="Descripcion" HeaderText="Descripción" />--%>

                                <asp:BoundField DataField="FechaCreacion" DataFormatString="{0:d}" HeaderText="Fecha Creación" ItemStyle-CssClass="align-center" />
                                
                                <asp:TemplateField HeaderText="Fecha Asignación" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFechaAsignada" runat="server" Text='<%# (Item.FechaAsignacion.HasValue ? Item.FechaAsignacion.Value.ToString("dd/MM/yyyy"): "")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="FechaFinalizacion" DataFormatString="{0:d}" HeaderText="Fecha Finalización" ItemStyle-CssClass="align-center" />

                                <asp:TemplateField HeaderText="Usuario" ItemStyle-CssClass="align-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsuario" runat="server" Text="<%# Item.ApenomUsuario %>"></asp:Label>

                                        <asp:LinkButton ID="lnkTomarTarea" runat="server" title="Haga click aquí para asignarse la tarea" CommandArgument="<%# Item.id_tramitetarea %>"
                                            Text="Tomar" OnClick="lnkTomarTarea_Click" Visible="false" ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Username" HeaderText="Username" ItemStyle-CssClass="align-center" />

                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>

                </asp:UpdatePanel>


                <div class="alert alert-info" style="width:500px">
                    <strong>Nota: </strong>Para visualizar o editar la tarea, haga click sobre el nombre de la misma.
                </div>
            </div>
        </div>
    </div>
</div>

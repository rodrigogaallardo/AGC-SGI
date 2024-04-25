<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListaCalle.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucListaCalle" %>

<asp:HiddenField ID="hid_lNot_collapse" runat="server" Value="false"/>


<div class="accordion-group widget-box">
    <div class="accordion-heading">
        <a id="btnUpDownNot" data-parent="#collapse-group" href="#collapseCalle" 
            data-toggle="collapse" onclick="btnUpDownNot_click(this)">

            <div class="widget-title">
                <span class="icon"><i class="icon-th-list"></i></span>
                <h5>Calles</h5>
                <span class="btn-right"><i class="icon-chevron-up"></i></span>        
            </div>
        </a>
    </div>
    <div class="accordion-body collapse in" id="collapseCalle">
        <div class="widget-content">
            <asp:UpdatePanel ID="updPnlCalles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdCodCalle" runat="server" />
                    <asp:HiddenField ID="hdIdCalle" runat="server" />
                    <asp:GridView   ID="grdBuscarCalles" 
                                    runat="server"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    GridLines="None"
                                    CssClass="table table-bordered table-striped table-hover with-check"
                                    DataKeyNames="Calle_Id" ItemType="SGI.Model.clsItemGrillaBuscarCalles"
                                    OnDataBound="grd_DataBound"
                                    OnRowDataBound="gridCalles_RowDataBound">
                    <Columns>
                        <asp:BoundField Visible="false" DataField="Calle_Id" HeaderText="ID" />
                        <asp:BoundField Visible="true" DataField="Calle_Cod" HeaderText="Código" ItemStyle-Width="50px"/>
                        <asp:BoundField Visible="true" DataField="Calle_Nombre" HeaderText="Calle" ItemStyle-Width="100px" />
                        <asp:BoundField Visible="true" DataField="Calle_AlturaIzquierdaInicio" HeaderText="Altura Izquierda Inicio" ItemStyle-Width="50px" />
                        <asp:BoundField Visible="true" DataField="Calle_AlturaIzquierdaFin" HeaderText="Altura Izquierda Fin" ItemStyle-Width="50px" />
                        <asp:BoundField Visible="true" DataField="Calle_AlturaDerechaInicio" HeaderText="Altura Derecha Incio" ItemStyle-Width="50px" />
                        <asp:BoundField Visible="true" DataField="Calle_AlturaDerechaFin" HeaderText="Altura Derecha Fin" ItemStyle-Width="50px" />
                        <asp:BoundField Visible="true" DataField="Calle_Tipo" HeaderText="Tipo de Calle" ItemStyle-Width="50px" />
                        <asp:TemplateField ItemStyle-Width="15px" HeaderText="Editar" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditar"
                                    runat="server" ToolTip="Editar"
                                    CssClass="link-local"
                                    CommandArgument='<%#Eval ("Calle_Cod") %>'
                                    CommandName='<%#Eval ("Calle_Id")%>'
                                    OnClick="lnkEditar_Click">
                                    <i class="icon icon-pencil" style="transform: scale(1.1);"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="15px" HeaderText="Eliminar" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton 
                                    ID="lnkEliminar"
                                    runat="server" ToolTip="Eliminar"
                                    CssClass="link-local"
                                    CommandArgument='<%#Eval ("Calle_Cod")%>'
                                    CommandName='<%#Eval ("Calle_Id")%>'
                                    OnClientClick="javascript:return tda_confirm_del();"
                                    OnClick="lnkEliminar_Click"
                                    PageSize="10"
                                    >
                                <i class="icon-trash" style="transform: scale(1.1);"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                    </Columns> 
                        <PagerTemplate>
                                    <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                        <div style="display:inline-table">
                                            <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlCalles" runat="server"
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</div>

<div id="frmEliminarLog" class="modal fade" style="max-width: 400px;">
    <div class="modal-dialog">
        <div class="modal-content">
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

<script type="text/javascript">

    function btnUpDownNot_click(obj) {
        var href_collapse = $(obj).attr("href");
        if ($(href_collapse).attr("id") != undefined) {
            if ($(href_collapse).css("height") == "0px") {
                $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
            }
            else {
                $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
            }
        }
    }

    function tda_confirm_del() {
        return confirm('¿Esta seguro que desea eliminar este Registro?');
    }

    function showfrmError() {
        $("#frmError").modal("show");
        return false;
    }
</script>
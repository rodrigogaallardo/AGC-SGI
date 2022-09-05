<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListaRubros.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucListaRubros" %>

<div class="accordion-group widget-box">
    <div class="accordion-heading">
        <a id="btnUpDown" data-parent="#collapse-group" href="#collapseGOne"
            data-toggle="collapse" onclick="btnUpDown_click(this)">

            <div class="widget-title">
                <span class="icon"><i class="icon-th-list"></i></span>
                <h5>Lista de Rubros</h5>
                <span class="btn-right"><i class="icon-chevron-up"></i></span>
            </div>
        </a>

    </div>
    <div class="accordion-body collapse in" id="collapseGOne">
        <div class="widget-content">
            <asp:GridView ID="grdRubros" runat="server" AutoGenerateColumns="false" GridLines="none" CssClass="table table-striped table-bordered" OnRowDataBound="grdRubros_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="&nbsp;" ItemStyle-Width="12px">
                        <ItemTemplate>
                            <img alt="" style="cursor: pointer" src="../../Content/img/app/grid_arrow-down.png" runat="server" id="ImgBtn" visible="false" />
                            <asp:Panel ID="pnlSubRubros" runat="server" Style="display: none">
                                <asp:GridView ID="gvSubRubros" runat="server" AutoGenerateColumns="false" GridLines="none" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="50px" DataField="id_EncomiendaRubro" HeaderText="Id" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="100%" DataField="Nombre" HeaderText="Sub Rubro" />
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="gvRubrosDepositos" runat="server" AutoGenerateColumns="false" GridLines="none" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="50px" DataField="Codigo" HeaderText="Cód. Dep."/>
                                        <asp:BoundField ItemStyle-Width="100%" DataField="Descripcion" HeaderText="Depósito" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                    <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" />
                    <asp:BoundField DataField="nom_tipoactividad" HeaderText="Tipo de Actividad" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="Nomenclatura" HeaderText="Nomenclatura" ItemStyle-Width="70px" ItemStyle-CssClass="align-center" />
                    <asp:BoundField DataField="SuperficieHabilitar" HeaderText="Superficie (m2)" ItemStyle-Width="90px" ItemStyle-CssClass="align-center" />

                </Columns>
            </asp:GridView>
            <br />
            <div id="pnlRubrosAnterior" runat="server">
                <strong>Rubros de la habilitación anterior</strong>
                <asp:GridView ID="grdRubrosANT" runat="server" AutoGenerateColumns="false" GridLines="none" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                        <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" />
                        <asp:BoundField DataField="nom_tipoactividad" HeaderText="Tipo de Actividad" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="Nomenclatura" HeaderText="Nomenclatura" ItemStyle-Width="70px" ItemStyle-CssClass="align-center" />
                        <asp:BoundField DataField="SuperficieHabilitar" HeaderText="Superficie (m2)" ItemStyle-Width="90px" ItemStyle-CssClass="align-center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    try {
        $("[src*=down]").on("click", function () {
            var strImag = "";
            strImag = $(this).attr('src');
            if (strImag.indexOf("down") > 0) {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../../Content/img/app/grid_arrow-up.png");
            }
            else {
                $(this).attr("src", "../../Content/img/app/grid_arrow-down.png");
                $(this).closest("tr").next().remove();
            }
        });
    }
    catch (e) {
        alert(e.message);
    }
    function btnUpDown_click(obj) {

        if ($("[id*='collapseGOne']").css("height") == "0px") {
            $(".icon-chevron-down")[0].className = "icon-chevron-up";
        }
        else {
            $(".icon-chevron-up")[0].className = "icon-chevron-down";
        }
    }

</script>

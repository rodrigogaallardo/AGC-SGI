<%@ Page Title="Parametros de Bandeja" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Parametros_Bandeja.aspx.cs"
    Inherits="SGI.GestionTramite.Parametros_Bandeja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <asp:Panel ID="pnlBotonDefault" runat="server">

        <div class="">

            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- Titulo collapsible Parametros Superficie --%>
                <div class="accordion-heading">
                    <a id="bt_Parametros_Superficie_btnUpDown" data-parent="#collapse-group" href="#bt_Parametros_Superficie_Bandeja" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_Parametros_Superficie_tituloControl" runat="server" Text="Parametros por Superficie"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="bt_Parametros_Superficie_Bandeja">
                    <div class="widget-content">
                        <%-- Campos de Texto --%>
                        <asp:UpdatePanel ID="updPnlSuperficie" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <%-- SSP --%>
                                        <div class="row-fluid">
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblSSP" runat="server" AssociatedControlID="txtSSP" Text="Simple Sin plano:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSP" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMenorSSP" runat="server" AssociatedControlID="ddlMenorSSP" Text="Si es menor o igual revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMenorSSP" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMayorSSP" runat="server" AssociatedControlID="ddlMayorSSP" Text="Si es mayor revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMayorSSP" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- SCP --%>
                                        <div class="row-fluid">
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblSCP" runat="server" AssociatedControlID="txtSCP" Text="Simple Con plano:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSCP" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMenorSCP" runat="server" AssociatedControlID="ddlMenorSCP" Text="Si es menor o igual revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMenorSCP" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMayorSCP" runat="server" AssociatedControlID="ddlMayorSCP" Text="Si es mayor revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMayorSCP" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- Especiales --%>
                                        <div class="row-fluid">
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblESPE" runat="server" AssociatedControlID="txtESPE" Text="Inspección Previa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtESPE" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMenorESPE" runat="server" AssociatedControlID="ddlMenorESPE" Text="Si es menor o igual revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMenorESPE" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMayorESPE" runat="server" AssociatedControlID="ddlMayorESPE" Text="Si es mayor revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMayorESPE" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- Esparcimiento --%>
                                        <div class="row-fluid">
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblESPA" runat="server" AssociatedControlID="txtESPA" Text="Habilitación Previa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtESPA" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMenorESPA" runat="server" AssociatedControlID="ddlMenorESPA" Text="Si es menor o igual revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMenorESPA" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMayorESPA" runat="server" AssociatedControlID="ddlMayorESPA" Text="Si es mayor revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMayorESPA" runat="server" Width="150px" AutoPostBack="true">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- Boton para Actualizar --%>
                        <asp:UpdatePanel ID="updPnlGuardarSuperficie" runat="server">
                            <ContentTemplate>
                                <div class="pull-right">
                                    <asp:LinkButton ID="btnGuardarSuperficie" runat="server" CssClass="btn  btn-inverse" ValidationGroup="Guardar" OnClick="btnGuardarSuperficie_OnClick">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                    </div>
                </div>
                </div>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px" >

                <%-- Titulo collapsible Parametros Rubro --%>
                <div class="accordion-heading">
                    <a id="bt_Parametros_Rubro_btnUpDown" data-parent="#collapse-group" href="#bt_Parametros_Rubro_Bandeja" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_Parametros_Rubro_tituloControl" runat="server" Text="Parametros por Rubro"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="bt_Parametros_Rubro_Bandeja">
                    <div class="widget-content; border=None">
                        <%-- Campos de Texto --%>
                        <asp:UpdatePanel ID="updPnlRubro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <div class="row-fluid">
                                            <div class="span6">
                                                <div class="control-group">
                                                    <asp:Label ID="lblRubro" runat="server" AssociatedControlID="ddlRubro" Text="Código o descripción del rubro o palabra clave:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlRubro" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlRubro_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblRevisaRubro" runat="server" AssociatedControlID="ddlRevisaRubro" Text="Revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlRevisaRubro" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlRevisaRubro_SelectedIndexChanged">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span2">
                                                <asp:UpdatePanel ID="updPnlGuardarRubro" runat="server">
                                                    <ContentTemplate>
                                                        <div class="pull-right mright20 mtop10">
                                                            <asp:LinkButton ID="btnGuardarRubro" runat="server" CssClass="btn  btn-inverse" ValidationGroup="Guardar" OnClick="btnGuardarRubro_OnClick">
                                                                <i class="icon-white icon-refresh"></i>
                                                                <span class="text">Actualizar</span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- Boton para Actualizar --%>
                        <%--                        <asp:UpdatePanel ID="updPnlGuardarRubro" runat="server">
                            <ContentTemplate>
                                <div class="pull-right mright20 mbottom20">
                                    <asp:LinkButton ID="btnGuardarRubro" runat="server" CssClass="btn  btn-inverse" ValidationGroup="Guardar" OnClick="btnGuardarRubro_OnClick">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>


                        <asp:UpdatePanel ID="updPnlFiltroRubro" runat="server" UpdateMode="Conditional" class="mleft10 mright10 mtop10">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <div class="row-fluid">
                                            <div class="span3">
                                                <div class="control-group">
                                                    <asp:Label ID="lblFiltroTipoTramiteRubro" runat="server" AssociatedControlID="ddlFiltroTipoTramiteRubro" Text="Tipo de Tramite:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlFiltroTipoTramiteRubro" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroTipoTramiteRubro_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <asp:Label ID="lblFiltroRubro" runat="server" AssociatedControlID="ddlFiltroRubro" Text="Rubro:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlFiltroRubro" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroRubro_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span4">
                                                <div class="control-group">
                                                    <asp:Label ID="lblFiltroRevisa" runat="server" AssociatedControlID="ddlFiltroRevisa" Text="Revisa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlFiltroRevisa" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroRevisa_SelectedIndexChanged">
                                                            <asp:ListItem Text="SubGerente" Value="SubGerente"></asp:ListItem>
                                                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                                                            <asp:ListItem Text="Todos" Value="" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:UpdatePanel ID="updPnlGrdRubro" runat="server" UpdateMode="Conditional" class="mleft10 mright10">
                            <ContentTemplate>
                                <asp:GridView ID="grdRubrosParametros" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                    ItemType="SGI.Model.clsItemGrillaParametrosRubro" AllowPaging="true" PageSize="30"
                                    OnDataBound="grdRubrosParametros_DataBound" OnRowDataBound="grdRubrosParametros_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="id_param" HeaderText="Parametro" Visible="false" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Tipo de Tramite" ItemStyle-Width="15%" />
                                        <asp:BoundField DataField="cod_rubro" HeaderText="Rubro" ItemStyle-Width="80%" />
                                        <asp:BoundField DataField="Revisa" HeaderText="Revisa" ItemStyle-Width="15%" />
                                           <%-- Eliminar --%>
                                    <asp:TemplateField ItemStyle-Width="15px" HeaderText="Acción" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEliminarRevisa" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local" 
                                                    CommandArgument='<%# Eval("id_param") %>' OnCommand="lnkEliminarRevisa_Command">
                                                    <i class="icon-remove" style="transform: scale(1.2);"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField> 

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                            <img src="../Content/img/app/NoRecords.png" />No se encontraron registros.
                                        </asp:Panel>
                                    </EmptyDataTemplate>
                                    <PagerTemplate>
                                        <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
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
        </div>
    </asp:Panel>
    
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Error</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div id="frmMsj" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Mensaje</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon imoon-info fs32" style="color: darkcyan"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updMsj" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMsj" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('.modal-backdrop').remove();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            inicializar_controles();
        });

        function bt_btnUpDown_collapse_click(obj) {
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
        function inicializar_controles() {
            $("#<%: ddlMenorSSP.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMenorSCP.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMenorESPE.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMenorESPA.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMayorSSP.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMayorSCP.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMayorESPE.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMayorESPA.ClientID %>").select2({ allowClear: true });

            $("#<%: ddlRubro.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlFiltroRubro.ClientID %>").select2({ allowClear: true });

            $('#<%=txtSSP.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtSCP.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtESPE.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtESPA.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });

        }

        function showfrmMsj() {
            $('.modal-backdrop').remove();
            $("#frmMsj").modal("show");
            return false;
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }
    </script>

</asp:Content>

<%@ Page Title="ABM de rubros" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMRubrosCUR.aspx.cs" Inherits="SGI.ABM.AbmRubrosCUR" %>

<%@ Register Src="~/ABM/RubrosCUR/Controls/VisualizarRubroCUR.ascx" TagPrefix="uc" TagName="visualizarRubroCUR" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>
    <script src="../../Scripts/Select2-locales/select2_locale_es.js"></script>
    <script src="../../Scripts/Funciones.js" type="text/javascript"></script>
    <script src="../../Scripts/Datepicker_es.js" type="text/javascript"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />

    <%: Styles.Render("~/Content/themes/base/css") %>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#<%: btnCargarDatos.ClientID %>").click();
            loadPopOverRubro();
        });

        function loadPopOverRubro() {
            $("[id*='lnkSubRubros']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

            $("[id*='MainContent_grdResultados_lnkSubRubros_']").each(function () {
                //para cada fila de la grilla, se busca el link y se lo vincula al panel de la misma fila
                //para que con el click del link habra el popOver de un html
                var id_pnlSubRubros = $(this).attr("id").replace("MainContent_grdResultados_lnkSubRubros_", "MainContent_grdResultados_pnlSubRubros_");
                var objRubros = $("#" + id_pnlSubRubros).html();
                $(this).popover({
                    title: 'Sub Rubros',
                    content: objRubros,
                    html: 'true'
                });
            });
        }

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function validarBuscar() {
            var ret = true;
            hideSummary();
            $("#Buscar_CamposReq").hide();

            if ($.trim($("#<%: txtCodigoDescripcionoPalabraClave.ClientID %>").val()).length == 0) {
                $("#Req_txtCodigoDescripcionoPalabraClave").css("display", "inline-block");
                ret = false;
            }

            if (ret) {

            }
            else {
                $("#Buscar_CamposReq").css("display", "inline-block");
            }
            return ret;
        }

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
            $("#box_resultado").hide("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");

            $("#<%: btnCargarDatos.ClientID %>").click();
        }

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
        }

        function popOverSubRubros(obj) {
            if ($(obj).attr("data-visible") == "true") {
                $(obj).attr("data-visible", "false");
            }
            else {
                $("[data-visible='true']").popover("toggle");
                $("[data-visible='true']").attr("data-visible", "false");
                $(obj).attr("data-visible", "true");
            }
            return false;
        }


    </script>

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                <td>
                    <img src="<%: ResolveUrl("../../Content/img/app/Loading128x128.gif") %>" alt="" />
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>

    <div id="page_content" style="display: none">
        <%--Busqueda de RubrosCUR--%>
        <div id="box_busqueda" class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="icon-search"></i></span>
                <h5>Búsqueda de Rubros</h5>
            </div>
            <div class="widget-content">
                <div>
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label" style="width: 500px">Código o Descripción del Rubro o grupo circuito o Palabra Clave: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCodigoDescripcionoPalabraClave" runat="server" CssClass="input-xlarge" Width="250px"></asp:TextBox>
                                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClick="btnBuscar_Click"
                                            ValidationGroup="buscar" OnClientClick="return validarBuscar();">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Buscar</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnNuevoRubroCUR" runat="server" CssClass="btn btn-success"
                                            Style="margin-left: 40px" OnClick="btnNuevoRubroCUR_Click">
                                            <i class="icon-white icon-plus"></i>
                                            <span class="text">Nuevo Rubro</span>
                                        </asp:LinkButton>
                                        <div class="control-group inline-block">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updpnlBuscar">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
                                <div id="Buscar_CamposReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Debe ingresar algún valor en los campos para poder realizar la búsqueda. El Código o Descripción o grupo circuito es obligatorio.
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!-- /.fin Busqueda de RubrosCUR -->

        <%--Grilla Resultados --%>
        <div id="box_resultado" class="widget-box" style="display: none;">
            <%--Resultados --%>
            <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="margin-left: 10px; margin-right: 10px;">
                        <asp:HiddenField ID="hid_grdRubros_rowIndexselected" runat="server" Value="false" />
                        <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                            <div class="text-left">
                                <h5>Resultado de la búsqueda</h5>
                            </div>
                            <div class="text-right">
                                <span class="badge">Cantidad de registros:
                                    <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                            </div>
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            DataKeyNames="Id_Rubro" ItemType="SGI.Model.clsItemRubroCUR" SelectMethod="GetRubros"
                            OnPageIndexChanging="grdResultados_PageIndexChanging"
                            OnRowDataBound="grdResultados_RowDataBound"
                            OnDataBound="grdResultados_DataBound"
                            PageSize="50" AllowSorting="true">
                            <Columns>
                                <asp:BoundField DataField="cod_rubro" HeaderText="Codigo" ItemStyle-CssClass="text-center" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" ItemStyle-Width="350px" />
                                <asp:BoundField DataField="cir_rubro" HeaderText="Circuito" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />
                                <asp:TemplateField ItemStyle-Width="10px" HeaderText="Sub Rubros">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSubRubros" runat="server" OnClientClick="return popOverSubRubros(this);" data-toggle="popover"
                                            data-visible="false" data-placement="right" CommandArgument="<%# Item.Id_rubro %>"
                                            title="Lista de Sub Rubros">
                                            <i class="imoon imoon-eye-open" style="font-size: medium; margin-right: 5px; margin-left: 5px"></i>
                                            <%--<i class="icon-share"></i>--%>
                                            <%--Popover con la lista de subRubros--%>
                                            <asp:Panel ID="pnlSubRubros" runat="server" Style="display: none; padding: 10px; max-height: 300px; max-width: 500px">
                                                <asp:DataList ID="lstSubRubros" runat="server" Width="290px" CssClass="table table-bordered table-striped">
                                                    <ItemTemplate>
                                                        <div class="inline">
                                                            <asp:Label ID="lblCodRubro" runat="server" CssClass="badge badge-info"><%# Eval("CodigoSubRubro") %></asp:Label>
                                                        </div>
                                                        <div class="inline">
                                                            <asp:Label ID="lblDescRubro" runat="server" CssClass="pLeft5"><%# Eval("DescripcionSubRubro") %></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </asp:Panel>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciónes" ItemStyle-CssClass="text-center" ItemStyle-Width="75px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="btnVerRubro" runat="server" ToolTip="Ver detalle del rubro" data-visible="false"
                                            NavigateUrl='<%#"~/ABM/RubrosCUR/VerRubroCur.aspx?Id=" + Eval("Id_rubro")%>'>                                          
                                            <i class="imoon imoon-eye-open" style="font-size: medium; margin-right: 5px; margin-left: 5px"></i>
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="btnEditarRubro" runat="server" ToolTip="Editar el rubro" data-visible="false"
                                            NavigateUrl='<%#"~/ABM/RubrosCUR/EditarRubroCur.aspx?Id=" + Eval("Id_rubro")%>'>                                          
                                            <i class="imoon imoon-pencil" style="font-size: medium; margin-right: 5px; margin-left: 5px"></i>
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="btnHistorial" runat="server" ToolTip="Ver Historial del rubro" data-visible="false"
                                            NavigateUrl='<%#"~/ABM/RubrosCUR/HistorialRubroCur.aspx?Id=" + Eval("Id_rubro") %>'>
                                            <i class="imoon imoon-file" style="font-size:medium;margin-right:5px;margin-left:5px"></i>
                                        </asp:HyperLink>
                                       
                                             <asp:CheckBox ToolTip="Ver SoloApra" ID="ChkSoloApra" runat="server" Enabled="false" style="font-size: medium; margin-right: 5px; margin-left: 5px"
                                             Checked='<%# Eval("SoloApra") %>' />
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div>
                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                    <span class="mleft20">No se encontraron registros.</span>
                                </div>
                            </EmptyDataTemplate>
                            <PagerTemplate>
                                <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                    <div style="display: inline-table">
                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
                                            DisplayAfter="0">
                                            <ProgressTemplate>
                                                <img src="../../Content/img/app/Loading24x24.gif" alt="" />
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
                    </div>
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- /.fin grilla resultados -->

        <%--Alta RubroCUR --%>
        <div id="box_datos" style="display: none">
            <asp:HiddenField ID="hid_id_rubroReq" runat="server" />
            <asp:Panel ID="PnlVisualizarRubro" runat="server">
                <asp:UpdatePanel ID="updVisualizarRubro" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc:visualizarRubroCUR ID="VisualizarRubroCUR" runat="server"></uc:visualizarRubroCUR>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <!-- /.fin alta -->

        <%--modal de Errores--%>
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
        <!-- /.modal modal de Errores-->
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmZonificacionParcelasCUR.aspx.cs" Inherits="SGI.ABM.AbmZonificacionParcelasCUR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <asp:HiddenField ID="hiddenTipoBusq" runat="server" />

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                <td>
                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>

    <div id="page_content" style="display: none;">
        <hgroup class="title">
            <h1>Zonificador Masivo de Ubicaciones CUR</h1>
        </hgroup>
        <%--Busqueda de Zonificación de Parcelas--%>
        <div id="filtros" style="display: none">
            <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">
                <%--buscar ubicacion --%>
                <div class="accordion-group widget-box">
                    <div class="accordion-heading">
                        <a id="bt_ubicacion_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_ubicacion"
                            data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                            <div class="widget-title">
                                <span class="icon" style="margin-left: 4px"><i class="icon-search"></i></span>
                                <h5>B&uacute;squeda de Ubicaciones</h5>
                                <span class="btn-right"><i class="imoon-chevron-up"></i></span>
                            </div>
                        </a>
                    </div>

                    <div class="accordion-body collapse in" id="collapse_bt_ubicacion">
                        <div class="widget-content">
                            <%--tipos de busquedad por ubicacion--%>
                            <div id="ubics" class="widget-content">
                                <div class="btn-group" data-toggle="buttons-radio" style="display: table-cell;">
                                    <button id="btnBuscarPorSMP" type="button" class="btn active" onclick="switchear_buscar_ubicacion(1);">Por Sección y Manzana</button>
                                    <button id="btnBuscarPorDom" type="button" class="btn" onclick="switchear_buscar_ubicacion(2);">Por Domicilio</button>
                                </div>
                            </div>
                            <%--buscar por seccion, manzana--%>
                            <div id="buscar_ubi_por_smp" class="widget-content">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_smp" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <fieldset>
                                                <div class="control-group">
                                                    <asp:Label ID="lblSeccion" runat="server" AssociatedControlID="txtSeccion"
                                                        Text="Sección:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                    <div class="control-label" style="margin-left: -75px; margin-top: -20px">
                                                        <asp:TextBox ID="txtSeccion" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="lblManzana" runat="server" AssociatedControlID="txtManzana"
                                                        Text="Manzana:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                    <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                        <asp:TextBox ID="txtManzana" runat="server" MaxLength="6" Width="50px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--buscar por nombre de calle--%>
                            <div id="buscar_ubi_por_dom" class="widget-content" style="display: none">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_dom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                                        <div class="form-horizontal">
                                            <fieldset>
                                                <div class="control-group">
                                                    <asp:Label ID="lblUbiCalle" runat="server" AssociatedControlID="ddlUbiCalle"
                                                        CssClass="control-label">Búsqueda de Calle:</asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlUbiCalle" runat="server" Width="500px"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="lblPuertaDesde" runat="server" AssociatedControlID="txtPuertaDesde"
                                                        CssClass="control-label">Nro. Puerta Desde:</asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtPuertaDesde" runat="server" MaxLength="10" Width="50px"
                                                            CssClass="input-xlarge">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="lblPuertaHasta" runat="server" AssociatedControlID="txtPuertaHasta"
                                                        CssClass="control-label">Nro. Puerta Hasta:</asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtPuertaHasta" runat="server" MaxLength="10" Width="50px"
                                                            CssClass="input-xlarge">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group form-inline">
                                                    <div class="control-group inline-block">
                                                        <asp:Label ID="lblTipoVereda" runat="server" AssociatedControlID="radioNumeracionAmbas"
                                                            CssClass="control-label">Tipo de Vereda:</asp:Label>
                                                        <div class="controls form-inline">
                                                            <asp:RadioButton ID="radioNumeracionAmbas" runat="server"
                                                                Text="Ambas" GroupName="numeracion" Checked="true" />
                                                            <asp:RadioButton ID="radioNumeracionPar" runat="server" Style="padding-left: 5px"
                                                                Text="Par" GroupName="numeracion" />
                                                            <asp:RadioButton ID="radioNumeracionImpar" runat="server" Style="padding-left: 5px"
                                                                Text="Impar" GroupName="numeracion" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div id="Val_CallePuerta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Para realizar la búsqueda por domicilio es necesario ingresar todos los valores (Calle, puerta desde y puerta hasta).
                                                    </div>
                                                    <div id="Val_DesdeHasta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        En el rango de alturas: La altura hasta no puede ser mayor a la altura desde
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <%--fin buscar ubicacion --%>
                <%--botones accion --%>
                <asp:UpdatePanel ID="btn_BuscarPartida" style="margin-top: 10px" runat="server">
                    <ContentTemplate>
                        <div class="pull-right" style="margin-bottom: 20px">
                            <div class="control-group inline-block">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="btn_BuscarPartida">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" ValidationGroup="buscar" OnClick="btnBuscar_Click" OnClientClick="return validarBuscar();">
                                <i class="icon-white icon-search"></i>
                                <span class="text">Buscar</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick">
                                <i class=" icon-refresh"></i>
                                <span class="text">Limpiar</span>
                            </asp:LinkButton>
                        </div>
                        <div class="control-group" style="margin-bottom: 0px">
                            <div id="ValFields" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                Para poder realizar la búsqueda es necesario ingresar al menos uno de los siguientes filtros de búsqueda: Sección, manzana o calle (entre rangos).
                            </div>
                            <div id="ValSummary" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                Revise las validaciones en pantalla.
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--fin botones accion --%>
            </asp:Panel>
        </div>

        <%--resultados busqueda --%>
        <div id="box_resultado" class="widget-box">
            <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="margin-left: 10px; margin-right: 10px;">
                        <asp:Panel ID="pnlResultadoBuscar" runat="server" Visible="true">
                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                                <div class="text-left">
                                    <h5 style="width: 100%; float: left">&nbsp;&nbsp;Resultado de la b&uacute;squeda</h5>
                                </div>
                                <div class="text-right">
                                    <span class="badge">Cantidad de registros:
                                            <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                </div>
                            </asp:Panel>
                            <br />
                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" Visible="true"
                                GridLines="None" CssClass="table table-bordered"
                                SelectMethod="GetResultados"
                                AllowPaging="true" PageSize="30" OnPageIndexChanging="grdResultados_PageIndexChanging"
                                OnDataBound="grdResultados_DataBound">
                                <Columns>
                                    <asp:BoundField DataField="id_ubicacion" HeaderText="Id" ItemStyle-Width="80px" HeaderStyle-ForeColor="#0088cc" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="partidaMatriz" HeaderText="Partida Matriz" ItemStyle-Width="80px" HeaderStyle-ForeColor="#0088cc" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="15px" />
                                    <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="15px" />
                                    <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="15px" />
                                    <asp:BoundField DataField="mixturas" HeaderText="Mixturas" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="15%" />
                                    <asp:BoundField DataField="distritos_zonas" HeaderText="Grupo - Distrito - Zona - Subzona" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="25%" />
                                    <asp:BoundField DataField="direccion" HeaderText="Domicilio" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="25%" />
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
                            <br />
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--fin resultados busqueda --%>

        <%-- Mixturas y Distritos --%>
        <div id="box_MixtDist" class="widget-box">
            <div class="accordion-heading">
                <a id="bt_actualizar_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_actualizar"
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                    <div class="widget-title">
                        <span class="icon" style="margin-left: 4px"><i class="icon-pencil"></i></span>
                        <h5>Actualizar Datos</h5>
                        <span class="btn-right"><i class="imoon-chevron-up"></i></span>
                    </div>
                </a>
            </div>
            <div class="accordion-body collapse in" id="collapse_bt_actualizar">
                <div class="widget-content">
                    <asp:UpdatePanel ID="updMixtDistZona" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%-- Mixturas --%>
                            <asp:UpdatePanel ID="updDDLMixtura" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="widget-content">
                                        <fieldset>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <asp:Label ID="lblMixtura" runat="server" AssociatedControlID="ddlMixtura"
                                                        CssClass="control-label">Mixtura:</asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMixtura" runat="server" Width="500px" AutoPostBack="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <div class="control-group pleft20 ptop10 pright25">
                                            <asp:UpdatePanel ID="updMixtura" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdMixturas" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None"
                                                        OnRowDataBound="grdMixturas_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="mix" HeaderText="Mixtura" />
                                                            <asp:BoundField DataField="mixDescripcion" HeaderText="Descripción" />
                                                            <asp:BoundField DataField="mixturaAccion" HeaderText="Acción" />
                                                            <asp:TemplateField ItemStyle-Width="60px" ItemStyle-CssClass="text-center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEliminarMixtura" runat="server" title='Eliminar' data-toggle="tooltip" OnClick="btnEliminarMixtura_Click">
                                                                    <i class="imoon imoon-remove fs16 color-black"></i>        
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="pad10">
                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">Agregue Mixturas para actualizar las Ubicaciones</span>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="form-inline">
                                                <div class="control-group pull-right">
                                                    <div class="control-group inline-block">
                                                        <asp:UpdateProgress ID="UpdateProgressMixtura" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updDDLMixtura">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <asp:LinkButton ID="btnEliminarMixturasMasivas" runat="server" CssClass="btn btn-default" OnClick="btnEliminarMixturasMasivas_Click">
                                                        <i class="imoon imoon-plus"></i>
                                                        <span class="text">Eliminar Mixtura</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnAgregarMixtura" runat="server" CssClass="btn btn-default" OnClick="btnAgregarMixtura_Click">
                                                        <i class="imoon imoon-plus"></i>
                                                        <span class="text">Agregar Mixtura</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%-- Fin Mixturas --%>
                            <%-- Distritos --%>
                            <asp:UpdatePanel ID="updDDLDistritos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="widget-content">
                                        <fieldset>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label for="ddlGrupoDistritos" class="control-label">Grupo Distrito:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlGrupoDistritos" runat="server" OnSelectedIndexChanged="ddlGrupoDistritos_SelectedIndexChanged"
                                                            AutoPostBack="true" Width="350px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label for="ddlDistritos" class="control-label">Distrito:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlDistritos" runat="server" Enabled="false" OnSelectedIndexChanged="ddlDistritos_SelectedIndexChanged"
                                                            AutoPostBack="true" Width="350px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label for="ddlDistritosZonas" class="control-label">Zonas:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlDistritosZonas" runat="server" Enabled="false" OnSelectedIndexChanged="ddlDistritosZonas_SelectedIndexChanged"
                                                            AutoPostBack="true" Width="350px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label for="ddlDistritosSubZonas" class="control-label">Sub Zonas:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlDistritosSubZonas" runat="server" Enabled="false" Width="350px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <div class="control-group pleft20 ptop10 pright25">
                                            <asp:UpdatePanel ID="updDistritos" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdDistritos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None"
                                                        EmptyDataText="No se cargaron Distritos..."
                                                        OnRowDataBound="grdDistritos_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="grupoDistrito" HeaderText="Grupo Distrito" />
                                                            <asp:BoundField DataField="distrito" HeaderText="Distrito" />
                                                            <asp:BoundField DataField="zonas" HeaderText="Zonas" />
                                                            <asp:BoundField DataField="subzonas" HeaderText="Sub Zonas" />
                                                            <asp:BoundField DataField="IdDistrito" HeaderText="IdDistrito" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="IdZona" HeaderText="IdZona" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="IdSubZona" HeaderText="IdSubZona" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="distritoAccion" HeaderText="Acción" />
                                                            <asp:TemplateField ItemStyle-Width="60px" ItemStyle-CssClass="text-center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEliminarDistrito" runat="server" title='Eliminar' data-toggle="tooltip" OnClick="btnEliminarDistrito_Click">
                                                                    <i class="imoon imoon-remove fs16 color-black"></i>        
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="pad10">
                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">Agregue Mixturas para actualizar las Ubicaciones</span>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="form-inline">
                                                <div class="control-group pull-right">
                                                    <div class="control-group inline-block">
                                                        <asp:UpdateProgress ID="UpdateProgressDistritos" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updDDLDistritos">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <asp:LinkButton ID="btnEliminarDistritosMasivos" runat="server" CssClass="btn btn-default" OnClick="btnEliminarDistritosMasivos_Click">
                                                        <i class="imoon imoon-plus"></i>
                                                        <span class="text">Eliminar Distrito</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnAgregarDistrito" runat="server" CssClass="btn btn-default" OnClick="btnAgregarDistrito_Click">
                                                        <i class="imoon imoon-plus"></i>
                                                        <span class="text">Agregar Distrito</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                    </div>
                                    <br />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%-- FIn Distritos --%>
                            <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                                <ContentTemplate>
                                    <div id="pnlBotonesGuardar" class="control-group">
                                        <div class="controls">
                                            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" OnClick="btnGuardar_Click">
                                        <i class="imoon imoon-save"></i>
                                        <span class="text">Actualizar Ubicaciones</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="control-group inline-block">
                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updMixtDistZona">
                                            <ProgressTemplate>
                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <%--                                    <div class="text-left">
                                        <label style="color: gray;">&nbsp;&nbsp;(Se actualizaran solo aquellas ubicaciones que no posean mixturas o distritos.)</label>
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--modal de mensajes--%>
    <div id="frmMensaje" class="modal fade">
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
                                <label class="imoon icon-chevron-down fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updMensaje" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMensaje" runat="server" Style="color: Black"></asp:Label>
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

    <!-- /.modal -->
    <%--Modal Errores--%>
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="updfrmerror" runat="server">
                        <ContentTemplate>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="frmerrortitle" runat="server" Text="Error"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <i class="imoon imoon-remove-circle fs64" style="color: #f00"></i>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" Style="color: black"></asp:Label>
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
    <%--Fin Modal Errores--%>

    <%--Modal Advertencia--%>
    <div id="frmAdvertencia" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="updfrmAdvTitle" runat="server">
                        <ContentTemplate>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="frmAdvTitle" runat="server" Text="Atención"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 15px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <i class="imoon imoon-warning fs64" style="color: #f00"></i>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updfrmAdvBody" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblAdv" runat="server" Style="color: black"></asp:Label>
                                        <br />
                                        <br />
                                        <div id="pnlBotonesAdv" class="control-group" style="text-align: right">
                                            <asp:Button ID="btnContinuarAdv" runat="server" CssClass="btn btn-primary" Text="Continuar"
                                                OnClick="btnContinuarAdv_Click" />
                                            <asp:Button ID="btnCancelarAdv" runat="server" CssClass="btn btn-default" Text="Cancelar"
                                                OnClick="btnCancelarAdv_Click" />
                                            <asp:HiddenField ID="hidAdvIdUbi" runat="server" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <%--Fin Modal Advertencia--%>

    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_controles();
            init_Js_updpnlBuscar();
            init_Js_updpnlBuscarDistritos();
            $("#page_content").hide();
            $("#Loading").show();
            $("#filtros").show("slow");

            $("#<%: btnCargarDatos.ClientID %>").click();
        });

        function init_Js_updpnlBuscarDistritos() {
            $("#<%: ddlGrupoDistritos.ClientID %>").select2({
                allowClear: true
            });

            $("#<%: ddlDistritos.ClientID %>").select2({
                allowClear: true
            });

            return false;
        }


        function init_Js_updpnlBuscar() {
            $("#<%: ddlUbiCalle.ClientID %>").select2({
                allowClear: true,
                placeholder: "Seleccione una calle",
                minimumInputLength: 2
            });

            $("#<%: ddlDistritos.ClientID %>").select2({
                allowClear: true
            });

            $("#Val_CallePuerta").hide();
            $("#Val_DesdeHasta").hide();
            $("#ValFields").hide();

            $("#box_MixtDist").hide();

            return false;
        }

        function inicializar_controles() {
            camposAutonumericos();
        }

        function camposAutonumericos() {
            $("#<%= txtPuertaDesde.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $("#<%= txtPuertaHasta.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $('#<%=txtSeccion.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        }

        function validarBuscar() {
            $("#Val_CallePuerta").hide();
            $("#Val_DesdeHasta").hide();
            $("#ValFields").hide();
            var ret = true;

            if ($("#buscar_ubi_por_dom").css("display") != "none") {

                var calle = $('#<%: ddlUbiCalle.ClientID %>').val();
                var nropuertad = $('#<%: txtPuertaDesde.ClientID %>').val();
                var nropuertah = $('#<%: txtPuertaHasta.ClientID %>').val();
                if (calle == 0 && (nropuertad.length > 0 || nropuertah.length > 0)) {
                    $("#Val_CallePuerta").css("display", "inline-block");
                    ret = false;
                }
            }

            if ($("#MainContent_ddlUbiCalle").val() == 0 &&
                $("#MainContent_txtPuertaDesde").val() == 0 &&
                $("#MainContent_txtPuertaHasta").val() == 0 &&
                $("#MainContent_txtUbiSeccion").val() == 0 &&
                $("#MainContent_txtUbiManzana").val() == 0) {

                $("#ValFields").css("display", "inline-block");
                ret = false;
            }
            else {
                hideSummary();
            }

            return ret;
        }

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function bt_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(href_collapse).css("height") == "0px") {
                    $(obj).find(".imoon-chevron-down").switchClass("imoon-chevron-down", "imoon-chevron-up", 0);
                }
                else {
                    $(obj).find(".imoon-chevron-up").switchClass("imoon-chevron-up", "imoon-chevron-down", 0);
                }
            }
        }

        function switchear_buscar_ubicacion(btn) {

            if (btn == 1) {
                $("#buscar_ubi_por_dom").hide();
                $("#buscar_ubi_por_smp").show();

                $("#btnBuscarPorSMP").addClass("active");
                $("#btnBuscarPorDom").removeClass("active");
            }
            else if (btn == 2) {
                $("#buscar_ubi_por_smp").hide();
                $("#buscar_ubi_por_dom").show();

                $("#btnBuscarPorSMP").removeClass("active");
                $("#btnBuscarPorDom").addClass("active");
            }

            var opc = '' + btn;

            $("#<%: hiddenTipoBusq.ClientID %>").val(opc);
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;
        }

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showResultado() {
            $("#box_resultado").show("slow");
            $("#box_MixtDist").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
            $("#box_MixtDist").hide("slow");
        }

        function showMixturasDistritos() {
            $("#box_MixtDist").show("slow");
        }

        function hideMixturasDistritos() {
            $("#box_MixtDist").hide("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
        }

        function showfrmAdvertencia() {

            $("#frmAdvertencia").modal("show");
            return false;
        }

        function hidefrmAdvertencia() {

            $("#frmAdvertencia").modal("hide");
            return false;
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function showfrmMensaje() {
            $("#frmMensaje").modal("show");
            return false;
        }

    </script>

</asp:Content>

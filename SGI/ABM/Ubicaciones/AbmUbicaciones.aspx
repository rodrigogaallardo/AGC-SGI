<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmUbicaciones.aspx.cs" Inherits="SGI.ABM.Partidas.AbmUbicaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>


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
    <div id="page_content" style="display: none">
        <hgroup class="title">
            <h1>ABM de Ubicaciones</h1>
            <h1><%: Title %>.</h1>
        </hgroup>
        <div id="filtros" style="display: none">
            <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">
                <%--buscar ubicacion --%>
                <div class="accordion-group widget-box">
                    <%-- titulo collapsible buscar ubicacion --%>
                    <div class="accordion-heading">
                        <a id="bt_ubicacion_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_ubicacion"
                            data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                            <div class="widget-title">
                                <span class="icon" style="margin-left: 4px"><i class="imoon-map-marker"></i></span>
                                <h5>
                                    <asp:Label ID="Label1" runat="server" Text="Ubicación"></asp:Label></h5>
                                <span class="btn-right"><i class="imoon-chevron-up"></i></span>
                            </div>
                        </a>
                    </div>
                    <%-- controles collapsible buscar por ubicacion --%>
                    <div class="accordion-body collapse in" id="collapse_bt_ubicacion">
                        <div class="widget-content">
                            <%--tipos de busquedad por ubicacion--%>
                            <div id="ubics" class="widget-content">

                                <div class="btn-group" data-toggle="buttons-radio" style="display: table-cell;">
                                    <button id="btnBuscarPorPartida" type="button" class="btn active" onclick="switchear_buscar_ubicacion(1);">Por Partida</button>
                                    <button id="btnBuscarPorDom" type="button" class="btn" onclick="switchear_buscar_ubicacion(2);">Por Domicilio</button>
                                    <button id="btnBuscarPorSMP" type="button" class="btn" onclick="switchear_buscar_ubicacion(3);">Por SMP</button>
                                    <button id="btnBuscarPorUbiEspecial" type="button" class="btn" onclick="switchear_buscar_ubicacion(4);">Por Ubicaciones Especiales</button>
                                </div>

                            </div>
                            <%--buscar por numero de partida--%>
                            <div id="buscar_ubi_por_partida" class="widget-content">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_partida" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <fieldset>
                                                <div class="control-group">
                                                    <asp:Label ID="lblUbiPartidaMatriz" runat="server" AssociatedControlID="rbtnUbiPartidaMatriz"
                                                        CssClass="control-label">Tipo de Partida:</asp:Label>
                                                    <div class="controls">
                                                        <div class="form-inline">
                                                            <asp:RadioButton ID="rbtnUbiPartidaMatriz" runat="server" onclick="switchear_buscar_partida(1);"
                                                                Text="Matriz" GroupName="TipoDePartida" Checked="true" />
                                                            <asp:RadioButton ID="rbtnUbiPartidaHoriz" runat="server" onclick="switchear_buscar_partida(2);"
                                                                Style="padding-left: 5px" Text="Horizontal" GroupName="TipoDePartida" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="lblUbiNroPartida" runat="server" AssociatedControlID="txtUbiNroPartida"
                                                        CssClass="control-label">Nro. Partida:</asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtUbiNroPartida" runat="server" MaxLength="10" Width="100px"
                                                            CssClass="input-xlarge"></asp:TextBox>
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
                                                    <asp:Label ID="lblUbiNroPuerta" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                        CssClass="control-label">Nro. Puerta:</asp:Label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtUbiNroPuerta" runat="server" MaxLength="10" Width="50px"
                                                            CssClass="input-xlarge">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div id="Val_CallePuerta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Para realizar la búsqueda por domicilio es necesario ingresar ambos valores (Calle y Nro. de puerta).
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--buscar por seccion, manzana, parcela--%>
                            <div id="buscar_ubi_por_smp" class="widget-content" style="display: none">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_smp" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <fieldset>
                                                <div class="control-group">
                                                    <asp:Label ID="lblUbiSeccion" runat="server" AssociatedControlID="txtUbiManzana"
                                                        Text="Sección:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                    <div class="control-label" style="margin-left: -75px; margin-top: -20px">
                                                        <asp:TextBox ID="txtUbiSeccion" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="lblUbiManzana" runat="server" AssociatedControlID="txtUbiManzana"
                                                        Text="Manzana:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                    <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                        <asp:TextBox ID="txtUbiManzana" runat="server" MaxLength="6" Width="50px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="lblUbiParcela" runat="server" AssociatedControlID="txtUbiParcela"
                                                        Text="Parcela:" class="control-label" Style="padding-top: 0"></asp:Label>
                                                    <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                        <asp:TextBox ID="txtUbiParcela" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--buscar por tipo subtipo ubicacion--%>
                            <div id="buscar_ubi_por_especial" class="widget-content" style="display: none">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_especial" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="control-group">
                                                <label for="ddlUbiTipoUbicacion" class="control-label">Tipo de Ubicación:</label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddlUbiTipoUbicacion" runat="server"
                                                        OnSelectedIndexChanged="ddlUbiTipoUbicacion_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="350px">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label for="ddlUbiSubTipoUbicacion" class="control-label">Subtipo de Ubicación:</label>
                                                <div class="controls">

                                                    <asp:DropDownList ID="ddlUbiSubTipoUbicacion" runat="server"
                                                        Width="350px">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div id="Val_TipoSubtipo" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Para realizar la búsqueda es necesario ingresar ambos valores (Tipo y Subtipo).
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--filtro dadas de baja--%>
                            <div id="filtro" class="widget-content">
                                <div class="control-group" id="divDadaDBaja" runat="server">
                                    <asp:Label ID="lblUbiFiltro" runat="server" AssociatedControlID="txtUbiNroPartida"
                                        CssClass="control-label">Ubicaciones dadas de baja:</asp:Label>
                                    <div class="controls">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList runat="server" ID="ddlBaja" Width="115px">
                                                    <asp:ListItem Text="No" Value="false" />
                                                    <asp:ListItem Text="Si" Value="true" />
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="control-group" id="divPhDadaDBaja" runat="server" style="display: none">
                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtUbiNroPartida"
                                        CssClass="control-label">Partidas horizontales dadas de baja:</asp:Label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlPHDadaDBaja" runat="server" Width="115px">
                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                            <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
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
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="btn_BuscarPartida">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" ValidationGroup="buscar" OnClick="btnBuscar_OnClick" OnClientClick="return validarBuscar();">
                    <i class="icon-white icon-search"></i>
                    <span class="text">Buscar</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick">
                    <i class=" icon-refresh"></i>
                    <span class="text">Limpiar</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnNuevaPartida" runat="server" CssClass="btn" OnClick="btnNuevaPartida_Click">
                                            <i class="icon-plus"></i>
                                            <span class="text">Nueva Ubicación</span>
                            </asp:LinkButton>
                        </div>
                        <div class="control-group" style="margin-bottom: 0px">
                            <div id="ValFields" class="alert alert-small alert-danger" style="display: none;">
                                Para poder realizar la búsqueda es necesario ingresar al menos un valor en alguno de los filtros.
                            </div>
                            <div id="ValSummary" class="alert alert-small alert-danger " style="display: none;">
                                Revise las validaciones en pantalla.
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--fin botones accion --%>
            </asp:Panel>

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
                                    OnDataBound="grdResultados_DataBound" OnRowDataBound="grdResultados_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="partida" HeaderText="Partida Matriz" ItemStyle-Width="80px" HeaderStyle-ForeColor="#0088cc" ItemStyle-CssClass="text-center" />
                                        <asp:TemplateField ItemStyle-Width="10px" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" HeaderText="Horizontales">
                                            <%--Popover con la lista de Horizontales--%>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkHorizontales" runat="server" ToolTip="Partidas Horizontales"
                                                    OnClientClick="return popOverHorizontales(this);" data-toggle="focus" data-visible="false" data-placement="right"
                                                    CommandArgument='<%#Eval("id_ubicacion")%>' title="partidas horizontales"><i class="icon-share"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnNuevaPartidaHori" runat="server" ToolTip="Nueva Partida Horizontal" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnNuevaPartidaHori_Click"><i class="icon-plus"></i>
                                                </asp:LinkButton>
                                                <asp:Panel ID="pnlHorizontales" runat="server" Style="display: none; min-width: 50px; padding: 10px;">
                                                    <div style="max-height: 300px; overflow: auto; padding-right: 1px">
                                                        <asp:GridView ID="grdResultadosh" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                            OnRowDataBound="grdResultadosH_RowDataBound" CssClass="table table-bordered">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Opciones" ItemStyle-CssClass="text-center" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnVerHor" runat="server" ToolTip="Ver" data-toggle="tooltip" CssClass="link-local"
                                                                            CommandArgument='<%#Eval("id_ubicacion") + ";" +Eval("id_propiedadhorizontal")%>' OnClick="btnVer_ClickHor">
                                                        <i class="imoon imoon-eye-open"></i>
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="btnEditarHor" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local"
                                                                            CommandArgument='<%#Eval("id_ubicacion") + ";nada;" +Eval("id_propiedadhorizontal")%> ' OnClick="btnEditar_ClickHor">
                                                        <i class="imoon imoon-edit"></i>
                                                                        </asp:LinkButton>
                                                                        <asp:LinkButton ID="btnEliminarHor" runat="server" ToolTip="Baja" data-toggle="tooltip" CssClass="link-local"
                                                                            CommandArgument='<%#Eval("id_ubicacion") + ";baja;" +Eval("id_propiedadhorizontal")%>' OnClick="btnEditar_ClickHor">
                                                        <i class="imoon imoon-remove"></i>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="partidaHor" HeaderText="Partida" ItemStyle-CssClass="text-center" ItemStyle-Width="70px" />
                                                                <asp:BoundField DataField="pisoHor" HeaderText="Piso" ItemStyle-CssClass="text-center" ItemStyle-Width="150px" />
                                                                <asp:BoundField DataField="deptoHor" HeaderText="Depto" ItemStyle-CssClass="text-center" ItemStyle-Width="150px" />
                                                                <asp:BoundField DataField="baja_logica" HeaderText="Dada de Baja" ItemStyle-CssClass="text-center" ItemStyle-Width="90px" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <asp:Panel ID="pnlhNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                                                    <p>
                                                                        sin partidas horizontales.
                                                                    </p>
                                                                </asp:Panel>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <%--Fin Popover con la lista de Horizontales--%>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                        <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                        <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                        <asp:BoundField DataField="direccion" HeaderText="Domicilio" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="60%" />
                                        <asp:BoundField DataField="baja_logica" HeaderText="Dada de baja" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="45px" />
                                        <asp:TemplateField HeaderText="Acción" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnVer" runat="server" ToolTip="Ver" data-toggle="tooltip" CssClass="link-local" Visible="true"
                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnVer_Click">                                                    
                                                <i class="imoon imoon-eye-open fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnEditar_Click">                                                    
                                                <i class="imoon imoon-pencil2 fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnHistSMP" runat="server" ToolTip="Historial SMP" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnHistSMP_Click">                                                    
                                                <i class="imoon imoon-history fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnEliminar_Click">                                                    
                                                <i class="imoon imoon-remove fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
        </div>

        <div id="box_datos" style="display: none">

            <div class="col-sm-12 col-md-12">
                <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_id_ubihistcam" runat="server" />
                        <asp:HiddenField ID="hid_id_ubicacion" runat="server" />
                        <asp:HiddenField ID="hid_id_tipo_ubicacion" runat="server" />


                        <asp:Button ID="btnCargarDatos2" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />

                        <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">

                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                    <h5>ABM Partidas</h5>

                                </div>
                                <div class="widget-content">
                                    <div class="control-group">
                                        <label class="control-label">Usuarios Intervinientes:</label>
                                        <div class="controls">
                                            <asp:UpdatePanel ID="updHistorial" runat="server">
                                                <ContentTemplate>

                                                    <asp:GridView ID="grdHistorial" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField DataField="UserName" HeaderText="Usuario" />
                                                            <asp:BoundField DataField="Apenom" HeaderText="Apellido y Nombres" />
                                                            <asp:BoundField DataField="LastUpdateDate" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                                                HeaderText="Fecha de modif." />

                                                        </Columns>
                                                        <EmptyDataTemplate>

                                                            <div class="pad10">

                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">Aun sin usuarios intervinientes</span>

                                                            </div>

                                                        </EmptyDataTemplate>
                                                    </asp:GridView>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>


                                        </div>

                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">

                                                <label class="control-label">Tipo de Solicitud:</label>
                                                <div class="controls">

                                                    <asp:TextBox ID="txtTipoSolicitud" Enabled="false" runat="server" CssClass="form-control" Width="250px" MaxLength="10"></asp:TextBox>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label">Seccion</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtSeccion" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                                    <div id="Req_Seccion" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe ingresar la Seccion, Manzana y Parcela.
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">

                                                <label class="control-label">Número de Solicitud:</label>
                                                <div class="controls">

                                                    <asp:TextBox ID="txtNroSol" Enabled="false" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label">Manzana:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtManzana" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label">Nro. de Partida Matriz:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtNroPartida" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label">Parcela:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtParcela" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-horizontal">

                                        <div class="control-group">
                                            <label for="ddlbiTipoUbicacionABM" class="control-label">Tipo de Ubicación:</label>
                                            <div class="controls">

                                                <asp:DropDownList ID="ddlbiTipoUbicacionABM" runat="server"
                                                    OnSelectedIndexChanged="ddlbiTipoUbicacionABM_SelectedIndexChanged"
                                                    AutoPostBack="true" Width="350px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label for="ddlUbiSubTipoUbicacionABM" class="control-label">Subtipo de Ubicación:</label>
                                            <div class="controls">

                                                <asp:DropDownList ID="ddlUbiSubTipoUbicacionABM" runat="server"
                                                    Width="350px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Comisaría:</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlComisaria" runat="server" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Barrio:</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlBarrio" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Entidad Gubernamental:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="chbEntidadGubernamental" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Edificio Protegido:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="chbEdificioProtegido" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">¿Dada de baja?:</label>
                                        <div class="controls mtop5">
                                            <asp:RadioButton ID="rbtnBajaSi" runat="server" Text="Si" GroupName="BajaUbicacion" />
                                            <asp:RadioButton ID="rbtnBajaNo" runat="server" Text="No" GroupName="BajaUbicacion" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div>
                                <asp:UpdatePanel ID="UpdateZonas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="widget-box">
                                            <div class="widget-title">
                                                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                                <h5>Zonas</h5>
                                            </div>
                                            <div class="widget-content">
                                                <div class="control-group">
                                                    <label for="ddlZona1" class="control-label">Parcela:</label>
                                                    <div class="controls">

                                                        <asp:DropDownList ID="ddlZona1" runat="server"
                                                            AutoPostBack="false" Width="400px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label for="ddlZona2" class="control-label">Frentista:</label>
                                                    <div class="controls">

                                                        <asp:DropDownList ID="ddlZona2" runat="server"
                                                            AutoPostBack="false" Width="400px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label for="ddlZona3" class="control-label">Distrito Especial:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlZona3" runat="server"
                                                            AutoPostBack="false" Width="400px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <div id="Req_UbiZona" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe seleccionar al menos una zona de planeamiento.
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Observaciones:</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine"
                                                            Rows="5" Width="96%" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div>
                                <div class="widget-box">
                                    <div class="widget-title">
                                        <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                        <h5>Calles y puertas</h5>
                                    </div>
                                    <div class="widget-content">
                                        <div class="control-group pleft20 ptop10 pright25">
                                            <asp:UpdatePanel ID="updUbicaciones" runat="server">
                                                <ContentTemplate>

                                                    <asp:GridView ID="grdUbicaciones" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None"
                                                        OnRowDataBound="grdUbicaciones_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="calles" HeaderText="Calle" />
                                                            <asp:BoundField DataField="nroPuerta" HeaderText="Nro. Puerta" />


                                                            <asp:TemplateField ItemStyle-Width="60px" ItemStyle-CssClass="text-center">
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="btnEliminarUbicacion" runat="server" title='Eliminar' data-toggle="tooltip" OnClick="btnEliminarUbicacion_Click">
                                                                    <i class="imoon imoon-remove fs16 color-black"></i>
                                                                    
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>

                                                            <div class="pad10">

                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">No se encontraron Ubicaciones.</span>

                                                            </div>

                                                        </EmptyDataTemplate>
                                                    </asp:GridView>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <div class="form-inline">
                                                <div class="control-group text-left inline-block">
                                                    <div id="Req_Ubicaciones" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe ingresar al menos una ubicación.
                                                    </div>
                                                </div>
                                                <div class="control-group pull-right">
                                                    <asp:LinkButton ID="btnAgregarUbicacion" runat="server" CssClass="btn btn-default" OnClick="btnAgregarUbicacion_Click">
                                                <i class="imoon imoon-plus"></i>
                                                <span class="text">Agregar Ubicación</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <br />

                                        </div>
                                    </div>
                                </div>
                                <div class="widget-box">
                                    <div class="widget-content">
                                        <div class="control-group" id="divEstado" runat="server">
                                            <label for="ddlEstados" class="control-label">Estado solicitud de cambio:</label>
                                            <div class="controls">

                                                <asp:DropDownList ID="ddlEstados" runat="server"
                                                    AutoPostBack="true" Width="350px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label" style="display: none">Observaciones:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtObservacionesEst" runat="server" TextMode="MultiLine" Rows="5" Width="96%" CssClass="form-control" Visible="false"></asp:TextBox>

                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                                            <ContentTemplate>
                                                <div class="form-horizontal">
                                                    <div class="control-group">
                                                        <div id="Val_PartidaNueva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                            Revise las validaciones en pantalla.
                                                        </div>
                                                    </div>
                                                    <div id="pnlBotonesGuardar" class="control-groupp">
                                                        <div class="controls">

                                                            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" OnClick="btnGuardar_Click" OnClientClick="return validarGuardarNuevaPartida();">
                                                            <i class="imoon imoon-save"></i>
                                                            <span class="text">Guardar</span>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();" OnClick="btnCancelar_Click">
                                                            <i class="imoon imoon-blocked"></i>
                                                            <span class="text">Cancelar</span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnNuevaBusqueda" runat="server" CssClass="btn btn-default" OnClick="btnLimpiar_OnClick" OnClientClick="return newBusqueda();">
                                                            <i class="icon-refresh"></i>
                                                            <span class="text">Nueva Búsqueda</span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:UpdateProgress ID="UpdateProgress5" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                                Guardando...
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <%--Datos Partidas Horizontales--%>
        <div id="box_datos_horizon" style="display: none">
            <div class="col-sm-12 col-md-12">
                <asp:UpdatePanel ID="updDatosHorizon" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlDatosHorizon" runat="server" CssClass="form-horizontal">
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                    <h5>ABM Partidas Horizontales</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="control-group">
                                        <label class="control-label">Usuarios Intervinientes:</label>
                                        <div class="controls">
                                            <asp:UpdatePanel ID="updHistorialHori" runat="server">
                                                <ContentTemplate>

                                                    <asp:GridView ID="grdHistorialHori" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField DataField="UserNameHori" HeaderText="Usuario" />
                                                            <asp:BoundField DataField="ApenomHori" HeaderText="Apellido y Nombres" />
                                                            <asp:BoundField DataField="LastUpdateDateHori" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                                                HeaderText="Fecha de modif." />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="pad10">

                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">Aun sin usuarios intervinientes</span>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="control-group">

                                        <label class="control-label">Tipo de Solicitud:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriTipoSolicitud" Enabled="false" runat="server" CssClass="form-control" Width="250px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Número de Solicitud:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriNroSol" Enabled="false" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                    <h5>Datos de Partida Matriz</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="control-group">

                                        <label class="control-label">Nro. de Partida Matriz:</label>
                                        <div class="controls">

                                            <asp:TextBox ID="txtHoriNroPartidaM" Enabled="false" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Seccion</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriSeccion" Enabled="false" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Manzana:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriManzana" Enabled="false" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Parcela:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriParcela" Enabled="false" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Dirección:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriDireccion" Enabled="false" runat="server" CssClass="form-control" Width="80%" MaxLength="10"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                    <h5>Datos de Partida Horizontal</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="control-group">
                                        <label class="control-label">Nro. de Partida Horizontal:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriNroPartidaHor" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Piso:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriPiso" runat="server" CssClass="form-control" Width="180px" MaxLength="20"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">UF./Depto.:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriDepto" runat="server" CssClass="form-control" Width="180px" MaxLength="20"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Observaciones:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoriObservaciones" runat="server" TextMode="MultiLine" Rows="5" Width="96%" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Entidad Gubernamental:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="chbHoriEntidadGubernamental" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div id="ph_CamposReq" class="alert alert-danger mbottom0 mtop5 span5" style="display: none;">
                                            El campo Nro. de Partida Horizontal es requerido.
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="widget-box">
                                <div class="widget-content">
                                    <div runat="server" id="divObsH">
                                        <%-- <div class="control-group">
                                            <label for="ddlHoriEstadosHori" class="control-label">Estado solicitud de cambio:</label>
                                            <div class="controls">

                                                <asp:DropDownList ID="ddlHoriEstadosHori" runat="server"
                                                    AutoPostBack="true" Width="350px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>--%>
                                        <div runat="server" id="ObserEditHP">
                                            <div class="control-group">
                                                <label class="control-label">Observaciones:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtHoriObservacionesEst" runat="server" TextMode="MultiLine" Rows="5" Width="96%" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="updHoriBotonesGuardar" runat="server">
                                        <ContentTemplate>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <div id="Div5" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Revise las validaciones en pantalla.
                                                    </div>
                                                </div>


                                                <div id="pnlBotonesGuardarHori" class="control-groupp">
                                                    <div class="controls">

                                                        <asp:LinkButton ID="btnHoriGuardar" runat="server" CssClass="btn btn-primary" OnClick="btnHoriGuardar_Click" OnClientClick="return validarGuardarHori();">
                                                            <i class="imoon imoon-save"></i>
                                                            <span class="text">Guardar</span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="btnHoriCancelar" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();" OnClick="btnCancelar_Click">
                                                            <i class="imoon-blocked"></i>
                                                            <span class="text">Cancelar</span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnHoriNuevaBusqueda" runat="server" CssClass="btn btn-default" OnClick="btnLimpiar_OnClick" OnClientClick="return showBusqueda();">
                                                            <i class="icon-refresh"></i>
                                                            <span class="text">Nueva Búsqueda</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updHoriBotonesGuardar">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                            Guardando...
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <%-- Fin Datos Partidas Horizontales--%>
    </div>

    <%-- Agregar Ubicacion--%>
    <div id="frmAgregarUbicacion" class="modal fade" style="width: auto;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h5 class="modal-title">Agregar Ubicación</h5>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="updBodyAgregarUbicacion" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlagregarubicacionpartida" runat="server" DefaultButton="btnGuardarUbicacion" CssClass="form-horizontal">

                                <div class="control-group">
                                    <label class="control-label">Calle:</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlCalle" runat="server" Width="450px" CssClass="form-control"></asp:DropDownList>

                                        <div id="Req_txtCalle" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el nombre de la calle.
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Nro de Puerta:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNroPuerta" runat="server" Width="70px" MaxLength="5"></asp:TextBox>
                                        <div id="Req_txtNroPuerta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el número de puerta.
                                        </div>

                                    </div>
                                </div>
                                <div id="Req_txtNroPuertaParametro" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Por favor ingrese un número de puerta que se encuentre dentro del rango indicado para la calle.
                                </div>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="updBotonesGuardarUbicacion" runat="server">
                        <ContentTemplate>

                            <div class="form-inline">

                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="updBotonesGuardarUbicacion">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div id="pnlBotonesGuardarUbicacion" class="control-group">
                                    <asp:Button ID="btnGuardarUbicacion" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="btnGuardarUbicacion_Click"
                                        OnClientClick="return validarGuardarUbicacion();" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

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

    <%-- Historial de Ubicaciones por SMP --%>
    <div id="frmHistUbicacionSMP" class="modal fade" style="width: auto;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h5 class="modal-title">Historial Ubicación SMP</h5>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updHistUbiSMP" runat="server">
                        <ContentTemplate>
                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updBotonesCargarUbicacion">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>

                                <asp:Panel ID="pnlHistUbiSMP" runat="server" CssClass="form-horizontal">
                                    <%-- Ubicacion Actual --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Ubicación Actual</h5>
                                        </div>
                                        <div class="widget-content">
                                            <div class="control-group pleft20 ptop10 pright25">
                                                <asp:UpdatePanel ID="updUbiActual" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdUbiActual" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" Visible="false" />
                                                                <asp:BoundField DataField="PartidaMatriz" HeaderText="Partida Matriz" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha de Alta" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Creador" HeaderText="Creador" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="FechaUltimaActualizacion" HeaderText="Fecha de Ultima Actualización" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Actualizador" HeaderText="Actualizador" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="baja_logica" HeaderText="Dado de Baja" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="pad10">
                                                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                    <span class="mleft20">Sin resultados.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%-- Fin Ubicacion Actual --%>

                                    <%-- Ubicaciones Anteriores --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Ubicaciones Anteriores</h5>
                                        </div>
                                        <div class="widget-content">
                                            <div class="control-group pleft20 ptop10 pright25">
                                                <asp:UpdatePanel ID="updUbisAnteriores" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdUbisAnteriores" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" Visible="false" />
                                                                <asp:BoundField DataField="PartidaMatriz" HeaderText="Partida Matriz" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="baja_logica" HeaderText="Dado de Baja" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="TipoDeOperacion" HeaderText="Tipo de Operación" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="EstadoDeOperacion" HeaderText="Estado de Operación" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha de Operación" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="pad10">
                                                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                    <span class="mleft20">Sin resultados.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%-- Fin Ubicaciones Anteriores --%>

                                    <%-- Ubicaciones Posteriores --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Ubicaciones Posteriores</h5>
                                        </div>
                                        <div class="widget-content">
                                            <div class="control-group pleft20 ptop10 pright25">
                                                <asp:UpdatePanel ID="updUbisPosteriores" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdUbisPosteriores" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" Visible="false" />
                                                                <asp:BoundField DataField="PartidaMatriz" HeaderText="Partida Matriz" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="baja_logica" HeaderText="Dado de Baja" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="TipoDeOperacion" HeaderText="Tipo de Operación" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="EstadoDeOperacion" HeaderText="Estado de Operación" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha de Operación" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="pad10">
                                                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                    <span class="mleft20">Sin resultados.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%-- FIn Ubicaciones Posteriores --%>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <%-- Fin Historial de Ubicaciones por SMP --%>

    <%-- Cargar Ubicacion --%>
    <div id="frmCargarUbicacion" class="modal fade" style="width: auto;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h5 class="modal-title">Cargar Ubicación</h5>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updBotonesCargarUbicacion" runat="server">
                        <ContentTemplate>
                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="updBotonesCargarUbicacion">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div id="pnlBotonesCargarUbicacion" class="control-group">
                                    <asp:Button ID="btnNuevaUbicacion" runat="server" CssClass="btn btn-primary" Text="Nueva Ubicación" OnClick="btnNuevaUbicacion_Click" />
                                    <asp:Button ID="btnUnificarUbicacion" runat="server" CssClass="btn btn-primary" Text="Unificar Ubicación" OnClick="btnUnificarUbicacion_Click" />
                                    <asp:Button ID="btnSubdividirUbicacion" runat="server" CssClass="btn btn-primary" Text="Fraccionar Ubicación" OnClick="btnSubdividirUbicacion_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <%-- Fin Cargar Ubicacion --%>

    <style type="text/css">
        .FixedHeader {
            top: expression(document.getElementById("grdResultadosh").scrollTop-2);
            left: expression(parentNode.scrollLeft);
            position: absolute;
            z-index: 20;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_controles();
            init_Js_updpnlBuscar();
            $("#page_content").hide();
            $("#Loading").show();
            $("#filtros").show("slow");
            $("#<%: btnCargarDatos.ClientID %>").click();

            init_Js_updPnlResultadoBuscar();
            init_Js_updDatos();
            init_Js_updDatosHorizon();
            init_Js_updPnlFiltroBuscar_ubi_especial();
        });

        function init_Js_updPnlFiltroBuscar_ubi_especial() {
            $("#<%: ddlUbiTipoUbicacion.ClientID %>").select2({
                allowClear: true,
                placeholder: 'Seleccione ubicación',
                minimumInputLength: 0
            });

            $("#<%: ddlUbiSubTipoUbicacion.ClientID %>").select2({
                allowClear: true,
                placeholder: 'Seleccione subtipo de ubicación',
                minimumInputLength: 0
            });
        }

        function init_Js_updDatosHorizon() {
            $('#<%=txtHoriNroPartidaHor.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });

<%--            $("#<%: txtHoriNroPartidaHor.ClientID %>").on("keydown", function () {
                $("#ph_CamposReq").hide();
            });

            $("#<%: txtHoriPiso.ClientID %>").on("keydown", function () {
                $("#ph_CamposReq").hide();
            });

            $("#<%: txtHoriDepto.ClientID %>").on("keydown", function () {
                $("#ph_CamposReq").hide();
            });--%>

        }

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function init_Js_updPnlResultadoBuscar() {

            // Popovers horizontales
            $("[id*='MainContent_grdResultados_lnkHorizontales_']").each(function () {
                var id_pnlHorizontales = $(this).attr("id").replace("MainContent_grdResultados_lnkHorizontales_", "MainContent_grdResultados_pnlHorizontales_");
                var objHorizontales = $("#" + id_pnlHorizontales).html();
                $(this).popover({ title: 'Horizontales', content: objHorizontales, html: 'true' });

            });

            $("#Val_CallePuerta").hide();
            $("#Val_TipoSubtipo").hide();
            $("#ValFields").hide();
            return false;
        }


        function init_Js_updpnlBuscar() {
            $("#<%: ddlUbiCalle.ClientID %>").select2({
                allowClear: true
            });
            $("#Val_CallePuerta").hide();
            $("#Val_TipoSubtipo").hide();
            $("#ValFields").hide();

            return false;
        }

        function ocultarBotonesGuardarUbicacion() {
            $("#pnlBotonesGuardarUbicacion").hide();
            return false;
        }

        function init_Js_updUbicacion() {
            toolTips();
            return false;
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function showfrmAgregarUbicacion() {

            $("#frmAgregarUbicacion").modal("show");
            return false;
        }

        function hidefrmAgregarUbicacion() {

            $("#frmAgregarUbicacion").modal("hide");
            $("#Req_Ubicaciones").hide();
            return false;
        }

        function showfrmCargarUbicacion() {

            $("#frmCargarUbicacion").modal("show");
            return false;
        }

        function hidefrmCargarUbicacion() {

            $("#frmCargarUbicacion").modal("hide");
            return false;
        }

        function showfrmAdvertencia() {

            $("#frmAdvertencia").modal("show");
            return false;
        }

        function hidefrmAdvertencia() {

            $("#frmAdvertencia").modal("hide");
            return false;
        }

        function showfrmHistUbicacionSMP() {

            $("#frmHistUbicacionSMP").modal("show");
            return false;
        }

        function hidefrmHistUbicacionSMP() {

            $("#frmHistUbicacionSMP").modal("hide");
            return false;
        }

        function inicializar_controles() {
            camposAutonumericos();

        }

        function inicializar_popover() {

        }

        function popoverprueba(obj) {

            $(this).popover("show");

            return false;
        }

        function popOverHorizontales(obj) {
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
        function armarUrl() {
            var valor = "~/Reportes/ExportarElevadoresExcel.aspx";
            return valor;
        }


        function camposAutonumericos() {
            $('#<%=txtHoriPiso.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99' });
            $('#<%=txtHoriNroPartidaHor.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtUbiSeccion.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtUbiNroPartida.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtUbiNroPuerta.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '999999' });
        }
        function validarBuscar() {
            $("#Val_CallePuerta").hide();
            $("#Val_TipoSubtipo").hide();
            $("#ValFields").hide();
            var ret = true;
            //ddlUbiTipoUbicacion
            if ($("#buscar_ubi_por_dom").css("display") != "none") {

                var calle = $('#<%: ddlUbiCalle.ClientID %>').val();
                var nropuerta = $('#<%: txtUbiNroPuerta.ClientID %>').val();
                if (calle.length == 0 && nropuerta.length > 0) {
                    $("#Val_CallePuerta").css("display", "inline-block");
                    ret = false;
                }
            }
            if ($("#buscar_ubi_por_especial").css("display") != "none") {

                var tipoU = $('#<%: ddlUbiTipoUbicacion.ClientID %>').val();
                var subtipoU = $('#<%: ddlUbiSubTipoUbicacion.ClientID %>').val();
                if (tipoU == 0 || subtipoU == 0) {
                    $("#Val_TipoSubtipo").css("display", "inline-block");
                    ret = false;
                }
            }


            if ($("#MainContent_txtUbiNroPartida").val().length == 0 &&
                $("#MainContent_ddlUbiCalle").val().length == 0 &&
                $("#MainContent_txtUbiNroPuerta").val() == 0 &&
                $("#MainContent_txtUbiSeccion").val() == 0 &&
                $("#MainContent_txtUbiManzana").val() == 0 &&
                $("#MainContent_txtUbiParcela").val().length == 0 &&
                $("#MainContent_txtUbiParcela").val().length == 0 &&
                $("#MainContent_txtUbiParcela").val().length == 0 &&
                $("#MainContent_ddlUbiTipoUbicacion").val() == 0
            ) {
                $("#ValFields").css("display", "inline-block");
                ret = false;
            }
            hideSummary();


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

        function switchear_buscar_partida(opt) {
            if (opt == 1) {
                $("#MainContent_divDadaDBaja").show();
                $("#MainContent_divPhDadaDBaja").hide();
                $("#<%: ddlPHDadaDBaja.ClientID %>").attr('selected', 'selected');
            }
            else if (opt == 2) {
                $("#MainContent_divDadaDBaja").hide();
                $("#MainContent_divPhDadaDBaja").show();
                $("#<%: ddlBaja.ClientID %>").attr('selected', 'selected');
            }
    }

    function switchear_buscar_ubicacion(btn) {

        if (btn == 1) {
            $("#buscar_ubi_por_partida").show();
            $("#buscar_ubi_por_dom").hide();
            $("#buscar_ubi_por_smp").hide();
            $("#buscar_ubi_por_especial").hide();

            $("#btnBuscarPorPartida").addClass("active");
            $("#btnBuscarPorDom").removeClass("active");
            $("#btnBuscarPorSMP").removeClass("active");
            $("#btnBuscarPorUbiEspecial").removeClass("active");

        }
        else if (btn == 2) {
            $("#buscar_ubi_por_partida").hide();
            $("#buscar_ubi_por_dom").show();
            $("#buscar_ubi_por_smp").hide();
            $("#buscar_ubi_por_especial").hide();

            $("#btnBuscarPorPartida").removeClass("active");
            $("#btnBuscarPorDom").addClass("active");
            $("#btnBuscarPorSMP").removeClass("active");
            $("#btnBuscarPorUbiEspecial").removeClass("active");
        }
        else if (btn == 3) {
            $("#buscar_ubi_por_partida").hide();
            $("#buscar_ubi_por_dom").hide();
            $("#buscar_ubi_por_smp").show();
            $("#buscar_ubi_por_especial").hide();

            $("#btnBuscarPorPartida").removeClass("active");
            $("#btnBuscarPorDom").removeClass("active");
            $("#btnBuscarPorSMP").addClass("active");
            $("#btnBuscarPorUbiEspecial").removeClass("active");
        }
        else if (btn == 4) {
            $("#buscar_ubi_por_partida").hide();
            $("#buscar_ubi_por_dom").hide();
            $("#buscar_ubi_por_smp").hide();
            $("#buscar_ubi_por_especial").show();

            $("#btnBuscarPorPartida").removeClass("active");
            $("#btnBuscarPorDom").removeClass("active");
            $("#btnBuscarPorSMP").removeClass("active");
            $("#btnBuscarPorUbiEspecial").addClass("active");
        }

        var opc = '' + btn;

        $("#<%: hiddenTipoBusq.ClientID %>").val(opc);
        }
        function showResultado() {
            $("#box_resultado").show("slow");
        }
        function hideResultado() {
            $("#box_resultado").hide("slow");
            $("#boxActivas").hide("slow");

        }

        function showDatos() {
            $("#box_datos_horizon").hide("slow");
            $("#boxActivas").hide("slow");
            $("#filtros").hide("slow");
            $("#box_datos").show("slow");
        }
        function showDatosHorizon() {
            $("#box_datos").hide("slow");
            $("#boxActivas").hide("slow");
            $("#filtros").hide("slow");
            $("#box_datos_horizon").show("slow");
        }
        function showActivas() {
            $("#box_resultado").hide("slow");
            $("#box_datos").hide("slow");
            $("#box_datos_horizon").hide("slow");
            $("#boxActivas").show("slow");
            $("#filtros").show("slow");

        }

        function newBusqueda() {
            $("#boxActivas").hide("slow");
            $("#box_resultado").hide("slow");
            $("#box_datos").hide("slow");
            $("#box_datos_horizon").hide("slow");
            $("#filtros").show("slow");
        }
        function showBusqueda() {
            $("#boxActivas").hide("slow");
            $("#box_datos").hide("slow");
            $("#box_datos_horizon").hide("slow");
            $("#filtros").show("slow");
        }
        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }
        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function validarGuardar() {
            var ret = true;
            hideSummary();

            $("#Req_txtCalle1").hide();
            $("#Req_txtNroPuerta1").hide();


            if (ret) {
                ocultarBotonesGuardar();
            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }
        function validarGuardarHori() {
            var ret = false;

            if ($("#<%: txtHoriNroPartidaHor.ClientID %>").val().length > 0){
                ret = true;
            }
            if (ret) {
                ocultarBotonesGuardarHori();
            }
            else {
                $("#ph_CamposReq").css("display", "inline-block");
            }
            return ret;
        }

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }
        function ocultarBotonesGuardarHori() {
            $("#pnlBotonesGuardarHori").hide();
            return false;
        }

        function init_Js_updDatos() {
            $("#<%: txtNroPartida.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $("#<%: txtSeccion.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });


            $("#<%: txtSeccion.ClientID %>").on("keyup", function () {
                $("#Req_Seccion").hide();
                hideSummary();
            });
        }
    </script>
</asp:Content>

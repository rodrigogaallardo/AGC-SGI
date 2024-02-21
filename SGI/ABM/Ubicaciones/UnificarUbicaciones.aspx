<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnificarUbicaciones.aspx.cs" Inherits="SGI.ABM.Ubicaciones.UnificarUbicaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

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

    <div id="page_content">
        <hgroup class="title">
            <h1>Unificar Ubicaciones</h1>
            <h1><%: Title %>.</h1>
        </hgroup>
        <div id="box_datos">
            <div class="col-sm-12 col-md-12">
                <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_id_ubis_padres" runat="server" />
                        <asp:HiddenField ID="hid_id_ubi_nva" runat="server" />
                        <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                        <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                            <%-- Grillas Parcelas --%>
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                    <h5>Ubicación</h5>
                                </div>
                                <div class="widget-content">

                                    <%-- Grilla Parcela A Subdividir --%>
                                    <asp:UpdatePanel ID="updGridParcelasAUnificar" style="margin-top: 10px" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlParcelasAUnificar" runat="server" Visible="true">
                                                <div class="text-left">
                                                    <h5 style="width: 100%; float: left">Ubicaciones a unificar</h5>
                                                </div>
                                                <br />
                                                <asp:GridView ID="grdParcelasAUnificar" runat="server" AutoGenerateColumns="false" Visible="true"
                                                    GridLines="None" CssClass="table table-bordered" SelectMethod="GetResultadosUnif" AllowPaging="true">
                                                    <Columns>
                                                        <asp:BoundField DataField="partida" HeaderText="Partida Matriz" ItemStyle-Width="80px" HeaderStyle-ForeColor="#0088cc" ItemStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                        <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                        <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                        <asp:BoundField DataField="direccion" HeaderText="Domicilio" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="60%" />
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local"
                                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnEliminar_Click">                                                    
                                        <i class="imoon imoon-remove fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
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
                                                <br />
                                                <div class="pull-right" style="margin-bottom: 20px">
                                                    <asp:LinkButton ID="btnAgregarParAUnif" runat="server" CssClass="btn btn-success" OnClick="btnAgregarParAUnif_Click">
                            <i class="icon-white icon-search"></i>
                            <span class="text">Elegir Ubicación</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <br />
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%-- Fin Grilla Parcela A Subdividir --%>

                                    <%-- Grilla Parcelas Subdivididas --%>
                                    <asp:UpdatePanel ID="updGridParcelaNueva" style="margin-top: 10px" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlParcelaNueva" runat="server" Visible="true">
                                                <div class="text-left">
                                                    <h5 style="width: 100%; float: left">Ubicación unificada</h5>
                                                </div>
                                                <br />
                                                <asp:GridView ID="grdParcelaNueva" runat="server" AutoGenerateColumns="false" Visible="true"
                                                    GridLines="None" CssClass="table table-bordered" SelectMethod="GetResultado" AllowPaging="true">
                                                    <Columns>
                                                        <asp:BoundField DataField="partida" HeaderText="Partida Matriz" ItemStyle-Width="80px" HeaderStyle-ForeColor="#0088cc" ItemStyle-CssClass="text-center" />
                                                        <asp:BoundField DataField="seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                        <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                        <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                        <asp:BoundField DataField="direccion" HeaderText="Domicilio" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="60%" />
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnEditarNva" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local"
                                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnEditarNva_Click">                                                    
                                                                    <i class="imoon imoon-pencil2 fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnEliminarNva" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local"
                                                                    CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnEliminarNva_Click">                                                    
                                                                    <i class="imoon imoon-remove fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>
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
                                                <br />
                                                <div class="pull-right" style="margin-bottom: 20px">
                                                    <asp:LinkButton ID="btnAgregarParNueva" runat="server" CssClass="btn btn-success" OnClick="btnAgregarParNueva_Click">
                                                        <span class="text">Agregar Ubicación</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <br />
                                                <br />
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%-- Fin Grilla Parcelas Subdivididas --%>
                                </div>
                            </div>
                            <%-- Fin Grillas Parcelas --%>
                            <%-- Botones --%>
                            <div class="widget-box">
                                <div class="widget-content">
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
                                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" OnClientClick="return validarGuardar();" OnClick="btnGuardar_Click">
                                    <i class="imoon imoon-save"></i>
                                    <span class="text">Guardar</span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClick="btnCancelar_Click">
                                    <i class="imoon imoon-blocked"></i>
                                    <span class="text">Cancelar</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
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
                            <%-- Fin Botones --%>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%-- Fin Content --%>

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


    <%-- Parcelas a Unificar - Agregar Ubicación --%>
    <div id="frmAgregarUbicacion" class="modal fade" style="width: auto;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h5 class="modal-title">Agregar Ubicación</h5>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updAgregarUbicacion" runat="server">
                        <ContentTemplate>
                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="updAgregarUbicacion">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div id="filtros">
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
                                                                <asp:Button ID="btnCargarDatosDom" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
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
                                                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-default" OnClientClick="return;" OnClick="btnLimpiar_Click">
                                                        <i class="icon-refresh"></i>
                                                        <span class="text">Limpiar</span>
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
                                                                <asp:BoundField DataField="seccion" HeaderText="Seccion" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                                <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                                <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="20px" />
                                                                <asp:BoundField DataField="direccion" HeaderText="Domicilio" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="60%" />
                                                                <asp:BoundField DataField="baja_logica" HeaderText="Dada de baja" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="45px" />
                                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="70px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnAgregarUbicacion" runat="server" CssClass="btn btn-success" Visible="true"
                                                                            CommandArgument='<%#Eval("id_ubicacion")%>' OnClick="btnAgregarUbicacion_Click">                                                    
													                        <%--<i class="imoon imoon-eye-open fs16" style="margin-right:3px;margin-left:3px;color:#337AB7"></i>--%>
                                                                            <span class="text">Agregar</span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="pad10">
                                                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                    <span class="mleft20">No se encontraron trámites con los filtros ingresados.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                        <br />
                                                    </asp:Panel>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <%--fin resultados busqueda --%>
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
    <%-- Fin Parcelas a Unificar - Agregar Ubicación --%>

    <%-- Parcela Nueva - Agregar Ubicaciones --%>
    <div id="frmNuevaUbicacion" class="modal fade" style="width: auto;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h5 class="modal-title">Agregar Ubicación</h5>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updNuevaUbicacion" runat="server">
                        <ContentTemplate>
                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="updNuevaUbicacion">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <asp:HiddenField ID="hid_id_ubicacion" runat="server" />
                                <asp:HiddenField ID="hid_id_tipo_ubicacion" runat="server" />
                                <asp:HiddenField ID="hid_tipo_reqSMP" runat="server" />
                                <asp:Button ID="btnDatosNva" runat="server" OnClick="btnDatosNva_Click" Style="display: none" />
                                <asp:Panel ID="pnlDatosNva" runat="server" CssClass="form-horizontal">
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>ABM Partidas</h5>
                                        </div>
                                        <div class="widget-content">
                                            <div class="form-horizontal">
                                                <div>
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
                                                <div>
                                                    <div class="control-group">
                                                        <label class="control-label">Manzana:</label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtManzana" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div class="control-group">
                                                        <label class="control-label">Parcela:</label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtParcela" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div class="control-group">
                                                        <label class="control-label">Nro. de Partida Matriz:</label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtNroPartida" runat="server" CssClass="form-control" Width="150px" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label for="ddlUbiTipoUbicacionABM" class="control-label">Tipo de Ubicación:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlUbiTipoUbicacionABM" runat="server"
                                                            OnSelectedIndexChanged="ddlUbiTipoUbicacionABM_SelectedIndexChanged"
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
                                                    <div id="Req_Subtipo" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe ingresar el Subtipo.
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
                                                <div class="control-group">
                                                    <label class="control-label">Comuna:</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlComuna" runat="server" />
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
                                        </div>
                                    </div>
                                    <%-- Calles --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Calles y puertas</h5>
                                        </div>
                                        <div class="widget-content">
                                            <div class="control-group pleft20 ptop10 pright25">
                                                <asp:UpdatePanel ID="updCalleNva" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdCalleNva" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None"
                                                            OnRowDataBound="grdCalleNva_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="calles" HeaderText="Calle" />
                                                                <asp:BoundField DataField="nroPuerta" HeaderText="Nro. Puerta" />
                                                                <asp:BoundField DataField="codigo_calle" HeaderText="CodigoCalle" />
                                                                <asp:TemplateField ItemStyle-Width="60px" ItemStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnEliminarCalleNva" runat="server" title='Eliminar' data-toggle="tooltip" OnClick="btnEliminarCalleNva_Click">
                                                                    <i class="imoon imoon-remove fs16 color-black"></i>        
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="pad10">
                                                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                    <span class="mleft20">No se encontraron Calles.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="form-inline">
                                                    <div class="control-group text-left inline-block">
                                                        <div id="Req_CalleNva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                            Debe ingresar al menos una calle.
                                                        </div>
                                                    </div>
                                                    <div class="control-group pull-right">
                                                        <asp:LinkButton ID="btnAgregarCalleNva" runat="server" CssClass="btn btn-default" OnClick="btnAgregarCalleNva_Click">
                                                <i class="imoon imoon-plus"></i>
                                                <span class="text">Agregar Calle</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%-- Fin Calles --%>
                                    <%-- Mixturas --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Mixturas</h5>
                                        </div>
                                        <div class="widget-content">
                                            <fieldset>
                                                <div class="control-group">
                                                    <asp:Label ID="lblMixtura" runat="server" AssociatedControlID="ddlMixtura"
                                                        CssClass="control-label">Mixtura:</asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlMixtura" runat="server" Width="500px" AutoPostBack="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </fieldset>
                                            <div class="control-group pleft20 ptop10 pright25">
                                                <asp:UpdatePanel ID="updMixturas" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdMixturas" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None"
                                                            OnRowDataBound="grdMixturas_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="mix" HeaderText="Mixtura" />
                                                                <asp:BoundField DataField="mixDescripcion" HeaderText="Descripción" />
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
                                                                    <span class="mleft20">No se encontraron Mixturas.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="form-inline">
                                                    <div class="control-group text-left inline-block">
                                                        <div id="Req_MixturaNva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                            Debe ingresar al menos una mixtura.
                                                        </div>
                                                    </div>
                                                    <div class="control-group pull-right">
                                                        <asp:LinkButton ID="btnAgregarMixtura" runat="server" CssClass="btn btn-default" OnClick="btnAgregarMixtura_Click">
                                                <i class="imoon imoon-plus"></i>
                                                <span class="text">Agregar Mixtura</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%-- Fin Mixturas --%>
                                    <%-- Distritos --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Distritos</h5>
                                        </div>
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
                                                                    <span class="mleft20">No se encontraron Distritos.</span>
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                        <div class="form-inline">
                                                            <div class="control-group text-left inline-block">
                                                                <div id="Req_DistritoNva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar al menos un distrito.
                                                                </div>
                                                            </div>
                                                            <div class="control-group pull-right">
                                                                <asp:LinkButton ID="btnAgregarDistrito" runat="server" CssClass="btn btn-default" OnClick="btnAgregarDistrito_Click">
                                                <i class="imoon imoon-plus"></i>
                                                <span class="text">Agregar Distrito</span>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <%-- FIn Distritos --%>
                                    <div class="widget-box">
                                        <div class="widget-content">
                                            <asp:UpdatePanel ID="updBotonesGuardarNva" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-horizontal">
                                                        <div class="control-group">
                                                            <div id="Val_PartidaNuevaNva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                Revise las validaciones en pantalla.
                                                            </div>
                                                        </div>
                                                        <div id="pnlBotonesGuardarNva" class="control-groupp">
                                                            <div class="controls">
                                                                <asp:LinkButton ID="btnGuardarNva" runat="server" CssClass="btn btn-primary" OnClientClick="return validarGuardarNuevaPartida();" OnClick="btnGuardarNva_Click">
                                                            <i class="imoon imoon-save"></i>
                                                            <span class="text">Guardar</span>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnCancelarNva" runat="server" CssClass="btn btn-default" OnClick="btnCancelarNva_Click">
                                                            <i class="imoon imoon-blocked"></i>
                                                            <span class="text">Cancelar</span>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:UpdateProgress ID="UpdateProgress5" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardarNva">
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <%-- Fin Parcela Nueva - Agregar Ubicaciones --%>

    <%-- Parcela Nueva - Agregar Calle --%>
    <div id="frmAgregarCalleNva" class="modal fade" style="width: auto;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h5 class="modal-title">Agregar Calle</h5>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updAgregarCalleNva" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlagregarcalleNva" runat="server" DefaultButton="btnGuardarCalleNva" CssClass="form-horizontal">

                                <div class="control-group">
                                    <label class="control-label">Calle:</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlCalleNva" runat="server" Width="450px" CssClass="form-control"></asp:DropDownList>

                                        <div id="Req_txtCalleNva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el nombre de la calle.
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Nro de Puerta:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNroPuertaNva" runat="server" Width="70px" MaxLength="5"></asp:TextBox>
                                        <div id="Req_txtNroPuertaNva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar el número de puerta.
                                        </div>
                                    </div>
                                </div>
                                <div id="Req_txtNroPuertaNvaParametro" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Por favor ingrese un número de puerta que se encuentre dentro del rango indicado para la calle.
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="updBotonesGuardarCalleNva" runat="server">
                        <ContentTemplate>
                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="updBotonesGuardarCalleNva">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div id="pnlBotonesGuardarCalleNva" class="control-group">
                                    <asp:Button ID="btnGuardarCalleNva" runat="server" CssClass="btn btn-primary" Text="Aceptar"
                                        OnClientClick="return validarGuardarCalleNva();" OnClick="btnGuardarCalleNva_Click" />
                                    <asp:Button ID="btnCancelCalleNva" runat="server" CssClass="btn btn-default" Text="Cancelar"
                                        OnClick="btnCancelCalleNva_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%-- Fin Parcela Nueva - Agregar Calle --%>

    <%-- CSS --%>
    <style type="text/css">
        .FixedHeader {
            top: expression(document.getElementById("updGridParcelasAUnificar").scrollTop-2);
            left: expression(parentNode.scrollLeft);
            position: absolute;
            z-index: 20;
        }
    </style>
    <%-- Fin CSS --%>

    <%-- Scripts --%>
    <script type="text/javascript">
        $(document).ready(function () {
            inicializar_controles();
            $("#page_content").hide();
            $("#Loading").show();
            $("#filtros").show("slow");
            $("#frmAgregarUbicacion").hide();
            $("#frmNuevaUbicacion").hide();
            $("#frmAgregarCalleNva").hide();
            $("#<%: btnCargarDatos.ClientID %>").click();
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

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function init_Js_updPnlResultadoBuscar() {
            $("#Val_CallePuerta").hide();
            $("#Val_TipoSubtipo").hide();
            $("#ValFields").hide();
            return false;
        }

        function init_Js_updDatos() {

            $("#<%: txtNroPartida.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $("#<%: txtSeccion.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });

            $("#<%: txtSeccion.ClientID %>").on("keyup", function () {
                $("#Req_Seccion").hide();
                hideSummary();
            });

            $("#<%: ddlDistritos.ClientID %>").select2({
                allowClear: true
            });
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

        function init_Js_updUbiAgregarUbicacion() {
            $("#<%: txtUbiNroPuerta.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });

            $("#<%: ddlUbiCalle.ClientID %>").select2({
                allowClear: true,
                placeholder: "Seleccione una calle",
                minimumInputLength: 2
            });

            $("#<%: txtUbiNroPuerta.ClientID %>").on("keyup", function () {
                $("#Req_txtNroPuertaParametro").hide();
            });

            return false;
        }

        function init_Js_updAgregarCalleNva() {
            $("#<%: txtNroPuertaNva.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });

            $("#<%: ddlCalleNva.ClientID %>").select2({
                allowClear: true,
                placeholder: "Seleccione una calle",
                minimumInputLength: 2
            });

            $("#<%: txtNroPuertaNva.ClientID %>").on("keyup", function () {
                $("#Req_txtNroPuertaParametro").hide();
            });

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

        function showfrmNuevaUbicacion() {
            $("#frmNuevaUbicacion").modal("show");
            return false;
        }

        function hidefrmNuevaUbicacion() {
            $("#frmNuevaUbicacion").modal("hide");
            return false;
        }

        function showfrmAgregarCalleNva() {

            $("#frmAgregarCalleNva").modal("show");
            return false;
        }

        function hidefrmAgregarCalleNva() {

            $("#frmAgregarCalleNva").modal("hide");
            $("#Req_Ubicaciones").hide();
            return false;
        }

        function inicializar_controles() {
            camposAutonumericos();
            //icializar_autocomplete();
            init_Js_updDatos();
        }

        function armarUrl() {
            //NavigateUrl = "~/Reportes/ExportarElevadoresExcel.aspx?vigencia_desde=<%#Eval("ddlVigenciaDesde")%>"
            var valor = "~/Reportes/ExportarElevadoresExcel.aspx";
            return valor;
        }

        function camposAutonumericos() {
<%--            $('#<%=txtHoriPiso.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99' });
            $('#<%=txtHoriNroPartidaHor.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });--%>
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
        }

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
            $("#boxActivas").hide("slow");
        }

        function showDatos() {
            $("#boxActivas").hide("slow");
            $("#filtros").hide("slow");
            $("#box_datos").show("slow");
        }

        function showActivas() {
            $("#box_resultado").hide("slow");
            $("#box_datos").hide("slow");
            $("#boxActivas").show("slow");
            $("#filtros").show("slow");
        }

        function newBusqueda() {
            $("#boxActivas").hide("slow");
            $("#box_resultado").hide("slow");
            $("#box_datos").hide("slow");
            $("#filtros").show("slow");
        }

        function showBusqueda() {
            $("#boxActivas").hide("slow");
            $("#box_datos").hide("slow");
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

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
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

        function validarGuardarCalleNva() {
            var ret = true;
            var calle;
            var Nroinicio;
            var Nrofinal;
            var Nropuerta;

            $("#Req_txtCalleNva").hide();
            $("#Req_txtNroPuertaNva").hide();
            $("#Req_txtNroPuertaNvaParametro").hide();

            if ($.trim($("#<%: ddlCalleNva.ClientID %> ").val()).length == 0) {
            $("#Req_txtCalleNva").css("display", "inline-block");
            ret = false;
        }
        else {
            calle = $("#<%= ddlCalleNva.ClientID %> :selected").text().split('[');
            Nroinicio = parseInt(calle[1].split(' - ')[0]);
            Nrofinal = parseInt(((calle[1].split(' - ')[1]).split(']')[0]));
            Nropuerta = parseInt($.trim($("#<%: txtNroPuertaNva.ClientID %>").val()));
        }

        if ($.trim($("#<%: txtNroPuertaNva.ClientID %>").val()).length == 0) {
                $("#Req_txtNroPuertaNva").css("display", "inline-block");
                ret = false;
            }
            if (Nropuerta >= Nrofinal || Nropuerta <= Nroinicio) {
                $("#Req_txtNroPuertaSubParametro").css("display", "inline-block");
                ret = false;
            }
            if (ret) {
                $("#updBotonesGuardarCalleNva").hide();
            }
            return ret;
        }

        function validarGuardarNuevaPartida() {
            var ret = true;

            $("#Val_PartidaNuevaNva").hide();
            $("#Req_Seccion").hide();
            $("#Req_Subtipo").hide();
            $("#Req_DistritoNva").hide();
            $("#Req_MixturaNva").hide();
            $("#Req_CalleNva").hide();

            <%--if ($.trim($("#<%: txtSeccion.ClientID %>").val()).length == 0 &&
            $.trim($("#<%: txtManzana.ClientID %>").val()).length == 0 &&
            $.trim($("#<%: txtParcela.ClientID %>").val()).length == 0 &&
            $("#<%: hid_tipo_reqSMP.ClientID %>").val() == "1") 
            {
            $("#Req_Seccion").css("display", "inline-block");
            ret = false;
              }else {
                ret = true;
            }--%>

            if ($.trim($("#<%: txtParcela.ClientID %>").val()).length == 0 &&
                $("#<%: hid_tipo_reqSMP.ClientID %>").val() == "1") {
                $("#Req_Seccion").css("display", "inline-block");
                ret = false;
            } else {
                ret = true;
            }

        if ($.trim($("#<%: ddlUbiSubTipoUbicacionABM.ClientID %> ").val()).length == 0
            && $("#<%: hid_tipo_reqSMP.ClientID %>").val() == "0") {
                $("#Req_Subtipo").css("display", "inline-block");
                ret = false;
            }

            //var rowCountdp = $("[id*=grdCalleNva] tr").length;
            //if (rowCountdp < 2) {
            //    $("#Req_CalleNva").css("display", "inline-block");
            //    ret = false;
            //}

            var rowCountdp = $("[id*=grdMixturas] tr").length;
            if (rowCountdp < 2) {
                $("#Req_MixturaNva").css("display", "inline-block");
                ret = false;
            }

            //var rowCountd = $("[id*=grdDistritos] tr").length;
            //if (rowCountd < 2) {
            //    $("#Req_DistritoNva").css("display", "inline-block");
            //    ret = false;
            //}

            if (!ret) {
                $("#Val_PartidaNuevaNva").css("display", "inline-block");
            }

            return ret;
        }
    </script>
    <%-- Fin Scripts --%>
</asp:Content>

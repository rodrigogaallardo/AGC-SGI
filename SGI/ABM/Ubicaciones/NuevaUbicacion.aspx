<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaUbicacion.aspx.cs" Inherits="SGI.ABM.Partidas.NuevaUbicacion" %>

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
    <div id="page_content" style="display: none">
        <div id="box_datos">
            <div class="col-sm-12 col-md-12">
                <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_id_ubicacion" runat="server" />
                        <asp:HiddenField ID="hid_id_tipo_ubicacion" runat="server" />
                        <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                        <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
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
                                                    <asp:TextBox ID="txtManzana" runat="server" CssClass="form-control" Width="150px" MaxLength="6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="control-group">
                                                <label class="control-label">Parcela:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtParcela" runat="server" CssClass="form-control" Width="150px" MaxLength="6"></asp:TextBox>
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
                            <div>
                                <%-- Ubicaciones --%>
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
                                                            <asp:BoundField DataField="codigo_calle" HeaderText="CodigoCalle" />
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
                                <%-- Fin Ubicaciones --%>
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
                                <%-- Observaciones --%>
                                <div class="widget-box">
                                    <div class="widget-title">
                                        <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                        <h5>Observaciones</h5>
                                    </div>
                                    <div class="widget-content">
                                        <fieldset>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <label class="control-label">Observaciones:</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" Columns="12" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                                <%-- Fin Observaciones --%>
                                <div class="widget-box">
                                    <div class="widget-content">
                                        <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                                            <ContentTemplate>
                                                <div class="form-horizontal">
                                                    <div id="Val_Req_Comun" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Para las ubicaciones comunes se requiere sección, manzana y parcela.
                                                    </div>
                                                    <div id="Val_Req_Especial" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Para las ubicaciones Especiales se requiere tipo y subtipo de ubicación.
                                                    </div>
                                                </div>
                                                <div id="pnlBotonesGuardar" class="control-groupp">
                                                    <div class="controls">
                                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" OnClientClick="return validarGuardarNuevaUbicacion();" OnClick="btnGuardar_Click">
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
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
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
                                    <div id="Req_txtUbicacion" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                        La ubicación no pudo ser encontrada.
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
                                        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="updBotonesGuardarUbicacion">
                                            <ProgressTemplate>
                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div id="pnlBotonesGuardarUbicacion" class="control-group">
                                        <asp:Button ID="btnGuardarUbicacion" runat="server" CssClass="btn btn-primary" Text="Aceptar"
                                            OnClientClick="return validarGuardarUbicacion();" OnClick="btnGuardarUbicacion_Click" />
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
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

     <%--modal Aviso OK--%>
    <div id="frmAviso" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Informaci&oacute;n</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-ok fs64" style="color: green"></label>
                            </td>
                            <td>
                                <div class="pad20">

                                    <asp:UpdatePanel ID="updLabelAviso" runat="server" class="form-group">
                                        <ContentTemplate>
                                            <asp:Label ID="lblAviso" runat="server" Style="color: Black"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>


                                </div>

                            </td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>                   
                    <asp:LinkButton runat="server" CssClass="btn btn-success" OnClick="btnContinuar">Continuar</asp:LinkButton>
                    
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            init_Js_updpnlBuscar();
            $("#page_content").hide();
            $("#Loading").show();
            $("#filtros").show("slow");
            $("#<%: btnCargarDatos.ClientID %>").click();

            init_Js_updDatos();
            init_Js_updBodyAgregarUbicacion();
            init_Js_updpnlBuscar();
        });

        //campos obligatorios SMP para las ubicaciones de parcela comun y los campos tipo y subtipo de ubicacion para las ubicaciones especiales.
        function validarGuardarNuevaUbicacion() {
            ret = false;
            if ($("#<%: ddlbiTipoUbicacionABM.ClientID %> :selected").val() == 0) {
                if ($("#<%: txtSeccion.ClientID %>").val().length > 0 && $("#<%: txtManzana.ClientID %>").val().length > 0 && $("#<%: txtParcela.ClientID %>").val().length > 0) {
                    ret = true;
                }
                else {
                    $("#Val_Req_Comun").css("display", "inline-block");
                    ret = false;
                }
            }
            else if ($("#<%: ddlbiTipoUbicacionABM.ClientID %> :selected").val() != 0 && $("#<%: ddlUbiSubTipoUbicacionABM.ClientID %> :selected").val() != 0) {
                ret = true;
            }
            else {
                $("#Val_Req_Especial").css("display", "inline-block");
                ret = false;
            }
            return ret;
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

        function init_Js_updBodyAgregarUbicacion() {
            $("#<%: txtNroPuerta.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });

            $("#<%: ddlCalle.ClientID %>").select2({
                allowClear: true,
                placeholder: "Seleccione una calle",
                minimumInputLength: 2
            });

            $("#<%: txtNroPuerta.ClientID %>").on("keyup", function () {
                $("#Req_txtNroPuertaParametro").hide();
            });

            return false;
        }

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function init_Js_updpnlBuscar() {
            $("#<%: ddlCalle.ClientID %>").select2({
                allowClear: true
            });
            $("#Val_CallePuerta").hide();
            $("#Val_TipoSubtipo").hide();
            $("#ValFields").hide();

            return false;
        }

        function showfrmAgregarUbicacion() {

            $("#frmAgregarUbicacion").modal("show");
            $("#Req_txtUbicacion").hide();
            return false;
        }

        function hidefrmAgregarUbicacion() {

            $("#frmAgregarUbicacion").modal("hide");
            $("#Req_Ubicaciones").hide();
            $("#Req_txtUbicacion").hide();
            return false;
        }

        function showErrorAgregarUbicacion() {

            $("#Req_txtUbicacion").show();
            return false;
        }

        function validarGuardarUbicacion() {
            var ret = true;
            var calle;
            var Nroinicio;
            var Nrofinal;
            var Nropuerta;

            $("#Req_txtCalle").hide();
            $("#Req_txtNroPuerta").hide();
            $("#Req_txtNroPuertaParametro").hide();

            if ($.trim($("#<%: ddlCalle.ClientID %> ").val()).length == 0) {
                $("#Req_txtCalle").css("display", "inline-block");
                ret = false;
            }
            else {
                calle = $("#<%= ddlCalle.ClientID %> :selected").text().split('[');
                Nroinicio = parseInt(calle[1].split(' - ')[0]);
                Nrofinal = parseInt(((calle[1].split(' - ')[1]).split(']')[0]));
                Nropuerta = parseInt($.trim($("#<%: txtNroPuerta.ClientID %>").val()));
            }

            if ($.trim($("#<%: txtNroPuerta.ClientID %>").val()).length == 0) {
                $("#Req_txtNroPuerta").css("display", "inline-block");
                ret = false;
            }
            if (Nropuerta > Nrofinal || Nropuerta < Nroinicio) {
                $("#Req_txtNroPuertaParametro").css("display", "inline-block");
                ret = false;
            }
            if (ret) {
                $("#pnlBotonesGuardarUbicacion").hide();
            }
            return ret;
        }
        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function showfrmAviso() {
            $("#frmAviso").modal("show");
            return false;
        }
    </script>
</asp:Content>

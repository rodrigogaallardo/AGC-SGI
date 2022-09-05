<%@ Page Title="Baja Solicitud" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BajaSolicitud.aspx.cs" Inherits="SGI.GestionTramite.BajaSolicitud" %>

<%@ Register Src="~/GestionTramite/Controls/ucCargaDocumentos.ascx" TagPrefix="uc" TagName="CargaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

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


    <asp:UpdatePanel ID="updCargaInicial" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hid_formulario_cargado" runat="server" Value="false" />
            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="page_content" style="display: none">

        <div id="box_busqueda">

            <asp:Panel ID="Panel1" class="widget-box" runat="server" DefaultButton="btnBuscar">
                <div class="widget-title">
                    <span class="icon"><i class="icon-search"></i></span>
                    <h5>B&uacute;squeda de Bajas Administrativas</h5>
                </div>
                <div class="widget-content">
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" CssClass="form-horizontal" DefaultButton="btnBuscar">
                                <div class="row">
                                    <div class="span4">
                                        <asp:Label ID="lblObservacion" runat="server" Text="Nro Solicitud:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSolicitudBus" runat="server" MaxLength="100" Width="80px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="span4">
                                        <asp:Label ID="lblTipoTramite" runat="server" AssociatedControlID="ddlTipoTramite"
                                            Text="Tipo Trámite:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTipoTramite_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="span3">
                                        <asp:Label ID="Label4" runat="server" AssociatedControlID="ddlMotivoBajaBus" Text="Motivo Baja:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlMotivoBajaBus" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="span5">
                                        <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtFechaDesde"
                                            Text="Fecha Baja Desde:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaDesde" runat="server"
                                                    ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaDesde" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="span5">
                                        <asp:Label ID="lblFechaHasta" runat="server" AssociatedControlID="txtFechaHasta"
                                            Text="Fecha Baja Hasta:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaHasta" runat="server"
                                                    ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaHasta" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="pull-right">
                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>

                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click">
                            <i class="icon-white icon-search"></i>
                            <span class="text">Buscar</span>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_Click">
                            <i class="icon-refresh"></i>
                            <span class="text">Reestablecer filtros por defecto</span>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnNuevaBaja" runat="server" CssClass="btn btn-success" OnClick="btnNuevaBaja_Click">
                            <i class="icon-white icon-plus"></i>
                            <span class="text">Nueva Baja Administrativa</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <%--Resultados --%>
            <div id="box_resultado" style="display: none">
                <div class="widget-box">
                    <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-horizontal" style="margin-left: 15px; margin-right: 15px">
                                <script type="text/javascript">
                                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                                    function endRequestHandler() {
                                        //inicializar_popover();
                                    }
                                </script>
                                <asp:Panel ID="pnlResultadoBuscar" runat="server">
                                    <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">
                                        <div style="display: inline-block; margin-left: 10px;">
                                            <h5>Lista de Bajas Administrativas</h5>
                                        </div>
                                        <div style="display: inline-block">
                                            (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>
                                            )
                                        </div>
                                    </asp:Panel>
                                    <asp:GridView ID="grdResultados"
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        CssClass="table table-bordered table-striped table-hover with-check"
                                        SelectMethod="GetResultados"
                                        AllowPaging="true"
                                        AllowSorting="true"
                                        PageSize="30"
                                        OnPageIndexChanging="grdResultados_PageIndexChanging"
                                        OnDataBound="grdResultados_DataBound"
                                        OnRowDataBound="grdResultados_RowDataBound">
                                        <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                                        <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud" ItemStyle-Width="150px" />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" ItemStyle-Width="150px" />
                                            <asp:BoundField DataField="motivo" HeaderText="Motivo Baja" ItemStyle-Width="50px" />
                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-Width="50px" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" ItemStyle-Width="350px" />
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkArchivoPdf" runat="server" CssClass="btn-link" Target="_blank" NavigateUrl='<%#Eval("url")%>'>
                                                        <i class="imoon imoon-file-pdf color-red fs16"></i>
                                                    </asp:HyperLink>
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
            </div>
        </div>
        <%--Abm--%>
        <div id="box_datos" class="widget-box" style="display: none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos de la Baja Administrativa</h5>
            </div>
            <div class="widget-content">
                <%-- Campos de Texto --%>
                <asp:UpdatePanel ID="updBaja" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <fieldset>
                                <div class="row-fluid">
                                    <div class="span8">
                                        <div class="control-group">
                                            <asp:Label ID="Label5" runat="server" AssociatedControlID="ddlTipoTramiteBaja" Text="Tipo de Tramite:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlTipoTramiteBaja" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span8">
                                        <div class="control-group">
                                            <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ReqSolicitud" runat="server" ValidationGroup="Reabrir"
                                                    ControlToValidate="txtSolicitud" Display="Dynamic" CssClass="field-validation-error"
                                                    ErrorMessage="Ingrese la Solicitud."></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span8">
                                        <div class="control-group">
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlMotivoBaja" Text="Motivo Baja:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlMotivoBaja" runat="server" Width="300px" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span8">
                                        <div class="control-group">
                                            <asp:Label ID="Label2" runat="server" Text="Observaciones:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Width="100%" Height="80px" MaxLength="2000"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <uc1:ucProcesosSADEv1 runat="server" ID="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />
            </div>

            <div id="box_documentos" class="widget-box">
                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-hammer"></i></span>
                    <h5>Documento</h5>
                </div>
                <div class="widget-content">
                    <div class="row">
                        <div class="col-sm-12 col-md-12">
                            <asp:UpdatePanel ID="updpnlGrillaDoc" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gridAgregados_db" runat="server" AutoGenerateColumns="false"
                                        AllowPaging="false" Style="border: none;" CssClass="table table-bordered mtop5"
                                        GridLines="None" Width="100%"
                                        DataKeyNames="id_tdocreq,tdocreq_detalle,nombre_tdocreq,id_file,rowid,nombre_archivo">
                                        <HeaderStyle CssClass="grid-header" />
                                        <RowStyle CssClass="grid-row" />
                                        <AlternatingRowStyle BackColor="#efefef" />
                                        <Columns>
                                            <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo de Documento" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="nombre_archivo" HeaderText="Nombre del archivo" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                            <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="Acciones" HeaderStyle-CssClass="text-center" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEliminar" title="Eliminar" runat="server"
                                                        CommandArgument='<%# Eval("rowid") %>'
                                                        OnClick="lnkEliminar_Click"
                                                        Width="70px">
                                                        <span class="icon"><i class="imoon imoon-trash fs24"></i></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="pull-right">
                                <asp:LinkButton ID="btnMostrarAgregadoDocumentos" runat="server" CssClass="btn btn-primary" OnClientClick="return DatosDocumentoAgregarToggle();">
                                    <i class="imoon-white imoon-chevron-down"></i>
                                    <span class="text">Agregar Documentos</span>
                                </asp:LinkButton>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="updpnlAgregarDocumentos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlAgregarDocumentos" runat="server">

                        <asp:HiddenField ID="hid_doc_id_solicitud" runat="server" />

                        <asp:Panel ID="pnlDatosDocumento" runat="server" Style="display: none">
                            <div id="Div1" class="accordion-group widget-box" style="background-color: #ffffff">
                                <div class="accordion-heading">
                                    <a id="A1" data-parent="#collapse-group" href="#collapse_documentosAdicionales"
                                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-file2" style="color: #344882"></i></span>
                                            <h5>
                                                <asp:Label ID="Label3" runat="server" Text="Carga de documentos"></asp:Label></h5>
                                            <span class="btn-right"><i class="imoon imoon-chevron-up" style="color: #344882"></i></span>
                                        </div>
                                    </a>
                                </div>
                                <div class="accordion-body collapse in" id="collapse_documentosAdicionales">
                                    <uc:CargaDocumentos runat="server" ID="CargaDocumentos" OnSubirDocumentoClick="CargaDocumentos_SubirDocumentoClick" />
                                </div>
                            </div>

                        </asp:Panel>
                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="accordion-body collapse in" id="boton">

                <%-- Boton para Actualizar --%>
                <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <div class="control-group">
                                <div id="ValSummary" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Revise las validaciones en pantalla.
                                </div>
                            </div>
                            <div id="pnlBotonesGuardar" class="control-groupp">
                                <div class="controls">
                                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-inverse" OnClientClick="return ConfirmarBaja();" OnClick="btnGuardar_Click">
                                        <i class="imoon-white imoon-save"></i>
                                        <span class="text">Guardar</span>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();">
                                        <i class="imoon-blocked"></i>
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
            $("#page_content").hide();
            $("#Loading").show();

            $("#<%: btnCargarDatos.ClientID %>").click();

        });

        function finalizarCargaBaja() {

            $("#Loading").hide();
            $("#page_content").show();

            return false;
        }


        // el script para mostrar errores debe estar arriba porque en caso de que de error en 
        // load page debe tener el script cargado desde el inicio
        function mostratMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
        function camposAutonumericos() {
            $('#<%=txtSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            $('#<%=txtSolicitudBus.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
            return false;
        }

        function inicializar_controles() {
            camposAutonumericos();
            inicializar_fechas();
            inicializar_ddl();
            return false;
        }

        function inicializar_ddl() {
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMotivoBajaBus.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTipoTramiteBaja.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlMotivoBaja.ClientID %>").select2({ allowClear: true });

        }

        function inicializar_fechas() {

            var fechaDesde = $('#<%: txtFechaDesde.ClientID %>');
            var es_readonly = $(fechaDesde).prop("readonly");

            $("#<%: txtFechaDesde.ClientID %>").datepicker({
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                    $("#Req_FechaDesde").hide();
                    $("#Val_Formato_FechaDesde").hide();
                    $("#Val_FechaDesdeMenor").hide();
                }
            });

            if (!($(fechaDesde).is('[disabled]') || $(fechaDesde).is('[readonly]'))) {
                $(fechaDesde).datepicker(
                    {
                        maxDate: "0",
                        closeText: 'Cerrar',
                        prevText: '&#x3c;Ant',
                        nextText: 'Sig&#x3e;',
                        currentText: 'Hoy',
                        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                                    'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                                        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                        weekHeader: 'Sm',
                        dateFormat: 'dd/mm/yy',
                        firstDay: 0,
                        isRTL: false,
                        showMonthAfterYear: false,
                        yearSuffix: ''

                    }
                    );
            };

            var fechaHasta = $('#<%=txtFechaHasta.ClientID%>');
            var es_readonly = $(fechaHasta).prop("readonly");
            if (!($(fechaHasta).is('[disabled]') || $(fechaHasta).is('[readonly]'))) {
                $(fechaHasta).datepicker({
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                                'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                                    'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 0,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''

                });
            };
            return false;
        }

        function showDatos() {
            camposAutonumericos();
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
            return false;
        }
        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
            return false;
        }
        function showResultados() {
            $("#box_resultado").show("slow");
            return false;
        }
        function hideResultados() {
            $("#box_resultado").hide("slow");
            return false;
        }
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
            return false;
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

        function DatosDocumentoAgregarToggle() {
            if ($("#<%: pnlDatosDocumento.ClientID %>").css("display") == "none") {
                $("#<%: pnlDatosDocumento.ClientID %>").show("slow");
                $("#<%: btnMostrarAgregadoDocumentos.ClientID %> i").removeClass('imoon-chevron-down');
                $("#<%: btnMostrarAgregadoDocumentos.ClientID %> i").addClass('imoon-chevron-up');
            }
            else {
                $("#<%: pnlDatosDocumento.ClientID %>").hide("slow");
                $("#<%: btnMostrarAgregadoDocumentos.ClientID %> i").removeClass('imoon-chevron-up');
                $("#<%: btnMostrarAgregadoDocumentos.ClientID %> i").addClass('imoon-chevron-down');

            }
            return false;
        }

        function ConfirmarBaja() {
            return confirm('¿Esta seguro que desea realizar la baja?');
        }
    </script>

</asp:Content>

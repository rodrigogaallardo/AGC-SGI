<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_Rubros.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tab_Rubros" %>

<%: Scripts.Render("~/bundles/autoNumeric") %>

<asp:UpdatePanel ID="Hiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_cpadron" runat="server" />
        <asp:HiddenField ID="hid_id_encomienda" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
        <asp:HiddenField ID="hid_editar" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>


<div id="page_content">

    <div id="titulo" runat="server" class="accordion-group widget-box">
        <h3 style="line-height: 20px;">Rubros o Actividades</h3>
    </div>


    <div id="box_infoRubros" class="accordion-group widget-box" runat ="server">

        <%-- titulo box Informacion--%>
        <div class="accordion-heading">
            <a id="ubicacion_btnUpDown" data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-info"></i></span>
                    <h5>Informaci&oacute;n ingresada en el tr&aacute;mite</h5>
                </div>
            </a>
        </div>
        <%-- contenido del box Informacion --%>


        <div class="accordion-body collapse in">
            <div class="widget-content">

                <%--Información ingresada en el trámite--%>
                <asp:UpdatePanel ID="updInformacionTramite" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3" style="width: 200px">Superficie total a habilitar:</label>
                                <asp:Label ID="lblSuperficieLocal" runat="server" Text="0" CssClass="control-label col-sm-9 text-left" Style="font-weight: bold !important"></asp:Label>
                                <asp:HiddenField ID="hid_Superficie_Local" runat="server" />

                            </div>

                            <div class="form-group">
                                <label class="control-label col-sm-3" style="width: 200px">Zonificaci&oacute;n Declarada:</label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlZonaDeclarada" runat="server" Width="500px" OnSelectedIndexChanged="ddlZonaDeclarada_SelectedIndexChanged" AutoPostBack="true" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>


                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <div class="accordion-group widget-box">

        <%-- titulo box Rubros--%>
        <div class="accordion-heading">
            <a id="A1" data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-hammer"></i></span>
                    <h5>Rubros</h5>
                </div>
            </a>
        </div>
        <%-- contenido del box Rubros --%>
        <div id="box_editarRubros" class="accordion-body collapse in" runat="server">
            <div class="widget-content">

                <div class="row mbottom10">
                    <div class="col-sm-12 text-right pright15">
                        <button id="btnAgregarRubros_Rubros" class="btn btn-success pbottom5" onclick="return showfrmAgregarRubros_Rubros();" data-group="controles-accion">
                            <i class="imoon imoon-plus"></i>
                            <span class="text">Agregar Rubro</span>
                        </button>
                        <button id="btnAgregarRubros_Nuevos" class="btn btn-info pbottom5" onclick="return showfrmAgregarRubroUsoNoContemplado();" data-group="controles-accion">
                            <i class="imoon imoon-plus"></i>
                            <span class="text">Agregar Rubros Uso No contemplado Normativa</span>
                        </button>
                    </div>
                </div>
                <asp:UpdatePanel ID="updRubros" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:GridView ID="grdRubrosIngresados" runat="server" AutoGenerateColumns="false"
                            AllowPaging="false" Style="border: none;" CssClass="table table-bordered mtop5"
                            GridLines="None" Width="100% ">
                            <HeaderStyle CssClass="grid-header" />
                            <RowStyle CssClass="grid-row" />
                            <AlternatingRowStyle BackColor="#efefef" />
                            <Columns>
                                <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TipoActividadNombre" HeaderText="Actividad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="TipoTamite" HeaderText="Tipo Trámite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />
                                <asp:BoundField DataField="EsAnterior" HeaderText="Anterior" Visible="false" />
                                <%-- <asp:BoundField DataField="SuperficieHabilitar" HeaderText="m2." ItemStyle-Width="70px" HeaderStyle-CssClass="text-center"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />--%>
                                <asp:TemplateField HeaderText="m2" HeaderStyle-CssClass="text-center" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <label><%# Eval("SuperficieHabilitar") %></label>
                                        <input id="txtSupHabNew" runat="server" type="text" style="display: none; width: 50%" value='<%# Eval("SuperficieHabilitar") %>' />
                                        <asp:LinkButton ID="lnkSupHabEdit" runat="server" ToolTip="Editar Superficie" OnClientClick="return EditarSuperficie(this);" data-group="controles-accion">
                                            <i class="imoon imoon-pencil2" style="color:#377bb5;font-size:medium;margin-left:5px"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkSupHabSave" runat="server" ToolTip="Guardar" Style="display: none" OnClientClick="return ValidarNuevaSuperficie(this);"
                                            OnClick="lnkSupHabSave_Click" data-rubro='<%# Eval("desc_rubro") + ";" + Eval("IdTipodocReq") + ";" + Eval("IdTipoActividad") %>' CommandName='<%# Eval("cod_rubro") %>' CommandArgument='<%# Eval("id_caarubro") %>'>
                                            <i class="imoon imoon-ok color-green" style="font-size:medium;"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkSupHabClose" runat="server" ToolTip="Cancelar" Style="display: none" OnClientClick="return CloseEditarSuperficie(this);">
                                            <i class="imoon imoon-close color-red" ></i>
                                        </asp:LinkButton>
                                        <asp:UpdateProgress ID="updprogRubro" runat="server" AssociatedUpdatePanelID="updRubros">
                                            <ProgressTemplate>
                                                <asp:Image ID="imgProgFinalizarTarea" runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif" CssClass="pull-right" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <div id="Val_Nueva_Superficie" class="field-validation-error" style="display: none">
                                            La superficie a habilitar debe ser un número entre 0 y la superficie total del local.
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="95px">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnEliminarRubro" runat="server" data-id-rubro-eliminar='<%# Eval("id_caarubro") %>' CssClass="link-local"
                                            OnClientClick="return showConfirmarEliminarRubro_Rubros(this);" data-group="controles-accion">
                                                <i class="imoon imoon-close"></i>
                                                <span class="text">Eliminar</span>
                                        </asp:LinkButton>

                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <EmptyDataTemplate>

                                <div class="mtop10">

                                    <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                    <span class="mleft10">No se encontraron registros.</span>

                                </div>

                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:HiddenField ID="hid_id_caarubro_eliminar" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <%-- contenido del box Rubros --%>
        <div id="box_MostrarRubros" class="accordion-body collapse in" runat="server">
            <div class="widget-content">


                <asp:GridView ID="grdRubrosMostrar" runat="server" AutoGenerateColumns="false"
                    AllowPaging="false" CssClass="table table-bordered mtop5"
                    GridLines="None" Width="100% ">
                    <HeaderStyle CssClass="grid-header" />
                    <RowStyle CssClass="grid-row" />
                    <AlternatingRowStyle BackColor="#efefef" />

                    <Columns>
                        <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="TipoActividadNombre" HeaderText="Actividad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="TipoTamite" HeaderText="Tipo Trámite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="SuperficieHabilitar" HeaderText="Superficie (m2)" ItemStyle-Width="90px" ItemStyle-CssClass="align-center" />


                    </Columns>
                    <EmptyDataTemplate>

                        <div class="mtop10">

                            <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                            <span class="mleft10">No se encontraron registros.</span>

                        </div>

                    </EmptyDataTemplate>
                </asp:GridView>

            </div>
        </div>

    </div>
</div>




<%--Modal form Agregar normativa--%>
<div id="frmAgregarNormativa_Rubros" class="modal fade" style="display: none;">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Agregar Normativa</h4>
            </div>
            <div class="modal-body pbottom5">
                <asp:UpdatePanel ID="updAgregarNormativa" runat="server">
                    <ContentTemplate>

                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3">Tipo de Normativa:</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlTipoNormativa" runat="server" Width="400px" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3">Entidad Normativa:</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlEntidadNormativa" runat="server" Width="400px" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3">N&uacute;mero Normativa:</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtNroNormativa" runat="server" MaxLength="15" Width="120px" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div id="ReqtxtNormativa_Rubros" class="form-group mbottom5" style="display: none">
                                <div class="col-sm-offset-3 col-sm-9">
                                    <div class="alert alert-danger mbottom0">
                                        <span>El N&uacute;mero de normativa es obligatorio.</span>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div class="modal-footer mtop5">

                <asp:UpdatePanel ID="updBotonesIngresarNormativa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="form-inline text-right">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress_updBotonesIngresarNormativa" AssociatedUpdatePanelID="updBotonesIngresarNormativa"
                                    runat="server" DisplayAfter="200">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>

                            <div class="form-group">
                                <asp:LinkButton ID="btnIngresarNormativa" runat="server" CssClass="btn btn-primary" OnClientClick="return validarAgregarNormativa_Rubros();">
                                        <i></i>
                                        <span class="text">Aceptar</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCerrarNormativa" runat="server" CssClass="btn btn-default" data-dismiss="modal">
                                        <i></i>
                                        <span class="text">Cancelar</span>
                                </asp:LinkButton>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>
<!-- /.modal -->


<%--Modal Confirmar Eliminar Normativa--%>
<div id="frmConfirmarEliminarNormativa_Rubros" class="modal fade" style="display: none;">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Eliminar Normativa</h4>
            </div>
            <div class="modal-body">
                <table style="border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="text-align: center; vertical-align: text-top">
                            <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                        </td>
                        <td style="vertical-align: middle">
                            <label class="mleft10">¿ Est&aacute; seguro de eliminar la Normativa ?</label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="modal-footer">

                <asp:UpdatePanel ID="updConfirmarEliminarNormativa" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updConfirmarEliminarNormativa">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesConfirmacionEliminarnormativa_Rubros" class="form-group">
                                <asp:Button ID="btnEliminarNormativa" runat="server" CssClass="btn btn-primary" Text="Sí" OnClientClick="ocultarBotonesConfirmacionEliminarNormativa_Rubros();" />
                                <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>
<!-- /.modal -->

<%--Modal Confirmar Eliminar Rubro--%>
<div id="frmConfirmarEliminarRubro_Rubros" class="modal fade" style="display: none;">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Eliminar Rubro</h4>
            </div>
            <div class="modal-body">
                <table style="border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="text-align: center; vertical-align: text-top">
                            <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                        </td>
                        <td style="vertical-align: middle">
                            <label class="mleft10">¿ Est&aacute; seguro de eliminar el Rubro ?</label>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="modal-footer">

                <asp:UpdatePanel ID="updConfirmarEliminarRubro" runat="server">
                    <ContentTemplate>

                        <div class="form-inline">
                            <div class="form-group">
                                <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="updConfirmarEliminarRubro">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <div id="pnlBotonesConfirmacionEliminarRubro_Rubros" class="form-group">
                                <asp:Button ID="btnEliminarRubro" runat="server" CssClass="btn btn-primary" Text="Sí" OnClick="btnEliminarRubro_Click" />
                                <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>
<!-- /.modal -->


<%--Modal form Agregar Rubros--%>
<div id="frmAgregarRubros_Rubros" class="modal fade" style="margin-left: -25%; width: 50%; display: none; max-height: 90%; overflow: auto">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Agregar Rubros</h4>
            </div>
            <div class="modal-body pbottom20">
                <asp:UpdatePanel ID="updBuscarRubros" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlBuscarRubros" runat="server" CssClass="form-horizontal" DefaultButton="btnBuscar">

                            <div class="form-group">
                                <h3 class="pleft20 col-sm-12">B&uacute;squeda de Rubros</h3>
                            </div>
                            <div class="row-fluid">
                                <label class="span3">Superficie del rubro:</label>
                                <div class="span1">
                                    <asp:TextBox ID="txtSuperficie" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="span8 pleft40">
                                    <div id="Req_Superficie_Rubros" class="alert alert-danger mbottom0" style="display: none">
                                        La superficie a habilitar debe ser un número entre 1 y la superficie total del local.
                                    </div>
                                </div>

                            </div>

                            <div class="row-fluid">
                                <label class="span3" style="margin-top: -1px">Ingrese el código o parte de la descipción del rubro a buscar:</label>
                                <div class="span9">
                                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-offset-3 col-sm-9 ptop5">
                                    <div id="Req_txtBuscar_Rubros" class="alert alert-danger mbottom0" style="display: none">
                                        Debe ingresar al menos 3 caracteres para iniciar la b&uacute;squeda.
                                    </div>
                                </div>
                            </div>

                            <hr class="mbottom0 mtop0" />


                            <asp:UpdatePanel ID="updBotonesBuscarRubros" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:Panel ID="pnlBotonesBuscarRubros" runat="server" CssClass="form-inline text-right">
                                        <div class="form-group">
                                            <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="updBotonesBuscarRubros"
                                                runat="server" DisplayAfter="200">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                    Buscando...
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>

                                        <asp:Panel ID="BotonesBuscarRubros" runat="server" CssClas="form-group">
                                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" Style="color: white" OnClick="btnBuscar_Click" OnClientClick="return validarBuscar_Rubros();">
                                                    <i class="imoon imoon-search"></i>
                                                    <span class="text">Buscar</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-default" data-dismiss="modal">
                                                    <i class="imoon imoon-close"></i>
                                                    <span class="text">Cerrar</span>
                                            </asp:LinkButton>
                                        </asp:Panel>
                                    </asp:Panel>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </asp:Panel>

                        <asp:Panel ID="pnlResultadoBusquedaRubros" runat="server" CssClass="form-horizontal" Style="display: none">

                            <div style="max-height: 500px; overflow-y: auto">

                                <asp:GridView ID="grdRubros" runat="server" AutoGenerateColumns="false" DataKeyNames="cod_rubro,EsAnterior,SuperficieHabilitar,PregAntenaEmisora"
                                    AllowPaging="false" Style="border: none;" CssClass="table table-bordered mtop5"
                                    GridLines="None" Width="100% ">
                                    <HeaderStyle CssClass="grid-header" />
                                    <RowStyle CssClass="grid-row" />
                                    <AlternatingRowStyle BackColor="#efefef" />
                                    <Columns>
                                        <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="nom_rubro" HeaderText="Descripción" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="TipoActividad" HeaderText="Actividad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px" />
                                        <%--<asp:BoundField DataField="cod_impactoambiental" HeaderText="Impacto Ambiental" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />--%>
                                        <asp:BoundField DataField="EsAnterior" HeaderText="Anterior" Visible="false" />
                                        <asp:BoundField DataField="SuperficieHabilitar" HeaderText="m2." ItemStyle-Width="70px" HeaderStyle-CssClass="text-center"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />

                                        <asp:TemplateField ItemStyle-Width="55px" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRubroElegido" runat="server" Enabled="true" onclick="ocultarValidadorAgregarRubros_Rubros();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>

                                                <asp:Panel ID="pnlAntenas" runat="server" Style="display: none" Width="150px">
                                                    <div>¿Posee Antena Emisora?</div>
                                                    <asp:RadioButton ID="optAntenaSI" runat="server" Text="Sí" GroupName="AntenaEmisora" />
                                                    <asp:RadioButton ID="optAntenasNO" runat="server" Text="No" GroupName="AntenaEmisora" Checked="true" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlBarrio" runat="server" Style="display: none">
                                                    <div>
                                                        Barrio donde se encuentra la vivienda
                                                    </div>
                                                    <asp:DropDownList ID="ddlBarrio" runat="server" Width="250px">
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataTemplate>

                                        <div class="mtop10">

                                            <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                            <span class="mleft10">No se encontraron registros.</span>

                                        </div>

                                    </EmptyDataTemplate>
                                </asp:GridView>

                            </div>

                            <asp:UpdatePanel ID="updBotonesAgregarRubros" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:Panel ID="pnlGrupoAgregarRubros" runat="server" CssClass="row ptop10 pleft10 pright10" Style="display: none">
                                        <div class="span2">

                                            <asp:LinkButton ID="btnnuevaBusqueda" runat="server" CssClass="btn btn-default" OnClick="btnnuevaBusqueda_Click">
                                                    <i class="imoon imoon-search"></i>
                                                    <span class="text">Nueva B&uacute;squeda</span>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="span7 pleft20">

                                            <asp:UpdatePanel ID="updValidadorAgregarRubros" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="ValidadorAgregarRubros" runat="server" CssClass="alert alert-danger mbottom0" Style="display: none">
                                                        <asp:Label ID="lblValidadorAgregarRubros" runat="server"></asp:Label>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>



                                        <asp:Panel ID="pnlBotonesAgregarRubros" runat="server" CssClass="col-sm-3 text-right">

                                            <asp:UpdateProgress ID="UpdateProgress4" AssociatedUpdatePanelID="updBotonesAgregarRubros"
                                                runat="server" DisplayAfter="200">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                    Procesando...
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>


                                            <div id="BotonesAgregarRubros_Rubros">
                                                <asp:LinkButton ID="btnIngresarRubros" runat="server" CssClass="btn btn-primary" OnClick="btnIngresarRubros_Click" OnClientClick="ocultarBotonesAgregarRubros_Rubros();">
                                                        <i class="imoon imoon-plus"></i>
                                                        <span class="text">Agregar</span>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-default" data-dismiss="modal">
                                                        <i class="imoon imoon-close"></i>
                                                        <span class="text">Cerrar</span>
                                                </asp:LinkButton>
                                            </div>

                                        </asp:Panel>
                                    </asp:Panel>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </asp:Panel>


                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

        </div>
    </div>
</div>
<!-- /.modal -->

<%--Modal Form Agregar Rubros uso no contemplado --%>
<div id="frmAgregarActividades" class="modal fade" style="margin-left: -20%; width: 600px; display: none; max-height: 90%; overflow: auto">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title">Agregar Rubros no existentes en el nomenclador</h4>
            </div>
            <div class="modal-body pbottom5">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="form-horizontal">
                            <div class="row">
                                <label class="control-label col-sm-2">Descripción de la actividad:</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDesc_runc" runat="server" Width="80%" Style="max-width: 80%; max-height: 70px" CssClass="form-control" TextMode="MultiLine" Rows="2">
                                    </asp:TextBox>
                                    <div id="Req_txtDesc_runc" class="field-validation-error" style="display: none">
                                        Debe ingresar la descripción de la Actividad que desea ingresar.
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="control-label col-sm-2">Documentación Requerida:</label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlTipoDocReq_runc" Width="84%" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <div id="Req_ddlTipoDocReq_runc" class="field-validation-error" style="display: none">
                                        Debe seleccionar el tipo de documentacion requerida.
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="control-label col-sm-2">Tipo Actividad:</label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlTipoActividad_runc" Width="84%" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <div id="Req_ddlTipoActividad_runc" class="field-validation-error" style="display: none">
                                        Debe seleccionar el tipo de actividad.
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="control-label col-sm-2">Superficie</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtSuperficieRubro_runc" runat="server" Width="50%" MaxLength="15"></asp:TextBox>
                                    <div id="Req_TxtSuperficie" class="field-validation-error" style="display: none">
                                        La superficie a habilitar debe ser un número entre 0 y la superficie total del local.
                                    </div>
                                </div>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div class="modal-footer mtop5 mleft20 mright20" style="margin-left: 20px; margin-right: 20px">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="form-inline text-right">
                            <div class="control-group inline-block">
                                <asp:UpdateProgress ID="UpdateProgress8" AssociatedUpdatePanelID="UpdatePanel2"
                                    runat="server" DisplayAfter="200">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>

                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary" Style="color: white" OnClientClick="return validarAgregarRubroNoContemplado();" OnClick="btnAgregarRubroUsoNoContemplado_Click">
                                        <i class="imoon imoon-ok"></i>
                                        <span class="text">Aceptar</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-default" OnClientClick="hidefrmAgregarRubroUsoNoContemplado();">
                                        <i class="imoon imoon-close"></i>
                                        <span class="text">Cancelar</span>
                            </asp:LinkButton>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</div>
<%--/.modal --%>

<%--Modal mensajes de error--%>
<div id="frmError_Rubros" class="modal fade" style="display: none;">
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
                                    <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
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

<script type="text/javascript">

    $(document).ready(function () {


        init_JS_updAgregarNormativa();
        init_JS_updBuscarRubros();

    });


    function init_JS_updAgregarNormativa() {
        $("#<%: txtNroNormativa.ClientID %>").keypress(function (e) {
            $("#ReqtxtNormativa_Rubros").hide();
            if (e.which == 32)
                return false;
            return true;
        });


    }
    function ValidarNuevaSuperficie(obj) {
        debugger;
        $("#Val_Nueva_Superficie").hide();
        var dad = $(obj).parent();
        var value1 = dad.find('input[type="text"]').val();
        var value2 = $("#<%: hid_Superficie_Local.ClientID %>").val();

        var superficie = stringToFloat(value1);
        var superficieMaxima = stringToFloat(value2);

        if (superficie > superficieMaxima) {
            dad.find('[id*=Val_Nueva_Superficie]').css("display", "inline-block");
            return false;
        }

        return true;
    }
    function EditarSuperficie(obj) {
        $(obj).hide();
        var dad = $(obj).parent();
        dad.find('label').hide();
        dad.find('input[type="text"]').show().focus();
        dad.find('[id*=lnkSupHabSave]').show();
        dad.find('[id*=lnkSupHabClose]').show();
        return false;
    }
    function CloseEditarSuperficie(obj) {
        var dad = $(obj).parent();
        dad.find('label').show();
        dad.find('input[type="text"]').hide();
        dad.find('[id*=lnkSupHabSave]').hide();
        dad.find('[id*=lnkSupHabClose]').hide();
        dad.find('[id*=lnkSupHabEdit]').show();
        dad.find('[id*=Val_Nueva_Superficie]').hide();
        return false;
    }

    function init_JS_updBuscarRubros() {
        debugger;
        vSeparadorDecimal = $("#<%: hid_DecimalSeparator.ClientID %>").attr("value");
        eval("$('#<%: txtSuperficie.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");

        $("#<%: txtSuperficie.ClientID %>").on("keyup", function () {
            $("#Req_Superficie").hide();
        });

        $("#<%: txtBuscar.ClientID %>").on("keyup", function (event) {
            if (event.keyCode != 13) {
                $("#Req_txtBuscar").hide();
            }
        });

        eval("$('#<%: txtSuperficieRubro_runc.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");

        eval("$('[id*=txtSupHabNew]').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2', vMax: '999999.99'})");

        $("#<%: ddlTipoDocReq_runc.ClientID %>").select2({
            placeholder: "(Seleccionar)",
            allowClear: true,
        });
        $("#<%: ddlTipoActividad_runc.ClientID %>").select2({
            placeholder: "(Seleccionar)",
            allowClear: true,
        });
    }
    function init_JS_updInformacionTramite() {

        $("#<%: ddlZonaDeclarada.ClientID %>").select2();


        return false;
    }

    function showfrmError_Rubros() {
        $("#pnlBotonesGuardar_Rubros").show();
        $("#frmError_Rubros").modal("show");
        return false;

    }

    function showfrmConfirmarEliminar_Rubros() {

        $("#pnlBotonesConfirmacion_Rubros").show();
        $("#frmConfirmarEliminar_Rubros").modal("show");
        return false;
    }

    function hidefrmConfirmarEliminar_Rubros() {

        $("#frmConfirmarEliminar_Rubros").modal("hide");
        return false;
    }

    function ocultarBotonesConfirmacion_Rubros() {

        $("#pnlBotonesConfirmacion_Rubros").hide();
        return false;
    }

    function ocultarBotonesGuardado_Rubros() {

        $("#pnlBotonesGuardar_Rubros").hide();

        return true;
    }

    function showfrmAgregarNormativa_Rubros() {

        $("#<%: txtNroNormativa.ClientID %>").val("");
            $("#<%: ddlTipoNormativa.ClientID %>")[0].selectedIndex = 0;
            $("#<%: ddlEntidadNormativa.ClientID %>")[0].selectedIndex = 0;
            $("#ReqtxtNormativa_Rubros").hide();
            $("#frmAgregarNormativa_Rubros").modal("show");
            return false;
        }

        function hidefrmAgregarNormativa_Rubros() {

            $("#frmAgregarNormativa_Rubros").modal("hide");
            return false;
        }

        function ocultarBotonesConfirmacionEliminarNormativa_Rubros() {
            $("#pnlBotonesConfirmacionEliminarnormativa_Rubros").hide();
            return false;
        }

        function showfrmConfirmarEliminarNormativa_Rubros() {

            $("#frmConfirmarEliminarNormativa_Rubros").modal("show");
            return false;
        }

        function hidefrmConfirmarEliminarNormativa_Rubros() {

            $("#frmConfirmarEliminarNormativa_Rubros").modal("hide");
            return false;
        }

        function showfrmAgregarRubros_Rubros() {

            $("#<%: txtSuperficie.ClientID %>").val($("#<%: hid_Superficie_Local.ClientID %>").val());
            $("#<%: txtBuscar.ClientID %>").val("");
            $("#<%: pnlResultadoBusquedaRubros.ClientID %>").hide();
            $("#<%: pnlBuscarRubros.ClientID %>").show();
            $("#<%: pnlResultadoBusquedaRubros.ClientID %>").hide();
            $("#<%: pnlBotonesAgregarRubros.ClientID %>").hide();
            $("#<%: pnlBotonesBuscarRubros.ClientID %>").show();

            $("#<%: BotonesBuscarRubros.ClientID %>").show();


            $("#frmAgregarRubros_Rubros").on("shown.bs.modal", function (e) {
                $("#<%: txtBuscar.ClientID %>").focus();
            });

            $("#frmAgregarRubros_Rubros").modal({
                "show": true,
                "backdrop": "static"
            });

            return false;
            }

            function hidefrmAgregarRubros_Rubros() {

                $("#frmAgregarRubros_Rubros").modal("hide");
                return false;
            }

            function ocultarBotonesBusquedaRubros_Rubros() {
                $("#<%: BotonesBuscarRubros.ClientID %>").hide();
                return false;
            }

            function ocultarBotonesConfirmacionEliminarRubro_Rubros() {
                $("#pnlBotonesConfirmacionEliminarRubro_Rubros").hide();
                return false;
            }

            function ocultarValidadorAgregarRubros_Rubros() {

                $("#<%: ValidadorAgregarRubros.ClientID %>").hide();
                return false;
            }

            function ocultarBotonesAgregarRubros_Rubros() {

                $("#BotonesAgregarRubros_Rubros").hide();
                return false;
            }

            function showConfirmarEliminarRubro_Rubros(obj) {

                var id_caarubro_eliminar = $(obj).attr("data-id-rubro-eliminar");
                $("#<%: hid_id_caarubro_eliminar.ClientID %>").val(id_caarubro_eliminar);

            $("#frmConfirmarEliminarRubro_Rubros").modal("show");
            return false;
        }

        function hidefrmConfirmarEliminarRubro_Rubros() {

            $("#frmConfirmarEliminarRubro_Rubros").modal("hide");
            return false;
        }

        function validarAgregarNormativa_Rubros() {
            var ret = true;

            $("#ReqtxtNormativa_Rubros").hide();
            if ($("#<%: txtNroNormativa.ClientID %>").val().length == 0) {
                $("#ReqtxtNormativa_Rubros").show();
                ret = false;
            }

            return ret;

        }

        function validarAgregarRubroNoContemplado() {
            debugger;
            var ret = true;
            var value1 = $("#<%: txtSuperficieRubro_runc.ClientID %>").val();
            var value2 = $("#<%: hid_Superficie_Local.ClientID %>").val();

            var superficie = stringToFloat(value1);
            var superficieMaxima = stringToFloat(value2);

            $("#Req_TxtSuperficie").hide();
            if ($("#<%: txtSuperficieRubro_runc.ClientID %>").val().length == 0) {
                $("#Req_TxtSuperficie").show();
                ret = false;
            }

            if (superficie > superficieMaxima) {
                $("#Req_TxtSuperficie").show();
                ret = false;
            }

            $("#Req_txtDesc_runc").hide();
            if ($("#<%: txtDesc_runc.ClientID %>").val().length == 0) {
            $("#Req_txtDesc_runc").show();
            ret = false;
        }

        $("#Req_ddlTipoActividad_runc").hide();
        if ($("#<%: ddlTipoActividad_runc.ClientID %>").val() == 99 || $("#<%: ddlTipoActividad_runc.ClientID %>").val() == 0 || $("#<%: ddlTipoActividad_runc.ClientID %>").val() == null) {
            $("#Req_ddlTipoActividad_runc").show();
            ret = false;
        }

        $("#Req_ddlTipoDocReq_runc").hide();
        if ($("#<%: ddlTipoDocReq_runc.ClientID %>").val() == 99 || $("#<%: ddlTipoDocReq_runc.ClientID %>").val() == 0 || $("#<%: ddlTipoDocReq_runc.ClientID %>").val() == null) {
            $("#Req_ddlTipoDocReq_runc").show();
            ret = false;
        }
        return ret;
    }

    function hidefrmAgregarRubroUsoNoContemplado() {
        $("#frmAgregarActividades").modal("hide");
        return false;
    }
    function showfrmAgregarRubroUsoNoContemplado() {

        $("#frmAgregarActividades").modal("show");

        $("#<%: txtDesc_runc.ClientID %>").val("");
        $("#<%: txtSuperficieRubro_runc.ClientID %>").val("");

        $("#<%=ddlTipoDocReq_runc.ClientID%>").val('first').change();
        $("#<%=ddlTipoActividad_runc.ClientID%>").val('first').change();
        $("#Req_TxtSuperficie").hide();
        $("#Req_txtDesc_runc").hide();
        $("#Req_ddlTipoActividad_runc").hide();
        $("#Req_ddlTipoDocReq_runc").hide();

        return false;
    }
    function validarBuscar_Rubros() {
        var ret = true;
        $("#Req_Superficie_Rubros").hide();
        $("#Req_txtBuscar_Rubros").hide();

        var value1 = $("#<%: txtSuperficie.ClientID %>").val();
        var value2 = $("#<%: hid_Superficie_Local.ClientID %>").val();
        var superficie = stringToFloat(value1);
        var superficieMaxima = stringToFloat(value2);
        /*if (superficie <= 0 || superficie > superficieMaxima) {
            $("#Req_Superficie_Rubros").css("display", "inline-block");
            ret = false;
        }*/
        if ($("#<%: txtBuscar.ClientID %>").val().length < 3) {
            $("#Req_txtBuscar_Rubros").css("display", "inline-block");
            ret = false;
        }

        if (ret) {
            ocultarBotonesBusquedaRubros_Rubros();
            $("#<%: pnlGrupoAgregarRubros.ClientID %>").css("display", "block");
            }
            return ret;
        }

        function habilitarControlesRubros(habilitar) {
            debugger;
            if (habilitar) {
                $("#<%: ddlZonaDeclarada.ClientID %>").removeAttr('disabled');
            }
            else {
                $("#<%: ddlZonaDeclarada.ClientID %>").attr('disabled', 'disabled');
            }

            return false;
        }

        function hideEditRubros() {
            $("#box_editarRubros").hide("slow");
            $("#box_infoRubros").hide("slow");
        }
        function hideMostrarRubros() {
            $("#box_MostrarRubros").hide("slow");
        }

</script>

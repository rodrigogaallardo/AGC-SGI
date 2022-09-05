<%@ Page Title="Indicadores KPI" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndicadoresKPI.aspx.cs" Inherits="SGI.GestionTramite.IndicadoresKPI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <div id="page_content">

        <%-- Muestra Busqueda--%>
        <div id="box_busqueda">
            <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">

                <%-- filtros de busqueda--%>
                <div class="">
                    <%--buscar tramite --%>
                    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                        <%-- titulo collapsible buscar por tramite --%>
                        <div class="accordion-heading">
                            <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                                data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                                <div class="widget-title">
                                    <span class="icon"><i class="icon-list-alt"></i></span>
                                    <h5>
                                        <asp:Label ID="bt_tramite_tituloControl" runat="server" Text="Filtro de datos para definición del lote"></asp:Label></h5>
                                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                                </div>
                            </a>
                        </div>
                        <%-- controles collapsible buscar por tramite --%>
                        <div class="accordion-body collapse in" id="collapse_bt_tramite">
                            <div class="widget-content">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_tramite" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="row-fluid">
                                                <div class="span6">
                                                    <asp:Label ID="lblTipoTramite" runat="server" AssociatedControlID="ddlTipoTramite"
                                                        Text="Tipo Trámite:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoTramite_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="lblFechaInicioDesde" runat="server" AssociatedControlID="txtFechaInicioDesde"
                                                            Text="Fecha Inicio Desde:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaInicioDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <asp:RegularExpressionValidator
                                                                    ID="rev_txtFechaInicioDesde" runat="server"
                                                                    ValidationGroup="buscar"
                                                                    ControlToValidate="txtFechaInicioDesde" CssClass="field-validation-error"
                                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                                <div id="req_txtFechaInicioDesde" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar un valor.
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label9" runat="server" AssociatedControlID="txtFechaInicioHasta"
                                                            Text="Fecha Inicio Hasta:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaInicioHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <asp:RegularExpressionValidator
                                                                    ID="rev_txtFechaInicioHasta" runat="server"
                                                                    ValidationGroup="buscar"
                                                                    ControlToValidate="txtFechaInicioHasta" CssClass="field-validation-error"
                                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                                <div id="req_txtFechaInicioHasta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar un valor.
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlrfd" runat="server">
                                                <div class="row-fluid">
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <asp:Label ID="Label10" runat="server" AssociatedControlID="txtFechaCrfdDesde"
                                                                Text="Fecha cierre revision firma Dispo Desde:" class="control-label"></asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtFechaCrfdDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div class="req">
                                                                    <asp:RegularExpressionValidator
                                                                        ID="rev_txtFechaCrfdDesde" runat="server"
                                                                        ValidationGroup="buscar"
                                                                        ControlToValidate="txtFechaCrfdDesde" CssClass="field-validation-error"
                                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                        Display="Dynamic">
                                                                    </asp:RegularExpressionValidator>
                                                                    <div id="req_txtFechaCrfdDesde" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                        Debe ingresar un valor.
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <asp:Label ID="Label11" runat="server" AssociatedControlID="txtFechaCrfdHasta"
                                                                Text="Fecha cierre revision firma Dispo Hasta:" class="control-label"></asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtFechaCrfdHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div class="req">
                                                                    <asp:RegularExpressionValidator
                                                                        ID="rev_txtFechaCrfdHasta" runat="server"
                                                                        ValidationGroup="buscar"
                                                                        ControlToValidate="txtFechaCrfdHasta" CssClass="field-validation-error"
                                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                        Display="Dynamic">
                                                                    </asp:RegularExpressionValidator>
                                                                    <div id="req_txtFechaCrfdHasta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                        Debe ingresar un valor.
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="row-fluid">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label12" runat="server" AssociatedControlID="txtFechaCgeDesde"
                                                            Text="Fecha generación expediente Desde:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaCgeDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <asp:RegularExpressionValidator
                                                                    ID="rev_txtFechaCgeDesde" runat="server"
                                                                    ValidationGroup="buscar"
                                                                    ControlToValidate="txtFechaCgeDesde" CssClass="field-validation-error"
                                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                                <div id="req_txtFechaCgeDesde" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar un valor.
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label13" runat="server" AssociatedControlID="txtFechaCgeHasta"
                                                            Text="Fecha generación expediente Hasta:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtFechaCgeHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                            <div class="req">
                                                                <asp:RegularExpressionValidator
                                                                    ID="rev_txtFechaCgeHasta" runat="server"
                                                                    ValidationGroup="buscar"
                                                                    ControlToValidate="txtFechaCgeHasta" CssClass="field-validation-error"
                                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                    Display="Dynamic">
                                                                </asp:RegularExpressionValidator>
                                                                <div id="req_txtFechaCgeHasta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar un valor.
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlRdg" runat="server">
                                                <div class="row-fluid">
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <asp:Label ID="Label16" runat="server" AssociatedControlID="txtFechaRdgDesde"
                                                                Text="Fecha inicio revisión DGHyP Desde:" class="control-label"></asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtFechaRdgDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div class="req">
                                                                    <asp:RegularExpressionValidator
                                                                        ID="rev_txtFechaRdgDesde" runat="server"
                                                                        ValidationGroup="buscar"
                                                                        ControlToValidate="txtFechaRdgDesde" CssClass="field-validation-error"
                                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                        Display="Dynamic">
                                                                    </asp:RegularExpressionValidator>
                                                                    <div id="req_txtFechaRdgDesde" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                        Debe ingresar un valor.
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <asp:Label ID="Label17" runat="server" AssociatedControlID="txtFechaRdgHasta"
                                                                Text="Fecha inicio revisión DGHyP Hasta:" class="control-label"></asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtFechaRdgHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div class="req">
                                                                    <asp:RegularExpressionValidator
                                                                        ID="rev_txtFechaRdgHasta" runat="server"
                                                                        ValidationGroup="buscar"
                                                                        ControlToValidate="txtFechaRdgHasta" CssClass="field-validation-error"
                                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                        Display="Dynamic">
                                                                    </asp:RegularExpressionValidator>
                                                                    <div id="req_txtFechaRdgHasta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                        Debe ingresar un valor.
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <%--                                            <div id="req_ValidacionFecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar alguna de las fechas.
                                            </div>--%>

                                            <asp:Panel ID="pnlGrt" runat="server">
                                                <div class="row-fluid">
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtFechaRdgDesde"
                                                                Text="Fecha inicio Revisión Gerente 2º Desde:" class="control-label"></asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtFechaGrt2Desde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div class="req">
                                                                    <asp:RegularExpressionValidator
                                                                        ID="RegularExpressionValidator1" runat="server"
                                                                        ValidationGroup="buscar"
                                                                        ControlToValidate="txtFechaGrt2Desde" CssClass="field-validation-error"
                                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                        Display="Dynamic">
                                                                    </asp:RegularExpressionValidator>
                                                                    <div id="req_txtFechaGrtDesde" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                        Debe ingresar un valor.
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="span6">
                                                        <div class="control-group">
                                                            <asp:Label ID="Label5" runat="server" AssociatedControlID="txtFechaRdgHasta"
                                                                Text="Fecha inicio Revisión Gerente 2º Hasta:" class="control-label"></asp:Label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtFechaGrt2Hasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                                <div class="req">
                                                                    <asp:RegularExpressionValidator
                                                                        ID="RegularExpressionValidator2" runat="server"
                                                                        ValidationGroup="buscar"
                                                                        ControlToValidate="txtFechaGrt2Hasta" CssClass="field-validation-error"
                                                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                                        Display="Dynamic">
                                                                    </asp:RegularExpressionValidator>
                                                                    <div id="req_txtFechaGrt2Hasta" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                        Debe ingresar un valor.
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div id="req_ValidacionFecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar alguna de las fechas.
                                            </div>

                                            <div class="row-fluid">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label6" runat="server" CssClass="control-label" Text="Circuito:" AssociatedControlID="ddlCircuito" />
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlCircuito" runat="server" multiple="true" Width="400px"></asp:DropDownList>
                                                            <asp:HiddenField ID="hid_circuitos_selected" runat="server"></asp:HiddenField>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label8" CssClass="control-label" runat="server" Text="Observado:" AssociatedControlID="ddlObservado"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlObservado" runat="server" Width="80px">
                                                                <asp:ListItem Value="0" Text="Todos"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <%--buscar rubros --%>
                    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
                        <%-- titulo collapsible buscar rubros --%>
                        <div class="accordion-heading">
                            <a id="bt_rubro_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_rubro"
                                data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                                <div class="widget-title">
                                    <span class="icon"><i class="icon-list-alt"></i></span>
                                    <h5>
                                        <asp:Label ID="Label2" runat="server" Text="Filtro de datos para definir las tareas entre los cuales se calcula la demora"></asp:Label></h5>
                                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                                </div>
                            </a>
                        </div>

                        <%-- controles collapsible buscar por rubros --%>
                        <div class="accordion-body collapse in" id="collapse_bt_rubro">
                            <div class="widget-content">
                                <asp:UpdatePanel ID="updPnlFiltroBuscar_rubros" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="row-fluid">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlTareaOrigen"
                                                            Text="Tarea Origen:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTareaOrigen" runat="server" ValidationGroup="Buscar" Style="width: 50%; min-width: 100px" AutoPostBack="true"></asp:DropDownList>
                                                            <div class="req">
                                                                <div id="req_ddlTareaOrigen" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar un valor.
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <asp:RadioButton ID="rbTareaOrigenPrimera" Text="Primera" runat="server" GroupName="tareaOrigen" Checked="true" />
                                                                <asp:RadioButton ID="rbTareaOrigenUltima" Text="Ultima" runat="server" GroupName="tareaOrigen" />
                                                            </div>
                                                            <div>
                                                                <asp:RadioButton ID="rbTareaOrigenFechaInicio" Text="Fecha Inicio" runat="server" GroupName="tareaOrigenInicio" Checked="true" />
                                                                <asp:RadioButton ID="rbTareaOrigenFechaFin" Text="Fecha Fin" runat="server" GroupName="tareaOrigenInicio" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span7">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label4" runat="server" AssociatedControlID="ddlTareaFin"
                                                            Text="Tarea Fin:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTareaFin" runat="server" Style="width: 50%; min-width: 100px" AutoPostBack="true"></asp:DropDownList>
                                                            <div class="req">
                                                                <div id="req_ddlTareaFin" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                    Debe ingresar un valor.
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <asp:RadioButton ID="rbTareaFinPrimera" Text="Primera" runat="server" GroupName="tareaFin" Checked="true" />
                                                                <asp:RadioButton ID="rbTareaFinUltima" Text="Ultima" runat="server" GroupName="tareaFin" />
                                                            </div>
                                                            <div>
                                                                <asp:RadioButton ID="rbTareaFinFechaInicio" Text="Fecha Inicio" runat="server" GroupName="tareaFinInicio" Checked="true" />
                                                                <asp:RadioButton ID="rbTareaFinFechaFin" Text="Fecha Fin" runat="server" GroupName="tareaFinInicio" />
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="span6">
                                                    <div class="control-group">
                                                        <asp:Label ID="Label15" runat="server"
                                                            Text="Formato del resultado:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:RadioButton ID="rbDiasLaborales" Text="Días laborales" runat="server" GroupName="formatoResultado" Checked="true" />
                                                            <asp:RadioButton ID="rbDiasCorridos" Text="Días corridos" runat="server" GroupName="formatoResultado" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                </div>

                <br />

                <asp:UpdatePanel ID="btn_BuscarTramite" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="pull-right">
                            <div class="control-group inline-block">
                                <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="btn_BuscarTramite"
                                    runat="server" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClientClick="validarBuscar();"
                                ValidationGroup="buscar" OnClick="btnBuscar_Click">
                            <i class="icon-white icon-search"></i>
                            <span class="text">Buscar</span>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </asp:Panel>
        </div>

        <br />
        <br />

        <%-- Muestra Resultados--%>
        <div id="box_resultado" style="display: none;">
            <div class="widget-box">
                <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal" style="margin-left: 15px; margin-right: 15px">
                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Resultado general: </h5>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Dias promedio de demora total:
                                                <asp:Label ID="lbl_diastotales" runat="server" Text="0"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Dias promedio de demora total sin correcci&oacuten; de solicitud:
                                                <asp:Label ID="lbl_diastotal_sincorreccion" runat="server" Text="0"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Dias promedio de demora DGHYP:
                                                <asp:Label ID="lbl_diasDghyp" runat="server" Text="0"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Dias promedio de demora DGHYP sin correcci&oacuten; de solicitud:
                                                <asp:Label ID="lbl_diasDghy_sincorreccion" runat="server" Text="0"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Dias promedio de demora DGHYP sin tiempos muertos y sin corrección de solicitud:
                                                <asp:Label ID="lbl_diasDghyp_sintiemposmuertos_ni_correcciones" runat="server" Text="0"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="display: inline-block">
                                            <h5>Lista de Solicitudes</h5>
                                        </div>
                                        <div style="display: inline-block">
                                            <span class="badge">
                                                <asp:Label ID="lblCantidadRegistros" runat="server"></asp:Label></span>
                                        </div>
                                        <div style="display: inline-block">
                                            <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click" OnClientClick="return showfrmExportarExcel();" CssClass="link-local pright20" Style="font-size: 14px;">
                                            <i class="imoon imoon-file-excel color-green"></i>
                                            <span>Exportar a Excel</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

                            </asp:Panel>
                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                SelectMethod="grdResultados_GetData" ItemType="SGI.Model.clsItemIndicadoresExcel"
                                AllowPaging="true" AllowSorting="true" PageSize="30" OnPageIndexChanging="grdResultados_PageIndexChanging"
                                OnDataBound="grdResultados_DataBound">
                                <Columns>
                                    <asp:BoundField DataField="id_solicitud" ItemStyle-Width="150px" HeaderText="Solicitud" SortExpression="username" />
                                    <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaInicio" />
                                    <asp:BoundField DataField="Fecha_Generacion_Expediente" HeaderText="Fecha Generacion Expediente" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaCierreGe" />
                                    <asp:BoundField DataField="Fecha_Firma_Dispo" HeaderText="Fecha Firma Dispo" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaCierreRfd" />
                                    <asp:BoundField DataField="Fecha_Tarea_Origen" HeaderText="Fecha Tarea Origen" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaOrigen" />
                                    <asp:BoundField DataField="Fecha_Tarea_Fin" HeaderText="Fecha Tarea Fin" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaFin" />
                                    <asp:BoundField DataField="dias_totales" HeaderText="Días Totales" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="dias_totales" />
                                    <asp:BoundField DataField="dias_dghyp" HeaderText="Días DGHyP" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="dias_dghyp" />
                                    <asp:BoundField DataField="dias_avh" HeaderText="Días AVH" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="dias_avh" />
                                    <asp:BoundField DataField="dias_dictamen" HeaderText="Días Dictámenes" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="dias_dictamen" />
                                    <asp:BoundField DataField="dias_contribuyente" HeaderText="Días Contribuyente" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="dias_contribuyente" />
                                    <asp:BoundField DataField="tiempo_muerto" HeaderText="Tiempos Muertos" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="tiempo_muerto" />
                                    <asp:BoundField DataField="observado" HeaderText="Observado" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="observado" />
                                    <asp:BoundField DataField="cant_obs" HeaderText="Cantidad de observaciones" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="cant_obs" />
                                </Columns>
                                <PagerTemplate>
                                    <asp:Panel ID="pnlPager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                        <asp:Button ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click"
                                            CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn" />
                                        <asp:Button ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn" />
                                        <asp:Button ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click"
                                            CssClass="btn btn-default" />
                                    </asp:Panel>
                                </PagerTemplate>
                                <EmptyDataTemplate>

                                    <div>

                                        <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                        <span class="mleft20">No se encontraron registros.</span>

                                    </div>

                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- /.modal -->
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
                                    <asp:UpdatePanel ID="updmpeInfo" runat="server" class="control-group">
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
        <!-- /.modal -->

        <%--Exportación a Excel--%>
        <div id="frmExportarExcel" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel ID="updExportaExcel" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <h4 class="modal-title">Exportar a Excel</h4>
                            </div>
                            <div class="modal-body">


                                <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" Enabled="false">
                                </asp:Timer>

                                <asp:Panel ID="pnlExportandoExcel" runat="server">
                                    <div class="row text-center">
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                                    </div>
                                    <div class="row text-center">
                                        <h2>
                                            <asp:Label ID="lblRegistrosExportados" runat="server"></asp:Label></h2>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlDescargarExcel" runat="server" Style="display: none">
                                    <div class="row text-center">
                                        <asp:HyperLink ID="btnDescargarExcel" runat="server" Target="_blank" CssClass="btn btn-link">
                                        <i class="imoon imoon-file-excel color-green fs48"></i>
                                        <br />
                                        <span class="text">Descargar archivo</span>
                                        </asp:HyperLink>
                                    </div>
                                </asp:Panel>

                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnCerrarExportacion" runat="server" CssClass="btn btn-default" OnClick="btnCerrarExportacion_Click" Text="Cerrar" Visible="false" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            init_Js_updpnlBuscar();
            init_Js_updResultados();
        });

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;
        }

        function init_Js_updpnlBuscar() {
            toolTips();
            inicializar_fechas();
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTareaOrigen.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTareaFin.ClientID %>").select2({ allowClear: true });
            /// Inicializar select2 de busqueda
            var tags_selecionados = "";
            if ($("#<%: hid_circuitos_selected.ClientID %>").val().length > 0) {
                tags_selecionados = $("#<%: hid_circuitos_selected.ClientID %>").val().split(",");
            }

            $("#<%: ddlCircuito.ClientID %>").select2({
                tags: true,
                tokenSeparators: [","],
                placeholder: "Ingrese las etiquetas de búsqueda",
                language: "es",
                data: tags_selecionados
            });
            $("#<%: ddlCircuito.ClientID %>").val(tags_selecionados);
            $("#<%: ddlCircuito.ClientID %>").trigger("change.select2");

            $("#<%: ddlCircuito.ClientID %>").on("change", function () {
                $("#<%: hid_circuitos_selected.ClientID %>").val($("#<%: ddlCircuito.ClientID %>").val());
            });


            return false;
        }

        function inicializar_fechas() {
            inicializar_fecha($('#<%=txtFechaInicioDesde.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaInicioHasta.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaCgeDesde.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaCgeHasta.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaRdgDesde.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaRdgHasta.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaCrfdDesde.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaCrfdHasta.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaGrt2Desde.ClientID%>'));
            inicializar_fecha($('#<%=txtFechaGrt2Hasta.ClientID%>'));
        }

        function inicializar_fecha(fecha) {
            var es_readonly = $(fecha).attr("readonly");

            fecha.datepicker({
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

            if (!($(fecha).is('[disabled]') || $(fecha).is('[readonly]'))) {
                $(fecha).datepicker(
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
            }
        }

        function init_Js_updResultados() {
            toolTips();
        }

        function ocultarBotonesConfirmacion() {
            $("#pnlBotonesConfirmacionEliminar").hide();
            return false;
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function showBusqueda() {
            $("#box_resultado").hide("slow");
            $("#box_busqueda").show("slow");
        }
        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
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
        }

        function hideSummary() {
            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function validarBuscar() {
            var ret = true;
            debugger;
            hideSummary();
            debugger;
            if ($.trim($("#<%: txtFechaInicioDesde.ClientID %>").val()).length == 0 && $.trim($("#<%: txtFechaInicioHasta.ClientID %>").val()).length == 0
            && $.trim($("#<%: txtFechaCrfdDesde.ClientID %>").val()).length == 0 && $.trim($("#<%: txtFechaCrfdHasta.ClientID %>").val()).length == 0
            && $.trim($("#<%: txtFechaCgeDesde.ClientID %>").val()).length == 0 && $.trim($("#<%: txtFechaCgeHasta.ClientID %>").val()).length == 0
            && $.trim($("#<%: txtFechaRdgDesde.ClientID %>").val()).length == 0 && $.trim($("#<%: txtFechaRdgHasta.ClientID %>").val()).length == 0) {
                $("#req_ValidacionFecha").css("display", "inline-block");
                ret = false;
            }

            if ($('#<%: ddlTareaOrigen.ClientID %>').val() == 0) {
                $("#req_ddlTareaOrigen").css("display", "inline-block");
                ret = false;
            }
            if ($('#<%: ddlTareaFin.ClientID %>').val() == 0) {
                $("#req_ddlTareaFin").css("display", "inline-block");
                ret = false;
            }

            /*if (ret) {
                   
            }
            else {
                $("#ValSummary").css("display", "inline-block");
    
            }*/
            return ret;
        }

        function showfrmExportarExcel() {
            $("#frmExportarExcel").modal({
                backdrop: "static",
                show: true
            });
            return true;
        }
        function hidefrmExportarExcel() {
            $("#frmExportarExcel").modal("hide");
            return false;
        }
    </script>
</asp:Content>

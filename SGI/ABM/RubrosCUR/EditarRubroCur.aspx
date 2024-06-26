﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarRubroCUR.aspx.cs" Inherits="SGI.ABM.RubrosCUR.EditarRubroCur" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .col1 {
            width: 150px;
            text-align: right;
            vertical-align: top;
            padding-top: 7px;
        }

        .col2 {
            padding-top: 5px;
        }
    </style>

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>


    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function inicializar_dropdownlists() {
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTipoActividad.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlCircuito.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlEstacionamiento.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlRubrosBicicleta.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlCyD.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlEditTipoDocReq.ClientID %>").select2({ allowClear: true });
        }

        window.onload = function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        }

        function endRequestHandler(sender, args) {
            init_updDatos();
        }

        $(function () { // DOM ready
            init_updDatos();
        });
        function init_updDatos() {
            inicializar_dropdownlists();
            var fechaVigenciaHasta = $('#<%=txtFechaVigenciaHasta.ClientID%>');
            var es_readonlyHasta = $('#<%=txtFechaVigenciaHasta.ClientID%>').attr("readonly");

            $("#<%: txtFechaVigenciaHasta.ClientID %>").datepicker({
                //minDate: "-100Y",
                //maxDate: "0y",
                //yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                }
            });

            vSeparadorDecimal = $("#<%: hid_DecimalSeparator.ClientID %>").attr("value");
            eval("$('#<%: txtDesde.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
            eval("$('#<%: txtHasta.ClientID %>').autoNumeric({ aSep: '', aDec: '" + vSeparadorDecimal + "', mDec: '2',vMax: '999999.99'})");
        }

        function showfrmErrorVisual() {
            $("#frmErrorVisual").modal("show");
            return false;
        }
    </script>
    <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
    <asp:HiddenField ID="hid_rol_edicion" runat="server" />
    <asp:HiddenField ID="hid_id_rubhistcam" runat="server" />

    <asp:Panel ID="pnlDatosRubro" runat="server">
        <asp:UpdatePanel ID="updEditarRubro" runat="server">
            <ContentTemplate>
                <div class="widget-box">
                    <div class="widget-title ">
                        <span class="icon"><i class="imoon imoon-hammer"></i></span>
                        <h5>Datos del Rubro</h5>
                    </div>
                    <div class="widget-content">
                        <table border="0" style="width: 100%">
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblCodRubro" runat="server" Text="Código del Rubro:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtCodRubro" runat="server" MaxLength="6" Width="80px" Enabled="false"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator ID="ReqtxtCodRubro" runat="server" ControlToValidate="txtCodRubro" CssClass="error"
                                            ErrorMessage="El código de rubro es obligatorio." Display="Dynamic" ValidationGroup="CrearRubro">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblDescRubro" runat="server" Text="Descripción del Rubro:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtDescRubro" runat="server" MaxLength="300"
                                        TextMode="MultiLine" Width="90%"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator ID="ReqtxtDescRubro" runat="server" ControlToValidate="txtDescRubro" CssClass="error"
                                            ErrorMessage="La descripción del rubro es obligatorio." Display="Dynamic" ValidationGroup="CrearRubro">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblBusqueda" runat="server" Text="Palabras Claves:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtBusqueda" runat="server" MaxLength="400" TextMode="MultiLine"
                                        Width="90%"></asp:TextBox>
                                    <div class="info2">
                                        Palabras separadas por (/), las mismas sirven para que el rubro sea encontrado por
                            otra palabra habitual.
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblTooTip" runat="server" Text="Información descriptiva:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtToolTip" runat="server" MaxLength="2000" TextMode="MultiLine"
                                        Width="90%"></asp:TextBox>
                                    <div class="info2">
                                        Lo escrito en este campo saldrá como ayuda en el Asesor Virtual de Habilitaciones.
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblFechaVigenciaHasta" runat="server" Text="Rubro vigente hasta el:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtFechaVigenciaHasta" runat="server" Width="75px"></asp:TextBox>
                                    <div class="req">
                                        <asp:RegularExpressionValidator
                                            ID="rev_txtFechaVigenciaHasta" runat="server"
                                            ValidationGroup="buscar"
                                            ControlToValidate="txtFechaVigenciaHasta" CssClass="field-validation-error"
                                            ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                            ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                            Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                    </div>

                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblTipoActividad" runat="server" Text="Tipo de Actividad:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="pnlTipoActividad" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlTipoActividad" runat="server" Width="500px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTipoActividad" CssClass="error"
                                            InitialValue="0" ErrorMessage="Por favor seleccione un tipo de actividad" Display="Dynamic" ValidationGroup="CrearRubro" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblTipoTramite" runat="server" Text="Tipo de Trámite:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="pnlTipoTramite" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="500px">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlTipoTramite" CssClass="error"
                                            InitialValue="0" ErrorMessage="Por favor seleccione un tipo de tramite" Display="Dynamic" ValidationGroup="CrearRubro" />--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label20" runat="server" Text="Grupo Circuito:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="pnlCircuito" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlCircuito" runat="server" Width="500px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCircuito" CssClass="error"
                                            InitialValue="0" ErrorMessage="Por favor seleccione un tipo de circuito" Display="Dynamic" ValidationGroup="CrearRubro" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label21" runat="server" Text="Librado al Uso:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:CheckBox ID="ChkLibrado" runat="server"  />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label50" runat="server" Text="Solo Apra:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:CheckBox ID="ChkSoloApra" runat="server" Enabled="false" />
                                </td>
                            </tr>


                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label15" runat="server" Text="Condicion Express:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:CheckBox ID="ChkExpress" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label8" runat="server" Text="Asistentes 350:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:CheckBox ID="chkAsistentes350" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label14" runat="server" Text="Sin Baño PCD hasta 60m²"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:CheckBox ID="chkSinBanioPCD" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label1" runat="server" Text="Zona Mixtura 1:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtZonaMixtura1" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtZonaMixtura" runat="server" ControlToValidate="txtZonaMixtura1" CssClass="error"
                                            ErrorMessage="El campo mixtura 1 es obligatorio." Display="Dynamic" ValidationGroup="CrearRubro">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label2" runat="server" Text="Zona Mixtura 2:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtZonaMixtura2" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtZonaMixtura2" runat="server" ControlToValidate="txtZonaMixtura2" CssClass="error"
                                            ErrorMessage="El campo mixtura 2 es obligatorio." Display="Dynamic" ValidationGroup="CrearRubro">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label3" runat="server" Text="Zona Mixtura 3:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtZonaMixtura3" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtZonaMixtura3" runat="server" ControlToValidate="txtZonaMixtura3" CssClass="error"
                                            ErrorMessage="El campo mixtura 3 es obligatorio." Display="Dynamic" ValidationGroup="CrearRubro">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label4" runat="server" Text="Zona Mixtura 4:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtZonaMixtura4" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtZonaMixtura4" runat="server" ControlToValidate="txtZonaMixtura4" CssClass="error"
                                            ErrorMessage="El campo mixtura 4 es obligatorio" Display="Dynamic" ValidationGroup="CrearRubro">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label5" runat="server" Text="Estacionamiento:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="Panel1" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlEstacionamiento" runat="server" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlEstacionamiento_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblEstacionamientoInfo" runat="server" Text="Descripción Estacionamiento: " Visible="false"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Label ID="lblEstacionamientoDesc" Width="90%" runat="server"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label6" runat="server" Text="Bicicleta:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="Panel2" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlRubrosBicicleta" runat="server" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlBicicleta_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblBicicletaInfo" runat="server" Text="Descripción Bicicleta: " Visible="false"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Label ID="lblBicicletaDesc" Width="90%" runat="server"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label7" runat="server" Text="Carga y descarga:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="Panel3" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlCyD" runat="server" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlCyD_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblCyDInfo" runat="server" Text="Descripción Carga y descarga: " Visible="false"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Label ID="lblCyD" Width="90%" runat="server"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblCondicionesIncendioHead" runat="server" Text="Condiciones de Incendio:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Panel ID="Panel4" runat="server" Width="350px">
                                        <asp:DropDownList ID="ddlCondicionesIncendio" runat="server" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlCondicionesIncendio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblCondicionesIncendioTitle" runat="server" Text="Condiciones de Incendio: " Visible="false"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:Label ID="lblCondicionesIncendio" Width="90%" runat="server" Font-Bold="True"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="widget-box">
                                        <div class="widget-title ">
                                            <span class="icon"><i class="imoon imoon-hammer"></i></span>
                                            <h5>Documentos requeridos</h5>
                                        </div>
                                        <div class="widget-content">
                                            <%--Documentos requeridos--%>
                                            <div style="padding: 15px 5px 0px 5px">
                                                <asp:Label ID="Label10" runat="server" Text="Configuración de Documentación Requerida para todas las zonas:" Font-Bold="true"></asp:Label>
                                                <%--grilla con datos de documentos requeridos--%>
                                                <asp:UpdatePanel ID="pnlGrdDocReq" runat="server" Width="90%" style="text-align: right; padding-top: 5px">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hidIdDeletedDocReq" Value="" runat="server" />
                                                        <asp:GridView
                                                            ID="grdDocReq"
                                                            runat="server"
                                                            AutoGenerateColumns="false"
                                                            DataKeyNames="id_rubtdocreq"
                                                            Style="margin-top: 5px"
                                                            GridLines="None"
                                                            CssClass="table table-bordered table-striped table-hover with-check">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="60px" />
                                                                <asp:BoundField DataField="Accion" HeaderText="Accion" HeaderStyle-Width="15px" Visible="false" />
                                                                <asp:TemplateField ItemStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton
                                                                            ID="btnEliminarDocReq"
                                                                            runat="server"
                                                                            Text="Eliminar"
                                                                            CssClass="btnEliminarDocReq"
                                                                            CommandArgument='<%# Eval("id_rubtdocreq") %>'
                                                                            OnClick="btnEliminarDocReq_Click"
                                                                            OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se han encontrado Documentos Requeridos para este rubro.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <%--Agregar nueva documentos requeridos--%>
                                                <asp:Panel
                                                    ID="pnlAccionesAgregarDocReq"
                                                    runat="server"
                                                    Width="90%"
                                                    Style="text-align: right; padding-top: 5px"
                                                    Visible="true">
                                                    <asp:LinkButton
                                                        ID="lnkBtnAccionesAgregarDocReq"
                                                        runat="server"
                                                        CssClass="btn btn-primary"
                                                        OnClick="lnkBtnAccionesAgregarDocReq_Click">
                                                            <i class="icon-white icon-plus"></i>
                                                            <span class="text">Agregar Documento Requerido</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                                <%--panel para editar documentos requeridos--%>
                                                <asp:Panel ID="pnlAgregarDocReqEdit" runat="server" Visible="false">

                                                    <div style="padding: 10px 5px 10px 5px" class="info2">
                                                        <label>
                                                            Para Agregar documentaci&oacute;n requerida al rubro ingrese los datos y luego presione el bot&oacute;n "Agregar"
                                                        </label>
                                                        <div style="margin-top: 10px">

                                                            <asp:HiddenField ID="hid_id_rubtdocreq" runat="server" />

                                                            <table>
                                                                <tr>
                                                                    <td>Tipo Documento: 
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlEditTipoDocReq" runat="server" Width="550px">
                                                                        </asp:DropDownList>
                                                                        <asp:CompareValidator ID="cv_ddlEditTipoDocReq" runat="server"
                                                                            ErrorMessage="Debe seleccionar el tipo de documento."
                                                                            ControlToValidate="ddlEditTipoDocReq"
                                                                            ValidationGroup="guardarTipoDocReq"
                                                                            ValueToCompare="0"
                                                                            Display="Dynamic"
                                                                            Type="Integer" Operator="NotEqual">
                                                                        </asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Documentación Obligatoria
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlEditObligatorio" runat="server" Width="150px">
                                                                            <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                                            <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:CompareValidator ID="cv_ddlEditObligatorio" runat="server"
                                                                            ErrorMessage="Debe seleccionar si el documento es obligatorio."
                                                                            ControlToValidate="ddlEditObligatorio"
                                                                            ValidationGroup="guardarTipoDocReq"
                                                                            ValueToCompare="0"
                                                                            Display="Dynamic"
                                                                            Type="String" Operator="NotEqual">
                                                                        </asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>

                                                                    <td>
                                                                        <asp:UpdatePanel ID="updPnlAgregarDocReq" runat="server" RenderMode="Inline">
                                                                            <ContentTemplate>
                                                                                <asp:LinkButton
                                                                                    ID="btnGuardarDocReq"
                                                                                    runat="server"
                                                                                    ValidationGroup="guardarTipoDocReq"
                                                                                    Style="margin-left: 10px"
                                                                                    OnClick="btnGuardarDocReq_Click"
                                                                                    CssClass="btn btn-inverse">
                                                <i class="icon-white icon-plus"></i>
                                                <span class="text">Agregar</span>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton
                                                                                    ID="btnCancelarDocReq"
                                                                                    runat="server"
                                                                                    OnClick="btnCancelarDocReq_Click"
                                                                                    OnClientClick=""
                                                                                    CssClass="btn btn-default">
                                                <i class="imoon imoon-blocked"></i>
                                                <span class="text">Cancelar</span>
                                                                                </asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>

                                                </asp:Panel>


                                                <%--grilla con de documentos requeridos que seran eliminados--%>
                                                <asp:Panel ID="pnlDocReqEliminada" runat="server" Visible="false">

                                                    <asp:Label ID="Label11" runat="server" Text="Documentación Requerida que ha sido eliminada:" Font-Bold="true"></asp:Label>
                                                    <asp:GridView
                                                        ID="grdDocReqEliminada"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        DataKeyNames="id_rubtdocreq"
                                                        Style="margin-top: 5px"
                                                        GridLines="None"
                                                        CssClass="table table-bordered table-striped table-hover with-check">
                                                        <Columns>
                                                            <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="60px" />
                                                            <asp:TemplateField ItemStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton
                                                                        ID="btnEliminarDocReqElim"
                                                                        runat="server" Text="Eliminar"
                                                                        CssClass="btnEliminarDocReq"
                                                                        CommandArgument='<%# Eval("id_rubtdocreq") %>'
                                                                        OnClick="btnEliminarDocReqElim_Click"
                                                                        OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                                    </asp:LinkButton>

                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            NO HAY DATOS ...
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>

                                            </div>

                                            <%--informacion reelevante--%>
                                            <div style="padding: 15px 5px 0px 5px">
                                                <asp:Label ID="Label12" runat="server" Text="Configuración de Información Relevante:" Font-Bold="true"></asp:Label>
                                                <%--grilla con datos de info relevante--%>
                                                <asp:UpdatePanel
                                                    ID="pnlGrdInfoRelevante"
                                                    runat="server"
                                                    Width="90%"
                                                    style="text-align: right; padding-top: 5px">
                                                    <ContentTemplate>
                                                        <asp:GridView
                                                            ID="grdInfoRelevante"
                                                            runat="server"
                                                            AutoGenerateColumns="false"
                                                            DataKeyNames="id_rubinf"
                                                            Style="margin-top: 5px"
                                                            GridLines="None"
                                                            CssClass="table table-bordered table-striped table-hover with-check">
                                                            <Columns>
                                                                <asp:BoundField DataField="descripcion_rubinf" HeaderText="Observación" />
                                                                <asp:TemplateField ItemStyle-Width="80px">
                                                                    <ItemTemplate>

                                                                        <asp:LinkButton ID="btnEliminarInfoRelevante" runat="server" Text="Eliminar"
                                                                            CssClass="btnEliminarInfoRelevante"
                                                                            CommandArgument='<%# Eval("id_rubinf") %>'
                                                                            OnClick="btnEliminarInfoRelevante_Click"
                                                                            OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se han encontrado datos sobre información relevante para este rubro.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <%--Agregar nueva info relevante--%>
                                                <asp:Panel ID="pnlAccionesAgregarInfoRelevante" runat="server" Width="90%" Style="text-align: right; padding-top: 5px" Visible="true">
                                                    <asp:LinkButton
                                                        ID="lnkBtnAccionesAgregarInfoRelevante"
                                                        runat="server"
                                                        CssClass="btn btn-primary"
                                                        OnClick="lnkBtnAccionesAgregarInfoRelevante_Click">
                    <i class="icon-white icon-plus"></i>
                    <span class="text">Agregar Información Relevante</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <%--panel para editar info relevante--%>
                                                <asp:Panel ID="pnlAgregarInfoRelevanteEdit" runat="server" Visible="false">

                                                    <div style="padding: 10px 5px 10px 5px" class="info2">
                                                        <label>
                                                            Para Agregar informaci&oacute;n relevante al rubro ingrese el dato y luego presione el bot&oacute;n "Agregar"
                                                        </label>
                                                        <div style="margin-top: 10px">
                                                            <%--<asp:HiddenField ID="hid_id_rubinf" runat="server" />--%>
                                                            <table>
                                                                <tr>
                                                                    <td valign="middle">Informaci&oacute;n:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEditDescripInfoRelevante" runat="server" TextMode="MultiLine" MaxLength="500" Height="100px" Width="650px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="updPnlAgregarInfoRelevante" runat="server" RenderMode="Inline">
                                                                            <ContentTemplate>
                                                                                <asp:LinkButton
                                                                                    ID="btnGuardarInfoRelevante"
                                                                                    runat="server"
                                                                                    Style="margin-left: 10px"
                                                                                    OnClick="btnGuardarInfoRelevante_Click"
                                                                                    CssClass="btn btn-inverse">
                                                    <i class="icon-white icon-plus"></i>
                                                    <span class="text">Agregar</span>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton
                                                                                    ID="btnCancelarInfoRelevante"
                                                                                    runat="server"
                                                                                    CssClass="btn btn-default"
                                                                                    OnClick="btnCancelarInfoRelevante_Click">
                                                    <i class="imoon imoon-blocked"></i>
                                                    <span class="text">Cancelar</span>
                                                                                </asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <%--grilla con info relevante que sera eliminada--%>
                                                <asp:Panel ID="pnlInfoRelevanteEliminada" runat="server" Visible="false">
                                                    <asp:Label ID="Label13" runat="server" Text="Información relevante que ha sido eliminada:"
                                                        Font-Bold="true"></asp:Label>
                                                    <asp:GridView
                                                        ID="grdInfoRelevanteEliminada"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        DataKeyNames="id_rubinf"
                                                        GridLines="None"
                                                        CssClass="table table-bordered table-striped table-hover with-check">
                                                        <Columns>
                                                            <asp:BoundField DataField="descripcion_rubinf" HeaderText="Observación" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            NO HAY DATOS ...
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>

                                            </div>
                                            <%--Configuracion contra incendio--%>
                                            <div style="padding: 15px 5px 0px 5px">
                                                <asp:Label ID="Label23" runat="server" Text="Configuración de Incendio:"
                                                    Font-Bold="true"></asp:Label>
                                                <%--grilla con datos de info relevante--%>
                                                <asp:UpdatePanel ID="pnlGrdConfIncendio" runat="server" Width="90%" style="text-align: right; padding-top: 5px">
                                                    <ContentTemplate>
                                                        <asp:GridView
                                                            ID="grdConfIncendio"
                                                            runat="server"
                                                            AutoGenerateColumns="false"
                                                            DataKeyNames="id_rubro_incendio"
                                                            Style="margin-top: 5px"
                                                            GridLines="None"
                                                            CssClass="table table-bordered table-striped table-hover with-check">
                                                            <Columns>
                                                                <asp:BoundField DataField="riesgo" HeaderText="Riesgo" />
                                                                <asp:BoundField DataField="DesdeM2" HeaderText="Sup. Desde" />
                                                                <asp:BoundField DataField="HastaM2" HeaderText="Sup. Hasta" />
                                                                <asp:TemplateField ItemStyle-Width="80px">
                                                                    <ItemTemplate>

                                                                        <asp:LinkButton ID="btnEliminarConfIncendio" runat="server" Text="Eliminar"
                                                                            CssClass="btnEliminarConfIncendio"
                                                                            CommandArgument='<%# Eval("id_rubro_incendio") %>'
                                                                            OnClick="btnEliminarConfIncendio_Click"
                                                                            OnClientClick="return confirm('¿Está seguro que desea eliminar la fila?');">
                                                                        </asp:LinkButton>

                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se han encontrado datos sobre configuracion de incendio para este rubro.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <%--Agregar nueva info relevante--%>
                                                <asp:Panel ID="pnlAccionesAgregarConfIncendio" runat="server" Width="90%" Style="text-align: right; padding-top: 5px">
                                                    <asp:LinkButton ID="lnkBtnAccionesAgregarConfIncendio" runat="server"
                                                        CssClass="btn btn-primary"
                                                        OnClick="lnkBtnAccionesAgregarConfIncendio_Click">
            <i class="icon-white icon-plus"></i>
            <span class="text">Agregar Configuración de Incendio</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>

                                                <%--panel para editar info relevante--%>
                                                <asp:Panel ID="pnlAgregarConfIncendioEdit" runat="server" Visible="false">

                                                    <div style="padding: 10px 5px 10px 5px" class="info2">
                                                        <label>
                                                            Para Agregar Configuración Incendio al rubro ingrese el dato y luego presione el bot&oacute;n "Agregar"
                                                        </label>
                                                        <div style="margin-top: 10px">
                                                            <table>
                                                                <tr>
                                                                    <td>Riesgo:</td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRiesgo" runat="server" Width="80px">
                                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Sup. Desde:</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDesde" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Sup. Hasta:</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtHasta" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="updPnlAgregarConfIncendio" runat="server" RenderMode="Inline">
                                                                            <ContentTemplate>
                                                                                <asp:LinkButton ID="btnGuardarConfIncendio"
                                                                                    runat="server"
                                                                                    Style="margin-left: 10px"
                                                                                    OnClick="btnGuardarConfIncendio_Click"
                                                                                    CssClass="btn btn-inverse">
                                            <i class="icon-white icon-plus"></i>
                                            <span class="text">Agregar</span>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton
                                                                                    ID="btnCancelarConfIncendio"
                                                                                    runat="server"
                                                                                    OnClick="btnCancelarConfIncendio_Click"
                                                                                    CssClass="btn btn-default">
                                            <i class="imoon imoon-blocked"></i>
                                            <span class="text">Cancelar</span>
                                                                                </asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </div>
                                                    </div>

                                                </asp:Panel>

                                                <%--grilla con info relevante que sera eliminada--%>
                                                <asp:Panel ID="pnlConfIncendioEliminada" runat="server" Visible="false">

                                                    <asp:Label ID="Label24" runat="server" Text="Configuracion de incendio que ha sido eliminada:"
                                                        Font-Bold="true"></asp:Label>

                                                    <asp:GridView
                                                        ID="grdConfIncendioEliminada"
                                                        runat="server"
                                                        AutoGenerateColumns="false"
                                                        DataKeyNames="id_rubro_incendio"
                                                        GridLines="None"
                                                        CssClass="table table-bordered table-striped table-hover with-check">
                                                        <Columns>
                                                            <asp:BoundField DataField="riesgo" HeaderText="Riesgo" />
                                                            <asp:BoundField DataField="DesdeM2" HeaderText="Sup. Desde" />
                                                            <asp:BoundField DataField="HastaM2" HeaderText="Sup. Hasta" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            NO HAY DATOS ...
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </div>

                                        </div>
                                    </div>


                                </td>
                            </tr>

                            <br />
                            <%--****************	0138775: JADHE 53248 - SGI - REQ ABM Rubros CUR - Documentacion obligatoria     ***************--%>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="Label9" runat="server" Text="Observaciones:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="6" Width="90%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Label ID="lblObservacionesSolicitante" runat="server" Text="Observaciones del Solicitante:"></asp:Label>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="txtObservacionesSolicitante" runat="server" MaxLength="6" Width="90%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:LinkButton ID="btnGuardarRubroCN" runat="server" Style="margin-left: 10px" CssClass="btn btn-primary"
                                        ValidationGroup="CrearRubro" OnClick="btnGuardarRubroCN_Click">
                                <i class="icon-white icon-pencil"></i>
                                <span class="text">Guardar</span>
                                    </asp:LinkButton>
                                </td>
                                <td class="col2">
                                    <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" PostBackUrl="~/ABM/RubrosCUR/AbmRubrosCUR.aspx">
                                <i class="imoon imoon-blocked"></i>
                                <span class="text">Cancelar</span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--modal de Errores--%>
    <div id="frmErrorVisual" class="modal fade">
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
</asp:Content>

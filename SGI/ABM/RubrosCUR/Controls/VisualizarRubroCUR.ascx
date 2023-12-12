<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualizarRubroCUR.ascx.cs" Inherits="SGI.Controls.VisualizarRubroCUR" %>


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
<%: Scripts.Render("~/bundles/autoNumeric") %>

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
<link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

    function inicializar_dropdownlists() {
        $("#<%: ddlTipoTramite.ClientID %>").select2();
        $("#<%: ddlTipoActividad.ClientID %>").select2();
        $("#<%: ddlCircuito.ClientID %>").select2();
        $("#<%: ddlEstacionamiento.ClientID %>").select2();
        $("#<%: ddlRubrosBicicleta.ClientID %>").select2();
        $("#<%: ddlCyD.ClientID %>").select2();
    }

    function init_updDatos() {
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
    }
</script>

<asp:HiddenField ID="hid_DecimalSeparator" runat="server" />

<asp:Panel ID="pnlDatosRubro" runat="server">
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
                        <asp:TextBox ID="txtCodRubro" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="ReqtxtCodRubro" runat="server" ControlToValidate="txtCodRubro" CssClass="error"
                                ErrorMessage="El código de rubro es obligatorio." Display="Dynamic" ValidationGroup="CrearRubro">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CusValCodRubroUnique" runat="server" CssClass="error" Display="Dynamic" ControlToValidate="txtCodRubro"
                             ValidationGroup="CrearRubro" onservervalidate="CusValCodRubroUnique_ServerValidate">
                            </asp:CustomValidator>
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
                            <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="500px" AutoPostBack="true">
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
                        <asp:CheckBox ID="ChkLibrado" runat="server" />
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
                        <asp:Label ID="Label10" runat="server" Text="Sin Baño PCD hasta 60m²"></asp:Label>
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
                        <asp:Label ID="lblEstacionamientoDesc" Width="90%" runat="server" Font-Bold="True"></asp:Label>
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
                        <asp:Label ID="lblBicicletaDesc" Width="90%" runat="server" Font-Bold="True"></asp:Label>
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
                        <asp:Label ID="lblCyD" Width="90%" runat="server" Font-Bold="True"></asp:Label>
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
                    <td class="col1">
                        <asp:Label ID="Label9" runat="server" Text="Observaciones:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="6" Width="90%" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:Label ID="Label11" runat="server" Text="Observaciones del Solicitante:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtObservacionesSolicitantes" runat="server" MaxLength="6" Width="90%" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <asp:LinkButton ID="btnCrearRubroCN" runat="server" Style="margin-left: 10px" CssClass="btn btn-primary"
                            ValidationGroup="CrearRubro" OnClick="btnCrearRubroCN_Click">
                                <i class="icon-white icon-plus"></i>
                                <span class="text">Agregar Rubro</span>                            
                        </asp:LinkButton>
                    </td>
                    <td class="col2">
                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();">
                                <i class="imoon imoon-blocked"></i>
                                <span class="text">Cancelar</span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Panel>

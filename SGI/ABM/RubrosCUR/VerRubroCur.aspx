<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerRubroCUR.aspx.cs" Inherits="SGI.ABM.AbmRubrosCurVisualizar" %>

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
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
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
                            <asp:TextBox ID="txtCodRubro" runat="server" MaxLength="6" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="lblDescRubro" runat="server" Text="Descripción del Rubro:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtDescRubro" runat="server" MaxLength="300" Enabled="false"
                                TextMode="MultiLine" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="lblBusqueda" runat="server" Text="Palabras Claves:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtBusqueda" runat="server" MaxLength="400" TextMode="MultiLine" Enabled="false"
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
                            <asp:TextBox ID="txtToolTip" runat="server" MaxLength="2000" TextMode="MultiLine" Enabled="false"
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
                            <asp:TextBox ID="txtFechaVigenciaHasta" runat="server" Width="80px" Enabled="false"></asp:TextBox>
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
                                <asp:DropDownList ID="ddlTipoActividad" runat="server" Width="500px" Enabled="false">
                                </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="lblTipoTramite" runat="server" Text="Tipo de Trámite:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:Panel ID="pnlTipoTramite" runat="server" Width="350px">
                                <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="500px" Enabled="false">
                                </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label20" runat="server" Text="Grupo Circuito:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:Panel ID="pnlCircuito" runat="server" Width="350px">
                                <asp:DropDownList ID="ddlCircuito" runat="server" Width="500px" Enabled="false">
                                </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label21" runat="server" Text="Librado al Uso:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:CheckBox ID="ChkLibrado" runat="server" Enabled="false" />
                        </td>
                    </tr>

                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label14" runat="server" Text="Condicion Express:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:CheckBox ID="ChkExpress" runat="server" Enabled="false" />
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
                            <asp:Label ID="Label8" runat="server" Text="Asistentes 350:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:CheckBox ID="chkAsistentes350" runat="server" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label13" runat="server" Text="Sin Baño PCD hasta 60m²"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:CheckBox ID="chkSinBanioPCD" runat="server" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label1" runat="server" Text="Zona Mixtura 1:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtZonaMixtura1" runat="server" MaxLength="50" Width="90%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label2" runat="server" Text="Zona Mixtura 2:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtZonaMixtura2" runat="server" MaxLength="50" Width="90%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label3" runat="server" Text="Zona Mixtura 3:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtZonaMixtura3" runat="server" MaxLength="50" Width="90%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label4" runat="server" Text="Zona Mixtura 4:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtZonaMixtura4" runat="server" MaxLength="50" Width="90%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label5" runat="server" Text="Estacionamiento:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:Panel ID="Panel1" runat="server" Width="350px">
                                <asp:DropDownList ID="ddlEstacionamiento" runat="server" Width="80px" Enabled="false">
                                </asp:DropDownList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="lblEstacionamientoInfo" runat="server" Text="Descripción Estacionamiento: " Visible="false"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:Label ID="lblEstacionamientoDesc" Width="90%" runat="server" Enabled="false"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label6" runat="server" Text="Bicicleta:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:Panel ID="Panel2" runat="server" Width="350px">
                                <asp:DropDownList ID="ddlRubrosBicicleta" runat="server" Width="80px" Enabled="false">
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
                                <asp:DropDownList ID="ddlCyD" runat="server" Width="80px" Enabled="false">
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
                                <asp:DropDownList ID="ddlCondicionesIncendio" runat="server" Width="180px" Enabled="false">
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
                        <td></td>
                        <td colspan="2">
                            <div class="widget-box">
                                <div class="widget-title ">
                                    <span class="icon"><i class="imoon imoon-hammer"></i></span>
                                    <h5>Documentos requeridos</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="control-group">
                                        <%--grilla con datos de documentos requeridos--%>
                                        <div>
                                            <div>
                                                <asp:Label ID="Label10" runat="server" Text="Configuración de Documentación Requerida para todas las zonas:" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:UpdatePanel
                                                    ID="pnlGrdDocReq"
                                                    runat="server"
                                                    Width="85%"
                                                    style="text-align: right; padding-top: 5px">
                                                    <ContentTemplate>
                                                        <asp:GridView
                                                            ID="grdDocReq"
                                                            runat="server"
                                                            AutoGenerateColumns="false"
                                                            DataKeyNames="id_rubtdocreq"
                                                            Style="margin-top: 5px"
                                                            GridLines="None"
                                                            CssClass="table table-bordered table-striped table-hover with-check">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombre_tdocreq" HeaderText="Tipo Documento" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="es_obligatorio" HeaderText="Obligatorio" HeaderStyle-Width="10%" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se han encontrado Documentos Requeridos para este rubro.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                        <%--grilla con datos de info relevante--%>
                                        <div>
                                            <div>
                                                <asp:Label ID="Label11" runat="server" Text="Configuración de Información Relevante:" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:UpdatePanel ID="pnlGrdInfoRelevante" runat="server" Width="90%" style="text-align: right; padding-top: 5px">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdInfoRelevante"
                                                            runat="server"
                                                            AutoGenerateColumns="false"
                                                            DataKeyNames="id_rubinf"
                                                            Style="margin-top: 5px"
                                                            GridLines="None"
                                                            CssClass="table table-bordered table-striped table-hover with-check">
                                                            <Columns>
                                                                <asp:BoundField DataField="descripcion_rubinf" HeaderText="Observación" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se han encontrado datos sobre información relevante para este rubro.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                        <%--grilla con datos de info relevante--%>
                                        <div>
                                            <div>
                                                <asp:Label ID="Label12" runat="server" Text="Configuración de Incendio:" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:UpdatePanel ID="pnlGrdConfIncendio" runat="server" Width="90%" style="text-align: right; padding-top: 5px">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdConfIncendio" runat="server" AutoGenerateColumns="false"
                                                            DataKeyNames="id_rubro_incendio" Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                                            <Columns>
                                                                <asp:BoundField DataField="riesgo" HeaderText="Riesgo" />
                                                                <asp:BoundField DataField="DesdeM2" HeaderText="Sup. Desde" />
                                                                <asp:BoundField DataField="HastaM2" HeaderText="Sup. Hasta" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se han encontrado datos sobre configuracion de incendio para este rubro.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:Label ID="Label9" runat="server" Text="Observaciones:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="6" Width="90%" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1"></td>
                        <td class="col2">
                            <div style="width: 90%">
                                <asp:GridView ID="grdImpactoAmbiental" runat="server" AutoGenerateColumns="false"
                                    Style="margin-top: 5px" GridLines="None" CssClass="table table-bordered table-striped table-hover with-check">
                                    <Columns>
                                        <asp:BoundField DataField="nom_impactoAmbiental" HeaderText="Impacto Ambiental" HeaderStyle-Width="40%" ItemStyle-Width="40%" FooterStyle-Width="40%" />
                                        <asp:BoundField DataField="DesdeM2" HeaderText="Desde m2" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="HastaM2" HeaderText="Hasta m2" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="LetraAnexo" HeaderText="Letra" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="col2">
                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" PostBackUrl="~/ABM/RubrosCUR/AbmRubrosCUR.aspx">
                                <i class="imoon imoon-reply"></i>
                                <span class="text">Volver</span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

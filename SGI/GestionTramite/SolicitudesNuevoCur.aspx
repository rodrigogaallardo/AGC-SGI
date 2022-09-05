<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitudesNuevoCur.aspx.cs" Inherits="SGI.GestionTramite.SolicitudesNuevoCur" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_controles();
            camposAutonumericos();
            inicializar_fechas();
            loadPopOverRubro();

        });

        function loadPopOverRubro() {
            $("[id*='lnkRubros']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

            // Popovers rubros de la bandeja propia
            $("[id*='MainContent_grdTramites_lnkRubros_']").each(function () {

                var id_pnlRubros = $(this).attr("id").replace("MainContent_grdTramites_lnkRubros_", "MainContent_grdTramites_pnlRubros_");
                var objRubros = $("#" + id_pnlRubros).html();
                $(this).popover({ title: 'Rubros', content: objRubros, html: 'true' });

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

        function inicializar_controles() {
            $("#<%: ddlEstado.ClientID %>").select2({ allowClear: true });
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

        function camposAutonumericos() {
            $('#<%=txtNroSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        }

        function inicializar_fechas() {
            var fechaDesde = $('#<%=txtFechaDesde.ClientID%>');
            var es_readonly = $(fechaDesde).attr("readonly");

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

            ///FechaHasta
            var fechaCierreDesde = $('#<%=txtFechaHasta.ClientID%>');
            var es_readonlyCierre = $(fechaCierreDesde).attr("readonly");

            $("#<%: txtFechaHasta.ClientID %>").datepicker({
                minDate: "-100Y",
                maxDate: "0Y",
                yearRange: "-100:-0",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                    $("#Req_FechaCierreDesde").hide();
                    $("#Val_Formato_FechaCierreDesde").hide();
                    $("#Val_FechaCierreDesdeMenor").hide();
                }
            });


            if (!($(fechaCierreDesde).is('[disabled]') || $(fechaCierreDesde).is('[readonly]'))) {
                $(fechaCierreDesde).datepicker(
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

        };

        function updPanel() {
            return false;
        };

        function popOverRubros(obj) {
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

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");
        }

    </script>

    <br />
    <asp:Panel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
        <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
            <%-- titulo tramite --%>
            <div class="accordion-heading">
                <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                    <div class="widget-title">
                        <span class="icon"><i class="icon-list-alt"></i></span>
                        <h5>
                            <asp:Label ID="bt_tramite_tituloControl" runat="server" Text="Trámites"></asp:Label></h5>
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
                                <fieldset>
                                    <div class="row">
                                        <div class="span5">
                                            <asp:Label ID="lblNroSolicitud" runat="server" AssociatedControlID="txtNroSolicitud"
                                                Text="Solicitud:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtNroSolicitud" runat="server" MaxLength="10" Width="110px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="span5">
                                            <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado"
                                                Text="Estado del Trámite:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlEstado" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:HiddenField ID="hid_estados_selected" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="span5">
                                            <asp:Label ID="lblFechaDesde" runat="server" AssociatedControlID="txtFechaDesde"
                                                Text="Fecha Desde:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="10" Width="110px"></asp:TextBox>
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
                                    </div>
                                    <div class="row">
                                        <div class="span5">
                                            <asp:Label ID="lblFechaHasta" runat="server" AssociatedControlID="txtFechaHasta"
                                                Text="Fecha Hasta:" class="control-label"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="110px"></asp:TextBox>
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
                                </fieldset>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <br />
        <!-- Botones -->
        <asp:UpdatePanel ID="upd_BuscarTramite" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="pull-right">
                    <div class="control-group inline-block">
                        <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="upd_BuscarTramite"
                            runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnBuscar_OnClick">
                    <i class="icon-white icon-search"></i>
                    
                    <span class="text">Buscar</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick">
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
                    </asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <!-- Grilla -->
        <div id="box_resultado" class="widget-box" style="display: none">
            <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server">
                <ContentTemplate>

                    <div style="margin-left: 10px; margin-right: 10px; overflow-x: scroll;">
                        <asp:Panel ID="pnlResultadoBuscar" runat="server">
                            <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false" Style="display: inline-block">
                                <div style="display: inline-block">
                                    <h5>Lista de Tr&aacute;mites</h5>
                                </div>
                                <div style="display: inline-block">
                                    (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>)
                                </div>
                            </asp:Panel>
                            <asp:LinkButton ID="btnExportarExcel" runat="server" CssClass="btn btn-success pull-right" OnClientClick="return showfrmExportarExcel();" OnClick="btnExportarExcel_Click" Style="margin-top: 10px; margin-bottom: 10px">
                                        <i class="imoon imoon-file-excel color-white"></i>
                                        <span>Exportar a Excel</span>
                            </asp:LinkButton>

                            <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                DataKeyNames="id_solicitud"
                                SelectMethod="GetTramites" ItemType="SGI.Model.clsItemConsultaTramiteNuevoCur"
                                AllowPaging="true" AllowSorting="true" PageSize="30"
                                OnPageIndexChanging="grdTramites_PageIndexChanging"
                                OnRowDataBound="grdBandeja_RowDataBound"                               
                                OnDataBound="grdTramites_DataBound">
                                <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                                <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                                <Columns>
                                    <asp:BoundField DataField="Id_tad" HeaderText="Tad" />
                                    <asp:BoundField DataField="Id_solicitud" HeaderText="Solicitud" />
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRubros" runat="server" OnClientClick="return popOverRubros(this);" data-toggle="popover" 
                                                data-visible="false" data-placement="right"
                                                CommandArgument="<%# Item.Id_solicitud %>" title="Lista de rubros"><i class="icon-share"></i>
                                            </asp:LinkButton>
                                            <%--Popover con la lista de Rubros--%>
                                            <asp:Panel ID="pnlRubros" runat="server" Style="display:none;min-width: 800px; padding: 10px">
                                                <asp:DataList ID="lstRubros" runat="server" Width="500px" CssClass="table table-bordered table-striped">
                                                    <ItemTemplate>
                                                        <div class="inline">
                                                            <asp:Label ID="lblCodRubro" runat="server" CssClass="badge badge-info"><%# Eval("Codido") %></asp:Label>
                                                        </div>
                                                        <div class="inline">
                                                            <asp:Label ID="lblDescRubro" runat="server" CssClass="pLeft5"><%# Eval("Descripcion") %></asp:Label>
                                                        </div>
                                                        <div class="inline">
                                                            <asp:Label ID="lblSuperRubro" runat="server" CssClass="pLeft5"><%# Eval("Superficie") %></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Nombre_RazonSocial" HeaderText="Nombre - Razón Social" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Cuit" HeaderText="Cuit" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Nombre_Profesional" HeaderText="Nombre Profesional" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Matricula" HeaderText="Matricula" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="NroPartidaMatriz" HeaderText="Partida Matriz" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="NroPartidaHorizontal" HeaderText="Partida Horizontal" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Calle" HeaderText="Calle" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Altura_calle" HeaderText="Altura Calle" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="UnidadFuncional" HeaderText="Unidad Funcional" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Superficie" HeaderText="Superficie" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Mixtura" HeaderText="Mixtura" ItemStyle-CssClass="align-center" />
                                    <asp:BoundField DataField="Fecha_confirmacion" HeaderText="Fecha Confirmación" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkVerDoc" runat="server"
                                                CssClass="btnVerPdf20" Target="_blank"
                                                NavigateUrl='<%# ResolveUrl("~/Reportes/Imprimir_SolicitudNuevoCur.aspx?id=" + Eval("Id_solicitud")) %>'
                                                Text="Ver" Width="40px">
                                            </asp:HyperLink>
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
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- Fin Grilla -->
    </asp:Panel>

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
</asp:Content>

<%@ Page Title="Panel de Estados" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Estado_Pagos.aspx.cs"
    Inherits="SGI.GestionTramite.Estado_Pagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>--%>
    
    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/Unicorn.Tables") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
  
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
    
<script type="text/javascript">

    $(document).ready(function () {
        inicializar_controles();
    });

    function mostrarMensaje(texto, titulo) {
        $.gritter.add({
            title: titulo,
            text: texto,
            image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

    function inicializar_controles() {
        inicializar_popover();
        inicializar_fechas();
    }

    function toolTips() {
        $("[data-toggle='tooltip']").tooltip();
        return false;

    }

    function inicializar_popover() {

        $('[rel=popover]').popover({
            html: 'true',
            placement: 'right'
        })

    }

    function validarGuardar() {
        var ret = true;


        if ($.trim($("#<%: txtFechaDesde.ClientID %>").val()).length <= 9) {
           ret = false;
            }

        if ($.trim($("#<%: txtFechaHasta.ClientID %>").val()).length <= 9) {
            ret = false;
        }
        if (ret == true)
            showfrmExportarExcel();

        return ret;

        }

    function inicializar_fechas() {

        var fechaDesde = $('#<%=txtFechaDesde.ClientID%>');
        var es_readonly = $(fechaDesde).attr("readonly");
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

        }

        var fechaHasta = $('#<%=txtFechaHasta.ClientID%>');
        var es_readonly = $(fechaHasta).attr("readonly");
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



        }


    }

</script>
    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />
    <hgroup class="title">
        <h1>Panel de Estados.</h1>
    </hgroup>
    
    <asp:Panel ID="pnlBotonDefault" runat="server">

    <asp:UpdatePanel ID="updPnlEstados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- filtros de busqueda--%>
            <div class="accordion-group widget-box">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>Panel de estados</h5>
                </div>

                <div class="widget-content">
                    <div class="form-horizontal">
                        <fieldset>
                            <div class="row-fluid">
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblJobEjecVencido" runat="server" AssociatedControlID="txtJobEjecVencido" Text="Ultimo proceso ejecutado Vencimiento de boletas:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:Label ID="txtJobEjecVencido" Style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblJobEjecBolVencido" runat="server" AssociatedControlID="txtJobEjecBolVencido" Text="Ultimo proceso ejecutado actualización de pago post-Vencimiento:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:Label ID="txtJobEjecBolVencido" Style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblJobEjecBoletas" runat="server" AssociatedControlID="txtJobEjecBoletas" Text="Ultimo proceso ejecutado de validación de estados de boletas:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:Label ID="txtJobEjecBoletas" Style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblTotalVencido" runat="server" AssociatedControlID="txtTotalVencido" Text="Total boletas vencidas hoy:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:Label ID="txtTotalVencido" Style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblTotalBolVencido" runat="server" AssociatedControlID="txtTotalBolVencido" Text="Total boletas actualizadas post-Vencimiento hoy:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:Label ID="txtTotalBolVencido" Style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblTotalBoleta" runat="server" AssociatedControlID="txtTotalBoleta" Text="Total boletas pendientes actualizadas hoy:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:Label ID="txtTotalBoleta" Style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>

            </div>

            
            <div class="accordion-group widget-box">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>Historial de Recuperos</h5>
                </div>

                <div class="widget-content">
                    <div class="form-horizontal">
                        <fieldset>
                            <div class="row-fluid">
                                <div class="span5">
                                
                            <div id="info_Vencimiento" class="alert alert-small alert-info mbottom0 mtop0 mright0" >
                                                Se descarga una lista de BUI recuperadas post-vencimiento entre las fechas declaradas.
                                            </div>
                            </div>
                            </div>
                            
                            <div class="row-fluid">
                                
                                
                        <div class="span4">

                            <div class="control-group" >

                            <label for="txtFechaDesde" class="control-label">Fecha Desde:</label>

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

                        </div>

                        <div class="span4">

                            <div class="control-group">
                                <label for="txtFechaHasta" class="control-label">Fecha Hasta:</label>

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
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:LinkButton ID="lnkpostvencidasExportar" runat="server"  CssClass="btn btn-inverse"   OnClick="lnkpostvencidasExportar_Click"   >
                                                <i class="imoon-white imoon-arrow-down"></i>
                                                <span class="text" Style="color:#fff" >Exportar Listado</span>
                                            </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


        
    <br />
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
                                    <h2><asp:Label ID="lblRegistrosExportados" runat="server"></asp:Label></h2>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDescargarExcel" runat="server" style="display:none">
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
    <script>
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
    </asp:Panel>
</asp:Content>

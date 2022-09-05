<%@ Page Title="Tarea: Resultado de Inspección" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Resultado_Inspector.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Resultado_Inspector" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaDocumentoAdjuntoAnteriores.ascx" TagPrefix="uc1" TagName="ucSGI_ListaDocumentoAdjuntoAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_DocumentoAdjunto.ascx" TagPrefix="uc1" TagName="ucSGI_DocumentoAdjunto" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
            function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
    <uc1:ucTramitesRelacionados runat="server" id="ucTramitesRelacionados" />
    <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
    
    <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
    <asp:HiddenField ID="hid_id_tramitetarea" runat="server"  Value="0"/>

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5><%: Page.Title %></h5>
        </div>



        <div class="widget-content">
            <div style="padding: 20px">
                <div style="width: 100%;">
                    
                    <uc1:ucSGI_ListaDocumentoAdjuntoAnteriores runat="server" ID="ucSGI_ListaDocumentoAdjuntoAnteriores" />

                    <uc1:ucSGI_DocumentoAdjunto runat="server" ID="ucSGI_DocumentoAdjunto" />

                    <br />

                    <asp:Panel ID="pnl_area_tarea" runat="server">

                        <div class="form-inline">
                            <div class="control-group">
                                <label class="form-control">Fecha Inspecci&oacute;n:</label>

                                <div class="input-prepend">
                                    <asp:TextBox ID="txtFechaInspeccion" runat="server" MaxLength="10"
                                            Width="90px" CssClass="form-control" >
                                    </asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="form-inline">
                           <asp:RequiredFieldValidator ID="rfv_txtFechaInspeccion" runat="server" 
                                ValidationGroup="finalizar"
                                ControlToValidate="txtFechaInspeccion" CssClass="field-validation-error" 
                                ErrorMessage="Debe ingresar este campo."
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator
                                ID="rev_txtFechaInspeccion" runat="server" 
                                ValidationGroup="finalizar"
                                ControlToValidate="txtFechaInspeccion" CssClass="field-validation-error" 
                                ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                Display="Dynamic">
                            </asp:RegularExpressionValidator>    
                        </div>

                  
                    </asp:Panel>

                    <br />

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" ValidationGroupFinalizar="finalizar" />

                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_fechas();
        });

        function inicializar_fechas() {
            var fechaInspeccion = $('#<%=txtFechaInspeccion.ClientID%>');
            var es_readonly = $(fechaInspeccion).attr("readonly");
            if ( ! ( $(fechaInspeccion).is('[disabled]') || $(fechaInspeccion).is('[readonly]') ) ) {
                $(fechaInspeccion).datepicker( 
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

    </script>

</asp:Content>

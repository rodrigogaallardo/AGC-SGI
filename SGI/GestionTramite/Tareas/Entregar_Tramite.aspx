<%@ Page Title="Tarea: Entregar Trámite" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Entregar_Tramite.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Entregar_Tramite" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_DocumentoAdjunto.ascx" TagPrefix="uc1" TagName="ucSGI_DocumentoAdjunto" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>


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
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>

    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <script type="text/javascript">

        $(document).ready(function () {
            inicializar_controles();
        });


        function inicializar_controles() {
            inicializar_fechas();
            camposAutonumericos();
        }

        function camposAutonumericos() {
   
        }

        function inicializar_fechas() {
            var fechaInspeccion = $('#<%=txtFechaEntregaTramite.ClientID%>');
            var es_readonly = $(fechaInspeccion).attr("readonly");
            if (!($(fechaInspeccion).is('[disabled]') || $(fechaInspeccion).is('[readonly]'))) {
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
                        yearSuffix: '',
                        beforeShowDay: noExcursion
                    }
                );

            }
        }

        //funcion que bloquea todos lo dias expecto los que queremos habilitar para la seleccion
        function noExcursion(date) {
            var day = date.getDay();
            // aqui indicamos el numero correspondiente a los dias que ha de bloquearse (el 0 es Domingo, 1 Lunes, etc...) en el ejemplo bloqueo todos menos los domingos y sabado.
            return [(day != 0 && day != 6), ''];
        };
    </script>

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

                    <uc1:ucSGI_DocumentoAdjunto runat="server" ID="ucSGI_DocumentoAdjunto" />

                    <div class="widget-box">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>Personas Habilitadas</h5>
                        </div>
                        <div class="widget-content">
                            <asp:Repeater ID="rptPersonasHabilitadas" runat="server"
                                    OnItemDataBound="rptPersonasHabilitadas_OnItemDataBound" >
                                <ItemTemplate>
                                    <asp:GridView ID="grdPersonasHabilitados" runat="server" AutoGenerateColumns="false"
                                        ItemType="SGI.GestionTramite.Tareas.Entregar_Tramite.PersonasHabilitadas"
                                        ShowFooter="true" 
                                        OnRowDataBound="grdPersonasHabilitados_RowDataBound"
                                        GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"                 >

                                        <Columns>
                                            <asp:BoundField DataField="TipoPersonaDesc" HeaderText="Tipo Persona" HeaderStyle-HorizontalAlign="Left"  ItemStyle-Width="100px" />
                                            <asp:TemplateField HeaderText="Apellido y Nombre/s" ItemStyle-Width="200px" FooterStyle-Width="200px" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNombre" runat="server" Text='<%# Item.ApellidoNomRazon%>' Width="200px"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div class="widget-title" style="padding-bottom: 0px; border-bottom: 0px; margin-bottom:0px">
                                                        <span class="icon"><i class="icon-asterisk"></i></span>
                                                        <h5>Firmantes</h5>
                                                    </div>

                                                    <asp:GridView ID="grdFirmantesHabilitados" runat="server" AutoGenerateColumns="false"
                                                        ItemType="SGI.GestionTramite.Tareas.Entregar_Tramite.FirmantesHabilitados"
                                                        GridLines="None" CssClass="table table-bordered table-striped table-hover with-check" 
                                                        style="padding-top:0; border-top:0; margin-top:0"                >
                                                        <Columns>
                                                            <asp:BoundField DataField="ApellidoNombres" HeaderText="Apellido y Nombre/s" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="310px"/>
                                                            <asp:BoundField DataField="DescTipoDocPersonal" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Left"  ItemStyle-Width="50px"/>
                                                            <asp:BoundField DataField="Nro_Documento" HeaderText="Documento" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"/>
                                                            <asp:BoundField DataField="nom_tipocaracter" HeaderText="Carácter Legal" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"/>
                                                            <asp:BoundField DataField="cargo_firmante_pj" HeaderText="Cargo" HeaderStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                    </asp:GridView>
                                                 </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Cuit" HeaderText="CUIT" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"/>
                                            <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <asp:Panel ID="pnl_area_tarea" runat="server">
                        <div class="form-horizontal">
                            <fieldset>
                                <div class="control-group">
                                    <label class="control-label">Nro. Expediente</label>
                                    <div class="controls" style="">
                                         <asp:TextBox ID="txtNroExpediente" runat="server" 
                                                class="uneditable-input" Enabled="false" Width="300px">
                                         </asp:TextBox>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <div class="span3">
                                        <label class="control-label" for="txtFechaEntregaTramite">Fecha Entrega del Trámite:</label>
                                         <div class="controls">
                                            <asp:TextBox ID="txtFechaEntregaTramite" runat="server" MaxLength="10" Width="90px">
                                            </asp:TextBox>
                                            <div class="req">
                                               <asp:RequiredFieldValidator ID="rfv_txtFechaEntregaTramite" runat="server"
                                                   ControlToValidate="txtFechaEntregaTramite" CssClass="field-validation-error"
                                                   ErrorMessage="Debe ingresar la fecha de entrega del trámite." ValidationGroup="finalizar"
                                                   Display="Dynamic" Enabled="true">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="rev_txtFechaEntregaTramite" runat="server"
                                                    ControlToValidate="txtFechaEntregaTramite" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    ValidationGroup="finalizar" Display="Dynamic" Enabled="true">
                                                </asp:RegularExpressionValidator>   
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span3">
                                        <label class="control-label" for="ddlEnviar">Enviar a:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlEnviar" runat="server" Width="120px">
                                                <%--<asp:ListItem Text="" Value="0"></asp:ListItem>--%>
                                                <%--<asp:ListItem Text="DGFyC" Value="SGOI"></asp:ListItem>--%>
                                                <%--<asp:ListItem Text="GUARDA" Value="025"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <div class="req">
                                                <asp:RequiredFieldValidator ID="Req_ddlTiposInforme" runat="server"
                                                    ControlToValidate="ddlEnviar" CssClass="field-validation-error"
                                                    ErrorMessage="Debe ingresar el Informe" ValidationGroup="finalizar"
                                                    Display="Dynamic" Enabled="true" InitialValue="0" />
                                            </div>

                                        </div>                                    
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </asp:Panel>

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucProcesosSADEv1 runat="server" id="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" ValidationGroupFinalizar="finalizar"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnResultadoSelectedIndexChanged="ucResultadoTarea_ResultadoSelectedIndexChanged"
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />

                </div>
            </div>
        </div>
    </div>



</asp:Content>

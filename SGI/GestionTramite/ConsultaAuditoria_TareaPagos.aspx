<%@ Title="Auditoria Tareas de Pagos"  Language="C#"  MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="ConsultaAuditoria_TareaPagos.aspx.cs" 
    Inherits="SGI.ConsultaAuditoria_TareaPagos" %>

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
    <%: Scripts.Render("~/bundles/autoNumeric") %>
  
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">
        
        <div class="accordion-group widget-box" style="margin-top:0px; margin-bottom:5px">
            
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Buscar Registro de Pago</h5>
            </div>
            
            <div class="widget-content">

    <asp:UpdatePanel ID="updPnlFiltroBuscar" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>



                <div class="form-horizontal">
                <fieldset>


                    <div class="row-fluid">

                        <div class="span6">

                            <div class="control-group">
                                <label for="txtNroSolicitud" class="control-label">Nro. Solicitud:</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtNroSolicitud" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="span6">

                            <div class="control-group">
                                <label for="txtNroBoleta" class="control-label">Nro. Boleta:</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtNroBoleta" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                                </div>
                            </div>

                        </div>


                    </div>




                    <div class="row-fluid">

                        <div class="span6">

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

                        <div class="span6">

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


                    </div>

                    <div class="form-horizontal">
                    <div class="control-group">
                        <label for="ddlTareas" class="control-label">Tarea:</label>

                        <div class="controls">
                            <asp:DropDownList ID="ddlTareas" runat="server" Width="150px" CssClass="form-control" >
                            </asp:DropDownList>
                        </div>
                    </div>
                        </div>
                </fieldset>
                </div>



    </ContentTemplate>
    </asp:UpdatePanel>
</div>
    </div>
    <br />

    <asp:UpdatePanel ID="updPnlBuscarPagos" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                function endRequestHandler() {
                    inicializar_controles();
                }
            </script>  
                 
            <div class="pull-right">  
                
                                        <div class="control-group inline-block">

                <asp:UpdateProgress ID="updPrgss_BuscarPagos" AssociatedUpdatePanelID="updPnlBuscarPagos"
                    runat="server" DisplayAfter="0"  >
                    <ProgressTemplate>
                        <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>  

            </div>                                                   
                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" 
                    ValidationGroup="buscar" OnClick="btnBuscar_OnClick" >
                    <i class="icon-white icon-search"></i>
                    <span class="text">Buscar</span>
                </asp:LinkButton>


                <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick"  >
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
                </asp:LinkButton>

            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

    <br /> <br />

    </asp:Panel>

    
    <div id="box_resultado" class="widget-box"style="display:none;">
     
    <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
            <div style="margin-left:10px; margin-right:10px">
        <asp:Panel ID="pnlResultadobuscar" runat="server" Visible="false">


            <asp:Panel ID="pnlCantRegistros" runat="server" >

                <div style="display: inline-block">
                    <h5>Lista de Auditoria</h5>
                </div>
                <div style="display: inline-block">
                    (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server" ></asp:Label></span>
                    )
                </div>

            </asp:Panel>

             <%--OnRowDataBound="grdAuditoriaPagos_RowDataBound"--%>
            <asp:GridView ID="grdAuditoriaPagos" runat="server" AutoGenerateColumns="false"
                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                DataKeyNames="id" 
                AllowPaging="true" PageSize="30" OnPageIndexChanging="grdAuditoriaPagos_PageIndexChanging"
                OnDataBound="grdAuditoriaPagos_DataBound">

                <Columns>

                    <asp:BoundField DataField="id_solicitud" HeaderText="Nro. Solicitud" ItemStyle-Width="80px" ItemStyle-CssClass="align-center"  />
                    <asp:BoundField DataField="nro_boleta_unica" HeaderText="Nro. Boleta" ItemStyle-Width="80px" ItemStyle-CssClass="align-center"/>
                    <asp:BoundField DataField="nombre_tarea" HeaderText="Tarea" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="fecha_inicio" HeaderText="Procesada" DataFormatString="{0:d}" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" />
                    <asp:BoundField DataField="mensaje" HeaderText="Resultado"  />


                </Columns>
                <EmptyDataTemplate>


                    <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                        <img src="../Content/img/app/NoRecords.png" />
                       
                            No se encontraron registros con los filtros ingresados.
                       
                    </asp:Panel>                    
                </EmptyDataTemplate>

                <PagerTemplate>

                        <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                            <div style="display:inline-table">
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



<script type="text/javascript">

    $(document).ready(function () {
        inicializar_controles();
    });

    function inicializar_controles() {
        inicializar_fechas();
        camposAutonumericos();
    }

    function camposAutonumericos() {
        $('#<%=txtNroSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        $('#<%=txtNroBoleta.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
    }
    function showResultado() {
        $("#box_resultado").show("slow");
    }

    function hideResultado() {
        $("#box_resultado").hide("slow");
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

</asp:Content>


<%@ Page Title="Indicadores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndicadoresSolicitudes.aspx.cs" Inherits="SGI.GestionTramite.IndicadoresSolicitudes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

  <%--  <script src="../Scripts/Select2-locales/select2_locale_es.js"></script>
    <script src="../Scripts/Funciones.js" type="text/javascript"></script>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>--%>
    
     <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>


    <script type="text/javascript">


        $(document).ready(function () {
            inicializar_controles();
            inicializar_controlesB3();
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#<%: btnCargarDatos.ClientID %>").click();


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
            inicializar_autocomplete();
        }
        function init_Js_updpnlBuscar() {
            $("#<%: txtBusFechaRevisionAnio.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999' });
            $("#<%: txtBusFechaRevisionMes.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '12' });
        }

        function inicializar_controlesB3() {
            $("#<%: ddlBusCircuito3.ClientID %>").select2({ allowClear: true });
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function bt_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(href_collapse).css("height") == "0px") {
                    $(obj).find(".imoon-chevron-down").switchClass("imoon-chevron-down", "imoon-chevron-up", 0);
                }
                else {
                    $(obj).find(".imoon-chevron-up").switchClass("imoon-chevron-up", "imoon-chevron-down", 0);
                }
            }
        }

        function inicializar_popover() {

            $('[rel=popover]').popover({
                html: 'true',
                placement: 'right'
            })

        }
        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function validarBuscar() {
            var ret = true;
            hideSummary();

            if (ret) {

            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }

        function validarBuscarB3() {
            var ret = true;
            hideSummary();
            debugger;

            if (ret) {

            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }
        function validarGuardar() {
            var ret = true;
            hideSummary();
            $("#Val_BotonGuardar").hide();

            if (ret) {
                ocultarBotonesGuardar();
            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }


        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }

        function ConfirmDel() {
            return confirm('¿Esta seguro que desea quitar esta condición?');
        }

        function inicializar_autocomplete() {

        }


    </script>

<div id="box_busqueda">
    <asp:Panel ID="pnlBotonDefault" runat="server" >
        <div class="accordion-group widget-box" >

            <%-- titulo collapsible buscar ubicacion --%>    
            <div class="accordion-heading">
                <a id="bt_ubicacion_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_ubicacion" 
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

    <%--                <asp:HiddenField ID="hid_bt_ubicacion_collapse" runat="server" Value="false"/>
                    <asp:HiddenField ID="hid_bt_ubicacion_visible" runat="server" Value="false"/>--%>

                    <div class="widget-title">
                        <span class="icon" style="margin-left:4px"><i class="imoon-library"></i></span>
                        <h5><asp:Label ID="Label1" runat="server" Text="Consulta Tramite y Circuito por Solicitud"></asp:Label></h5>
                        <span class="btn-right"><i class="imoon-chevron-up"></i></span>        
                    </div>
                </a>
            </div>

            <%-- controles collapsible buscar por ubicacion --%>    
            <div  class="accordion-body collapse in" id="collapse_bt_ubicacion" >
		     <div class="widget-content">
                <%-- Busqueda1 --%>
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnDescargarBloque1" >
                                <div class="row">
                                    <%--<div class="span5">
                                        <asp:Label ID="lblBusArea" runat="server" CssClass="control-label" Text="Area:" AssociatedControlID="ddlBusArea" />
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlBusArea" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>--%>
                                    <div class="span7">
                                        <asp:Label ID="lblBusTipoTramite" runat="server" CssClass="control-label" Text="Tipo de Tramite" AssociatedControlID="ddlBusTipoTramite" />
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlBusTipoTramite" runat="server" style="width:40%;min-width:100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusTipoTramite_SelectedIndexChanged" />
                                            <asp:DropDownList ID="ddlBusTipoExpediente" runat="server" style="width:25%;min-width:100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusTipoExpediente_SelectedIndexChanged" />
                                            <asp:DropDownList ID="ddlBusSubtipoExpediente" runat="server" style="width:30%;min-width:100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusSubtipoExpediente_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    </div>
                                <div class="row">
                                    <div class="span6">
                                        <asp:Label ID="Label2" IDç="lblBusCircuito" runat="server" CssClass="control-label" Text="Circuito:" AssociatedControlID="ddlBusCircuito" />
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlBusCircuito" runat="server" style="width:50%;min-width:100px" AutoPostBack="true" ></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
            <div class="pull-right">  
                
                                        <div class="control-group inline-block">

                <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="updpnlBuscar"
                    runat="server" DisplayAfter="0"  >
                    <ProgressTemplate>
                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                    </ProgressTemplate>
                </asp:UpdateProgress>  

            </div>
                                        <asp:LinkButton ID="btnDescargarBloque1" runat="server" CssClass="btn btn-success btn-large" OnClick="btnDescargarBloque1_Click" ValidationGroup="validarBuscar">
                                            <i class="imoon-file-excel"></i>
                                            <span class="text">Descargar</span>
                                        </asp:LinkButton>
                                      
                </div>
                      </div>
				            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                     </div>
                </div>
                </div>
            </div>
           
        <div class="accordion-group widget-box">
            <div class="accordion-heading">
                <a id="bt_bloque2_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_bloque2" 
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                    <div class="widget-title">
                        <span class="icon" style="margin-left:4px"><i class="imoon-library"></i></span>
                        <h5><asp:Label ID="Label3" runat="server" Text="Consulta de Revision de DGHyP por Solicitud"></asp:Label></h5>
                        <span class="btn-right"><i class="imoon-chevron-up"></i></span>        
                    </div>
                </a>
            </div>
            <div class="accordion-body collapse in" id="collapse_bt_bloque2">
                 <div class="widget-content">
                     <div class="form-horizontal">
                         <asp:UpdatePanel ID="updpnlBloque2" runat="server" UpdateMode="Conditional">
                             <ContentTemplate>
                                 <asp:Panel ID="pnlBloque2" runat="server" DefaultButton="btnDescargarBloque2">
                                      <div class="row">
                                    <div class="span7">
                                        <asp:Label ID="lblBusTipoTramiteRev" runat="server" CssClass="control-label" Text="Tipo de Tramite" AssociatedControlID="ddlBusTipoTramiteRev" />
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlBusTipoTramiteRev" runat="server" style="width:40%;min-width:100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusTipoTramite_SelectedIndexChanged" />
                                            <asp:DropDownList ID="ddlBusTipoExpedienteRev" runat="server" style="width:25%;min-width:100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusTipoExpediente_SelectedIndexChanged" />
                                            <asp:DropDownList ID="ddlBusSubtipoExpedienteRev" runat="server" style="width:30%;min-width:100px" AutoPostBack="true" OnSelectedIndexChanged="ddlBusSubtipoExpediente_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    </div>
                                <div class="row">
                                    <div class="span6">
                                        <asp:Label ID="lblBusCircuitoRev" runat="server" CssClass="control-label" Text="Circuito:" AssociatedControlID="ddlBusCircuitoRev" />
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlBusCircuitoRev" runat="server" style="width:50%;min-width:100px" AutoPostBack="true" ></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                     <div class="row">
                                         <div class="span4">
                                             <asp:Label ID="lblBusFechaRevision" CssClass="control-label" runat="server"  Text="Mes y Año de la ultima Revision DGHyP:" AssociatedControlID="txtBusFechaRevisionMes"></asp:Label>
                                             <div class="controls">
                                                 <asp:TextBox ID="txtBusFechaRevisionMes" runat="server" MaxLength="3" Width="30px" ></asp:TextBox>
                                                 <asp:TextBox ID="txtBusFechaRevisionAnio" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                             </div>
                                         </div>
                                         <div class="span4">
                                             <asp:Label ID="lblBusObservado" CssClass="control-label" runat="server" Text="Observado:"  AssociatedControlID="ddlBusObservado"></asp:Label>
                                             <div class="controls">
                                                 <asp:DropDownList ID="ddlBusObservado" runat="server" Width="80px">
                                                     <asp:ListItem Value="99" Text="Todos"></asp:ListItem>
                                                     <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                     <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                 </asp:DropDownList>
                                             </div>
                                         </div>
                                     </div>
                                <div class="row">
            <div class="pull-right">  
                
                                        <div class="control-group inline-block">

                <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="updpnlBloque2"
                    runat="server" DisplayAfter="0"  >
                    <ProgressTemplate>
                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                    </ProgressTemplate>
                </asp:UpdateProgress>  

            </div>
                                        <asp:LinkButton ID="btnDescargarBloque2" runat="server" CssClass="btn btn-success btn-large" OnClick="btnDescargarBloque2_Click" ValidationGroup="validarBuscar">
                                            <i class="imoon-file-excel"></i>
                                            <span class="text">Descargar</span>
                                        </asp:LinkButton>
                                      
                </div>
                      </div>
                                 </asp:Panel>
                             </ContentTemplate>
                         </asp:UpdatePanel>
                     </div>
                </div>
            </div>
        </div> 
           
        <div class="accordion-group widget-box" >
            <%-- titulo collapsible buscar ubicacion --%>    
            <div class="accordion-heading">
                <a id="bt_hojas_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_hojas" 
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                    <div class="widget-title">
                        <span class="icon" style="margin-left:4px"><i class="imoon-library"></i></span>
                        <h5><asp:Label ID="Label4" runat="server" Text="Consulta de Hoja 1 - 2 - 3 - 6 - 7 - 8"></asp:Label></h5>
                        <span class="btn-right"><i class="imoon-chevron-up"></i></span>        
                    </div>
                </a>
            </div>

            <%-- controles collapsible buscar por ubicacion --%>    
            <div  class="accordion-body collapse in" id="collapse_bt_hojas" >
                <div class="widget-content">
                    <%-- Busqueda1 --%>
                    <div class="form-horizontal">
                        <asp:UpdatePanel ID="updpnlBloque3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="Button1" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnDescargarBloque3" >
                                    <div class="row">
                                        <div class="span6">
                                            <asp:Label ID="Label6" runat="server" CssClass="control-label" Text="Circuito:" AssociatedControlID="ddlBusCircuito3" />
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlBusCircuito3" runat="server" style="width:600px" AutoPostBack="true" ></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="pull-right">  
                                            <div class="control-group inline-block">
                                                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="updpnlBloque3"
                                                    runat="server" DisplayAfter="0"  >
                                                    <ProgressTemplate>
                                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>  
                                            </div>
                                            <asp:LinkButton ID="btnDescargarBloque3" runat="server" CssClass="btn btn-success btn-large" OnClick="btnDescargarBloque3_Click" ValidationGroup="validarBuscarB3">
                                                <i class="imoon-file-excel"></i>
                                                <span class="text">Descargar</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
				                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel> 
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
                                <label class="imoon imoon-info color-blue fs64" ></label>
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
                            <asp:Panel ID="pnlExportacionError" runat="server" style="display:none">
                                <div class="row text-center">
                                    <i class="imoon imoon-notification color-blue fs64"></i>
                                       <h3> <asp:Label ID="lblExportarError" runat="server" Text="Error exportando el contenido, por favor vuelva a intentar." ></asp:Label></h3>
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


</asp:Content>

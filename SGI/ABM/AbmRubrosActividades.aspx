<%@ Page Title="Consulta de Rubros o Actividades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmRubrosActividades.aspx.cs" Inherits="SGI.ABM.AbmRubrosActividades" %>

 <%@ Register Src="~/Controls/VisualizarRubro.ascx" TagPrefix="uc" TagName="visualizarRubro" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <script src="../Scripts/Datepicker_es.js"></script>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <%: Styles.Render("~/Content/themes/base/css") %>
    <script type="text/javascript">

        $(document).ready(function () {
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#page_content").hide();
            $("#Loading").show();


            $("#<%: btnCargarDatos.ClientID %>").click();


        });


        function init_Js_updDatos() {


        }


        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
            $("#box_resultado").hide("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");

            $("#<%: btnBuscar.ClientID %>").click();
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
            $("#Buscar_CamposReq").hide();

            if ($.trim($("#<%: txtCodigoDescripcionoPalabraClave.ClientID %>").val()).length == 0) {
                $("#Req_txtCodigoDescripcionoPalabraClave").css("display", "inline-block");
                ret = false;
            }

            if (ret) {

            }
            else {
                $("#Buscar_CamposReq").css("display", "inline-block");

            }
            return ret;
        }

        function validarGuardar() {
            var ret = true;
            hideSummary();
            $("#Val_BotonGuardar").hide();
            $("#Req_txtCodigoDescripcionoPalabraClave").hide();

            if ($.trim($("#<%: txtCodigoDescripcionoPalabraClave.ClientID %>").val()).length == 0) {
                $("#Req_txtCodigoDescripcionoPalabraClave").css("display", "inline-block");
                ret = false;
            }

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


        function mostarPanel(pnlNameMostrar, pnlNameOcultar) {

            $("#" + pnlNameOcultar).hide();
            $("#" + pnlNameMostrar).slideDown(400);

            return true;
        }
    </script>

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                <td>
                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>

    <div id="page_content" style="display:none">

        <%--Busqueda de Profesionales--%>
        <div id="box_busqueda" class="widget-box">
		    <div class="widget-title">
			    <span class="icon"><i class="icon-search"></i></span>
			    <h5>B&uacute;squeda de Rubros</h5>
		    </div>
		    <div class="widget-content">
			    <div>
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">

                               
                                <div class="control-group">
                                    <label class="control-label">Código o Descripción del Rubro o Palabra Clave:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCodigoDescripcionoPalabraClave" runat="server" CssClass="input-xlarge" Width="250px"></asp:TextBox>
                                        <div id="Req_txtCodigoDescripcionoPalabraClave" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                 	El Código o Descripción es obligatorio.
                                        </div>
                                    </div>
                                </div>
                                <div id="Buscar_CamposReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                      Debe ingresar algún valor en los campos para poder realizar la búsqueda.
                                </div>

                                <div class="control-group">
                                    <label class="control-label"></label>
                                    <div class="controls">
                                        <asp:LinkButton ID="btnNuevaPersonaInhibida" runat="server" CssClass="btn" OnClick="btnNueva_Click">
                                            <i class="icon-plus"></i>
                                            <span class="text">Nuevo Rubro</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClick="btnBuscar_Click" ValidationGroup="buscar" OnClientClick="return validarBuscar();">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Buscar</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnVerSolicitudesActivas" runat="server" CssClass="btn btn-inverse" OnClick="btnVerSolicitudesActivas_Click">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Solic. de cambio activas</span>
                                        </asp:LinkButton>

                                       <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-success" OnClick="btnExportar_Click">
                                            <i class="icon-white imoon-file-excel"></i>
                                            <span class="text">Exportar rubros</span>
                                        </asp:LinkButton>

                                        <div class="control-group inline-block">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updpnlBuscar">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
				            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
				    
                    </div>
                </div>
            </div>
    <div id="box_resultado" class="widget-box" style="display:none;">

                    <%--Resultados --%>
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
            <div  style="margin-left:10px; margin-right:10px;">
                            <asp:HiddenField ID="hid_grdRubros_rowIndexselected" runat="server" Value="false" />
                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                                <div "text-left">
                                    <h5>Resultado de la b&uacute;squeda</h5>
                                </div>
                                <div class="text-right">
                                    <span class="badge">Cantidad de registros:
                                    <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                </div>
                            </asp:Panel>
                            <br />
                            <div style="float:right">
                                <asp:Panel ID="pnlReferencias" runat="server" style="float:right; margin-bottom:5px;width:250px;" Visible="false">
                                            <div style="width:50%;float:left;" >
                                                <div style="width: 20px; height: 30px; background-color: #fcd8b8; border: solid 1px #ea8f3e;float:left; margin-right:10px"></div>
                                                <div style="float:left" class="text-left">En Proceso. </div>
                                            </div>


                                            <div style="width:50%;float:right" >
                                                <div style="width: 20px; height: 30px; background-color: #cff3d7; border: solid 1px #68be7b;float:left; margin-right:10px"></div>
                                                <div style="float:left" class="text-left">Confirmada.</div>
                                            </div>
                                </asp:Panel>
                            </div>
                            <br /><br />
                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                PageSize="50" AllowSorting="true" OnPageIndexChanging="grd_PageIndexChanging"
                                OnDataBound="grd_DataBound" OnRowDataBound="grd_RowDataBound" >
                                <Columns>
                                    <%--Ver --%>
                                    <asp:BoundField DataField="cod_rubro" HeaderText="Codigo" ItemStyle-CssClass="text-center" ItemStyle-Width="50px" />
                                    
                                    <asp:BoundField DataField="nomb_rubro" HeaderText="Descripción" />
                                    <asp:BoundField DataField="tipo_actividad" HeaderText="Tipo de Actividad" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />
                                    <%--<asp:BoundField DataField="id_licenciaAlcohol" HeaderText="Licencia Alcohol" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />--%>
                                  
                                    <asp:CheckBoxField DataField="es_anterior_rubro" HeaderText="Histórico" ItemStyle-CssClass="text-center" ItemStyle-Width="30px" />
                                 
                                    <asp:TemplateField HeaderText="Acción" ItemStyle-CssClass="text-center" ItemStyle-Width="75px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnVisualizarRubro" runat="server" Visible="false" ToolTip="Visualizar" data-toggle="tooltip" 
                                                CommandArgument='<%# Eval("cod_rubro") %>' OnClick="btnVisualizarRubro_Click" >
                                                <i class="imoon-eye-open" style="font-size:medium;margin-right:5px;margin-left:5px"></i>
                                            </asp:LinkButton>
                                            
                                            <asp:LinkButton ID="btnEditar" runat="server" Visible="false" ToolTip="Editar" data-toggle="tooltip" 
                                                CommandArgument='<%# Eval("id_rubro") %>' OnClick="btnEditar_Click" >
                                                <i class="imoon-pencil2" style="font-size:medium;margin-right:5px;margin-left:5px"></i>
                                            </asp:LinkButton> 
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div>
                                        <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                        <span class="mleft20">No se encontraron registros.</span>
                                    </div>
                                </EmptyDataTemplate>

                                <PagerTemplate>
                                    <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                        <div style="display:inline-table">
                                            <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
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

                            <asp:GridView ID="grdSolicitudesActivas" runat="server" AutoGenerateColumns="false" 
                                DataKeyNames="id_rubhistcam,tipo_solicitud_rubhistcam"
                                AllowPaging="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                PageSize="30" AllowSorting="true" Visible="false"
                                 OnPageIndexChanging="grdSolicitudesActivas_PageIndexChanging"
                                OnDataBound="grdSolicitudesActivas_DataBound"  >

                             
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="Solicitud Nº" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnVisualizarSolicitudCambio" runat="server" 
                                                Text='<%# Eval("id_rubhistcam") %>' OnClick="btnVisualizarSolicitudCambio_Click" ></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="tipo_solicitud" HeaderText="Tipo de solicitud" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="cod_rubro" HeaderText="Cód Rubro" ItemStyle-Width="70px" />
                                    <asp:BoundField DataField="nom_rubro" HeaderText="Descripción" />
                                    <asp:BoundField DataField="estado_modif" HeaderText="Estado" ItemStyle-Width="150px" />
                                    
                                </Columns>
                                <EmptyDataTemplate>
                                    No se han encontrado solicitudes de cambio activas.
                                </EmptyDataTemplate>
                                                                <PagerTemplate>
                                    <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                        <div style="display:inline-table">
                                            <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
                                                DisplayAfter="0">
                                                <ProgressTemplate>
                                                    <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>

                                        <asp:LinkButton ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior2_Click" CssClass="btn" />
                                        <asp:LinkButton ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage2" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage2" CssClass="btn"  />
                                        <asp:LinkButton ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage2" CssClass="btn"  />
                                        <asp:LinkButton ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage2" CssClass="btn" />
                                        <asp:LinkButton ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage2" CssClass="btn" />
                                        <asp:LinkButton ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage2" CssClass="btn"  />
                                        <asp:LinkButton ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage2" CssClass="btn"  />
                                        <asp:LinkButton ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage2" CssClass="btn" />
                                        <asp:LinkButton ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage2" CssClass="btn" />
                                        <asp:LinkButton ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage2" CssClass="btn"  />
                                        <asp:LinkButton ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage2" CssClass="btn" />
                                        <asp:LinkButton ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente2_Click" CssClass="btn" />

                                    </asp:Panel>
                            </PagerTemplate>
                            </asp:GridView>
                <br />
                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
        
 
        <%--ABM --%>
        <div id="box_datos"  style="display:none">
            <%--<div class="widget-title ">
			    <span class="icon"><i class="imoon imoon-hammer"></i></span>
			    <h5>Alta de Rubros</h5>
            </div>
            <div class="widget-content">--%>
                        <asp:HiddenField ID="hid_id_rubroReq" runat="server" />
                        <asp:Panel ID="PnlVisualizarRubro" runat="server" >
                                <asp:UpdatePanel ID="updVisualizarRubro" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <uc:visualizarRubro ID="VisualizarRubro" runat="server"></uc:visualizarRubro>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
    <!-- /.modal -->
   </div>

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
                            <asp:Panel ID="pnlDescargarExcel" runat="server" Style="display: normal">
                                <div class="row text-center">
                                    <asp:HyperLink ID="btnDescargarExcel" runat="server" Target="_blank" CssClass="btn btn-link">
                                        <i class="imoon imoon-file-excel color-green fs48"></i>
                                        <br />
                                        <span class="text">Descargar archivo</span>
                                    </asp:HyperLink>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlExportacionError" runat="server" Style="display: none">
                                <div class="row text-center">
                                    <i class="imoon imoon-notification color-blue fs64"></i>
                                    <h3>
                                        <asp:Label ID="lblExportarError" runat="server" Text="Error exportando el contenido, por favor vuelva a intentar."></asp:Label></h3>
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

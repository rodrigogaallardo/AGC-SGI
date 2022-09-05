<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMUsuarioAnexoTecnico.aspx.cs" Inherits="SGI.ABM.ABMUsuarioAnexoTecnico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
       

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        $(document).ready(function () {
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#page_content").hide();
            $("#Loading").show();

            init_Js_updpnlBuscar();

            $("#<%: btnCargarDatos.ClientID %>").click();
            init_Js_updPnlUsuSSIT();
        });
        function PuedeTransferir(obj) {

            var dad = $(obj).parent();

            var ret = true;

            if (ret) {
                dad.find('[id*=btnTransferir]').hide();
                dad.find('[id*=imgTransferir]').css("display", "inline-block");
            }

            return ret;

        }
        function init_Js_updPnlUsuSSIT() {
            debugger;
            'use strict';
            $("#progress").hide();
            var nrorandom = Math.floor(Math.random() * 1000000);

            var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();

        }

        function tda_progress(value) {

            $("#progress .bar").css(
                'width',
                value + '%'
            );
        }

        function tda_validar_input(arch_nombre, arch_size, nroArch) {

            var es_valido = false;
            var val__errores_visibles;
            if (nroArch == 1) {

                tda_validar_fileupload_tipo_archivo(arch_nombre, 1);
                tda_validar_fileupload_size_archivo(arch_size, 1);
                val__errores_visibles = $("[id*='val_upload']:visible").length;
            }

            if (val__errores_visibles > 0) {
                es_valido = false;
            }
            else {
                es_valido = true;
            }

            if (es_valido) {
                //verificar si existe antes de guardar
                var existeDocumento = tda_validar_existe_documento();

                if (!existeDocumento) {
                    es_valido = true;
                }
                else {
                    var confirmar = confirm('El tipo de documento existe. ¿Desea reemplazarlo?');

                    if (confirmar) {
                        es_valido = true;
                    }
                    else {
                        es_valido = false;
                    }
                }

            }

            return es_valido;
        }

        function tda_validar_existe_documento() {
            return false;
        }

        function tda_validar_fileupload_tipo_archivo(arch_nombre, archT) { }

        function tda_validar_fileupload_size_archivo(arch_size, archT) {

            if (archT == 1) {

                if (arch_size < 1) { }

                if (arch_size > 4000000) { }
            }
        }

        function init_Js_updpnlBuscar() { }

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
        function hideResultado() {
            $("#box_resultado").hide("slow");
        }

        function showUsuarios() {
            $("#box_usuarios").show("slow");
        }
        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }

        function showBusqueda() {
            $("#box_usuarios").hide("slow");
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
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

        $(document).ready(function () {

            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#page_content").hide();
            $("#Loading").show();

            init_Js_updpnlBuscar();

            //init_Js_updPnlUsuSSIT();
        });

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function init_Js_updpnlBuscar() {

        }

    </script>

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>

    <div id="page_content" style="display:block;">

        <%--Busqueda de Profesionales--%>
        <div id="box_busqueda" >
        <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar" >
		    <div class="widget-title">
			    <span class="icon"><i class="icon-search"></i></span>
			    <h5>B&uacute;squeda de Usuario de un Anexo Técnico</h5>
		    </div>
		    <div class="widget-content">
			    <div>
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">
                                
                                <div class="row">
                                    <div class="span6">
                                        <asp:Label ID="lblAnexoTecnico" runat="server" Text="Nro. Anexo Técnico:" CssClass="control-label" AssociatedControlID="txtAnexoTecnico"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAnexoTecnico" runat="server" Width="95%" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="span6">
                                        <asp:Label ID="lblSolicitud" runat="server" Text="Nro. Solicitud:" CssClass="control-label" AssociatedControlID="txtSolicitud"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSolicitud" runat="server" Width="95%" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>                                
                                
                                <div class="row">
                                    <div id="Buscar_CamposReq" class="alert alert-danger mbottom0 mtop5 span5" style="display: none;">
                                          Debe seleccionar algunos de los filtros antes de proceder con la búsqueda.
                                    </div>
                                </div>
                                <div class="row">
                                    <div id="Buscar_filtroAnexo"  class="alert alert-info mbottom0 mtop5 span5" style="display: none;">
                                        Usted esta utilizando el filtro de "Nro de Anexo Tecnico" por lo cual no podra consultar un tipo de tramite distinto de "Habilitacion" o "Rectificatoria".
                                    </div>
                                </div>

                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
				    
                    </div>
                </div>
            </asp:Panel>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <div class="pull-right">
                        
                                <div id="Div1" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                      Debe ingresar algún valor en los campos para poder realizar la búsqueda.
                                </div>
                                        <div class="control-group inline-block">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                        
                                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" ValidationGroup="buscar" OnClientClick="return validarBuscar();">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Buscar</span>
                                        </asp:LinkButton>
                        
                                        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-default" OnClick="btnLimpiar_Click" >
                                            <i class="imoon-blocked"></i>
                                            <span class="text">Limpiar</span>
                                        </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            
            <br /><br />
            
        <%-- Muestra Resultados--%>
        <div id="box_resultado" style="display:none;"> 
                    <%--Resultados --%>
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
            
                            
                            <div class="widget-box"> 
                                <div class="form-horizontal" style="margin-left:15px;margin-right:15px">
                                    <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                                        <div "text-left">
                                    
                                            <div class="control-group pleft20 ">
                                                <h5>Solicitante</h5>
                                                <hr class="line-subtitle" />
                                            </div>
                                        </div>
                                        <div class="text-right">
                                            <span class="badge">Cantidad de registros:
                                            <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                        </div>
                                    </asp:Panel>
                                    <br />
                                    <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                        GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                        PageSize="30" AllowSorting="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    OnDataBound="grd_DataBound" OnRowDataBound="grdResultados_RowDataBound" style="max-width:90%;margin-left:20px" >
                                        <Columns>
                                            <%--Ver --%>
                                            <asp:BoundField DataField="nro_anexo_tecnico" HeaderText="Nro. Anexo Tecnico" ItemStyle-CssClass="text-center" ItemStyle-Width="50px" />
                                            <asp:BoundField DataField="nro_solicitud" HeaderText="Nro. Solicitud" ItemStyle-CssClass="text-center" ItemStyle-Width="50px" />
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre del usuario" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="user_name" HeaderText="Usuario" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="consejo_profesional" HeaderText="Consejo Profesional" ItemStyle-CssClass="text-center" ItemStyle-Width="80px" />
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
                                <br />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
        </div>    

        
        <div id="box_datos"  style="display:none">
            <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos de la Solicitud</h5>
            </div>
            <div class="widget-content">
                <div>
                    <div class="col-sm-12 col-md-12">
                        <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hid_id_solicitud" runat="server" />
                                <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                                    <div class="row">
                                        <div class="span5">
                                            <asp:Label ID="lblResNroSolicitud" runat="server" CssClass="control-label" AssociatedControlID="txtResNroSolicitud" Text="Nro. de Solicitud:"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtResNroSolicitud" runat="server" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        </div>
                                    <asp:Panel ID="pnlResNroAnexo" runat="server" CssClass="row" Visible="false">
                                        <div class="span5">
                                            <label class="control-label">Nro. de Anexo Tecnico:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtResNroAnexo" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    <div class="row">
                                        <div class="span5">
                                            <asp:Label ID="lblResUsername" runat="server" CssClass="control-label" AssociatedControlID="txtResUsername" Text="Nombre de Usuario:"></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtResUsername" runat="server" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        
                                        <div class="span5">
                                            <asp:Label ID="lblResApeNom" runat="server" CssClass="control-label" AssociatedControlID="txtResApeNom" Text="Apellido y Nombre:" ></asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtResApeNom" runat="server" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="span5">
                                            <label class="control-label">Tipo de Tramite:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtResTipoTramite" runat="server" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            </div>
            <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-accessibility" style="font-size:medium"></i></span>
                <h5>Datos del nuevo usuario</h5>
            </div>
                <div class="widget-content">
                    
                
                </div>
                </div>
        </div>
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
</asp:Content>

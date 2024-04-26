<%@ Page Title="Abm Tipo de Documentos Requeridos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmTipoDocRequerido.aspx.cs" Inherits="SGI.ABM.AbmTipoDocRequerido" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/gritter") %>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            inicializar_controles();
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarTipoDocReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#page_content").hide();
            $("#Loading").show();
            init_Js_updDatos();

            $("#<%: btnCargarDatos.ClientID %>").click();

        });

        
        function init_Js_updDatos() {

            $("#<%: txtTamano.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999' });

            return false;
        }

        function inicializar_controles() {
            $("#<%: txtTamano.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999' });

            inicializar_popover();
            $("#<%: ddlNombreDoc.ClientID %>").select2({
                allowClear: true,
                placeholder: "",
                minimumInputLength: 2,
                formatInputTooShort: function () {
                    return "Por favor introduzca 2 caracteres o mas";
                }
            });
        }
        function inicializar_popover() {

            $("[id*='lnkTareasSolicitud']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

            $("[id*='MainContent_grdResultados_lnkTareasSolicitud_']").each(function () {
                //para cada fila de la grilla, se busca el link y se lo vincula al panel de la misma fila
                //para que con el clikc del link habra el popOver de un html
                var id_pnlTareas = $(this).attr("id").replace("MainContent_grdResultados_lnkTareasSolicitud_", "MainContent_grdResultados_pnlTareas_");
                var objTareas = $("#" + id_pnlTareas).html();
                $(this).popover({
                    title: 'Tareas',
                    content: objTareas,
                    html: 'true'
                });
            });
        }
        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
        }
        function showResultados() {
            $("#box_resultado").show("slow");
        }
        function hideResultados() {
            $("#box_resultado").hide("slow");
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

            function validarGuardar() {
                var ret = true;
                hideSummary();
                $("#Val_BotonGuardar").hide();
                $("#Req_NombreDocReq").hide();
                $("#Req_Formato").hide();
                $("#Req_Tamano").hide();
                $("#Req_Acronimo").hide();
                

                if ($.trim($("#<%: txtNombreDocReq.ClientID %>").val()).length == 0) {
                    $("#Req_NombreDocReq").css("display", "inline-block");
                    ret = false;
                }
                if ($.trim($("#<%: txtFormato.ClientID %>").val()).length == 0) {
                    $("#Req_Formato").css("display", "inline-block");
                    ret = false;
                }
                if ($.trim($("#<%: txtTamano.ClientID %>").val()).length == 0) {
                    $("#Req_Tamano").css("display", "inline-block");
                    ret = false;
                }

                if ( parseInt($("#<%: txtTamano.ClientID %>").val()) > parseInt($("#<%: hdnTamanioMaximo.ClientID %>").val()) ) {
                    $("#Valida_Tamano").css("display", "inline-block");
                    ret = false;
                }

                if ($.trim($("#<%: txtAcronimoSade.ClientID %>").val()).length == 0) {
                    $("#Req_Acronimo").css("display", "inline-block");
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
                return confirm('¿Esta seguro que desea quitar este Documento?');
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

        <asp:HiddenField ID="hid_valor_boton" runat="server" />
        <asp:HiddenField ID="hid_observaciones" runat="server" />
        <%--Busqueda de Tipo Doc Requeridos--%>
        <div id="box_busqueda" >
            <asp:HiddenField ID="hdnTamanioMaximo" runat="server" />
        <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar" >
            
		    <div class="widget-title">
			    <span class="icon"><i class="icon-search"></i></span>
			    <h5>B&uacute;squeda de Tipo de Documentos Requeridos</h5>
		    </div>
		    <div class="widget-content">
			    <div >
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" CssClass="form-horizontal" DefaultButton="btnBuscar" >

                                <%--<div class="control-group">
                                    <asp:label ID="lblNombreDoc" runat="server"
                                         Text="Nombre de Tipo de Documento Requerido:" class="control-label" ></asp:label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNombreDoc" runat="server" CssClass="input-xlarge" Width="220px"></asp:TextBox>
                                    </div>
                                </div>--%>
                                
                                  <div class="row">  
                                <div class="span5">
                                    <asp:Label ID="lblNombreDoc" runat="server" AssociatedControlID="ddlNombreDoc"
                                         Text="Nombre de Tipo de Documento Requerido:" class="control-label"></asp:Label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlNombreDoc" runat="server" Height="30px" ></asp:DropDownList>
                                    </div>
                                </div>
                                    <div class="span5">
                                         <asp:Label ID="lblRequiereDetalle" runat="server" AssociatedControlID="ddlRequiereDetalle" 
                                                Text="Requiere Detalle:" class="control-label"></asp:Label>
                                            <div class="controls">
                                            <asp:DropDownList ID="ddlRequiereDetalle" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                      
                                      </div>
                            <div class="row">
                                   <div class="span5">                                       
                                       <asp:Label ID="lblVisibleSSIT" runat="server" AssociatedControlID="ddlVisibleSSIT"
                                           Text="Visible en SSIT:" class="control-label"></asp:Label>
                                       <div class="controls">
                                           <asp:DropDownList ID="ddlVisibleSSIT" runat="server"  AutoPostBack="true">
                                           </asp:DropDownList>
                                       </div>                                        
                                    </div>
                                
                                    <div class="span5">
                                        <asp:Label ID="lblVisibleSGI" runat="server" AssociatedControlID="ddlVisibleSGI" 
                                                    Text="Visible en SGI:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlVisibleSGI" runat="server"  AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                <div class="span5">
                                        <asp:Label ID="lblVisibleAT" runat="server" AssociatedControlID="ddlVisibleAT" 
                                                    Text="Visible en Anexo Técnico:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlVisibleAT" runat="server"  AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                            </div>
                            <div class="row">
                                
                                    <div class="span5">
                                        <asp:Label ID="lblVisibleObservaciones" runat="server" AssociatedControlID="ddlVisibleObservaciones"
                                            Text="Visible para Observaciones:" class="control-label" Style="padding-top: 0"></asp:Label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlVisibleObservaciones" runat="server"  AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="span5">
                                        <asp:Label ID="lblVerificarFirma" runat="server" AssociatedControlID="ddlVerificarFirma" 
                                            Text="Verificar Firma Digital:" class="control-label"></asp:Label>
                                         <div class="controls">
                                            <asp:DropDownList ID="ddlVerificarFirma" runat="server"  AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                            </div>
                                <div class="row">
                                    <div class="span5">
                                        <asp:Label ID="lblBagaLogica2" runat="server" AssociatedControlID="ddlBajaLogica2" 
                                            Text="Baja Logica:" class="control-label"></asp:Label>
                                         <div class="controls">
                                            <asp:DropDownList ID="ddlBajaLogica2" runat="server"  AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="span5">
                                       <asp:Label ID="lblObservacion" runat="server" AssociatedControlID="txtObservacion" Text="Observaciones:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtObservacion" runat="server" ></asp:TextBox>
                                        </div>
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

                                        <div class="control-group inline-block">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                            </div>
                        
                                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Buscar</span>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick" >
                                            <i class="icon-refresh"></i>
                                                <span class="text">Reestablecer filtros por defecto</span>
                                        </asp:LinkButton>
                        
                                        <asp:LinkButton ID="btnNuevoTipo" runat="server" CssClass="btn btn-success" OnClick="btnNuevoTipo_Click">
                                            <i class="icon-white icon-plus"></i>
                                            <span class="text">Nuevo Tipo de Documento Requerido</span>
                                        </asp:LinkButton>
                                    </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
				    
            <br /><br />
                    <%--Resultados --%>
                
    <div id="box_resultado" style="display:none">
            <div class="widget-box">
         <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                        <div class="form-horizontal" style="margin-left:15px;margin-right:15px">

                <script type="text/javascript">
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                    function endRequestHandler() {
                        inicializar_popover();
                    }
                </script> 

            <asp:Panel ID="pnlResultadoBuscar" runat="server">

                <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">

                    <div style="display: inline-block;margin-left:10px;">
                        <h5>Lista de Documentos Requeridos</h5>
                    </div>
                    <div style="display: inline-block">
                        (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server" ></asp:Label></span>
                        )
                    </div>

                </asp:Panel>

                <asp:GridView ID="grdResultados" 
                    runat="server" 
                    AutoGenerateColumns="false"
                    GridLines="None" 
                    CssClass="table table-bordered table-striped table-hover with-check"
                    SelectMethod="GetResultados" 
                    AllowPaging="true" 
                    AllowSorting="true" 
                    PageSize="30" 
                    OnPageIndexChanging="grdResultados_PageIndexChanging"
                    OnDataBound="grdResultados_DataBound" 
                    OnRowDataBound="grdResultados_RowDataBound">
                   <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                   <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                   <Columns>

                                    <%--Editar --%>
                                    <asp:BoundField DataField="nombre_tdocreq" HeaderText="Nombre" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="observaciones_tdocreq" HeaderText="Observación" ItemStyle-Width="350px" />
                                    <asp:BoundField DataField="RequiereDetalle" HeaderText="Requiere Detalle" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="visible_en_SSIT" HeaderText="Visible en SSIT" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="visible_en_SGI" HeaderText="Visible en SGI" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="visible_en_AT" HeaderText="Visible en AT" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="visible_en_Obs" HeaderText="Visible para Observaciones" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="verificar_firma_digital" HeaderText="Verificar Firma Digital" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="baja_tdocreq" HeaderText="Baja Lógica" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center"/>

                                    <%--Eliminar --%>
                                    <asp:TemplateField HeaderText="Acción" HeaderStyle-ForeColor="#337AB7" ItemStyle-CssClass="text-center" ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local" 
                                                CommandArgument='<%# Eval("id_tdocreq") %>' OnClick="btnEditar_Click" >
                                                <i class="imoon-edit" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnkEliminarTipoDocReq" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local" 
                                                CommandArgument='<%# Eval("id_tdocreq") %>' OnClientClick="javascript:return ConfirmDel();"
                                                OnCommand="lnkEliminarTipoDocReq_Command">
                                                <i class="imoon-remove" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i>
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
                <br />
            </asp:Panel>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
                </div>
  </div>
        <%--ABM --%>
        <div id="box_datos" class="widget-box" style="display:none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos del Tipo de Documento Requerido </h5>
            </div>
            <div class="widget-content">
                <div>
                    <div class="col-sm-12 col-md-12">
                        <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hid_id_tipoDocReq" runat="server" />
                                <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label">Nombre:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNombreDocReq" runat="server" Width="550px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_NombreDocReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Nombre.
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Requiere Detalle:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="cBoxRequiereDetalle" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Visible en el Sistema Público de Inicio de Trámite (SSIT):</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="cBoxVisible_en_ssit" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Visible en el Sistema Interno de Trámite (SGI):</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="cBoxVisible_en_sgi" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Visible en el Sistema Anexo Técnico (AT):</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="cBoxVisible_en_at" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Visible para Observaciones:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="cBoxVisible_en_Obs" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Verificar firma digital:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="cBoxVerificar_firma" runat="server" Checked="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Tamaño Maximo (mb):</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTamano" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                                            <div id="Req_Tamano" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Tamaño maximo del archivo.
                                            </div>
                                            <div id="Valida_Tamano" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                <asp:Literal runat="server" ID="lblTamanomax"></asp:Literal>
                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Formato:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFormato" runat="server" Width="100px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_Formato" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Formato.
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Acronimo SADE:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAcronimoSade" runat="server" Width="100px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_Acronimo" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Nombre.
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Observación:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtEditObserv" runat="server" 
                                                TextMode="MultiLine" MaxLength="1000" Height="100px" Width="550px"
                                               >
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Observación del Solicitante:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtObservacionesSolicitante" runat="server" 
                                                TextMode="MultiLine" MaxLength="1000" Height="100px" Width="550px"
                                               >
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Panel ID="pnlBajaLogica" runat="server" Visible="false"> 
                                        <div id="divBaja" class="control-group">
                                            <label class="col-sm-3 control-label">Baja Lógica:</label>
                                            <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddlBajaLogica" runat="server" Width="80px">
                                                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                        <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:UpdatePanel ID="updBotonesGuardar" runat="server" >
                                        <ContentTemplate>
                                            <div class="form-horizontal">
                                                <div class="control-group">
                                                    <div id="ValSummary" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Revise las validaciones en pantalla.
                                                    </div>
                                                </div>
                                                <div id="pnlBotonesGuardar" class="control-groupp">
                                                    <div class="controls">
                                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-inverse" OnClick="btnGuardar_Click" OnClientClick="return validarGuardar();">
                                                            <i class="imoon-white imoon-save"></i>
                                                            <span class="text">Guardar</span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();">
                                                            <i class="imoon-blocked"></i>
                                                            <span class="text">Cancelar</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
                                                        <ProgressTemplate>
                                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" /> Guardando...
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
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
    <%--Modal Eliminar Log--%>
    <div id="frmEliminarLog" class="modal fade" style="max-width: 400px;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Eliminar</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="control-label">Observaciones del Solicitante:</label>
                        <div class="controls">
                            <asp:TextBox ID="txtObservacionesSolicitanteEliminar" runat="server" CssClass="form-control" Columns="10" Width="95%" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Botones --%>
                <div class="modal-footer" style="text-align: left;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptar_Click" />
                    <asp:Button ID="Button1" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>


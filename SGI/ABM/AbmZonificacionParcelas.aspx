<%@ Page Title="Abm Zonificación de Parcelas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmZonificacionParcelas.aspx.cs" Inherits="SGI.ABM.AbmZonificacionParcelas" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
 <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>
    <script src="../Scripts/Select2-locales/select2_locale_es.js"></script>
    <script src="../Scripts/Funciones.js" type="text/javascript"></script>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <%: Styles.Render("~/Content/themes/base/css") %>
    <script type="text/javascript">

        $(document).ready(function () {
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#page_content").hide();
            $("#Loading").show();
            init_Js_updpnlBuscar();
            init_Js_updpnlCrearActu();

            $("#<%: btnCargarDatos.ClientID %>").click();
        });
        function init_Js_updpnlBuscar() {

            $("#<%: txtPuertaDesde.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $("#<%: txtPuertaHasta.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });

            return false;
        }
        function init_Js_updpnlCrearActu() {

            return false;
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
            $("#box_resultado").hide("slow");
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_busqueda").show("slow");
        }
        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function showfrmMensaje() {
            $("#frmMensaje").modal("show");
            return false;
        }

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

              function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }

        function ConfirmDel() {
            return confirm('¿Esta seguro que desea quitar esta Zonificación de Parcela?');
        }

        function limpiarCombo() {

            var control = $("#MainContent_txtBusquedaCalle")[0];
            if (control != undefined) {
                if (control.options != undefined) {
                    if (control.options.length > 0) {
                        control.options.length = 0;
                    }
                }
            }

            return false;

        }


        function ValidarControlesActualizar() {

            var ret;
            if (ValidarControlesBusqueda() && $("#MainContent_ddlZona").attr("value").length > 0) {
                ret = true;
            }
            else {
                $("#Req_ddlZonaReq").css("display", "inline-block");
                ret = false;
            }

            return ret;

        }

        function ValidarControlesActualizar2() {

            var ret;
            if (ValidarControlesBusqueda() && $("#MainContent_ddlZona2").attr("value").length > 0) {
                ret = true;
            }
            else {
                $("#Req_ddlZona2Req").css("display", "inline-block");
                ret = false;
            }

            return ret;

        }
        function ValidarControlesActualizar3() {

            var ret;
            if (ValidarControlesBusqueda() && $("#MainContent_ddlZona3").attr("value").length > 0) {
                ret = true;
            }
            else {
                $("#Req_ddlZona3Req").css("display", "inline-block");
                ret = false;
            }

            return ret;

        }

        function ValidarControlesBusqueda() {

            var ret = true;
            hideSummary();
            $("#Val_BotonGuardar").hide();
            $("#ValFields").hide();
            $("#txtCallesReq").hide();
            $("#desdeHastaReq").hide();

            var cmbcalle = $("#MainContent_txtCalleList")[0];


            if ($("#MainContent_txtSeccion").attr("value").length == 0 &&
                $("#MainContent_txtManzana").attr("value").length == 0 &&
                ($("#MainContent_txtCalleList").attr("value") == 0 || $("#MainContent_txtCalleList").attr("value").length == 0)
            ) {
                $("#ValFields").css("display", "inline-block");
                ret = false;
            }

            // Validar ingreso de calle
            if (($("#MainContent_txtPuertaDesde").attr("value").length > 0 || $("#MainContent_txtPuertaHasta").attr("value").length > 0)
                && ($("#MainContent_txtCalleList").attr("value") == 0 || $("#MainContent_txtCalleList").attr("value").length == 0)) {
                $("#txtCallesReq").css("display", "inline-block");
                ret = false;
            }

            if ($("#MainContent_txtPuertaDesde").attr("value").length > 0) {
                var desde = parseInt($("#MainContent_txtPuertaDesde").attr("value"));
                var hasta = 0;
                if ($("#MainContent_txtPuertaHasta").attr("value").length > 0) {
                    hasta = parseInt($("#MainContent_txtPuertaHasta").attr("value"));

                    if (desde > hasta) {
                        $("#desdeHastaReq").css("display", "inline-block");
                        ret = false;
                    }
                }

            }

            if (ret) {
                ocultarBotonesGuardar();
            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
            

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

        <%--Busqueda de Zonificación de Parcelas--%>
        <div id="box_busqueda" class="widget-box">
		    <div class="widget-title">
			    <span class="icon"><i class="icon-search"></i></span>
			    <h5>B&uacute;squeda de Zonificación de Parcelas</h5>
		    </div>
		    <div class="widget-content">
			    <div>
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hid_formulario_cargado" runat="server" />

                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">

                                <div class="control-group" style="width:50%;float:left;">
                                    <label class="control-label">Sección:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSeccion" runat="server" CssClass="input-xlarge" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group" style="width:50%;float:left;">
                                    <label class="control-label">Manzana:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtManzana" runat="server" CssClass="input-xlarge" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Búsqueda de Calle:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtBusquedaCalle" runat="server" CssClass="input-xlarge" Width="150px" onkeypress="limpiarCombo();"></asp:TextBox>
                                        <asp:ImageButton id="cmdBusquedaCalle" onclick="cmdBusquedaCalle_Click" style="padding-left:5px; padding-right:5px"
                            runat="server" ImageUrl="~/Content/img/app/search.png" ></asp:ImageButton> 
                                    </div>
                                </div>
                                <div class="control-group">
                                   <asp:Label runat="server" CssClass="control-label">Calle:</asp:Label>
                                    <div class="controls">
                                        <div class="form-inline">
                                            <asp:DropDownList ID="txtCalleList" runat="server" ValidationGroup="guardar"
                                                 CssClass="checkboxlist"  RepeatColumns="2">
                                             </asp:DropDownList>
                                             <div id="txtCallesReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Si especifica un rango de alturas debe ingresar/seleccionar la calle.
                                          </div>
                                       </div>
                                    </div>
                                </div>
                                 <div class="control-group" style="width:50%;float:left;">
                                    <label class="control-label">Desde:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtPuertaDesde" runat="server" CssClass="input-xlarge" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group" style="width:50%;float:left;">
                                     <label class="control-label">Hasta:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtPuertaHasta" runat="server" CssClass="input-xlarge" Width="150px"></asp:TextBox>
                                        <div id="desdeHastaReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                En el rango de alturas: La altura hasta no puede ser mayor a la altura desde
                                          </div>
                                    </div>
                                </div>
                                <div class="control-group form-inline">
                                    <div class="control-group inline-block">
                                    <asp:Label ID="lblTipoPartida" runat="server" AssociatedControlID="radioNumeracionAmbas"
                                        CssClass="control-label">Tipo de Partida:</asp:Label>
                                        <div class="controls form-inline">
                                            <asp:RadioButton ID="radioNumeracionAmbas" runat="server"
                                                Text="Ambas" GroupName="numeracion" Checked="true" />
                                            <asp:RadioButton ID="radioNumeracionPar" runat="server" Style="padding-left: 5px"
                                                Text="Par" GroupName="numeracion" />
                                            <asp:RadioButton ID="radioNumeracionImpar" runat="server" Style="padding-left: 5px"
                                                Text="Impar" GroupName="numeracion" />
                                        </div>
                                    </div>
                                </div>
            
                                <div class="control-group">
                                    <label class="control-label"></label>
                                    <div class="controls">
                                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" OnClientClick="return ValidarControlesBusqueda();">
                                             <i class="icon-white icon-search"></i>
                                             <span class="text">Buscar</span>
                                         </asp:LinkButton>
                                        <div class="control-group inline-block">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updpnlBuscar">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                        
                                        <div class="control-group">
                                            <div id="ValFields" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Para poder realizar la búsqueda es necesario ingresar al menos uno de los siguientes filtros de búsqueda: Sección, manzana o calle (entre rangos).
                                            </div>
                                            <div id="ValSummary" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Revise las validaciones en pantalla.
                                            </div>
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
            <div class="controls form-inline" style="margin-left:20px; margin-right:20px">
                <br />
                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">

                                <div class="text-left control-group inline-block">
                                    <h5>Resultado de la b&uacute;squeda</h5>
                                </div>
                                <div class="text-right control-group inline-block pull-right">
                                    <span class="badge">Cantidad de registros:
                                    <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                </div>

                                
                            </asp:Panel>
                            
                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                PageSize="50"  AllowSorting="true"  ItemType="SGI.Model.ItemZonificacionParcelas" SelectMethod="GetZonificacionParcelas"
                    OnDataBound="grdResultados_DataBound">

                                <Columns>
                                    
                                    <%--Editar --%>
                                    <asp:BoundField DataField="partidaMatriz" HeaderText="Partida Matriz" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="seccion" HeaderText="Sección" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="Zona1" HeaderText="Zona" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Zona2" HeaderText="Zona 2" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Zona3" HeaderText="Zona 3" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="direccion" HeaderText="Dirección" ItemStyle-Width="300px" />
                                   
                                </Columns>
                                <PagerTemplate>

                                    <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                        <asp:Button ID="cmdAnterior" runat="server" Text="<< Anterior" OnClick="cmdAnterior_Click"
                                            CssClass="btn btn-default" Width="100px" />
                                        <asp:Button ID="cmdPage1" runat="server" Text="1 " OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn btn-default" />
                                        <asp:Button ID="cmdSiguiente" runat="server" Text="Siguiente >>" OnClick="cmdSiguiente_Click"
                                            CssClass="btn btn-default" Width="100px" />
                                    </asp:Panel>

                                </PagerTemplate>
                                <EmptyDataTemplate>
                                    <div>
                                        <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                        <span class="mleft20">No se encontraron registros.</span>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                                
                                <asp:UpdatePanel ID="updBotonesGuardar1" runat="server" >
                                     <ContentTemplate>
                                <div class="control-group">
                                           <asp:Label ID="Label1" runat="server" CssClass="control-label">Cambiar Zona 1 por:</asp:Label>
                                            <div class="controls">
                                                <div class="form-inline">
                                                    <div class="control-group inline-block">
                                                        <asp:DropDownList ID="ddlZona" runat="server"
                                                            CssClass="checkboxlist" RepeatColumns="2">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="control-group inline-block">
                                                        <asp:UpdatePanel ID="updActualizar" runat="server">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="btnActualizarZona1" runat="server" CssClass="btn btn-info" OnClick="btnActualizar_Click"
                                                                    OnClientClick="return ValidarControlesActualizar();">
                                                                <i class="icon-white icon-pencil"></i>
                                                                <span class="text">Actualizar</span>
                                                                </asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                    <div class="control-group inline-block">
                                                        <asp:UpdateProgress ID="UpdateProgressActualizar1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar1">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" /> Guardando...
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>

                                                     <div id="Req_ddlZonaReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe seleccionar una zona.
                                                    </div>
                                               </div>
                                            </div>
                                        </div> 
                                         </ContentTemplate>
                                    </asp:UpdatePanel>

                                 <asp:UpdatePanel ID="updBotonesGuardar2" runat="server" >
                                     <ContentTemplate>
                                    <div class="control-group">
                                           <asp:Label ID="Label3" runat="server" CssClass="control-label">Cambiar Zona 2 por:</asp:Label>
                                            <div class="controls">
                                                <div class="form-inline">
                                                    <asp:DropDownList ID="ddlZona2" runat="server" 
                                                         CssClass="checkboxlist"  RepeatColumns="2">
                                                     </asp:DropDownList>

                                                    <div class="control-group inline-block">
                                                        <asp:UpdatePanel ID="btnActualizar2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="btnActualizarZona2" runat="server" CssClass="btn btn-info" OnClick="btnActualizar2_Click"
                                                                OnClientClick="return ValidarControlesActualizar2();">
                                                                <i class="icon-white icon-pencil"></i>
                                                                <span class="text">Actualizar</span>
                                                                </asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                    <div class="control-group inline-block">
                                                        <asp:UpdateProgress ID="UpdateProgressActualizar2" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar2">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" /> Guardando...
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>

                                                     <div id="Req_ddlZona2Req" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe seleccionar una zona.
                                                    </div>
                                               </div>
                                            </div>
                                        </div> 
                                    </ContentTemplate>
                                    </asp:UpdatePanel>

                                 <asp:UpdatePanel ID="updBotonesGuardar3" runat="server" >
                                     <ContentTemplate>
                                    <div class="control-group">
                                           <asp:Label ID="Label4" runat="server" CssClass="control-label">Cambiar Zona 3 por:</asp:Label>
                                            <div class="controls">
                                                <div class="form-inline">
                                                    <asp:DropDownList ID="ddlZona3" runat="server" 
                                                         CssClass="checkboxlist"  RepeatColumns="2">
                                                     </asp:DropDownList>

                                                    <div class="control-group inline-block">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="btnActualizarZona3" runat="server" CssClass="btn btn-info" OnClick="btnActualizar3_Click"
                                                                OnClientClick="return ValidarControlesActualizar3();">
                                                                <i class="icon-white icon-pencil"></i>
                                                                <span class="text">Actualizar</span>
                                                                </asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                   <div class="control-group inline-block">
                                                        <asp:UpdateProgress ID="UpdateProgressActualizar3" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar3">
                                                            <ProgressTemplate>
                                                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" /> Guardando...
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>

                                                     <div id="Req_ddlZona3Req" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Debe seleccionar una zona.
                                                    </div>
                                               </div>
                                            </div>
                                        </div> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
           
      
    </div>

    <%--modal de mensajes--%>
    <div id="frmMensaje" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Mensaje</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon icon-chevron-down fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMensaje" runat="server" Style="color: Black"></asp:Label>
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



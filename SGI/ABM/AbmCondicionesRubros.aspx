﻿<%@ Page Title="Abm Tipo de Condiciones de Rubros" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmCondicionesRubros.aspx.cs" Inherits="SGI.ABM.AbmCondicionesRubros" %>

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
           

            $("#<%: txtSupMinima.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $("#<%: txtSupMaxima.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            

            return false;
        }
        function init_Js_updpnlCrearActu() {


            $("#<%: txtSupMinimaCondicionReq.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
            $("#<%: txtSupMaximaCondicionReq.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });


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

        function showDatos() {
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

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function validarGuardar() {
            var ret = true;
            hideSummary();
            $("#Val_BotonGuardar").hide();
            $("#Req_txtCodigoCondicionReq").hide();
            $("#Req_txtNombreCondicionReq").hide();
            $("#Req_txtSupMinimaCondicionReq").hide();
            $("#Req_txtSupMaximaCondicionReq").hide();
            $("#Req_txtSupMinimaNumeroReq").hide();
            $("#Req_txtSupMaximaNumeroReq").hide();
            $("#MAX_MIN").hide();
           
            if ($.trim($("#<%: txtCodigoCondicionReq.ClientID %>").val()).length == 0) {
                $("#Req_txtCodigoCondicionReq").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtNombreCondicionReq.ClientID %>").val()).length == 0) {
                $("#Req_txtNombreCondicionReq").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtSupMinimaCondicionReq.ClientID %>").val()).length == 0) {
                $("#Req_txtSupMinimaCondicionReq").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtSupMaximaCondicionReq.ClientID %>").val()).length == 0) {
                $("#Req_txtSupMaximaCondicionReq").css("display", "inline-block");
                ret = false;
            }
            
            
                if (parseInt($("#<%: txtSupMinimaCondicionReq.ClientID %>").val()) > parseInt($("#<%: txtSupMaximaCondicionReq.ClientID %>").val())) {
                    $("#MAX_MIN").css("display", "inline-block");
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

        <%--Busqueda de Condiciones de Rubros--%>
        <div id="box_busqueda" class="widget-box">
		    <div class="widget-title">
			    <span class="icon"><i class="icon-search"></i></span>
			    <h5>B&uacute;squeda de Condiciones de Rubros</h5>
		    </div>
		    <div class="widget-content">
			    <div>
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">
                             <div class="form-horizontal">
                              <fieldset>
                               <div class="form-inline">
                                   <div class="span">
                                <asp:label runat="server" >C&oacute;digo de Condici&oacute;n:</asp:label>                                              
                                <asp:TextBox ID="txtCodigoCondicion" runat="server" CssClass="input-xlarge" Width="75px"></asp:TextBox>                          
                               </div>
                              <div class="span">
                                         <asp:label runat="server" >Superficie M&iacute;nima:</asp:label>   
                                <asp:TextBox ID="txtSupMinima"  runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
                                  </div>
                                     <div class="span">
                                         <asp:label runat="server" >Nombre de Condici&oacute;n:</asp:label>   
                                <asp:TextBox ID="txtNombreCondicion" runat="server" CssClass="input-xlarge" Width="100px"></asp:TextBox>
                                  </div>
                                     <div class="span">
                                         <asp:label runat="server">Superficie M&aacute;xima:</asp:label>   
                                <asp:TextBox ID="txtSupMaxima"  runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
                              </div>
                                          </div>
                             </fieldset>
                            </div>
                               
                                <div class="control-group text-right">
                                    <label class="control-label"></label>
                                        <div class="control-group  inline-block">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updpnlBuscar">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    <div class="controls">
                                        <asp:LinkButton ID="btnNuevaCondicion" runat="server" CssClass="btn" OnClick="btnNuevaCondicion_Click">
                                            <i class="icon-plus"></i>
                                            <span class="text">Nueva Condici&oacute;n de Rubro</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClick="btnBuscar_Click">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Buscar</span>
                                        </asp:LinkButton>

                                        
                                    </div>
                                </div>
				            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
				    
                    <%--Resultados --%>
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                           
                                <div class="text-left">
                                    <span>Cantidad de registros:
                                    <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                </div>
                            </asp:Panel>

                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                PageSize="50" OnDataBound="grd_DataBound">

                                <Columns>

                                    <asp:BoundField DataField="codigo_condicion" HeaderText="Codigo" ItemStyle-Width="50px" ItemStyle-CssClass="text-center"/>
                                    <asp:BoundField DataField="nombre_condicion" HeaderText="Nombre" ItemStyle-Width="200px" ItemStyle-CssClass="text-center"/>
                                    <asp:BoundField DataField="sup_minima" HeaderText="Superficie Mínima" ItemStyle-Width="30px" ItemStyle-CssClass="text-center"/>
                                    <asp:BoundField DataField="sup_maxima" HeaderText="Superficie Máxima" ItemStyle-Width="30px" ItemStyle-CssClass="text-center"/>
                                    

                                    <%--Editar - Eliminar --%>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="3px" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>

                                            <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local" 
                                                CommandArgument='<%# Eval("id_cond") %>' OnClick="btnEditar_Click" >
                                                <i class="icon-edit"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="lnkEliminarCondicionReq" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local" 
                                                CommandArgument='<%# Eval("id_cond") %>' OnClientClick="javascript:return ConfirmDel();"
                                                OnCommand="lnkEliminarCondicionReq_Command">
                                                <i class="icon-remove"></i>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>    
        <%--ABM --%>
        <div id="box_datos" class="widget-box" style="display:none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos de la nueva Condici&oacute;n de Rubro </h5>
            </div>
            <div class="widget-content">
                <div>
                    <div class="col-sm-12 col-md-12">
                        <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hid_id_condReq" runat="server" />
                                <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">

                                 <div class="form-horizontal">
                                   <fieldset>
                                     <div class="form-inline">
                                         <div class="span">
                                        <asp:label runat="server" style="margin-left:0px;">C&oacute;digo:</asp:label>
                                        <asp:TextBox ID="txtCodigoCondicionReq" runat="server" Width="75px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_txtCodigoCondicionReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Código.
                                            </div>
                                             </div>
                                          <div class="span">
                                        <asp:label runat="server">Nombre:</asp:label>
                                        <asp:TextBox ID="txtNombreCondicionReq" runat="server" Width="60px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_txtNombreCondicionReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Nombre.
                                            </div>
                                                 </div>
                                          <div class="span">
                                        <asp:label runat="server">Superficie M&iacute;nima:</asp:label>
                                        <asp:TextBox ID="txtSupMinimaCondicionReq"   runat="server" Width="100px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_txtSupMinimaCondicionReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la superficie Mínima.
                                            </div>
                                            <div id="Req_txtSupMinimaNumeroReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Superficie Mínima debe ser numérico.
                                            </div>
                                                 </div>
                                          <div class="span">
                                        <asp:label runat="server">Superficie M&aacute;xima:</asp:label>
                                        <asp:TextBox ID="txtSupMaximaCondicionReq"  runat="server" Width="60px" MaxLength="100" ></asp:TextBox>                         

                                            <div id="MAX_MIN" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Máximo no puede ser menor al mínimo.
                                            </div>
                                            <div id="Req_txtSupMaximaCondicionReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la superficie Máxima.
                                            </div>
                                            <div id="Req_txtSupMaximaNumeroReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                               Superficie Máxima debe ser numérico.
                                            </div>
                                 
                                         </div>
                                         </div>
                                    </fieldset>
                                   </div>

                                    <div class="control-group" style="padding-top: 50px;">
                                        <label class="control-label">Observaciones del Solicitante:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtObservacionesSolicitantes" runat="server" CssClass="form-control" Columns="12" Width="500px" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>

                                    <asp:UpdatePanel ID="updBotonesGuardar" runat="server" >
                                        <ContentTemplate>
                                            <div class="form-horizontal text-right">
                                                <div class="control-group">
                                                    <div id="ValSummary" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                        Revise las validaciones en pantalla.
                                                    </div>
                                                </div>
                                                <div id="pnlBotonesGuardar" class="control-groupp">
                                                    <div class="controls">
                                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-inverse" OnClick="btnGuardar_Click" OnClientClick="return validarGuardar();">
                                                            <i class="imoon-save"></i>
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
                            <asp:TextBox ID="txtObservacionesSolicitante" runat="server" CssClass="form-control" Columns="10" Width="95%" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Botones --%>
                <div class="modal-footer" style="text-align: left;">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptar_Click" />
                    <asp:Button ID="btnCancelarObservacion" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>



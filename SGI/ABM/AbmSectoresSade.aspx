﻿<%@ Page Title="Abm Sectores SADE" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmSectoresSade.aspx.cs" Inherits="SGI.ABM.AbmSectoresSade" %>


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
            $(".lnkEliminarSectorReq").tooltip({ 'delay': { show: 500, hide: 0 } });
           
            $("#page_content").hide();
            $("#Loading").show();
           

            $("#<%: btnCargarDatos.ClientID %>").click();
        });
    

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }

        function showResultado() {
            $("#box_resultado").show("slow");
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
            $("#Req_txtCodigoSectorReq").hide();
            $("#Req_txtNombreSectorReq").hide();
            $("#Req_txtReparticionSectorReq").hide();
           
           
            if ($.trim($("#<%: txtCodigoSectorReq.ClientID %>").val()).length == 0) {
                $("#Req_txtCodigoSectorReq").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtNombreSectorReq.ClientID %>").val()).length == 0) {
                $("#Req_txtNombreSectorReq").css("display", "inline-block");
                ret = false;
            }
             if ($.trim($("#<%: txtReparticionSectorReq.ClientID %>").val()).length == 0) {
                $("#Req_txtReparticionSectorReq").css("display", "inline-block");
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
                return confirm('¿Esta seguro que desea quitar este sector?');
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

        <%--Busqueda de Sectors--%>
        <div id="box_busqueda" >
            
        <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar" >
            
		    <div class="widget-title">
			    <span class="icon"><i class="icon-search"></i></span>
			    <h5>B&uacute;squeda de Sectores</h5>
		    </div>
		    <div class="widget-content">
			    <div>
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">

                                <div class="control-group">
                                    <label class="control-label">C&oacute;digo de Sector:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCodigoSector" runat="server" CssClass="input-xlarge" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Nombre de Sector:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNombreSector" runat="server" CssClass="input-xlarge" Width="250px"></asp:TextBox>
                                    </div>
                                </div>
                                  <div class="control-group">
                                    <label class="control-label">Repartici&oacute;n:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtReparticionSector" runat="server" CssClass="input-xlarge" Width="250px"></asp:TextBox>
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
                
                                        <asp:LinkButton ID="btnNuevoSector" runat="server" CssClass="btn" OnClick="btnNuevoSector_Click">
                                            <i class="icon-plus"></i>
                                            <span class="text">Nuevo Sector</span>
                                        </asp:LinkButton>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel> 
            
            <br /><br />
            
        <%-- Muestra Resultados--%>
        <div id="box_resultado" style="display:none;">
            <div class="widget-box">     
                    <%--Resultados --%>
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <div class="form-horizontal" style="margin-left:15px;margin-right:15px">
                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                                <div class="text-left">
                                    <span>Cantidad de Sectores:
                                    <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                </div>
                            </asp:Panel>

                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                PageSize="50">
                                <Columns>
                                    <asp:BoundField DataField="codigo_Sector" HeaderText="Codigo" ItemStyle-Width="50px" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Nombre_Sector" HeaderText="Nombre" ItemStyle-Width="200px" ItemStyle-CssClass="text-center"  />
                                    <asp:BoundField DataField="Reparticion_Sector" HeaderText="Repartición" ItemStyle-Width="200px" ItemStyle-CssClass="text-center"  />
                                   
                                    <%--Editar/Eliminar --%>
                                    <asp:TemplateField HeaderText="Acción" HeaderStyle-ForeColor="#337AB7" ItemStyle-Width="3px" ItemStyle-CssClass="text-center" >
                                        <ItemTemplate>

                                              <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local" 
                                                CommandArgument='<%# Eval("id_Sector") %>' OnClick="btnEditar_Click" >
                                                <i class="imoon-edit" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="lnkEliminarReq" runat="server" ToolTip="Eliminar" data-toggle="tooltip" CssClass="link-local" 
                                                CommandArgument='<%# Eval("id_Sector") %>' OnClientClick="javascript:return ConfirmDel();"
                                                OnCommand="lnkEliminarSectorReq_Command">
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
                            </asp:GridView>
                    <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
               </div>
        </div>    
            </div>
        <%--ABM --%>
        <div id="box_datos" style="display:none">
            <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos del Sector SADE </h5>
            </div>
            <div class="widget-content">
                <div>
                    <div class="col-sm-12 col-md-12">
                        <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hid_id_SectorReq" runat="server" />
                                <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label">C&oacute;digo:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCodigoSectorReq" runat="server" Width="350px" MaxLength="10" ></asp:TextBox>                         
                                            <div id="Req_txtCodigoSectorReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Código.
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Nombre:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNombreSectorReq" runat="server" Width="550px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_txtNombreSectorReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Nombre.
                                            </div>
                                        </div>
                                    </div>
                                     <div class="control-group">
                                        <label class="control-label">Repartici&oacute;n:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtReparticionSectorReq" runat="server" Width="550px" MaxLength="100" ></asp:TextBox>                         
                                            <div id="Req_txtReparticionSectorReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Repartición.
                                            </div>
                                        </div>
                                    </div>
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



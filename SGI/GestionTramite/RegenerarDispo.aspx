<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegenerarDispo.aspx.cs" Inherits="SGI.GestionTramite.RegenerarDispo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <script src="../Scripts/Datepicker_es.js"></script>
    <%: Scripts.Render("~/bundles/fileUpload") %>
    <%: Styles.Render("~/bundles/fileUploadCss") %>    
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

        <div id="page_content" style="display:none">
        <%--Busqueda de Profesionales--%>
        <div id="box_busqueda" >
            
            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnGenerar" >
            
	    	    <div class="widget-title">
		    	    <span class="icon"><i class="icon-search"></i></span>
			        <h5>Regenerar Disposición</h5>
		        </div>
		        <div class="widget-content">
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnGenerar" CssClass="form-horizontal">
                                <div class="control-group">
                                    <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="ReqSolicitud" runat="server" ValidationGroup="Reabrir"
                                            ControlToValidate="txtSolicitud" Display="Dynamic" CssClass="field-validation-error"
                                            ErrorMessage="Ingrese la Solicitud."></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
				            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <asp:HiddenField ID="hid_id_tarea" runat="server" Value="0" />
                    <asp:HiddenField ID="hid_id_tramite_tarea" runat="server" Value="0" />
                    <asp:HiddenField ID="hid_id_grupotramite" runat="server" Value="0" />
                    <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
                    <div class="pull-right">
                        <div id="Buscar_CamposReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                Debe ingresar algún valor en los campos para poder realizar la búsqueda.
                        </div>
                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <asp:LinkButton ID="btnGenerar" runat="server" CssClass="btn btn-primary" OnClick="btnGenerar_Click" ValidationGroup="buscar">
                            <i class="icon-white icon-search"></i>
                            <span class="text">Generar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <br /><br />
        </div>
    </div>

    <br />
    <br />
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
        <script type="text/javascript">

        $(document).ready(function () {
            $("#page_content").hide();
            $("#Loading").show();
            $("#<%: btnCargarDatos.ClientID %>").click();
        });

        function init_Js_updpnlBuscar() {
            $("#<%: txtSolicitud.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999' });
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

        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function mostratMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
            sticky: false
            });
        }

        function hideResultado() {
            $("#box_resultado").hide("slow");            
        }
    </script>

</asp:Content>

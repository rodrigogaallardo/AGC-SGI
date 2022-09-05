<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EximirCAA.aspx.cs" Inherits="SGI.GestionTramite.EximirCAA" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>

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



    <div id="page_content" style="display: none">
        <%--Busqueda de Solicitud--%>
        <div id="box_busqueda">
            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar">
                <div class="widget-title">
                    <span class="icon"><i class="icon-search"></i></span>
                    <h5>Eximici&oacute;n de CAA</h5>
                </div>

                <div class="widget-content">
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">
                                <div class="form-horizontal">
                                    <div class="row-fluid" style="padding: 5px">
                                        <div class="span4" style="padding-left: 5px">
                                            <div class="control-group">
                                                <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hid_id_solicitud" runat="server" />
                    <asp:HiddenField ID="hid_tipoTramite" runat="server" />

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
                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" ValidationGroup="buscar">
                            <i class="icon-white icon-search"></i>
                            <span class="text">Buscar</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick">
                            <i class=" icon-refresh"></i>
                            <span class="text">Limpiar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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

    <%-- Muestra Resultados--%>

    <div id="box_resultado" style="display: none;">
        <div class="widget-box">
            <%--Resultados --%>
            <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widget-title">
                        <span class="icon"><i class="icon-list-alt"></i></span>
                        <h5>Datos del Trámite</h5>
                    </div>
                    <div class="widget-content">
                        <div>
                            <div style="width: 50%; float: left">

                                <ul class="cabecera">
                                    <li>Solicitud:<strong><asp:Label ID="lblSol" runat="server"></asp:Label></strong><strong style="color: #64b460"><asp:Label ID="lblExpediente" runat="server"></asp:Label></strong>
                                    </li>
                                    <li>Estado:<strong><asp:Label ID="lblEstado" runat="server"></asp:Label></strong>
                                    </li>
                                    <asp:Panel ID="pnlExpediente" runat="server">
                                        <li>
                                            <asp:Label ID="lblTextEncomienda" runat="server"></asp:Label>:<strong><asp:Label ID="lblEncomienda" runat="server"></asp:Label></strong>
                                        </li>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEximir" runat="server">
                                        <li>
                                            <asp:Label ID="lblEximida" runat="server" Text="Eximir CAA:"></asp:Label>
                                            <asp:CheckBox ID="ChkEximir" runat="server" />
                                        </li>
                                    </asp:Panel>
                                </ul>

                            </div>
                            <asp:Panel ID="pnlUbicaciones" runat="server">
                                <div style="width: 50%; float: left">
                                    <ul class="cabecera" style="padding-left: 10px">
                                        <li>Ubicación:<strong><asp:Label ID="lblUbicacion" runat="server"></asp:Label></strong></li>
                                        <li>Superficie total:<strong><asp:Label ID="lblSuperficieTotal" runat="server"></asp:Label>
                                            m2</strong></li>
                                        <li>Titular/es:<strong><asp:Label ID="lblTitulares" runat="server"></asp:Label></strong>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <br style="clear: both" />
                            <!-- limpia el float anterior -->
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="updPnlGuardar" runat="server">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div id="pnlBotonesGuardar" class="control-groupp">
                            <div class="pull-right">
                                <div class="control-group inline-block">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updPnlGuardar">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-inverse mtop10" OnClick="btnGuardar_OnClick">
                                                            <i class="imoon-save"></i>
                                                            <span class="text">Guardar</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

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
            hideResultado();
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



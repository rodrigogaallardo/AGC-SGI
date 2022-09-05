<%@ Page Title="Modificar Transferencias y Cpadrón" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModificarOperarios.aspx.cs" Inherits="SGI.GestionTramite.ModificarTransferencia" %>



<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Ubicaciones.ascx" TagPrefix="uc1" TagName="Tab_Ubicaciones" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tabs_Tramite.ascx" TagPrefix="uc1" TagName="Tabs_Tramite" %>

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

    

    <div id="page_content" style="display: none">
        <%--Busqueda de Solicitud--%>
        <div id="box_busqueda">
            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar">
                <div class="widget-title">
                    <span class="icon"><i class="icon-search"></i></span>
                    <h5>Modificar Transferencias y Cpadrón</h5>
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
                                                <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solic. Transferencia:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                                </div>                                    
                                            </div>
                                        </div>
                                        <div class="span4" style="padding-left: 5px">
                                            <div class="control-group">
                                                <asp:Label ID="lblCPadron" runat="server" AssociatedControlID="txtCPadron" Text="Nro. Consulta al Padrón:" class="control-label"></asp:Label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtCPadron" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
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
                    <asp:HiddenField ID="hid_id_cpadron" runat="server" Value="0" />

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
    <!-- /.modal -->

   <%-- Muestra Resultados--%>
    
    <div id="box_resultado" style="display: none;">
        <uc1:Tabs_Tramite runat="server" ID="Tabs_Tramite" 
                 OnNroExpedienteSaveClick="lnkNroExpSave" />             
       
    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#page_content").hide();
            $("#Loading").show();
            $("#<%: btnCargarDatos.ClientID %>").click();
        });

        function init_Js_updpnlBuscar() {
            $("#<%: txtSolicitud.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999' });
            $("#<%: txtCPadron.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999' });
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

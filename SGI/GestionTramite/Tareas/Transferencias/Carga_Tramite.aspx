<%@ Page Title="Tarea: Control e informe" async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"  CodeBehind="Carga_Tramite.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Transferencias.Carga_Tramite" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tabs_Tramite.ascx" TagPrefix="uc1" TagName="Tabs_Tramite" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucDocumentoAdjunto.ascx" TagPrefix="uc1" TagName="ucDocumentoAdjunto" %>


<asp:content id="Content1" contentplaceholderid="HeadContent" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="FeaturedContent" runat="server">
</asp:content>
<asp:content id="Content3" contentplaceholderid="MainContent" runat="server">
    
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>

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


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
            <asp:HiddenField ID="hid_id_tramitetarea" runat="server" Value="0" />
            <asp:HiddenField ID="hid_es_Transferencia" runat="server" Value="0" />            
            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="page_content" style="display: none">
        <asp:UpdatePanel ID="updCargaTramite" runat="server">
            <ContentTemplate>
                <uc1:ucCabecera runat="server" ID="ucCabecera" />
                <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
               </ContentTemplate>
            </asp:UpdatePanel>
                <uc1:Tabs_Tramite runat="server" ID="Tabs_Tramite" 
                      OnNroExpedienteSaveClick="lnkNroExpSave" />
        
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>

                <div class="widget-box">
                    <div class="widget-title">
                        <span class="icon"><i class="icon-list-alt"></i></span>
                        <h5><%: Page.Title %></h5>
                    </div>
                    <div class="widget-content">
                        <div style="padding: 20px">
                            <div style="width: 100%;">

                                <uc1:ucObservacionesTarea runat="server" ID="ucObservacionContribuyente"
                                    LabelObservacion="Observaciones para Contribuyente" />

                                <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea"  
                                    LabelObservacion="Observaciones Internas"/>

                                <uc1:ucObservacionesTarea runat="server" ID="ucObservacionInforme"
                                    LabelObservacion="Observaciones para el Informe" ValidationGroup="finalizar" ValidarRequerido="true" />

                                <uc1:ucObservacionesAnteriores runat="server" ID="ucObservacionesAnteriores"
                                    LabelObservacion="Observaciones Anteriores"/>

                                <uc1:ucDocumentoAdjunto runat="server" ID="ucDocumentoAdjunto" 
                                    OnGuardarDocumentoAdjuntoClick="ucDocumentoAdjunto_GuardarDocumentoAdjuntoClick" 
                                    OnEliminarDocumentoAdjuntoClick="ucDocumentoAdjunto_EliminarDocumentoAdjuntoClick" />


                                <div style="width: 100%; border-spacing: 5px">
                                    <label style="margin-left:5px; margin-right:35px">Informe:</label>
                                    <asp:DropDownList ID="ddlTiposInforme" runat="server" Width="200px" AutoPostBack="true"
                                        Style="margin-bottom: 0px">
                                    </asp:DropDownList>
                                    <asp:LinkButton runat="server" OnClick="Unnamed_Click" ValidationGroup="finalizar">
                                        <i class="icon-file"></i>
                                        <strong>Previsualizacion Informe</strong>
                                    </asp:LinkButton>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="Req_ddlTiposInforme" runat="server"
                                        ControlToValidate="ddlTiposInforme" CssClass="field-validation-error"
                                        ErrorMessage="Debe ingresar el Informe" ValidationGroup="finalizar"
                                        Display="Dynamic" Enabled="true" InitialValue="0" />
                                </div>

                                <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" ValidationGroupFinalizar="finalizar" 
                                    OnGuardarClick="ucResultadoTarea_GuardarClick"
                                    OnCerrarClick="ucResultadoTarea_CerrarClick"
                                    OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" 
                                    OnResultadoSelectedIndexChanged="ucResultadoTarea_ResultadoSelectedIndexChanged" />

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--Modal mensajes de error--%>
    <div id="frmError_Carga" class="modal fade" style="display: none;">
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
                                        <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
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
        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showfrmError_Carga() {
            debugger;
            $("#frmError_Carga").modal("show");
            return false;
        }

    </script>
</asp:Content>

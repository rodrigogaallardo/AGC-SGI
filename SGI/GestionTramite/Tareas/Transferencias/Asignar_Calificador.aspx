<%@ Page Title="Tarea: Asignar Calificador" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Asignar_Calificador.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Transferencias.Asignar_Calificador" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnterioresv1.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnterioresv1" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>

<asp:content id="Content1" contentplaceholderid="HeadContent" runat="server">
    <script type="text/javascript">
        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="FeaturedContent" runat="server">
</asp:content>
<asp:content id="Content3" contentplaceholderid="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
    

    <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
    <asp:HiddenField ID="hid_id_tramitetarea" runat="server"  Value="0"/>
   

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5><%: Page.Title %></h5>
        </div>
        <div class="widget-content">
            <div style="padding: 20px">

                <div style="width: 100%;">
                    <uc1:ucListaObservacionesAnteriores ID="ucListaObservacionesAnteriores" runat="server" Collapse="true" />
                    <uc1:ucListaObservacionesAnterioresv1 ID="ucListaObservacionesAnterioresv1" runat="server" Collapse="true" />

                    <asp:Panel ID="pnlUsuarioAsignado" runat="server" Visible="false">
                        <table border="0" style="vertical-align:middle; border-collapse:separate; border-spacing:5px">
                            <tr>
                                <td style="width:120px">
                                    <asp:Label ID="lblUsuarioAsignado" runat="server">Usuario Asignado:</asp:Label>
                                </td>
                                <td>
                                   
                                  <asp:TextBox ID="txtUsuarioasignado" runat="server" Width="350px" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>


                    <asp:Panel ID="pnlAsignarUsuario" runat="server" Visible="false">
                        <table border="0" style="vertical-align:middle; border-collapse:separate; border-spacing:5px">
                            <tr>
                                <td style="width:80px">
                                    <asp:Label ID="lblasignar" runat="server">Asignar a:</asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hid_ddlEmpleado" runat="server" Value=""/>
                                    <asp:DropDownList ID="ddlUsuarioAsignar" runat="server" Width="300px"
                                        onblur="return at_Select2CloseAll(this);">
                                    </asp:DropDownList>

                                    <div class="req">
                                        <asp:CompareValidator ID="cv_asignarUsuario" runat="server" ControlToValidate="ddlUsuarioAsignar"
                                            Display="Dynamic" ErrorMessage="Seleccione calificador." ValidationGroup="asignar"
                                            SetFocusOnError="True" CssClass="field-validation-error"
                                            Type="String" ValueToCompare="0" Operator="NotEqual">

                                        </asp:CompareValidator>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>


                  <%--  <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />--%>


                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" 
                        LabelObservacion="Observaciones internas"/>

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionPlancheta" 
                        LabelObservacion="Observaciones para plancheta"/>

                    <uc1:ucObservacionesTarea runat="server" id="UcObservacionesContribuyente" 
                        LabelObservacion="Observaciones para Contribuyente"/>

                    <uc1:ucObservacionesTarea runat="server" id="ucObservacionProvidencia" 
                        LabelObservacion="Providencia"/>

                    <%--<uc1:ucObservacionesTareav1 runat="server" id="ucObservaciones"/>--%>


                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick" 
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" ValidationGroupFinalizar="asignar" />

                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">

        $(document).ready(function () {

            $("#<%=ddlUsuarioAsignar.ClientID%>").select2({
                allowClear: true,
                placeholder: "Seleccione calificador",
                formatSelection: ddlUsuarioAsignar_formatSelection,
                formatResult: ddlUsuarioAsignar_formatResult
            });

            $("#<%=ddlUsuarioAsignar.ClientID%>").on("change", function () {
                var valores = $(this).select2().val();
                $("#<%=hid_ddlEmpleado.ClientID%>").val(valores);
            });


        });

        function at_Select2CloseAll() {
            // Cerrar todos los combos al abrir uno
            $("select").each(function (ind, elem) {
                $(elem).select2("close");
            });

            return false;
        }

        function ddlUsuarioAsignar_formatSelection(obj) {

            // el texto que se envia desde servidor esta tiene dos campos separados por pipe
            // esta funcion le da un formato usando esos dos campos
            if (obj.text == "") {
                return obj.text;
            }

            var partes = obj.text.split('|');
            var cambiarItem = partes[0];

            return cambiarItem;
        }

        function ddlUsuarioAsignar_formatResult(obj) {

            // el texto que se envia desde servidor esta tiene dos campos separados por pipe
            // esta funcion le da un formato usando esos dos campos
            if (obj.text == "") {
                return obj.text;
            }

            var partes = obj.text.split('|');
            var cambiarItem = "";
            if (partes.length >= 2) {
                cambiarItem = partes[0] + " (<span class='badge'>" + partes[1] + "</span>)";
            }
            else {
                cambiarItem = partes[0] + " (<span class='badge'>Sin tr&aacute;mites pendientes</span>)";
            }

            return cambiarItem;

        }


    </script>

</asp:content>

<%@ Page Title="Visualización del trámite" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisorTramite_CP.aspx.cs" Inherits="SGI.GestionTramite.VisorTramite_CP" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />
    
    
    <div id="box_observaciones" class="accordion-group widget-box">

        <%-- titulo collapsible observacioness--%>
        <div class="accordion-heading">
            <a id="observaciones_btnUpDown" data-parent="#collapse-group" href="#collapse_observaciones"
                data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                <asp:HiddenField ID="hid_observaciones_collapse" runat="server" Value="true" />
                <asp:HiddenField ID="hid_observaciones_visible" runat="server" Value="false" />

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="lbl_observaciones_tituloControl" runat="server" Text="Observaciones"></asp:Label></h5>
                    <span class="btn-right"><i class="imoon imoon-chevron-down"></i></span>
                </div>
            </a>
        </div>

        <%-- contenido del collapsible observaciones --%>
        <div class="accordion-body collapse" id="collapse_observaciones">
            <div class="widget-content">
                <asp:UpdatePanel ID="updObservaciones" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table border="0" style="border-collapse: separate; border-spacing: 5px;">
                            <tr>
                                <td>Observaciones: </td>
                                <td>
                                    <asp:Label ID="lblObservaciones" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <uc1:ucListaTareas runat="server" ID="ucListaTareas" />

    <script type="text/javascript">
        function observaciones_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(obj).find("i.icon-chevron-down").length > 0) {
                    $(obj).find("i.icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
                else {
                    $(obj).find("i.icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }

    </script>
</asp:Content>

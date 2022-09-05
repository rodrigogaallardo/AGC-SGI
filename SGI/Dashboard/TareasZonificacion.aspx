<%@ Page Title="Tareas de Zonificación" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TareasZonificacion.aspx.cs" Inherits="SGI.Dashboard.TareasZonificacion" %>

<%@ Register Src="~/Dashboard/Controls/ucGraficoTorta.ascx" TagPrefix="uc" TagName="ucGraficoTorta" %>
<%@ Register Src="~/Dashboard/Controls/ucGraficoTorta2.ascx" TagPrefix="uc" TagName="ucGraficoTorta2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                    sticky: false
                });
            }
        function visibleTorta2() {
            $('#<%=celdaTorta2.ClientID%>').show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <hgroup class="title">
        <h1>Tareas de Zonificación</h1>
    </hgroup>


    <div style="width: 100%; padding-top: 10px;">
        <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Solicitudes en Tarea de Zonificación</h5>
            </div>

            <div class="widget-content">
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell VerticalAlign="Top">
                            <uc:ucGraficoTorta runat="server" ID="ucGraficoTorta" OnError="ucGraficoTorta_Error"/>
                        </asp:TableCell>
                        <asp:TableCell ID="celdaTorta2" VerticalAlign="Top" style="display:none">
                            <uc:ucGraficoTorta2 runat="server" ID="ucGraficoTorta2" OnError="ucGraficoTorta2_Error"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </div>
</asp:Content>

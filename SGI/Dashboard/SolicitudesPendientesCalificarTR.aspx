<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="SolicitudesPendientesCalificarTR.aspx.cs" 
    Inherits="SGI.Dashboard.SolicitudesPendientesCalificarTR" %>

<%@ Register Src="~/Dashboard/Controls/ucSolicitudesPendientesCalificarTR.ascx" TagPrefix="uc1" TagName="ucSolicitudesPendientesCalificarTR" %>
<%@ Register Src="~/Dashboard/Controls/ucSolicitudesAsignadasCalificarTR.ascx" TagPrefix="uc1" TagName="ucSolicitudesAsignadasCalificarTR" %>

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
        function visiblePanel2() {
            $('#<%=celdaTorta2.ClientID%>').show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
    .plot_pie {
	    width: 600px;
	    height: 250px;
    }

	</style>
    <hgroup class="title">
        <h1>Panel de control</h1>
    </hgroup>
    <div style="width: 100%; padding-top: 10px;">
        <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Solicitudes asignados por calificador</h5>
            </div>
            <div class="widget-content">
                <asp:Table ID="Table1" runat="server">
                    <asp:TableRow>
                        <asp:TableCell VerticalAlign="Top">
                            <uc1:ucSolicitudesPendientesCalificarTR runat="server" ID="ucSolicitudesPendientesCalificar" 
                            OnError="ucSolicitudesPendientes_Error"/>
                        </asp:TableCell>
                        <asp:TableCell ID="celdaTorta2" VerticalAlign="Top" style="display:none">
                            <uc1:ucSolicitudesAsignadasCalificarTR runat="server" ID="ucSolicitudesAsignadasCalificar" 
                                OnError="ucSolicitudesAsignadas_Error"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </div>
</asp:Content>

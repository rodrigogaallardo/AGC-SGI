<%@ Page Title="Indicadores" Language="C#" MasterPageFile="~/Site.Master" 
    CodeBehind="SSP_Indicadores.aspx.cs" 
    Inherits="SGI.Dashboard.SSP_Indicadores" %>

<%@ Register Src="~/Dashboard/Controls/ucSSP_Indicadores.ascx" TagPrefix="uc1" TagName="ucSSP_Indicadores" %>


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

    <div class="widget-box">

        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5>Indicadores</h5>
        </div>

        <div class="widget-content">

            <uc1:ucSSP_Indicadores ID="uc_indicadores" runat="server" 
                OnError="ucSSP_Indicadores_Error" />
  
        </div>

    </div>
    
</asp:Content>




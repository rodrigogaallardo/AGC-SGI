<%@ Page Title="Tarea: Aprobado" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="Aprobado.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Transferencias.Aprobado" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaObservacionesAnteriores.ascx" TagPrefix="uc1" TagName="ucListaObservacionesAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaResultadoTareasAnteriores.ascx" TagPrefix="uc1" TagName="ucListaResultadoTareasAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>



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

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaDocumentos runat="server" ID="ucListaDocumentos" />

    <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
    <asp:HiddenField ID="hid_id_tramitetarea" runat="server" Value="0" />

    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5><%: Page.Title %></h5>
        </div>
        <div class="widget-content">
            <div style="padding: 20px">
                <div style="width: 100%;">

           
                    <uc1:ucListaObservacionesAnteriores ID="ucListaObservacionesAnteriores" runat="server" />

                    <uc1:ucListaResultadoTareasAnteriores ID="ucListaResultadoTareasAnteriores" runat="server" Collapse="true" Titulo="Resultado Tarea Anterior" />

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />


                    <uc1:ucProcesosSADEv1 runat="server" id="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />
                    

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />

                </div>
            </div>
        </div>
    </div>

</asp:content>


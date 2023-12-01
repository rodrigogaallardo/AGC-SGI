<%@ Page Title="Tarea: Revisión Firma Disposición" MasterPageFile="~/Site.Master" Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Revision_Firma_Disposicion.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Transferencias.Revision_Firma_Disposicion" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucProcesosSADEv1.ascx" TagPrefix="uc1" TagName="ucProcesosSADEv1" %>
<%@ Register Src="~/GestionTramite/Controls/ucPreviewDocumentos.ascx" TagPrefix="uc1" TagName="ucPreviewDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/Transferencias/ucTitulares.ascx" TagPrefix="uc1" TagName="ucTitulares" %>

<asp:content id="Content3" contentplaceholderid="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>

    <uc1:uccabecera runat="server" id="ucCabecera" />
    <uc1:ucTitulares runat="server" id="ucTitulares" />
    <uc1:uclistadocumentos runat="server" id="ucListaDocumentos" />
    <uc1:ucPreviewDocumentos runat="server" id="ucPreviewDocumentos" />

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

                    <uc1:ucProcesosSADEv1 runat="server" ID="ucProcesosSADE" OnFinalizadoEnSADE="ucProcesosSADE_FinalizadoEnSADE" />

                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea" 
                        btnGuardar_Visible ="false"
                        OnCerrarClick="ucResultadoTarea_CerrarClick" 
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick"  />

               </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">


        // el script para mostrar errores debe estar arriba porque en caso de que de error en 
        // load page debe tener el script cargado desde el inicio
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

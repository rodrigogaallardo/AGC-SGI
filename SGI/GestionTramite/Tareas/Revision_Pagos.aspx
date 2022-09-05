<%@ Page Title="Tarea: Revisión de Pagos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Revision_Pagos.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Revision_Pagos" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucMediosPagos.ascx" TagPrefix="uc1" TagName="ucMediosPagos" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>

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

    <%: Scripts.Render("~/bundles/Unicorn") %>

    <uc1:ucCabecera runat="server" ID="ucCabecera" />
    <uc1:ucListaRubros runat="server" ID="ucListaRubros" />
    <uc1:ucTramitesRelacionados runat="server" id="ucTramitesRelacionados" />
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

                    <div class="widget-box">

                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>Boleta/s Unica/s generada/s</h5>
                        </div>

                        <div class="widget-content">


                            <asp:GridView ID="grdPagosGeneradosBU" runat="server" AutoGenerateColumns="false" DataKeyNames="id_pago"
                                AllowPaging="false" GridLines="None" Width="880px" CssClass="table table-striped table-bordered" 
                                OnRowDataBound="grdPagosGeneradosBU_OnRowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="id_pago" HeaderText="Nº identificación"
                                        ItemStyle-Width="90px" ItemStyle-CssClass="align-center" />

                                    <asp:BoundField DataField="CreateDate" HeaderText="Fecha" ItemStyle-Width="80px"
                                        HeaderStyle-HorizontalAlign="Left" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="align-center" />

                                    <asp:BoundField DataField="monto_pago" HeaderText="Monto Total" DataFormatString="{0:$ #,##0.00 }"
                                        ItemStyle-Width="90px" ItemStyle-CssClass="align-right" />


                                    <asp:TemplateField ItemStyle-CssClass="align-center" HeaderText="Estado">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescripicionEstadoPago" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="titulo-4">
                                        No se ingresaron 
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </div>

                    </div>

                    
                    
                    
                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />

                </div>
            </div>
        </div>
    </div>

</asp:Content>

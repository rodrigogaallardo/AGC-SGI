<%@ Page Title="Tarea: Validar Zonificación" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Validar_Zonificacion.aspx.cs" Inherits="SGI.GestionTramite.Tareas.Validar_Zonificacion" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaTareas.ascx" TagPrefix="uc1" TagName="ucListaTareas" %>
<%@ Register Src="~/GestionTramite/Controls/ucObservacionesTarea.ascx" TagPrefix="uc1" TagName="ucObservacionesTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucResultadoTarea.ascx" TagPrefix="uc1" TagName="ucResultadoTarea" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaRubros.ascx" TagPrefix="uc1" TagName="ucListaRubros" %>
<%@ Register Src="~/GestionTramite/Controls/ucSGI_ListaDocumentoAdjuntoAnteriores.ascx" TagPrefix="uc1" TagName="ucSGI_ListaDocumentoAdjuntoAnteriores" %>
<%@ Register Src="~/GestionTramite/Controls/ucDocumentoAdjunto.ascx" TagPrefix="uc1" TagName="ucDocumentoAdjunto" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramitesRelacionados.ascx" TagPrefix="uc1" TagName="ucTramitesRelacionados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <script type="text/javascript">

        $(document).ready(function () {
            $(".tabFotos").tabs();
        });


        function noExisteFotoParcela(objimg) {

            $(objimg).attr("src", "../../Content/img/app/ImageNotFound.png");
            fotoCargada();
            return true;
        }

        function fotoCargada(objOcultar, objMostrar) {

            $("#" + objOcultar).css("display", "none");
            $(objMostrar).css("display", "inherit");

            return true;
        }

        function mostratMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

    </script>

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

                    <uc1:ucSGI_ListaDocumentoAdjuntoAnteriores runat="server" ID="ucSGI_ListaDocumentoAdjuntoAnteriores" Collapse="true" />
                    <uc1:ucDocumentoAdjunto runat="server" ID="ucDocumentoAdjunto" />

                    <asp:GridView ID="grdDatosUbicacion" runat="server" AutoGenerateColumns="false" 
                        ShowHeader="false" Width="100%" GridLines="None"  ItemType="ENC_Ubicacion"
                        OnRowDataBound="grdDatosUbicacion_OnRowDataBound" 
                        AllowPaging="false" ShowFooter="false">


                        <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>

                                    <table border="0" style="width: 100%">
                                        <tr>
                                            <td style="vertical-align: top; width: 40%" >

                                                <h4>Datos de la Ubicaci&oacute;n</h4>

                                                <asp:Panel ID="pnlDatosUbicacionSMP" runat="server">

                                                    <div>
                                                        <span >Zona:</span>
                                                        <span style="padding-left:3px;color:#4f4ab8"><%# Item.ZonaDeclarada %> - <%# Item.DescripcionZonaPla %></span>
                                                    </div>

                                                    <div>
                                                        <span>Número de Partida Matriz: </span>
                                                        <span style="padding-left:3px; color:#4f4ab8"><%# Item.NroPartidaMatriz %></span>
                                                    </div>
                                                    <div>
                                                        <span>Sección:</span>
                                                        <span style="padding-left:3px; color:#4f4ab8"><%# Item.Seccion %></span>
                                                        <span style="padding-left:10px;">Manzana: </span>
                                                        <span style="padding-left:3px; color:#4f4ab8"><%# Item.Manzana %></span>
                                                        <span style="padding-left:10px;">Parcela:</span>
                                                        <span style="padding-left:3px; color:#4f4ab8"><%# Item.Parcela %></span>
                                                    </div>
                                                </asp:Panel>

                                                <asp:DataList ID="lstDatosUbicacion_Puertas" runat="server" 
                                                    RepeatColumns="1" RepeatDirection="Vertical" 
                                                    RepeatLayout="Table" Width="100%" ItemType="ENC_Ubicacion.Puerta">

                                                    <HeaderTemplate>
                                                        <h4>Puertas</h4>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <div>
                                                            <span><%# Item.Nombre_calle %></span>
                                                            <span style="padding-left:5px; "><%# Item.NroPuerta %></span>
                                                        </div>
                                                        
                                                    </ItemTemplate>

                                                </asp:DataList>

                                                <asp:Panel ID="pnlDatosUbicacion_ph" runat="server" Visible="false">

                                                
                                                    <asp:DataList ID="lstDatosUbicacion_ph" runat="server" 
                                                        RepeatColumns="1" RepeatDirection="Vertical" 
                                                        RepeatLayout="Table" Width="100%" ItemType="ENC_Ubicacion.PH">

                                                        <HeaderTemplate>
                                                            <h4>Partida Horizontal</h4>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <span style="color:#4f4ab8"><%# Item.NroPartidaHorizontal %></span>
                                                            <span style="padding-left:20px;"><%# Item.DescripcionCompleta %></span>
                                                        
                                                        </ItemTemplate>
                                                    </asp:DataList> 
                                                </asp:Panel>

                                            </td>
                                            <td style="width: 60%">
                                        
                                                <div class="tabFotos" id="tabFotos">

                                                    <ul>
                                                        <li id="li2">
                                                            <a href="#tabFotos-2">
                                                               <i class='icon-th'></i>
                                                                <span>Mapas</span>
                                                            </a>
                                                         
                                                        </li>
                                                        <li id="li1">
                                                            <a href="#tabFotos-1">
                                                                <i class='icon-camera'></i>
                                                                <span>Foto</span>
                                                            </a>
                                                            
                                                        </li>
                                                    </ul>

                                                    <div id="tabFotos-2">

                                                        <div style="margin-top: 10px; min-height: 250px; min-width: 300px; display: inline; margin-left: 10px; margin-bottom:10px">
                                                            <img id="imgCargandoBack2<%#Container.DataItemIndex%>" src="../../Content/img/app/Loading128x128.gif" alt="" style="margin-left: 60px; margin-top: 40px" />
                                                            <img id="imgFotoBack2" class="img-polaroid" src='<%# Item.GetUrlMapa(300,250, "") %>' onload="fotoCargada('imgCargandoBack2<%#Container.DataItemIndex%>',this);" onerror="noExisteFotoParcela(this);" style="display: none" />
                                                        </div>

                                                        <div style="margin-top: 10px; min-height: 250px; min-width: 300px; display: inline; margin-left: 10px;">
                                                            <img id="imgCargandoBack3<%#Container.DataItemIndex%>" src="../../Content/img/app/Loading128x128.gif" alt="" style="margin-left: 60px; margin-top: 40px" />
                                                            <img id="imgFotoBack3" class="img-polaroid" src="<%# Item.GetUrlCroquis(300,250) %>" onload="fotoCargada('imgCargandoBack3<%#Container.DataItemIndex%>',this);" onerror="noExisteFotoParcela(this);" style="display: none" />
                                                        </div>

                                                    </div>

                                                    <div id="tabFotos-1">

                                                        <div style="margin-top: 10px; min-height: 250px; min-width: 300px; margin-left: 10px"">
                                                            <img id="imgCargandoBack1<%#Container.DataItemIndex%>" src="../../Content/img/app/Loading128x128.gif" alt="" style="margin-left: 60px; margin-top: 40px" />
                                                            <img id="imgFotoParcelaBack1" class="img-polaroid" src="<%# Item.GetUrlFoto(300,250) %>" onload="fotoCargada('imgCargandoBack1<%#Container.DataItemIndex%>',this);" onerror="noExisteFotoParcela(this);" style="display: none" />
                                                        </div>

                                                    </div>

                                                </div>




                                            </td>

                                        </tr>

                                    </table>


                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>


                    </asp:GridView>


                    <uc1:ucObservacionesTarea runat="server" ID="ucObservacionesTarea"
                            ValidationGroup="finalizar"  ValidarRequerido="true" />

                    <uc1:ucResultadoTarea runat="server" ID="ucResultadoTarea"
                        ValidationGroupFinalizar="finalizar"
                        OnGuardarClick="ucResultadoTarea_GuardarClick"
                        OnCerrarClick="ucResultadoTarea_CerrarClick"
                        OnFinalizarTareaClick="ucResultadoTarea_FinalizarTareaClick" />

                </div>
            </div>
        </div>
    </div>




</asp:Content>

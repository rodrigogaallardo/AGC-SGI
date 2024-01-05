<%@ Page Title="Visualización del trámite" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisorTramite_SSIT.aspx.cs" Inherits="SGI.GestionTramite.Consulta_SSIT.VisorTramite_SSIT" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<%@ Register Src="~/GestionTramite/Controls/CPadron/Ubicacion.ascx" TagPrefix="uc" TagName="Ubicacion" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_DatosLocal.ascx" TagPrefix="uc1" TagName="Tab_DatosLocal" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Rubros.ascx" TagPrefix="uc1" TagName="Tab_Rubros" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Titulares.ascx" TagPrefix="uc1" TagName="Tab_Titulares" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_TitularesSolicitud.ascx" TagPrefix="uc1" TagName="Tab_TitularesSol" %>
<%@ Register Src="~/GestionTramite/Controls/Transferencias/ucTitulares.ascx" TagPrefix="uc1" TagName="Tab_TitularesTr" %>
<%@ Register Src="~/GestionTramite/Controls/ucUbicacion.ascx" TagPrefix="uc1" TagName="Ubicacion_hab" %>
<%@ Register Src="~/GestionTramite/Controls/ucTitulares.ascx" TagPrefix="uc1" TagName="Titulares_hab" %>
<%@ Register Src="~/GestionTramite/Controls/ucRubros.ascx" TagPrefix="uc1" TagName="Rubros_hab" %>
<%@ Register Src="~/GestionTramite/Controls/ucDatosLocal.ascx" TagPrefix="uc1" TagName="DatosLocal_hab" %>
<%@ Register Src="~/GestionTramite/Controls/ucEncomienda.ascx" TagPrefix="uc1" TagName="Encomienda_sol" %>
<%@ Register Src="~/GestionTramite/Controls/ucAnexoNotarial.ascx" TagPrefix="uc1" TagName="AnexoNotarial_sol" %>
<%@ Register Src="~/GestionTramite/Controls/ucTramiteCAA.ascx" TagPrefix="uc1" TagName="TramiteCAA" %>
<%@ Register Src="~/GestionTramite/Controls/ucCabeceraV2.ascx" TagPrefix="uc1" TagName="CabeceraV2" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosSSIT.ascx" TagPrefix="uc1" TagName="DocumentoAdjunto" %>
<%@ Register Src="~/GestionTramite/Controls/ucHistorial.ascx" TagPrefix="uc1" TagName="Historial" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/Unicorn.Tables") %>

    <asp:HiddenField ID="hid_return_url" runat="server" />

    <div id="Cabecera">
        <uc1:CabeceraV2 runat="server" ID="CabeceraV2" />
    </div>

    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="box_ubicacion" class="accordion-group widget-box">

                <%-- titulo collapsible ubicaciones--%>
                <div class="accordion-heading">
                    <a id="ubicacion_btnUpDown" data-parent="#collapse-group">
                        <div class="widget-title">
                            <span class="icon"><i class="imoon imoon-map-marker"></i></span>
                            <h5>
                                <asp:Label ID="lbl_ubicacion_tituloControl" runat="server" Text="Ubicación"></asp:Label></h5>
                        </div>
                    </a>
                </div>
                <%-- contenido del collapsible ubicaciones --%>
                <div class="accordion-body collapse in" id="collapse_ubicacion">
                    <div class="widget-content">
                        <uc:Ubicacion runat="server" ID="visUbicaciones" />
                    </div>
                </div>
            </div>
            <div id="tabDatosLocal">
                <uc1:Tab_DatosLocal runat="server" ID="Tab_DatosLocal" />
            </div>
            <div id="tabRubros">
                <uc1:Tab_Rubros runat="server" ID="Tab_Rubros" />
            </div>
            <div id="tabTitulares" runat="server">
                <uc1:Tab_Titulares runat="server" ID="Tab_Titulares" />
            </div>
            <div id="tabTitularesSol" runat="server">
                <uc1:Tab_TitularesSol runat="server" ID="Tab_TitularesSol" />
            </div>
            <div id="tabTitularesTr" runat="server">
                <uc1:Tab_TitularesTr runat="server" ID="Tab_TitularesTr" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upSolicitud" runat="server">
        <ContentTemplate>
            <div id="Ubicacion" class="accordion-group widget-box">

                <%-- titulo collapsible ubicaciones--%>
                <div class="accordion-heading">
                    <a id="ubicacion_btn" data-parent="#collapse-group">
                        <div class="widget-title">
                            <span class="icon"><i class="imoon imoon-map-marker"></i></span>
                            <h5>
                                <asp:Label ID="Label1" runat="server" Text="Ubicación"></asp:Label></h5>
                        </div>
                    </a>
                </div>
                <%-- contenido del collapsible ubicaciones --%>
                <div class="accordion-body collapse in" id="collap_ubicacion">
                    <div class="widget-content">
                        <uc1:Ubicacion_hab runat="server" ID="Ubicacion_hab" />
                    </div>
                </div>
            </div>
            <div id="DatosLocalhab" runat="server">
                <uc1:DatosLocal_hab runat="server" ID="DatosLocal_hab" />
            </div>
            <div id="enc" runat="server">
                <uc1:Encomienda_sol runat="server" ID="Encomienda_sol" />
            </div>
            <div id="Rubroshab" runat="server">
                <uc1:Rubros_hab runat="server" ID="Rubros_hab" />
            </div>
            <div id="Titulareshab" runat="server">
                <uc1:Titulares_hab runat="server" ID="Titulares_hab" />
            </div>
            <div id="Anexo" runat="server">
                <uc1:AnexoNotarial_sol runat="server" ID="AnexoNotarial_sol" />
            </div>
            <div id="CAA" runat="server">
                <uc1:TramiteCAA runat="server" ID="TramiteCAA" />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="Documentos" runat="server">
        <uc1:DocumentoAdjunto runat="server" ID="DocumentoAdjunto" />
    </div>
    <div id="HistorialTramite">
        <uc1:Historial runat="server" ID="Historial" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="pull-right">

                <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-primary" OnClick="btnVolver_Click">
                            <i class="icon-white icon-arrow-left"></i>
                            <span class="text">Volver</span>
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

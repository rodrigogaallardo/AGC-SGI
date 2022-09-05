<%@ Page Title="Panel de Estados" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Estado_mail.aspx.cs"
    Inherits="SGI.GestionTramite.Estado_mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>



    <asp:UpdatePanel ID="updPnlEstados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- filtros de busqueda--%>
            <div class="accordion-group widget-box">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>Panel de estados</h5>
                </div>

                <div class="widget-content">
                    <div class="form-horizontal">
                        <fieldset>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblMailEnviado" runat="server" AssociatedControlID="txtMailEnviado" Text="Ultimo mail enviado:" class="control-label"></asp:Label>
                                        <asp:Label ID="txtMailEnviado" style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblProceso" runat="server" AssociatedControlID="txtProceso" Text="Ultimo proceso ejecutado:" class="control-label"></asp:Label>
                                        <asp:Label ID="txtProceso" style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblEnviadoHoy" runat="server" AssociatedControlID="txtEnviadoHoy" Text="Total enviados hoy:" class="control-label"></asp:Label>
                                        <asp:Label ID="txtEnviadoHoy" style="text-align: center;" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                    </div>
                                </div>
                                <%--<div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="Label3" runat="server" AssociatedControlID="txtProceso" Text="Ultimo proceso ejecutado:" class="control-label"></asp:Label>
                                        <asp:Label ID="Label4" Font-Size="Medium" Font-Italic="true" runat="server" class="control-label"></asp:Label>
                                    </div>
                                </div>--%>
                            </div>

                        </fieldset>
                    </div>
                </div>
    
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
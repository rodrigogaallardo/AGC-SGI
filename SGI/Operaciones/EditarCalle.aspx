<%@ Title="Editar Calle" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarCalle.aspx.cs" Inherits="SGI.EditarCalle"%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function redirect(obj) {
            location.href = obj.data - href;
        }
    </script>

    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

<table>
    <tr>
        <td style="vertical-align:top">
            <div class="form-horizontal" style="width: auto">
                <fieldset>
                    <%--Codigo Calle--%>
                    <div class="control-group">
                        <asp:Label ID="lblCodigoCalle" runat="server" AssociatedControlID="codigoCalle" Text="Código" CssClass="control-label" />
                        <div class="controls">
                            <asp:TextBox ID="codigoCalle" runat="server" Width="100px" Enabled="false" />
                        </div>
                    </div>

                    <%--Altura Izquierda Inicio--%>
                    <div class="control-group">
                        <asp:Label ID="lblAltIzqInicio" runat="server" AssociatedControlID="altIzqInicio" 
                            Text="Altura Izquierda Inicio" CssClass="control-label" />
                        <div class="controls">
                            <asp:TextBox ID="altIzqInicio" runat="server" Width="100px" />
                        </div>
                    </div>

                    <%--Altura Derecha Inicio--%>
                    <div class="control-group">
                        <asp:Label ID="lblAltDerInicio" runat="server" AssociatedControlID="altDerInicio"
                            Text="Altura Derecha Inicio" CssClass="control-label" />
                        <div class="controls">
                            <asp:TextBox ID="altDerInicio" runat="server" Width="100px" />
                        </div>
                    </div>
                </fieldset>
            </div>
        </td>

        <td style="vertical-align:top">
            <div class="form-horizontal" style="width: auto">
                <fieldset>

                    <%--Nombre Calle--%>
                    <div class="control-group">
                        <asp:Label ID="lblCalle" runat="server" AssociatedControlID="nombreCalle"
                            Text="Calle" CssClass="control-label" />
                        <div class="controls">
                            <asp:TextBox ID="nombreCalle" runat="server" Width="300px" />
                        </div>
                    </div>

                    <%--Altura Izquierda Fin--%>
                    <div class="control-group">
                       <asp:Label ID="lblAltIzqFin" runat="server" AssociatedControlID="altIzqFin"
                           Text="Fin" CssClass="control-label" />
                        <div class="controls">
                            <asp:TextBox ID="altIzqFin" runat="server" Width="100px" />
                        </div>
                    </div>

                    <%--Altura Derecha Fin--%>
                    <div class="control-group">
                        <asp:Label ID="lblAltDerFin" runat="server" AssociatedControlID="altDerFin"
                            Text="Fin" CssClass="control-label" />
                        <div class="controls">
                            <asp:TextBox ID="altDerFin" runat="server" Width="100px" />
                        </div>
                    </div>
                </fieldset>
            </div>
        </td>

        <td style="vertical-align:top">
            <div class="form-horizontal" style="width: auto">
                <fieldset>
                    <%--Tipo Tramite--%>
                    <div class="control-group">
                        <asp:Label ID="lblTipoCalle" runat="server" AssociatedControlID="ddlTipoCalle"
                            Text="Tipo de Calle" class="control-label"></asp:Label>
                        <div class="controls">
                            <asp:DropDownList ID="ddlTipoCalle" runat="server" Width="150px"></asp:DropDownList>
                        </div>
                    </div>
                </fieldset>
            </div>
        </td>
    </tr>
</table>

    
    <%--modal de Errores--%>
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Error</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updResultados" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--modal de Success--%>
    <div id="frmSuccess" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Éxito.</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-ok-sign fs64" style="color: #67eb34"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updResultados2" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblSuccess" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

        <%--Botones--%>
        <div class="control-group" style="float: right">
            <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary" ValidationGroup="caducar" OnClick="btnAgregar_OnClick" Text="Agregar"></asp:Button>
            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClientClick="LimpiarFormulario();">
            <i class="icon-refresh"></i>
            <span class="text">Limpiar</span>
            </asp:LinkButton>
            <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_OnClick" CssClass="btn btn-primary" />
        </div>


<script>
    function LimpiarFormulario() {
        document.getElementById("MainContent_codigoCalle").value = "";
        document.getElementById("MainContent_altIzqInicio").value = "";
        document.getElementById("MainContent_altIzqFin").value = "";
        document.getElementById("MainContent_altDerInicio").value = "";
        document.getElementById("MainContent_altDerFin").value = "";
        document.getElementById("MainContent_nombreCalle").value = "";
        document.getElementById("MainContent_ddlTipoCalle").value = "";
    }

    function showfrmError() {
        $("#frmError").modal("show");
        return false;
    }

    function showfrmSuccess() {
        $("#frmSuccess").modal("show");
        return false;
    }


</script>
</asp:Content>

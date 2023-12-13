<%@ Title="Consulta Movimientos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ConsultaMovimientos.aspx.cs" Inherits="SGI.ConsultaMovimientos"%>

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
            <td style="vertical-align: top">
                <div class="form-horizontal" style="width: 50%; display: inline-block;">
                    <fieldset>
                        <%--Usuario HACER UN DROPDOWN--%>
                        <div class="control-group">
                            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="usuario" Text="Usuario" CssClass="control-label" />
                            <div class="controls">
                                <asp:TextBox ID="txtUsuario" runat="server" Width="100px" />
                            </div>
                        </div>

                        <%--Fecha Desde:--%>
                        <div class="control-group">
                            <label for="txtFechaDesde" class="control-label">Fecha Desde:</label>
                            <div class="controls">
                                <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                <div class="req">
                                    <asp:RegularExpressionValidator
                                        ID="rev_txtFechaDesde" runat="server"
                                        ValidationGroup="buscar"
                                        ControlToValidate="txtFechaDesde" CssClass="field-validation-error"
                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                        Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>

                        <%--Tipo Movimiento--%>
                        <div class="control-group">
                            <asp:Label ID="lblTipoMov" runat="server" AssociatedControlID="ddlTipoMov"
                                Text="Tipo de Movimiento" class="control-label"></asp:Label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlTipoMov" runat="server" Width="150px">
                                    <asp:ListItem Text="" Value="" />
                                    <asp:ListItem Text="Agregado" Value="I" />
                                    <asp:ListItem Text="Modificación" Value="U" />
                                    <asp:ListItem Text="Eliminación" Value="D" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <%--Funcionalidad--%>
                        <div class="control-group">
                            <asp:Label ID="lblFuncionalidad" runat="server" AssociatedControlID="funcionalidad" Text="Funcionalidad" CssClass="control-label" />
                            <div class="controls">
                                <asp:TextBox ID="txtFuncionalidad" runat="server" Width="100px" />
                            </div>
                        </div>

                        <%--Observacion Solicitante--%>
                        <div class="control-group">
                            <asp:Label ID="lblObservacionSolicitante" runat="server" AssociatedControlID="ddlObservacionSolicitante"
                                Text="Observacion Solicitante" class="control-label"></asp:Label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlObservacionSolicitante" runat="server" Width="150px">
                                    <asp:ListItem Text="" Value="" />
                                    <asp:ListItem Text="Todos" Value="Todos" />
                                    <asp:ListItem Text="Si" Value="Si" />
                                    <asp:ListItem Text="No" Value="No" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </td>
            <td style="vertical-align: top">
                <div class="form-horizontal" style="width: 50%; display: inline-block;">
                    <fieldset>
                        <%--Solicitud--%>
                        <div class="control-group">
                            <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="solicitud" Text="Solicitud" CssClass="control-label" />
                            <div class="controls">
                                <asp:TextBox ID="txtSolicitud" runat="server" Width="100px" />
                            </div>
                        </div>

                        <%--Fecha Hasta:--%>
                        <div class="control-group">
                            <label for="txtFechaHasta" class="control-label">Fecha Hasta:</label>
                            <div class="controls">
                                <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                <div class="req">
                                    <asp:RegularExpressionValidator
                                        ID="rev_txtFechaHasta" runat="server"
                                        ValidationGroup="buscar"
                                        ControlToValidate="txtFechaDesde" CssClass="field-validation-error"
                                        ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                        ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                        Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>

        <%--Botones--%>
        <div class="control-group" style="float: left; padding-top:50px;padding-bottom:50px;">
            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" ValidationGroup="caducar" OnClick="btnBuscar_OnClick" Text="Buscar"></asp:Button>
            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClientClick="LimpiarFormulario();">
            <i class="icon-refresh"></i>
            <span class="text">Limpiar</span>
            </asp:LinkButton>            
        </div>

   <%--Encabezado--%>
    <div class="accordion-group widget-box">
        <div class="accordion-heading">
            <a id="btnUpDownNot" data-parent="#collapse-group" href="#collapseMovimientos"
                data-toggle="collapse" onclick="btnUpDownNot_click(this)">

                <div class="widget-title">
                    <span class="icon"><i class="icon-th-list"></i></span>
                    <h5>Movimientos</h5>
                </div>
            </a>

        </div>
         <%--Grilla--%>
        <div class="accordion-body collapse in" id="collapseMovimientos">
            <div class="widget-content">
                <asp:Panel runat="server" ID="pnlResultadoBuscar">
                    <asp:UpdatePanel ID="updPnlMovimientos" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <asp:HiddenField ID="hfBuscarMovimientos" runat="server" />
                            <asp:GridView ID="grdBuscarMovimientos"
                                runat="server"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                CssClass="table table-bordered table-striped table-hover with-check"
                                DataKeyNames="Mov_Id, usuario"
                                ItemType="SGI.Model.clsItemGrillaBuscarMovimientos"
                                OnRowDataBound="grdBuscarMovimientos_RowDataBound">
                                <Columns>
                                    <asp:BoundField Visible="false" DataField="Mov_id" HeaderText="ID" />
                                    <asp:BoundField Visible="false" DataField="Mov_Usuario" HeaderText="Username" />
                                    <asp:BoundField DataField="Mov_Username" HeaderText="Usuario" ItemStyle-Width="300px" />
                                    <asp:BoundField Visible="false" DataField="Mov_TipoMov" HeaderText="Tipo Movimiento" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Mov_FechaLog" HeaderText="Fecha de Log" DataFormatString="{0:d}" ItemStyle-Width="70px" />
                                    <asp:BoundField DataField="Mov_DescripcionMov" HeaderText="Tipo de Movimiento" ItemStyle-Width="300px" />
                                    <asp:BoundField DataField="Mov_Observacion" HeaderText="Observacion Solicitante" ItemStyle-Width="300px" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                        No se encontraron registros.
                                    </asp:Panel>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
        </div>
    </div>

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
    <script>

        $(document).ready(function () {
            inicializar_controles();
        });

        function LimpiarFormulario() {
            document.getElementById("MainContent_usuario").value = "";
            document.getElementById("MainContent_txtFechaDesde").value = "";
            document.getElementById("MainContent_ddlTipoMov").value = "";
            document.getElementById("MainContent_funcionalidad").value = "";
            document.getElementById("MainContent_ddlObservacionSolicitante").value = "";
            document.getElementById("MainContent_solicitud").value = "";
            document.getElementById("MainContent_txtFechaHasta").value = "";
        }


        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function inicializar_fecha() {
            var fechaDesde = $('#<%=txtFechaDesde.ClientID%>');
        var es_readonly = $(fechaDesde).attr("readonly");
        if (!($(fechaDesde).is('[disabled]') || $(fechaDesde).is('[readonly]'))) {
            $(fechaDesde).datepicker(
                {
                    maxDate: "0",
                    closeText: 'Cerrar',
                    prevText: '&#x3c;Ant',
                    nextText: 'Sig&#x3e;',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 0,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''

                }
            );
        }
        var fechaHasta = $('#<%=txtFechaHasta.ClientID%>');
            var es_readonly = $(fechaHasta).attr("readonly");
            if (!($(fechaHasta).is('[disabled]') || $(fechaHasta).is('[readonly]'))) {
                $(fechaHasta).datepicker(
                    {
                        maxDate: "0",
                        closeText: 'Cerrar',
                        prevText: '&#x3c;Ant',
                        nextText: 'Sig&#x3e;',
                        currentText: 'Hoy',
                        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                            'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                            'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                        weekHeader: 'Sm',
                        dateFormat: 'dd/mm/yy',
                        firstDay: 0,
                        isRTL: false,
                        showMonthAfterYear: false,
                        yearSuffix: ''

                    }
                );
            }
        }


        function inicializar_controles() {
            inicializar_fecha();
        }

        function btnUpDownNot_click(obj) {
            var href_collapse = $(obj).attr("href");
            if ($(href_collapse).attr("id") != undefined) {
                if ($(href_collapse).css("height") == "0px") {
                    $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
                else {
                    $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }
    </script>
</asp:Content>

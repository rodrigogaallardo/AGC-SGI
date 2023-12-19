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
                            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="ddlUsuario"
                                Text="Usuario" class="control-label"></asp:Label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlUsuario" runat="server" Width="150px">
                                </asp:DropDownList>
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

                            <%--Fecha Hasta:--%>
                        <div class="control-group">
                            <label for="txtFechaHasta" class="control-label">Fecha Hasta:</label>
                            <div class="controls">
                                <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                <div class="req">
                                    <asp:RegularExpressionValidator
                                        ID="rev_txtFechaHasta" runat="server"
                                        ValidationGroup="buscar"
                                        ControlToValidate="txtFechaHasta" CssClass="field-validation-error"
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
                        <%--Observacion Solicitante--%>
                        <div class="control-group">
                            <asp:Label ID="lblObservacionSolicitante" runat="server" AssociatedControlID="ddlObservacionSolicitante"
                                Text="Observacion Solicitante" class="control-label"></asp:Label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlObservacionSolicitante" runat="server" Width="150px">
                                    <asp:ListItem Text="Todos" Value="Todos" />
                                    <asp:ListItem Text="Si" Value="Si" />
                                    <asp:ListItem Text="No" Value="No" />
                                </asp:DropDownList>
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
            <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClick="btnLimpiar_OnClick">
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
                    <asp:UpdatePanel ID="updPnlMovimientos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfBuscarMovimientos" runat="server" />
                            <asp:GridView ID="grdBuscarMovimientos"
                                runat="server"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                CssClass="table table-bordered table-striped table-hover with-check"
                                DataKeyNames="id, usuario"
                                AllowPaging="true"
                                PageSize ="10"
                                OnDataBound="grd_DataBound"
                                ItemType="SGI.Model.clsItemGrillaBuscarMovimientos">
                                <Columns>
                                    <asp:BoundField Visible="false" DataField="id" HeaderText="ID" />
                                    <asp:BoundField DataField="usuario" HeaderText="Nombre de Usuario" ItemStyle-Width="70px" />
                                    <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha de Ingreso" DataFormatString="{0:d}" ItemStyle-Width="20px" />
                                    <asp:BoundField DataField="URL" HeaderText="URL" ItemStyle-Width="80 px" />
                                    <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo de Movimiento" ItemStyle-Width="10px" />
                                    <asp:BoundField DataField="Observacion_Solicitante" HeaderText="Observacion Solicitante" ItemStyle-Width="50px" />
                                </Columns> 
                                 <PagerTemplate>
                                    <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                        <div style="display:inline-table">
                                            <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlMovimientos" runat="server"
                                                DisplayAfter="0">
                                                <ProgressTemplate>
                                                    <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>

                                        <asp:LinkButton ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click" CssClass="btn" />
                                        <asp:LinkButton ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                        <asp:LinkButton ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click" CssClass="btn" />

                                    </asp:Panel>
                            </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
        </div>
    </div>


  
    <script>

        $(document).ready(function () {
            inicializar_controles();
        });


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

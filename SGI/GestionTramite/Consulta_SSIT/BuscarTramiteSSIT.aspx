<%@  Title="Búsqueda del trámite" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarTramiteSSIT.aspx.cs"
    Inherits="SGI.BuscarTramiteSSIT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Styles.Render("~/bundles/jqueryCustomCss") %>

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="Hiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_solicitud" runat="server" />
        <asp:HiddenField ID="hid_id_tipoTramite" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
        <asp:HiddenField ID="hid_editar" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

    <script type="text/javascript">

        //TOMAS -- visibilizar el filtro del acordeon
        $(document).ready(function () {
            var myValue = $('#ultBTN input[type=hidden]').val();
            if (myValue == '') {
                inicializar_controles0();
            }
            {
                inicializar_controles0();
            };
        });

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function inicializar_controles0() {

            //debugger;
            //inicializar tootip del popOver
            inicializar_fechas();
            camposAutonumericos();
            inicializar_autocomplete();
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
        }

        function inicializar_controles1() {

            //debugger;
            //inicializar tootip del popOver
            inicializar_fechas();
            camposAutonumericos();
            inicializar_autocomplete();
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });

        switchear_buscar_ubicacion(1);
        $("div.accordion-body").collapse().show();
        //$('div.accordion-body').collapse().slideDown();
        //$('div.accordion-body').collapse().slideUp();

        if ($("#<%: hid_estados_selected.ClientID %>").val().length > 0) {
            tags_selecionados = $("#<%: hid_estados_selected.ClientID %>").val().split(",");
        }

        $("#<%: ddlEstado.ClientID %>").select2({
            tags: true,
            tokenSeparators: [","],
            placeholder: "Ingrese las etiquetas de búsqueda",
            language: "es",
            data: tags_selecionados
        });
        $("#<%: ddlEstado.ClientID %>").val(tags_selecionados);
        $("#<%: ddlEstado.ClientID %>").trigger("change.select2");
    }

    function camposAutonumericos() {
        $('#<%=txtNroSolicitud.ClientID%>').autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
    }

    function showResultado() {
        $("#box_resultado").show("slow");
    }

    function hideResultado() {
        $("#box_resultado").hide("slow");
    }
    </script>

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar">

        <%-- filtros de busqueda--%>
        <div class="">

            <%--buscar tramite --%>
            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- titulo collapsible buscar por tramite --%>
                <div class="accordion-heading">
                    <a id="bt_tramite_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_tramite"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                        <%--            <asp:HiddenField ID="hid_bt_tramite_collapse" runat="server" Value="true"/>
                    <asp:HiddenField ID="hid_bt_tramite_visible" runat="server" Value="false"/>--%>

                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_tramite_tituloControl" runat="server" Text="Trámites"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible buscar por tramite --%>
                <div class="accordion-body collapse in" id="collapse_bt_tramite">
                    <div class="widget-content">

                        <asp:UpdatePanel ID="updPnlFiltroBuscar_tramite" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>


                                <%--                <div id="myContainer">
                    <asp:HiddenField ID="hdMyControl" runat="server" /></div>--%>

                                <div id="ultBTN">
                                    <asp:HiddenField ID="hdUltBtn" runat="server" />
                                </div>

                                <table>
                                    <tr>

                                        <td style="vertical-align: top">
                                            <div class="form-horizontal" style="width: 50%">
                                                <fieldset>

                                                    <div class="control-group">
                                                        <asp:Label ID="lblNroSolicitud" runat="server" AssociatedControlID="txtNroSolicitud"
                                                            Text="Solicitud:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtNroSolicitud" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblDomicilio" runat="server" AssociatedControlID="txtDomicilio"
                                                            Text="Domicilio:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtDomicilio" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblNroExpediente" runat="server" AssociatedControlID="txtNroExp"
                                                            Text="Nro. Expediente:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtNroExp" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblTipoTramite" runat="server" AssociatedControlID="ddlTipoTramite"
                                                            Text="Tipo de Trámite:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="200px"
                                                                OnSelectedIndexChanged="ddlTipoTramite_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hid_tipotramite_selected" runat="server"></asp:HiddenField>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado"
                                                            Text="Estado del Trámite:" class="control-label"></asp:Label>
                                                        <div class="controls">
                                                            <asp:DropDownList ID="ddlEstado" runat="server" Width="200px" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hid_estados_selected" runat="server"></asp:HiddenField>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </td>
                                    </tr>

                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>

            </div>


        </div>

        <br />

        <asp:UpdatePanel ID="btn_BuscarTramite" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="pull-right">

                    <div class="control-group inline-block">

                        <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="btn_BuscarTramite"
                            runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="../Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                    </div>
                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnBuscar_OnClick">
                    <i class="icon-white icon-search"></i>
                    <span class="text">Buscar</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_OnClick">
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
                    </asp:LinkButton>

                </div>


            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>

    <br />
    <br />

    <div id="box_resultado" class="widget-box" style="display: none;">

        <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <script type="text/javascript">
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);

                </script>
                <div style="margin-left: 10px; margin-right: 10px">

                    <asp:Panel ID="pnlResultadoBuscar" runat="server">

                        <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">

                            <div style="display: inline-block">
                                <h5>Lista de Tr&aacute;mites</h5>
                            </div>
                            <div style="display: inline-block">
                                (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server"></asp:Label></span>
                                )
                            </div>

                        </asp:Panel>

                        <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false"
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            SelectMethod="GetTramites" ItemType="SGI.Model.clsItemConsultaSSIT"
                            AllowPaging="true" AllowSorting="true" PageSize="30" OnPageIndexChanging="grdTramites_PageIndexChanging"
                            OnDataBound="grdTramites_DataBound" OnRowDataBound="grdTramites_RowDataBound">

                            <Columns>
                                <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="id_solicitud">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# Item.url_visorTramite%>'><%# Item.id_solicitud %></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FechaInicio" DataFormatString="{0:d}" HeaderText="Fecha" HeaderStyle-CssClass="text-center" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="TipoTramite" HeaderText="Trámite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="130px" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="text-center min-width-100" ItemStyle-Width="130px"/>
                                <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" HtmlEncodeFormatString="true" />

                            </Columns>

                            <EmptyDataTemplate>
                                <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                                    <p>
                                        No se encontraron trámites con los filtros ingresados.
                                    </p>
                                </asp:Panel>
                            </EmptyDataTemplate>


                            <PagerTemplate>


                                <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                    <div style="display: inline-table">

                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updPnlResultadoBuscar" runat="server"
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
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>




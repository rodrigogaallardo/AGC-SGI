<%@ Page Title="Consulta de Profesionales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmProfesionales.aspx.cs" Inherits="SGI.ABM.AbmProfesionales" %>


<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <%--  <script src="../Scripts/Select2-locales/select2_locale_es.js"></script>
    <script src="../Scripts/Funciones.js" type="text/javascript"></script>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>--%>

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>


    <script type="text/javascript">


        $(document).ready(function () {
            inicializar_controles();
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            //$("#page_content").hide();
            //$("#Loading").show();

            $("#<%: btnCargarDatos.ClientID %>").click();


        });

        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }

        function inicializar_controles() {
            toolTips();
            inicializar_popover();
            inicializar_autocomplete();
        }


        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function inicializar_popover() {

            $('[rel=popover]').popover({
                html: 'true',
                placement: 'right'
            })

        }
        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function LimpiarBusqueda() {
            $("#box_resultado").hide();
        }

        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_resultado").hide("slow");
            $("#box_datos").show("slow");
        }

        function showBusqueda() {
            $("#box_datos").hide("slow");
            $("#box_resultado").show("slow");
            $("#box_busqueda").show("slow");
        }
        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function hideSummary() {

            if ($("[id!='ValSummary'][class*='alert-danger']:visible").length == 0) {
                $("#ValSummary").hide();
            }
        }

        function validarBuscar() {
            var ret = true;
            hideSummary();

            if ($.trim($("#<%: txtNroMatricula.ClientID %>").val()).length == 0 && $.trim($("#<%: txtApellidoyNombre.ClientID %>").val()).length == 0 && $.trim($("#<%: txtCuit.ClientID %>").val()).length == 0) {
                $("#Buscar_CamposReq").css("display", "inline-block");
                ret = false;
            }

            if ($.trim($("#<%: txtApellidoyNombre.ClientID %>").val()).length > 1 && $.trim($("#<%: txtApellidoyNombre.ClientID %>").val()).length < 3) {
                $("#Buscar_NombreyApellidoReq").css("display", "inline-block");
                ret = false;
            }

            var cuitRegex = '/^[0-9]{2}-[0-9]{8}-[0-9]{1}$/';
            if (!cuitRegex.test($.trim($("#<%: txtCuit.ClientID %>").val()))) {
                $("#Buscar_CuitReq").css("display", "inline-block");
                ret = false;
            }

            if (ret) {

            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }

        function validarGuardar() {
            var ret = true;
            hideSummary();
            $("#Val_BotonGuardar").hide();

            if (ret) {
                ocultarBotonesGuardar();
            }
            else {
                $("#ValSummary").css("display", "inline-block");

            }
            return ret;
        }


        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }

        function ConfirmDel() {
            return confirm('¿Esta seguro que desea quitar esta condición?');
        }

        function inicializar_autocomplete() {

        }


    </script>

    <%--ajax cargando ...--%>
    <%--<div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                <td>
                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>--%>

    <%-- <div id="page_content" style="display:none">--%>

    <%--Busqueda de Profesionales--%>

    <div id="box_busqueda">
        <asp:Panel ID="pnlBotonDefault" runat="server" CssClass="widget-box" DefaultButton="btnBuscar">


            <div class="widget-title">
                <span class="icon"><i class="icon-search"></i></span>
                <h5>B&uacute;squeda de Profesionales</h5>
            </div>
            <div class="widget-content">
                <%-- Busqueda1 --%>
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar">
                                <div class="row">
                                    <div class="span5">
                                        <label class="control-label">Nro. de Matrícula:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNroMatricula" runat="server" CssClass="form-control" Style="width: 95%"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="span5">
                                        <label class="control-label">C.U.I.T.:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCuit" runat="server" CssClass="form-control" Style="width: 95%" placeholder="Ej: 20-25006281-9"></asp:TextBox>
                                            <div id="Buscar_CuitReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Nº de CUIT no tiene un formato válido. Ej: 20-25006281-9.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="span5">
                                        <label class="control-label">Apellido y Nombre:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtApellidoyNombre" runat="server" CssClass="form-control" Style="width: 95%"></asp:TextBox>
                                            <div id="Buscar_NombreyApellidoReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Si ingresa el Apellido y Nombre, el mismo debe contener más de 3 caracteres.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="span5">
                                        <label class="control-label">Usuario:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" Style="width: 95%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="span5">
                                        <label class="control-label">Consejo:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlConsejos" runat="server" CssClass="form-control" Style="width: 100%"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="span5">
                                        <label class="control-label">E-mail:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Style="width: 95%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="span5">
                                        <label class="control-label">Perfil:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlPerfiles" runat="server" CssClass="form-control" Style="width: 100%" OnSelectedIndexChanged="ddlPerfiles_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="span5">
                                        <label class="control-label">SubPerfil:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlSubperfiles" runat="server" CssClass="form-control" Style="width: 100%"></asp:DropDownList>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="span5">
                                        <label class="control-label">Dado de Baja:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlDadoBaja" runat="server" CssClass="form-control" Style="width: 100%">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Value="true">Si</asp:ListItem>
                                                <asp:ListItem Value="false">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <label class="control-label">Inhibido:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlinhibido" runat="server" CssClass="form-control" Style="width: 100%">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Value="true">Si</asp:ListItem>
                                                <asp:ListItem Value="false">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="Buscar_CamposReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                    Debe ingresar algún valor en los campos para poder realizar la búsqueda.
                                </div>

                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="btn_BuscarTramite" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="pull-right">

                    <div class="control-group inline-block">

                        <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="btn_BuscarTramite"
                            runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                    </div>
                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" ValidationGroup="validarBuscar" OnClick="btnBuscar_Click">
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


        <br />
        <br />
        <%--para separar los botones de la tabla--%>
        <div id="box_resultado" class="widget-box" style="display: none;">
            <%--Resultados --%>
            <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <script type="text/javascript">
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                        function endRequestHandler() {
                            inicializar_popover();
                        }
                    </script>

                    <div style="overflow: hidden; text-wrap: inherit; margin-left: 10px; margin-right: 10px">
                        <asp:Panel ID="pnlResultadoBuscar" runat="server">

                            <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false">

                                <div style="display: inline-block; margin-left: 10px">
                                    <h5>Listado de Profesionales</h5>
                                </div>
                                <div style="display: inline-block">
                                    <span class="badge">
                                        <asp:Label ID="lblCantidadRegistros" runat="server"></asp:Label></span>
                                </div>
                            </asp:Panel>
                            <div style="margin-left: 10px; margin-right: 10px">
                                <asp:GridView ID="grdResultados"
                                    runat="server"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    CssClass="table table-bordered table-striped table-hover with-check"
                                    AllowPaging="true"
                                    PageSize="30"
                                    SelectMethod="GetProfesionales"
                                    AllowSorting="true"
                                    OnDataBound="grd_DataBound">
                                    <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                                    <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-ForeColor="#337AB7" HeaderText="Consejo y Matrícula" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <label style="color: #337AB7; font-weight: bold; text-align: right">Consejo: </label>
                                                <asp:Label ID="grdlblConsejo" runat="server" Text='<%#  Eval("concejo") %>' /><br />
                                                <label style="color: #337AB7; font-weight: bold; text-align: right">Matrícula: </label>
                                                <asp:Label ID="grdlblMatricula" runat="server" Text='<%# Eval("nro_matricula") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-ForeColor="#337AB7" HeaderText="Apellido, Nombre y C.U.I.T." ItemStyle-Width="240px">
                                            <ItemTemplate>
                                                <asp:Label ID="grdlblApeNom" runat="server" Text='<%# Eval("nombre_apellido")%>' /><br />
                                                <asp:Label ID="grdlblCuit" runat="server" Text='<%# Eval("cuit")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="direccion" HeaderText="Dirección" ItemStyle-Width="120px" SortExpression="direccion" />
                                        <asp:BoundField DataField="perfiles" HeaderStyle-ForeColor="#337AB7" HeaderText="Perfiles (y Subperfiles)" ItemStyle-Width="150px" />
                                        <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-Width="10px" ItemStyle-CssClass="align-center" SortExpression="usuario" />
                                        <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="90px" SortExpression="email" />
                                        <asp:BoundField DataField="DadoBaja" HeaderStyle-ForeColor="#337AB7" HeaderText="Dado de Baja" ItemStyle-Width="40px" ItemStyle-CssClass="align-center" />
                                        <asp:BoundField DataField="Inhibido" HeaderStyle-ForeColor="#337AB7" HeaderText="Inhibido" ItemStyle-Width="40px" ItemStyle-CssClass="align-center" />
                                        <asp:TemplateField HeaderText="Acción" HeaderStyle-ForeColor="#337AB7" ItemStyle-CssClass="text-center" ItemStyle-Width="3px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnVer" runat="server" ToolTip="Ver" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%# Eval("id_profesional") %>' OnClick="btnVer_Click">
                                                <span class="icon" > <i class="imoon-eye-open"  style="font-size:medium;color:#337AB7"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div>
                                            <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                            <span class="mleft20">No se encontraron registros.</span>
                                        </div>
                                    </EmptyDataTemplate>

                                    <PagerTemplate>
                                        <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                            <div style="display: inline-table">
                                                <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
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
                                <asp:LinkButton ID="btnExportarCabecera" runat="server" Style="margin-bottom: 10px; margin-left: 10px" CssClass="btn btn-inverse" Visible="false" OnClick="btnExportarProfesionalesAExcel_Click" OnClientClick="return showfrmExportarExcel();">
                                                <i class="imoon-white imoon-arrow-down"></i>
                                                <span class="text" Style="color:#fff" >Exportar Listado</span>
                                </asp:LinkButton>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--ABM --%>
    <div id="box_datos" class="widget-box" style="display: none">
        <div class="widget-title">
            <span class="icon"><i class="imoon imoon-user-md"></i></span>
            <h5>Datos del Profesional </h5>
        </div>
        <div class="widget-content">
            <div class="form-horizontal">
                <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_id_profesionalReq" runat="server" />
                        <asp:Panel ID="pnlDatos" runat="server">
                            <div class="row">
                                <div class="span10">
                                    <label class="control-label">Consejo:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtConsejoReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="span10">
                                    <label class="control-label">Perfiles:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtPerfilReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Nro. Matrícula:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNroMatriculaReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="span5">
                                    <label class="control-label">Tipo y Nº de Doc.:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTipoYNroDocReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Apellido:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtApellidoReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="span5">
                                    <label class="control-label">Nombres:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNombreReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">C.U.I.T.:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCuitReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="span5">
                                    <label class="control-label">Nº Ingresos Brutos:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNroIngresosBrutosReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Calle:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCalleReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="span5">
                                    <label class="control-label">Nro de Puerta:</label>
                                    <div class="controls">
                                        <div class="span5" style="margin-left: 0px">
                                            <asp:TextBox ID="txtNroReq" runat="server" Width="14%" MaxLength="30" Enabled="false"></asp:TextBox>
                                            <label class="pleft5 pright5">Piso:</label>
                                            <asp:TextBox ID="txtPisoReq" runat="server" Width="10%" MaxLength="30" Enabled="false"></asp:TextBox>
                                            <label class="pleft5 pright5">Depto:</label>
                                            <asp:TextBox ID="txtDeptoReq" runat="server" Width="10%" MaxLength="30" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Provincia:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtProvinciaReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="span5">
                                    <label class="control-label">Localidad:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtLocalidadReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Email:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtEmailReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Teléfono:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTelefonoReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="span5">
                                    <label class="control-label">SMS:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSmsReq" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="span5">
                                    <label class="control-label">Matrícula gasista:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtMatriculaMetrogas" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="span5">
                                    <label class="control-label">Categoría:</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCategoriaMetrogas" runat="server" Style="width: 100%" MaxLength="100" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                                <ContentTemplate>
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <div id="ValSummary" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Revise las validaciones en pantalla.
                                            </div>
                                        </div>
                                        <div id="pnlBotonesGuardar" class="control-groupp">
                                            <div class="controls">
                                                <asp:LinkButton ID="btnNuevaBusqueda" runat="server" CssClass="btn btn-inverse" OnClientClick="return showBusqueda();">
                                                            <span class="text">Nueva Búsqueda</span>
                                                </asp:LinkButton>
                                                <asp:HyperLink ID="btnExportarInformes" runat="server" Target="_blank" CssClass="btn btn-success" Height="20" title="Exportar"
                                                    Text="Exportar"
                                                    Width="150px" Visible="true">
                                                </asp:HyperLink>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                    Guardando...
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--</div>--%>

    <!-- /.modal -->
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
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
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
    <!-- /.modal -->

    <%--Exportación a Excel--%>
    <div id="frmExportarExcel" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updExportaExcel" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h4 class="modal-title">Exportar a Excel</h4>
                        </div>
                        <div class="modal-body">


                            <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" Enabled="false">
                            </asp:Timer>

                            <asp:Panel ID="pnlExportandoExcel" runat="server">
                                <div class="row text-center">
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                                </div>
                                <div class="row text-center">
                                    <h2>
                                        <asp:Label ID="lblRegistrosExportados" runat="server"></asp:Label></h2>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDescargarExcel" runat="server" Style="display: none">
                                <div class="row text-center">
                                    <asp:HyperLink ID="btnDescargarExcel" runat="server" Target="_blank" CssClass="btn btn-link">
                                        <i class="imoon imoon-file-excel color-green fs48"></i>
                                        <br />
                                        <span class="text">Descargar archivo</span>
                                    </asp:HyperLink>
                                </div>
                            </asp:Panel>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrarExportacion" runat="server" CssClass="btn btn-default" OnClick="btnCerrarExportacion_Click" Text="Cerrar" Visible="false" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <script>
        function showfrmExportarExcel() {
            $("#frmExportarExcel").modal({
                backdrop: "static",
                show: true
            });
            return true;
        }
        function hidefrmExportarExcel() {
            $("#frmExportarExcel").modal("hide");
            return false;
        }
    </script>


</asp:Content>



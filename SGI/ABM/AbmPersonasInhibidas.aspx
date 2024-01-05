<%@ Page Title="Consulta de Personas Inhibidas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmPersonasInhibidas.aspx.cs" Inherits="SGI.ABM.AbmPersonasInhibidas" %>


<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <script src="../Scripts/Datepicker_es.js"></script>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    <%: Styles.Render("~/Content/themes/base/css") %>
    <script type="text/javascript">

        $(document).ready(function () {
            $(".btnEditar").tooltip({ 'delay': { show: 500, hide: 0 } });
            $(".lnkEliminarCondicionReq").tooltip({ 'delay': { show: 500, hide: 0 } });

            $("#page_content").hide();
            $("#Loading").show();


            $("#<%: btnCargarDatos.ClientID %>").click();


        });


        function init_Js_updDatos() {

            var fechaVencimientoReq = $('#<%=txtFechaVencimientoReq.ClientID%>');
            var es_readonly = $(fechaVencimientoReq).attr("readonly");
            $("#<%: txtFechaVencimientoReq.ClientID %>").datepicker({
                minDate: "-100Y",
                maxDate: "+100Y",
                yearRange: "-100:+50",
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function () {
                    //$("#Req_FechaDesde").hide();
                    //$("#Val_Formato_FechaDesde").hide();
                    //$("#Val_FechaDesdeMenor").hide();
                }
            });
        }

        function init_Js_updpnlBuscar() {

            $("#<%: txtNroDocumento.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999' });
        }

        function Js_autonum() {
            $("#<%: txtNroDocumentoReq.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999' });
            $("#<%: txtNroOperadorReq.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999999' });
        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }

        function finalizarCarga() {
            $("#Loading").hide();
            $("#page_content").show();
            return false;
        }

        function showDatos() {
            $("#box_busqueda").hide("slow");
            $("#box_datos").show("slow");
        }

        function showResultado() {
            $("#box_resultado").show("slow");
        }
        function showBusqueda() {
            $("#box_datos").hide("slow");
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

            if ($.trim($("#<%: txtNroDocumento.ClientID %>").val()).length == 0 && $.trim($("#<%: txtApellidoyNombre.ClientID %>").val()).length == 0 && $.trim($("#<%: txtCuit.ClientID %>").val()).length == 0) {
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
            $("#Req_txtTipoDocumentoReq").hide();
            $("#Req_txtNroDocumentoReq").hide();
            $("#Req_txtCuitReq").hide();
            $("#Req_txtNroOrdenReq").hide();
            $("#Req_txtNombreyApellidoReq").hide();
            $("#Req_txtEstadoReq").hide();
            $("#Invalido_txtCuitReq").hide();
            $("#Req_txtTipoPersonaReq").hide();



            if ($.trim($("#<%: txtTipoPersona.ClientID %>").val()).length == 0) {
                $("#Req_txtTipoPersonaReq").css("display", "inline-block");
                ret = false;
            } else {
                if ($("#<%: txtTipoPersona.ClientID %>").val() == "1") {
                    if ($.trim($("#<%: txtTipoDocumentoReq.ClientID %>").val()).length == 0) {
                        $("#Req_txtTipoDocumentoReq").css("display", "inline-block");
                        ret = false;
                    }
                    if ($.trim($("#<%: txtNroDocumentoReq.ClientID %>").val()).length == 0) {
                        $("#Req_txtNroDocumentoReq").css("display", "inline-block");
                        ret = false;
                    }
                }
            }

            if ($.trim($("#<%: txtCuitReq.ClientID %>").val()).length == 0) {
                $("#Req_txtCuitReq").css("display", "inline-block");
                ret = false;
            } else {
                var m = $.trim($("#<%: txtCuitReq.ClientID %>").val());
                var expreg = /^([0-9]{2}-[0-9]{8}-[0-9]{1})$/;
                if (!expreg.test(m)) {
                    $("#Invalido_txtCuitReq").css("display", "inline-block");
                    ret = false;
                }
            }


            if ($.trim($("#<%: txtNroOrdenReq.ClientID %>").val()).length == 0) {
                $("#Req_txtNroOrdenReq").css("display", "inline-block");
                ret = false;
            }
            if ($.trim($("#<%: txtNombreyApellidoReq.ClientID %>").val()).length == 0) {
                $("#Req_txtNombreyApellidoReq").css("display", "inline-block");
                ret = false;
            }


            if ($.trim($("#<%: txtEstadoReq.ClientID %>").val()).length == 0) {
                $("#Req_txtEstadoReq").css("display", "inline-block");
                ret = false;
            }


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

    </script>

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
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
    </div>

    <div id="page_content" style="display: none">
        <%--Busqueda de Profesionales--%>
        <div id="box_busqueda">

            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar">
                <div class="widget-title">
                    <span class="icon"><i class="icon-search"></i></span>
                    <h5>B&uacute;squeda de Personas Inhibidas</h5>
                </div>
                <div class="widget-content">
                    <div>
                        <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">

                                    <div class="control-group">
                                        <label class="control-label">Nombre / Razón social:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtApellidoyNombre" runat="server" CssClass="input-xlarge" Width="300px"></asp:TextBox>
                                            <div id="Buscar_NombreyApellidoReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Si ingresa el Apellido y Nombre, el mismo debe contener más de 3 caracteres.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Tipo y Nº de Documento:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="130px" />
                                            <asp:TextBox ID="txtNroDocumento" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">C.U.I.T.:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCuit" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
                                            <div>(formato 20-12345678-0)</div>
                                            <div id="Buscar_CuitReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Nº de CUIT no tiene un formato válido. Ej: 20-25006281-9.
                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="pull-right">

                        <div id="Buscar_CamposReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                            Debe ingresar algún valor en los campos para poder realizar la búsqueda.
                        </div>

                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>

                        <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" ValidationGroup="buscar">
                                            <i class="icon-white icon-search"></i>
                                            <span class="text">Buscar</span>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnNuevaPersonaInhibida" runat="server" CssClass="btn" OnClick="btnNueva_Click">
                                            <i class="icon-plus"></i>
                                            <span class="text">Nueva Persona Inhibida</span>
                        </asp:LinkButton>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <br />
            <br />

            <%-- Muestra Resultados--%>
            <div id="box_resultado" style="display: none;">
                <div class="widget-box">
                    <%--Resultados --%>
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-horizontal" style="margin-left: 15px; margin-right: 15px">
                                <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                                    <div>
                                        <h5>Resultado de la b&uacute;squeda</h5>
                                    </div>
                                    <div class="text-right">
                                        <span class="badge">Cantidad de registros:
                                        <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                    </div>
                                </asp:Panel>

                                <asp:GridView ID="grdResultados" runat="server"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    GridLines="None"
                                    CssClass="table table-bordered table-striped table-hover with-check"
                                    ItemType="SGI.Model.clsItemGrillaPersonasInhibidas"
                                    PageSize="30"
                                    AllowSorting="true"
                                    OnPageIndexChanging="grd_PageIndexChanging"
                                    OnDataBound="grd_DataBound"
                                    OnRowDataBound="grdResultados_RowDataBound">
                                    <Columns>
                                        <%--Ver --%>

                                        <asp:BoundField DataField="documento" HeaderText="Tipo Y Nro de Documento" ItemStyle-Width="150px" />
                                        <asp:BoundField DataField="nomape_personainhibida" HeaderText="Apellido y Nombre / Razon social" />
                                        <asp:BoundField DataField="fecharegistro_personainhibida" HeaderText="Fecha Registro" ItemStyle-Width="80px" />
                                        <asp:BoundField DataField="fechavencimiento_personainhibida" HeaderText="Fecha Vencimineto" ItemStyle-Width="80px" />
                                        <asp:TemplateField HeaderText="Estado" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lnkestado" runat="server" Text='<%# Eval("estado").ToString() == "0" ? "Dado de baja" : Eval("estado").ToString() == "1" ? "Activo" : "NUEVO" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="fechabaja_personainhibida" HeaderText="Fecha Baja" ItemStyle-Width="80px" />

                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-center" ItemStyle-Width="3px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" data-toggle="tooltip" CssClass="link-local"
                                                    CommandArgument='<%# Item.id_personainhibida %>' OnClick="btnEditar_Click">
                                                <i class="icon-edit" style="transform: scale(1.2);margin-right:5px;margin-left:5px"></i>
                                                </asp:LinkButton>

                                                <asp:HyperLink ID="lnkPDF" runat="server" CssClass="" ToolTip="Imprimir" data-toggle="tooltip" Target="_blank"
                                                    CommandArgument='<%# Item.id_personainhibida %>'>
                                                <span class="icon" style="color:#337AB7"><i class="icon-file"></i></span>
                                                </asp:HyperLink>
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
                                <br />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <%--ABM --%>
        <div id="box_datos" class="widget-box" style="display: none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos de la Persona inhibida</h5>
            </div>
            <div class="widget-content">
                <div>
                    <div class="col-sm-12 col-md-12">
                        <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hid_id_personainhibidaReq" runat="server" />
                                <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                                    <div class="control-group">
                                        <label class="control-label">Tipo de Persona:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="txtTipoPersona" runat="server" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="txtTipoPersona_SelectedIndexChanged"></asp:DropDownList>
                                            <div id="Req_txtTipoPersonaReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Tipo de Persona.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Tipo de Documento:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="txtTipoDocumentoReq" runat="server" Width="250px"></asp:DropDownList>
                                            <div id="Req_txtTipoDocumentoReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Tipo de Documento.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Nro. Documento:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNroDocumentoReq" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                            <div id="Req_txtNroDocumentoReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Nro de Documento.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">C.U.I.T.:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCuitReq" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                            <div>(formato 20-12345678-0)</div>
                                            <div id="Invalido_txtCuitReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Nº de CUIT no tiene un formato válido. Ej: 20-25006281-9.
                                            </div>
                                            <div id="Req_txtCuitReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el CUIT.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Nro. Orden:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="txtNroOrdenReq" runat="server" Width="200px">
                                                <asp:ListItem Text=""></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Una sentencia"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Más de una sentencia"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div id="Req_txtNroOrdenReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Nro de Orden.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Nombre y Apellido / Razon social:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNombreyApellidoReq" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                            <div id="Req_txtNombreyApellidoReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar Nombre y Apellido / Razon social.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Fecha de Registro:</label>
                                        <div class="controls">
                                            <asp:Label ID="txtFechaRegistroReq" runat="server" MaxLength="10" Width="100px"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Fecha de Vencimiento:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaVencimientoReq" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaVencimiento" runat="server"
                                                    ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaVencimientoReq" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Autos:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAutosReq" runat="server" Width="350px" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Juzgado:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtJuzgadoReq" runat="server" Width="350px" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Secretaria:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSecretariaReq" runat="server" Width="350px" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Estado:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="txtEstadoReq" runat="server" Width="200px">
                                                <asp:ListItem Text=""></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Dado de baja"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div id="Req_txtEstadoReq" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el Estado.
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Fecha de Baja:</label>
                                        <div class="controls">
                                            <asp:Label ID="txtFechaBajaReq" runat="server" MaxLength="10" Width="100px"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Nro. operador:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNroOperadorReq" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Observaciones:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtObservacionesReq" runat="server" Width="350px" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Motivo de Levantamiento:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtMotivo" runat="server" Width="350px" MaxLength="300" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Observaciones del Solicitante:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtObservacionesSolicitante" runat="server" Width="350px" TextMode="MultiLine" Height="50px"></asp:TextBox>
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
                                                <div id="pnlBotonesGuardar" class="control-group">
                                                    <div class="controls">
                                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-inverse" OnClick="btnGuardar_Click" OnClientClick="return validarGuardar();">
                                                            <i class="imoon-white imoon-save"></i>
                                                            <span class="text">Guardar</span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClientClick="return showBusqueda();">
                                                            <i class="imoon-blocked"></i>
                                                            <span class="text">Cancelar</span>
                                                        </asp:LinkButton>
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
        </div>
    </div>

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

</asp:Content>

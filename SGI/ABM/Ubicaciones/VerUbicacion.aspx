<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerUbicacion.aspx.cs" Inherits="SGI.ABM.Ubicaciones.VerUbicacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js") %>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js") %>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css") %>
    <%: Styles.Render("~/Content/themes/base/css") %>

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
        <div id="box_datos">
            <div class="col-sm-12 col-md-12">
                <asp:UpdatePanel ID="updDatos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hid_id_ubicacion" runat="server" />
                        <asp:HiddenField ID="hid_id_tipo_ubicacion" runat="server" />
                        <asp:Button ID="btnCargarDatos" runat="server" Style="display: none" OnClick="btnCargarDatos_Click" />
                        <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                            <div class="widget-box">
                                <div class="widget-title">
                                    <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                    <h5>Ver ubicación</h5>
                                </div>
                                <div class="widget-content">
                                    <div class="form-horizontal">
                                        <div>
                                            <div class="control-group">
                                                <label class="control-label">Seccion</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtSeccion" runat="server" CssClass="form-control" Width="150px" MaxLength="10" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="control-group">
                                                <label class="control-label">Manzana:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtManzana" runat="server" CssClass="form-control" Width="150px" MaxLength="10" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="control-group">
                                                <label class="control-label">Parcela:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtParcela" runat="server" CssClass="form-control" Width="150px" MaxLength="10" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="control-group">
                                                <label class="control-label">Nro. de Partida Matriz:</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtNroPartida" runat="server" CssClass="form-control" Width="150px" MaxLength="10" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-horizontal">
                                        <div class="control-group">
                                            <label for="ddlbiTipoUbicacion" class="control-label">Tipo de Ubicación:</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlbiTipoUbicacion" runat="server" Width="350px" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label for="ddlUbiSubTipoUbicacion" class="control-label">Subtipo de Ubicación:</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddlUbiSubTipoUbicacion" runat="server" Width="350px" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Entidad Gubernamental:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="chbEntidadGubernamental" runat="server" Checked="false" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Edificio Protegido:</label>
                                        <div class="controls">
                                            <asp:CheckBox ID="chbEdificioProtegido" runat="server" Checked="false" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">¿Dada de baja?:</label>
                                        <div class="controls mtop5">
                                            <asp:RadioButton ID="rbtnBajaSi" runat="server" Text="Si" GroupName="BajaUbicacion" Enabled="false" />
                                            <asp:RadioButton ID="rbtnBajaNo" runat="server" Text="No" GroupName="BajaUbicacion" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <%-- Ubicaciones --%>
                                <div class="widget-box">
                                    <div class="widget-title">
                                        <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                        <h5>Calles y puertas</h5>
                                    </div>
                                    <div class="widget-content">
                                        <div class="control-group pleft20 ptop10 pright25">
                                            <asp:UpdatePanel ID="updUbicaciones" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdUbicaciones" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField DataField="calles" HeaderText="Calle" />
                                                            <asp:BoundField DataField="nroPuerta" HeaderText="Nro. Puerta" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="pad10">
                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">No se encontraron Ubicaciones.</span>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <br />
                                        </div>
                                    </div>
                                </div>
                                <%-- Fin Ubicaciones --%>
                                <%-- Mixturas --%>
                                <div class="widget-box">
                                    <div class="widget-title">
                                        <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                        <h5>Mixturas</h5>
                                    </div>
                                    <div class="widget-content">
                                        <div class="control-group pleft20 ptop10 pright25">
                                            <asp:UpdatePanel ID="updMixturas" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdMixturas" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField DataField="mix" HeaderText="Mixtura" />
                                                            <asp:BoundField DataField="mixDescripcion" HeaderText="Descripción" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="pad10">
                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">No se encontraron Mixturas.</span>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <br />
                                        </div>
                                    </div>
                                </div>
                                <%-- Fin Mixturas --%>
                                <%-- Distritos --%>
                                <div class="widget-box">
                                    <div class="widget-title">
                                        <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                        <h5>Distritos</h5>
                                    </div>
                                    <div class="widget-content">
                                        <div class="control-group pleft20 ptop10 pright25">
                                            <asp:UpdatePanel ID="updDistritos" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdDistritos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mtop5" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundField DataField="grupoDistrito" HeaderText="Grupo Distrito" />
                                                            <asp:BoundField DataField="distrito" HeaderText="Distrito" />
                                                            <asp:BoundField DataField="zonas" HeaderText="Zonas" />
                                                            <asp:BoundField DataField="subzonas" HeaderText="Sub Zonas" />
                                                            <asp:BoundField DataField="IdDistrito" HeaderText="IdDistrito" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="IdZona" HeaderText="IdZona" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="IdSubZona" HeaderText="IdSubZona" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <div class="pad10">
                                                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                                                <span class="mleft20">No se encontraron Distritos.</span>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <br />
                                        </div>
                                    </div>
                                    <%-- FIn Distritos --%>
                                    <%-- Observaciones --%>
                                    <div class="widget-box">
                                        <div class="widget-title">
                                            <span class="icon"><i class="imoon imoon-user-md"></i></span>
                                            <h5>Observaciones</h5>
                                        </div>
                                        <div class="widget-content">
                                            <fieldset>
                                                <div class="form-horizontal">
                                                    <div class="control-group">
                                                        <label class="control-label">Observaciones:</label>
                                                        <div class="controls">
                                                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" Columns="12" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <%-- Fin Observaciones --%>
                                    <div class="widget-box">
                                        <div class="widget-content">
                                            <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-horizontal">
                                                        <div class="control-group">
                                                            <div id="Val_PartidaNueva" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                                Revise las validaciones en pantalla.
                                                            </div>
                                                        </div>
                                                        <div id="pnlBotonesVolver" class="control-group">
                                                            <div class="controls">
                                                                <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-default" OnClick="btnVolver_Click">
                                                            <i class="imoon imoon-blocked"></i>
                                                            <span class="text">Volver</span>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            init_Js_updpnlBuscar();
            $("#page_content").hide();
            $("#Loading").show();
            $("#filtros").show("slow");
            $("#<%: btnCargarDatos.ClientID %>").click();

            init_Js_updDatos();
            init_Js_updpnlBuscar();
        });

        function init_Js_updDatos() {

            $("#<%: txtNroPartida.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '9999999' });
                $("#<%: txtSeccion.ClientID %>").autoNumeric({ aSep: '', mDec: '0', vMax: '99999' });


                $("#<%: txtSeccion.ClientID %>").on("keyup", function () {
                    $("#Req_Seccion").hide();
                    hideSummary();
                });
            }

            function finalizarCarga() {
                $("#Loading").hide();
                $("#page_content").show();
                return false;
            }
    </script>
</asp:Content>

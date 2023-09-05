<%@ Page Title="Reasignar Tarea" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Reasignar_Tarea.aspx.cs"
    Inherits="SGI.Reasignar_Tarea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
                sticky: false
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>


    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server">

        <asp:UpdatePanel ID="updPnlEquipo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <table border="0" style="vertical-align: middle; border-collapse: separate; border-spacing: 5px">
                    <tr>
                        <td style="width: 80px">
                            <asp:Label ID="lblasignar" runat="server">Equipo:</asp:Label>
                        </td>
                        <td>
                            <asp:HiddenField ID="hid_ddlEquipo" runat="server" Value="" />
                            <asp:DropDownList ID="ddlEquipo" runat="server" Width="300px"
                                onblur="return rt_Select2CloseAll(this);">
                            </asp:DropDownList>

                            <div class="pull-right" style="margin-left: 20px">

                                <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="updPnlEquipo"
                                    runat="server" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                            </div>

                            <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_OnClick"
                                Text="Buscar" Style="display: none" />

                            <%--<div class="req">
                            <asp:CompareValidator ID="cv_asignarUsuario" runat="server" ControlToValidate="ddlEquipo"
                                Display="Dynamic" ErrorMessage="Seleccione calificador." ValidationGroup="buscar"
                                SetFocusOnError="True" CssClass="field-validation-error"
                                Type="String" ValueToCompare="0" Operator="NotEqual">
                            </asp:CompareValidator>
                        </div>--%>
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>


    </asp:Panel>

    <br />
    <br />

    <asp:UpdatePanel ID="updPnlResultadoBuscar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                function endRequestHandler() {
                    rt_incializar_dllUsuario();
                    /*rt_ocultar_boton_guardar();*/
                }
            </script>

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

                <%-- SortExpression="direccion"
                      
                    AllowPaging="true" PageSize="30" 
                   SelectMethod="GetTramitesBandeja" AllowSorting="true" 
                    OnDataBound="grdBandeja_DataBound" 
                        <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                        <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                    ItemStyle-Width="200px" 
                    <i class="icon-white icon-search"></i>
                --%>
                <asp:GridView ID="grdTramites" runat="server" AutoGenerateColumns="false"
                    GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                    DataKeyNames="id_solicitud" ItemType="SGI.Model.clsItemReasignar">


                    <Columns>

                        <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" SortExpression="id_solicitud">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# Item.url_visorTramite%>'><%# Item.id_solicitud %></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="direccion" HeaderText="Ubicación" SortExpression="direccion"
                            ItemStyle-Width="130px" />

                        <asp:HyperLinkField DataNavigateUrlFormatString="~/GestionTramite/Tareas/{0}?id={1}"
                            DataNavigateUrlFields="formulario_tarea,id_tramitetarea"
                            DataTextField="nombre_tarea" HeaderText="Tarea" SortExpression="nombre_tarea"
                            ItemStyle-Width="90px" />

                        <asp:BoundField DataField="FechaInicio_tramitetarea" HeaderText="Creada el"
                            DataFormatString="{0:d}" ItemStyle-CssClass="align-center" SortExpression="FechaInicio_tramitetarea"
                            ItemStyle-Width="60px" />

                        <asp:BoundField DataField="superficie_total" HeaderText="Superficie"
                            ItemStyle-CssClass="align-center" SortExpression="superficie_total"
                            ItemStyle-Width="60px" />

                        <asp:BoundField DataField="dias_transcurridos" HeaderText="Días" HeaderStyle-Wrap="true"
                            HeaderStyle-Width="15px" ItemStyle-CssClass="align-center" SortExpression="Dias_Transcurridos" />

                        <asp:TemplateField HeaderText="Usuario" ItemStyle-Width="40px" ItemStyle-CssClass="align-center">
                            <ItemTemplate>

                                <asp:UpdatePanel ID="updPnlReasignarUsuario" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <div class="pull-left">
                                            <asp:Label ID="lblUsuarioAsignado" runat="server" Text='<%# Eval("UsuarioAsignado_tramitetarea_username") %>'></asp:Label>
                                        </div>

                                        <div class="pull-left">
                                            <asp:DropDownList ID="ddlUsuario" runat="server" 
                                                OnSelectedIndexChanged="ddlUsuario_SelectedIndexChanged" 
                                                Width="160px" Style="display: none"></asp:DropDownList>
                                        </div>

                                        <div class="pull-center">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn" OnCommand="btnEdit_Command"
                                                CommandArgument='<%# Eval("id_tramitetarea") %>'> 
                                                <span class="text">Editar</span>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="pull-right">
                                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn" 
                                                OnCommand="btnCancel_Command"
                                                Style="display: none"
                                                CommandArgument='<%# Eval("id_tramitetarea") %>'> 
                                                <span class="text">Cancelar</span>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="pull-right">
                                            <asp:LinkButton ID="btnGuadarUsuario" runat="server" CssClass="btn"
                                                OnCommand="btnGuadarUsuario_Command" Style="display: none"
                                                CommandArgument='<%# Eval("id_tramitetarea") %>'> 
                                                <span class="text">Guadar</span>
                                            </asp:LinkButton>
                                        </div>


                                        <asp:Label ID="lbl_usuarioAsignado" runat="server" Style="display: none"
                                            Text='<%# Eval("UsuarioAsignado_tramitetarea") %>'></asp:Label>

                                        <asp:Label ID="lbl_id_tramitetarea" runat="server" Style="display: none"
                                            Text='<%# Eval("id_tramitetarea") %>'></asp:Label>


                                    </ContentTemplate>
                                </asp:UpdatePanel>


                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>
                        <asp:Panel ID="pnlNotDataFound" runat="server" CssClass="GrayText-1 ptop10">
                            <p>
                                No se encontraron trámites para el usuario indicado.
                            </p>
                        </asp:Panel>
                    </EmptyDataTemplate>


                </asp:GridView>



            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>




    <script type="text/javascript">

        $(document).ready(function () {

            $('select').select2();
            rt_incializar_dllEquipo();
            rt_incializar_dllUsuario();
        });

        function rt_incializar_dllEquipo() {

            $("#<%=ddlEquipo.ClientID%>").select2({
            allowClear: true,
            placeholder: "Seleccione usuario"
            //formatSelection: ddlUsuarioAsignar_formatSelection,
            //formatResult: ddlUsuarioAsignar_formatResult
        });

        $("#<%=ddlEquipo.ClientID%>").on("change", function () {
            debugger;
            var valores = $(this).select2().val();
            $("#<%=hid_ddlEquipo.ClientID%>").val(valores);
            if (valores != "0") {
                $("#<%=pnlResultadoBuscar.ClientID%>").show();
                $("#<%=btnBuscar.ClientID%>").click();
            }
            else {
                $("#<%=pnlResultadoBuscar.ClientID%>").hide();
            }
            return false;
        });
        }

        function rt_ocultar_boton_guardar() {

            $("#<%=grdTramites.ClientID%>").find("[id*='updPnlReasignarUsuario']").each(function () {

                var lbl_usuario = $(this).find("[id*='lbl_usuarioAsignado']");
                var btn_guardar = $(this).find("[id*='btnGuadarUsuario']");
                var userid_actual = lbl_usuario[0].innerHTML;
                var ddl_usuario = $(this).find("[id*='ddlUsuario']");
                var userid_nuevo = $(ddl_usuario).val();

                if (userid_actual != userid_nuevo) {
                    $(btn_guardar).show();
                }
                else {
                    $(btn_guardar).hide();
                }


            });

        }

        function rt_incializar_dllUsuario() {

            $("[id*='ddlUsuario']").each(function () {

                $(this).change(function () {

                    var update_panel = $(this)[0].parentElement.parentElement;
                    var lbl_usuario = $(update_panel).find("[id*='lbl_usuarioAsignado']");
                    var btn_guardar = $(update_panel).find("[id*='btnGuadarUsuario']");
                    var userid_actual = lbl_usuario[0].innerHTML;
                    var userid_nuevo = $(this).prop("value");

                    if (userid_actual != userid_nuevo) {
                        $(btn_guardar).show();
                    }
                    else {
                        $(btn_guardar).hide();
                    }

                });

            });


        }

        function rt_Select2CloseAll() {
            // Cerrar todos los combos al abrir uno
            $("select").each(function (ind, elem) {
                $(elem).select2("close");
            });

            return false;
        }

    </script>

</asp:Content>

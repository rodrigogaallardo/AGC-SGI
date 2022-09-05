<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab_Planos.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tab_Planos" %>
<%: Scripts.Render("~/bundles/autoNumeric") %>
<%: Scripts.Render("~/bundles/fileUpload") %>
<%: Styles.Render("~/bundles/fileUploadCss") %>

<asp:UpdatePanel ID="Hiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_cpadron" runat="server" />
        <asp:HiddenField ID="hid_id_encomienda" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>


<div id="page_content">

    <h3 style="line-height: 20px;">Carga de Planos</h3>

    <div id="box_Planos" class="accordion-group widget-box mtop20">

        <asp:Panel ID="pnlPage" runat="server" CssClass="PageContainer">
            <%--Contenido--%>
            <asp:Panel ID="pnlContenido" runat="server" CssClass="box-contenido" BackColor="White">
                <div>
                    <asp:UpdatePanel ID="updPnlCargarPlano" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hid_tamanio" runat="server" />
                            <asp:HiddenField ID="hid_tamanio_max" runat="server" />
                            <asp:HiddenField ID="hid_requierre_detalle" runat="server" />
                            <asp:HiddenField ID="hid_extension" runat="server" />
                            <script type="text/javascript">
                                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
                                function endRequestHandler() {
                                    init_fileUpload();
                                }
                            </script>

                            <div class="form-horizontal pleft20">
                                <div class="form-group" data-group="controles-accion">
                                    <asp:Label ID="Label1" runat="server"><b>Tipo de Plano:</b></asp:Label>
                                    <asp:DropDownList ID="TipoDropDown" runat="server" Width="200px" AutoPostBack="True"
                                        CssClass="form-control" OnSelectedIndexChanged="TipoDropDown_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblDetalle" runat="server">Detalle Plano:</asp:Label></td>
                                    <asp:TextBox ID="txtDetalle" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <span class="btn btn-inverse fileinput-button" data-group="controles-accion">
                                        <i class="icon-white icon-th"></i>
                                        <span>Cargar Plano</span>
                                        <input id="fileupload" type="file" name="files[]" multiple>
                                    </span>
                                    <label id="val_upfile_txtDetalle" class="error-label" style="display: none">Debe ingresar el detalle.</label>
                                </div>
                                <asp:HiddenField ID="hid_filename_plano_random" runat="server" />
                                <asp:HiddenField ID="hid_filename_plano" runat="server" />
                                <asp:Button ID="btnCargarPlano" runat="server" Text="Cambiar" CssClass="btn btn-inverse" OnClick="btnCargarPlano_Click" Style="display: none" />
                            </div>

                            <!-- The global progress bar -->
                            <div id="progress" class="progress mtop5" style="display: none">
                                <div class="bar bar-success"></div>
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <%--Grilla de planos--%>
                    <asp:UpdatePanel ID="updPnlGrillaPlanos" runat="server">
                        <ContentTemplate>

                            <div class="pleft20" style="width: 900px; overflow: auto">
                                <asp:GridView ID="grdPlanos" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="id_cpadron_plano, id_cpadron" AllowPaging="false" Style="border: none; margin-top: 10px"
                                    GridLines="None" Width="900px"
                                    CellPadding="3">
                                    <HeaderStyle CssClass="grid-header" />
                                    <AlternatingRowStyle BackColor="#efefef" />

                                    <Columns>

                                        <asp:BoundField DataField="detalle" HeaderText="Detalle del Plano" ItemStyle-Width="200px" />

                                        <asp:TemplateField ItemStyle-Height="24px" HeaderText="Descargar">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkDescargarPlano" runat="server" Target="_blank" Style="padding-right: 10px" NavigateUrl='<%#"~/DescargarPlanosCP/" + Eval("id_cpadron_plano") %>'>
                                                <i class="icon-download-alt"></i>
                                                <span class="text"><%# Eval("nombre_archivo")%></span>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CreateDate" HeaderText="Subido el " ItemStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" />

                                        <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEliminar" runat="server"
                                                    CommandArgument='<%# Eval("id_cpadron_plano") %>'
                                                    OnClientClick="javascript:return confirmar_eliminar();"
                                                    OnCommand="lnkEliminar_Command"
                                                    Width="70px" data-group="controles-accion">
                                                    <i class="icon icon-trash"></i> 
                                                    <span class="text">Eliminar</span></a>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="titulo-4">
                                            No hay planos aún...
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                            <div class="pleft20">
                                <asp:Label ID="alertPlanoIncendio" runat="server" CssClass="label-warning" Style="margin-top: 20px;">Se debe cargar el Plano Contra Incendios.</asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="back-panel-gris pleft20" style="margin-top: 20px; padding: 10px 0px 10px 0px; text-align: left">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 80px">CONSIDERACIÓN IMPORTANTE EN UN PLANO DE HABILITACION: 
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* TIENEN QUE ESTAR EN FORMATO DWF DE AUTOCAD
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* TIENE QUE ESTAR EN 2D
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* TIENE QUE TENER FONDO BLANCO
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* TIENE QUE TENER LÍNEAS NEGRAS CON ESPESOR REGLAMENTARIO
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="back-panel-gris pleft20" style="margin-top: 20px; padding: 10px 0px 10px 0px; text-align: left">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 80px">CONSIDERACIÓN IMPORTANTE EN UN PLANO QUE NO ES EL DE HABILITACION: 
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* TIENEN QUE ESTAR EN FORMATO JPG
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* TIENE QUE ESTAR ESCANEADO EN UNA SOLA PASADA (NO PUEDE ESTAR EN PARTES)
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 40px;">* EL PLANO TIENE QUE ESTAR CERTIFICADO ANTE ESCRIBANO PUBLICO
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>

            <%--Cierre contenido--%>
            <div class="footer">
            </div>

        </asp:Panel>

        <div class="footer-sombra">
        </div>

    </div>


    <%--Modal mensajes de error--%>
    <div id="frmError_Planos" class="modal fade" style="display: none;">
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
                                        <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
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

    <script type="text/javascript">
        //if ($("#" + id + ":visible").length > 0) {
        //    ValidatorEnable(objval, true);
        //    $(objval).hide();
        //}

        $(document).ready(function () {

            //$("#pnlInfoPaso").corner();
            init_fileUpload();

        });

        function confirmar_eliminar() {
            return confirm('¿Esta seguro que desea eliminar este Registro?');
        }
        function mostrarError() {
            mostrarPopup('pnlError');
            return false;
        }


        function init_fileUpload() {
            'use strict';
            // https://github.com/blueimp/jQuery-File-Upload/wiki/Options
            var nrorandom = Math.floor(Math.random() * 1000000);
            $("#<%: hid_filename_plano_random.ClientID%>").val(nrorandom);

            var url = '<%: ResolveUrl("~/Controls/Upload.ashx?nrorandom=") %>' + nrorandom.toString();
            $("[id*='fileupload']").fileupload({
                url: url,
                dataType: 'json',
                formData: { folder: 'c:\Temporal' },
                //acceptFileTypes: /(\.|\/)(dwf|dwf|)$/i,
                add: function (e, data) {

                    var goUpload = true;
                    var uploadFile = data.files[0];

                    if (!validar_input(uploadFile)) {
                        goUpload = false;
                    }

                    if (goUpload == true) {
                        $("[id*='progress']").show();
                        data.submit();
                    }
                },
                done: function (e, data) {
                    //var filename = nrorandom.toString() + "." + fileObj.name;
                    $("#<%: hid_filename_plano.ClientID %>").val(data.files[0].name);
                    $("#<%: btnCargarPlano.ClientID %>").click();

                },
                progressall: function (e, data) {
                    var porc = parseInt(data.loaded / data.total * 100, 10);
                    progress(porc);
                },
                fail: function (e, data) {
                    alert(data.files[0].error);
                }

            }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');

        }


        function progress(value) {

            $("#progress .bar").css(
                'width',
                value + '%'
            );
        }

        function validar_input(uploadFile) {
            if ($("#<%: hid_requierre_detalle.ClientID %>").val() == 'True') {
                if ($("#<%: txtDetalle.ClientID%>").val() == null ||
                    $("#<%: txtDetalle.ClientID%>").val().trim() == '') {
                    alert('El Detalle es requerido.');
                    return false;
                }
            }

            if ($("#<%: hid_extension.ClientID %>").val() == 'dwf') {
                if (!(/\.(dwf|dwf)$/i).test(uploadFile.name)) {
                    alert('Solo se permiten archivos de tipo plano (*.dwf)');
                    return false;
                }
            } else if ($("#<%: hid_extension.ClientID%>").val() == 'jpg') {
                if (!(/\.(jpg|jpg)$/i).test(uploadFile.name)) {
                    alert('Solo se permiten archivos de tipo plano (*.jpg)');
                    return false;
                }
            } else {
                alert('El tipo ' + $("#<%: hid_extension.ClientID%>").val() + ' no esta soportado');
                return false;
            }

        var p = parseInt($("#<%: hid_tamanio_max.ClientID%>").val(), 10);
            if (uploadFile.size > p) { // 2mb
                alert('El tamaño máximo permitido es de ' + $("#<%:hid_tamanio.ClientID %>").val() + ' MB');
                return false;
            }

            return true;
        }

        function finalizarCarga222() {
            $("#<%: txtDetalle.ClientID %>").val("");
            return false;
        }

        function showfrmError_Planos() {
            $("#frmError_Planos").modal("show");
            return false;
        }

    </script>
</div>

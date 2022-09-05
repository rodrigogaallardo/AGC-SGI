
<%@ Control Language="C#" AutoEventWireup="true" 
    CodeBehind="ucDocumentoAdjunto.ascx.cs" 
    Inherits="SGI.GestionTramite.Controls.ucDocumentoAdjunto" %>

<%: Scripts.Render("~/bundles/fileUpload") %>
<%: Styles.Render("~/bundles/fileUploadCss") %>

<div class="accordion-group widget-box">

    <div class="accordion-heading">
        <a id="btnUpDown" data-parent="#collapse-group" href="#collapse_doc_adj" 
            data-toggle="collapse" onclick="tda_btnUpDown_collapse_click(this)">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5><asp:Label ID="tituloControl" runat="server" Text="Lista de Documentos Adjuntos"></asp:Label></h5>
                <span class="btn-right"><i class="icon-chevron-down"></i></span>        
            </div>
        </a>
    </div>

    <div  class="accordion-body collapse in" id="collapse_doc_adj" >

        <div class="widget-content" >


            <div>

                <asp:Panel ID="pnlDocAdj" runat="server" Visible="true">

                    <asp:UpdatePanel ID="updPnlDocumentoAdjunto" runat="server">
                    <ContentTemplate>

                        <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
                        <asp:HiddenField ID="hid_id_tramitetarea" runat="server"  Value="0"/>
                        <asp:HiddenField ID="hid_id_encomienda" runat="server"  Value="0"/>
                        <asp:HiddenField ID="hid_editable" runat="server" Value="true" />
                        <asp:HiddenField ID="hid_filename" runat="server"/>
                        <asp:HiddenField ID="hid_filename_random" runat="server"/>
                        <asp:HiddenField ID="hid_filesize" runat="server"/>

                        <%-- 1mg --%>
                        <asp:HiddenField ID="hid_size_max" runat="server" Value="2097152" /> 
                        
                        <asp:Panel ID="pnlDocumentosAdjuntos" runat="server" Visible="true">

                            <asp:GridView ID="grd_doc_adj" runat="server" AutoGenerateColumns="false" 
                                DataKeyNames="id_docadjunto,id_tdocreq"
                                GridLines="none" CssClass="table table-striped table-bordered"
                                OnRowDataBound="grd_doc_adj_RowDataBound" >
                                <Columns>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="200px" 
                                        HeaderText="Tipo de Documento" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_id_tdocreq" runat="server" 
                                                Text='<%# Eval("id_tdocreq")%>' style="display:none">
                                            </asp:Label>
                                            <%# Eval("tdocreq_detalle") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CreateDate" HeaderText="Fecha"  DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px"/>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkVerDoc" runat="server" 
                                                CssClass="btnVerPdf20"  NavigateUrl='<%# ResolveUrl("~/Reportes/Imprimir_Documentos_Adjuntos.aspx?id=" + Eval("id_docadjunto")) %>' 
                                                Text="Ver" Width="40px" >
                                            </asp:HyperLink>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminarDocAdj" runat="server" 
                                                    CommandArgument='<%# Eval("id_docadjunto") %>' 
                                                    OnClientClick="javascript:return tda_confirm_del();"
                                                    OnCommand="lnkEliminarDocAdj_Command" 
                                                    Width="70px" >
                                                <i class="icon icon-trash"></i> 
                                                <span class="text">Eliminar</span></a>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-block" >
                                        <span class="text">No posee documentos adjuntos en la tarea actual.</span>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </asp:Panel>


                        
                        <asp:Panel ID="pnlFiles" runat="server" Visible="false">

                            <asp:GridView ID="grdFiles" runat="server" AutoGenerateColumns="false" 
                                GridLines="none" CssClass="table table-striped table-bordered" >
                                <Columns>

                                    <asp:BoundField DataField="nombre_tdocreq" HeaderText="Nombre" />
                                    <asp:BoundField DataField="CreateDate" HeaderText="Fecha"  DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px"/>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>

                                            <asp:HyperLink ID="lnkVerDoc" runat="server" 
                                                CssClass="btnVerPdf20"  Target="_blank"
                                                NavigateUrl='<%# Eval("url") %>' 
                                                Text="Ver" Width="40px" >
                                            </asp:HyperLink>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminarDocAdj" runat="server" 
                                                    CommandArgument='<%# Eval("id_docadjunto") %>' 
                                                    OnClientClick="javascript:return tda_confirm_del();"
                                                    OnCommand="lnkEliminarDocAdj_Command" 
                                                    Width="70px" >
                                                <i class="icon icon-trash"></i> 
                                                <span class="text">Eliminar</span></a>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-block" >
                                        <span class="text">No posee documentos adjuntos en la tarea actual.</span>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>

                </asp:Panel>

            </div>

        </div>

        <asp:Panel ID="aspPanelBoton" runat="server" Visible="true">    
            <div id="controles_otro_adjunto" class="widget-content form-horizontal">

                <asp:Table ID="tblTipoDoc" runat="server" style="display: none">
                    <%--Desde js se usa esta tabla pasa saber cuando mostrar o no el campo detalle --%>
                </asp:Table> 
             
                <%--tipo de documento y detalle--%>
                <div id="div_input_datos_arch">

                    <asp:UpdatePanel ID="updPnlInputDatosArch" runat="server">
                    <ContentTemplate>
                        <%--combo con tipo de documento--%>
                        <div class="control-group">
                            <asp:Label ID="lbl_tipo_doc" runat="server" AssociatedControlID="ddl_tipo_doc" 
                                CssClass="control-label">Tipo de Documento:</asp:Label>

                            <div class="controls">
                                <asp:UpdatePanel ID="upd_ddl_tipo_doc" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_tipo_doc" runat="server" Width="600px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_tipo_doc_SelectedIndexChanged"  >
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                                <div class="req">
                                    <asp:Label ID="val_upload_req_ddl_tipo_doc" runat="server" 
                                        Text="Debe seleccionar el tipo de documentos."  
                                        CssClass="field-validation-error" 
                                        style="display:none" >
                                    </asp:Label>
                                </div>
                            </div>

                            <asp:Button ID="btnComenzarCargaArchivo" runat="server"
                                style="display:none" OnClick="btnComenzarCargaArchivo_Click" />
                            
                        </div>

                        <%--texto libre con detalle--%> 
                        <div id="div_detalle_otro" style="display:none">

                           <div class="control-group">
                       
                                <asp:Label ID="lblDetalle" runat="server" AssociatedControlID="txtDetalle" 
                                    CssClass="control-label">Detalle Documento:</asp:Label>

                                <div class="controls">
                                    <asp:TextBox ID="txtDetalle" runat="server"  
                                        Width="350px" MaxLength="50"></asp:TextBox>
                                    <div class="req">
                                        <asp:Label ID="val_upload_txtDetalle" runat="server" 
                                            Text="Debe ingresar el detalle para este tipo de documento."  
                                            CssClass="field-validation-error" 
                                            style="display:none" ></asp:Label>
                                    </div>

                                </div>

                            </div>

                        </div>

                    </ContentTemplate>
                    </asp:UpdatePanel>

                </div>


                <%--boton cargar archivo--%>
                <div id="div_input_upload"  >
                    <div class="control-group">
                        <div class="controls">
                                                       
                            <span class="btn btn-inverse fileinput-button">
                                    <i class="icon-white icon-th"></i>
                                    <span>Seleccionar Documento</span>
                                    <input id="fileupload" type="file" name="files[]" multiple accept="acrobat/pdf" >
                            </span>

                            <div class="req">
                                <asp:Label ID="val_upload_fileupload_pdf" runat="server" 
                                    Text="Solo se permiten archivos de tipo Acrobat Reader(*.pdf)"  
                                    CssClass="field-validation-error" 
                                    style="display:none" ></asp:Label>
                            </div>
                            
                            <div class="req">
                                <asp:Label ID="val_upload_fileupload_size" runat="server"
                                    Text="El tamaño máximo permitido es de 2 MB"
                                    CssClass="field-validation-error"
                                    Style="display: none"></asp:Label>
                            </div>

                            <div class="req">
                                <asp:Label ID="val_upload_fileupload_vacio" runat="server"
                                    Text="El archivo está vacio."
                                    CssClass="field-validation-error"
                                    Style="display: none"></asp:Label>
                            </div>

                            <div id="listaArchivos">
                            </div>
                    </div>
                </div>
                <!-- The global progress bar -->
                <div id="progress" class="progress mtop5" style="display:none">
                    <div class="bar bar-success"></div>
                </div>
            </div>
                <%--imagen de cargando de espera --%>
                <div id="div_loading_carga" style="display:none; padding-top:5px; padding-bottom:5px" >
                    <div style="text-align:center">
                        <img src='<%: ResolveUrl("~/Content/img/app/Loading64x64.gif")%>' />
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>

</div>




<%--Modal mensajes de error--%>
<div id="frmError_ucDocumentoAdjunto" class="modal fade" style="display: none;">
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
<!-- /.modal -->


<script type="text/javascript">


    $(document).ready(function () {
        tda_init_fileUpload();
    });

    function showfrmError_ucDocumentoAdjunto() {
        $("#frmError_ucDocumentoAdjunto").modal("show");
        return false;
    }

    function init_Js_updPnlInputDatosArch(){
        tda_init_fileUpload();
        $("#<%: ddl_tipo_doc.ClientID %>").select2({ allowClear: true });
        return false;
    }

    function init_Js_upd_ddl_tipo_doc() {
        $("#<%: ddl_tipo_doc.ClientID %>").select2({ allowClear: true });
        return false;
    }
    

    function tda_confirm_del() {
        return confirm('¿Esta seguro que desea eliminar este Registro?');
    }

    function tda_mostrarError() {
        mostrarPopup('pnlError');
        return false;
    } 

    function showDetalleDocumento() {
        $("#div_detalle_otro").show();
        return false;
    }

    function hideDetalleDocumento() {
        $("#div_detalle_otro").hide();
        return false;
    }

    function tda_mostrar_mensaje(texto, titulo) {

        if (titulo == "") {
            titulo = "Documentos Adjuntos";
        }

        $.gritter.add({
            title: titulo,
            text: texto,
            image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
            sticky: false
        });

    }

    function tda_init_fileUpload() {
        'use strict';


        $("[id*='progress']").hide();

        var txt_detalle = $('#<%=txtDetalle.ClientID%>')[0];
        $(txt_detalle).attr("value", "");

        var nrorandom = Math.floor(Math.random() * 1000000);

        $("#<%: hid_filename_random.ClientID %>").prop("value", nrorandom);

        var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();

        $("[id*='fileupload']").fileupload({
            url: url,
            dataType: 'json',
            formData: { folder: 'c:\Temporal' },
            acceptFileTypes: /(\.|\/)(pdf|pdf|)$/i,
            add: function (e, data) {
                var goUpload = true;
                var uploadFile = data.files[0];
                
                if (!tda_validar_input(uploadFile.name, uploadFile.size)) {
                    goUpload = false;
                }

                if (goUpload == true) {
                    $("[id*='progress']").show();
                    data.submit();
                }
            },
            done: function (e, data) {
                //var filename = nrorandom.toString() + "." + fileObj.name;
                $("#<%: hid_filename.ClientID %>").val(data.files[0].name);
                $("#<%: btnComenzarCargaArchivo.ClientID %>").click();
                },
            progressall: function (e, data) {
                var porc = parseInt(data.loaded / data.total * 100, 10);
                tda_progress(porc);
            },
            fail: function (e, data) {
                alert(data.files[0].error);
            }

        }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');

    }

    function tda_progress(value) {

        $("#progress .bar").css(
            'width',
            value + '%'
        );
    }

    function tda_validar_input(arch_nombre, arch_size) {

        var es_valido = false;

        tda_validar_fileupload_tipo_archivo(arch_nombre);
        tda_validar_fileupload_size_archivo(arch_size);

        var tipo_doc = tda_validar_tipo_documento();
        var detalle_tipo_doc = tda_validar_detalle_tipo_documento();
        
        var val__errores_visibles = $("[id*='val_upload']:visible").length;
        if (val__errores_visibles > 0) {
            es_valido =  false;
        }
        else {
            es_valido =  true;
        }

        if (es_valido) {
            //verificar si existe antes de guardar
            var existeDocumento = tda_validar_existe_documento();

            if (!existeDocumento) {
                es_valido = true;
            }
            else {
                var confirmar = confirm('El tipo de documento existe. ¿Desea reemplazarlo?');

                if (confirmar) {
                    es_valido = true;
                }
                else {
                    es_valido = false;
                }
            }

        }

        return es_valido;
    }

    function tda_validar_existe_documento() {

        var ddl_tipo_doc = $('#<%=ddl_tipo_doc.ClientID%>').attr("value");
        var existe = false;

        $('#<%=grd_doc_adj.ClientID%> tbody tr').each(function (indexFila) {
            if (existe == false && indexFila > 0) {

                var lbl_tipo_doc = $(this).find("td")[0];
                var id_tipo_doc = $(this).find("td span")[0].innerHTML;

                if ($.trim(id_tipo_doc) == $.trim(ddl_tipo_doc)) {
                    existe = true;
                }
            }

        });

        return existe;
    }


    function tda_validar_tipo_documento() {
        var combo = $('#<%=ddl_tipo_doc.ClientID%>')[0];
        var valor = $(combo).attr("value"); // es los mismo que esto  $(combo).val();
        var es_valido = false;

        if (valor == "0") {
            es_valido = false;
            $('#<%=val_upload_req_ddl_tipo_doc.ClientID%>').show();
        }
        else {
            es_valido = true;
            $('#<%=val_upload_req_ddl_tipo_doc.ClientID%>').hide();
        }

        return es_valido;

    }

    function tda_validar_detalle_tipo_documento() {
        var es_valido = true;

        if ($("#div_detalle_otro").is(':visible') && $.trim($("#<%: txtDetalle.ClientID %>").val()).length == 0) {
            
            var comboText = $('#<%=ddl_tipo_doc.ClientID%> option:selected').text();
            $('#<%=val_upload_txtDetalle.ClientID%>').text("En tipo de documento '" + comboText + "', es obligatoria la aclaración.");  // asi funciona

            var textoDetalle = $('#<%=txtDetalle.ClientID%>').attr('value');

            if (textoDetalle.length == 0) {
                es_valido = false;
            }
            else {
                es_valido = true;
            }

        }

        if (es_valido) {
            $('#<%=val_upload_txtDetalle.ClientID%>').hide();
        }
        else {
            $('#<%=val_upload_txtDetalle.ClientID%>').show();
        }

        return es_valido;
    }

    function tda_validar_fileupload_tipo_archivo(arch_nombre) {

        $('#<%=val_upload_fileupload_pdf.ClientID%>').hide();
        if (!(/\.(pdf|pdf)$/i).test(arch_nombre)) {
            $('#<%=val_upload_fileupload_pdf.ClientID%>').show();
        }

    }

    function tda_validar_fileupload_size_archivo(arch_size) {
        $('#<%=val_upload_fileupload_size.ClientID%>').hide();
        $('#<%=val_upload_fileupload_vacio.ClientID%>').hide();
        var max = $('#<%=hid_size_max.ClientID%>').val();
        if (arch_size < 1)
            $('#<%=val_upload_fileupload_vacio.ClientID%>').show();
        if (arch_size > max) { // 1mb
            $('#<%=val_upload_fileupload_size.ClientID%>').show();
        }
    }

    function tda_btnUpDown_collapse_click(obj) {
        var href_collapse = $(obj).attr("href");
        
        if ($(href_collapse).attr("id") != undefined) {
            if ($(href_collapse).css("height") == "0px") {
                tda_init_fileUpload(false);
                $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
            }
            else {
                $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
            }

        }
    }

</script>



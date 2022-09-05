<%@ Page Title="Agregar Presentacion a agregar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AgregarPA.aspx.cs" Inherits="SGI.ABM.AgregarPA" %>

<%@ Register Src="~/GestionTramite/Controls/ucCabecera.ascx" TagPrefix="uc1" TagName="ucCabecera" %>
<%@ Register Src="~/GestionTramite/Controls/ucListaDocumentosv1.ascx" TagPrefix="uc1" TagName="ucListaDocumentos" %>

  
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/Unicorn") %>
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <script src="../Scripts/Datepicker_es.js"></script>
    <%: Scripts.Render("~/bundles/fileUpload") %>
    <%: Styles.Render("~/bundles/fileUploadCss") %>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

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

    <div id="page_content" style="display:none">
        <%--Busqueda de Profesionales--%>
        <div id="box_busqueda" >
            
            <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar" >
            
	    	    <div class="widget-title">
		    	    <span class="icon"><i class="icon-search"></i></span>
			        <h5>Agregar Presentacion a agregar</h5>
		        </div>
		        <div class="widget-content">
                    <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">
                                <div class="control-group">
                                    <asp:Label ID="lblSolicitud" runat="server" AssociatedControlID="txtSolicitud" Text="Solicitud:" class="control-label"></asp:Label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSolicitud" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="ReqSolicitud" runat="server" ValidationGroup="Reabrir"
                                            ControlToValidate="txtSolicitud" Display="Dynamic" CssClass="field-validation-error"
                                            ErrorMessage="Ingrese la Solicitud."></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
				            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <asp:HiddenField ID="hid_id_tarea" runat="server" Value="0" />
                    <asp:HiddenField ID="hid_id_tramite_tarea" runat="server" Value="0" />
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <br /><br />
            
            <%-- Muestra Resultados--%>
            <div id="box_resultado" style="display:none;">
                <div class="widget-box">     
                    <%--Resultados --%>
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDatos" runat="server" CssClass="form-horizontal">
                                <div class="control-group">
                                    <uc1:ucCabecera runat="server" id="ucCabecera" />
                                    <uc1:ucListaDocumentos runat="server" id="ucListaDocumentos" OnEliminarListaDocumentosv1Click="ucListaDocumentos_EliminarListaDocumentosv1Click" />

                                    <div  class="accordion-body collapse in" id="collapse_doc_adj" >
                                        <asp:Panel ID="aspPanelBoton" runat="server" Visible="true">
                                            <div id="controles_otro_adjunto" class="widget-content form-horizontal">
                                                <asp:HiddenField ID="hid_id_solicitud" runat="server" Value="0" />
                                                <asp:HiddenField ID="hid_id_grupotramite" runat="server" Value="0" />
                                                <asp:HiddenField ID="hid_filename" runat="server" />
                                                <asp:HiddenField ID="hid_filename_random" runat="server" />

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
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_tipo_doc_SelectedIndexChanged">
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
                                                                    Style="display: none" OnClick="btnComenzarCargaArchivo_Click" />

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
                                                <div id="div_input_upload" runat="server" >
                                                    <div class="control-group">
                                                        <div class="controls">

                                                            <span class="btn btn-inverse fileinput-button">
                                                                    <i class="icon-white icon-th"></i>
                                                                    <span>Seleccionar Documento</span>
                                                                    <input id="fileupload" type="file" name="files[]" multiple accept="acrobat/pdf" >
                                                            </span>

                                                            <asp:UpdatePanel ID="updPnlMensajes" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:HiddenField ID="hid_formato_archivo" runat="server" Value="pdf" />
                                                                    <asp:HiddenField ID="hid_size_max" runat="server" Value="2097152" />
                                                                    <div class="req">
                                                                        <asp:Label ID="val_upload_fileupload_pdf" runat="server" 
                                                                            Text="Solo se permiten archivos de tipo Acrobat Reader(*.pdf)"  
                                                                            CssClass="field-validation-error" 
                                                                            style="display:none" ></asp:Label>
                                                                    </div>
                            
                                                                    <div class="req">
                                                                        <asp:Label ID="val_upload_fileupload_size" runat="server" 
                                                                            Text="El tamaño máximo permitido es de 1 MB"  
                                                                            CssClass="field-validation-error" 
                                                                            style="display:none" ></asp:Label>
                                                                    </div>

                                                                    <div class="req">
                                                                        <asp:Label ID="val_upload_fileupload_vacio" runat="server"
                                                                            Text="El archivo está vacio."
                                                                            CssClass="field-validation-error"
                                                                            Style="display: none"></asp:Label>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                            <div id="listaArchivos">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- The global progress bar -->
                                                    <div id="progress" class="progress mtop5" style="display: none">
                                                        <div class="bar bar-success"></div>
                                                    </div>
                                                </div>
                                                <%--imagen de cargando de espera --%>
                                                <div id="div_loading_carga" style="display: none; padding-top: 5px; padding-bottom: 5px">
                                                    <div style="text-align: center">
                                                        <img src='<%: ResolveUrl("~/Content/img/app/Loading64x64.gif")%>' />
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>


                                    </div>
                                        
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <script type="text/javascript">

        $(document).ready(function () {
            $("#page_content").hide();
            $("#Loading").show();
            $("#<%: btnCargarDatos.ClientID %>").click();
        });

        function init_Js_updDatos() {
        }
    
        function init_Js_updpnlBuscar() {
            $("#<%: txtSolicitud.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999999' });
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
            tda_init_fileUpload();
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

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }

        function ConfirmDel() {
            return confirm('¿Esta seguro que desea quitar esta condición?');
        }

    function showfrmError_ucDocumentoAdjunto() {
        $("#frmError_ucDocumentoAdjunto").modal("show");
        return false;
    }

    function init_Js_updPnlInputDatosArch() {
        tda_init_fileUpload();
        //$("#<%: ddl_tipo_doc.ClientID %>").select2({ allowClear: true });
        return false;
    }

    function init_Js_upd_ddl_tipo_doc() {
        //$("#<%: ddl_tipo_doc.ClientID %>").select2({ allowClear: true });
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

        $("[id*='progress']").hide();
        tda_progress(0);
    }

    function tda_init_fileUpload() {
        'use strict';

        $("[id*='progress']").hide();

        //if ($('#<%=ddl_tipo_doc.ClientID%>').length == 1) 
        //    $('#<%=ddl_tipo_doc.ClientID%> option[value=0]').attr("selected", true);

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

        //var ddl_tipo_doc = $('#<%=ddl_tipo_doc.ClientID%>').attr("value");
        var existe = false;

<%--        $('#<%=grd_doc_adj.ClientID%> tbody tr').each(function (indexFila) {
            if (existe == false && indexFila > 0) {

                var lbl_tipo_doc = $(this).find("td")[0];
                var id_tipo_doc = $(this).find("td span")[0].innerHTML;

                if ($.trim(id_tipo_doc) == $.trim(ddl_tipo_doc)) {
                    existe = true;
                }
            }

        });--%>

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
        debugger;
        if ($("#div_detalle_otro").is(':visible') && $("#div_detalle_otro").parents(':hidden').length == 0 && $("#MainContent_txtDetalle").val().length == 0) {
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
        var format = $('#<%=hid_formato_archivo.ClientID%>').val();
        if (format == 'pdf') {
            $('#<%=val_upload_fileupload_pdf.ClientID%>').text("Solo se permiten archivos de tipo Acrobat Reader(*.pdf)");
            if (!(/\.(pdf|pdf)$/i).test(arch_nombre))
                $('#<%=val_upload_fileupload_pdf.ClientID%>').show();
        } else if (format == 'jpg') {
            $('#<%=val_upload_fileupload_pdf.ClientID%>').text("Solo se permiten archivos de tipo Imagen (*.jpg)");
            if (!(/\.(jpg|jpg)$/i).test(arch_nombre))
                $('#<%=val_upload_fileupload_pdf.ClientID%>').show();
        } else {
            $('#<%=val_upload_fileupload_pdf.ClientID%>').text("El formato de archivo no esta soportado");
            $('#<%=val_upload_fileupload_pdf.ClientID%>').show();
        }
    }

    function tda_validar_fileupload_size_archivo(arch_size) {
        debugger;
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
</asp:Content>

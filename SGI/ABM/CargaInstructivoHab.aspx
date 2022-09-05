<%@ Title="Instructivos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CargaInstructivoHab.aspx.cs" Inherits="SGI.ABM.CargaInstructivoHab" %>


<asp:content id="Content3" contentplaceholderid="MainContent" runat="server">
    
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/fileUpload") %>
    <%: Scripts.Render("~/bundles/gritter") %>
    <%: Styles.Render("~/bundles/fileUploadCss") %>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>
    <%: Styles.Render("~/Content/themes/base/css") %>

    <hgroup class="title">
        <h1>Carga de Instructivos</h1>
    </hgroup>

    <asp:Panel ID="pnlBotonDefault" runat="server">
        
        <div class="accordion-group widget-box">

                <%-- titulo collapsible --%>
                <div class="accordion-heading">
                    <a id="bt_elevador_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_instructivos"
                        data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_elevador_tituloControl" runat="server" Text="Carga de Instructivos"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>

                <%-- controles collapsible --%>
                <div class="accordion-body collapse in" id="collapse_bt_instructivos">
                    <div class="widget-content">

                        <%--Instructivo de Consulta al Padron --%>
                        <asp:UpdatePanel ID="updPnlInstructivoCPadron" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">
                                    <fieldset>

                                        <div class="control-group  widget-title" style="height: 60px;">
                                            <span class="icon"><i class="icon-info-sign"></i></span>

                                            <h5>
                                                <asp:Label ID="Label1" runat="server" Text="Instructivo Consulta al Padron"></asp:Label></h5>

                                            <div class="control-group" style="float: right;">

                                                <div class="controls">
                                                    <span class="btn btn-inverse fileinput-button">
                                                        <i class="icon-white icon-th"></i>
                                                        <span>Seleccionar Instructivo</span>
                                                        <input id="fileupload" type="file" name="files[]" multiple accept="acrobat/pdf">
                                                    </span>
                                                    <asp:HyperLink ID="HyperLinkInst" runat="server" CssClass="btn btn-success" NavigateUrl="~/Reportes/DescargarInstructivo.aspx">
                                                        <i class="imoon imoon-arrow-down"></i>
                                                        <span class="text">Descargar Instructivo</span>
                                                    </asp:HyperLink>
                                                    <div class="req">
                                                        <asp:Label ID="val_upload_fileupload_pdf" runat="server"
                                                            Text="Solo se permiten archivos de tipo Acrobat Reader(*.pdf)"
                                                            CssClass="field-validation-error"
                                                            Style="display: none"></asp:Label>
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
                                                    <!-- The global progress bar -->
                                                    <div id="progress" class="progress mtop5" style="display: none">
                                                        <div class="bar bar-success"></div>
                                                    </div>

                                                    <div id="div_loading_carga" style="display: none; padding-top: 5px; padding-bottom: 5px">
                                                        <div style="text-align: center">
                                                            <img src='<%: ResolveUrl("~/Content/img/app/Loading64x64.gif")%>' />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <%--boton cargar archivo--%>

                                            <%--controles con ajax--%>
                                            <asp:UpdatePanel ID="updPnlCargarArchivo" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hid_filename" runat="server" />
                                                    <asp:HiddenField ID="hid_filename_random" runat="server" />
                                                    <asp:HiddenField ID="hid_filesize" runat="server" />

                                                    <asp:Button ID="btnComenzarCargaArchivo" runat="server" Style="display: none" OnClick="btnComenzarCargaArchivo_Click" />

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </fieldset>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%--Instructivo de transferencias--%>
                        <asp:UpdatePanel ID="updPnlInstructivoTransf" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">
                                    <fieldset>

                                        <div class="control-group  widget-title" style="height: 60px;">
                                            <span class="icon"><i class="icon-info-sign"></i></span>

                                            <h5>
                                                <asp:Label ID="Label2" runat="server" Text="Instructivo Transferencias"></asp:Label></h5>

                                            <div class="control-group" style="float: right;">
                                                <div class="controls">
                                                    <span class="btn btn-inverse fileinput-button">
                                                        <i class="icon-white icon-th"></i>
                                                        <span>Seleccionar Instructivo</span>
                                                        <input id="fileupload2" type="file" name="files[]" multiple accept="acrobat/pdf">
                                                    </span>
                                                    <asp:HyperLink ID="HyperLinkProf" runat="server" CssClass="btn btn-success" NavigateUrl="~/Reportes/DescargarInstructivo.aspx">
                                                        <i class="imoon imoon-arrow-down"></i>
                                                        <span class="text">Descargar Instructivo</span>
                                                    </asp:HyperLink>
                                                    <div class="req">
                                                        <asp:Label ID="v2al_upload_fileupload_pdf" runat="server"
                                                            Text="Solo se permiten archivos de tipo Acrobat Reader(*.pdf)"
                                                            CssClass="field-validation-error"
                                                            Style="display: none"></asp:Label>
                                                    </div>
                                                    <div class="req">
                                                        <asp:Label ID="v2al_upload_fileupload_size" runat="server"
                                                            Text="El tamaño máximo permitido es de 2 MB"
                                                            CssClass="field-validation-error"
                                                            Style="display: none"></asp:Label>
                                                    </div>
                                                    <div class="req">
                                                        <asp:Label ID="v2al_upload_fileupload_vacio" runat="server"
                                                            Text="El archivo está vacio."
                                                            CssClass="field-validation-error"
                                                            Style="display: none"></asp:Label>
                                                    </div>
                                                    <div id="p2rogress" class="progress mtop5" style="display: none">
                                                        <div class="bar bar-success"></div>
                                                    </div>

                                                    <div id="div_loading_carga2" style="display: none; padding-top: 5px; padding-bottom: 5px">
                                                        <div style="text-align: center">
                                                            <img src='<%: ResolveUrl("~/Content/img/app/Loading64x64.gif")%>' />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <%--boton cargar archivo--%>

                                            <!-- The global progress bar -->

                                            <%--controles con ajax--%>
                                            <asp:UpdatePanel ID="updPnlCargarArchivo2" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hid_filename2" runat="server" />
                                                    <asp:HiddenField ID="hid_filename_random2" runat="server" />
                                                    <asp:HiddenField ID="hid_filesize2" runat="server" />

                                                    <asp:Button ID="btnComenzarCargaArchivo2" runat="server"
                                                        Style="display: none" OnClick="btnComenzarCargaArchivo2_Click" />


                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </fieldset>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%--Instructivo de Habilitaciones--%>
                        <asp:UpdatePanel ID="updPnlInstructivoHab" runat="server" UpdateMode="Conditional">
	                    <ContentTemplate>

		                    <div class="form-horizontal">
			                    <fieldset>

				                    <div class="control-group  widget-title" style="height: 60px;">
					                    <span class="icon"><i class="icon-info-sign"></i></span>

					                    <h5>
						                    <asp:Label ID="Label3" runat="server" Text="Instructivo de Habilitaciones"></asp:Label></h5>

					                    <div class="control-group" style="float: right;">
						                    <div class="controls">
							                    <span class="btn btn-inverse fileinput-button">
								                    <i class="icon-white icon-th"></i>
								                    <span>Seleccionar Instructivo</span>
								                    <input id="fileupload3" type="file" name="files[]" multiple accept="acrobat/pdf">
							                    </span>
							                    <asp:HyperLink ID="lnkDescargarInstHab" runat="server" CssClass="btn btn-success" NavigateUrl="~/Reportes/DescargarInstructivo.aspx">
											        <i class="imoon imoon-arrow-down"></i>
											        <span class="text">Descargar Instructivo</span>
							                    </asp:HyperLink>
							                    <div class="req">
								                    <asp:Label ID="v3al_upload_fileupload_pdf" runat="server"
									                    Text="Solo se permiten archivos de tipo Acrobat Reader(*.pdf)"
									                    CssClass="field-validation-error"
									                    Style="display: none"></asp:Label>
							                    </div>
							                    <div class="req">
								                    <asp:Label ID="v3al_upload_fileupload_size" runat="server"
									                    Text="El tamaño máximo permitido es de 2 MB"
									                    CssClass="field-validation-error"
									                    Style="display: none"></asp:Label>
							                    </div>
							                    <div class="req">
								                    <asp:Label ID="v3al_upload_fileupload_vacio" runat="server"
									                    Text="El archivo está vacio."
									                    CssClass="field-validation-error"
									                    Style="display: none"></asp:Label>
							                    </div>
							                    <div id="p3rogress" class="progress mtop5" style="display: none">
								                    <div class="bar bar-success"></div>
							                    </div>


							                    <div id="div_loading_carga3" style="display: none; padding-top: 5px; padding-bottom: 5px">
								                    <div style="text-align: center">
									                    <img src='<%: ResolveUrl("~/Content/img/app/Loading64x64.gif")%>' />
								                    </div>
							                    </div>

						                    </div>
					                    </div>
					                    <%--boton cargar archivo--%>

					                    <!-- The global progress bar -->
					                    <%--controles con ajax--%>
					                    <asp:UpdatePanel ID="updPnlCargarArchivo3" runat="server">
						                    <ContentTemplate>
							                    <asp:HiddenField ID="hid_filename3" runat="server" />
							                    <asp:HiddenField ID="hid_filename_random3" runat="server" />
							                    <asp:HiddenField ID="hid_filesize3" runat="server" />

							                    <asp:Button ID="btnComenzarCargaArchivo3" runat="server"
								                    Style="display: none" OnClick="btnComenzarCargaArchivo3_Click" />

						                    </ContentTemplate>
					                    </asp:UpdatePanel>
				                    </div>
			                    </fieldset>
		                    </div>

	                    </ContentTemplate>
                    </asp:UpdatePanel>

                        <%--Instructivo de Anexo Tecnico --%>
                        <asp:UpdatePanel ID="updPnlInstructivoAXTecnico" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                
		                    <div class="form-horizontal">
			                    <fieldset>

				                    <div class="control-group  widget-title" style="height: 60px;">
					                    <span class="icon"><i class="icon-info-sign"></i></span>

					                    <h5>
						                    <asp:Label ID="lblAXTecnico" runat="server" Text="Instructivo de Anexo Técnico de Habilitaciones"></asp:Label></h5>

					                    <div class="control-group" style="float: right;">
						                    <div class="controls">
							                    <span class="btn btn-inverse fileinput-button">
								                    <i class="icon-white icon-th"></i>
								                    <span>Seleccionar Instructivo</span>
                                                     <input id="fileupload5" type="file" name="files[]" multiple accept="acrobat/pdf">
							                    </span>
							                    <asp:HyperLink ID="HyperLink5" runat="server" CssClass="btn btn-success" NavigateUrl="~/Reportes/DescargarInstructivo.aspx">
											        <i class="imoon imoon-arrow-down"></i>
											        <span class="text">Descargar Instructivo</span>
							                    </asp:HyperLink>
							                    <div class="req">
								                    <asp:Label ID="v5al_upload_fileupload_pdf" runat="server"
									                    Text="Solo se permiten archivos de tipo Acrobat Reader(*.pdf)"
									                    CssClass="field-validation-error"
									                    Style="display: none"></asp:Label>
							                    </div>
							                    <div class="req">
								                    <asp:Label ID="v5al_upload_fileupload_size" runat="server"
									                    Text="El tamaño máximo permitido es de 2 MB"
									                    CssClass="field-validation-error"
									                    Style="display: none"></asp:Label>
							                    </div>
							                    <div class="req">
								                    <asp:Label ID="v5al_upload_fileupload_vacio" runat="server"
									                    Text="El archivo está vacio."
									                    CssClass="field-validation-error"
									                    Style="display: none"></asp:Label>
							                    </div>
							                    <div id="p5rogress" class="progress mtop5" style="display: none">
								                    <div class="bar bar-success"></div>
							                    </div>


							                    <div id="div_loading_carga5" style="display: none; padding-top: 5px; padding-bottom: 5px">
								                    <div style="text-align: center">
									                    <img src='<%: ResolveUrl("~/Content/img/app/Loading64x64.gif")%>' />
								                    </div>
							                    </div>

						                    </div>
					                    </div>
					                    <%--boton cargar archivo--%>

					                    <!-- The global progress bar -->
					                    <%--controles con ajax--%>
					                    
					                    <asp:UpdatePanel ID="updPnlCargarArchivo5" runat="server">
						                    <ContentTemplate>
							                    <asp:HiddenField ID="hid_filename5" runat="server" />
							                    <asp:HiddenField ID="hid_filename_random5" runat="server" />
							                    <asp:HiddenField ID="hid_filesize5" runat="server" />

							                    <asp:Button ID="btnComenzarCargaArchivo5" runat="server"
								                    Style="display: none" OnClick="btnComenzarCargaArchivo5_Click" />

						                    </ContentTemplate>
					                    </asp:UpdatePanel>
				                    </div>
			                    </fieldset>
		                    </div>

	                    </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                </div>


        </div>

        <br />

    </asp:Panel>

    
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

    <script type="text/javascript">


        $(document).ready(function () {
            init_Js_updPnlInstructivoCPadron();
            init_Js_updPnlInstructivoTransf();
            init_Js_updPnlInstructivoHab();
            init_Js_updPnlInstructivoAXTecnico();
        });

        function showfrmError() {

            $("#frmError").modal("show");

            return false;
        }

        function tda_confirm_del() {
            return confirm('¿Esta seguro que desea eliminar este Registro?');
        }
        function tda_mostrarError() {
            mostrarPopup('pnlError');
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


        function init_Js_updPnlInstructivoCPadron() {

            'use strict';
            $("#progress").hide();
            var nrorandom = Math.floor(Math.random() * 1000000);

            $("#<%: hid_filename_random.ClientID %>").prop("value", nrorandom);

            var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();

            $("#fileupload").fileupload({
                url: url,
                dataType: 'json',
                formData: { folder: 'c:\Temporal' },
                acceptFileTypes: /(\.|\/)(pdf|pdf|)$/i,
                add: function (e, data) {
                    var goUpload = true;
                    var uploadFile = data.files[0];

                    if (!tda_validar_input(uploadFile.name, uploadFile.size, 1)) {
                        goUpload = false;
                    }
                    if (goUpload == true) {
                        $("#progress").show();
                        data.submit();
                    }
                },
                done: function (e, data) {

                    $("#<%: hid_filename.ClientID %>").val(data.files[0].name);
                    $("#<%: btnComenzarCargaArchivo.ClientID %>").click();
                    $("#progress").hide();
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
        function init_Js_updPnlInstructivoTransf() {

            'use strict';
            $("#p2rogress").hide();
            var nrorandom = Math.floor(Math.random() * 1000000);

            $("#<%: hid_filename_random2.ClientID %>").prop("value", nrorandom);

            var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();

            $("#fileupload2").fileupload({
                url: url,
                dataType: 'json',
                formData: { folder: 'c:\Temporal' },
                acceptFileTypes: /(\.|\/)(pdf|pdf|)$/i,
                add: function (e, data) {
                    var goUpload = true;
                    var uploadFile = data.files[0];

                    if (!tda_validar_input(uploadFile.name, uploadFile.size, 2)) {
                        goUpload = false;
                    }
                    if (goUpload == true) {
                        $("#p2rogress").show();
                        data.submit();
                    }
                },
                done: function (e, data) {

                    $("#<%: hid_filename2.ClientID %>").val(data.files[0].name);
                    $("#<%: btnComenzarCargaArchivo2.ClientID %>").click();
                    $("#p2rogress").hide();
                },
                progressall: function (e, data) {
                    var porc = parseInt(data.loaded / data.total * 100, 10);
                    tda_progress2(porc);
                },
                fail: function (e, data) {
                    alert(data.files[0].error);
                }


            }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');


        }

        function init_Js_updPnlInstructivoHab() {

            'use strict';
            $("#p3rogress").hide();
            var nrorandom = Math.floor(Math.random() * 1000000);

            $("#<%: hid_filename_random3.ClientID %>").prop("value", nrorandom);

            var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();

            $("#fileupload3").fileupload({
                url: url,
                dataType: 'json',
                formData: { folder: 'c:\Temporal' },
                acceptFileTypes: /(\.|\/)(pdf|pdf|)$/i,
                add: function (e, data) {
                    var goUpload = true;
                    var uploadFile = data.files[0];

                    if (!tda_validar_input(uploadFile.name, uploadFile.size, 3)) {
                        goUpload = false;
                    }
                    if (goUpload == true) {
                        $("#p3rogress").show();
                        data.submit();
                    }
                },
                done: function (e, data) {

                    $("#<%: hid_filename3.ClientID %>").val(data.files[0].name);
                    $("#<%: btnComenzarCargaArchivo3.ClientID %>").click();
                    $("#p3rogress").hide();
                },
                progressall: function (e, data) {
                    var porc = parseInt(data.loaded / data.total * 100, 10);
                    tda_progress3(porc);
                },
                fail: function (e, data) {

                    alert(data.files[0].error);
                }


            }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');


        }

        function init_Js_updPnlInstructivoAXTecnico() {
            'use strict';
            $("#p5rogress").hide();
            var nrorandom = Math.floor(Math.random() * 1000000);

            $("#<%: hid_filename_random5.ClientID %>").prop("value", nrorandom);

            var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();

            $("#fileupload5").fileupload({
                url: url,
                dataType: 'json',
                formData: { folder: 'c:\Temporal' },
                acceptFileTypes: /(\.|\/)(pdf|pdf|)$/i,
                add: function (e, data) {
                    var goUpload = true;
                    var uploadFile = data.files[0];

                    if (!tda_validar_input(uploadFile.name, uploadFile.size, 5)) {
                        goUpload = false;
                    }
                    if (goUpload == true) {
                        $("#p5rogress").show();
                        data.submit();
                    }
                },
                done: function (e, data) {

                    $("#<%: hid_filename5.ClientID %>").val(data.files[0].name);
                    $("#<%: btnComenzarCargaArchivo5.ClientID %>").click();
                    $("#p5rogress").hide();
                },
                progressall: function (e, data) {
                    var porc = parseInt(data.loaded / data.total * 100, 10);
                    tda_progress5(porc);
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
        function tda_progress2(value) {

            $("#p2rogress .bar").css(
                'width',
                value + '%'
            );
        }
        function tda_progress3(value) {

            $("#p3rogress .bar").css(
                'width',
                value + '%'
            );
        }
        function tda_progress5(value) {

            $("#p5rogress .bar").css(
                'width',
                value + '%'
            );
        }
        function tda_validar_input(arch_nombre, arch_size, nroArch) {

            var es_valido = false;
            var val__errores_visibles;
            if (nroArch == 1) {

                tda_validar_fileupload_tipo_archivo(arch_nombre, 1);
                tda_validar_fileupload_size_archivo(arch_size, 1);
                val__errores_visibles = $("[id*='val_upload']:visible").length;
            }
            else if (nroArch == 2) {
                tda_validar_fileupload_tipo_archivo(arch_nombre, 2);
                tda_validar_fileupload_size_archivo(arch_size, 2);
                val__errores_visibles = $("[id*='v2al_upload']:visible").length;
            }
            else if (nroArch == 3) {
                tda_validar_fileupload_tipo_archivo(arch_nombre, 3);
                tda_validar_fileupload_size_archivo(arch_size, 3);
                val__errores_visibles = $("[id*='v3al_upload']:visible").length;
            }
            else if (nroArch == 5) {
                tda_validar_fileupload_tipo_archivo(arch_nombre, 5);
                tda_validar_fileupload_size_archivo(arch_size, 5);
                val__errores_visibles = $("[id*='v5al_upload']:visible").length;
            }

            if (val__errores_visibles > 0) {
                es_valido = false;
            }
            else {
                es_valido = true;
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
            return false;
        }

        function tda_validar_fileupload_tipo_archivo(arch_nombre, archT) {

            if (archT == 1) {
                $('#<%=val_upload_fileupload_pdf.ClientID%>').hide();

                if (!(/\.(pdf|pdf)$/i).test(arch_nombre)) {
                    $('#<%=val_upload_fileupload_pdf.ClientID%>').show();
                }
            }
            else if (archT == 2) {
                $('#<%=v2al_upload_fileupload_pdf.ClientID%>').hide();

                if (!(/\.(pdf|pdf)$/i).test(arch_nombre)) {
                    $('#<%=v2al_upload_fileupload_pdf.ClientID%>').show();
                }
            }
            else if (archT == 3) {
                $('#<%=v3al_upload_fileupload_pdf.ClientID%>').hide();

                if (!(/\.(pdf|pdf)$/i).test(arch_nombre)) {
                    $('#<%=v3al_upload_fileupload_pdf.ClientID%>').show();
                }
            }
            else if (archT == 5) {
                $('#<%=v5al_upload_fileupload_pdf.ClientID%>').hide();

                if (!(/\.(pdf|pdf)$/i).test(arch_nombre)) {
                    $('#<%=v5al_upload_fileupload_pdf.ClientID%>').show();
                }
            }

}

function tda_validar_fileupload_size_archivo(arch_size, archT) {

    if (archT == 1) {
        $('#<%=val_upload_fileupload_size.ClientID%>').hide();
                $('#<%=val_upload_fileupload_vacio.ClientID%>').hide();

                if (arch_size < 1) {
                    $('#<%=val_upload_fileupload_vacio.ClientID%>').show();
                }

                if (arch_size > 4000000) { // 4mb
                    $('#<%=val_upload_fileupload_size.ClientID%>').show();
                }
            }
            else if (archT == 2) {
                $('#<%=v2al_upload_fileupload_size.ClientID%>').hide();
                $('#<%=v2al_upload_fileupload_vacio.ClientID%>').hide();

                if (arch_size < 1) {
                    $('#<%=v2al_upload_fileupload_vacio.ClientID%>').show();
                }

                if (arch_size > 4000000) { // 4mb
                    $('#<%=v2al_upload_fileupload_size.ClientID%>').show();
                }
            }
            else if (archT == 3) {
                $('#<%=v3al_upload_fileupload_size.ClientID%>').hide();
                $('#<%=v3al_upload_fileupload_vacio.ClientID%>').hide();

                if (arch_size < 1) {
                    $('#<%=v3al_upload_fileupload_vacio.ClientID%>').show();
                }

                if (arch_size > 4000000) { // 4mb
                    $('#<%=v3al_upload_fileupload_size.ClientID%>').show();
                }
            }
            else if (archT == 5) {
                $('#<%=v5al_upload_fileupload_size.ClientID%>').hide();
                $('#<%=v5al_upload_fileupload_vacio.ClientID%>').hide();

                if (arch_size < 1) {
                    $('#<%=v5al_upload_fileupload_vacio.ClientID%>').show();
                }

                if (arch_size > 4000000) { // 4mb
                    $('#<%=v5al_upload_fileupload_size.ClientID%>').show();
                }
            }
}

    </script>
 </asp:content>







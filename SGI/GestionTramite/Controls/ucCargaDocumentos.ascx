<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCargaDocumentos.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucCargaDocumentos" %>
<asp:Panel ID="pnlucCargaDocumento" runat="server">

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>

    <%: Scripts.Render("~/bundles/fileupload") %>
    <%: Styles.Render("~/bundles/fileuploadCss") %>

    <div style="margin: 10px">
        <table>
            <tr>
                <td>
                    <i class="imoon-number fs48"></i>
                </td>
                <td>
                    <p>Selecciona el tipo de documento que deseas agregar </p>
                    <asp:UpdatePanel ID="updTiposDeDocumentosRequeridosCD" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hid_formato_archivo" runat="server" Value="pdf" />
                            <asp:HiddenField ID="hid_size_max" runat="server" Value="2097152" />
                            <asp:HiddenField ID="hid_size_max_MB" runat="server" Value="2" />
                            <asp:HiddenField ID="hid_requiere_detalle" runat="server" Value="false" />

                            <asp:DropDownList ID="ddlTiposDeDocumentosRequeridos" runat="server" Width="760px" AutoPostBack="true" OnSelectedIndexChanged="ddlTiposDeDocumentosRequeridos_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="tr_detalle" style="display: none">
                <td>
                    <br />
                </td>
                <td>
                    <p>Detalle</p>
                    <asp:TextBox ID="txtDetalle" runat="server" Width="760px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: initial">
                    <br />
                    <br />
                    <i class="imoon-number2 fs48"></i>
                </td>
                <td>
                    <br />
                    <br />
                    <p>Presiona el botón "Seleccionar Archivo" y elija el archivo que deseas agregar.</p>
                    <p><strong>Solo se admiten tipos de archivo con extensión .PDF </strong></p>

                    <span id="btnSeleccionarFilesUcCD" class="btn btn-default fileinput-button">
                        <i class="imoon imoon-folder-open"></i>
                        <span class="text">Seleccionar archivo</span>
                        <input id="fileuploadUcCD" type="file" name="files[]" multiple accept="application/pdf,image/jpeg">
                    </span>

                    <asp:Button ID="btnSubirDocumentoUcCD" runat="server" Text="Subir Documento" CssClass="btn btn-inverse" OnClick="btnSubirDocumentoUcCD_Click" Style="display: none" />
                    <asp:HiddenField ID="hid_filename_documentoUcCD" runat="server" />
                    <asp:HiddenField ID="hid_filenamereal_documentoUcCD" runat="server" />


                    <div id="pnlFilesUcCD" class="ptop20 hide">
                        <p>
                            Si el archivo que elegiste es el correcto presiona el bot&oacute;n "Subir documento" de lo contrario presiona el bot&oacute;n Cancelar y volv&eacute; a seleccionarlo.
                        </p>
                        <asp:UpdatePanel ID="updFilesUcCD" runat="server">
                            <ContentTemplate>
                                <div id="filesUcCD" class="files"></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <!-- The global progress bar -->
                        <div id="progressUcCD" class="progress mtop5" style="display: none">
                            <div class="progress-bar progress-bar-success">
                                <span id="progressvalue" class="sr-only"></span>
                            </div>
                        </div>
                    </div>
                    <div class="req">
                        <asp:Label ID="val_upload_fileupload" runat="server" Text="Error" CssClass="field-validation-error" 
                            style="display:none" ></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <asp:Panel ID="pnlErrorUcCD" runat="server" CssClass="alert alert-error mtop10 hide">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <asp:Label ID="lblErrorUcCD" runat="server">El detalle del documento es obligatorio.</asp:Label>
    </asp:Panel>

</asp:Panel>
<script type="text/javascript">
    uploadButtonUcCD = $('<button/>')
        .addClass('btn btn-primary uploadbuttonUcCD')
        .text('Subir Documento')
        .on('click', function () {
            if ($("#<%: ddlTiposDeDocumentosRequeridos.ClientID  %>").val().length == 0) {
                $('#<%=val_upload_fileupload.ClientID%>').text("Para poder subir el documento debes seleccionar el tipo de documento en el paso 1.");
                $('#<%=val_upload_fileupload.ClientID%>').show();
                return false;
            }
            else {
                $('#<%=val_upload_fileupload.ClientID%>').hide();
            }
            $("[id*='progressUcCD']").show();

            var $this = $(this),
            data = $this.data();

            data.submit().always(function (data, e) {

                if (data.files != undefined) {
                    $("#<%: hid_filename_documentoUcCD.ClientID %>").val(data.files[0].Name);
                    $("#<%: btnSubirDocumentoUcCD.ClientID %>").click();
                }

            });

        });

        cancelUploadButtonUcCD = $('<button/>')
        .addClass('btn btn-warning mleft5 cancelbuttonUcCd')
        .text('Cancelar')
        .on('click', function () {
            $("#filesUcCD table").remove();
            $('#<%=val_upload_fileupload.ClientID%>').hide();
            $("#pnlFilesUcCD").hide();
            return false;
        });

    $(document).ready(function () {
        $('#<%=val_upload_fileupload.ClientID%>').hide();
        $("#<%: ddlTiposDeDocumentosRequeridos.ClientID %>").select2("val", "");
    });

    function formatddlTipoDocumento_Selected(obj) {
        tooltips();

        var parts = obj.text.split('|');
        var html = "<b>" + parts[0] + "</b>";  //<div>" + parts[2] + "</div>";

        if (parts[0] == null)
            html = "";

        return html;
    }

    function formatddlTipoDocumento_Open(obj) {
        debugger;
        var parts = obj.text.split('|');

        var html = "<li class='select2-row'>";

        html += "<span class='select2-font-header1'>" + parts[0] + "</span>"
        if (parts[1] != undefined) {
            html += "<div class='select2-font-detalle1'>" + parts[1] + "</div>";
        }
        html += "<div class='select2-rowdivisor' /></li>";
        return html;
    }

    function init_Js_ucCargaDocumento() {
        $("#<%: ddlTiposDeDocumentosRequeridos.ClientID %>").select2({
            placeholder: 'Seleccione el tipo de documento.',
            allowClear: false,
            formatResult: formatddlTipoDocumento_Open,
            formatSelection: formatddlTipoDocumento_Selected
        });

        $("#<%: ddlTiposDeDocumentosRequeridos.ClientID %>").on("change", function () {
            $('#<%=val_upload_fileupload.ClientID%>').hide();
        });

        'use strict';
        // Change this to the location of your server-side upload handler:
        var nrorandom = Math.floor(Math.random() * 1000000);
        $("#<%: hid_filename_documentoUcCD.ClientID %>").val(nrorandom);

        var url = '<%: ResolveUrl("~/Scripts/jquery-fileupload/Upload.ashx?nrorandom=") %>' + nrorandom.toString();
        $("[id*='fileuploadUcCD']").fileupload({
            url: url,
            dataType: 'json',
            formData: { folder: 'C:\\Temporal' },

            acceptFileTypes: /(\.|\/)(pdf)$/i,
            add: function (e, data) {
                debugger;
                var goUpload = true;
                var uploadFile = data.files[0];
                $('#<%=val_upload_fileupload.ClientID%>').hide();
                //validacion de detalle
                var requiere = $('#<%=hid_requiere_detalle.ClientID%>').val();
                if (requiere == "True" && $('#<%=txtDetalle.ClientID%>').val().length == 0) {
                    $('#<%=val_upload_fileupload.ClientID%>').text("El detalle es obligatorio");
                    $('#<%=val_upload_fileupload.ClientID%>').show();
                    goUpload = false;
                }
                //validacion tipo
                var format = $('#<%=hid_formato_archivo.ClientID%>').val();
                if (format == 'pdf') {
                    if (!(/\.(pdf|pdf)$/i).test(uploadFile.name)) {
                        $('#<%=val_upload_fileupload.ClientID%>').text("Solo se permiten archivos de tipo Acrobat Reader(*.pdf)");
                        $('#<%=val_upload_fileupload.ClientID%>').show();
                        goUpload = false;
                    }
                } 
                //validar tamaño
                var max = $('#<%=hid_size_max.ClientID%>').val();
                if (uploadFile.size < 1) {
                    $('#<%=val_upload_fileupload.ClientID%>').text("El archivo está vacio");
                    $('#<%=val_upload_fileupload.ClientID%>').show();
                    goUpload = false;
                }else if (uploadFile.size > max) { 
                    $('#<%=val_upload_fileupload.ClientID%>').text("El tamaño máximo permitido es de " + $('#<%=hid_size_max_MB.ClientID%>').val() + " MB");
                    $('#<%=val_upload_fileupload.ClientID%>').show();
                    goUpload = false;
                }
                $("#<%: hid_filenamereal_documentoUcCD.ClientID %>").val(uploadFile.name);
                //genera el html con los datos del archivo y los botones para subirlo.
                $("#filesUcCD table").remove();
                data.context = $('<div/>').appendTo('#filesUcCD');
                $.each(data.files, function (index, file) {
                    var node = $('<table class="file-input-tabledata table"/>').append('<tr/>');
                    $(node).find("tr:last").append('<td class="col1"/>');
                    //$(node).find('tr:last td.col1').append('<i class="imoon-download icon20 " style="color:blue;"/>');
                    $(node).find('tr:last td.col1').append('<span class="btnpdf20x20"/>');
                    $(node).find('tr:last td.col1').append('<span class="text"/>');
                    $(node).find('tr:last td.col1 span.text').text(file.name);

                    $(node).find("tr:last").append('<td class="col2"/>');
                    $(node).find('tr:last td.col2').append('<p/>');
                    $(node).find('tr:last td.col2 p').text(parseInt(file.size / 1024).toString() + " kb.");

                    $(node).find("tr:last").append('<td class="col3"/>');
                    $(node).find('tr:last td.col3').append(uploadButtonUcCD.clone(true).data(data))
                                                    .append(cancelUploadButtonUcCD.clone(true).data(data));

                    node.appendTo(data.context);
                });

                if (goUpload == true) {
                    $("[id*='progressUcCD']").show();
                    $("#pnlFilesUcCD").removeClass("hide");
                    $("#pnlFilesUcCD").show();
                }


            },
            done: function (e, data) {
                debugger;
                $("#<%: hid_filename_documentoUcCD.ClientID %>").val(data.files[0].name);
                $("#filesUcCD .uploadbuttonUcCD").hide();
                $("#filesUcCD .cancelbuttonUcCD").hide();
            },

            progressall: function (e, data) {
                debugger;
                var porc = parseInt(data.loaded / data.total * 100, 10);
                $('#progress .progress-bar').css(
                        'width',
                        porc + '%'
                    );

                if (porc >= 100) {
                    $('#progress .progress-bar').text('Cargando archivo...espere');
                }
                else {
                    $('#progress .progress-bar').text(porc.toString() + '%');
                }
            },
            fail: function (e, data) {
                alert(data.files[0].error);
            }

        }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');
    }

    function showDetalleDocumentoCD() {
        init_Js_ucCargaDocumento();
        $("#tr_detalle").show();
        return false;
    }

    function hideDetalleDocumentoCD() {
        init_Js_ucCargaDocumento();
        $("#tr_detalle").hide();
        return false;
    }
</script>


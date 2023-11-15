<%@ Title="Emails Templates Editor"  Language="C#"  MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TemplateMailsEditor.aspx.cs" Inherits="SGI.Operaciones.ViewLayer.TestBorrar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>

        function GetTemplate(template) {
            document.getElementsByName('editor')[0].innerHTML = template;
        }
        function SaveTemplate() {
            var hdSaveTemplate = document.getElementById('<%= hdSaveTemplate.ClientID %>');
            hdSaveTemplate.value = document.getElementsByName('editor')[0].innerHTML;
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<html>


        <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
    <link href="https://cdn.quilljs.com/1.0.0/quill.snow.css" rel="stylesheet">


<body>
   <%-- <form id="quillForm" runat="server">--%>
    
        <br />

        <div>
            <label>Lista de Template</label>
            <asp:DropDownList ID="ddlTemplates" AutoPostBack="false" runat="server" OnSelectedIndexChanged="ddlTemplates_SelectedIndexChanged" ></asp:DropDownList>
            <asp:Button ID="btnGetTemplate" runat="server" Text="Traer Template" OnClick="btnGetTemplate_Click" CausesValidation="False" UseSubmitBehavior="false" />
        </div>
        <br />

             


        <div class="control-group">

            <label class="control-label" for="txtMensaje">Template</label>

            <div class="controls">
                <div name="editor" class="basic-quill" data-id="#txt_description" data-placeholder="Description" data-name="description">
                    <textarea id="txtMensaje" name="txtMensaje" runat="server" style="display: none;"></textarea>
                </div>
            </div>
        </div>
        <br />
      
        <asp:Button ID="btnSaveTemplate" runat="server" Text="Guardar Template" OnClientClick="SaveTemplate()" OnClick="btnSaveTemplate_Click" CausesValidation="False" UseSubmitBehavior="false" />

        <asp:HiddenField ID="hdSaveTemplate" runat="server" />
          <asp:HiddenField ID="hdEmailTemplateId" runat="server" />
  <%--  </form>--%>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdn.quilljs.com/1.0.0/quill.js"></script>
</body>

</html>

    <script>



        function quill_editor_load() {
            var quill_element = '.basic-quill',
                parent_form;
            if ($(quill_element).length) {
                if (typeof $(quill_element).attr("data-id") !== 'undefined' && typeof $(quill_element).attr("data-name") !== 'undefined') {
                    var place_holder = (typeof $(quill_element).attr("data-placeholder") !== 'undefined' ? $(quill_element).attr("data-placeholder") : "Enter your text here");
                    var quillElementId = (($(quill_element).attr("data-id").substring(0, 1) != '#') ? $(quill_element).attr("data-id") : $(quill_element).attr("data-id").substring(1, $(quill_element).attr("data-id").length));
                    if (typeof $(quill_element).attr("id") === 'undefined') {
                        $(quill_element).attr("id", quillElementId);
                    }
                    var quill = new Quill('#' + quillElementId, {
                        modules: {
                            toolbar: [
                                [{
                                    header: [1, 2, false]
                                }],
                                ['bold', 'italic', 'underline', 'strike'],
                                ['link', 'image', 'code-block'],
                                [{
                                    'list': 'ordered'
                                }, {
                                    'list': 'bullet'
                                }],
                            ]
                        },
                        placeholder: place_holder,
                        theme: 'snow' // or 'bubble'
                    });
                    //var input = $('input[name="' + quillElementId + '"]');
                    //parent_form = $(quill_element).parents('form');
                    //if (!input.length) {
                    //    if (parent_form.length) {
                    //        input = parent_form.append('<input type="hidden" id="' + quillElementId + '" name="' + quillElementId + '" value="">').find('input[name="' + quillElementId + '"]');
                    //    }
                    //}
                    //input.val(quill.root.innerHTML);
                    //quill.on('text-change', function (delta, oldDelta, source) {
                    //    input.val(quill.container.firstChild.innerHTML);
                    //    document.getElementById('MainContent_txtMensaje').value = quill.root.innerHTML;
                    //});
                    quill.on('text-change', function (delta, oldDelta, source) {
                        document.getElementById('MainContent_txtMensaje').value = quill.root.innerHTML;
                    });

                }
            }
        }

        $(function () {
            quill_editor_load();
            $('form#quillForm').on('submit', function (e) {
                var formData = new FormData($('form#quillForm')[0]);
                formData.forEach(function (item, idx) {
                    console.log(idx + ':', item);
                });
                e.preventDefault();
            });
        });

    </script>
</asp:Content>



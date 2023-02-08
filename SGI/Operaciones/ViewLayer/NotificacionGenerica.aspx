<%@  Title="Notificaciones de una Solicitud" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="NotificacionGenerica.aspx.cs" Inherits="SGI.NotificacionGenerica" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function redirect(obj) {
            location.href = obj.dat - href;
        }
    </script>

    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <script src="<%: ResolveUrl("~/Scripts/Datepicker_es.js")%>" type="text/javascript"></script>
    <script src="<%: ResolveUrl("~/Scripts/Funciones.js")%>" type="text/javascript"></script>

    <%: Styles.Render("~/bundles/select2Css")%>
    <%: Styles.Render("~/Content/themes/base/css")%>

    <link href="/Content/icon-moon/icon-moon.css" rel="stylesheet" type="text/css" />

    

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
   
        <%--Quill.JS--%>
   <link href="https://cdn.quilljs.com/1.3.6/quill.snow.css" rel="stylesheet">
       <script src="https://cdn.quilljs.com/1.3.6/quill.js"></script>




     <%--ASUNTO--%>
    <div class="control-group">
       <label class="control-label" for="txtAsunto">Asunto</label>
        <div class="controls">
            <asp:TextBox id="txtAsunto" Width="150px" runat="server" CssClass="controls" /> 
        </div>
    </div>

    <%--Variables Ocultas--%>
    <asp:HiddenField ID="hdNroSolicitud" runat="server" />
    <asp:HiddenField ID="hdIdNotificacionMotivo" runat="server" />
    <asp:HiddenField ID="hdFechaNotificacion" runat="server" />
    


    <%--MENSAJE--%>
    <div class="control-group">
        <label class="control-label" for="txtMensaje">Mensaje</label>
        <div class="controls">
            <div id="editor"></div>        
<%--            <asp:TextBox id="txtMensaje" name="txtMensaje" runat="server"/>--%>
            <textarea id="txtMensaje" name="txtMensaje" runat="server" style="display:none;"></textarea>
        </div>
    </div>

    

    <script src="<%: ResolveUrl("~/Scripts/quill-textarea.js")%>" type="text/javascript"></script>



    <%-- Botones--%>
   <div class="control-group pull-right">
       <asp:Button ID="btnNotificar" runat="server" CssClass="btn btn-primary" ValidationGroup="caducar" OnClick="btnNotificar_OnClick" Text="Notificar Solicitud" />
       <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary" OnClick="btnLimpiar_OnClick" OnClientClick="LimpiarFormulario();">
           <i class="icon-refresh"></i>
           <span class="text">Limpiar</span>
       </asp:LinkButton>
       <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />
   </div>

    <%--Modal de Errores--%>
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
               <div class="modal-header">
                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                   <h4 class="modal-title">Error</h4>
               </div>
                <div class="modal-body">
                    <table style="border-collapse: separate;" border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updResultados" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" Style="color: black"></asp:Label>
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

    <%--Modal de Success--%>
    <div id="frmSuccess" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Éxito</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-ok-sign fs64" style="color: #67eb34"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updResultados2" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblSuccess" runat="server" Style="color: Black"></asp:Label>
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
    

        <!-- Initialize Quill editor -->
    <script>
        var toolbarOptions = [
            ['bold', 'italic', 'underline', 'strike'],        //cambiar el formato del texto
            ['blockquote'],                                   //hacer una cita con comillas
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],    //hacer listas
            [{ 'size': ['small', false, 'large', 'huge'] }],  //dropdown con tamaño de letra
            ['link', 'image'],                                //agrega link e imagen
            [{ 'color': [] }, { 'background': [] }],          //color y fondo de texto
            [{ 'font': [] }],                                 //tipo de letra
            [{ 'align': [] }],                                //alineacion de texto
            ['clean']                                         // borrar formato
        ];
        var quill = new Quill('#editor', {
            modules: {
                toolbar: toolbarOptions
            },
            theme: 'snow'
        });
        quill.on('text-change', function (delta, oldDelta, source) {
            document.getElementById('MainContent_txtMensaje').value = quill.root.innerHTML;            
        });
       
    </script>
    <script>
        //$(document).ready(function () {
        //    inicializar_controles();
        //});

        function LimpiarFormulario() {
            document.getElementById("MainContent_txtNroSolicitud").value = "";
        }

        function showfrmError() {
            $("#frmError").modal("show");
            return false;
        }

        function showfrmSuccess() {
            $("#frmSuccess").modal("show");
            return false;
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#frmError').on('click', function () {
                $(location).attr('href', "/Operaciones/ViewLayer/NotificacionesCaducidad.aspx");
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#frmSuccess').on('click', function () {
                $(location).attr('href',"/Operaciones/ViewLayer/NotificacionesCaducidad.aspx");
            });
        });
    </script>

</asp:Content>
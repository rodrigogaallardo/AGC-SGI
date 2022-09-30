<%@ Page 
    Title="Agregar archivos a una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="AgregarArchivo.aspx.cs"
    Inherits="SGI.Operaciones.AgregarArchivo"
%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>
    <script src="../Scripts/Select2-locales/select2_locale_es.js"></script>
    <script src="../Scripts/Funciones.js" type="text/javascript"></script>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>
    <%: Styles.Render("~/Content/themes/base/css") %>
    
    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup>

    <div class="control-group">
        <label class="control-label" for="txtSolicitud">N&uacute;mero de Solicitud:</label>
        <div class="controls">
            <asp:TextBox ID="txtSolicitud" runat="server" Editable="False" CssClass="controls"/>
        </div>
    </div>

    <hr/>

    <div class="control-group">
        <label class="control-label" for="lblTdocReq">Tipo de Documento Requerido:</label>
        <div class="controls">
            <asp:DropDownList id="dropDownListEditTipoDeDocumentoRequerido" runat="server"
                DataTextField="nombre_tdocreq" DataValueField="id_tdocreq" Width="40%" DataSourceID="SqlDataSourceTDocReq">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceTDocReq" runat="server" ConnectionString="<%$ ConnectionStrings:Cnn %>" SelectCommand="SELECT * FROM [TiposDeDocumentosRequeridos] ORDER BY [nombre_tdocreq]"></asp:SqlDataSource>
        </div>
    </div>

    <div class="control-group">
        <label class="control-label" for="lblTdocRecDetalle">Detalle Tipo Documento Requerido:</label>
        <div class="controls">
            <asp:TextBox ID="txtTdocRecDetalle" runat="server" MaxDataLength=50 CssClass="controls" Width="40%" style="padding-left:0px; padding-right:0px;"/>
        </div>
    </div>

    <div class="control-group">
        <label class="control-label" for="lblTipoDocSis">Tipo de Documento Sistema:</label>
        <div class="controls">
            <asp:DropDownList id="dropDownListEditTipoDeDocumentoSistema" runat="server"
                DataTextField="nombre_tipodocsis" DataValueField="id_tipdocsis" Width="40%" DataSourceID="SqlDataSourceTipoDocSis">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceTipoDocSis" runat="server" ConnectionString="<%$ ConnectionStrings:Cnn %>" SelectCommand="SELECT * FROM [TiposDeDocumentosSistema] ORDER BY [nombre_tipodocsis]"></asp:SqlDataSource>
        </div>
    </div>

    <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
        <ContentTemplate>
            <div>
                <asp:FileUpload ID="FileUpload1" runat="server" Width="40%"/>
                <br/>
                <br/>
            </div>
            <div id="pnlBotonesGuardar" class="control-group">
                <div class="controls">
                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary" OnClientClick="return validarGuardarNuevoArchivo();" OnClick="btnGuardar_Click">
                    <i class="imoon imoon-save"></i>
                    <span class="text">Actualizar</span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClick="btnCancelar_Click">
                    <i class="imoon imoon-blocked"></i>
                    <span class="text">Cancelar</span>
                    </asp:LinkButton>
                </div>
            </div>
            <div class="control-group">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
                    <ProgressTemplate>
                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading"/>
                        Guardando...
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
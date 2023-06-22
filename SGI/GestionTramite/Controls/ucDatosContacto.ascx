<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ucDatosContacto.ascx.cs"
    Inherits="SGI.GestionTramite.Controls.ucDatosContacto" %>

<%: Scripts.Render("~/bundles/fileUpload") %>
<%: Styles.Render("~/bundles/fileUploadCss") %>
<%: Scripts.Render("~/bundles/select2") %>
<%: Styles.Render("~/bundles/Select2Css") %>

<div class="accordion-group widget-box">
      <div class="accordion-heading">
        <a id="btnUpDown" data-parent="#collapse-group" href="#collapse_datos_cont"
            data-toggle="collapse" onclick="tda_btnUpDown_collapse_click(this)">
            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>
                    <asp:Label ID="titulo" runat="server" Text="Datos de Contacto"></asp:Label></h5>
                <span class="btn-right"><i class="icon-chevron-down"></i></span>
            </div>
        </a>
    </div>

    <div class="accordion-body collapse in" id="collapse_datos_cont">

        <div class="widget-content">
            <asp:HiddenField ID="hid_id_solicitud" runat="server" />

        <%--Datos de Tramite--%>
            <div style="padding-bottom: 10px">
                <asp:Panel ID="pnlDatosTramite" runat="server" Visible="true">
                    <strong>
                        <asp:Label ID="tituloPanel" runat="server" Text="Datos de Contacto del Tramite:"></asp:Label></strong>
                    <div style="display: flex; justify-content: center; padding-top: 10px" runat="server">

                        <%--Codigo de Area--%>
                        <asp:Label ID="lblCodigoArea" runat="server" AssociatedControlID="codigoArea" Text="Cod. Area:" CssClass="control-label" style="padding-top: 5px"></asp:Label>
                        <div class="controls" style="padding-right: 10px; padding-left: 10px">
                            <asp:TextBox ID="codigoArea" runat="server" Width="100px" Enabled="false" />
                        </div>

                        <%--Prefijo--%>
                        <asp:Label ID="lblPrefijo" runat="server" AssociatedControlID="prefijo" Text="Prefijo:" CssClass="control-label" style="padding-top: 5px"/>
                        <div class="controls" style="padding-left: 10px; padding-right: 10px">
                            <asp:TextBox ID="prefijo" runat="server" Width="100px" Enabled="false" />
                        </div>

                        <%--Sufijo--%>
                        <asp:Label ID="lblSufijo" runat="server" AssociatedControlID="sufijo" Text="Sufijo:" CssClass="control-label" style="padding-top: 5px"/>
                        <div class="controls" style="padding-left: 10px">
                            <asp:TextBox ID="sufijo" runat="server" Width="100px" Enabled="false" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <%--Datos de Contacto de Personas Fisicas--%>
            <div style="padding-bottom: 10px">
                <asp:Panel ID="pnlDatosFisicos" runat="server" Visible="true">
                    <strong>
                        <asp:Label ID="tituloPanel2" runat="server" Text="Datos de Contacto de Personas Fisicas:" /></strong>
                    <div style="display: flex; justify-content: center; padding-top: 10px" runat="server">

                        <%--Telefono Movil--%>
                        <asp:Label ID="lblTelefonoMovil" runat="server" AssociatedControlID="telefonoMovil" Text="Teléfono Móvil: " CssClass="control-label" style="padding-top: 5px"/>
                        <div class="controls" style="padding-right: 20px; padding-left: 20px">
                            <asp:TextBox ID="telefonoMovil" runat="server" Width="100px" Enabled="false" />
                        </div>

                        <%--Telefono Fijo--%>
                        <asp:Label ID="lblTelefonoFijo" runat="server" AssociatedControlID="telefonoFijo" Text="Teléfono: " CssClass="control-label" style="padding-top: 5px"/>
                        <div class="controls" style="padding-right: 20px; padding-left: 20px">
                            <asp:TextBox ID="telefonoFijo" runat="server" Width="100px" Enabled="false" />
                        </div>

                        <%--Email--%>
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="emailFisico" Text="Email: " CssClass="control-label" style="padding-top: 5px"/>
                        <div class="controls" style="padding-left: 20px">
                            <asp:TextBox ID="emailFisico" runat="server" Width="300px" Enabled="false" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

           <%--Datos de Contacto de Personas Juridicas--%>
            <asp:Panel ID="pnlDatosJuridico" runat="server" Visible="true">
               <strong><asp:Label ID="tituloPanel3" runat="server" Text="Datos de Contacto de Personas Juridicas:" /></strong>
                <div style="display:flex; justify-content: center; padding-top:10px" runat="server">

                    <%--Telefono Movil--%>
                    <asp:Label ID="lblTelJuridico" runat="server" AssociatedControlID="telJuridico" Text="Teléfono Móvil: " CssClass="control-label" style="padding-top: 5px"/>
                    <div class="controls" style="padding-right: 20px; padding-left: 20px">
                        <asp:TextBox ID="telJuridico" runat="server" Width="100px" Enabled="false" />
                    </div>
                    <%--Email--%>
                    <asp:Label ID="lblEmailJuridico" runat="server" AssociatedControlID="emailJuridico" Text="Email: " CssClass="control-label" style="padding-top: 5px"/>
                    <div class="controls" style="padding-left: 20px">
                        <asp:TextBox ID="emailJuridico" runat="server" Width="300px" Enabled="false" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

<script>
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

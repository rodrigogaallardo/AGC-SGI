<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCabecera.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucCabecera" %>

<asp:UpdatePanel ID="updHiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_solicitud" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<div class="widget-box">
    <div class="widget-title">
        <span class="icon"><i class="icon-list-alt"></i></span>
        <h5>Datos del Trámite</h5>
    </div>
    <div class="widget-content">
        <div>
            <div style="width: 45%; float: left">

                <ul class="cabecera">
                    <li>Solicitud:<strong><asp:Label ID="lblSolicitud" runat="server"></asp:Label></strong><strong style="color: #64b460"><asp:Label ID="lblExpediente" runat="server"></asp:Label></strong>
                    </li>
                    <li>Estado:<strong><asp:Label ID="lblEstado" runat="server"></asp:Label></strong>
                    </li>
                    <asp:Panel ID="pnlExpediente" runat="server">
                        <li>
                            <asp:Label ID="lblTextEncomienda" runat="server"></asp:Label>:<strong><asp:Label ID="lblEncomienda" runat="server"></asp:Label></strong>
                        </li>
                    </asp:Panel>
                    <asp:Panel ID="pnlTransmision" runat="server">
                        <li>
                            <strong>
                                <asp:Label ID="lblTipoTransm" runat="server"></asp:Label></strong>
                        </li>
                    </asp:Panel>
                    <asp:Panel ID="pnlSolicitudOrigen" runat="server" Visible="false">
                        <li>Solicitud de Origen:<strong><asp:Label ID="lblSoliictitudOrigen" runat="server"></asp:Label></strong>
                        </li>
                    </asp:Panel>
                    <asp:Panel ID="pnlLibradoUso" runat="server">
                        <li>Fecha de Librado al Uso:<strong><asp:Label ID="lblLibradoUso" runat="server"></asp:Label></strong>
                        </li>
                    </asp:Panel>
                </ul>

            </div>
            <asp:Panel ID="pnlUbicaciones" runat="server">
                <div style="width: 55%; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li>Ubicación:<strong><asp:Label ID="lblUbicacion" runat="server"></asp:Label></strong></li>
                        <li>Superficie total:<strong><asp:Label ID="lblSuperficieTotal" runat="server"></asp:Label></strong></li>
                    </ul>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMixturaDistrito" runat="server" Visible="false">
                <div style="width: auto; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li><span style="width: 50%">Area de Mixtura / Distrito de Zonificación:<strong><asp:Label ID="lblMixturaDistrito" runat="server"></asp:Label></strong></span></li>
                    </ul>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlZonaDeclarada" runat="server">
                <div style="width: auto; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li><span style="width: 50%">Zona declarada:<strong><asp:Label ID="lblZona" runat="server"></asp:Label></strong></span></li>
                    </ul>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlSubZonaDeclarada" runat="server">
                <div style="width: auto; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li><span style="width: 50%">SubZona:<strong><asp:Label ID="lblSubZona" runat="server"></asp:Label></strong></span></li>
                    </ul>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlZonaParcela" runat="server">
                <div style="width: auto; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li><span style="width: 50%">Zona parcela:<strong><asp:Label ID="lblZonaParcela" runat="server"></asp:Label></strong></span></li>
                    </ul>
                </div>
            </asp:Panel>
<%--            <asp:Panel ID="pnlZonaFrentista" runat="server">
                <div style="width: auto; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li><span style="width: 50%">Zona frentista:<strong><asp:Label ID="lblZonaFrentista" runat="server"></asp:Label></strong></span></li>
                    </ul>
                </div>
            </asp:Panel>--%>
            <asp:Panel ID="pnlDistritosEspeciales" runat="server">
                <div style="width: auto; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li><span style="width: 50%">Distritos especiales:<strong><asp:Label ID="lblDistritosEspeciales" runat="server"></asp:Label></strong></span></li>
                    </ul>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCPadron" runat="server" Visible="false">
                <div style="width: 50%; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li runat="server" id="lifecha">Fecha:<strong><asp:Label ID="lblFecha" runat="server"></asp:Label></strong>
                        </li>
                        <li>Nro. Expediente Ant.:
                            <strong>
                                <asp:Label ID="lblNroExpediente" runat="server"></asp:Label>
                                <asp:TextBox ID="txtNroExpediente" runat="server" Width="100px" Style="display: none"></asp:TextBox>
                                <asp:LinkButton ID="lnkNroExpEdit" runat="server" OnClientClick="return EditarExpediente(this);" data-group="controles-accion">
                                    <i class="imoon imoon-pencil2" title="Editar Nro Expediente" style="color:#377bb5;font-size:medium;margin-left:5px"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="lnkNroExpSave" runat="server" ToolTip="Guardar" Style="display: none" OnClick="lnkNroExpSave_Click">
                                            <i class="imoon imoon-ok color-green" style="font-size:medium;"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkNroExpClose" runat="server" ToolTip="Cancelar" Style="display: none" OnClientClick="return CloseEditarExpediente(this);">
                                            <i class="imoon imoon-close color-red" ></i>
                                </asp:LinkButton>
                            </strong>
                        </li>
                    </ul>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlHAB" runat="server" Visible="false">
                <div style="width: 50%; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li>Telefono:<strong><asp:Label ID="lblTelefono" runat="server"></asp:Label></strong>
                        </li>
                    </ul>
                </div>
            </asp:Panel>

            <br style="clear: both" />
            <asp:Panel ID="pnlTitulares" runat="server" Visible="false">
                <div style="width: 100%; float: left">
                    <ul class="cabecera">
                        <li>Titular/es:<strong><asp:Label ID="lblTitulares" runat="server"></asp:Label></strong>
                        </li>
                    </ul>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlTitularesTransf" runat="server" Visible="false">
                <div style="width: 45%; float: left">
                    <ul class="cabecera">
                        <li>Cedente/s:<strong><asp:Label ID="lblCedentes" runat="server"></asp:Label></strong>
                        </li>
                    </ul>
                </div>
                <div style="width: 55%; float: left">
                    <ul class="cabecera" style="padding-left: 10px">
                        <li>Cesionario/s:<strong><asp:Label ID="lblCesionarios" runat="server"></asp:Label></strong>
                        </li>
                    </ul>
                </div>
            </asp:Panel>
            <br style="clear: both" />

            <asp:Panel ID="pnlExpeSadeRelacionado" runat="server" Visible="false">
                <div style="width: 100%; float: left">
                    <ul class="cabecera">
                        <li>Expediente Relacionado:<strong><asp:Label ID="lblExpeSadeRelacionado" runat="server"></asp:Label></strong>
                        </li>
                    </ul>
                </div>
            </asp:Panel>
            <br style="clear: both" />

            <asp:Panel ID="pnlPresentacionAgreagr" runat="server" Visible="false">
                <div style="width: 100%; float: left">
                    <ul class="cabecera">
                        <li><strong style="color: #e82a16; font-weight: bold; font-size: 16px">Presentacion a agregar:
                            <asp:Label ID="lblPresentacionAgreagr" runat="server"></asp:Label></strong>
                        </li>
                    </ul>
                </div>
            </asp:Panel>
            <br style="clear: both" />
            <!-- limpia el float anterior -->
        </div>


    </div>
</div>


<script type="text/javascript">

    $(document).ready(function () {


    });

    function EditarExpediente(obj) {
        $("#<%: lnkNroExpEdit.ClientID %>").hide();
        $("#<%: lblNroExpediente.ClientID %>").hide();
        $("#<%: txtNroExpediente.ClientID %>").show();
        $("#<%: lnkNroExpSave.ClientID %>").show();
        $("#<%: lnkNroExpClose.ClientID %>").show();
        return false;
    }

    function CloseEditarExpediente(obj) {
        $("#<%: txtNroExpediente.ClientID %>").hide();
        $("#<%: lnkNroExpSave.ClientID %>").hide();
        $("#<%: lnkNroExpClose.ClientID %>").hide();
        $("#<%: lnkNroExpEdit.ClientID %>").show();
        $("#<%: lblNroExpediente.ClientID %>").show();
        return false;
    }
</script>

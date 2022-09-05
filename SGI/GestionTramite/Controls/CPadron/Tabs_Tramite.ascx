<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tabs_Tramite.ascx.cs" Inherits="SGI.GestionTramite.Controls.CPadron.Tabs_Tramite" %>

<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Ubicaciones.ascx" TagPrefix="uc1" TagName="Tab_Ubicaciones" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_DatosLocal.ascx" TagPrefix="uc1" TagName="Tab_DatosLocal" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Rubros.ascx" TagPrefix="uc1" TagName="Tab_Rubros" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Titulares.ascx" TagPrefix="uc1" TagName="Tab_Titulares" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_TitularesSolicitud.ascx" TagPrefix="uc1" TagName="Tab_TitularesSol" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_ConformacionesLocal.ascx" TagPrefix="uc1" TagName="Tab_ConformacionesLocal" %>
<%@ Register Src="~/GestionTramite/Controls/CPadron/Tab_Planos.ascx" TagPrefix="uc1" TagName="Tab_Planos" %>

<%--Información ingresada en el trámite--%>
<asp:UpdatePanel ID="updDatoExpediente" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
            <asp:Panel ID="pnlCPadron" runat="server"  data-group="controles-accion" class="widget-box">
	            <div class="widget-title">
                    <span class="icon"><i class="imoon-list-alt"></i></span>
                        <h5>Editar Nro. Expediente</h5>
                </div>
	            <div class="widget-content">
                    
                <div class="form-horizontal" >
                    Nro. Expediente Ant.:
                            <strong>
                                <asp:Label ID="lblNumeroExpediente" runat="server" style="margin-bottom:5px;margin-top:5px"></asp:Label>
                                <asp:TextBox ID="txtNumeroExpediente" runat="server" Width="100px" style="display:none"></asp:TextBox>
                                
                                <asp:LinkButton ID="lnkNroExpEdit" runat="server" OnClientClick="return EditarExpediente();" data-group="controles-accion" >
                                    <i class="imoon imoon-pencil2" title="Editar Nro Expediente" style="color:#377bb5;font-size:medium;margin-left:5px"></i>
                                </asp:LinkButton>
                                
                                        <asp:LinkButton ID="lnkNroExpSave" runat="server" ToolTip="Guardar" style="display:none" OnClick="lnkNroExpSave_Click" >
                                            <i class="imoon imoon-ok color-green" style="font-size:medium;"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkNroExpClose" runat="server" ToolTip="Cancelar" style="display:none" OnClientClick="return CloseEditarExpediente();" >
                                            <i class="imoon imoon-close color-red" ></i>
                                        </asp:LinkButton>
                                
                <asp:UpdateProgress ID="updprgressNroExp" runat="server" AssociatedUpdatePanelID="updDatoExpediente"   >
                    <ProgressTemplate>
                        <asp:Image ID="imgProgFinalizarTarea" runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif"  />
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                            </strong>
                </div>
                </div>
            </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="updDatoTramiteCP" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <div id="tabsCP" class="mtop20">
            <asp:HiddenField ID="hid_cpadron_id" runat="server" />
            <asp:HiddenField ID="hid_editar" runat="server" />
            <ul>
                <li><a href="#tabUbicaciones">Ubicaci&oacute;n</a></li>
                <li><a href="#tabDatosLocal">Datos del Local</a></li>
                <li><a href="#tabRubros">Rubros</a></li>
                <li><a href="#tabTitulares">Titulares</a></li>
                <li><a href="#tabTitularesSol">Titulares Solicitud</a></li>
                <li><a href="#TabConformacionesLocal">Conformaciones del Local</a></li>
            </ul>

            <div id="tabUbicaciones">
                <uc1:tab_ubicaciones runat="server" id="Tab_Ubicaciones" onubicacionactualizada="Tab_Ubicaciones_UbicacionActualizada" />
            </div>
            <div id="tabDatosLocal">
                <uc1:tab_datoslocal runat="server" id="Tab_DatosLocal" ondatoslocalactualizada="Tab_DatosLocal_DatosLocalActualizada" />
            </div>
            <div id="tabRubros">
                <uc1:tab_rubros runat="server" id="Tab_Rubros" onrubrosactualizado="Tab_Rubros_RubrosActualizado" />
            </div>
            <div id="tabTitulares">
                <uc1:tab_titulares runat="server" id="Tab_Titulares" />
            </div>
            <div id="tabTitularesSol">
                <uc1:tab_titularessol runat="server" id="Tab_TitularesSol" />
            </div>
            <div id="TabConformacionesLocal">
                <uc1:tab_conformacioneslocal runat="server" id="Tab_ConformacionesLocal" onplanosactualizado="Tab_ConformacionesLocal_ConformacionesLocalActualizado" />
            </div>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    $(document).ready(function () {
        toolTips_CPadron();
        $("#tabsCP").tabs({ selected: 0 });
    });

    function cargaTabs() {
        toolTips_CPadron();
        $("#tabsCP").tabs({ selected: 0 });
        return false;
    }

    function toolTips_CPadron() {
        $("[data-toggle='tooltip']").tooltip();
        return false;
    }

    function habilitarControles(habilitar) {
        debugger;
        if (habilitar) {
            $("[data-group='controles-accion']").show();
            $("input[type='text']").removeAttr('disabled');
            $("input[type='radio']").removeAttr('disabled');
            $("input[type='checkbox']").removeAttr('disabled');
        }
        else {
            $("[data-group='controles-accion']").hide();
            $("input[type='text']").attr('disabled', 'disabled');
            $("input[type='radio']").attr('disabled', 'disabled');
            $("input[type='checkbox']").attr('disabled', 'disabled');
        }
        habilitarControlesDatosLocal(habilitar);
        habilitarControlesRubros(habilitar);
        habilitarControlesUbicacion(habilitar);
        return false;
    }

    function visibleConformacionLocal(visible) {
        if (visible)
            $("#tabsCP").find("[aria-controls='TabConformacionesLocal']").show();
        else
            $("#tabsCP").find("[aria-controls='TabConformacionesLocal']").hide();
        return false;
    }

    function EditarExpediente() {
        $("#<%: lnkNroExpEdit.ClientID %>").hide();
        $("#<%: lblNumeroExpediente.ClientID %>").hide();
        $("#<%: txtNumeroExpediente.ClientID %>").show();
        $("#<%: lnkNroExpSave.ClientID %>").show();
        $("#<%: lnkNroExpClose.ClientID %>").show();
        return false;
    }

    function CloseEditarExpediente() {
        $("#<%: txtNumeroExpediente.ClientID %>").hide();
        $("#<%: lnkNroExpSave.ClientID %>").hide();
        $("#<%: lnkNroExpClose.ClientID %>").hide();
        $("#<%: lnkNroExpEdit.ClientID %>").show();
        $("#<%: lblNumeroExpediente.ClientID %>").show();
        return false;
    }
</script>

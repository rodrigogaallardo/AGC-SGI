<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResultadoTarea.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucResultadoTarea" %>


<div>
    <asp:HiddenField ID="hid_id_tramite_tarea" runat="server" />
    <asp:HiddenField ID="hid_id_tarea" runat="server" />
    <asp:HiddenField ID="hid_id_grupotramite" runat="server" />
    <asp:HiddenField ID="hid_alert_conf" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%: ddlResultado.ClientID %>").on("change", function (e) {
                $("#Req_ddlResultado").hide();
                hideSummary();
            });
        });

        function ConfirmFinalizar(obj) {
            
            var ret = false;
            
            $("#Req_ddlResultado").hide();
            if ($("#<%: ddlResultado.ClientID %>").val() == 0) {
                $("#Req_ddlResultado").css("display", "inline-block");
                return ret;
            }

            var conf = $('#<%=hid_alert_conf.ClientID%>');
            if (conf.val() == 'True')
                ret = confirm('¿Esta seguro que desea cerrar esta tarea?');
            else
                ret = true;
            if (ret) {
                $(obj).hide();
                $("#<%: btnGuardar.ClientID %>").hide();
            }

            return ret;
        }
    </script>
    <table border="0" style="vertical-align:middle; border-collapse:separate; border-spacing:5px">

        <tr>
            <td>
                <asp:Label ID="lblResultado" runat="server">Resultado:</asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="updResultado" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlResultado" runat="server" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlResultado_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="Req_ddlResultado" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                        Debe ingresar el resultado.
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblProximaTarea" runat="server">Pr&oacute;xima Tarea:</asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:DropDownList ID="ddlProximaTarea" runat="server" Width="400px">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    
    <div style="border-top: solid 1px #dddddd; padding-top:15px; padding-bottom:15px">

        <asp:UpdatePanel ID="updGuardarTarea" runat="server" RenderMode="Inline" class="pull-left">
            <ContentTemplate>
        
                <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" CssClass="btn pull-left" Text="Guardar"  />
                
                <asp:UpdateProgress ID="progGuardarTarea" runat="server" AssociatedUpdatePanelID="updGuardarTarea" class="pull-left">
                    <ProgressTemplate>
                        <asp:Image ID="imgProgGuardarTarea" runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif" />Procesando...
                    </ProgressTemplate>

                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
                
        <asp:UpdatePanel ID="updFinalizarTarea" runat="server" RenderMode="Inline" class="pull-left">
            <ContentTemplate>
                <asp:Button ID="btnFinalizarTarea" runat="server" OnClick="btnFinalizarTarea_Click" CssClass="btn btn-primary pull-left" Text="Finalizar tarea" 
                    Style="margin-left: 5px" OnClientClick="javascript:return ConfirmFinalizar(this);"/>

                <asp:UpdateProgress ID="progFinalizarTarea" runat="server" AssociatedUpdatePanelID="updFinalizarTarea" class="pull-left" >
                    <ProgressTemplate>
                        <asp:Image ID="imgProgFinalizarTarea" runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif" />Procesando...
                    </ProgressTemplate>

                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:Button ID="btnCerrar" runat="server" OnClick="btnCerrar_Click" CssClass="btn pull-right" Text="Cancelar" />
        <span class="clearfix"></span>

    </div>

</div>
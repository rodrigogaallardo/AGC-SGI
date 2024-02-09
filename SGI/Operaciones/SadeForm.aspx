<%@ Page
    Title="Administrar Expedientes Electronicos"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="SadeForm.aspx.cs"
    Inherits="SGI.Operaciones.SadeForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
    
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid" style="text-align: left;">
        <div class="form-container">
            <p class="lead">Solicitud</p>
            <label class="control-label" for="txtBuscarSolicitud">Buscar por Número de Solicitud</label><br />
            <div class="form-group">
                <asp:TextBox ID="txtBuscarSolicitud" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnBuscarSolicitud" runat="server" Text="Consultar Expediente Electrónico" OnClick="btnBuscarSolicitud_Click" CssClass="btn btn-primary" />
            </div>
            <asp:Panel class="accordion" runat="server" id="myAccordion">
    <div class="accordion-group">
        <div class="accordion-heading">
            <asp:Label runat="server" class="accordion-toggle" data-toggle="collapse" data-parent="#myAccordion" href="#collapseExpedienteElectronicoData">
                Mostrar Detalles Expediente Electrónico <i class="icon-chevron-down"></i>
            </asp:Label>
        </div>
        <div id="collapseExpedienteElectronicoData" class="accordion-body collapse in">
             <asp:Panel ID="viewValorExpediente" runat="server" class="span6" Visible="false">
                <div class="panel">
                <table class="table table-bordered table-striped custom-table">
                     <tr>
                         <td style="width: 30%;"><label class="control-label" for="txtExpedienteElectronico">Expediente Electronico:</label></td>
                         <td><asp:Label ID="txtExpedienteElectronicoValor" runat="server" CssClass="control-label"></asp:Label></td>
                     </tr>
                     <tr>
                         <td><label class="control-label" for="txtEstado">Estado:</label></td>
                         <td><asp:Label ID="txtEstadoValor" runat="server" CssClass="control-label"></asp:Label></td>
                     </tr>
                     <tr>
                         <td><label class="control-label" for="txtUsuarioCaratulador">Usuario Caratulador:</label></td>
                         <td><asp:Label ID="txtUsuarioCaratuladorValor" runat="server" CssClass="control-label"></asp:Label></td>
                     </tr>
                     <tr>
                         <td><label class="control-label" for="txtUsuario">Usuario:</label></td>
                         <td><asp:Label ID="txtUsuarioValor" runat="server" CssClass="control-label"></asp:Label></td>
                     </tr>
                     <tr>
                         <td><label class="control-label" for="txtReparticion">Reparticion:</label></td>
                         <td><asp:Label ID="txtReparticionValor" runat="server" CssClass="control-label"></asp:Label></td>
                     </tr>
                     <tr>
                         <td><label class="control-label" for="txtSector">Sector:</label></td>
                         <td><asp:Label ID="txtSectorValor" runat="server" CssClass="control-label"></asp:Label></td>
                     </tr>
                     <tr>
                        <td><label class="control-label" for="txtSector">Bloqueado:</label></td>
                        <td><asp:Label ID="txtBloqueado" runat="server" CssClass="control-label"></asp:Label></td>
                    </tr>
                 </table>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Panel>
        </div>
        <!-- por ahora saco esto
            <asp:Repeater ID="txtBoxRepeater" runat="server">
                <ItemTemplate>
                    <asp:TextBox ID="txtBoxSolicitud" runat="server"></asp:TextBox>
                    <br />
                </ItemTemplate>
            </asp:Repeater> 
            <asp:Button ID="AddTextBoxButton" runat="server" Text="Agregar" OnClick="AddTextBoxButton_Click" />-->
        
            <div style="margin-top: 20px; margin-bottom: 20px;">
                <asp:Button ID="btnEjecutarPaquete" runat="server" Text="Ejecutar Paquete" OnClick="btnProcesar_Click" CssClass="btn btn-small" />
                <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" OnClick="btnDesbloquear_Click" CssClass="btn btn-small" />
                <asp:Button ID="btnBloquear" runat="server" Text="Bloquear" OnClick="btnBloquear_Click" CssClass="btn btn-small" />
                <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" CssClass="btn btn-small" />
            </div>




        <asp:Button ID="btnTogglePasesList" runat="server" Text="Toggle Drop Down List" CssClass="btn btn-small" OnClientClick="togglePasesList(); return false;" />

        <asp:Panel ID="PanelPases" runat="server" class="span3" style="display: none;">
            <label class="alert-danger"> Esta funcion esta en BETA, no usar sin soporte tecnico de BIWINI</label>
            <div class="additional-options">
                <div class="panel">
                    <asp:Panel ID="viewDropDownList" runat="server" class="span3" Visible="false">
                            <label class="control-label" for="txtBuscarSolicitud">Usuario SADE</label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlUsuario" runat="server" Width="150px"></asp:DropDownList>
                                <asp:HiddenField ID="hid_paquete" runat="server"/>
                                <asp:HiddenField ID="hid_ExpedienteElectronico" runat="server"/>
                            </div>
                        <div>
                            <div style="display: flex; justify-content: space-between;">
                                <asp:RadioButton ID="rdoUser" runat="server" Text="Usuario" GroupName="RadialGroup"></asp:RadioButton>
                                <asp:RadioButton ID="rdoGroup" runat="server" Text="Grupo" GroupName="RadialGroup"></asp:RadioButton>
                                <asp:CheckBox ID="chkboxDesbloqueo" runat="server" Text="Grupo" />
                                <div>
                                    <label for="textBoxDestino">Sector Destino:</label>
                                    <asp:TextBox ID="textBoxDestino" runat="server"></asp:TextBox>
                                </div>
                                <div>
                                    <label for="textBoxReparticionDestino">Reparticion Destino:</label>
                                    <asp:TextBox ID="textBoxReparticionDestino" runat="server"></asp:TextBox>
                                </div>
                                <div>
                                    <label for="textBoxEstadoSade">Estado SADE:</label>
                                    <asp:TextBox ID="textBoxEstadoSade" runat="server"></asp:TextBox>
                                </div>
                                <div>
                                    <label for="textBoxUserDestino">Usuario Destino:</label>
                                    <asp:TextBox ID="textBoxUsuarioDestino" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <asp:Button ID="btnRealizarPase" runat="server" Text="Realizar Pase" CssClass="btn btn-small" OnClick="btnPasear_Click" />
            </div>
        </asp:Panel>
        
    
        
    </div>

    <div class="result-container" style="margin-top: 20px; margin-bottom: 20px; padding: 10px; border: 1px solid #ccc; border-radius: 5px;">
        <asp:Label ID="lblResult" runat="server" Text="Resultado" CssClass="alert-success" ></asp:Label>
        <asp:Label ID="lblError" runat="server" Text="Errores" Visible="false" CssClass="alert-error"></asp:Label>
    </div>


    <script type="text/javascript">
        function togglePasesList() {
            var dropDownList = document.getElementById('<%= PanelPases.ClientID %>');
            if (dropDownList.style.display === "none") {
                dropDownList.style.display = "block";
            } else {
                dropDownList.style.display = "none";
            }
        }
    </script>
</asp:Content>
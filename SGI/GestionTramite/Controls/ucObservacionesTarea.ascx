<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucObservacionesTarea.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucObservacionesTarea" %>

<style type="text/css">

    textarea
    {
        width:98%;
        height:60px;
    }
</style>

<div>

    <div class="control-group">
        <asp:Label ID="lblObservacion" runat="server" Text="Observaciones"
            class="control-label" AssociatedControlID="txtObservaciones"></asp:Label>
        <div class="controls">
            <asp:UpdatePanel ID="updPnlObservaciones" runat="server"  updatemode ="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtObservaciones" runat="server" TextMode ="MultiLine" Width="100%" Height="80px"></asp:TextBox>
                      </ContentTemplate>
    </asp:UpdatePanel>      
            <div class="req">
                <asp:RequiredFieldValidator ID="rfv_txtObservaciones" runat="server"
                    ControlToValidate="txtObservaciones" CssClass="field-validation-error" 
                    ErrorMessage="Ingrese observación" ValidationGroup="noValidar"
                    Display="Dynamic">

                </asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
</div>
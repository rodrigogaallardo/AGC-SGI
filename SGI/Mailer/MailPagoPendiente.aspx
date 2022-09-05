<%@ Page Title="Trámite con Pago Pendiente" Language="C#" MasterPageFile="~/Mailer/Mail.Master" 
    AutoEventWireup="true" CodeBehind="MailPagoPendiente.aspx.cs" Inherits="SGI.Mailer.MailPagoPendiente" %>


<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:FormView ID="FormView1" runat="server" ItemType="SGI.Model.MailPagoPendiente" SelectMethod="GetData">
        <ItemTemplate>
    
            <table border="0" id="templateBody" style="padding-top: 20px; border-collapse: collapse; border-spacing: 0;">
                <tbody>
                    <tr >
                        <td style="text-align:center;vertical-align:top;padding-bottom: 40px; border-collapse: collapse">
                            <table border="0" style="border-collapse: collapse; border-spacing: 0; width:100%">
                                <tbody>
                                    <tr>
                                        <td style="padding-right: 40px; padding-left: 40px; border-collapse: collapse; text-align:center; vertical-align:top">
                                            <table border="0" style="border-collapse: collapse; border-spacing: 0; width: 100%" >
                                                <tbody>
                                                    <tr>
                                                        <td class="upperBodyContent" style="border-collapse: collapse; color: #43404D; font-family: Georgia; font-size: 16px; line-height: 150%; text-align: left; padding-top:10px; vertical-align:top">
                                                            <h2>Sr: <%# Item.Nombre  %></h2>
                                                            <h3>Le comunicamos que el tr&aacute;mite <%# Item.NumeroSolicitud  %> se encuentra Pendiente de Pago.</h3> 
                                                            <h3>Para acceder a la misma ingrese al Sistema de Solicitudes Inicio de tr&aacute;mite.</h3>
                                                            <h4><em style="color:green"><%# Item.UrlLogin %></em></h4>
                                                        </td>
                                                        <td style="border-collapse: collapse; width:40px">
                                                            <br>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
    </table>
        </ItemTemplate>

    </asp:FormView>

</asp:content>


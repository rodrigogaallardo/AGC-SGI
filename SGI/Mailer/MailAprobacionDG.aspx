<%@ Page Title="Trámite con Pago Pendiente" Language="C#" MasterPageFile="~/Mailer/Mail.Master" 
    AutoEventWireup="true" CodeBehind="MailAprobacionDG.aspx.cs" Inherits="SGI.Mailer.MailAprobacionDG" %>


<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:FormView ID="FormView1" runat="server" ItemType="SGI.Model.MailAprobacionDG" SelectMethod="GetData">
        <ItemTemplate>
    
            <table border="0" id="templateBody" style="padding-top: 20px; border-collapse: collapse; border-spacing: 0;">
                <tbody>
                    <tr >
                        <td style="text-align:center;vertical-align:top;padding-bottom: 40px; border-collapse: collapse">
                            <table border="0" style="border-collapse: collapse; border-spacing: 0; width:100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <div style="margin-left: 40px; margin-top:20px">
                                                
                                                <h4><em><%# Item.Renglon1 %></em></h4>
                                                <h4><em style="color:green"><%# Item.UrlLogin %></em></h4>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 40px; padding-left: 40px; border-collapse: collapse; text-align:center; vertical-align:top">
                                            <table border="0" style="border-collapse: collapse; border-spacing: 0; width: 100%" >
                                                <tbody>
                                                    <tr>
                                                        <td class="upperBodyContent" style="border-collapse: collapse; color: #43404D; font-family: Georgia; font-size: 16px; line-height: 150%; text-align: left; padding-top:10px; vertical-align:top">
                                                            <h2>Sr: <%# Item.Nombre  %></h2>
                                                            <h3>Le comunicamos que se encuentra aprobado su tr&aacute;mite <%# Item.NumeroSolicitud  %>.</h3> 
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


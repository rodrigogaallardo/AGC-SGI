<%@ Page Title="Trámite Generico" Language="C#" MasterPageFile="~/Mailer/Mail.Master" AutoEventWireup="true" CodeBehind="MailCambioEstado.aspx.cs" Inherits="SGI.Mailer.MailGenerico" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
   
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
                                                    <h2>Sr. Contribuyente,</h2>
                                                    <h3>Su solicitud ha cambiado de estado a "En Tr&aacute;mite".</h3> 
                                                    <h3>La informaci&oacute;n en su c&oacute;digo QR ha sido actualizada.</h3>
                                                </td>
                                                <td style="border-collapse: collapse; width:40px">
                                                    <br/>
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
</asp:content>

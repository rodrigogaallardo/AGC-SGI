<%@ Page Title="Notificación de Caratula" Language="C#" MasterPageFile="~/Mailer/Mail.Master" AutoEventWireup="true" CodeBehind="MailCaratula.aspx.cs" Inherits="SGI.Mailer.MailCaratula" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:FormView ID="FormView1" runat="server" ItemType="SGI.Model.MailCaratula" SelectMethod="GetData">
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
                                                            <h3>Le comunicamos que se encuentra disponible la Caratula del tr&aacute;mite <%# Item.NumeroSolicitud  %>.</h3> 
                                                            <h3>Para acceder a la misma ingrese al Sistema de Solicitudes Inicio de Tr&aacute;mite.</h3>
                                                            <br />
                                                            <h3>Se le hace saber que dispone de un plazo de diez d&iacute;as h&aacute;biles a los efectos de concurrir a la Mesa de Ayuda</h3>
                                                            <h3>y Atenci&oacute;n al P&uacute;blico de la Agencia Gubernamental de Control, muñido de una copia del certificado de Habilitaci&oacute;n,</h3>
                                                            <h3>a los efectos de protocolizar el libro de asiento de inspecciones, conforme lo establecido por el Art&iacute;culo 10</h3>
                                                            <h3>del Decreto N° 93/GCABA/06, bajo apercibimiento de archivo.</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-collapse: collapse; width:40px">
                                                            <br>
                                                            <h4><em style="color:green"><%# Item.UrlLogin %></em></h4>
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


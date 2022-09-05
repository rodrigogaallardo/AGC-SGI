<%@ Page Title="Recupero de contraseña" Language="C#" MasterPageFile="~/Mailer/Mail.Master" AutoEventWireup="true" CodeBehind="MailPassRecovery.aspx.cs" Inherits="SGI.Mailer.MailPassRecovery" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:FormView ID="FormView1" runat="server" ItemType="SGI.Model.MailPassRecovery" SelectMethod="GetData">
        <ItemTemplate>

            <div style="min-height: 500px;">
                <h2>
                    <span id="lblTitulo" class="color:#333;">Estimada/o: <%#Item.Nombre %></span>
                    <br />
                    <br />
                </h2>
                <p style="line-height: 35px; font-size: 17px">
                    Bienvenida/o al <strong style="font-size: 18px"><%#Item.ApplicationName %></strong> de la Agencia Gubernamental de Control (AGC).
                    <br />
                    A continuaci&oacute;n detallamos la informaci&oacute;n que Usted necesitar&aacute; para utilizar el sistema:
                    <br />
                    <br />
                    <strong style="font-size: 18px">Acceso al <%#Item.ApplicationName %></strong>
                    <br />
                    Direcci&oacute;n web del Sistema: <a href='<%# Item.UrlPage %>' style="font-size: 18px"><%# Item.UrlPage %></a>
                    <br />
                    Su nombre de usuario: <strong style="font-size: 18px"><%#Item.Username %></strong><br />
                    Su password: <strong style="font-size: 18px"><%#Item.Password %></strong>
                </p>
                <br />
                <br />
                <p style="line-height: 35px; font-size: 17px">
                    <strong style="font-size: 18px">Operatoria General</strong><br />
                    Este mensaje, al igual que las dem&aacute;s notificaciones que Usted reciba de ahora en m&aacute;s desde el <strong style="font-size: 18px"><%#Item.ApplicationName %></strong>, ser&aacute;n enviadas
autom&aacute;ticamente por el sistema. No responda esos mensajes.
                    <br />
                    <br />
                    Si quiere acceder al manual de usuario puede hacer clic en el siguiente link: <a href='<%# Item.UrlPage %>' style="font-size: 18px"><%# Item.UrlPage %></a>
                    <br />
                    Si Usted desea efectuar una consulta, realizar una solicitud de soporte o informar un evento, puede hacerlo de las siguientes formas:
                    <br />
                    1. Llamando telef&oacute;nicamente a la Mesa de Ayuda, al (11) 48605200
interno: 6333
                    <br />
                    2. Enviando un mail a la Mesa de Ayuda a la direcci&oacute;n de correo: <a href="mailto:soporte_agc@buenosaires.gob.ar">soporte_agc@buenosaires.gob.ar</a>
                    En respuesta Usted recibir&aacute; un mensaje confirmando la recepci&oacute;n del correo y un n&uacute;mero de Ticket asociado. De all&iacute; en m&aacute;s, el
seguimiento y la comunicaci&oacute;n que la Mesa de Ayuda mantendr&aacute; con Usted ser&aacute; efectuada utilizando ese Ticket como referencia del
caso.
                    <br />
                    <br />
                    <br />
                    Cordialmente,
                    <br />
                    <strong style="font-size: 18px">Agencia Gubernamental de Control</strong>

                </p>

                <br />
                <br />

            </div>
            <%--<table border="0" id="templateBody" style="padding-top: 20px; border-collapse: collapse; border-spacing: 0; width: 600px">
                <tbody>
                    <tr >
                        <td style="text-align:center;vertical-align:top;padding-bottom: 40px; border-collapse: collapse;">
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
                                                            <h2>Datos de acceso</h2>
                                                            <h3>Usuario: <%# Item.Username  %></h3>
                                                            <h3>Password: <%# Item.Password %></h3>
                                                        </td>
                                                        <td style="border-collapse: collapse; width:40px">
                                                            <br>
                                                        </td>
                                                    </tr>
                                                    <tr style="padding-right: 40px; padding-left: 40px; border-collapse: collapse; vertical-align:top;text-align:center">
                                                        <td style="border-collapse: collapse; vertical-align:top">
                                                            <div class="upperBodyContent" style="color: #43404D; font-family: Georgia; font-size: 16px; line-height: 150%; text-align: left;">
                                                            </div>
                                                            <br>
                                                            <table border="0" class="upperTemplateButton" style="-moz-border-radius: 5px; -webkit-border-radius: 5px; background-color: #ED5E29; border-radius: 5px; border-collapse: collapse; border-spacing: 0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="upperTemplateButtonContent" style="text-align:center;vertical-align:middle;padding-top: 15px; padding-right: 20px; padding-bottom: 15px; padding-left: 20px; border-collapse: collapse; color: #FFFFFF; font-family: Georgia; font-size: 31px; font-weight: normal; text-align: center; text-decoration: none;">
                                                                            <a href="<%# Item.UrlLogin %>" target="_blank" style="color: #FFFFFF; font-family: Georgia; font-size: 31px; font-weight: normal; text-align: center; text-decoration: none;"><em>Iniciar sesi&oacute;n</em></a>
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
                        </td>
                    </tr>
                </tbody>
    </table>--%>
        </ItemTemplate>

    </asp:FormView>

</asp:Content>


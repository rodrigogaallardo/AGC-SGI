<%@ Page Title="Activación de usuario" Language="C#" MasterPageFile="~/Mailer/Mail.Master" AutoEventWireup="true" CodeBehind="MailWelcome.aspx.cs" Inherits="SGI.Mailer.MailWelcome1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:FormView ID="FormView1" runat="server" ItemType="SGI.Model.MailWelcome" SelectMethod="GetData">
        <ItemTemplate>
            <div style="min-height: 500px;">
                <h2>
                    <span id="lblTitulo" class="color:#333;">Estimada/o: <%#Item.Nombre %></span>
                </h2>
                <br />
                <br />
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
                    Este mensaje, al igual que las dem&aacute;s notificaciones que Usted reciba de ahora en m&aacute;s desde el <strong style="font-size: 18px"><%#Item.ApplicationName %></strong>, 
                    ser&aacute;n enviadas autom&aacute;ticamente por el sistema. No responda esos mensajes.
                    <br />
                    <br />
                    Si quiere acceder al manual de usuario puede hacer clic en el siguiente link: <a href='<%# Item.UrlPage %>' style="font-size: 18px"><%# Item.UrlPage %></a>
                    <br />
                    Si Usted desea efectuar una consulta, realizar una solicitud de soporte o informar un evento, puede hacerlo de las siguientes formas:
                    <br />
                    1. Llamando telef&oacute;nicamente a la Mesa de Ayuda, al (11) 48605200 interno: 6333
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
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
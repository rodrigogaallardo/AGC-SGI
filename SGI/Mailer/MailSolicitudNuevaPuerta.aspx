<%@ Page Title="" Language="C#" MasterPageFile="~/Mailer/Mail.Master" AutoEventWireup="true" CodeBehind="MailSolicitudNuevaPuerta.aspx.cs" Inherits="SGI.Mailer.MailSolicitudNuevaPuerta" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:FormView ID="FormView1" runat="server" ItemType="SGI.Model.MailSolicitudNuevaPuerta" SelectMethod="GetData">
        <ItemTemplate>

            <div style="margin-left: 20px; margin-top:-20px">

                <h3>Datos del usuario</h3>
                <div>
                    <label>Usuario:</label><label><strong style="padding-left:5px;"><%# Item.Username %></label></strong> 
                </div>
                <div>
                    <label>Apellido/s:</label><label><strong style="padding-left:5px;"><%# Item.Apellido %></label></strong> 
                </div>
                <div>
                    <label>Nombre/s:</label><label><strong style="padding-left: 5px;"><%# Item.Nombre %></label></strong> 
                </div>
                <div>
                    <label>Email:</label><label><strong style="padding-left: 5px;"><%# Item.Email %></label></strong> 
                </div>

                <h3>Datos de la ubicaci&oacute;n</h3>

                <div>
                    <label>Nro. de partida matriz:</label><label><strong style="padding-left: 5px;"><%# Item.NroPartidaMatriz %></label></strong> 
                </div>
                <div>
                    <label>Secci&oacute;n:</label><label><strong style="padding-left: 5px;"><%# Item.Seccion %></label></strong> 
                </div>
                <div>
                    <label>Manzana:</label><label><strong style="padding-left: 5px;"><%# Item.Manzana %></label></strong> 
                </div>
                <div>
                    <label>Parcela:</label><label><strong style="padding-left: 5px;"><%# Item.Parcela %></label></strong> 
                </div>

                <h3>Datos de la puerta solicitada</h3>

                <div>
                    <label>Nombre de la calle:</label><label><strong style="padding-left: 5px; color:green">:Calle:</label></strong> 
                </div>
                <div>
                    <label>N&uacute;mero de puerta:</label><label><strong style="padding-left: 5px; color:green">:NroPuerta:</label></strong> 
                </div>

                <div style="margin-top: 5px">
                    <strong>Foto:</strong>
                </div>
                <div style="margin-top: 10px">
                    <img src='<%# Item.urlFoto %>' alt="" />
                </div>
                <div>
                    <small><a href='<%# Item.urlFoto %>'><%# Item.urlFoto %></a></small>
                </div>

                <div style="margin-top: 5px">
                    <strong>Mapa:</strong>
                </div>
                <div style="margin-top: 10px">
                    <img src='<%# Item.UrlMapa %>' alt="" />
                </div>
                <div>
                    <small><a href='<%# Item.UrlMapa %>'><%# Item.UrlMapa %></a></small>
                </div>
            </div>

        </ItemTemplate>

    </asp:FormView>

</asp:Content>

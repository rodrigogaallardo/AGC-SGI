<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TramiteQr.aspx.cs" Inherits="SGI.Tramite.TramiteQr" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Encomienda</title>
    <webopt:BundleReference ID="BundleReference1" runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <meta name="ROBOTS" content="NOINDEX, NOFOLLOW" />
</head>

<body style="margin:0;background-color: white">
    <form id="form1" runat="server">
        
        
        <div style="padding: 13px 0px 13px 0px">
            <img src="<%: ResolveClientUrl("~/Content/img/app/LOGO AGC.png")  %>" />
        </div>
        <div style="padding-left:10px; border-top: solid 1px gray;">
            <asp:HiddenField runat="server" ID="hdfid_solicitud" />
            <asp:HiddenField runat="server" ID="hdfid_tarea" />
            <table>
                <tr>
                    <td>Nº de Expediente:</td>
                    <td colspan="3">
                       <b><asp:Label ID="lblNroExpediente" runat="server"></asp:Label></b>
                    </td>
                </tr>
            </table>
            <div>
                <table>
                    <tr>
                        <td style="vertical-align:top">
                            Titular/es:
                        </td>
                        <td>
                            <table>
                                <asp:Repeater ID="repeater_titulares" runat="server" >
                                    <ItemTemplate>
                                        <tr>
                                            <td style="padding:0">
                                                <b><span class="text"><%# ( Eval("TipoPersona").Equals("PF") ? Eval("Apellido") + ", " + Eval("Nombres") : Eval("RazonSocial")) %></span></b>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>        
            <div>
                <table>
                    <tr>
                        <td>Secci&oacute;n:</td>
                        <td>
                            <asp:Label ID="lblSeccion" runat="server"></asp:Label>
                        </td>
                        <td>Manzana:</td>
                        <td>
                            <asp:Label ID="lblManzana" runat="server"></asp:Label>
                        </td>
                        <td>Parcela:</td>
                        <td>
                            <asp:Label ID="lblParcela" runat="server"></asp:Label>
                        </td>
                        <td>Partida Matriz:</td>
                        <td>
                            <asp:Label ID="lblPartidaMatriz" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Domicilio/s:</td>
                        <td colspan="7">
                            <b><asp:Label ID="lblDomicilio" runat="server"></asp:Label></b>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:GridView ID="grdRubros" runat="server" AutoGenerateColumns="false"
                AllowPaging="false" style="padding-right:20px">
                <Columns>
                    <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-CssClass="celda_center" HeaderStyle-CssClass="celda_center"/>
                    <asp:BoundField DataField="desc_rubro" HeaderText="Descripción"  ItemStyle-CssClass="celda_left" HeaderStyle-CssClass="celda_left" />
                    <asp:BoundField DataField="DocRequerida" HeaderText="Tipo" ItemStyle-CssClass="celda_center" HeaderStyle-CssClass="celda_center" />
                    <asp:BoundField DataField="SuperficieHabilitar" HeaderText="Superficie" ItemStyle-CssClass="celda_center" HeaderStyle-CssClass="celda_center" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="titulo-4">
                        No se ingresaron datos
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <div>
                <h5>
                <asp:LinkButton ID="linkDisposicion" runat="server" Text="Ver Disposicion" OnClick="linkDisposicion_Click"></asp:LinkButton>
                </h5>
            </div>
        </div>
       
    </form>
</body>
</html>

<%@ Page Title="ImprimirDispoHtmlNuevoCur" Language="C#" MasterPageFile="~/Reportes/Reporte.Master" AutoEventWireup="true" CodeBehind="ImprimirDispoHtmlNuevoCur.aspx.cs" Inherits="SGI.Reportes.ImprimirDispoHtmlNuevoCur" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <webopt:BundleReference ID="BundleReference1" runat="server" Path="~/Content/css" />
    <style type="text/css">
        .celda_center {
            text-align: center;
        }

        .celda_left {
            text-align: left;
            padding-left: 10px;
        }

        body {
            background-color: white;
        }

        .table th, .table td {
            padding: 8px;
            line-height: 20px;
            text-align: left;
            vertical-align: top;
            border-top: 0px solid #dddddd;
        }
    </style>
    <table class="table table-condensed" style="width: 100%">
        <tr>
            <td colspan="8" style="text-align: center">
                <h4>DIRECCI&Oacute;N GENERAL DE HABILITACIONES Y PERMISOS</h4>
            </td>
        </tr>
    </table>
    <br />
    <table class="table table-condensed">
        <tr>
            <td colspan="8">C. EXPEDIENTE N&#186:
               <asp:Label ID="lblNroExpediente" runat="server"></asp:Label>
            </td>
        </tr>

        <tr>
            <td colspan="8">SOLICITUD N&#186:
                <asp:Label ID="lblNroSolicitud" runat="server"></asp:Label>
            </td>
        </tr>

        <tr>
            <td colspan="8" style="text-align: right">Buenos Aires,
                <asp:Label ID="lblDia" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="8">VISTO lo solicitado en el expediente de referencia, y;</td>
        </tr>
    </table>

    <table class="table table-condensed">
        <tr>
            <td colspan="8">
                <div>
                    CONSIDERANDO:
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="8" style="text-align: justify">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>
                    Por ello y en uso de las facultades que le son propias,
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>EL DIRECTOR GENERAL DE HABILITACIONES Y PERMISOS</div>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>
                    DISPONE:
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>
                    <asp:Label ID="Label2" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
       
        <tr>
            <td colspan="8">
                <div>
                    Tr&aacute;mite
                    <asp:Label ID="Label3" runat="server"></asp:Label>:
                </div>
            </td>
        </tr>
    </table>   
    

    <asp:Panel ID="pnlDatosSolicitud" runat="server">

        <div style="background: #efefef">
            <table class="table table-condensed">
                <tr>
                    <td style="width: 40%">
                        <asp:Label ID="lblCiudResp" runat="server" Text=""></asp:Label>
                    </td>
                    <td colspan="7">
                        <table>
                            <asp:Repeater ID="repeater_titulares" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="padding: 0; display: <%# ( Eval("MuestraEnPlancheta").Equals(1) ? "block" : "none") %>">
                                            <span class="text"><%# ( Eval("TipoPersona").Equals("PF") ? Eval("Apellido") + ", " + Eval("Nombres") : Eval("RazonSocial")) %></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <asp:Table runat="server" ID="tblUbicaciones" CssClass="table table-condensed">
        </asp:Table>

        <table class="table table-condensed">
            <tr>
                <td colspan="8">
                    <asp:Label ID="lblPlantashabilitar" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

        <asp:Panel ID="pnlZonificacionFrentista" runat="server">
            <table class="table table-condensed">

                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblZonaDeclarada" runat="server"></asp:Label>
                    </td>
                    <td colspan="2">Cantidad de Operarios:
                        <asp:Label ID="lblCantOperarios" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <div>
            <table class="table table-condensed">
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblSuperficieTotal" runat="server"></asp:Label>
                    </td>
                    <td colspan="2">Calificador:
                        <asp:Label ID="lblCalificar" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />

        <asp:GridView ID="grdRubros" runat="server" AutoGenerateColumns="false" AllowPaging="false">

            <AlternatingRowStyle BackColor="#efefef" />
            <Columns>
                <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="80px" ItemStyle-CssClass="celda_center" HeaderStyle-CssClass="celda_center" />
                <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" ItemStyle-CssClass="celda_left" HeaderStyle-CssClass="celda_left" />
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
        <asp:GridView ID="grdSubRubros" runat="server" AutoGenerateColumns="false" AllowPaging="false" Visible="false">

            <AlternatingRowStyle BackColor="#efefef" />
            <Columns>
                <asp:BoundField DataField="CodigoRubro" HeaderText="Rubro" ItemStyle-Width="80px" ItemStyle-CssClass="celda_center" HeaderStyle-CssClass="celda_center" />
                <asp:BoundField DataField="Nombre" HeaderText="Detalle" ItemStyle-CssClass="celda_left" HeaderStyle-CssClass="celda_left" />

            </Columns>
            <EmptyDataTemplate>
                <div class="titulo-4">
                    No se ingresaron datos
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <asp:GridView ID="grdDepositos" runat="server" AutoGenerateColumns="false" AllowPaging="false" Visible="false">

            <AlternatingRowStyle BackColor="#efefef" />
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" ItemStyle-Width="80px" ItemStyle-CssClass="celda_center" HeaderStyle-CssClass="celda_center" />
                <asp:BoundField DataField="Descripcion" HeaderText="Depósito" ItemStyle-CssClass="celda_left" HeaderStyle-CssClass="celda_left" />

            </Columns>
            <EmptyDataTemplate>
                <div class="titulo-4">
                    No se ingresaron datos
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>

     <asp:Panel ID="pnlObservacion" runat="server">
        <table class="table table-condensed">
            <tr>
                <td style="vertical-align: top"><b>Observaciones:</b></td>
                <td>
                    <asp:Label ID="lblObservacion" runat="server"></asp:Label>
                </td>
            </tr>

        </table>
    </asp:Panel>
    <br />
      <asp:Panel ID="PanelArtAprobadoReconsideracion" runat="server" Visible ="false">
        <table class="table table-condensed">
            <tr>
                <td colspan="8">
                    <asp:Label ID="Label8" runat="server"></asp:Label>
                </td>
            </tr>
          
        </table>
    </asp:Panel>


    <br />
    <asp:Panel ID="pnlArt2" runat="server">
        <table class="table table-condensed">
            <tr>
                <td style="text-align: justify">
                    <asp:Label ID="Label4" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: justify">
                    <asp:Label ID="Label5" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: justify">
                    <asp:Label ID="Label6" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <br />

</asp:Content>


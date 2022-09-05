<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarElevadoresGeneral.aspx.cs" Inherits="SGI.Reportes.ExportarElevadoresGeneral" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
            <div id="box_datos" class="widget-box" style="display:none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
               <h5>Elevadores - General</h5>
            </div>
         
        </div>
   
        <asp:GridView ID="grdEmpresas" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="seccion" HeaderText="Sección" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="direccion" HeaderText="Ubicación" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Dado_baja" HeaderText="Dada de Baja" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="NroRegistro_empasc" HeaderText="ID ECA" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="RazonSocial_empasc" HeaderText="Razon Social ECA" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Email_empasc" HeaderText="Email Empresa" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Estado_Pago" HeaderText="Estado Pago" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Estado_Aceptacion" HeaderText="Estado Aceptacion ECA" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="fecha_estado_aceptacion" HeaderText="Fecha Aceptacion ECA" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="patente_elevador" HeaderText="Patente" ItemStyle-Width="75px" />
                <asp:BoundField DataField="nom_tipoelevador" HeaderText="Tipo Elevador" ItemStyle-Width="75px" />
                <asp:BoundField DataField="Email_administrador" HeaderText="Email Administrador" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
            </Columns>
            <EmptyDataTemplate>
                No se han encontrado datos con los filtros ingresados.
            </EmptyDataTemplate>
        </asp:GridView>
    
    </form>
</body>
</html>

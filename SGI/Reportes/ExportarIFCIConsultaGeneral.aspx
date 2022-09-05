<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarIFCIConsultaGeneral.aspx.cs" Inherits="SGI.Reportes.ExportarIFCIConsultaGeneral" %>

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
               <h5>IFCI - Consulta General</h5>
            </div>
         
        </div>
   
        <asp:GridView ID="grdEmpresas" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Seccion" HeaderText="Sección" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Manzana" HeaderText="Manzana" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Parcela" HeaderText="Parcela" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Direccion" HeaderText="Ubicación" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Dado_baja" HeaderText="Dada de Baja" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="NroRegistro_empici" HeaderText="ID ECI" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="RazonSocial_empici" HeaderText="Razon Social ECI" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="email_empici" HeaderText="Email ECI" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="nom_estado_aceptacion" HeaderText="Estado Aceptacion ECI" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="patente_ifci" HeaderText="Patente" ItemStyle-Width="75px" />
                <asp:BoundField DataField="nom_tipoinstalacion" HeaderText="Tipo Instalación" ItemStyle-Width="75px" />
                <asp:BoundField DataField="UserName" HeaderText="Usuario Consorcio" ItemStyle-Width="75px" />
                <asp:BoundField DataField="Email" HeaderText="Email Consorcio" ItemStyle-Width="75px" />
                <asp:BoundField DataField="cant_pisos" HeaderText="Cant. pisos" ItemStyle-Width="75px" />
                <asp:BoundField DataField="cant_subsuelos" HeaderText="Cant. Subsuelos" ItemStyle-Width="75px" />
                <asp:BoundField DataField="CreateDate" HeaderText="Fecha Creación" ItemStyle-Width="75px" />
                <asp:BoundField DataField="rubro" HeaderText="Rubro" ItemStyle-Width="75px" />
                <asp:BoundField DataField="confirmado" HeaderText="Confirmado" ItemStyle-Width="80px" />
                <asp:BoundField DataField="superficie" HeaderText="Superficie" ItemStyle-Width="80px" />
            </Columns>
            <EmptyDataTemplate>
                No se han encontrado datos con los filtros ingresados.
            </EmptyDataTemplate>
        </asp:GridView>
    
    </form>
</body>
</html>

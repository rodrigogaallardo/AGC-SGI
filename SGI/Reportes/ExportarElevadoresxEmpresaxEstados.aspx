<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarElevadoresxEmpresaxEstados.aspx.cs" Inherits="SGI.Reportes.ExportarElevadoresxEmpresaxEstados" %>

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
               <h5>Elevadores x Empresa x Estado</h5>
            </div>
         
        </div>
   
        <asp:GridView ID="grdEmpresas" runat="server" AutoGenerateColumns="false">
            <Columns>
                
                <asp:BoundField DataField="direccion" HeaderText="Ubicación" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Dado_baja" HeaderText="Dada de Baja" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" />
                                                <asp:BoundField DataField="RazonSocial_empasc" HeaderText="Empresa" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                                                <asp:BoundField DataField="Email_empasc" HeaderText="Email Empresa" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                                                <asp:BoundField DataField="Anio" HeaderText="Año vigencia" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                                                <asp:BoundField DataField="Estado_Pago" HeaderText="Estado del Pago" ItemStyle-Width="200px" />
                                                <asp:BoundField DataField="Estado_Aceptacion" HeaderText="Estado" ItemStyle-Width="250px" />
                                                <asp:BoundField DataField="Cant_elevadores" HeaderText="Cantidad Elevadores" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                                                <asp:BoundField DataField="Email_administrador" HeaderText="Email Administrador" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
            </Columns>
            <EmptyDataTemplate>
                No se han encontrado datos con los filtros ingresados.
            </EmptyDataTemplate>
        </asp:GridView>
    
    </form>
</body>
</html>

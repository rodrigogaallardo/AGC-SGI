<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarProfesionales.aspx.cs" Inherits="SGI.Reportes.ExportarProfesionales" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <meta name="ROBOTS" content="NOINDEX, NOFOLLOW" />
</head>
<body>
    <form id="form1" runat="server">
            <div id="box_datos" class="widget-box" style="display:none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h3>Listado de Profesionales</h3>
            </div>
         
        </div>
   
        <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="concejo" HeaderText="Concejo" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="nro_matricula" HeaderText="Matrícula" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="nombre_apellido" HeaderText="Apellido y Nombre" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="cuit" HeaderText="C.U.I.T" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="direccion" HeaderText="Dirección" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="perfiles" HeaderText="Perfiles" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-Width="75px" />
                                    <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="200px" />

            </Columns>
            <EmptyDataTemplate>
                No se han encontrado datos con los filtros ingresados.
            </EmptyDataTemplate>
        </asp:GridView>
    
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarElevadoresInspecciones.aspx.cs" Inherits="SGI.Reportes.ExportarElevadoresInspecciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                
                <asp:BoundField DataField="direccion" HeaderText="Ubicación" ItemStyle-Width="200px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="Dado_baja" HeaderText="Dada de Baja" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="seccion" HeaderText="Sección" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="manzana" HeaderText="Manzana" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="parcela" HeaderText="Parcela" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="patente_elevador" HeaderText="Patente" ItemStyle-Width="75px" />
                <asp:BoundField DataField="RazonSocial_empasc" HeaderText="ECA" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="tecnico_ult_informe" HeaderText="Técnico Ult. Informe" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="fecha_ult_inspeccion" HeaderText="Fecha Ult. Informe" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="hora_ult_inspeccion" HeaderText="Hora Ult. Informe" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="res_ult_informe" HeaderText="Resultado Ult. Informe" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="obs_ult_informe" HeaderText="Observaciones Ult. Informe" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="mail_administrador" HeaderText="Mail Administrador" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="mail_empasc" HeaderText="Mail EMPASC" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
                <asp:BoundField DataField="mail_tecnico" HeaderText="Mail Técnico" ItemStyle-Width="150px" ItemStyle-CssClass="align-center" />
            </Columns>
            <EmptyDataTemplate>
                No se han encontrado datos con los filtros ingresados.
            </EmptyDataTemplate>
        </asp:GridView>
    
    </form>
</body>
</html>

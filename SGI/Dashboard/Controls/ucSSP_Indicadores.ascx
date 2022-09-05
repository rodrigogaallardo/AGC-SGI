<%@ Control Language="C#" AutoEventWireup="true" 
    CodeBehind="ucSSP_Indicadores.ascx.cs" 
    Inherits="SGI.Dashboard.Controls.ucSSP_Indicadores" %>


    <%: Scripts.Render("~/bundles/flot") %>
    <%: Styles.Render("~/bundles/flotCss") %>

    <style type="text/css">
    .plot_pie {
	    width: 450px;
	    height: 250px;
    }
	</style>


<asp:HiddenField ID="hid_fecha_actividad" runat="server" Value="" />


<asp:DropDownList ID="ddlPeriodo" runat="server" 
    AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>


<table>

    <tr>

        <td>
            Nivel de actividad
        </td>
        <td>
            <asp:TextBox ID="txtNivelActividad" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>

        <td>
            Demora promedio de un trámite aprobado hasta revisión del gerente
        </td>
        <td>
            <asp:TextBox ID="txtTramiteRevisionGerente" runat="server"></asp:TextBox>
        </td>

    <tr>

        <td>
            Demora promedio de un trámite aprobado hasta generacón expediente
        </td>
        <td>
            <asp:TextBox ID="txtTramiteGeneracionExpediente" runat="server"></asp:TextBox>
        </td>

</table>



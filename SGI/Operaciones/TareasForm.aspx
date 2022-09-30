<%@ Page
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="TareasForm.aspx.cs"
    Inherits="SGI.Operaciones.TareasForm" %>

<%@ Register Assembly="Syncfusion.EJ.Web" Namespace="Syncfusion.JavaScript.Web" TagPrefix="ej" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup>
    <script type="text/javascript">
        function ConfirmaTransaccion() {
            var ddlSFtarea = $('#<%: ddlSFtarea.ClientID %>').val();
            if (ddlSFtarea == "") {
                alert("Debe seleccionar una Tarea");
                return false;
            }
            return confirm('¿Confirma la Transaccion?');
        }

    </script>
    <div class="control-group">
        <label class="control-label" for="txtBuscarSolicitud">Buscar por numero de solicitud</label>

    </div>

    <%--    <ej:Grid ID="Grid1" runat="server">
    </ej:Grid>--%>



    <hr />
    <p class="lead mtop20">Tareas Tramites</p>

    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecInicio" Text="Fec.Inicio" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecCierre" Text="Fec.Cierre" runat="server"></asp:Label>
            <asp:CheckBox ID="chkFechaCierre_tramitetarea" Text="No Establecer" AutoPostBack="false" runat="server" />
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" ID="lblFecAsignacion" Text="Fec.Asignacion" runat="server"></asp:Label>
        </div>
    </div>
    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaInicio_tramitetarea" OnSelectionChanged="calFechaInicio_tramitetarea_SelectionChanged" runat="server"></asp:Calendar>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaCierre_tramitetarea" OnSelectionChanged="calFechaCierre_tramitetarea_SelectionChanged" runat="server"></asp:Calendar>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Calendar ID="calFechaAsignacion_tramtietarea" OnSelectionChanged="calFechaAsignacion_tramtietarea_SelectionChanged" runat="server"></asp:Calendar>
        </div>
    </div>

    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Usuario" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Proxima Tarea" runat="server"></asp:Label>
            <asp:CheckBox ID="chkproxima_tarea" Text="No Establecer" AutoPostBack="false" runat="server" />
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Usuario Creador" runat="server"></asp:Label>
        </div>
    </div>
    <div class="control-group">
        <div style="width: 250px; display: inline-block;">
            <asp:DropDownList ID="ddlUsuarioAsignado_tramitetarea" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged"
                DataTextField="UserName" DataValueField="UserId">
            </asp:DropDownList>
        </div>
        <div style="width: 250px; display: inline-block;">

            <ej:DropDownList ID="ddlSFproxima_tarea" runat="server" Width="200px"
                DataTextField="nombre_Tarea" DataValueField="id_Tarea" DataGroupByField="id_circuito">
            </ej:DropDownList>

        </div>
        <div style="width: 250px; display: inline-block;">
            <asp:DropDownList ID="ddlCreateUser" runat="server"
                DataTextField="UserName" DataValueField="UserId">
            </asp:DropDownList>


        </div>

    </div>



    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Tarea" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" Text="Resultado" runat="server"></asp:Label>
        </div>

    </div>
    <div class="control-group">
        <div style="width: 250px; display: inline-block;">
            <%--  <asp:DropDownList ID="ddltarea" runat="server" AutoPostBack="false" 
                             DataTextField="nombre_tarea" DataValueField="id_tarea"></asp:DropDownList>--%>
            <ej:DropDownList ID="ddlSFtarea" runat="server" Width="200px"
                DataTextField="nombre_Tarea" DataValueField="id_Tarea" DataGroupByField="id_circuito">
            </ej:DropDownList>

        </div>
        <div style="width: 250px; display: inline-block;">
            <asp:DropDownList ID="ddlResultado" runat="server" AutoPostBack="false"
                DataTextField="nombre_resultado" DataValueField="id_resultado">
            </asp:DropDownList>
        </div>


    </div>

    <asp:HiddenField ID="hdidTramiteTarea" runat="server" />
    <asp:HiddenField ID="hdFechaInicio_tramitetarea" runat="server" />
    <asp:HiddenField ID="hdFechaCierre_tramitetarea" runat="server" />
    <asp:HiddenField ID="hdFechaAsignacion_tramtietarea" runat="server" />
    <asp:HiddenField ID="hdidSolicitud" runat="server" />
    <asp:HiddenField ID="hdHAB_TRANSF" runat="server" />
    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


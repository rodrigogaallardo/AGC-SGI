<%@ Page
    Title="Administrar tareas de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="TareasForm.aspx.cs"
    Inherits="SGI.Operaciones.TareasForm" %>

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
            var ddtarea = $('#<%: ddltarea.ClientID %>').val();
            if (ddltarea == "") {
                alert("Debe seleccionar una Tarea");
                return false;
            }
            return confirm('¿Confirma la Transaccion?');
        }

    </script>
  

    <%--    <ej:Grid ID="Grid1" runat="server">
    </ej:Grid>--%>




    <div class="control-group">
        <div style="width: 550px; display: inline-block; margin-top: 5px;">
            <fieldset style="margin: 8px; border: 1px solid grey; padding: 8px; border-radius: 4px;">
                <legend style="padding: 2px; width: 120px;margin-bottom:0px">Tarea Actual</legend>

                <div class="control-group" style="margin-top: 0px;">
                    <div style="width: 250px; display: inline-block; margin-top: 0px;">
                        <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" ID="Label1" Text="Circuito Tarea Actual" runat="server"></asp:Label>
                    </div>
                    <div style="width: 250px; display: inline-block; margin-top: 0px;">
                        <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" ID="Label2" Text="ID Tarea Actual" runat="server"></asp:Label>
                    </div>

                </div>
                <div class="control-group" style="margin-top: 0px;">
                    <div style="width: 250px; display: inline-block; margin-top: 0px;">
                        <asp:DropDownList ID="ddlCircuitoActual" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCircuitoActual_SelectedIndexChanged"
                            DataTextField="nombre_resultado" DataValueField="id_resultado">
                        </asp:DropDownList>
                    </div>
                    <div style="width: 250px; display: inline-block; margin-top: 0px;">
                        <asp:DropDownList ID="ddltarea" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltarea_SelectedIndexChanged"
                            DataTextField="nombre_Tarea" DataValueField="id_Tarea">
                        </asp:DropDownList>

                        
                    </div>
                </div>
            </fieldset>
        </div>


        <div style="width: 550px; display: inline-block; margin-top: 5px;">
            <div class="control-group">
                <div style="width: 250px; display: inline-block; margin-top: 5px;">
                    <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" Text="Usuario" runat="server"></asp:Label>
                  <asp:CheckBox ID="chkUsuario" Text="No Establecer" 
                       AutoPostBack="true" runat="server"  OnCheckedChanged="chkUsuario_CheckedChanged"/>
                </div>
            </div>
            <div class="control-group">
                <div style="width: 250px; display: inline-block; margin-top: 5px;">
                    <asp:DropDownList ID="ddlUsuarioAsignado_tramitetarea" runat="server" AutoPostBack="false" 
                        DataTextField="UserName" DataValueField="UserId">
                    </asp:DropDownList>
                </div>
            </div>
        </div>

    </div>





    <div class="control-group">

        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <div class="control-group">
                <div style="width: 250px; display: inline-block; margin-top: 5px;">
                    <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" Text="Resultado" runat="server"></asp:Label>
                </div>
            </div>
            <div class="control-group">
                <div style="width: 250px; display: inline-block;">
                    <asp:DropDownList ID="ddlResultado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlResultado_SelectedIndexChanged"
                        DataTextField="nombre_resultado" DataValueField="id_resultado">
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <div style="width: 550px; display: inline-block; margin-top: 5px;">
            

                <div class="control-group" style="margin-top: 0px;">
                    
                    <div style="width: 250px; display: inline-block; margin-top: 0px;">
                        <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" ID="Label4" Text="ID ProximaTarea" runat="server"></asp:Label>
                        <asp:CheckBox ID="chkproxima_tarea" Text="No Establecer" AutoPostBack="true" runat="server" OnCheckedChanged="chkproxima_tarea_CheckedChanged" />
                    </div>

                </div>
                <div class="control-group" style="margin-top: 0px;">
                    
                    <div style="width: 250px; display: inline-block; margin-top: 0px;">
                          <asp:DropDownList ID="ddlproxima_tarea" runat="server" AutoPostBack="false" 
                            DataTextField="nombre_Tarea" DataValueField="id_Tarea">
                        </asp:DropDownList>
                       </div>
                </div>
         
        </div>




    </div>







    <div class="control-group">
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" ID="lblFecInicio" Text="Fec.Inicio" runat="server"></asp:Label>
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" ID="lblFecCierre" Text="Fec.Cierre" runat="server"></asp:Label>
            <asp:CheckBox ID="chkFechaCierre_tramitetarea" Text="No Establecer" AutoPostBack="true" OnCheckedChanged="chkFechaCierre_tramitetarea_CheckedChanged" runat="server" />
        </div>
        <div style="width: 250px; display: inline-block; margin-top: 5px;">
            <asp:Label class="control-label" style="font-family:sans-serif;font-size:14px;" ID="lblFecAsignacion" Text="Fec.Asignacion" runat="server"></asp:Label>
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
            <asp:Calendar ID="calFechaAsignacion_tramtietarea"  OnSelectionChanged="calFechaAsignacion_tramtietarea_SelectionChanged" runat="server"></asp:Calendar>
        </div>
    </div>

 







    <asp:HiddenField ID="hdidTramiteTarea" runat="server" />
    <asp:HiddenField ID="hdFechaInicio_tramitetarea" runat="server" />
    <asp:HiddenField ID="hdFechaCierre_tramitetarea" runat="server" />
    <asp:HiddenField ID="hdFechaAsignacion_tramtietarea" runat="server" />
    <asp:HiddenField ID="hdidSolicitud" runat="server" />
    <asp:HiddenField ID="hdHAB_TRANSF" runat="server" />
    <asp:HiddenField ID="hdCreateUser" runat="server" />
    
    <div class="control-group">
        <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="btnSave_Click" OnClientClick="return ConfirmaTransaccion();" CssClass="btn btn-primary" />
        <asp:Button ID="btnReturn" runat="server" Text="Volver" OnClick="btnReturn_Click" CssClass="btn btn-primary" />

    </div>
</asp:Content>


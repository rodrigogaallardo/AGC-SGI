<%@ Title="Equipo de Trabajo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmEquipoDeTrabajo.aspx.cs" Inherits="SGI.AbmEquipoDeTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <%: Styles.Render("~/Content/themes/base/css") %>

   
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <div class="widget-box">
    <div class="widget-title">
            <span class="icon"><i class="icon-search"></i></span>
      <h5>Buscar Empleado</h5>
    </div>
    <div class="widget-content">

        <asp:HiddenField ID="hid_userid" runat="server" />
        <asp:HiddenField ID="hid_RoleId_Asignado" runat="server" />

    <asp:UpdatePanel ID="updPnlAgregar" runat="server" RenderMode="Inline">
        <ContentTemplate>
      
                <div class="control-group">

                    <div class="controls">
                        <div class="clearfix">

                            <div class="pull-left">
                                <asp:DropDownList ID="ddlEmpleado" runat="server" Width="350px"   multiple>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hid_ddlEmpleadoB" runat="server" />

                            </div>

                            <div class="pull-left mLeft">
                                <asp:TextBox ID="txtEmpleadoValidar" runat="server" AutoCompleteType="Disabled"  
                                    Enabled="false" Width="100px" style="display:none" >
                                </asp:TextBox>
                
                                <img id="loading_empleados" src="../Content/img/app/Loading24x24.gif" style="display: none" alt="" />
                            </div>

                            <div class="pull-left mLeft">
  

                                        <div class="pull-left mLeft" style="padding-left:20px">
       
                                            <asp:LinkButton ID="btnAgregarUsuario" runat="server" 
                                                    CssClass="btn btn-primary" ValidationGroup="agregar"
                                                    OnClick="btnAgregarUsuario_Click">
                                                <i class="icon-white icon-plus"></i>
                                                <span class="text">Agregar a mi Equipo</span>
                                            </asp:LinkButton>
                                        </div>
                                 
                                        <div class="pull-left mLeft">
                                            <asp:UpdateProgress ID="updProgressAgregar" AssociatedUpdatePanelID="updPnlAgregar"
                                                runat="server" DisplayAfter="0" DynamicLayout="false">
                                                <ProgressTemplate>
                                                    <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>

                            </div>

                        </div>

                        <div class="req">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Debe seleccionar una persona de la lista desplegable."
                                Display="Dynamic" ControlToValidate="ddlEmpleado" 
                                CssClass="field-validation-error"  ValidationGroup="agregar">
                            </asp:RequiredFieldValidator>
                        </div>

                    </div>

                </div>


        </ContentTemplate>
    </asp:UpdatePanel>
        
            </div>
    <asp:UpdatePanel ID="updPnlMensajes" runat="server" RenderMode="Inline">
        <ContentTemplate>

        <asp:Panel ID="pnlResultado" runat="server" Visible="false" Style="margin-top:20px"  >

            <div class="alert alert-info">
                <asp:Label ID="lblMensajeResultado" runat="server" 
                    class="titulo-2"
                    Text="Los cambios se han realizado exitosamente."></asp:Label>
            </div>

        </asp:Panel>

        <asp:Panel ID="pnlError" runat="server" Visible="false" Style="margin-top:20px"  >

            <div class="alert alert-error" >
                <button type="button" class="close" data-dismiss="alert">&times;</button>
                <asp:Label ID="lblMensajeError" runat="server" 
                    class="titulo-2"
                    Text="Ha ocurrido un error al realizr los cambios."></asp:Label>
            </div>
        </asp:Panel>


    </ContentTemplate>
</asp:UpdatePanel>
    </div>

    <div class="widget-box">

        <div class="widget-title">
            <span class="icon"><i class="icon-list-alt"></i></span>
            <h5>Su Equipo de Trabajo</h5>
        </div>

        <asp:UpdatePanel ID="updPnlEmpleado" runat="server" RenderMode="Inline">
            <ContentTemplate>
    

                <asp:GridView ID="grdEmpleado" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="id_equipo_trabajo"
                    AllowPaging="False"  ItemType="EquipoTrabajoAsignado"
                    CssClass="table table-bordered table-striped table-hover" GridLines="None"  >
                    <Columns>

                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" ItemStyle-Width="200px" />

                        <asp:BoundField DataField="Nombres"  HeaderText="Nombre" ItemStyle-Width="200px" />

                        <asp:TemplateField HeaderText="Acción" ItemStyle-CssClass="align-center"  ItemStyle-Width="200px" >
                            <ItemTemplate>
                                       
                                <asp:LinkButton ID="btnEliminar" runat="server" 
                                   OnClientClick="javascript:return ConfirmEliminarEquipo(this);"
                                    CommandArgument='<%# Item.id_equipo_trabajo  %>' 
                                    OnCommand="Eliminar_Empleado_Command" Width="200px">

                                    <i class="icon icon-trash"></i> 
                                    <span class="text">Quitar de mi equipo</span></a>

                                </asp:LinkButton>

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                    <EmptyDataTemplate>
                        <div style="width: 100%;">

                            
                            <i class="icon-ban-circle"></i>
                            <span class="text">No posee empleados asignados a su equipo de trabajo.</span>
                
                        </div>

                        
                    </EmptyDataTemplate>
                </asp:GridView>

            </ContentTemplate>
        </asp:UpdatePanel>

    </div>




    <script type="text/javascript">

        $(document).ready(function () {
            var p_userid = $("[id*='hid_userid']").val();
            var p_RoleId_Asignado = $("[id*='hid_RoleId_Asignado']").val();
            init_js();
        });

        function init_js() {

            /// Inicializar select2 de busqueda
            var tags_selecionados = "";
            if ($("#<%: hid_ddlEmpleadoB.ClientID %>").val().length > 0) {
                tags_selecionados = $("#<%: hid_ddlEmpleadoB.ClientID %>").val().split(",");
            }

            $("#<%: ddlEmpleado.ClientID %>").select2({
                tags: true,
                tokenSeparators: [","],
                placeholder: "Ingrese las etiquetas de búsqueda",
                language: "es",
                data: tags_selecionados
            });
            $("#<%: ddlEmpleado.ClientID %>").val(tags_selecionados);
            $("#<%: ddlEmpleado.ClientID %>").trigger("change.select2");

            $("#<%: ddlEmpleado.ClientID %>").on("change", function () {
                $("#<%: hid_ddlEmpleadoB.ClientID %>").val($("#<%: ddlEmpleado.ClientID %>").val());
            });
        }

        function Select2CloseAll() {
            // Cerrar todos los combos al abrir uno
            $("select").each(function (ind, elem) {
                $(elem).select2("close");
            });

            return false;
        }

        function Select2Close(obj) {
            $(obj).select2("close");
            return true;
        }


        function ConfirmEliminarEquipo(control) {
            return confirm('¿Esta seguro que desea quitar a está persona?');
        }

        function EjecutarScript() {
          
            limpiarBuscarEmpleado();
        }

        function limpiarBuscarEmpleado() {

            
        }


    </script>


</asp:Content>

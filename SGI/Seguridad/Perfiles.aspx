<%@ Page Title="Perfiles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="SGI.Seguridad.Perfiles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--ajax cargando ...--%>
    <div id="Loading" style="text-align: center; padding-bottom: 20px; margin-top: 120px">
        <table border="0" style="border-collapse: separate; border-spacing: 5px; margin: auto">
            <tr>
                <td>
                    <img src="<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>" alt="" />
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px">Cargando...
                </td>
            </tr>
        </table>
    </div>

    <div id="page_content" style="display: none">
        
        <div id="box_busqueda_perfil" >

        <asp:Panel ID="pnlBotonDefault" class="widget-box" runat="server" DefaultButton="btnBuscar" >

            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-search"></i></span>
                <h5>B&uacute;squeda de Perfiles</h5>
            </div>

            <div class="widget-content">
                        <asp:UpdatePanel ID="updpnlBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCargarDatos" runat="server" OnClick="btnCargarDatos_Click" Style="display: none" />
                                <asp:Panel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar" CssClass="form-horizontal">

                                    <div class="control-group">
                                        <label class="col-sm-2 control-label">Nombre:</label>
                                        <div class="col-sm-10 controls">
                                            <asp:TextBox ID="txtBusNombre" runat="server" CssClass="form-control" Style="max-width:500px" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="col-sm-2 control-label">Descripci&oacute;n:</label>
                                        <div class="col-sm-10 controls">
                                            <asp:TextBox ID="txtBusDescripcion" runat="server" CssClass="form-control" Style="max-width:500px" ></asp:TextBox>
                                        </div>
                                    </div>
                                    

                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
         </asp:Panel>
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <div class="pull-right" >
                                        <div class="control-group inline-block">
                                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="UpdatePanel2">
                                                    <ProgressTemplate>
                                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                  </div>
                                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-inverse" OnClick="btnBuscar_Click">
                                                <i class="imoon imoon-search"></i>
                                                <span class="text">Buscar</span>
                                            </asp:LinkButton>   
                                            
                                          
                                            <asp:LinkButton ID="btnNuevoPerfil" runat="server" CssClass="btn btn-default" OnClick="btnNuevoPerfil_Click">
                                                <i class="imoon imoon-plus"></i>
                                                <span class="text">Nuevo Perfil</span>
                                            </asp:LinkButton>
                                         
                                              
                                    </div>

            </ContentTemplate>
        </asp:UpdatePanel>
            
            <br /><br />
        
    <div id="box_resultado" style="display: none">              
        <div class="widget-box">
                    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            
                        <div class="form-horizontal" style="margin-left:15px;margin-right:15px">

                            <asp:Panel ID="pnlCantidadRegistros" runat="server" CssClass="form-horizontal" Visible="false">
                               <div "text-left">
                                    <h5>Resultado de la b&uacute;squeda</h5>
                                </div>
                                <div class="text-right">
                                    <span class="badge">Cantidad de registros:
                                    <asp:Label ID="lblCantidadRegistros" runat="server" CssClass="badge">0</asp:Label></span>
                                </div>
                            </asp:Panel>


                            <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                 GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                                PageSize="50" OnSorting="grdResultados_Sorting" AllowSorting="True">
                                <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                                <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                                <Columns>
                              <%--      <asp:TemplateField HeaderText="Acción" ItemStyle-CssClass="text-center" ItemStyle-Width="3px">
                                        <ItemTemplate>

                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:BoundField DataField="nombre_perfil" ItemStyle-Width="300px" HeaderText="Nombre" SortExpression="nombre_perfil"/>
                                    <asp:BoundField DataField="descripcion_perfil" HeaderText="Descripción / Aclaración" SortExpression="descripcion_perfil" />

                                    <asp:TemplateField HeaderText="Acción" HeaderStyle-ForeColor="#0088cc" ItemStyle-Width="85px" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditarPerfil" runat="server" title="Editar" data-toggle="tooltip" CssClass="link-local"
                                                CommandArgument='<%# Eval("id_perfil") %>' OnClick="btnEditarPerfil_Click">
                                                <i class="imoon-edit" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminarPerfil" runat="server" title="Eliminar" data-toggle="tooltip" 
                                                CssClass="link-local" data-id-perfil='<%# Eval("id_perfil") %>' OnClientClick="return showfrmConfirmarEliminar(this);">
                                                    <i class="imoon-remove" style="font-size:medium;margin-right:3px;margin-left:3px;color:#337AB7"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                                <EmptyDataTemplate>

                                    <div >

                                        <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                        <span class="mleft20">No se encontraron registros.</span>

                                    </div>

                                </EmptyDataTemplate>

                            </asp:GridView>

                            <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updResultados">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                            Cargando...
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                             </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
           </div>
        </div>

        <div id="box_datos_perfil" class="widget-box" style="display: none">
            <div class="widget-title">
                <span class="icon"><i class="imoon imoon-user-md"></i></span>
                <h5>Datos del perfil</h5>
            </div>
            <div class="widget-content">
                <div class="row">
                    <div class="col-sm-12 col-md-12">
                        <asp:UpdatePanel ID="updDatosPerfil" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <asp:HiddenField ID="hid_id_perfil" runat="server" />

                                <asp:Panel ID="pnlDatosPerfil" runat="server" CssClass="form-horizontal">

                                    <div class="col-sm-12">

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Nombre (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Style="max-width:500px" MaxLength="50"></asp:TextBox>
                                                <div id="Req_Nombre" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                    Debe ingresar el Nombre del perfil.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="col-sm-2 control-label">Descripci&oacute;n (*):</label>
                                            <div class="col-sm-10 controls">
                                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" Style="max-width:500px" MaxLength="200"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        
                    </div>
                    
                </div>
                <div class="row">
                    
                    <div class="" style="margin-left:170px;">
                        <div class="control-group">
                            <label class="col-sm-2 control-label">Permisos al men&uacute;:</label>
                            <div class="col-sm-10 mtop10">
                                <asp:UpdatePanel ID="updMenu" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Literal ID="trMenu" runat="server"></asp:Literal>


                                        <asp:TextBox ID="hid_ids_menus" runat="server" Width="100%" style="display:none"></asp:TextBox>

                                        <div id="Req_Permisos" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                            Debe ingresar los permisos de men&uacute; asociados al perfil.
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    
                </div>

                <div class="row">

                    <div class="" style="margin-left:170px;">
                        <div class="control-group">
                            <label class="col-sm-2 control-label">Permisos a las Tareas:</label>
                            <div class="col-sm-10 mtop10">
                                <asp:UpdatePanel ID="updPermisosTareas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Literal ID="trTareas" runat="server"></asp:Literal>


                                        <asp:TextBox ID="hid_ids_tareas" runat="server" Width="100%" Style="display: none"></asp:TextBox>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="updBotonesGuardar" runat="server">
                        <ContentTemplate>
                            
                            <div class="col-sm-offset-2 col-sm-10 form-inline">

                                <div id="pnlBotonesGuardar" class="form-inline">
                                    <div class="controls">
                                        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn" OnClick="btnGuardar_Click" OnClientClick="return validarGuardar();">
                                                                    <i class="imoon imoon-save"></i>
                                                                    <span class="text">Guardar</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-inverse" OnClientClick="return showBusqueda();">
                                                                    <i class="imoon imoon-blocked"></i>
                                                                    <span class="text">Cancelar</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="updBotonesGuardar">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                            Guardando...
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>

                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--Confirmar Eliminar Perfil--%>
    <div id="frmConfirmarEliminar" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Eliminar Perfil</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                            </td>
                            <td style="vertical-align: middle">
                                <label class="mleft10">¿ Est&aacute; seguro de eliminar el registro ?</label>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">

                    <asp:UpdatePanel ID="updConfirmarEliminar" runat="server">
                        <ContentTemplate>

                            <asp:HiddenField ID="hid_id_perfil_eliminar" runat="server" />

                            <div class="form-inline">
                                <div class="control-group">
                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="updConfirmarEliminar">
                                        <ProgressTemplate>
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div id="pnlBotonesConfirmacionEliminar" class="control-group">
                                    <asp:Button ID="btnEliminarPerfil" runat="server" CssClass="btn" Text="Sí" OnClick="btnEliminarPerfil_Click"
                                        OnClientClick="ocultarBotonesConfirmacion();" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->
    <%--modal de Errores--%>
    <div id="frmError" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Error</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64" style="color: #f00"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="control-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->

    <script type="text/javascript">

        $(document).ready(function () {

            $("#page_content").hide();
            $("#Loading").show();

            init_Js_updpnlBuscar();
            init_Js_updResultados();
            init_Js_updDatosPerfil();

            $("#<%: btnCargarDatos.ClientID %>").click();
        });
        
        function finalizarCarga() {

            $("#Loading").hide();

            $("#page_content").show();

            return false;

        }

        function toolTips() {
            $("[data-toggle='tooltip']").tooltip();
            return false;

        }
        function showfrmError() {

            $("#frmError").modal("show");

            return false;
        }

        function init_Js_updpnlBuscar() {
            toolTips();
            return false;
        }
        function init_Js_updResultados() {
            toolTips();
        }
        
        function init_Js_updDatosPerfil() {

            $("#<%: txtNombre.ClientID %>").on("keyup", function (e) {
                $("#Req_Nombre").hide();
            });

            return false;

        }
        function init_Js_updMenu() {
            $("#tree_menu .submenu").on('click', function (e) {
                
                var children = $(this).parent('li.parent_li').find(' > ul > li');
                if (children.is(":visible")) {
                    children.hide('fast');
                    $(this).addClass('imoon-plus-sign').removeClass('imoon-minus-sign');
                } else {
                    children.show('fast');
                    $(this).addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
                }
                e.stopPropagation();
            });
            toolTips();
            return false;

        }
        function Collapse_MenuTareas(obj) {
            var children = $(obj).parent('li.parent_li').find('ul > li');
            if (children.is(":visible")) {
                children.hide('slow');
                $(obj).addClass('imoon-plus-sign').removeClass('imoon-minus-sign');
            } else {
                children.show('slow');
                $(obj).addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
            }
            e.stopPropagation();

            toolTips();
            return false;
        }
        
        function init_Js_updPermisosTareas(idArbol) {


            //$(idArbol+" .submenu").on('click', function (e) {
                
            //    var children = $(this).parent('li.parent_li').find('ul > li');
            //    if (children.is(":visible")) {
            //        children.hide('fast');
            //        $(this).addClass('imoon-plus-sign').removeClass('imoon-minus-sign');
            //    } else {
            //        children.show('fast');
            //        $(this).addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
            //    }
            //    e.stopPropagation();
            //});

            //toolTips();
            //return false;

        }
        function showfrmConfirmarEliminar(obj) {

            var id_perfil_eliminar = $(obj).attr("data-id-perfil");

            $("#<%: hid_id_perfil_eliminar.ClientID %>").val(id_perfil_eliminar);

            $("#frmConfirmarEliminar").modal("show");
            return false;
        }

        function hidefrmConfirmarEliminar() {
            $("#frmConfirmarEliminar").modal("hide");
            return false;
        }
        function ocultarBotonesConfirmacion() {
            $("#pnlBotonesConfirmacionEliminar").hide();
            return false;
        }

        function showDatosPerfil(countCircuitos) {
            
            setearValoresTree();
            setearValoresTreeTareas(countCircuitos);
            $("#box_busqueda_perfil").hide("slow");
            $("#box_datos_perfil").show("slow");
        }

        function showBusqueda() {
            $("#box_datos_perfil").hide("slow");
            $("#box_busqueda_perfil").show("slow");
        }
        function showResultado() {
            $("#box_resultado").show("slow");
        }

        function ocultarBotonesGuardar() {
            $("#pnlBotonesGuardar").hide();
            return false;
        }

        function tree_chkclick(item) {

            
            var checked = $(item).prop("checked");
            var id = parseInt($(item).attr("data-id"));
            var parentid = parseInt($(item).attr("data-parentid"));
            var cantHijos = $("[data-parentid='" + id + "']").length;

            if (parentid > 0 && checked) {
                tildarPadres(item,true);
            }
            if (cantHijos > 0) {
                tildarHijos(item,checked);
            }

            verificarPadresSinhijos(item);

            var items = new Array($("#tree_menu :checkbox").filter(":checked").length);

            $("#tree_menu :checkbox").filter(":checked").each(function (index, element) {

                var item_id = parseInt($(element).attr("data-id"));
                items[index] = item_id;
            });
            
            $("#<%: hid_ids_menus.ClientID %>").val(items.join());

            $("#Req_Permisos").hide();

            return true;
        }
        function tildarPadres(item,checked) {

            var parentid = parseInt($(item).attr("data-parentid"));
            if (parentid > 0) {
                var itemPadre = $("[data-id='" + parentid + "']").eq(0);
                $(itemPadre).prop("checked", checked);
                tildarPadres(itemPadre,checked);
            }   
        }

        function tildarHijos(item, checked) {
            var id = parseInt($(item).attr("data-id"));
            
            $("[data-parentid='" + id + "']").each(function (index, itemHijo) {
                $(itemHijo).prop("checked", checked);
                tildarHijos(itemHijo,checked);
            });
        }

        function verificarPadresSinhijos(item) {

            var checked = $(item).prop("checked");
            var id = parseInt($(item).attr("data-id"));
            var parentid = parseInt($(item).attr("data-parentid"));
            
            if (!checked) {

                var algunoTildado = ($("[data-parentid='" + parentid + "']").filter(":checked").length > 0);
                if (!algunoTildado) {
                    tildarPadres(item, false);
                }
            }

        }

        function setearValoresTree() {

            var strItems = $("#<%: hid_ids_menus.ClientID %>").val();
            var items = strItems.split(",");
            //Limpiar todo el arbol
            $("#tree_menu :checkbox").filter(":checked").prop("checked", false);

            items.forEach(function (item) {
                $("[data-id='" + item + "']").prop("checked", true);
                    //checkedbox.parents().find('i.imoon-plus-sign').addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
                    //checkedbox.parents().find('i.imoon-plus-sign').show("slow");
                
                //if (menu.parents().children(":checked") == true)
                    //menu.parents().find('i.nivel0').children().is.css({ "color": "red", "border": "2px solid red" });
                //var padre = tarea.parent().parent().parent();//ul que contiene todos los li con tareas
                //menu.parents().find('i.nivel1').addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
                //menu.parentsUntil().find('li.nivel1').css({ "color": "green", "border": "2px solid red" });
                //padre.parent().parent().find('i.submenu').addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
                //padre.children().show("slow");//mostramos a todos los hijos o sea li con tareas
            });
            //aqui se colapsan si hay checkbox checkeados
            $("#tree_menu").find('span').each(function () {
                var row = $(this);
                if (row.children('input[type="checkbox"]').is(':checked')) {
                    //row.css({ "color": "green", "border": "2px solid green" });
                    row.parent().children('i.submenu').addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
                    row.closest('ul').children('li').show('slow');

                }
                else {
                    row.parent().children('i.submenu').addClass('imoon-plus-sign').removeClass('imoon-minus-sign');
                }
            });
            
            return false;   

        }

        function setearValoresTreeTareas(countCircuitos) {

            var strItems = $("#<%: hid_ids_tareas.ClientID %>").val();
            var items = strItems.split(",");
            //Limpiar todo el arbol
            for (i = 0; i < countCircuitos; i++) {
                $("#tree_tareas" + i + " :checkbox").filter(":checked").prop("checked", false);
            }

            items.forEach(function (item) {
                $("[data-id-tarea='" + item + "']").prop("checked", true);
                var tarea = $("[data-id-tarea='" + item + "']");
                var padre = tarea.parent().parent().parent();//ul que contiene todos los li con tareas
                padre.parent().parent().find('i').addClass('imoon-minus-sign').removeClass('imoon-plus-sign');
                padre.children().show("slow");//mostramos a todos los hijos o sea li con tareas
            });

            return false;

        }


        function tree_tareas_chkclick(item,countCircuitos) {


            var checked = $(item).prop("checked");
            var id = parseInt($(item).attr("data-id-tarea"));
            var parentid = parseInt($(item).attr("data-tarea-parentid"));
            
            var items = new Array();
                

            for (i = 0; i < countCircuitos; i++) {
                var itemsTemp = new Array($("#tree_tareas" + i + " :checkbox").filter(":checked").length);
                $("#tree_tareas" + i + " :checkbox").filter(":checked").each(function (index, element) {
                    var item_id = parseInt($(element).attr("data-id-tarea"));
                    itemsTemp[index] = item_id;
                });

                items = items.concat(itemsTemp);
            }
            
            $("#<%: hid_ids_tareas.ClientID %>").val(items.join());


            return true;
        }

        function validarGuardar() {

            var ret = true;
            $("#Req_Nombre").hide();
            $("#Req_Permisos").hide();
            
            if ($.trim($("#<%: txtNombre.ClientID %>").val()).length == 0) {
                $("#Req_Nombre").css("display", "inline-block");
                ret = false;
            }

            if ($("#tree_menu :checkbox").filter(":checked").length <= 0) {
                $("#Req_Permisos").css("display", "inline-block");
                ret = false;
            }

            if (ret) {
                ocultarBotonesGuardar();
            }

            return ret;

        }
    </script>

</asp:Content>

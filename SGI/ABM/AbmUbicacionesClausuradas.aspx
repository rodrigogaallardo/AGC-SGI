<%@ Page Title="Ubicaciones con iregularidades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbmUbicacionesClausuradas.aspx.cs" Inherits="SGI.ABM.AbmUbicacionesClausuradas" %>


<%@ Register Src="~/Controls/BuscarUbicacion.ascx" TagPrefix="uc1" TagName="BuscarUbicacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>
  

    <script type="text/javascript">
        $(document).ready(function () {
            inicializar_controles();
        });


        function mostrarMensaje(texto, titulo) {
            $.gritter.add({
                title: titulo,
                text: texto,
                image: '<%: ResolveUrl("~/Content/img/info32.png") %>',
            sticky: false
        });
    }

        function inicializar_controles() {
            inicializar_autocomplete();
            inicializar_fechas();
            $("#<%: txtUbiNroPuerta.ClientID%>").autoNumeric({ aSep: '', mDec: '0', vMax: '999999' });
        }
      
        function inicializar_fechas() {
            var FechaAlta = $('#<%=txtFechaAlta.ClientID%>');
            var es_readonly = $(FechaAlta).attr("readonly");
            if (!($(FechaAlta).is('[disabled]') || $(FechaAlta).is('[readonly]'))) {
                $(FechaAlta).datepicker(
                {
                    closeText: 'Cerrar', prevText: '&#x3c;Ant', nextText: 'Sig&#x3e;', currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm', dateFormat: 'dd/mm/yy', firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: ''
                }
                );
            }
            var FechaBaja = $('#<%=txtFechaBaja.ClientID%>');
            var es_readonly = $(FechaBaja).attr("readonly");
            if (!($(FechaBaja).is('[disabled]') || $(FechaBaja).is('[readonly]'))) {
                $(FechaBaja).datepicker(
                {
                    closeText: 'Cerrar', prevText: '&#x3c;Ant', nextText: 'Sig&#x3e;', currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
                    weekHeader: 'Sm', dateFormat: 'dd/mm/yy', firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: ''
                }
                );
            }
        }

        function showBusqueda() {
            $("#box_busqueda").show("slow");
            $("#box_resultado").show("slow");
        }

        function hideBusqueda() {
            $("#box_resultado").hide("slow");
        }

        function bt_btnUpDown_collapse_click(obj) {
            var href_collapse = $(obj).attr("href");

            if ($(href_collapse).attr("id") != undefined) {
                if ($(href_collapse).css("height") == "0px") {
                    $(obj).find(".imoon-chevron-down").switchClass("imoon-chevron-down", "imoon-chevron-up", 0);
                }
                else {
                    $(obj).find(".imoon-chevron-up").switchClass("imoon-chevron-up", "imoon-chevron-down", 0);
                }
            }
        }

        function switchear_buscar_ubicacion(btn) {

            if (btn == 1) {
                $("#buscar_ubi_por_partida").show();
                $("#buscar_ubi_por_dom").hide();
                $("#buscar_ubi_por_smp").hide();
                $("#buscar_ubi_por_especial").hide();

                $("#btnBuscarPorPartida").addClass("active");
                $("#btnBuscarPorDom").removeClass("active");
                $("#btnBuscarPorSMP").removeClass("active");
                $("#btnBuscarPorUbiEspecial").removeClass("active");

            }
            else if (btn == 2) {
                $("#buscar_ubi_por_partida").hide();
                $("#buscar_ubi_por_dom").show();
                $("#buscar_ubi_por_smp").hide();
                $("#buscar_ubi_por_especial").hide();

                $("#btnBuscarPorPartida").removeClass("active");
                $("#btnBuscarPorDom").addClass("active");
                $("#btnBuscarPorSMP").removeClass("active");
                $("#btnBuscarPorUbiEspecial").removeClass("active");
            }
            else if (btn == 3) {
                $("#buscar_ubi_por_partida").hide();
                $("#buscar_ubi_por_dom").hide();
                $("#buscar_ubi_por_smp").show();
                $("#buscar_ubi_por_especial").hide();

                $("#btnBuscarPorPartida").removeClass("active");
                $("#btnBuscarPorDom").removeClass("active");
                $("#btnBuscarPorSMP").addClass("active");
                $("#btnBuscarPorUbiEspecial").removeClass("active");
            }
            else if (btn == 4) {
                $("#buscar_ubi_por_partida").hide();
                $("#buscar_ubi_por_dom").hide();
                $("#buscar_ubi_por_smp").hide();
                $("#buscar_ubi_por_especial").show();

                $("#btnBuscarPorPartida").removeClass("active");
                $("#btnBuscarPorDom").removeClass("active");
                $("#btnBuscarPorSMP").removeClass("active");
                $("#btnBuscarPorUbiEspecial").addClass("active");
            }


        }
    </script>
    
    <div id="box_busqueda" >
    <asp:Panel ID="pnlBotonDefault" runat="server" DefaultButton="btnBuscar" >
        
    <div class="accordion-group widget-box" >

            <%-- titulo collapsible buscar ubicacion --%>
            <div class="accordion-heading">
                <a id="bt_ubicacion_btnUpDown" data-parent="#collapse-group" href="#collapse_bt_ubicacion"
                    data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">

                    <%--                <asp:HiddenField ID="hid_bt_ubicacion_collapse" runat="server" Value="false"/>
                    <asp:HiddenField ID="hid_bt_ubicacion_visible" runat="server" Value="false"/>--%>

                    <div class="widget-title">
                        <span class="icon" style="margin-left:4px"><i class="imoon-map-marker"></i></span>
                        <h5>
                            <asp:Label ID="Label1" runat="server" Text="Ubicaciones con iregularidades"></asp:Label></h5>
                        <span class="btn-right"><i class="imoon-chevron-up"></i></span>   
                    </div>
                </a>
            </div>

            <%-- controles collapsible buscar por ubicacion --%>
            <div class="accordion-body collapse in" id="collapse_bt_ubicacion">
                <div class="widget-content">

                    <%--tipos de busquedad por ubicacion--%>
                    <div class="widget-content">

                        <div class="btn-group" data-toggle="buttons-radio" style="display: table-cell;">
                            <button id="btnBuscarPorPartida" type="button" class="btn active" onclick="switchear_buscar_ubicacion(1);">Por Partida</button>
                            <button id="btnBuscarPorDom" type="button" class="btn" onclick="switchear_buscar_ubicacion(2);">Por Domicilio</button>
                            <button id="btnBuscarPorSMP" type="button" class="btn" onclick="switchear_buscar_ubicacion(3);">Por SMP</button>
                            <button id="btnBuscarPorUbiEspecial" type="button" class="btn" onclick="switchear_buscar_ubicacion(4);">Por Ubicaciones Especiales</button>
                        </div>

                    </div>


                    <%--buscar por numero de partida--%>
                    <div id="buscar_ubi_por_partida" class="widget-content">

                        <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_partida" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">

                                    <fieldset>

                                        <div class="control-group">
                                            <asp:Label ID="lblUbiPartidaMatriz" runat="server" AssociatedControlID="rbtnUbiPartidaMatriz"
                                                CssClass="control-label">Tipo de Partida:</asp:Label>
                                            <div class="controls">
                                                <div class="form-inline">
                                                    <asp:RadioButton ID="rbtnUbiPartidaMatriz" runat="server"
                                                        Text="Matriz" GroupName="TipoDePartida" Checked="true" />
                                                    <asp:RadioButton ID="rbtnUbiPartidaHoriz" runat="server" Style="padding-left: 5px"
                                                        Text="Horizontal" GroupName="TipoDePartida" />
                                                </div>
                                            </div>

                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="lblUbiNroPartida" runat="server" AssociatedControlID="txtUbiNroPartida"
                                                CssClass="control-label">Nro. Partida:</asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtUbiNroPartida" runat="server" MaxLength="10" Width="100px"
                                                    CssClass="input-xlarge"></asp:TextBox>
                                            </div>
                                        </div>


                                    </fieldset>

                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>

                    <%--buscar por nombre de calle--%>
                    <div id="buscar_ubi_por_dom" class="widget-content" style="display: none">
                        <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_dom" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <fieldset>
                                        <div class="control-group">
                                            <asp:Label ID="lblUbiCalle" runat="server" AssociatedControlID="AutocompleteCalles"
                                                CssClass="control-label">Búsqueda de Calle:</asp:Label>
                                            <div class="controls">
                                                <div class="clearfix">
                                                    <div class="pull-left">
                                                     <ej:Autocomplete ID="AutocompleteCalles" MinCharacter="3" DataTextField="NombreOficial_calle" DataUniqueKeyField="Codigo_calle" Width="500px" runat="server"/>
                                                        <span style="font-size: 8pt">Debe ingresar un mínimo de 3 letras y el sistema le mostrará
                                                                las calles posibles.</span>
                                                        <%--<asp:RequiredFieldValidator ID="ReqCalle" runat="server" ErrorMessage="Debe seleccionar una de las calles de la lista desplegable."
                                                            Display="Dynamic" ControlToValidate="AutocompleteCalles" ValidationGroup="Buscar2"
                                                            CssClass="field-validation-error"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblUbiNroPuerta" runat="server" AssociatedControlID="txtUbiNroPuerta"
                                                CssClass="control-label">Nro. Puerta:</asp:Label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtUbiNroPuerta" runat="server" MaxLength="10" Width="50px"
                                                    CssClass="input-xlarge"></asp:TextBox>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>


                    <%--buscar por seccion, manzana, parcela--%>
                    <div id="buscar_ubi_por_smp" class="widget-content" style="display: none">

                        <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_smp" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">

                                    <fieldset>

                                        <div class="control-group">

                                            <asp:Label ID="lblUbiSeccion" runat="server" AssociatedControlID="txtUbiManzana"
                                                Text="Sección:" class="control-label" Style="padding-top: 0"></asp:Label>
                                            <div class="control-label" style="margin-left: -75px; margin-top: -20px">
                                                <asp:TextBox ID="txtUbiSeccion" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                            </div>


                                            <asp:Label ID="lblUbiManzana" runat="server" AssociatedControlID="txtUbiManzana"
                                                Text="Manzana:" class="control-label" Style="padding-top: 0"></asp:Label>
                                            <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                <asp:TextBox ID="txtUbiManzana" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                            </div>


                                            <asp:Label ID="lblUbiParcela" runat="server" AssociatedControlID="txtUbiParcela"
                                                Text="Parcela:" class="control-label" Style="padding-top: 0"></asp:Label>
                                            <div class="control-label" style="margin-left: -65px; margin-top: -20px">
                                                <asp:TextBox ID="txtUbiParcela" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </fieldset>



                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>

                    <%--buscar por tipo subtipo ubicacion--%>
                    <div id="buscar_ubi_por_especial" class="widget-content" style="display: none">

                        <asp:UpdatePanel ID="updPnlFiltroBuscar_ubi_especial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="form-horizontal">

                                    <div class="control-group">
                                        <label for="ddlbiTipoUbicacion" class="control-label">Tipo de Ubicación:</label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddlbiTipoUbicacion" runat="server"
                                                OnSelectedIndexChanged="ddlbiTipoUbicacion_SelectedIndexChanged"
                                                AutoPostBack="true" Width="350px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label for="ddlUbiSubTipoUbicacion" class="control-label">Subtipo de Ubicación:</label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddlUbiSubTipoUbicacion" runat="server"
                                                Width="350px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                </div>
            </div>

        </div>

    </div>



    <asp:UpdatePanel ID="btn_BuscarTramite" runat="server" style="margin-top:10px" UpdateMode="Conditional" >
        <ContentTemplate>

                <div class="pull-right">
                    
                <div class="control-group inline-block">
                    <asp:UpdateProgress ID="updPrgss_BuscarTramite" AssociatedUpdatePanelID="btn_BuscarTramite" runat="server" DisplayAfter="0"  >
                    <ProgressTemplate>
                        <img src="../Content/img/app/Loading24x24.gif" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                </div>

                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="buscar" OnClick="btnBuscar_OnClick">
                    <i class="icon-white icon-search"></i>
                    <span class="text">Buscar</span>
                    </asp:LinkButton>

                <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn" OnClick="btnLimpiar_Click">
                    <i class="icon-refresh"></i>
                    <span class="text">Limpiar</span>
                </asp:LinkButton>


                </div>
                <asp:Panel ID="PanelBoton" runat="server" Visible="true" CssClass="control-group" style="margin-bottom:0px">
                    <span class="btn btn-primary " onclick="showfrmAgregarUbicacion();">
                <i class="imoon imoon-plus"></i>
                <span class="text">Agregar Ubicaci&oacute;n</span>
            </span>
                </asp:Panel>
            </ContentTemplate>
    </asp:UpdatePanel>

    </asp:Panel>
    </div>

    <div id="box_resultado" class="widget-box" style="display:none;">
         <asp:UpdatePanel ID="updPnlUbicacionesClausuradas" runat="server" class="mleft10 mright10 mtop10" UpdateMode="Conditional">
                    <ContentTemplate>
                        
            <asp:Panel ID="pnlResultadoBuscar" runat="server">

                <asp:Panel ID="pnlCantRegistros" runat="server" Visible="false">

                    <div style="display: inline-block;margin-left:10px">
                        
                        <h5>
                            <asp:Label ID="lblUbicClausuradas" runat="server" Text="Ubicaciones con iregularidades"></asp:Label></h5>
                    </div>
                    <div style="display: inline-block">
                        (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server" ></asp:Label></span>
                        )
                    </div>

                </asp:Panel>
                        <asp:GridView ID="grdUbicacionesClausuradas" runat="server" 
                            AutoGenerateColumns="false" 
                            GridLines="None" 
                            CssClass="table table-bordered table-striped table-hover with-check"
                            ItemType="SGI.Model.clsItemGrillaUbicacionesClausuradas" 
                            AllowPaging="true" 
                            PageSize="30" 
                            OnRowDataBound="grdUbicacionesClausuradas_RowDataBound"
                            OnPageIndexChanging="grdResultados_PageIndexChanging"
                            OnDataBound="grdResultados_DataBound">
                            <Columns>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="NroPartidaMatriz" HeaderText="N° Partida Matriz/Horizontal" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Seccion" HeaderText="Sección" />
                                <asp:BoundField DataField="Manzana" HeaderText="Manzana" />
                                <asp:BoundField DataField="Parcela" HeaderText="Parcela" />
                                <asp:BoundField DataField="domicilio" HeaderText="Domicilio" />
                                <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                                <asp:BoundField DataField="fecha_alta_clausura" HeaderText="Fecha alta de Clausura" />
                                <asp:BoundField DataField="fecha_baja_clausura" HeaderText="Fecha baja de Clausura" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-center" ItemStyle-Width="3px">
                                            <ItemTemplate>
                                                 <asp:LinkButton ID="lnkModificar" runat="server" CssClass="" ToolTip="Editar" data-toggle="tooltip"
                                                     CommandArgument='<%#Eval("id_ubicclausura")+","+Eval("Tipo")%>' OnClick="lnkModificar_Click">
                                            <span class="icon" style="color:#337AB7"><i class="icon-edit"></i></span>
                                        </asp:LinkButton>

                                                <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="" ToolTip="Eliminar" data-toggle="tooltip"
                                                    CommandArgument='<%#Eval("id_ubicclausura")+","+Eval("Tipo")%>' OnClick="lnkEliminar_Click">
                                            <span class="icon" style="color:#337AB7"><i class="icon-remove"></i></span>
                                        </asp:LinkButton>
                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>

                            </Columns>
                            <PagerTemplate>
                                <asp:Panel ID="pnlPager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                        <asp:Button ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click"
                                            CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn btn-sm btn-default" />
                                        <asp:Button ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn btn-xs btn-default" />
                                        <asp:Button ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click"
                                            CssClass="btn btn-sm btn-default"  />
                                    </asp:Panel>
                                </PagerTemplate>

                                <EmptyDataTemplate>

                                    <div>

                                        <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                        <span class="mleft20">No se encontraron registros.</span>

                                    </div>

                                </EmptyDataTemplate>
                        </asp:GridView>
            </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
        </div>


    <asp:UpdatePanel ID="updhidden" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hid_accion" runat="server" EnableViewState="true" ViewStateMode="Enabled" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_ID_ubic" runat="server"  EnableViewState="true" ViewStateMode="Enabled"  ClientIDMode="Static"  />
            <asp:HiddenField ID="hid_ID_ubicclausura" runat="server"  EnableViewState="true" ViewStateMode="Enabled" ClientIDMode="Static"  />
            <asp:HiddenField ID="hid_ID_tipo" runat="server"  EnableViewState="true" ViewStateMode="Enabled" ClientIDMode="Static"  />
            <asp:HiddenField ID="hid_IDs_horizontal" runat="server"  EnableViewState="true" ViewStateMode="Enabled" ClientIDMode="Static"  />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <%-- Modal Datos Ubicación --%>
    <asp:UpdatePanel ID="updPnlDatosUbic" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="frmDatosUbicacion" class="modal fade" style="width: auto;" role="dialog">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Datos de la clausura</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblMotivo" runat="server" AssociatedControlID="txtMotivo" Text="Motivo de clausura:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtMotivo" TextMode="MultiLine" runat="server" MaxLength="8000" Width="300px" Height="100px"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblFechaAlta" runat="server" AssociatedControlID="txtFechaAlta" Text="Fecha de Alta de clausura:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaAlta" runat="server" MaxLength="10" Width="185px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaAlta" runat="server" ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaAlta" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblFechaBaja" runat="server" AssociatedControlID="txtFechaBaja" Text="Fecha de Baja de clausura:" class="control-label"></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtFechaBaja" runat="server" MaxLength="10" Width="185px"></asp:TextBox>
                                            <div class="req">
                                                <asp:RegularExpressionValidator
                                                    ID="rev_txtFechaBaja" runat="server" ValidationGroup="buscar"
                                                    ControlToValidate="txtFechaBaja" CssClass="field-validation-error"
                                                    ErrorMessage="Fecha invalida. Ingrese fecha con formato dd/mm/aaaa."
                                                    ValidationExpression="(([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.](19[0-9][0-9]|20[0-9][0-9]|[0-9][0-9]))|^[0-9]{5}\d*[0-9 ]$"
                                                    Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group mbottom0">
                                <div id="Req_FechaAlta" class="alert alert-small alert-danger mbottom0" style="display: none;">
                                    Debe ingresar la fecha de Alta.
                                </div>
                                <div id="Val_Formato_FechaAlta" class="alert alert-small alert-danger mbottom0" style="display: none;">
                                    El Formato de la fecha es inv&aacute;lido. El mismo debe ser dd/mm/yyyy.
                                </div>
                                <div id="Val_Formato_FechaBaja" class="alert alert-small alert-danger mbottom0" style="display: none;">
                                    El Formato de la fecha es inv&aacute;lido. El mismo debe ser dd/mm/yyyy.
                                </div>
                            </div>
                            <div class="pull-right mright20 mbottom20">
                                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-inverse" OnClick="btnGuardar_Click" OnClientClick="hidefrmDatosUbicacion();" >
                        <span class="text">Guardar</span>
                                </asp:LinkButton>
                            </div>
                            <div class="pull-right mright20 mbottom20">
                                <%--<asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-default" OnClientClick="hidefrmDatosUbicacion();">--%>
                                <a href="#" onclick="hidefrmDatosUbicacion();" class="btn btn-default">Cancelar</a>
                                <%--    <span class="text">Cancelar</span>
                        </asp:LinkButton>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- /.modal -->

    <%-- Modal Agregar Ubicación --%>
    <div id="frmAgregarUbicacion" class="modal fade" role="dialog" style="width: auto">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Buscar Ubicaci&oacute;n</h4>
                </div>
                <div class="modal-body">
                    <uc1:BuscarUbicacion runat="server" ID="BuscarUbicacion"
                        OnCerrarClientClick="hidefrmAgregarUbicacion();" OnAgregarUbicacionClick="BuscarUbicacion_AgregarUbicacionClick" />
                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->


    <%--Modal mensajes de error--%>
    <div id="frmError" class="modal fade" role="dialog">
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
                                <asp:UpdatePanel ID="updmpeInfo" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblError" runat="server" class="pad10"></asp:Label>
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

        function hidefrmAgregarUbicacion() {
            $("#frmAgregarUbicacion").modal("hide");
            return false;
        }

        function showfrmAgregarUbicacion() {
            $("#frmAgregarUbicacion").modal("show");
            return false;
        }

        function showfrmDatosUbicacion() {
            $("#frmDatosUbicacion").modal({
                "show": true,
                "backdrop": "static",
                "keyboard": false
            });
            return false;
        }

        function hidefrmDatosUbicacion() {
            $("#frmDatosUbicacion").modal("hide");
            return false;
        }

        function showfrmError() {
            $("#pnlBotonesGuardar").show();
            $("#frmError").modal("show");
            return false;
        }

    </script>
</asp:Content>

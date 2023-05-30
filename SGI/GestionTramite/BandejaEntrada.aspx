<%@ Page Title="Bandeja de Entrada" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BandejaEntrada.aspx.cs" Inherits="SGI.BandejaEntrada" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/gritter") %>



    <hgroup class="title">
        <h1>Bandeja de Entrada</h1>
    </hgroup>

    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="lblTiempo1" runat="server"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblTiempo2" runat="server"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblTiempo3" runat="server"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblTiempo4" runat="server"></asp:Label>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="widget-box">
        <asp:Panel ID="pnlTipoBandeja" runat="server">
            <div class="widget-content">
                <div id="idTipoBandeja_txt" style="display: table-cell; vertical-align: middle;" runat="server">
                    Tipo de Bandeja
                </div>
                <div class="btn-group" data-toggle="buttons-radio" style="display: table-cell; padding-left: 10px" id="idTipoBandeja_btn" runat="server">
                    <asp:Button ID="btnPropia" runat="server" type="button" class="btn active" Text="Propia" OnClick="btnPropia_click" />
                    <asp:Button ID="btnAsignacion" runat="server" type="button" class="btn" Text="Asignación" OnClick="btnAsignacion_click" />
                </div>
                <div class="widget-content">
                    <div id="bandejaFilter" runat="server" style="display: table-cell; vertical-align: middle;">

                        <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlTipoTramite"
                            Text="Tipo tramite" Style="padding-right: 5px; padding-left: 0px; font-size: small; vertical-align: middle;"></asp:Label>

                        <asp:DropDownList ID="ddlTipoTramite" runat="server" Width="250px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTipoTramite_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:Label ID="lblTarea" runat="server" AssociatedControlID="ddlTarea"
                            Text="Tarea" Style="padding-right: 5px; padding-left: 0px; font-size: small; vertical-align: middle;"></asp:Label>

                        <asp:DropDownList ID="ddlTarea" runat="server" Width="250px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTarea_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:Label ID="lblAsignacion" runat="server" AssociatedControlID="ddlAsignacion"
                            Text="Asignación" Style="padding-right: 5px; vertical-align: middle; padding-left: 50px; font-size: small;"></asp:Label>
                        <asp:DropDownList ID="ddlAsignacion" Style="padding-right: 5px;"
                            OnSelectedIndexChanged="ddlAsignacion_SelectedIndexChanged" runat="server" Width="100px" AutoPostBack="true">
                            <asp:ListItem Value="">Todas</asp:ListItem>
                            <asp:ListItem Value="false">Sin asignar</asp:ListItem>
                            <asp:ListItem Value="true">Asignadas</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="bandejaFilterAsignacion" runat="server" style="display: table-cell; vertical-align: middle;">
                        <asp:Label ID="lblTareaAsignacion" runat="server" 
                            Text="Tarea" Style="padding-right: 5px; padding-left: 0px; font-size: small; vertical-align: middle;"></asp:Label>

                        <asp:UpdatePanel ID="updddlTareaAsignacion" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTareaAsignacion" runat="server" Width="450px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTareaAsignacion_SelectedIndexChanged">
                                </asp:DropDownList>

                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updddlTareaAsignacion">
                                    <ProgressTemplate>
                                        <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />Filtrando...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:HiddenField ID="hid_userid" runat="server" />

        <div id="BandejaPropia" runat="server" class="widget-content">

            <asp:UpdatePanel ID="updBandejaPropia" runat="server">
                <ContentTemplate>
                    <div style="display: inline-block">
                        <h5>Lista de trámites</h5>
                    </div>
                    <div style="display: inline-block">
                        (<span class="badge"><asp:Label ID="lblCantTramitesBandejaPropia" runat="server"></asp:Label></span>
                        <asp:LinkButton ID="btnActualizarBandejaPropia" runat="server" OnClick="btnActualizarBandejaPropia_Click" CssClass="btn btn-primary btn-mini">
                            <i class="icon-white icon-refresh"></i>
                            <span class="text">Actualizar</span>
                        </asp:LinkButton>
                        )
                        
                        <div class="control-group inline-block">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updBandejaPropia">
                                <ProgressTemplate>
                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>

                    </div>
                    <asp:GridView ID="grdBandeja" runat="server" AutoGenerateColumns="false"
                        GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                        DataKeyNames="id_solicitud,url_tareaTramite,cod_grupotramite" AllowPaging="true" PageSize="30"
                        ItemType="SGI.Model.clsItemBandejaEntrada" SelectMethod="GetTramitesBandeja" AllowSorting="true"
                        OnDataBound="grdBandeja_DataBound" >
                        <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                        <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />

                        <Columns>

                            <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="50px" ItemStyle-CssClass="align-center" SortExpression="id_solicitud">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# Item.url_visorTramite%>'><%# Item.id_solicitud %></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="tipoTramite" ItemStyle-Width="90px" HeaderText="Tipo Tramite" SortExpression="tipoTramite" />

                            <asp:TemplateField ItemStyle-Width="15px">
                                <ItemTemplate>

                                    <asp:LinkButton ID="lnkRubros" runat="server" OnClientClick="return popOverRubros(this);" data-toggle="popover" data-visible="false" data-placement="right" CommandArgument="<%# Item.id_solicitud %>" title="Lista de rubros">
                                        <i class="icon-share"></i>
                                    </asp:LinkButton>

                                    <%--Popover con la lista de Rubros--%>
                                    <asp:Panel ID="pnlRubros" runat="server" Style="display: none; min-width: 800px; padding: 10px">


                                        <asp:DataList ID="lstRubros" runat="server" Width="500px" CssClass="table table-bordered table-striped"   DataSource='<%# Item.Rubros %>'>
                                            <ItemTemplate>
                                                <div class="inline">
                                                    <asp:Label ID="lblCodRubro" runat="server" CssClass="badge badge-info"><%# Eval("cod_rubro") %></asp:Label>
                                                </div>
                                                <div class="inline">
                                                    <asp:Label ID="lblDescRubro" runat="server" CssClass="pLeft5"><%# Eval("Desc_rubro") %></asp:Label>
                                                </div>

                                            </ItemTemplate>

                                        </asp:DataList>
                                    </asp:Panel>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:BoundField DataField="superficie_total" ItemStyle-Width="60px" HeaderText="Superficie Total" SortExpression="superficie_total" />

                            
                            <asp:BoundField DataField="direccion" HeaderText="Ubicación" />

                            <asp:TemplateField HeaderText="Tarea" ItemStyle-Width="10%" ItemStyle-CssClass="align-center" SortExpression="nombre_tarea">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkid_tarea" runat="server" NavigateUrl='<%# Item.url_tareaTramite%>'><%# Item.nombre_tarea %></asp:HyperLink>
                                    <asp:HiddenField ID="hiddenIdTramiteTarea" runat="server" Value='<%# Item.id_tramitetarea %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FechaInicio_tramitetarea" HeaderText="Tarea creada el" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaInicio_tarea" />


                            <asp:TemplateField HeaderText="Asignada el" ItemStyle-Width="85px" ItemStyle-CssClass="align-center" SortExpression="FechaAsignacion_tramtietarea">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaAsignada" runat="server" Text='<%# (Item.FechaAsignacion_tramtietarea.HasValue ? Item.FechaAsignacion_tramtietarea.Value.ToString("dd/MM/yyyy"): "")  %>'></asp:Label>
                                    <asp:LinkButton ID="lnkTomarTarea" runat="server" title="Haga click aquí para asignarse la tarea" CommandArgument="<%# Item.id_tramitetarea %>"
                                        Text="Tomar" OnClick="lnkTomarTarea_Click" Visible="<%# (Item.FechaAsignacion_tramtietarea == null && Item.tomar_tarea ) %>"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="dias_transcurridos" HeaderText="Días" HeaderStyle-Wrap="true" HeaderStyle-Width="5%" ItemStyle-CssClass="align-center" />
                            <asp:BoundField DataField="cant_observaciones" HeaderText="Observaciones" HeaderStyle-Wrap="true" HeaderStyle-Width="5%" ItemStyle-CssClass="align-center" SortExpression="cant_observaciones" />
                            
                            <asp:TemplateField HeaderText="SADE OK" HeaderStyle-Wrap="true" HeaderStyle-Width="3%" ItemStyle-CssClass="align-center">
                                <ItemTemplate>
                                    <asp:Literal ID="status_sade" runat="server" Text='<%# Convert.ToBoolean(Eval("continuar_sade")) ? "<span class=\"btn-right\"><i class=\"imoon imoon-checkmark\" style=\"color: #4eb87c;\"></i></span>" : "<span class=\"btn-right\"><i class=\"imoon imoon-blocked\" style=\"color: #690d10;\"></i></span>" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                <asp:LinkButton ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click"
                                    CssClass="btn" />

                                <asp:LinkButton ID="cmdPage1" runat="server" Text="1" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click"
                                    CssClass="btn" />
                            </asp:Panel>
                        </PagerTemplate>

                    </asp:GridView>

                    <asp:Panel ID="pnlBandejaPropiaVacia" runat="server" CssClass="GrayText-1 ptop10" Visible="false">
                        <p>
                            En este momento no hay tr&aacute;mites en la bandeja.
                        </p>
                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div id="BandejaAsignacion" runat="server" class="widget-content">

            <asp:UpdatePanel ID="updBandejaAsignacion" runat="server" >

                <ContentTemplate>

                    <div style="display: inline-block">
                        <h5>Lista de trámites para asignar</h5>
                    </div>

                    <div style="display: inline-block;">
                        (<span class="badge"><asp:Label ID="lblCantTramitesBandejaAsignacion" runat="server"></asp:Label></span>
                        <asp:LinkButton ID="btnActualizarBandejaAsignacion" runat="server" OnClick="btnActualizarBandejaAsignacion_Click" CssClass="btn btn-primary btn-mini">
                            <i class="icon-white icon-refresh"></i>
                            <span class="text">Actualizar</span>
                        </asp:LinkButton>
                        )
                    </div>
                    <div class="control-group inline-block">
                        <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updBandejaAsignacion">
                            <ProgressTemplate>
                                <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>



                    <table style="float: right">
                        <tr>
                            <td>
                                <div style="float: right;">
                                    <a id="lnkEmpleados" class="btn" href="#" rel="popover" onclick="popOver();" data-placement="left" data-toggle="popover">
                                        <i class="icon-share"></i>
                                        <span class="text">Asignar a ...</span>
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="btn-group" style="text-align: left; float: right">
                                    <asp:LinkButton ID="btnSeleccionar" runat="server" data-toggle="dropdown" CssClass="btn dropdown-toggle">Seleccionar<span class="caret"></span></asp:LinkButton>
                                    <ul class="dropdown-menu" style="min-width: 120px">
                                        <li>
                                            <asp:LinkButton ID="lnkBASelectAll" runat="server" OnClick="BandejaAsignacionCheck" CommandArgument="0">Todo</asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkBASelectNone" runat="server" OnClick="BandejaAsignacionCheck" CommandArgument="1">Nada</asp:LinkButton></li>
                                    </ul>
                                </div>

                            </td>
                        </tr>
                    </table>


                    <asp:GridView ID="grdBandejaAsignacion" runat="server" AutoGenerateColumns="false" GridLines="None"
                        CssClass="table table-bordered table-striped table-hover with-check"
                        ItemType="SGI.Model.clsItemBandejaEntradaAsignacion" SelectMethod="GetTramitesBandejaAsignacion"
                        AllowPaging="true" AllowSorting="true" OnDataBound="grdBandejaAsignacion_DataBound" PageSize="30">

                        <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                        <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />

                        <Columns>

                            <asp:HyperLinkField DataTextField="id_solicitud" DataNavigateUrlFormatString="~/GestionTramite/VisorTramite.aspx?id={0}" DataNavigateUrlFields="id_solicitud"
                                HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="sol.id_solicitud" />

                            <asp:TemplateField ItemStyle-Width="15px">
                                <ItemTemplate>

                                    <asp:LinkButton ID="lnkRubrosAsig" runat="server" OnClientClick="return popOverRubros(this);" data-toggle="popover" data-visible="false" data-placement="right" CommandArgument="<%# Item.id_solicitud %>" title="Lista de rubros">
                                        <i class="icon-share"></i>
                                    </asp:LinkButton>

                                    <%--Popover con la lista de Rubros--%>
                                    <asp:Panel ID="pnlRubrosAsig" runat="server" Style="display: none; min-width: 800px; padding: 10px">

                                        <asp:DataList ID="lstRubrosAsig" runat="server" Width="500px" CssClass="table table-bordered table-striped" DataSource='<%# Item.Rubros %>'>
                                            <ItemTemplate>
                                                <div class="inline">
                                                    <asp:Label ID="lblCodRubroAsig" runat="server" CssClass="badge badge-info"><%# Eval("cod_rubro") %></asp:Label>
                                                </div>
                                                <div class="inline">
                                                    <asp:Label ID="lblDescRubroAsig" runat="server" CssClass="pLeft5"><%# Eval("Desc_rubro") %></asp:Label>
                                                </div>

                                            </ItemTemplate>

                                        </asp:DataList>
                                    </asp:Panel>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:BoundField DataField="superficie_total" ItemStyle-Width="60px" HeaderText="Superficie Total" SortExpression="superficie_total" />

                            <%-- %><asp:BoundField DataField="zona_declarada" ItemStyle-Width="90px" HeaderText="Distrito de Zonificación" SortExpression="zona_declarada" />--%>

                            <asp:BoundField DataField="direccion" HeaderText="Ubicación" SortExpression="direccion" />

                            <asp:HyperLinkField DataNavigateUrlFormatString="~/GestionTramite/Tareas/{0}?id={1}" DataNavigateUrlFields="formulario_tarea,id_tramitetarea" DataTextField="nombre_tarea" HeaderText="Tarea"
                                ItemStyle-Width="200px" SortExpression="nombre_tarea" />

                            <asp:BoundField DataField="FechaInicio_tramitetarea" HeaderText="Tarea creada el" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaInicio_tramitetarea" />
                            <asp:BoundField DataField="dias_transcurridos" HeaderText="Días" HeaderStyle-Width="50px" ItemStyle-CssClass="align-center" SortExpression="DATEDIFF(dd,tramite_tareas.FechaInicio_tramitetarea,GETDATE())" />

                            <asp:TemplateField HeaderText="" ItemStyle-Width="35px" ItemStyle-CssClass="align-center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSeleccionado" runat="server" CssClass="checker" onclick="visibilidadBotonAsignar();" />
                                    <asp:HiddenField ID="hid_RoleId_Asignador" runat="server" Value="<%# Item.id_perfil_asignador.ToString() %>" />
                                    <asp:HiddenField ID="hid_RoleId_Asignado" runat="server" Value="<%# Item.id_perfil_asignado.ToString() %>" />
                                    <asp:HiddenField ID="hid_id_tramitetarea" runat="server" Value="<%# Item.id_tramitetarea %>" />

                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                        <PagerTemplate>
                            <asp:Panel ID="pnlpagerAsig" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">

                                <asp:LinkButton ID="cmdAnteriorAsig" runat="server" Text="<<" OnClick="cmdAnteriorAsig_Click"
                                    CssClass="btn" />

                                <asp:LinkButton ID="cmdPageAsig1" runat="server" Text="1" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig2" runat="server" Text="2" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig3" runat="server" Text="3" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig4" runat="server" Text="4" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig5" runat="server" Text="5" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig6" runat="server" Text="6" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig7" runat="server" Text="7" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig8" runat="server" Text="8" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig9" runat="server" Text="9" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig10" runat="server" Text="10" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig11" runat="server" Text="11" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig12" runat="server" Text="12" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig13" runat="server" Text="13" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig14" runat="server" Text="14" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig15" runat="server" Text="15" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig16" runat="server" Text="16" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig17" runat="server" Text="17" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig18" runat="server" Text="18" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdPageAsig19" runat="server" Text="19" OnClick="cmdPageAsig" CssClass="btn" Style="max-width: 10px" />
                                <asp:LinkButton ID="cmdSiguienteAsig" runat="server" Text=">>" OnClick="cmdSiguienteAsig_Click"
                                    CssClass="btn" />
                            </asp:Panel>
                        </PagerTemplate>
                    </asp:GridView>


                    <asp:Panel ID="pnlBandejaAsignacionVacia" runat="server" CssClass="GrayText-1 ptop10" Visible="false">
                        <p>
                            En este momento no hay tr&aacute;mites para asignar.
                        </p>
                    </asp:Panel>


                </ContentTemplate>
            </asp:UpdatePanel>



        </div>

    </div>


    <%--Popover con la lista de empleados--%>
    <div id="Empleados" style="display: none; min-width: 400px">

        <div id="pnlEmpleados">

            <div id="imgLoadingEmpleados" style="width: 100%; text-align: center">
                <img src="../Content/img/app/Loading32x32.gif" alt="" />
                <label>Cargando el equipo de trabajo</label>
            </div>

            <ul id="lstEmpleados" class="unstyled">
            </ul>
            <div id="divAlertAsignar" class="alert alert-info">Presione sobre el nombre de la persona para asignar el trámite.</div>
        </div>
    </div>


    <script type="text/javascript">

        var popoverVisible = false;

        $(document).ready(function () {
            onload();
            inicializar_componentes();
            init_Js_updddlTareaAsignacion();
        });

        function init_Js_updddlTareaAsignacion()
        {
            
            $("#<%: ddlTareaAsignacion.ClientID %>").select2({
                allowClear: true,
                placeholder: "Seleccione"
            });
            return false;
        }

        function inicializar_componentes() {
            $("#<%: ddlTipoTramite.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlTarea.ClientID %>").select2({ allowClear: true });
            $("#<%: ddlAsignacion.ClientID %>").select2({ allowClear: true });
        }

        function onload() {
            //$('input[type=checkbox],input[type=radio],input[type=file]').uniform();
            
            popoverVisible = false;
            $("[id*='lnkTomarTarea']").tooltip({ delay: { show: 1000, hide: 100 } });
            $("[id*='lnkRubros']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });
            var obj = $("#Empleados").html();

            $("#lnkEmpleados").popover({ title: 'Equipo de trabajo', content: obj, html: 'true' });


            // Popovers rubros de la bandeja propia
            $("[id*='MainContent_grdBandeja_lnkRubros_']").each(function () {

                var id_pnlRubros = $(this).attr("id").replace("MainContent_grdBandeja_lnkRubros_", "MainContent_grdBandeja_pnlRubros_");
                var objRubros = $("#" + id_pnlRubros).html();
                $(this).popover({ title: 'Rubros', content: objRubros, html: 'true' });

            });
            // Popovers rubros de la bandeja de asignación
            $("[id*='MainContent_grdBandejaAsignacion_lnkRubrosAsig_']").each(function () {

                var id_pnlRubrosAsig = $(this).attr("id").replace("MainContent_grdBandejaAsignacion_lnkRubrosAsig_", "MainContent_grdBandejaAsignacion_pnlRubrosAsig_");
                var objRubrosAsig = $("#" + id_pnlRubrosAsig).html();
                $(this).popover({ title: 'Rubros', content: objRubrosAsig, html: 'true' });

            });

            visibilidadBotonAsignar()


        }

        function popOver() {

            var arrtareas = GetTareasSeleccionadas();


            if (popoverVisible) {
                $("#lnkEmpleados").popover();
                popoverVisible = false;
            }
            else {
                popoverVisible = true;
                if (arrtareas.length > 0) {
                    cargarEmpleados();
                }

            }


            return true;
        }


        function cargarEmpleados() {

            $("#imgLoadingEmpleados").show();

            var p_userid = $("[id*='hid_userid']").val();
            var p_RoleId_Asignado = "";

            $("#<%: grdBandejaAsignacion.ClientID %> :checkbox:checked").each(function () {

                var id_obj_hid_RoleId_Asignado = $(this).attr("id").replace("chkSeleccionado", "hid_RoleId_Asignado");
                p_RoleId_Asignado = $("#" + id_obj_hid_RoleId_Asignado).val();

                // alcanza con el primer rol encontrado, el return solo sale del each
                return false;

            });

            $.ajax({

                url: "<%: ResolveUrl("~/Webservices/Servicios.asmx/GetEquipoDeTrabajo") %>", type: "POST", dataType: "xml",
                data: { userid: p_userid, id_perfil_asignado: p_RoleId_Asignado },
                success: function (data) {


                    $("#lstEmpleados > li").remove();
                    $("#lstEmpleados > a").remove();
                    $("#lstEmpleados").empty();

                    $(data).find("clsEquipoTrabajo").each(function (indice_elemento) {

                        var nombres_apellido = $(this).find("nombres_apellido").text();
                        var userid = $(this).find("userid").text();
                        var tramites = $(this).find("tramites").text();

                        var id = "ctl00$MainContent$lnkAignarEmpleado" + indice_elemento;
                        var lnk = ' <button id="' + id + '" onclick="return onclickAsignarEmpleado(&#39;' + userid + '&#39;,&#39;&#39;)" class="btn btn-link">' + nombres_apellido + '</button><span class="badge" >' + tramites + '</span> ';


                        $("#lstEmpleados").append("<li style='padding-top: 5px'>");
                        $("#lstEmpleados").append(lnk);
                        $("#lstEmpleados").append("</li>");
                    });
                    $("#imgLoadingEmpleados").hide();
                    var obj = $("#Empleados").html();
                    $("#lnkEmpleados").popover({ title: 'Equipo de trabajo', content: obj, html: 'true' });

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#imgLoadingEmpleados").hide();
                    alert(textStatus);
                }

            });
        }

        function GetTareasSeleccionadas() {
            var arrtareas = new Array();
            var i = 0;
            $("#<%: grdBandejaAsignacion.ClientID %> :checkbox:checked ").each(function () {

                var objTareaId = $(this).attr("id").replace("MainContent_grdBandejaAsignacion_chkSeleccionado_", "MainContent_grdBandejaAsignacion_hid_id_tramitetarea_");
                arrtareas.push($("#" + objTareaId).val());
                i++;

            });

            return arrtareas;
        }


        function onclickAsignarEmpleado(puserid) {
            debugger;
            var arrtareas = GetTareasSeleccionadas();

            if (arrtareas.length > 0) {
                // Si se tildo alguna tarea

                $.ajax({
                    url: "../Webservices/Servicios.asmx/AsignarEmpleado", type: "POST", dataType: "xml",
                    data: { userid_a_asignar: puserid, ids_tramite_tarea: arrtareas.join(",") },
                    success: function (data) {

                        $("#lnkEmpleados").click();

                        eval($("#<%= btnActualizarBandejaAsignacion.ClientID %>").attr('href'));

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("El usuario no tiene permisos para tomar esta tarea.");

                    }
                });

            }
            return false;

        }

        function visibilidadBotonAsignar() {
            var arrtareas = GetTareasSeleccionadas();

            if (arrtareas.length > 0)
                $("#lnkEmpleados").show();
            else {

                $("#lnkEmpleados").hide();
                if (popoverVisible) {
                    $("#lnkEmpleados").click();
                    popoverVisible = false;
                }
            }

            return true;
        }
        function mostratMensaje(texto) {

            $.gritter.add({
                title: 'Bandeja',
                text: texto,
                image: '../Content/img/info32.png',
                sticky: false
            });

        }
        

        function popOverRubros(obj) {
            if ($(obj).attr("data-visible") == "true") {
                $(obj).attr("data-visible", "false");
            }
            else {
                $("[data-visible='true']").popover("toggle");
                $("[data-visible='true']").attr("data-visible", "false");
                $(obj).attr("data-visible", "true");
            }
            return false;
        }


    </script>

</asp:Content>


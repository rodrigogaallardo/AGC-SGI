<%@ Page 
    Title="Administrar archivos de una solicitud"
    MasterPageFile="~/Site.Master"
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="AdministrarArchivosDeUnaSolicitud.aspx.cs" 
    Inherits="SGI.Operaciones.AdministrarArchivosDeUnaSolicitud" 
%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <%: Scripts.Render("~/bundles/autoNumeric") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/select2Css") %>
    <script src="../Scripts/Select2-locales/select2_locale_es.js"></script>
    <script src="../Scripts/Funciones.js" type="text/javascript"></script>
    <script src="../Scripts/Datepicker_es.js" type="text/javascript"></script>

    <%: Styles.Render("~/Content/themes/base/css") %>

    <hgroup class="title">
        <h1><%= Title %>.</h1>
    </hgroup>

    <div class="control-group">
        <label class="control-label" for="txtBuscarSolicitud">Buscar por N&uacute;mero de Solicitud</label>
        <div class="controls">
            <asp:TextBox id="txtBuscarSolicitud" runat="server" CssClass="controls"/>
        </div>
    </div>

    <div class="control-group">
        <asp:Button id="btnBuscarSolicitud" runat="server" Text="Buscar" OnClick="btnBuscarSolicitud_Click" CssClass="btn btn-primary"/>
    </div>

    <hr/>

    <div id="box_resultado" style="display:none;">
        <asp:HiddenField ID="hid_valor_boton" runat="server" />
        <asp:HiddenField ID="hid_observaciones" runat="server" />
        <div class="widget-box">
            <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal" style="margin-left:10px; margin-right:10px">
                        <asp:Panel ID="pnlCantidadRegistros" runat="server" Visible="false" CssClass="form-horizontal">
                            <div style="display: inline-block;margin-left:10px;">
                                <h5>Lista de Documentos Adjuntos </h5>
                            </div>
                            <div style="display: inline-block">
                                (<span class="badge"><asp:Label ID="lblCantRegistros" runat="server" ></asp:Label></span>)
                            </div>
                        </asp:Panel>
                        <asp:GridView id="gridViewArchivosSolic" 
                            runat="server"
                            Width="100%" 
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            AutoGenerateColumns="false"
                            SelectMethod="CargarSolicitudConArchivos" 
                            AllowPaging="true" 
                            AllowSorting="true" 
                            PageSize="10" 
                            OnPageIndexChanging="gridViewArchivosSolic_PageIndexChanging"
                            OnDataBound="gridViewArchivosSolic_DataBound" 
                            OnRowDataBound="gridViewArchivosSolic_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdDocAdjunto" runat="server" Text='<%# Bind("id_docadjunto") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdSolicitud" runat="server" Text='<%# Bind("id_solicitud") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tipo Doc. Requerido" HeaderStyle-Wrap="True">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdTdocReq" runat="server"><%# Eval("TiposDeDocumentosRequeridos.nombre_tdocreq") %></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Detalle">
                                    <ItemTemplate>
                                        <asp:Label ID="labelTdocReqDetalle" runat="server" Text='<%# Bind("tdocreq_detalle") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tipo Doc. Sistema">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdTipDocSis" runat="server"><%# Eval("TiposDeDocumentosSistema.nombre_tipodocsis") %></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="N° de Archivo">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdFile" runat="server" Text='<%# Bind("id_file") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Generado Por Sistema" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="labelGeneradoXSistena" runat="server" Enabled=False Checked='<%# Eval("generadoxsistema") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha de Creacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelCreateDate" runat="server" Text='<%# Bind("CreateDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Usuario de Creacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUsuarioCreador" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha de Modificacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUpdateDate" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Usuario de Modificacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUsuarioModificador" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Nombre de Archivo">
                                    <ItemTemplate>
                                        <asp:Label ID="labelNombreArchivo" runat="server" Text='<%# Bind("nombre_archivo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha Presentado">
                                    <ItemTemplate>
                                        <asp:Label ID="labelFechaPresentado" runat="server" Text='<%# Bind("fechaPresentado") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Excluir Subida SADE" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="labelExcluirSubidaSADE" runat="server" Enabled=False Checked='<%# Bind("excluirSubidaSade") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:LinkButton HeaderText="Eliminar"
                                                ID="lnkEliminarDocSolic" runat="server" 
                                                CommandArgument='<%# Eval("id_docadjunto") %>' 
                                                CommandName ='<%# Eval("id_file") %>' 
                                                OnClientClick="javascript:return tda_confirm_del();"
                                                OnCommand="lnkEliminarDocSolic_Command" 
                                                Width="70px">
                                            <i class="icon icon-trash"></i> 
                                            <span class="text">Eliminar</span></a>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate>
                                <div>
                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                    <span class="mleft20">No se encontraron registros.</span>
                                </div>
                            </EmptyDataTemplate>

                            <PagerTemplate>
                                <asp:Panel ID="pnlpagerSolic" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                    <div style="display:inline-table">
                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
                                            DisplayAfter="0">
                                            <ProgressTemplate>
                                                <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <asp:LinkButton ID="cmdAnteriorSolic" runat="server" Text="<<" OnClick="cmdAnteriorSolic_Click" CssClass="btn" />
                                    <asp:LinkButton ID="cmdPageSolic1" runat="server" Text="1" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic2" runat="server" Text="2" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic3" runat="server" Text="3" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic4" runat="server" Text="4" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic5" runat="server" Text="5" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic6" runat="server" Text="6" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic7" runat="server" Text="7" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic8" runat="server" Text="8" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic9" runat="server" Text="9" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic10" runat="server" Text="10" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic11" runat="server" Text="11" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic12" runat="server" Text="12" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic13" runat="server" Text="13" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic14" runat="server" Text="14" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic15" runat="server" Text="15" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic16" runat="server" Text="16" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic17" runat="server" Text="17" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic18" runat="server" Text="18" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageSolic19" runat="server" Text="19" OnClick="cmdPageSolic" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdSiguienteSolic" runat="server" Text=">>" OnClick="cmdSiguienteSolic_Click" CssClass="btn" />
                                </asp:Panel>
                            </PagerTemplate>
                        </asp:GridView>
                        <asp:GridView id="gridViewArchivosTransf" 
                            runat="server"
                            Width="100%" 
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            AutoGenerateColumns="false"
                            SelectMethod="CargarTransferenciasConArchivos" 
                            AllowPaging="true" 
                            AllowSorting="true" 
                            PageSize="10" 
                            OnPageIndexChanging="gridViewArchivosTransf_PageIndexChanging"
                            OnDataBound="gridViewArchivosTransf_DataBound" 
                            OnRowDataBound="gridViewArchivosTransf_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdDocAdjunto" runat="server" Text='<%# Bind("id_docadjunto") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdSolicitud" runat="server" Text='<%# Bind("id_solicitud") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tipo Doc. Requerido" HeaderStyle-Wrap="True">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdTdocReq" runat="server"><%# Eval("TiposDeDocumentosRequeridos.nombre_tdocreq") %></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Detalle">
                                    <ItemTemplate>
                                        <asp:Label ID="labelTdocReqDetalle" runat="server" Text='<%# Bind("tdocreq_detalle") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tipo Doc. Sistema">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdTipDocSis" runat="server"><%# Eval("TiposDeDocumentosSistema.nombre_tipodocsis") %></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="N° de Archivo">
                                    <ItemTemplate>
                                        <asp:Label ID="labelIdFile" runat="server" Text='<%# Bind("id_file") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Generado Por Sistema" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="labelGeneradoXSistena" runat="server" Enabled=False Checked='<%# Eval("generadoxsistema") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha de Creacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelCreateDate" runat="server" Text='<%# Bind("CreateDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Usuario de Creacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUsuarioCreador" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha de Modificacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUpdateDate" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Usuario de Modificacion">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUsuarioModificador" runat="server" Text='<%# Bind("aspnet_Users.UserName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Nombre de Archivo">
                                    <ItemTemplate>
                                        <asp:Label ID="labelNombreArchivo" runat="server" Text='<%# Bind("nombre_archivo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:LinkButton HeaderText="Eliminar"
                                            ID="lnkEliminarDocTrans" runat="server"
                                            CommandArgument='<%# Eval("id_docadjunto") %>'
                                            CommandName='<%# Eval("id_file") %>'
                                            OnClientClick="javascript:return tda_confirm_del();"
                                            OnCommand="lnkEliminarDocTrans_Command"
                                            Width="70px">
                                            <i class="icon icon-trash"></i> 
                                            <span class="text">Eliminar</span></a>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate>
                                <div>
                                    <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                    <span class="mleft20">No se encontraron registros.</span>
                                </div>
                            </EmptyDataTemplate>

                            <PagerTemplate>
                                <asp:Panel ID="pnlpagerTransf" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                    <div style="display:inline-table">
                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
                                            DisplayAfter="0">
                                            <ProgressTemplate>
                                                <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <asp:LinkButton ID="cmdAnteriorTransf" runat="server" Text="<<" OnClick="cmdAnteriorTransf_Click" CssClass="btn" />
                                    <asp:LinkButton ID="cmdPageTransf1" runat="server" Text="1" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf2" runat="server" Text="2" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf3" runat="server" Text="3" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf4" runat="server" Text="4" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf5" runat="server" Text="5" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf6" runat="server" Text="6" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf7" runat="server" Text="7" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf8" runat="server" Text="8" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf9" runat="server" Text="9" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf10" runat="server" Text="10" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf11" runat="server" Text="11" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf12" runat="server" Text="12" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf13" runat="server" Text="13" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf14" runat="server" Text="14" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf15" runat="server" Text="15" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf16" runat="server" Text="16" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf17" runat="server" Text="17" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf18" runat="server" Text="18" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdPageTransf19" runat="server" Text="19" OnClick="cmdPageTransf" CssClass="btn" Style="max-width: 10px" />
                                    <asp:LinkButton ID="cmdSiguienteTransf" runat="server" Text=">>" OnClick="cmdSiguienteTransf_Click" CssClass="btn" />
                                </asp:Panel>
                            </PagerTemplate>
                        </asp:GridView>
                         <br />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <hr/>

        <%-- Modal Observacion Solicitante--%>
        <div id="frmEliminarLog" class="modal fade" style="max-width: 400px;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Eliminar</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="control-label">Observaciones del Solicitante:</label>
                            <div class="controls">
                                <asp:TextBox ID="txtObservacionesSolicitante" runat="server" CssClass="form-control" Columns="10" Width="95%" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <%-- Botones --%>
                    <div class="modal-footer" style="text-align: left;">
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
                    </div>
                </div>
            </div>
        </div>





        <div class="control-group">
            <asp:Button id="btnAgregarArchivo" runat="server" Text="Agregar" OnClick="btnAgregarArchivo_Click" CssClass="btn btn-primary"/>
        </div>
    </div>    
    
    <script type="text/javascript">
        function tda_confirm_del() {
            return confirm('¿Esta seguro que desea eliminar este Registro?');
        }


        function showResultado() {
            $("#box_resultado").show("slow");
        }
    </script>
</asp:Content>
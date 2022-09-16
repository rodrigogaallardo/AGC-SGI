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
        <p class="lead mtop20">Archivos de la solicitud</p>
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
                        <asp:GridView id="gridViewArchivos" 
                            runat="server"
                            Width="100%" 
                            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                            AutoGenerateColumns="false"
                            SelectMethod="CargarSolicitudConArchivos" 
                            AllowPaging="true" 
                            AllowSorting="true" 
                            PageSize="10" 
                            OnPageIndexChanging="gridViewArchivos_PageIndexChanging"
                            OnDataBound="gridViewArchivos_DataBound" 
                            OnRowDataBound="gridViewArchivos_RowDataBound">
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
                                                ID="lnkEliminarDocAdj" runat="server" 
                                                CommandArgument='<%# Eval("id_docadjunto") %>' 
                                                CommandName ='<%# Eval("id_file") %>' 
                                                OnClientClick="javascript:return tda_confirm_del();"
                                                OnCommand="lnkEliminarDocAdj_Command" 
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
                                <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                    <div style="display:inline-table">
                                        <asp:UpdateProgress ID="updPrgssPager" AssociatedUpdatePanelID="updResultados" runat="server"
                                            DisplayAfter="0">
                                            <ProgressTemplate>
                                                <img src="../Content/img/app/Loading24x24.gif" alt="" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <asp:LinkButton ID="cmdAnterior" runat="server" Text="<<" OnClick="cmdAnterior_Click" CssClass="btn" />
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
                                    <asp:LinkButton ID="cmdSiguiente" runat="server" Text=">>" OnClick="cmdSiguiente_Click" CssClass="btn" />
                                </asp:Panel>
                            </PagerTemplate>
                        </asp:GridView>
                         <br />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <hr/>

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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProcesosSADEv1.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucProcesosSADEv1" %>
    <%--grilla para ver estado de las operaciones del expediente--%>
    <asp:UpdatePanel ID="updPnlGrillaResultadoExpediente" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="grdResultadoExpediente" runat="server" AutoGenerateColumns="false"
                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check small"
                DataKeyNames="id_tarea_proc">
                <Columns>
                    <asp:BoundField DataField="id_proceso" HeaderText="Nro. de Proceso" ItemStyle-CssClass="text-center" />
                    <asp:BoundField DataField="nombre_proceso" HeaderText="Descripción" ItemStyle-Width="180px" />
                    <asp:BoundField DataField="descripcion_tramite" HeaderText="Referencia" ItemStyle-Width="350px" />

                    <asp:TemplateField HeaderText="Resultado SADE">
                        <ItemTemplate>
                            <div>
                                <asp:Label ID="lblREsultadoSADE" CssClass='<%# Eval("class_resultado_SADE") %>' runat="server"> <%# Eval("resultado_sade") %> </asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="fecha_sade" HeaderText="Fecha SADE" ItemStyle-Width="110px" DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                    <asp:TemplateField HeaderText="Realizado en pasarela" ItemStyle-CssClass="text-center" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <div>
                                <asp:CheckBox ID="chkRealizadoEnPasarela" runat="server" Checked='<%# Eval("realizado_en_pasarela") %>' Enabled="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Realizado en SADE" ItemStyle-CssClass="text-center" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <div>
                                <asp:CheckBox ID="chkRealizadoEnSADE" runat="server" Checked='<%# Eval("realizado_en_SADE") %>' Enabled="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--modal de items a procesar--%>
    <div id="frmProcesarEE" class="modal fade text-center" style="min-width:1100px;left:35%; ">
         
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
            <h4 class="modal-title">Generando tareas en Expediente Electr&oacute;nico...</h4>
        </div>
        <div class="modal-body">

            <asp:UpdatePanel ID="updPnlGrillaProcesos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="pnlScroll" style="max-height: 550px; overflow-x: hidden; overflow-y: auto;">

                    <%--grilla para visualizar como se van ejecutando cada operacion del expediente en un modal--%>
                    <asp:GridView ID="grdProcesosExpediente" runat="server" AutoGenerateColumns="false"
                        GridLines="None" CssClass="table table-bordered table-striped table-hover with-check small"
                        DataKeyNames="id_tarea_proc">

                        <Columns>

                            <asp:BoundField DataField="id_proceso" HeaderText="Nro. de Proceso" ItemStyle-Width="30px" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="nombre_proceso" HeaderText="Descripción" ItemStyle-Width="180px" />
                                    
                            <asp:TemplateField ItemStyle-Width="350px" HeaderText="Referencia">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="updDescripcionTramite" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblDescripcionTramite" runat="server" ><%# Eval("descripcion_tramite") %></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField  HeaderText="Resultado SADE">
                                <ItemTemplate>
                                                    
                                        <asp:Label ID="lblResultadoSADE" runat="server" CssClass='<%# Eval("class_resultado_SADE") %>'><%# Eval("resultado_sade") %></asp:Label>
                                        <asp:Panel ID="pnlProcesando" runat="server" style="display:none">
                                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />
                                            Procesando...

                                        </asp:Panel>
                                                            
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Proceso ejecutado" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkEjecutado" runat="server" CssClass="checker" Checked='<%# Eval("ejecutado_anteriormente") %>'  Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="70px" HeaderText="Realizado en pasarela" ItemStyle-CssClass="text-center">
                                <ItemTemplate>

                                    <asp:UpdatePanel ID="updPnlItemGrillaProcesos" runat="server">
                                        <ContentTemplate>

                                            <div class="inline">
                                                <%--pull-left--%>
                                                <asp:CheckBox ID="chkRealizado_en_pasarela" runat="server" CssClass="checker" Checked='<%# Eval("realizado_en_pasarela") %>' Enabled="false" />
                                            </div>

                                            <div class="inline">
                                                <%-- pull-left--%>
                                                <%--solo para pruebas unitarias--%>
                                                <asp:Button ID="btnProcesarItemExpediente" runat="server"
                                                    Text="Procesar" Style="display: none"
                                                    CommandArgument='<%#Eval("id_tarea_proc")%>'
                                                    OnClick="btnProcesarItemExpediente_Click" />
                                            </div>
                                                  

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px" HeaderText="Realizado en SADE" ItemStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRealizado_en_SADE" runat="server" CssClass="checker" Checked='<%# Eval("realizado_en_SADE") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div class="modal-footer">
            <asp:UpdatePanel ID="updCerrarProcesos" runat="server">
                <ContentTemplate>
                    <asp:LinkButton ID="btnCerrarModalProcesos" runat="server" OnClick="btnCerrarModalProcesos_Click" CssClass="btn btn-default" OnClientClick="$(this).hide();">
                        <span class="text">Cerrar</span>
                    </asp:LinkButton>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="200" AssociatedUpdatePanelID="updCerrarProcesos">
                        <ProgressTemplate>
                            <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" style="margin-left: 10px" alt="loading" />Procesando...
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>
    </div>

 <%--Modal mensajes de error--%>
    <div id="frmError_ProcesosSADE" class="modal fade" style="display: none;">
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


        function ejecutargeneracionEnPasarela() {


            $("#frmProcesarEE").modal({
                backdrop: "static",
                show: true
            });

            $("#<%: btnCerrarModalProcesos.ClientID %>").hide();
            ejecutarProcesoSiguiente();

            return false;
        }

        function ejecutarProcesoSiguiente() {


            // oculta todos las imagenes de loading
            $("[id*='pnlProcesando']").hide();

            // obtiene el primer check que no está tildado
            var chkEjecutado_ID = $("[id*='chkEjecutado']").not(":checked").eq(0).prop("id");

            // Si existe un check no tildado ejecuta el botón de la misma fila (boton para procesar el item)
            // y pone visible el loading de la misma fila.
            //---
            if (chkEjecutado_ID != undefined) {
                var btnProcesar_ID = chkEjecutado_ID.replace("chkEjecutado", "btnProcesarItemExpediente");
                var pnlProcesando_ID = chkEjecutado_ID.replace("chkEjecutado", "pnlProcesando");
                $("#" + pnlProcesando_ID).css("display", "inline-block");
                $("#" + chkEjecutado_ID).prop("checked", true);
                $("#" + btnProcesar_ID).click();
            }
            else {
                $("#<%: btnCerrarModalProcesos.ClientID %>").show();
            }

            return false;
        }

        function finalizarEjecucionProcesos() {

            // oculta todos las imagenes de loading y pone visible el boton para cerrar el modal
            $("[id*='pnlProcesando_ID']").hide();
            $("#<%: btnCerrarModalProcesos.ClientID %>").show();
            return false;
        }

        function ocultarfrmProcesarEE() {
            $("#frmProcesarEE").modal("hide");
            return false;
        }

        function showfrmErrorProcesosSADE() {

            ocultarfrmProcesarEE();
            $("#frmError_ProcesosSADE").modal("show");
            
            return false;
        }

    </script>

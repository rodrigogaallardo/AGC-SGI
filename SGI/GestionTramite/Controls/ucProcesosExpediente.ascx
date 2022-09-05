<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProcesosExpediente.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucProcesosExpediente" %>



<asp:UpdatePanel ID="updPnlUCDatosTramite" runat="server" >
<ContentTemplate>
    <asp:HiddenField ID="hid_cancelado_usuario" runat="server" Value="0" />
    <asp:HiddenField ID="hid_index_proceso_pendiente" runat="server" Value="-1" />
    <asp:HiddenField ID="hid_editable" runat="server" Value="False" />
    <asp:HiddenField ID="hid_id_tramite_tarea" runat="server" Value="0" />
</ContentTemplate>
</asp:UpdatePanel>


<%--grilla para ver estado de las operaciones del expediente--%>
<asp:UpdatePanel ID="updPnlGrillaResultadoExpediente" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:GridView ID="grdResultadoExpediente" runat="server" AutoGenerateColumns="false"
            GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
            DataKeyNames="id_generar_expediente_proc,realizado">
            <Columns>
                <asp:BoundField DataField="id_proceso" HeaderText="Nro. de Proceso" />
                <asp:BoundField DataField="nombre_proceso" HeaderText="Descripción" ItemStyle-Width="150px" />
                <asp:BoundField DataField="nombre_tramite" HeaderText="Referencia" ItemStyle-Width="300px" />
                <asp:TemplateField HeaderText="Resultado SADE">
                    <ItemTemplate>
                        <div>
                            
                            <p class="text-error"><%# HttpUtility.HtmlEncode(Eval("resultado_sade_error")) %></p>
                        </div>
                        <div>
                            <p class="text-success"><%# HttpUtility.HtmlEncode(Eval("resultado_sade_ok")) %></p>
                        </div>
                        <div>
                            <p class="text-info"><%# HttpUtility.HtmlEncode(Eval("resultado_alerta")) %></p>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="fecha_sade" HeaderText="Fecha SADE" ItemStyle-Width="110px" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:CheckBoxField DataField="realizado" HeaderText="Realizado" ItemStyle-CssClass="align-center" />


            </Columns>
        </asp:GridView>

    </ContentTemplate>
</asp:UpdatePanel>


<div id="modal_procesar_item" class="modal hide fade in" data-backdrop="static" style="min-width:800px">

    <div class="modal-header">
    <%--      <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>--%>
        <h3 id="myModalLabel">Operaciones Expediente Electr&oacute;nico</h3>
    </div>

    <div class="modal-body" style="max-height:400px;">
                          
        <asp:UpdatePanel ID="updPnlGrillaProcesos" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

            <%--grilla para visualizar como se van ejecutando cada operacion del expediente en un modal--%>
            <asp:GridView ID="grdProcesosExpediente" runat="server" AutoGenerateColumns="false"
                GridLines="None" CssClass="table table-bordered table-striped table-hover with-check"
                DataKeyNames="id_generar_expediente_proc,id_tramite_tarea,id_devolucion_ee,realizado" >

                <Columns>

                    <asp:BoundField DataField="id_proceso" HeaderText="Nro. de Proceso"  />
                    <asp:BoundField DataField="nombre_proceso"  HeaderText="Descripción" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="nombre_tramite" HeaderText="Referencia" ItemStyle-Width="300px" />

                    <asp:TemplateField ItemStyle-Width="200px" HeaderText="Realizado">
                        <ItemTemplate> 
                            <div id="div_buscar_fila" style="display:none"></div>

                            <asp:UpdatePanel ID="updPnlItemGrillaProcesos" runat="server">
                            <ContentTemplate>

                                <div class="inline">  <%--pull-left--%>
                                    <asp:CheckBox ID="chkRealizar" runat="server" CssClass="checker" Checked='<%# Eval("realizado") %>' Enabled="false" />
                                </div>


                                <div class="inline">  <%-- pull-left--%>
                                    <%--solo para pruebas unitarias--%>
                                    <asp:Button ID="btnProcesarItemExpediente" runat="server" 
                                        Text="Procesar" style="display:none"
                                        CommandArgument='<%#Eval("id_generar_expediente_proc")%>'
                                        OnCommand="grdProcesosExpediente_Click" />
                                </div>

                                <div class="inline"> <%-- pull-right--%>
                                    <div>
                                        <p class="text-error"><%# HttpUtility.HtmlEncode(Eval("resultado_sade_error")) %></p>
                                    </div>
                                    <div>
                                        <p class="text-success"><%# HttpUtility.HtmlEncode(Eval("resultado_sade_ok")) %></p>
                                    </div>
                                    <div>
                                        <p class="text-info"><%# HttpUtility.HtmlEncode(Eval("resultado_alerta")) %></p>
                                    </div>
                                </div>

                                <div class="inline"> <%--pull-right--%>
                                    <asp:Label ID="lblMensajeError" runat="server" Text=""></asp:Label>
                                </div>

                                <div class="inline pull-right">
                                    <%--se simula ajax porque se ejecutan click desde jquery--%>
                                    <asp:Image ID="imgLoadingProceso" runat="server" 
                                        ImageUrl="~/Content/img/app/Loading24x24.gif" style="display:none" />                                               
                                </div>

                            </ContentTemplate>
                            </asp:UpdatePanel>
                                                    
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <div class="modal-footer">
        <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="canceladoPorUsuario();">Cerrar</button>
    </div>

</div>




<script type="text/javascript">

    $(document).ready(function () {
        
        procesarExpediente();
    });


    function procesarExpediente() {
        debugger;
        //si hay operaciones pendientes de procesar, carga la pantalla modal 
        //y comienza a ejecutar los click de los botones
        var index_pendiente = $('#<%=hid_index_proceso_pendiente.ClientID%>').prop("value");
        var editable = $('#<%=hid_editable.ClientID%>').prop("value"); 
        if (index_pendiente >= 0 && editable=='True') {
            $('#modal_procesar_item').modal('show');
            ejecutarProcesoEE(index_pendiente)
            //ejecutarTodosProcesosEE();
        }
    }

    function ejecutarTodosProcesosEE() {

        //cuando index_pendiente = -1 no hay nada pendiente y se decidio cancelar la secuencia
        var index_pendiente = $('#<%=hid_index_proceso_pendiente.ClientID%>').prop("value");

        if (index_pendiente >= 0) {

            $("[id*='updPnlItemGrillaProcesos']").each(function (index, element) {

                // debugger;
                //var lbl_error = $(element).find("[id*='lblMensajeError']")[index];

                //var fila = $("[id*='div_buscar_fila']")[index].parentElement.parentElement;

                //if ($(lbl_error).attr("value") != undefined) {
                //    $(fila).switchClass("", "error", 0);

                //}

                //$(fila).switchClass("", "warning", 0);

                var img = $(element).find("[id*='imgLoadingProceso']");

                if ($(element).find("[id*='chkRealizar']").is(':checked')) {
                    $(img).hide();
                }
                else {
                    $(img).show();
                }

            });

            ejecutarProcesoEE(index_pendiente);
        }

        return false;
    }

    function ejecutarProcesoEE(index) {
    
        var cancelado_usuario = $('#<%=hid_cancelado_usuario.ClientID%>').prop("value"); 

        if (index >= 0 && cancelado_usuario == "0" ) {

            var fila = $("[id*='div_buscar_fila']")[index].parentElement.parentElement;
            $(fila).switchClass("", "warning", 0);

            $($("[id*='btnProcesarItemExpediente']")[index]).click();
        }
    }


    function canceladoPorUsuario() {
        $('#<%=hid_cancelado_usuario.ClientID%>').prop("value", "1"); 
    }


</script>


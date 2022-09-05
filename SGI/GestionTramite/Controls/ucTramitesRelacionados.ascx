<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTramitesRelacionados.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucTramitesRelacionados" %>

<script type="text/javascript">

    $(document).ready(function () {
        inicializar_popover();
        inicializar_ltr_btnUpDown_collapse();
    });

    function inicializar_ltr_btnUpDown_collapse() {
        //cuando cargua por primera vez, se muestra expandido o no segun el seteo de los atributos del control
        var colapsar = $('#<%=hid_ltr_collapse.ClientID%>').attr("value");

        var obj = $("#ltr_btnUpDown")[0];
        var href_collapse = $(obj).attr("href");

        if ($(href_collapse).attr("id") != undefined) {
            if ($(href_collapse).css("height") == "0px") {
                if (colapsar == "true") {
                    $(href_collapse).collapse();
                    $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
                }
            }
            else {
                if (colapsar == "false") {
                    $(href_collapse).collapse();
                    $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
                }
            }
        }
    }

    function inicializar_popover() {

        $("[id*='lnkTareasSolicitud']").tooltip({ delay: { show: 2000, hide: 100 }, placement: 'top' });

        $("[id*='MainContent_grdTramitesRelacionados_lnkTareasSolicitud_']").each(function () {
            //para cada fila de la grilla, se busca el link y se lo vincula al panel de la misma fila
            //para que con el clikc del link habra el popOver de un html
            var id_pnlTareas = $(this).attr("id").replace("MainContent_grdTramitesRelacionados_lnkTareasSolicitud_", "MainContent_grdTramitesRelacionados_pnlTareas_");
            var objTareas = $("#" + id_pnlTareas).html();
            $(this).popover({
                title: 'Tareas',
                content: objTareas,
                html: 'true'
            });
        });
    }

    function popOverTareas(obj) {

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

    function ltr_btnUpDown_collapse_click(obj) {
        var href_collapse = $(obj).attr("href");
        if ($(href_collapse).attr("id") != undefined) {
            if ($(href_collapse).css("height") == "0px") {
                $(obj).find(".icon-chevron-down").switchClass("icon-chevron-down", "icon-chevron-up", 0);
            }
            else {
                $(obj).find(".icon-chevron-up").switchClass("icon-chevron-up", "icon-chevron-down", 0);
            }
        }
    }
</script>
    

<asp:HiddenField ID="hid_ltr_collapse" runat="server" Value="false"/>


<asp:Panel ID="pnlTramiteRelacionados" runat="server">

    <div class="accordion-group widget-box">

        <div class="accordion-heading">
            <a id="ltr_btnUpDown" data-parent="#collapse-group" href="#collapse_tramites_relacionados" 
                data-toggle="collapse" onclick="ltr_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5><asp:Label ID="tituloControl" runat="server" Text="Trámites Relacionados"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-down"></i></span>        
                </div>
            </a>
        </div>


        <div  class="accordion-body collapse" id="collapse_tramites_relacionados" >

            <div class="widget-content">
                <div>
                    <div style="width: 100%;">
                        <asp:UpdatePanel ID="updBuscar" runat="server">
                            <ContentTemplate>
                              <%--         CssClass="table table-bordered table-striped"--%>
                                <asp:HiddenField ID="hddIdSolicitud" runat="server"/>
                                <asp:GridView ID="grdTramitesRelacionados" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" 
                                    CssClass="table table-bordered table-striped table-hover with-check" 
                                    GridLines="None" Width="100%" SelectMethod="grdTramitesRelacionados_GetData"
                                    DataKeyNames="id_solicitud,formulario_tarea" OnDataBound="grdTramitesRelacionados_DataBound">
                                    <SortedAscendingHeaderStyle CssClass="GridAscendingHeaderStyle" />
                                    <SortedDescendingHeaderStyle CssClass="GridDescendingHeaderStyle" />
                
                                    <Columns>

                                        <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="75px" ItemStyle-CssClass="align-center" SortExpression="sol.id_solicitud">
                                            <ItemTemplate> 
                                                <asp:HyperLink ID="lnkid_solicitud" runat="server" NavigateUrl='<%# "~/GestionTramite/VisorTramite.aspx?id=" + Eval("id_solicitud") %>'><%# Eval("id_solicitud") %></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="direccion" HeaderText="Ubicación" SortExpression="direccion" />
                    
                                        <asp:HyperLinkField DataNavigateUrlFormatString="~/GestionTramite/Tareas/{0}?id={1}" DataNavigateUrlFields="formulario_tarea,id_tramitetarea"
                                            DataTextField="nombre_tarea" HeaderText="Tarea" ItemStyle-Width="200px" SortExpression="nombre_tarea" />

                                        <asp:BoundField DataField="FechaInicio_tramitetarea" HeaderText="Tarea creada el" DataFormatString="{0:d}" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="FechaInicio_tramitetarea" />
                                        <asp:BoundField DataField="descripcion_estado" HeaderText="Estado" ItemStyle-Width="100px" ItemStyle-CssClass="align-center" SortExpression="descripcion_estado" />

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
                                    <EmptyDataTemplate> 
                                        <div style="width: 100%;">
                                            <i class="icon-remove"></i>
                                            <span class="text">No se encontraron Trámites Relacionados.</span>
                                        </div>
                                    </EmptyDataTemplate>

                                </asp:GridView>  
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>


        </div>

    </div>

</asp:Panel>




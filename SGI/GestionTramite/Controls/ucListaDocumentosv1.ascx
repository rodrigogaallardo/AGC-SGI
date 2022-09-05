<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListaDocumentosv1.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucListaDocumentosv1" %>


<div class="accordion-group widget-box">
    <div class="accordion-heading">
        <a id="btnUpDown" data-parent="#collapse-group" href="#collapseListaObservV1" 
            data-toggle="collapse" onclick="btnUpDownListaObservV1_click(this)">

            <div class="widget-title">
                <span class="icon"><i class="icon-th-list"></i></span>
                <h5>Lista de Documentos</h5>
                <span class="btn-right"><i class="icon-chevron-up"></i></span>        
            </div>
        </a>

    </div>
    <div  class="accordion-body collapse in" id="collapseListaObservV1" >
        <div class="widget-content">
            <asp:UpdatePanel ID="updListaDocumentos" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdDocumentosAdjuntos" runat="server" AutoGenerateColumns="false"
                        GridLines="none" CssClass="table table-striped table-bordered"
                        OnRowDataBound="grdDocumentosAdjuntos_RowDataBound"
                        DataKeyNames="id_tipodocsis">
                        <Columns>
                            <asp:TemplateField HeaderText="Archivo" >
                                <ItemTemplate >
                            
                                    <asp:HyperLink ID="lnkArchivoPdf" runat="server" CssClass="btn-link" Target="_blank" NavigateUrl='<%#Eval("url")%>'>
                                        <i class="imoon imoon-file-pdf color-red"></i>
                                        <span class="text"><%#Eval("nombre") %></span>
                                    </asp:HyperLink>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id_file" HeaderText="Nº Archivo" ItemStyle-Width="90px" ItemStyle-CssClass="text-center" />
                            <%--<asp:BoundField DataField="id_solicitud" HeaderText="Solicitud" ItemStyle-Width="90px" ItemStyle-CssClass="text-center"  />--%>
                            <asp:BoundField DataField="Fecha" DataFormatString="{0:d}" HeaderText="Fecha" ItemStyle-CssClass="text-center" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="UserName" HeaderText="Nombre de usuario" ItemStyle-Width="150px" ItemStyle-CssClass="text-center" />
                            <asp:BoundField DataField="numero_Gedo" HeaderText="Nº GEDO" ItemStyle-Width="150px" ItemStyle-CssClass="text-center" />
                            <asp:TemplateField ItemStyle-Height="24px" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminarDocAdj" runat="server" 
                                            CommandArgument='<%# Eval("id_doc_adj") %>' 
                                            OnClientClick="javascript:return tda_confirm_del();"
                                            OnCommand="lnkEliminarDocAdj_Command" 
                                            Width="70px" Visible="false">
                                        <i class="icon icon-trash"></i> 
                                        <span class="text">Eliminar</span></a>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>

                            <div class="mtop5">
                                <img src="<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>" />
                                <span class="mleft20">Este tr&aacute;mite no posee documentos adjuntos.</span>

                            </div>

                        </EmptyDataTemplate>
                    </asp:GridView>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script type="text/javascript">

    function btnUpDownListaObservV1_click(obj) {

        if ($("[id*='collapseListaObservV1']").css("height") == "0px") {
            $(".icon-chevron-down")[0].className = "icon-chevron-up";
        }
        else {
            $(".icon-chevron-up")[0].className = "icon-chevron-down";
        }
    }

</script>
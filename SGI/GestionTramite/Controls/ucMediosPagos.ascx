<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMediosPagos.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucMediosPagos" %>




 <%--   <asp:Panel ID="pnlMediosDePago" runat="server" Visible="false">--%>

        <asp:HiddenField ID="hid_id_tramite_tarea" runat="server" />
        <asp:HiddenField ID="hid_id_caa" runat="server" />
        <asp:HiddenField ID="hid_id_pago" runat="server" Value="0" />
        <asp:HiddenField ID="hid_estado_pago" runat="server" Value="" />
        <asp:HiddenField ID="hid_id_grupotramite" runat="server" />

<%--    </asp:Panel>--%>


    <asp:UpdatePanel ID="updPnlGenerarBoletaUnica" runat="server" RenderMode="Inline">
    <ContentTemplate>


    <asp:Panel ID="pnlGenerarBoletaUnica" runat="server" Visible="false" >

        <div class="widget-box">

            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Boleta &uacute;nica</h5>
            </div>


            <div class="widget-content">

                <table style="border-collapse:separate; border-spacing:7px">
                    <tr>
                        <td>
                            <asp:Image ID="imgPesos"  runat="server" ImageUrl="~/Content/img/pesos.png" />
                        </td>
                        <td>
                            <strong>Boleta &uacute;nica</strong>
                            <div>
                                Generar boleta de pago para ser abonada en las cajas de tesorer&iacute;a de la Ciudad.
                            </div>
                        </td>
                        <td style="padding-left:10px; padding-top:20px">

                                <div class="pull-right">

                                    <asp:UpdateProgress ID="UpPrgssGenerarBoletaUnica" runat="server"   AssociatedUpdatePanelID="updPnlGenerarBoletaUnica"  >
                                        <ProgressTemplate>
                                            <asp:Image ID="imgLoadinUpPrgssGenerarBoletaUnica"  runat="server" ImageUrl="~/Content/img/app/Loading24x24.gif" style="margin-left: 10px" alt="loading"/>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                </div>

                                <div class="pull-right" style="margin-right:5px">

                                    <asp:LinkButton ID="lnkGenerarBoletaUnica" runat="server" 
                                        CssClass="btn btn-success" 
                                        OnClick="lnkGenerarBoletaUnica_Click" OnClientClick="hidelnkGenerarBoletaUnica();"> 
                                        <i class="icon-white icon-search"></i>
                                        <span class="text">Generar Boleta</span>
                                    </asp:LinkButton>

                                </div>

                        </td>
                    </tr>
                </table>

            </div>

        </div>

    </asp:Panel>

    </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updPnl_grdPagosGeneradosBU" runat="server">
    <ContentTemplate>

    <asp:Panel ID="pnlPagosGeneradosBU" runat="server"  Visible="false">



        <div class="widget-box">

            <div class="widget-title">
                <span class="icon"><i class="icon-list-alt"></i></span>
                <h5>Boleta/s Unica/s generada/s</h5>
            </div>

            <div class="widget-content">


                <asp:GridView ID="grdPagosGeneradosBU" runat="server" AutoGenerateColumns="false" 
                    DataKeyNames="id_sol_pago,id_tramitetarea,id_pago"
                    AllowPaging="false" 
                    GridLines="None" Width="880px" OnRowDataBound="grdPagosGeneradosBU_OnRowDataBound"
                    CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="nro_boleta_unica" HeaderText="Nro. Boleta"
                            ItemStyle-Width="90px" ItemStyle-CssClass="align-center"/>
                        <asp:BoundField DataField="CreateDate" HeaderText="Fecha" ItemStyle-Width="80px" 
                            HeaderStyle-HorizontalAlign="Left" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="align-center" />
                        <asp:BoundField DataField="monto_pago" HeaderText="Monto Total" DataFormatString="{0:$ #,##0.00 }"
                            ItemStyle-Width="90px" ItemStyle-CssClass="align-right" />

                        <asp:TemplateField ItemStyle-CssClass="align-center"  HeaderText="Estado" >
                            <ItemTemplate>
                                <asp:Label ID="lblDescripicionEstadoPago" runat="server" Text="" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-CssClass="align-center"  >
                            <ItemTemplate>

                                <asp:HyperLink ID="lnkImprimirboleta" runat="server"  Target="_blank"
                                    NavigateUrl='<%# ResolveUrl("~/Reportes/ImprimirBoletaUnica.aspx?id=" + Eval("id_pago")) + "&id_grupotramite=" + hid_id_grupotramite.Value %>' 
                                    Visible="false"     >
                                        <i class="icon-file iconlocal"></i>
                                        <span class="text">Imprimir Boleta</span>
                                </asp:HyperLink>

                                      
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        <div class="titulo-4">
                            No se ingresaron 
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>

            </div>

        </div>



    </asp:Panel>


    </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

        function hidelnkGenerarBoletaUnica() {
            $('#<%=lnkGenerarBoletaUnica.ClientID%>').hide();
            return false;
        }
    </script>
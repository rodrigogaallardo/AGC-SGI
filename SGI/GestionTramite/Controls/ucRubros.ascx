<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRubros.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucRubros" %>

<%: Scripts.Render("~/bundles/autoNumeric") %>

<asp:UpdatePanel ID="Hiddens" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hid_id_solicitud" runat="server" />
        <asp:HiddenField ID="hid_id_encomienda" runat="server" />
        <asp:HiddenField ID="hid_return_url" runat="server" />
        <asp:HiddenField ID="hid_DecimalSeparator" runat="server" />
        <asp:HiddenField ID="hid_validar_estado" runat="server" />
        <asp:HiddenField ID="hid_editar" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>


<div id="page_content">
    
   <div class="accordion-group widget-box">

        <%-- titulo box Rubros--%>
        <div class="accordion-heading">
            <a id="A1" data-parent="#collapse-group">

                <div class="widget-title">
                    <span class="icon"><i class="imoon imoon-hammer"></i></span>
                    <h5>Rubros</h5>
                </div>
            </a>
        </div>
       
        <%-- contenido del box Rubros --%>
        <div id="box_MostrarRubros" class="accordion-body collapse in">
            <div class="widget-content">


                <asp:GridView ID="grdRubrosMostrar" runat="server" AutoGenerateColumns="false"
                    AllowPaging="false" CssClass="table table-bordered mtop5"
                    GridLines="None" Width="100% ">
                    <HeaderStyle CssClass="grid-header" />
                    <RowStyle CssClass="grid-row" />
                    <AlternatingRowStyle BackColor="#efefef" />

                    <Columns>
                        <asp:BoundField DataField="cod_rubro" HeaderText="Código" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="desc_rubro" HeaderText="Descripción" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="TipoActividadNombre" HeaderText="Actividad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="TipoTamite" HeaderText="Tipo Trámite" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="SuperficieHabilitar" HeaderText="Superficie (m2)" ItemStyle-Width="90px" ItemStyle-CssClass="align-center" />


                    </Columns>
                    <EmptyDataTemplate>

                        <div class="mtop10">

                            <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                            <span class="mleft10">No se encontraron registros.</span>

                        </div>

                    </EmptyDataTemplate>
                </asp:GridView>

            </div>
        </div>

    </div>
</div>



<%--Modal mensajes de error--%>
<div id="frmError_Rubros" class="modal fade" style="display: none;">
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

    $(document).ready(function () {


        init_JS_updAgregarNormativa();
        init_JS_updBuscarRubros();

    });


    function showfrmError_Rubros() {
        $("#pnlBotonesGuardar_Rubros").show();
        $("#frmError_Rubros").modal("show");
        return false;

    }

    function showfrmConfirmarEliminar_Rubros() {

        $("#pnlBotonesConfirmacion_Rubros").show();
        $("#frmConfirmarEliminar_Rubros").modal("show");
        return false;
    }

    function hidefrmConfirmarEliminar_Rubros() {

        $("#frmConfirmarEliminar_Rubros").modal("hide");
        return false;
    }

    function ocultarBotonesConfirmacion_Rubros() {

        $("#pnlBotonesConfirmacion_Rubros").hide();
        return false;
    }

    function ocultarBotonesGuardado_Rubros() {

        $("#pnlBotonesGuardar_Rubros").hide();

        return true;
    }


        function hidefrmAgregarNormativa_Rubros() {

            $("#frmAgregarNormativa_Rubros").modal("hide");
            return false;
        }

        function ocultarBotonesConfirmacionEliminarNormativa_Rubros() {
            $("#pnlBotonesConfirmacionEliminarnormativa_Rubros").hide();
            return false;
        }

        function showfrmConfirmarEliminarNormativa_Rubros() {

            $("#frmConfirmarEliminarNormativa_Rubros").modal("show");
            return false;
        }

        function hidefrmConfirmarEliminarNormativa_Rubros() {

            $("#frmConfirmarEliminarNormativa_Rubros").modal("hide");
            return false;
        }


            function hidefrmAgregarRubros_Rubros() {

                $("#frmAgregarRubros_Rubros").modal("hide");
                return false;
            }



            function ocultarBotonesConfirmacionEliminarRubro_Rubros() {
                $("#pnlBotonesConfirmacionEliminarRubro_Rubros").hide();
                return false;
            }


            function ocultarBotonesAgregarRubros_Rubros() {

                $("#BotonesAgregarRubros_Rubros").hide();
                return false;
            }



        function hidefrmConfirmarEliminarRubro_Rubros() {

            $("#frmConfirmarEliminarRubro_Rubros").modal("hide");
            return false;
        }



    function hidefrmAgregarRubroUsoNoContemplado() {
        $("#frmAgregarActividades").modal("hide");
        return false;
    }


        function hideEditRubros() {
            $("#box_editarRubros").hide("slow");
            $("#box_infoRubros").hide("slow");
        }
        function hideMostrarRubros() {
            $("#box_MostrarRubros").hide("slow");
        }

</script>

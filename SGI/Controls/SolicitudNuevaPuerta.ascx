<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudNuevaPuerta.ascx.cs" Inherits="SGI.Controls.SolicitudNuevaPuerta" %>

<asp:UpdatePanel ID="updSolicitarNuevaPuerta" runat="server">
    <ContentTemplate>


        <asp:GridView ID="gridubicacion" runat="server" AutoGenerateColumns="false" DataKeyNames="id_ubicacion" ItemType="SGI.Model.Ubicacion"
            OnRowDataBound="gridubicacion_OnRowDataBound" ShowHeader="false" Width="100%"
            GridLines="None" AllowPaging="false" Style="margin-top: -10px">

            <Columns>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="hid_id_ubicacion" runat="server" Value="<%# Item.id_ubicacion %>" />


                        <table style="width: 100%">
                            <tr>
                                <td style="width: 305px">

                                    <strong>Datos de la Ubicaci&oacute;n</strong>
                                    <asp:Panel ID="pnlSMP" runat="server">
                                        <div>
                                            Número de Partida Matriz: <span class="label-azul"><%# Item.NroPartidaMatriz %></span>
                                        </div>
                                        <div>
                                            Sección: <asp:Label ID="grd_seccion" runat="server" Text="<%# Item.Seccion %>" CssClass="label-azul"></asp:Label>
                                            Manzana: <asp:Label ID="grd_manzana" runat="server" Text="<%# Item.Manzana %>" CssClass="label-azul"></asp:Label>
                                            Parcela: <asp:Label ID="grd_parcela" runat="server" Text="<%# Item.Parcela %>" CssClass="label-azul"></asp:Label>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlTipoUbicacion" runat="server" Style="padding-top: 3px" Visible="false">
                                        <div>
                                            Ubicaci&oacute;n: <asp:Label ID="lblTipoUbicacion" runat="server" CssClass="label-azul"></asp:Label>
                                        </div>
                                        <div>
                                            Detalle: <asp:Label ID="lblSubTipoUbicacion" runat="server" CssClass="label-azul"></asp:Label>
                                        </div>
                                        <div>
                                            Local: <asp:Label ID="lblLocal" runat="server" CssClass="label-azul"></asp:Label>
                                        </div>
                                    </asp:Panel>

                                    <div style="margin-top: 10px; min-height: 250px; min-width: 300px">
                                        <img id="imgCargando_NP" src='<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>' alt="" style="margin-left: 60px; margin-top: 40px" />
                                        <img id="imgFotoParcela_NP" class="img-polaroid" src="<%# Item.GetUrlFoto(300,250) %>" onload="fotoCargada_NP('imgCargando_NP',this);" onerror="noExisteFotoParcela_NP(this);" style="display: none" />
                                    </div>
                                </td>
                                <td style="width: 10px; border-right: solid 1px #eeeeee"></td>
                                <td style="width: auto; padding-left: 10px; vertical-align: text-top">

                                    <asp:Panel ID="pnlPuertas" runat="server">

                                        <strong>Puertas</strong>

                                        <div style="overflow: auto; max-height: 300px">

                                            <asp:UpdatePanel ID="updPuertas" runat="server" >
                                                <ContentTemplate>

                                                    <asp:DataList ID="lstPuertas" runat="server" ItemType="SGI.Model.Ubicacion.Puerta"
                                                        RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%">
                                                        <AlternatingItemStyle BackColor="#f9f9f9" />
                                                        <ItemTemplate>

                                                            <div class="form-inline">
                                                                <asp:Label ID="lblnombreCalle" runat="server" Text="<%# Item.Nombre_calle %>"></asp:Label>
                                                                <asp:Label ID="lblNroPuerta" runat="server" Text="<%# Item.NroPuerta_ubic %>" CssClass="mleft5"></asp:Label>
                                                            </div>

                                                        </ItemTemplate>

                                                    </asp:DataList>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>
                                    </asp:Panel>

                                    <div class="mtop20">
                                        <strong >Datos de la puerta a solicitar:</strong>
                                    </div>

                                    <table class="mtop5">
                                        <tr>
                                            <td>Calle:
                                            </td>
                                            <td class="pleft5">
                                            <ej:Autocomplete ID="AutocompleteCalles" MinCharacter="3" DataTextField="NombreOficial_calle" DataUniqueKeyField="Codigo_calle" Width="500px" runat="server"/>
                                       </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td class="pleft5">
                                                <span style="font-size: 8pt">Debe ingresar un mínimo de 3 letras y el sistema le mostrará
                                                                        las calles posibles.</span>
                                                <p>
                                                    <asp:RequiredFieldValidator ID="ReqCalle" runat="server" ErrorMessage="Debe seleccionar una de las calles de la lista desplegable."
                                                        Display="Dynamic" ControlToValidate="ddlCalles_NP" ValidationGroup="EnviarMail"
                                                        CssClass="alert alert-small alert-danger mtop10"></asp:RequiredFieldValidator>
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>N&uacute;mero:
                                            </td>
                                            <td class="pleft5">
                                                <asp:TextBox ID="txtNroPuerta_NP" runat="server" Width="100px" MaxLength="5" CssClass="form-control"></asp:TextBox>
                                                <div class="mtop5">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe ingresar el número de puerta"
                                                    Display="Dynamic" ControlToValidate="txtNroPuerta_NP" ValidationGroup="EnviarMail" CssClass="alert alert-small alert-danger mtop10"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="El número de puerta debe ser mayor a 0 (cero)."
                                                    Display="Dynamic" ControlToValidate="txtNroPuerta_NP" ValidationGroup="EnviarMail" CssClass="alert alert-small alert-danger mtop10"
                                                    MinimumValue="1" MaximumValue="99999999"></asp:RangeValidator>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>


                                </td>
                            </tr>
                            
                        </table>


                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <asp:Panel ID="pnlNotFound" runat="server" Style="padding: 10px;">
                    <div class="form-inline">
                        <div class="controls">
                            <div class="mtop10">

                                <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                <span class="mleft10">No se encontraron datos para esta ubicaci&oacute;n.</span>

                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </EmptyDataTemplate>
        </asp:GridView>


        <div class="form-inline">
            <div class="col-sm-12 text-right">
                
                <asp:Panel ID="pnlEnviadoOK" runat="server" CssClass="alert alert-success inline-block" Visible="false">
                    Se ha enviado el mail correctamente, aguarde su respuesta.
                </asp:Panel>
                <asp:LinkButton ID="btnEnviarMail" runat="server" CssClass="btn btn-primary" ValidationGroup="EnviarMail" OnClick="btnEnviarMail_Click">
                    <i class="imoon imoon-envelope"></i>
                    <span class="text">Enviar</span>
                </asp:LinkButton>

                <asp:LinkButton ID="btnCerrar" runat="server" CssClass="btn btn-default" OnClientClick="hidefrmSolicitarNuevaPuerta();">
                    <span class="text">Cerrar</span>
                </asp:LinkButton>

            </div>

        </div>
        <div class="clearfix"></div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">

    function noExisteFotoParcela_NP(objimg) {

        $(objimg).attr("src", '<%: ResolveUrl("~/Content/img/app/ImageNotFound.png") %>');
        return true;
    }

    function fotoCargada_NP(objOcultar, objMostrar) {
        
        $("#" + objOcultar).css("display", "none");
        $(objMostrar).css("display", "inherit");

        return true;
    }

    function init_Js_updSolicitarNuevaPuerta() {

        $("[id*='ddlCalles_NP']").select2({
            minimumInputLength: 3
        });
        $("[id*='txtNroPuerta_NP']").autoNumeric("init", { aSep: "", mDec: 0, vMax: '99999' });

        return false;
    }


</script>

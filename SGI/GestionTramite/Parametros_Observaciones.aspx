<%@ Page Title="Parametros de Observaciones" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Parametros_Observaciones.aspx.cs"
    Inherits="SGI.GestionTramite.Parametros_Observaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/select2") %>
    <%: Styles.Render("~/bundles/Select2Css") %>
    <%: Scripts.Render("~/bundles/autoNumeric") %>

    <asp:Panel ID="pnlBotonDefault" runat="server">

        <div class="">

            <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">

                <%-- Titulo collapsible Parametros Superficie --%>
                <div class="accordion-heading">
                    <a id="bt_Parametros_btnUpDown" data-parent="#collapse-group" href="#bt_Parametros" data-toggle="collapse">
                        <div class="widget-title">
                            <span class="icon"><i class="icon-list-alt"></i></span>
                            <h5>
                                <asp:Label ID="bt_Parametros_tituloControl" runat="server" Text="Parametros de Observaciones"></asp:Label></h5>
                            <span class="btn-right"><i class="icon-chevron-up"></i></span>
                        </div>
                    </a>
                </div>
                <div class="accordion-body collapse in" id="bt_Parametros">
                    <div class="widget-content">
                        <%-- Campos de Texto --%>
                        <asp:UpdatePanel ID="updPnlObservaciones" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                
                                <div class="form-horizontal">
                                    
                                    <fieldset>
                                        <div class="mleft20 mtop20"><asp:Label Font-Size="Medium" runat="server">El valor declarado en cada caso corresponde a la cantidad de observaciones que puede realizar el Calificador directamente al Contribuyente.</asp:Label></div>
                                        <%-- SSP --%>
                                        <div class="row-fluid">
                                            <div class="span12">
                                                <div class="control-group">
                                                    <asp:Label ID="lblSSP" runat="server" AssociatedControlID="ddlSSP" Text="Simple Sin plano:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                         <asp:DropDownList ID="ddlSSP" runat="server" Width="130px" AutoPostBack="true" >
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem><asp:ListItem Text="1" Value="1"></asp:ListItem><asp:ListItem Text="2" Value="2"></asp:ListItem><asp:ListItem Text="3" Value="3"></asp:ListItem><asp:ListItem Text="4" Value="4"></asp:ListItem><asp:ListItem Text="5" Value="5"></asp:ListItem><asp:ListItem Text="6" Value="6"></asp:ListItem><asp:ListItem Text="7" Value="7"></asp:ListItem><asp:ListItem Text="8" Value="8"></asp:ListItem><asp:ListItem Text="9" Value="9"></asp:ListItem><asp:ListItem Text="10" Value="10"></asp:ListItem><asp:ListItem Text="11" Value="11"></asp:ListItem><asp:ListItem Text="12" Value="12"></asp:ListItem><asp:ListItem Text="13" Value="13"></asp:ListItem><asp:ListItem Text="14" Value="14"></asp:ListItem><asp:ListItem Text="15" Value="15"></asp:ListItem><asp:ListItem Text="16" Value="16"></asp:ListItem><asp:ListItem Text="17" Value="17"></asp:ListItem><asp:ListItem Text="18" Value="18"></asp:ListItem><asp:ListItem Text="19" Value="19"></asp:ListItem><asp:ListItem Text="20" Value="20"></asp:ListItem><asp:ListItem Text="21" Value="21"></asp:ListItem><asp:ListItem Text="22" Value="22"></asp:ListItem><asp:ListItem Text="23" Value="23"></asp:ListItem><asp:ListItem Text="24" Value="24"></asp:ListItem><asp:ListItem Text="25" Value="25"></asp:ListItem><asp:ListItem Text="26" Value="26"></asp:ListItem><asp:ListItem Text="27" Value="27"></asp:ListItem><asp:ListItem Text="28" Value="28"></asp:ListItem><asp:ListItem Text="29" Value="29"></asp:ListItem><asp:ListItem Text="30" Value="30"></asp:ListItem><asp:ListItem Text="31" Value="31"></asp:ListItem><asp:ListItem Text="32" Value="32"></asp:ListItem><asp:ListItem Text="33" Value="33"></asp:ListItem><asp:ListItem Text="34" Value="34"></asp:ListItem><asp:ListItem Text="35" Value="35"></asp:ListItem><asp:ListItem Text="36" Value="36"></asp:ListItem><asp:ListItem Text="37" Value="37"></asp:ListItem><asp:ListItem Text="38" Value="38"></asp:ListItem><asp:ListItem Text="39" Value="39"></asp:ListItem><asp:ListItem Text="40" Value="40"></asp:ListItem><asp:ListItem Text="41" Value="41"></asp:ListItem><asp:ListItem Text="42" Value="42"></asp:ListItem><asp:ListItem Text="43" Value="43"></asp:ListItem><asp:ListItem Text="44" Value="44"></asp:ListItem><asp:ListItem Text="45" Value="45"></asp:ListItem><asp:ListItem Text="46" Value="46"></asp:ListItem><asp:ListItem Text="47" Value="47"></asp:ListItem><asp:ListItem Text="48" Value="48"></asp:ListItem><asp:ListItem Text="49" Value="49"></asp:ListItem><asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span12">
                                                <div class="control-group">
                                                    <asp:Label ID="lblSCP" runat="server" AssociatedControlID="ddlSCP" Text="Simple Con plano:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlSCP" runat="server" Width="130px" AutoPostBack="true" >
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem><asp:ListItem Text="1" Value="1"></asp:ListItem><asp:ListItem Text="2" Value="2"></asp:ListItem><asp:ListItem Text="3" Value="3"></asp:ListItem><asp:ListItem Text="4" Value="4"></asp:ListItem><asp:ListItem Text="5" Value="5"></asp:ListItem><asp:ListItem Text="6" Value="6"></asp:ListItem><asp:ListItem Text="7" Value="7"></asp:ListItem><asp:ListItem Text="8" Value="8"></asp:ListItem><asp:ListItem Text="9" Value="9"></asp:ListItem><asp:ListItem Text="10" Value="10"></asp:ListItem><asp:ListItem Text="11" Value="11"></asp:ListItem><asp:ListItem Text="12" Value="12"></asp:ListItem><asp:ListItem Text="13" Value="13"></asp:ListItem><asp:ListItem Text="14" Value="14"></asp:ListItem><asp:ListItem Text="15" Value="15"></asp:ListItem><asp:ListItem Text="16" Value="16"></asp:ListItem><asp:ListItem Text="17" Value="17"></asp:ListItem><asp:ListItem Text="18" Value="18"></asp:ListItem><asp:ListItem Text="19" Value="19"></asp:ListItem><asp:ListItem Text="20" Value="20"></asp:ListItem><asp:ListItem Text="21" Value="21"></asp:ListItem><asp:ListItem Text="22" Value="22"></asp:ListItem><asp:ListItem Text="23" Value="23"></asp:ListItem><asp:ListItem Text="24" Value="24"></asp:ListItem><asp:ListItem Text="25" Value="25"></asp:ListItem><asp:ListItem Text="26" Value="26"></asp:ListItem><asp:ListItem Text="27" Value="27"></asp:ListItem><asp:ListItem Text="28" Value="28"></asp:ListItem><asp:ListItem Text="29" Value="29"></asp:ListItem><asp:ListItem Text="30" Value="30"></asp:ListItem><asp:ListItem Text="31" Value="31"></asp:ListItem><asp:ListItem Text="32" Value="32"></asp:ListItem><asp:ListItem Text="33" Value="33"></asp:ListItem><asp:ListItem Text="34" Value="34"></asp:ListItem><asp:ListItem Text="35" Value="35"></asp:ListItem><asp:ListItem Text="36" Value="36"></asp:ListItem><asp:ListItem Text="37" Value="37"></asp:ListItem><asp:ListItem Text="38" Value="38"></asp:ListItem><asp:ListItem Text="39" Value="39"></asp:ListItem><asp:ListItem Text="40" Value="40"></asp:ListItem><asp:ListItem Text="41" Value="41"></asp:ListItem><asp:ListItem Text="42" Value="42"></asp:ListItem><asp:ListItem Text="43" Value="43"></asp:ListItem><asp:ListItem Text="44" Value="44"></asp:ListItem><asp:ListItem Text="45" Value="45"></asp:ListItem><asp:ListItem Text="46" Value="46"></asp:ListItem><asp:ListItem Text="47" Value="47"></asp:ListItem><asp:ListItem Text="48" Value="48"></asp:ListItem><asp:ListItem Text="49" Value="49"></asp:ListItem><asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        
                                        <div class="row-fluid">
                                            <div class="span12">
                                                <div class="control-group">
                                                    <asp:Label ID="lblESPE" runat="server" AssociatedControlID="ddlESPE" Text="Inspección Previa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlESPE" runat="server" Width="130px" AutoPostBack="true" >
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem><asp:ListItem Text="1" Value="1"></asp:ListItem><asp:ListItem Text="2" Value="2"></asp:ListItem><asp:ListItem Text="3" Value="3"></asp:ListItem><asp:ListItem Text="4" Value="4"></asp:ListItem><asp:ListItem Text="5" Value="5"></asp:ListItem><asp:ListItem Text="6" Value="6"></asp:ListItem><asp:ListItem Text="7" Value="7"></asp:ListItem><asp:ListItem Text="8" Value="8"></asp:ListItem><asp:ListItem Text="9" Value="9"></asp:ListItem><asp:ListItem Text="10" Value="10"></asp:ListItem><asp:ListItem Text="11" Value="11"></asp:ListItem><asp:ListItem Text="12" Value="12"></asp:ListItem><asp:ListItem Text="13" Value="13"></asp:ListItem><asp:ListItem Text="14" Value="14"></asp:ListItem><asp:ListItem Text="15" Value="15"></asp:ListItem><asp:ListItem Text="16" Value="16"></asp:ListItem><asp:ListItem Text="17" Value="17"></asp:ListItem><asp:ListItem Text="18" Value="18"></asp:ListItem><asp:ListItem Text="19" Value="19"></asp:ListItem><asp:ListItem Text="20" Value="20"></asp:ListItem><asp:ListItem Text="21" Value="21"></asp:ListItem><asp:ListItem Text="22" Value="22"></asp:ListItem><asp:ListItem Text="23" Value="23"></asp:ListItem><asp:ListItem Text="24" Value="24"></asp:ListItem><asp:ListItem Text="25" Value="25"></asp:ListItem><asp:ListItem Text="26" Value="26"></asp:ListItem><asp:ListItem Text="27" Value="27"></asp:ListItem><asp:ListItem Text="28" Value="28"></asp:ListItem><asp:ListItem Text="29" Value="29"></asp:ListItem><asp:ListItem Text="30" Value="30"></asp:ListItem><asp:ListItem Text="31" Value="31"></asp:ListItem><asp:ListItem Text="32" Value="32"></asp:ListItem><asp:ListItem Text="33" Value="33"></asp:ListItem><asp:ListItem Text="34" Value="34"></asp:ListItem><asp:ListItem Text="35" Value="35"></asp:ListItem><asp:ListItem Text="36" Value="36"></asp:ListItem><asp:ListItem Text="37" Value="37"></asp:ListItem><asp:ListItem Text="38" Value="38"></asp:ListItem><asp:ListItem Text="39" Value="39"></asp:ListItem><asp:ListItem Text="40" Value="40"></asp:ListItem><asp:ListItem Text="41" Value="41"></asp:ListItem><asp:ListItem Text="42" Value="42"></asp:ListItem><asp:ListItem Text="43" Value="43"></asp:ListItem><asp:ListItem Text="44" Value="44"></asp:ListItem><asp:ListItem Text="45" Value="45"></asp:ListItem><asp:ListItem Text="46" Value="46"></asp:ListItem><asp:ListItem Text="47" Value="47"></asp:ListItem><asp:ListItem Text="48" Value="48"></asp:ListItem><asp:ListItem Text="49" Value="49"></asp:ListItem><asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                         <div class="row-fluid">
                                            <div class="span12">
                                                <div class="control-group">
                                                    <asp:Label ID="lblESPA" runat="server" AssociatedControlID="ddlESPA" Text="Habilitación Previa:" class="control-label"></asp:Label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlESPA" runat="server" Width="130px" AutoPostBack="true" >
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem><asp:ListItem Text="1" Value="1"></asp:ListItem><asp:ListItem Text="2" Value="2"></asp:ListItem><asp:ListItem Text="3" Value="3"></asp:ListItem><asp:ListItem Text="4" Value="4"></asp:ListItem><asp:ListItem Text="5" Value="5"></asp:ListItem><asp:ListItem Text="6" Value="6"></asp:ListItem><asp:ListItem Text="7" Value="7"></asp:ListItem><asp:ListItem Text="8" Value="8"></asp:ListItem><asp:ListItem Text="9" Value="9"></asp:ListItem><asp:ListItem Text="10" Value="10"></asp:ListItem><asp:ListItem Text="11" Value="11"></asp:ListItem><asp:ListItem Text="12" Value="12"></asp:ListItem><asp:ListItem Text="13" Value="13"></asp:ListItem><asp:ListItem Text="14" Value="14"></asp:ListItem><asp:ListItem Text="15" Value="15"></asp:ListItem><asp:ListItem Text="16" Value="16"></asp:ListItem><asp:ListItem Text="17" Value="17"></asp:ListItem><asp:ListItem Text="18" Value="18"></asp:ListItem><asp:ListItem Text="19" Value="19"></asp:ListItem><asp:ListItem Text="20" Value="20"></asp:ListItem><asp:ListItem Text="21" Value="21"></asp:ListItem><asp:ListItem Text="22" Value="22"></asp:ListItem><asp:ListItem Text="23" Value="23"></asp:ListItem><asp:ListItem Text="24" Value="24"></asp:ListItem><asp:ListItem Text="25" Value="25"></asp:ListItem><asp:ListItem Text="26" Value="26"></asp:ListItem><asp:ListItem Text="27" Value="27"></asp:ListItem><asp:ListItem Text="28" Value="28"></asp:ListItem><asp:ListItem Text="29" Value="29"></asp:ListItem><asp:ListItem Text="30" Value="30"></asp:ListItem><asp:ListItem Text="31" Value="31"></asp:ListItem><asp:ListItem Text="32" Value="32"></asp:ListItem><asp:ListItem Text="33" Value="33"></asp:ListItem><asp:ListItem Text="34" Value="34"></asp:ListItem><asp:ListItem Text="35" Value="35"></asp:ListItem><asp:ListItem Text="36" Value="36"></asp:ListItem><asp:ListItem Text="37" Value="37"></asp:ListItem><asp:ListItem Text="38" Value="38"></asp:ListItem><asp:ListItem Text="39" Value="39"></asp:ListItem><asp:ListItem Text="40" Value="40"></asp:ListItem><asp:ListItem Text="41" Value="41"></asp:ListItem><asp:ListItem Text="42" Value="42"></asp:ListItem><asp:ListItem Text="43" Value="43"></asp:ListItem><asp:ListItem Text="44" Value="44"></asp:ListItem><asp:ListItem Text="45" Value="45"></asp:ListItem><asp:ListItem Text="46" Value="46"></asp:ListItem><asp:ListItem Text="47" Value="47"></asp:ListItem><asp:ListItem Text="48" Value="48"></asp:ListItem><asp:ListItem Text="49" Value="49"></asp:ListItem><asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                         <%-- Boton para Actualizar --%>
                        <asp:UpdatePanel ID="updPnlGuardar" runat="server">
                            <ContentTemplate>
                                <div class="pull-right ">
                                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn  btn-inverse" ValidationGroup="Guardar" OnClick="btnGuardar_OnClick">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                    </div>
                </div>
            </div>
        </div>
        
    <div id="frmMsj" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Mensaje</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon imoon-info fs32" style="color: darkcyan"></label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updMsj" runat="server" class="form-group">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMsj" runat="server" Style="color: Black"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="$('.modal-backdrop').remove();">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function showfrmMsj() {
            $('.modal-backdrop').remove();
            $("#frmMsj").modal("show");
            return false;
        }
    </script>

    </asp:Panel>

</asp:Content>
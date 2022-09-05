<%@ Page Title="Configuración de procesos de pagos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Configurar_Pagos.aspx.cs" Inherits="SGI.GestionTramite.Configurar_Pagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1>Configuración de procesos de pagos.</h1>
    </hgroup>

    <%: Styles.Render("~/Content/themes/base/css") %>
    <%: Scripts.Render("~/bundles/timepicker") %>
    <%: Styles.Render("~/bundles/timepickerCss") %>
    <%: Scripts.Render("~/bundles/gritter") %>



    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
        <div class="accordion-heading">
            <a id="bt_Vencimiento" data-parent="#collapse-group" href="#collapse_bt_Vencimiento" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="lblVencimiento" runat="server" Text="Proceso de vencimiento de boletas"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                </div>
            </a>
        </div>
        <div class="accordion-body collapse in" id="collapse_bt_Vencimiento">
            <asp:UpdatePanel ID="updPnlVencimiento" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset>
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraInicioVencimiento" runat="server" class="control-label" AssociatedControlID="txtHoraInicioVencimiento" Text="Hora Inicial: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraInicioVencimiento" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraInicioVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraInicioVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Inicio del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraFinVencimiento" runat="server" class="control-label" AssociatedControlID="txtHoraFinVencimiento" Text="Hora Final: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraFinVencimiento" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraFinVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraFinVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Fin del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span4">
                                    <div class="control-group">
                                        <asp:Label ID="lblIntervaloVencimiento" runat="server" class="control-label" AssociatedControlID="txtIntervaloVencimiento" Text="Intervalo de Ejecución (Minutos): "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtIntervaloVencimiento" runat="server" CssClass="form-control" Width="120px" onkeypress="return IsNumeric(event, $('#Val_Formato_IntervaloVencimiento'), $('#Req_IntervaloVencimiento'));"></asp:TextBox>
                                            <div id="Val_Formato_IntervaloVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar solo Números enteros.
                                            </div>
                                            <div id="Req_IntervaloVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el intervalo de ejecución del proceso.
                                            </div>
                                            <div id="Rango_IntervaloVencimiento" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Intervalo no puede ser superior a 1380 minutos (23 Horas).
                                            </div>
                                            <div id="info_Vencimiento" class="alert alert-small alert-info mbottom0 mtop5 mright10" style="display: none;">
                                                Si el intervalo supera los 60 minutos, redondea por hora.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%-- Boton para Actualizar --%>
            <asp:UpdatePanel ID="updPnlGuardarVencimiento" runat="server">
                <ContentTemplate>
                    <div class="pull-right mright20 mbottom20">
                        <asp:LinkButton ID="btnGuardarVencimiento" runat="server" CssClass="btn btn-inverse" OnClientClick="return validarGuardarVencimiento();" OnClick="btnGuardarVencimiento_Click">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
        <div class="accordion-heading">
            <a id="bt_BolVencidas" data-parent="#collapse-group" href="#collapse_bt_BolVencidas" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="lblBolVencidas" runat="server" Text="Proceso de actualización de pago post-Vencimiento"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                </div>
            </a>
        </div>
        <div class="accordion-body collapse in" id="collapse_bt_BolVencidas">
            <asp:UpdatePanel ID="updPnlBolVencidas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset>
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraInicioBolVencidas" runat="server" class="control-label" AssociatedControlID="txtHoraInicioBolVencidas" Text="Hora Inicial: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraInicioBolVencidas" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraInicioBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraInicioBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Inicio del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraFinBolVencidas" runat="server" class="control-label" AssociatedControlID="txtHoraFinBolVencidas" Text="Hora Final: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraFinBolVencidas" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraFinBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraFinBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Fin del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblIntervaloBolVencidas" runat="server" class="control-label" AssociatedControlID="txtIntervaloBolVencidas" Text="Intervalo de Ejecución (Minutos): "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtIntervaloBolVencidas" runat="server" CssClass="form-control" Width="120px" onkeypress="return IsNumeric(event, $('#Val_Formato_IntervaloBolVencidas'), $('#Req_IntervaloBolVencidas'));"></asp:TextBox>
                                            <div id="Val_Formato_IntervaloBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar solo Números enteros.
                                            </div>
                                            <div id="Req_IntervaloBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el intervalo de ejecución del proceso.
                                            </div>
                                            <div id="Rango_IntervaloBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Intervalo no puede ser superior a 1380 minutos (23 Horas).
                                            </div>
                                            <div id="info_BolVencida" class="alert alert-small alert-info mbottom0 mtop5 mright10" style="display: none;">
                                                Si el intervalo supera los 60 minutos, redondea por hora.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblDiasEvaluarBolVencidas" runat="server" class="control-label" AssociatedControlID="txtDiasEvaluarBolVencidas" Text="Días a evaluar: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDiasEvaluarBolVencidas" runat="server" CssClass="form-control" Width="120px" onkeypress="return IsNumeric(event, $('#Val_Formato_DiasEvaluarBolVencidas'), $('#Req_DiasEvaluarBolVencidas'));"></asp:TextBox>
                                            <div id="Val_Formato_DiasEvaluarBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar solo Números enteros.
                                            </div>
                                            <div id="Req_DiasEvaluarBolVencidas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la cantidad de dias a evaluar.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%-- Boton para Actualizar --%>
            <asp:UpdatePanel ID="updPnlGuardarBolVencidas" runat="server">
                <ContentTemplate>
                    <div class="pull-right mright20 mbottom20">
                        <asp:LinkButton ID="btnGuardarBolVencidas" runat="server" CssClass="btn btn-inverse" OnClientClick="return validarGuardarBolVencidas();" OnClick="btnGuardarBolVencidas_Click">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
        <div class="accordion-heading">
            <a id="bt_EstadoBoletas" data-parent="#collapse-group" href="#collapse_bt_EstadoBoletas" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="Label1" runat="server" Text="Proceso de validación de estados de boletas"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                </div>
            </a>
        </div>
        <div class="accordion-body collapse in" id="collapse_bt_EstadoBoletas">
            <asp:UpdatePanel ID="updPnlEstadoBoletas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset>
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraInicioEstadoBoletas" runat="server" class="control-label" AssociatedControlID="txtHoraInicioEstadoBoletas" Text="Hora Inicial: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraInicioEstadoBoletas" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Va_Formato_HoraInicioEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraInicioEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Inicio del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraFinEstadoBoletas" runat="server" class="control-label" AssociatedControlID="txtHoraFinEstadoBoletas" Text="Hora Final: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraFinEstadoBoletas" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraFinEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraFinEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Fin del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblIntervaloEstadoBoletas" runat="server" class="control-label" AssociatedControlID="txtIntervaloEstadoBoletas" Text="Intervalo de Ejecución (Minutos): "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtIntervaloEstadoBoletas" runat="server" CssClass="form-control" Width="120px" onkeypress="return IsNumeric(event, $('#Val_Formato_IntervaloEstadoBoletas'), $('#Req_IntervaloEstadoBoletas'));"></asp:TextBox>
                                            <div id="Val_Formato_IntervaloEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar solo Números enteros.
                                            </div>
                                            <div id="Req_IntervaloEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el intervalo de ejecución del proceso.
                                            </div>
                                            <div id="Rango_IntervaloEstadoBoletas" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Intervalo no puede ser superior a 1380 minutos (23 Horas).
                                            </div>
                                            <div id="info_EstadoBoleta" class="alert alert-small alert-info mbottom0 mtop5 mright10" style="display: none;">
                                                Si el intervalo supera los 60 minutos, redondea por hora.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%-- Boton para Actualizar --%>
            <asp:UpdatePanel ID="updPnlGuardarEstadoBoletas" runat="server">
                <ContentTemplate>
                    <div class="pull-right mright20 mbottom20">
                        <asp:LinkButton ID="btnGuardarEstadoBoletas" runat="server" CssClass="btn btn-inverse" OnClientClick="return validarGuardarEstadoBoletas();" OnClick="btnGuardarEstadoBoletas_Click">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
        <div class="accordion-heading">
            <a id="bt_Pagas_Sin_Fecha" data-parent="#collapse-group" href="#collapse_bt_Pagas_Sin_Fecha" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="lblPagas_Sin_Fecha" runat="server" Text="Proceso de actualización de boletas pagas sin fecha"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                </div>
            </a>
        </div>
        <div class="accordion-body collapse in" id="collapse_bt_Pagas_Sin_Fecha">
            <asp:UpdatePanel ID="updPnlPagas_Sin_Fecha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset>
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraInicioPagas_Sin_Fecha" runat="server" class="control-label" AssociatedControlID="txtHoraInicioPagas_Sin_Fecha" Text="Hora Inicial: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraInicioPagas_Sin_Fecha" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraInicioPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraInicioPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Inicio del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblHoraFinPagas_Sin_Fecha" runat="server" class="control-label" AssociatedControlID="txtHoraFinPagas_Sin_Fecha" Text="Hora Final: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtHoraFinPagas_Sin_Fecha" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
                                            <div id="Val_Formato_HoraFinPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Formato de la Hora es inv&aacute;lido, el mismo debe ser HH:mm. Ej: 23:57
                                            </div>
                                            <div id="Req_HoraFinPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la Hora de Fin del proceso.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblIntervaloPagas_Sin_Fecha" runat="server" class="control-label" AssociatedControlID="txtIntervaloPagas_Sin_Fecha" Text="Intervalo de Ejecución (Minutos): "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtIntervaloPagas_Sin_Fecha" runat="server" CssClass="form-control" Width="120px" onkeypress="return IsNumeric(event, $('#Val_Formato_IntervaloPagas_Sin_Fecha'), $('#Req_IntervaloPagas_Sin_Fecha'));"></asp:TextBox>
                                            <div id="Val_Formato_IntervaloPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar solo Números enteros.
                                            </div>
                                            <div id="Req_IntervaloPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar el intervalo de ejecución del proceso.
                                            </div>
                                            <div id="Rango_IntervaloPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                El Intervalo no puede ser superior a 1380 minutos (23 Horas).
                                            </div>
                                            <div id="info_Pagas_Sin_Fecha" class="alert alert-small alert-info mbottom0 mtop5 mright10" style="display: none;">
                                                Si el intervalo supera los 60 minutos, redondea por hora.
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span6">
                                    <div class="control-group">
                                        <asp:Label ID="lblDiasEvaluarPagas_Sin_Fecha" runat="server" class="control-label" AssociatedControlID="txtDiasEvaluarPagas_Sin_Fecha" Text="Días a evaluar: "></asp:Label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDiasEvaluarPagas_Sin_Fecha" runat="server" CssClass="form-control" Width="120px" onkeypress="return IsNumeric(event, $('#Val_Formato_DiasEvaluarPagas_Sin_Fecha'), $('#Req_DiasEvaluarPagas_Sin_Fecha'));"></asp:TextBox>
                                            <div id="Val_Formato_DiasEvaluarPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar solo Números enteros.
                                            </div>
                                            <div id="Req_DiasEvaluarPagas_Sin_Fecha" class="alert alert-small alert-danger mbottom0 mtop5" style="display: none;">
                                                Debe ingresar la cantidad de dias a evaluar.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%-- Boton para Actualizar --%>
            <asp:UpdatePanel ID="updPnlGuardarPagas_Sin_Fecha" runat="server">
                <ContentTemplate>
                    <div class="pull-right mright20 mbottom20">
                        <asp:LinkButton ID="btnGuardarPagas_Sin_Fecha" runat="server" CssClass="btn btn-inverse" OnClientClick="return validarGuardarPagas_Sin_Fecha();" OnClick="btnGuardarPagas_Sin_Fecha_Click">
                                    <i class="icon-white icon-refresh"></i>
                                    <span class="text">Actualizar</span>
                        </asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <div class="accordion-group widget-box" style="margin-top: 0px; margin-bottom: 5px">
        <div class="accordion-heading">
            <a id="bt_Jobs" data-parent="#collapse-group" href="#collapse_bt_Jobs" data-toggle="collapse" onclick="bt_btnUpDown_collapse_click(this)">
                <div class="widget-title">
                    <span class="icon"><i class="icon-list-alt"></i></span>
                    <h5>
                        <asp:Label ID="lblEstadoJobs" runat="server" Text="Estado de Jobs"></asp:Label></h5>
                    <span class="btn-right"><i class="icon-chevron-up"></i></span>
                </div>
            </a>
        </div>
        <div class="accordion-body collapse in" id="collapse_bt_Jobs">
            <asp:UpdatePanel ID="updEstadoJobsVencimiento" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <asp:Label ID="lblEstadoVencimiento" runat="server" class="control-label" AssociatedControlID="txtEstadoVencimiento" Text="Job Vencer Boletas: "></asp:Label>
                            <asp:Label ID="txtEstadoVencimiento" runat="server" Style="text-align: center" Font-Size="Medium" class="control-label" Text="Estado"></asp:Label>
                            <div class="controls">
                                <asp:LinkButton ID="lnkEstadoVencimiento" Text="Boton Estado" runat="server" CssClass="btn  btn-inverse" OnClick="lnkEstadoVencimiento_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="updEstadoJobsBolVencidas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <asp:Label ID="lblEstadoBolVencidas" runat="server" class="control-label" AssociatedControlID="txtEstadoBolVencidas" Text="Job Evaluar Boletas Vencidas: "></asp:Label>
                            <asp:Label ID="txtEstadoBolVencidas" runat="server" Style="text-align: center" Font-Size="Medium" class="control-label" Text="Estado"></asp:Label>
                            <div class="controls">
                                <asp:LinkButton ID="lnkEstadoBolVencidas" Text="Boton Estado" runat="server" CssClass="btn  btn-inverse" OnClick="lnkEstadoBolVencidas_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="updEstadoJobsBoletas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <asp:Label ID="lblEstadoBoletas" runat="server" class="control-label" AssociatedControlID="txtEstadoBoletas" Text="Job Evaluar Boletas Pendientes de Pago: "></asp:Label>
                            <asp:Label ID="txtEstadoBoletas" runat="server" Style="text-align: center" Font-Size="Medium" class="control-label" Text="Estado"></asp:Label>
                            <div class="controls">
                                <asp:LinkButton ID="lnkEstadoBoletas" Text="Boton Estado" runat="server" CssClass="btn  btn-inverse" OnClick="lnkEstadoBoletas_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updEstadoJobsPagas_Sin_Fecha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <asp:Label ID="lblEstadoPagas_Sin_Fecha" runat="server" class="control-label" AssociatedControlID="txtEstadoPagas_Sin_Fecha" Text="Job Evaluar Boletas sin fecha pago: "></asp:Label>
                            <asp:Label ID="txtEstadoPagas_Sin_Fecha" runat="server" Style="text-align: center" Font-Size="Medium" class="control-label" Text="Estado"></asp:Label>
                            <div class="controls">
                                <asp:LinkButton ID="lnkEstadoPagas_Sin_Fecha" Text="Boton Estado" runat="server" CssClass="btn  btn-inverse" OnClick="lnkEstadoPagas_Sin_Fecha_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            init_Js_Vencimiento();
            init_Js_BolVencidas();
            init_Js_EstadoBoletas();
            MostrarInfo();
        });

        function mostratMensaje(texto) {

            $.gritter.add({
                title: 'Parametros',
                text: texto,
                image: '../Content/img/info32.png',
                sticky: false
            });

        }

        function bt_btnUpDown_collapse_click(obj) {
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
        function MostrarInfo() {
            $("#info_BolVencida").css("display", "inline-block");
            $("#info_Vencimiento").css("display", "inline-block");
            $("#info_EstadoBoleta").css("display", "inline-block");
            $("#info_Pagas_Sin_Fecha").css("display", "inline-block");
        }

        function IsNumeric(evt, Val, Req) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                (Val).css("display", "inline-block");
                (Req).hide();
                return false;
            }
            (Val).hide();
            (Req).hide();
            return true;
        }

        function validarGuardarEstadoBoletas() {
            var ret = true;
            var formatoHora = /^([0-1][0-9]|[0-2][0-3]):([0-5][0-9])$/;

            $("#Val_Formato_HoraInicioEstadoBoletas").hide();
            $("#Req_HoraInicioEstadoBoletas").hide();

            $("#Val_Formato_HoraFinEstadoBoletas").hide();
            $("#Req_HoraFinEstadoBoletas").hide();

            $("#Val_Formato_IntervaloEstadoBoletas").hide();
            $("#Req_IntervaloEstadoBoletas").hide();

            $("#Rango_IntervaloEstadoBoletas").hide();


            if ($.trim($("#<%: txtIntervaloEstadoBoletas.ClientID %>").val()).length == 0) {
                $("#Req_IntervaloEstadoBoletas").css("display", "inline-block");
                ret = false;
            } else if ($.trim($("#<%: txtIntervaloEstadoBoletas.ClientID %>").val()) > 1380) {
                $("#Rango_IntervaloEstadoBoletas").css("display", "inline-block");
                ret = false;
            }


            if ($.trim($("#<%: txtHoraInicioEstadoBoletas.ClientID %>").val()).length == 0) {
                $("#Req_HoraInicioEstadoBoletas").css("display", "inline-block");
                ret = false;
            } else if ($.trim($("#<%: txtHoraInicioEstadoBoletas.ClientID %>").val()).length > 0) {
                if (!formatoHora.test($.trim($("#<%: txtHoraInicioEstadoBoletas.ClientID %>").val()))) {
                    $("#Val_Formato_HoraInicioEstadoBoletas").css("display", "inline-block");
                    ret = false;
                }
            }

        if ($.trim($("#<%: txtHoraFinEstadoBoletas.ClientID %>").val()).length == 0) {
                $("#Req_HoraFinEstadoBoletas").css("display", "inline-block");
                ret = false;
            } else if ($.trim($("#<%: txtHoraFinEstadoBoletas.ClientID %>").val()).length > 0) {
            if (!formatoHora.test($.trim($("#<%: txtHoraFinEstadoBoletas.ClientID %>").val()))) {
                    $("#Val_Formato_HoraFinEstadoBoletas").css("display", "inline-block");
                    ret = false;
                }
            }

        return ret;
    }


    function validarGuardarBolVencidas() {
        var ret = true;
        var formatoHora = /^([0-1][0-9]|[0-2][0-3]):([0-5][0-9])$/;

        $("#Req_IntervaloBolVencidas").hide();
        $("#Val_Formato_IntervaloBolVencidas").hide();

        $("#Req_DiasEvaluarBolVencidas").hide();
        $("#Val_Formato_DiasEvaluarBolVencidas").hide();

        $("#Req_HoraInicioBolVencidas").hide();
        $("#Val_Formato_HoraInicioBolVencidas").hide();

        $("#Req_HoraFinBolVencidas").hide();
        $("#Val_Formato_HoraFinBolVencidas").hide();

        $("#Rango_IntervaloBolVencidas").hide();



        if ($.trim($("#<%: txtIntervaloBolVencidas.ClientID %>").val()).length == 0) {
            $("#Req_IntervaloBolVencidas").css("display", "inline-block");
            ret = false;
        } else if ($.trim($("#<%: txtIntervaloBolVencidas.ClientID %>").val()) > 1380) {
            $("#Rango_IntervaloBolVencidas").css("display", "inline-block");
            ret = false;
        }

        if ($.trim($("#<%: txtDiasEvaluarBolVencidas.ClientID %>").val()).length == 0) {
            $("#Req_DiasEvaluarBolVencidas").css("display", "inline-block");
            ret = false;
        }


        if ($.trim($("#<%: txtHoraInicioBolVencidas.ClientID %>").val()).length == 0) {
            $("#Req_HoraInicioBolVencidas").css("display", "inline-block");
            ret = false;
        } else if ($.trim($("#<%: txtHoraInicioBolVencidas.ClientID %>").val()).length > 0) {
            if (!formatoHora.test($.trim($("#<%: txtHoraInicioBolVencidas.ClientID %>").val()))) {
                $("#Val_Formato_HoraInicioBolVencidas").css("display", "inline-block");
                ret = false;
            }
        }

    if ($.trim($("#<%: txtHoraFinBolVencidas.ClientID %>").val()).length == 0) {
            $("#Req_HoraFinBolVencidas").css("display", "inline-block");
            ret = false;
        } else if ($.trim($("#<%: txtHoraFinBolVencidas.ClientID %>").val()).length > 0) {
        if (!formatoHora.test($.trim($("#<%: txtHoraFinBolVencidas.ClientID %>").val()))) {
                $("#Val_Formato_HoraFinBolVencidas").css("display", "inline-block");
                ret = false;
            }
        }
    return ret;

}


function validarGuardarVencimiento() {

    var ret = true;
    var formatoHora = /^([0-1][0-9]|[0-2][0-3]):([0-5][0-9])$/;

    $("#Val_Formato_HoraInicioVencimiento").hide();
    $("#Req_HoraInicioVencimiento").hide();

    $("#Val_Formato_HoraFinVencimiento").hide();
    $("#Req_HoraFinVencimiento").hide();

    $("#Val_Formato_IntervaloVencimiento").hide();
    $("#Req_IntervaloVencimiento").hide();

    $("#Rango_IntervaloVencimiento").hide();


    if ($.trim($("#<%: txtIntervaloVencimiento.ClientID %>").val()).length == 0) {
        $("#Req_IntervaloVencimiento").css("display", "inline-block");
        ret = false;
    } else if ($.trim($("#<%: txtIntervaloVencimiento.ClientID %>").val()) > 1380) {
        $("#Rango_IntervaloVencimiento").css("display", "inline-block");
        ret = false;
    }


    if ($.trim($("#<%: txtHoraInicioVencimiento.ClientID %>").val()).length == 0) {
        $("#Req_HoraInicioVencimiento").css("display", "inline-block");
        ret = false;
    } else if ($.trim($("#<%: txtHoraInicioVencimiento.ClientID %>").val()).length > 0) {
        if (!formatoHora.test($.trim($("#<%: txtHoraInicioVencimiento.ClientID %>").val()))) {
            $("#Val_Formato_HoraInicioVencimiento").css("display", "inline-block");
            ret = false;
        }
    }

    if ($.trim($("#<%: txtHoraFinVencimiento.ClientID %>").val()).length == 0) {
        $("#Req_HoraFinVencimiento").css("display", "inline-block");
        ret = false;
    } else if ($.trim($("#<%: txtHoraFinVencimiento.ClientID %>").val()).length > 0) {
        if (!formatoHora.test($.trim($("#<%: txtHoraFinVencimiento.ClientID %>").val()))) {
            $("#Val_Formato_HoraFinVencimiento").css("display", "inline-block");
            ret = false;
        }
    }

    return ret;

}


        function validarGuardarPagas_Sin_Fecha() {

            var ret = true;
            var formatoHora = /^([0-1][0-9]|[0-2][0-3]):([0-5][0-9])$/;

            $("#Val_Formato_HoraInicioPagas_Sin_Fecha").hide();
            $("#Req_HoraInicioPagas_Sin_Fecha").hide();

            $("#Val_Formato_HoraFinPagas_Sin_Fecha").hide();
            $("#Req_HoraFinPagas_Sin_Fecha").hide();

            $("#Val_Formato_IntervaloPagas_Sin_Fecha").hide();
            $("#Req_IntervaloPagas_Sin_Fecha").hide();

            $("#Rango_IntervaloPagas_Sin_Fecha").hide();


            if ($.trim($("#<%: txtIntervaloPagas_Sin_Fecha.ClientID %>").val()).length == 0) {
                $("#Req_IntervaloPagas_Sin_Fecha").css("display", "inline-block");
                ret = false;
            } else if ($.trim($("#<%: txtIntervaloPagas_Sin_Fecha.ClientID %>").val()) > 1380) {
                $("#Rango_IntervaloPagas_Sin_Fecha").css("display", "inline-block");
                ret = false;
            }


            if ($.trim($("#<%: txtHoraInicioPagas_Sin_Fecha.ClientID %>").val()).length == 0) {
                $("#Req_HoraInicioPagas_Sin_Fecha").css("display", "inline-block");
                ret = false;
            } else if ($.trim($("#<%: txtHoraInicioPagas_Sin_Fecha.ClientID %>").val()).length > 0) {
                if (!formatoHora.test($.trim($("#<%: txtHoraInicioPagas_Sin_Fecha.ClientID %>").val()))) {
                    $("#Val_Formato_HoraInicioPagas_Sin_Fecha").css("display", "inline-block");
                    ret = false;
                }
            }

            if ($.trim($("#<%: txtHoraFinPagas_Sin_Fecha.ClientID %>").val()).length == 0) {
                $("#Req_HoraFinPagas_Sin_Fecha").css("display", "inline-block");
                ret = false;
            } else if ($.trim($("#<%: txtHoraFinPagas_Sin_Fecha.ClientID %>").val()).length > 0) {
                if (!formatoHora.test($.trim($("#<%: txtHoraFinPagas_Sin_Fecha.ClientID %>").val()))) {
                    $("#Val_Formato_HoraFinPagas_Sin_Fecha").css("display", "inline-block");
                    ret = false;
                }
            }

            return ret;

        }


function init_Js_Vencimiento() {
    $("#<%: txtHoraInicioVencimiento.ClientID %>").timepicker({
        // Localization
        hourText: 'Hora',             // Define the locale text for "Hours"
        minuteText: 'Minuto',         // Define the locale text for "Minute"
        amPmText: ['', ''],       // Define the locale text for periods
        // custom hours and minutes
        hours: {
            starts: 0,                // First displayed hour
            ends: 23                  // Last displayed hour
        },
        minutes: {
            starts: 0,                // First displayed minute
            ends: 59,                 // Last displayed minute
            interval: 1,              // Interval of displayed minutes
            manual: []                // Optional extra entries for minutes
        },
        rows: 6,                      // Number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
        showHours: true,              // Define if the hours section is displayed or not. Set to false to get a minute only dialog
        showMinutes: true,
        showCloseButton: true,
        closeButtonText: 'Cerrar',
        onSelect: function () {
            $("#Req_HoraInicioVencimiento").hide();
            $("#Val_Formato_HoraInicioVencimiento").hide();
        }

    });

    $("#<%: txtHoraInicioVencimiento.ClientID %>").on("blur", function (e) {
        var hora = $.trim($("#<%: txtHoraInicioVencimiento.ClientID %>").val());
        if (hora.length > 0 && hora.length <= 5) {
            var horas = ("00" + hora.substring(0, 2));
            horas = horas.substring(horas.length - 2)
            var minutos = ("00" + hora.substring(3, 5));
            minutos = minutos.substring(minutos.length - 2)
            $("#<%: txtHoraInicioVencimiento.ClientID %>").val(horas + ":" + minutos);
        }
    });


    $("#<%: txtHoraInicioVencimiento.ClientID %>").on("keyup", function (e) {
        if (e.keyCode != 13) {
            $("#Req_HoraInicioVencimiento").hide();
            $("#Val_Formato_HoraInicioVencimiento").hide();
        }
    });

    $("#<%: txtHoraFinVencimiento.ClientID %>").timepicker({
        // Localization
        hourText: 'Hora',             // Define the locale text for "Hours"
        minuteText: 'Minuto',         // Define the locale text for "Minute"
        amPmText: ['', ''],       // Define the locale text for periods
        // custom hours and minutes
        hours: {
            starts: 0,                // First displayed hour
            ends: 23                  // Last displayed hour
        },
        minutes: {
            starts: 0,                // First displayed minute
            ends: 59,                 // Last displayed minute
            interval: 1,              // Interval of displayed minutes
            manual: []                // Optional extra entries for minutes
        },
        rows: 6,                      // Number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
        showHours: true,              // Define if the hours section is displayed or not. Set to false to get a minute only dialog
        showMinutes: true,
        showCloseButton: true,
        closeButtonText: 'Cerrar',
        onSelect: function () {
            $("#Req_HoraFinVencimiento").hide();
            $("#Val_Formato_HoraFinVencimiento").hide();
        }

    });

    $("#<%: txtHoraFinVencimiento.ClientID %>").on("blur", function (e) {
        var hora = $.trim($("#<%: txtHoraFinVencimiento.ClientID %>").val());
        if (hora.length > 0 && hora.length <= 5) {
            var horas = ("00" + hora.substring(0, 2));
            horas = horas.substring(horas.length - 2)
            var minutos = ("00" + hora.substring(3, 5));
            minutos = minutos.substring(minutos.length - 2)
            $("#<%: txtHoraFinVencimiento.ClientID %>").val(horas + ":" + minutos);
        }
    });


    $("#<%: txtHoraFinVencimiento.ClientID %>").on("keyup", function (e) {
        if (e.keyCode != 13) {
            $("#Req_HoraFinVencimiento").hide();
            $("#Val_Formato_HoraFinVencimiento").hide();
        }
    });
}

function init_Js_EstadoBoletas() {
    $("#<%: txtHoraInicioEstadoBoletas.ClientID %>").timepicker({
        // Localization
        hourText: 'Hora',             // Define the locale text for "Hours"
        minuteText: 'Minuto',         // Define the locale text for "Minute"
        amPmText: ['', ''],       // Define the locale text for periods
        // custom hours and minutes
        hours: {
            starts: 0,                // First displayed hour
            ends: 23                  // Last displayed hour
        },
        minutes: {
            starts: 0,                // First displayed minute
            ends: 59,                 // Last displayed minute
            interval: 1,              // Interval of displayed minutes
            manual: []                // Optional extra entries for minutes
        },
        rows: 6,                      // Number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
        showHours: true,              // Define if the hours section is displayed or not. Set to false to get a minute only dialog
        showMinutes: true,
        showCloseButton: true,
        closeButtonText: 'Cerrar',
        onSelect: function () {
            $("#Req_HoraInicioEstadoBoletas").hide();
            $("#Val_Formato_HoraInicioEstadoBoletas").hide();
        }

    });

    $("#<%: txtHoraInicioEstadoBoletas.ClientID %>").on("blur", function (e) {
        var hora = $.trim($("#<%: txtHoraInicioEstadoBoletas.ClientID %>").val());
        if (hora.length > 0 && hora.length <= 5) {
            var horas = ("00" + hora.substring(0, 2));
            horas = horas.substring(horas.length - 2)
            var minutos = ("00" + hora.substring(3, 5));
            minutos = minutos.substring(minutos.length - 2)
            $("#<%: txtHoraInicioEstadoBoletas.ClientID %>").val(horas + ":" + minutos);
            }
    });


        $("#<%: txtHoraInicioEstadoBoletas.ClientID %>").on("keyup", function (e) {
        if (e.keyCode != 13) {
            $("#Req_HoraInicioEstadoBoletas").hide();
            $("#Val_Formato_HoraInicioEstadoBoletas").hide();
        }
    });

    $("#<%: txtHoraFinEstadoBoletas.ClientID %>").timepicker({
        // Localization
        hourText: 'Hora',             // Define the locale text for "Hours"
        minuteText: 'Minuto',         // Define the locale text for "Minute"
        amPmText: ['', ''],       // Define the locale text for periods
        // custom hours and minutes
        hours: {
            starts: 0,                // First displayed hour
            ends: 23                  // Last displayed hour
        },
        minutes: {
            starts: 0,                // First displayed minute
            ends: 59,                 // Last displayed minute
            interval: 1,              // Interval of displayed minutes
            manual: []                // Optional extra entries for minutes
        },
        rows: 6,                      // Number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
        showHours: true,              // Define if the hours section is displayed or not. Set to false to get a minute only dialog
        showMinutes: true,
        showCloseButton: true,
        closeButtonText: 'Cerrar',
        onSelect: function () {
            $("#Req_HoraFinEstadoBoletas").hide();
            $("#Val_Formato_HoraFinEstadoBoletas").hide();
        }

    });

    $("#<%: txtHoraFinEstadoBoletas.ClientID %>").on("blur", function (e) {
        var hora = $.trim($("#<%: txtHoraFinEstadoBoletas.ClientID %>").val());
        if (hora.length > 0 && hora.length <= 5) {
            var horas = ("00" + hora.substring(0, 2));
            horas = horas.substring(horas.length - 2)
            var minutos = ("00" + hora.substring(3, 5));
            minutos = minutos.substring(minutos.length - 2)
            $("#<%: txtHoraFinEstadoBoletas.ClientID %>").val(horas + ":" + minutos);
            }
    });


        $("#<%: txtHoraFinEstadoBoletas.ClientID %>").on("keyup", function (e) {
        if (e.keyCode != 13) {
            $("#Req_HoraFinEstadoBoletas").hide();
            $("#Val_Formato_HoraFinEstadoBoletas").hide();
        }
    });
}

function init_Js_BolVencidas() {
    $("#<%: txtHoraInicioBolVencidas.ClientID %>").timepicker({
        // Localization
        hourText: 'Hora',             // Define the locale text for "Hours"
        minuteText: 'Minuto',         // Define the locale text for "Minute"
        amPmText: ['', ''],       // Define the locale text for periods
        // custom hours and minutes
        hours: {
            starts: 0,                // First displayed hour
            ends: 23                  // Last displayed hour
        },
        minutes: {
            starts: 0,                // First displayed minute
            ends: 59,                 // Last displayed minute
            interval: 1,              // Interval of displayed minutes
            manual: []                // Optional extra entries for minutes
        },
        rows: 6,                      // Number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
        showHours: true,              // Define if the hours section is displayed or not. Set to false to get a minute only dialog
        showMinutes: true,
        showCloseButton: true,
        closeButtonText: 'Cerrar',
        onSelect: function () {
            $("#Req_HoraInicioBolVencidas").hide();
            $("#Val_Formato_HoraInicioBolVencidas").hide();
        }

    });

    $("#<%: txtHoraInicioBolVencidas.ClientID %>").on("blur", function (e) {
        var hora = $.trim($("#<%: txtHoraInicioBolVencidas.ClientID %>").val());
            if (hora.length > 0 && hora.length <= 5) {
                var horas = ("00" + hora.substring(0, 2));
                horas = horas.substring(horas.length - 2)
                var minutos = ("00" + hora.substring(3, 5));
                minutos = minutos.substring(minutos.length - 2)
                $("#<%: txtHoraInicioBolVencidas.ClientID %>").val(horas + ":" + minutos);
            }
        });


        $("#<%: txtHoraInicioBolVencidas.ClientID %>").on("keyup", function (e) {
        if (e.keyCode != 13) {
            $("#Req_HoraInicioBolVencidas").hide();
            $("#Val_Formato_HoraInicioBolVencidas").hide();
        }
    });

    $("#<%: txtHoraFinBolVencidas.ClientID %>").timepicker({
        // Localization
        hourText: 'Hora',             // Define the locale text for "Hours"
        minuteText: 'Minuto',         // Define the locale text for "Minute"
        amPmText: ['', ''],       // Define the locale text for periods
        // custom hours and minutes
        hours: {
            starts: 0,                // First displayed hour
            ends: 23                  // Last displayed hour
        },
        minutes: {
            starts: 0,                // First displayed minute
            ends: 59,                 // Last displayed minute
            interval: 1,              // Interval of displayed minutes
            manual: []                // Optional extra entries for minutes
        },
        rows: 6,                      // Number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
        showHours: true,              // Define if the hours section is displayed or not. Set to false to get a minute only dialog
        showMinutes: true,
        showCloseButton: true,
        closeButtonText: 'Cerrar',
        onSelect: function () {
            $("#Req_HoraFinBolVencidas").hide();
            $("#Val_Formato_HoraFinBolVencidas").hide();
        }

    });

    $("#<%: txtHoraFinBolVencidas.ClientID %>").on("blur", function (e) {
        var hora = $.trim($("#<%: txtHoraFinBolVencidas.ClientID %>").val());
            if (hora.length > 0 && hora.length <= 5) {
                var horas = ("00" + hora.substring(0, 2));
                horas = horas.substring(horas.length - 2)
                var minutos = ("00" + hora.substring(3, 5));
                minutos = minutos.substring(minutos.length - 2)
                $("#<%: txtHoraFinBolVencidas.ClientID %>").val(horas + ":" + minutos);
            }
        });


        $("#<%: txtHoraFinBolVencidas.ClientID %>").on("keyup", function (e) {
        if (e.keyCode != 13) {
            $("#Req_HoraFinBolVencidas").hide();
            $("#Val_Formato_HoraFinBolVencidas").hide();
        }
    });
}
    </script>
</asp:Content>

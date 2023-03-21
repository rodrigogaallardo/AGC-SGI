<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuscarUbicacion.ascx.cs" Inherits="SGI.Controls.BuscarUbicacion" %>
<%@ Register Src="~/Controls/SolicitudNuevaPuerta.ascx" TagPrefix="uc1" TagName="SolicitudNuevaPuerta" %>

<link href="<%: ResolveUrl("~/Content/themes/base/jquery.ui.custom.css") %>" rel="stylesheet" />

<%: Scripts.Render("~/bundles/autoNumeric") %>
<%: Scripts.Render("~/bundles/select2") %>
<%: Styles.Render("~/bundles/Select2Css") %>

<style>
    label,
    input,
    button,
    select,
    textarea {
        font-size: 12px;  
    }
</style>
<div id="pnlBuscarUbicacion" style="width: 100%;">
    <asp:UpdatePanel ID="updBuscarUbicacion" runat="server" >
        <ContentTemplate>

            <asp:Panel ID="pnlContentBuscar" runat="server">

                <%--Tabs de Busqueda--%>
                <div id="tabs" class="mtop10">

                    <ul>
                        <li id="li1"><a href="#tabs-1">N&uacute;mero de Partida</a></li>
                        <li id="li2"><a href="#tabs-2">Domicilio</a></li>
                        <li id="li3"><a href="#tabs-3">Secci&oacute;n / Manzana / Parcela</a></li>
                        <li id="li4"><a href="#tabs-4">Ubicaciones Especiales (Subte/Tren/etc)</a></li>
                    </ul>

                    <asp:HiddenField ID="hid_tabselected" runat="server" />

                    <div id="tabs-1">

                        <asp:Panel ID="pnlPartidas" runat="server" DefaultButton="btnBuscar1">
                            <div>
                                <strong class="mleft10">
                                    Seleccione el Tipo de Partida:
                                </strong>
                                <table border="0" style="border-collapse: separate; border-spacing: 10px;">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="optTipoPartidaHorizontal" runat="server" GroupName="TipoPartida" />
                                        </td>
                                        <td>
                                            <b>Horizontal:</b> Es aquella que figura en la boleta de ABL y corresponde a un
                                                                lote subdividido. Un ejemplo es un Edificio de departamentos.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="optTipoPartidaMatriz" runat="server" GroupName="TipoPartida"
                                                Checked="true" />
                                        </td>
                                        <td>
                                            <b>Matriz:</b> Es aquella que figura en la boleta de ABL y corresponde a un lote
                                                                sin subdividir. Un ejemplo es una casa.
                                        </td>
                                    </tr>
                                </table>
                                
                                <div class="form-inline">
                                    <div class="form-group" >
                                        Ingrese el N&uacute;mero de Partida:
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtNroPartida" runat="server" Width="90px" MaxLength="8" Style="padding-left: 5px" CssClass="form-control"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe ingresar el número de partida"
                                            Display="Dynamic" ControlToValidate="txtNroPartida" ValidationGroup="Buscar1"
                                            CssClass="field-validation-error mleft5"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="El número de partida debe ser mayor a 0 (cero)."
                                            Display="Dynamic" ControlToValidate="txtNroPartida" ValidationGroup="Buscar1"
                                            CssClass="field-validation-error mleft5" MinimumValue="1" MaximumValue="99999999"></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>



                            <%--Contenedor del botón buscar--%>
                            <asp:UpdatePanel ID="updPanelBuscar1" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    
                                    <div class="form-inline text-right">
                                        
                                        <div class="form-group">
                                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="updPanelBuscar1" 
                                                runat="server" DisplayAfter="200">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>

                                        <div class="form-group mbottom0" >
                                            <asp:LinkButton ID="btnBuscar1" runat="server" CssClass="btn btn-default" ValidationGroup="Buscar1" OnClick="btnBuscar1_Click">
                                                <i class="imoon imoon-search"></i>
                                                <span class="text">Buscar</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCerrar1" runat="server" CssClass="btn btn-danger" OnClick="btnCerrar_Click">
                                                    <i class="imoon-white imoon-close"></i>
                                                    <span class="text">Cerrar</span>
                                            </asp:LinkButton>
                                        </div>

                                    </div>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </asp:Panel>


                    </div>

                    <%--Tab Domicilio--%>
                    <div id="tabs-2">

                        <asp:Panel ID="pnlDomicilio" runat="server" DefaultButton="btnBuscar2">
                            <table style="border-collapse: separate; border-spacing: 5px">
                                <tr>
                                    <td>Calle:
                                    </td>
                                    <td>
                                                     <ej:Autocomplete ID="AutocompleteCalles" MinCharacter="3" DataTextField="NombreOficial_calle" DataUniqueKeyField="id_calle" Width="500px" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <span style="font-size: 8pt">Debe ingresar un mínimo de 3 letras y el sistema le mostrará
                                                                las calles posibles.</span>
                                        <p>
                                            <asp:RequiredFieldValidator ID="ReqCalle" runat="server" ErrorMessage="Debe seleccionar una de las calles de la lista desplegable."
                                                Display="Dynamic" ControlToValidate="AutocompleteCalles" ValidationGroup="Buscar2"  
                                                CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>N&uacute;mero:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNroPuerta" runat="server" Width="100px" MaxLength="5" CssClass="form-control"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe ingresar el número de puerta"
                                            Display="Dynamic" ControlToValidate="txtNroPuerta" ValidationGroup="Buscar2" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="El número de puerta debe ser mayor a 0 (cero)."
                                            Display="Dynamic" ControlToValidate="txtNroPuerta" ValidationGroup="Buscar2" CssClass="field-validation-error"
                                            MinimumValue="1" MaximumValue="99999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>

                            <%--Contenedor del botón buscar--%>
                            <asp:UpdatePanel ID="updPanelBuscar2" runat="server" RenderMode="Inline">
                                <ContentTemplate>

                                    <div class="form-inline text-right">

                                        <div class="form-group">

                                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="updPanelBuscar2"
                                                runat="server" DisplayAfter="200" DynamicLayout="false">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>

                                        </div>
                                        <div class="form-group">

                                            <asp:LinkButton ID="btnBuscar2" runat="server" CssClass="btn btn-default" ValidationGroup="Buscar2" OnClick="btnBuscar2_Click">
                                                <i class="imoon imoon-search"></i>
                                                <span class="text">Buscar</span>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnCerrar2" runat="server" CssClass="btn btn-danger" OnClick="btnCerrar_Click">
                                                <i class="imoon-white imoon-close"></i>
                                                <span class="text">Cerrar</span>
                                            </asp:LinkButton>
                                        </div>
                                    
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>

                    </div>

                    <%--Búsqueda por Nomenclatura Catastral--%>
                    <div id="tabs-3">
                        <asp:Panel ID="pnlSMP_DFBTN" runat="server" DefaultButton="btnBuscar3">
                            <p>
                                Ingrese los datos catastrales:
                            </p>
                            <table style="border-collapse: separate; border-spacing: 5px">
                                <tr>
                                    <td>Secci&oacute;n:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeccion" runat="server" Width="90px" MaxLength="3" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe ingresar el número de Sección"
                                            Display="Dynamic" ControlToValidate="txtSeccion" ValidationGroup="Buscar3" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Manzana:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtManzana" runat="server" Width="90px" MaxLength="4" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe ingresar la Manzana."
                                            Display="Dynamic" ControlToValidate="txtManzana" ValidationGroup="Buscar3" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Parcela
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtParcela" runat="server" Width="90px" MaxLength="4" Style="text-transform: uppercase" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe ingresar la Parcela."
                                            Display="Dynamic" ControlToValidate="txtParcela" ValidationGroup="Buscar3" CssClass="field-validation-error"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>

                            <%--Contenedor del botón buscar--%>
                            <asp:UpdatePanel ID="updPanelBuscar3" runat="server" RenderMode="Inline">
                                <ContentTemplate>

                                    <div class="form-inline text-right">

                                        <div class="form-group">
                                            <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="updPanelBuscar3"
                                                runat="server" DisplayAfter="200">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>

                                        </div>

                                        <div class="form-group">
                                            <asp:LinkButton ID="btnBuscar3" runat="server" CssClass="btn btn-default" ValidationGroup="Buscar3" OnClick="btnBuscar3_Click">
                                                <i class="imoon imoon-search"></i>
                                                <span class="text">Buscar</span>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnCerrar3" runat="server" CssClass="btn btn-danger" OnClick="btnCerrar_Click">
                                                <i class="imoon-white imoon-close"></i>
                                                <span class="text">Cerrar</span>
                                            </asp:LinkButton>
                                        </div>
                                    
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>

                    <%--Búsquedas Especiales (Subtes,Trnes,etc)--%>
                    <div id="tabs-4">
                        <asp:Panel ID="pnlBusquedasEspeciales" runat="server" DefaultButton="btnBuscar4">

                            <asp:UpdatePanel ID="updBuscarUbicacioneEspeciales" runat="server">
                                <ContentTemplate>

                                    <table style="border-collapse: separate; border-spacing: 5px">
                                        <tr>
                                            <td class="form-label">Tipo de Ubicaci&oacute;n:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoDeUbicacion" runat="server" OnSelectedIndexChanged="ddlTipoDeUbicacion_SelectedIndexChanged"
                                                    AutoPostBack="true" Width="350px" CssClass="form-control">
                                                </asp:DropDownList>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="ReqddlTipoDeUbicacion" runat="server" ControlToValidate="ddlTipoDeUbicacion" CssClass="field-validation-error"
                                                        Display="Dynamic" ErrorMessage="Debe seleccionar el tipo de ubicación." ValidationGroup="Buscar4"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="form-label">Subtipo de Ubicaci&oacute;n:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSubTipoUbicacion" runat="server" Width="350px" CssClass="form-control">
                                                </asp:DropDownList>
                                                <div>

                                                    <asp:RequiredFieldValidator ID="ReqddlSubTipoUbicacion" runat="server" ControlToValidate="ddlSubTipoUbicacion" CssClass="field-validation-error"
                                                        Display="Dynamic" ErrorMessage="Debe seleccionar el sub tipo de ubicación." ValidationGroup="Buscar4"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="form-label">
                                                <asp:Label ID="lbldescUbicacion" runat="server" Text="Local:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescUbicacion" runat="server" MaxLength="25" Width="150px" CssClass="form-control"></asp:TextBox>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="ReqtxtDescUbicacion" runat="server" ControlToValidate="txtDescUbicacion" CssClass="field-validation-error"
                                                        Display="Dynamic" ErrorMessage="Debe ingresar el Nº de local." ValidationGroup="Buscar4"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>


                                </ContentTemplate>

                            </asp:UpdatePanel>

                            <%--Contenedor del botón buscar--%>
                            <asp:UpdatePanel ID="updPanelBuscar4" runat="server" RenderMode="Inline">
                                <ContentTemplate>

                                    <div class="form-inline text-right">

                                        <div class="form-group">
                                            
                                            <asp:UpdateProgress ID="UpdateProgress4" AssociatedUpdatePanelID="updPanelBuscar4"
                                                runat="server" DisplayAfter="200">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>

                                        </div>
                                        <div class="form-group">
                                            
                                            <asp:LinkButton ID="btnbuscar4" runat="server" CssClass="btn btn-default" ValidationGroup="Buscar4" OnClick="btnBuscar4_Click">
                                                <i class="imoon imoon-search"></i>
                                                <span class="text">Buscar</span>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnCerrar4" runat="server" CssClass="btn btn-danger" OnClick="btnCerrar_Click">
                                                <i class="imoon-white imoon-close"></i>
                                                <span class="text">Cerrar</span>
                                            </asp:LinkButton>
                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </asp:Panel>
                    </div>
                </div>

            </asp:Panel>

            <%--Resultados de la búsqueda--%>
            <asp:UpdatePanel ID="pnlResultados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>


                    <asp:Panel ID="pnlGridResultados" runat="server" Visible="false" Style="display: block">


                        <legend>Resultados de la b&uacute;squeda
                            <asp:Label ID="lblCantResultados" runat="server" Style="padding-left: 10px; font-size: small; font-style: italic"></asp:Label>
                        </legend>


                        <fieldset>

                            <asp:GridView ID="gridubicacion" runat="server" AutoGenerateColumns="false" DataKeyNames="id_ubicacion" ItemType="SGI.Model.Ubicacion"
                                OnRowDataBound="gridubicacion_OnRowDataBound" ShowHeader="false" Width="100%"
                                GridLines="None" OnDataBound="gridubicacion_DataBound" AllowPaging="true" Style="margin-top: -10px"
                                PageSize="1" OnPageIndexChanging="gridubicacion_PageIndexChanging">

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
                                                                Número de Partida Matriz: 
                                                               <span class="label-azul"><%# Item.NroPartidaMatriz %></span>
                                                            </div>
                                                            <div>
                                                                Sección:
                                                                    <asp:Label ID="grd_seccion" runat="server" Text="<%# Item.Seccion %>" CssClass="label-azul"></asp:Label>
                                                                Manzana: 
                                                                    <asp:Label ID="grd_manzana" runat="server" Text="<%# Item.Manzana %>" CssClass="label-azul"></asp:Label>
                                                                Parcela:
                                                                    <asp:Label ID="grd_parcela" runat="server" Text="<%# Item.Parcela %>" CssClass="label-azul"></asp:Label>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlTipoUbicacion" runat="server" Style="padding-top: 3px" Visible="false">
                                                            <div>
                                                                Ubicaci&oacute;n:
                                                                    <asp:Label ID="lblTipoUbicacion" runat="server" CssClass="label-azul"></asp:Label>
                                                            </div>
                                                            <div>
                                                                Detalle:
                                                                    <asp:Label ID="lblSubTipoUbicacion" runat="server" CssClass="label-azul"></asp:Label>
                                                            </div>
                                                            <div>
                                                                Local:
                                                                    <asp:Label ID="lblLocal" runat="server" CssClass="label-azul"></asp:Label>
                                                            </div>
                                                        </asp:Panel>

                                                        <div style="margin-top: 10px; min-height: 250px; min-width: 300px">
                                                            <img id="imgCargando" src= '<%: ResolveUrl("~/Content/img/app/Loading128x128.gif") %>' alt="" style="margin-left: 60px; margin-top: 40px" />
                                                            <img id="imgFotoParcela" class="img-polaroid" src="<%# Item.GetUrlFoto(300,250) %>" onload="fotoCargada('imgCargando',this);" onerror="noExisteFotoParcela(this);" style="display: none" />
                                                        </div>
                                                    </td>
                                                    <td style="width: 10px; border-right: solid 1px #eeeeee"></td>
                                                    <td style="width: auto; padding-left: 10px; vertical-align: text-top">

                                                        <div class="alert alert-small alert-warning">
                                                            El resultado de las puertas es referencial al momento de buscar la ubicación
                                                            <%--Si al intentar seleccionar las puertas, la calle no existe en el sistema. Haga click 
                                                            <asp:LinkButton ID="btnNuevaPuerta" runat="server" Text="aquí" OnClick="btnNuevaPuerta_Click" 
                                                                CommandArgument='<%# Item.id_ubicacion %>'></asp:LinkButton>.--%>
                                                        </div>

                                                        <asp:Panel ID="pnlPuertas" runat="server">

                                                            <strong>Puertas</strong>

                                                            <div style="overflow: auto; max-height: 300px">
                                                                
                                                                <asp:UpdatePanel ID="updPuertas" runat="server" OnLoad="updPuertas_Load">
                                                                    <ContentTemplate>

                                                                    <asp:DataList ID="lstPuertas" runat="server" ItemType="SGI.Model.Ubicacion.Puerta"
                                                                        RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%">
                                                                        <AlternatingItemStyle BackColor="#f9f9f9" />
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hid_ubic_puerta" runat="server" Value="<%# Item.id_ubic_puerta %>" />
                                                                            <asp:HiddenField ID="hid_codigo_calle" runat="server" Value="<%# Item.codigo_calle %>" />
                                                                            <asp:HiddenField ID="hid_NroPuerta_ubic" runat="server" Value="<%# Item.NroPuerta_ubic %>" />

                                                                            <div class="form-inline">
                                                                                <div class="control-group">
                                                                                    <label class="checkbox">
                                                                                        <asp:CheckBox ID="chkPuerta" runat="server" Enabled="false" />
                                                                                        <asp:Label ID="lblnombreCalle" runat="server" Text="<%# Item.Nombre_calle %>"></asp:Label>
                                                                                    </label>
                                                                                    <asp:TextBox ID="txtNroPuerta" runat="server" Text="<%# Item.NroPuerta_ubic %>" Width="65px" CssClass="form-control grid-txtNroPuerta" Enabled="false"></asp:TextBox>
                                                                                    <asp:LinkButton ID="lnkAgregarOtraPuerta" runat="server" Text="Agregar otra puerta" Style="display: inline"
                                                                                        data-toggle="tooltip" Enabled="false" Visible="false"
                                                                                        ToolTip="Agrega otra puerta en la misma calle y dentro de la misma cuadra." CssClass="AgregarOtraPuerta"
                                                                                        Font-Size="9pt" OnClick="lnkAgregarOtraPuerta_Click" CommandArgument='<%# Item.id_ubic_puerta  %>'></asp:LinkButton>
                                                                                </div>
                                                                            </div>


                                                                        </ItemTemplate>

                                                                    </asp:DataList>

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                            </div>
                                                        </asp:Panel>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <%--Depto / Local / Otros--%>
                                                    <td colspan="3">
                                                        <asp:Panel ID="pnlDeptoLocal" runat="server" Visible="true" CssClass="pbottom10">
                                                            <%--<table border="0" style="margin-top: 10px; width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <div class="form-inline">
                                                                            <label>Otros:</label>
                                                                            <asp:TextBox ID="txtOtros" runat="server" MaxLength="50" Width="250px" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 190px">
                                                                        <div class="form-inline">
                                                                            <label>Depto:</label>
                                                                            <asp:TextBox ID="txtDepto" runat="server" MaxLength="8" Width="80px" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 190px">
                                                                        <div class="form-inline">
                                                                            <label>Local:</label>
                                                                            <asp:TextBox ID="txtLocal" runat="server" MaxLength="8" Width="80px" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top; width: 250px">
                                                                        <div style="font-size: 8pt; font-weight: bold; color: #9a9a9a">
                                                                            * Indicar los textos completos del sector deseado. <br />
                                                                             Ej: "Oficina 23 y 24", "Sección 18", etc.
                                                                        </div>
                                                                    </td>
                                                                    <td style="vertical-align: top">
                                                                        <div style="font-size: 8pt; font-weight: bold; width: 200px; color: #9a9a9a">
                                                                            * Indicar únicamente el nº o letra del departamento.
                                                                        </div>
                                                                    </td>
                                                                    <td style="vertical-align: top">
                                                                        <div style="font-size: 8pt; font-weight: bold; width: 200px; color: #9a9a9a">
                                                                            * Indicar únicamente el nº o letra del local.
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>--%>
                                                        </asp:Panel>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="3">
                                                        <%--Grilla para seleccionar partida horizontal--%>
                                                        <asp:UpdatePanel ID="updPartidasHorizontales" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlPartidasHorizontales" runat="server" Style="border-bottom: solid 1px #e1e1e1; display: none" CssClass="BuscarUbicacion-pnlPartidasHorizontales">
                                                                    <div class="titulo-1" style="border-bottom: solid 1px #e1e1e1; padding-top: 5px; font-size: medium">
                                                                        Partidas Horizontales o Subdivisiones:
                                                                    </div>
                                                                    <asp:Panel ID="pnlChecksListPHorizontales" runat="server" Style="overflow: auto; max-height: 129px">
                                                                        <asp:CheckBoxList ID="CheckBoxListPHorizontales" runat="server" Width="750px" RepeatDirection="Horizontal"
                                                                            RepeatLayout="Table" RepeatColumns="3" CellPadding="1" Font-Size="9pt" >                                                                        
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>

                                            </table>

                                       


                                            <div class="alert alert-info alert-block" style="margin-top: 10px; display:none">
                                                <strong>Info!</strong>
                                                <ul>
                                                    <li>Si la numeración de la puerta no es correcta, puede modificarla siempre que se encuentre
                                                                        dentro de la cuadra.</li>
                                                    <li>Si la cantidad de puertas en la calle es inferior a las que ud posee, puede utilizar
                                                                        el botón "Agregar otra puerta" y cambiar su numeración por la correcta.</li>
                                                </ul>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerTemplate>
                                    <asp:Panel ID="pnlpager" runat="server" Style="padding: 10px; text-align: center; border-top: solid 1px #e1e1e1">
                                        <asp:Button ID="cmdAnterior" runat="server" Text="<< Anterior" OnClick="cmdAnterior_Click"
                                            CssClass="btn btn-default" Width="100px" />
                                        <asp:Button ID="cmdPage1" runat="server" Text="1 " OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage2" runat="server" Text="2" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage3" runat="server" Text="3" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage4" runat="server" Text="4" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage5" runat="server" Text="5" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage6" runat="server" Text="6" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage7" runat="server" Text="7" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage8" runat="server" Text="8" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage9" runat="server" Text="9" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage10" runat="server" Text="10" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage11" runat="server" Text="11" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage12" runat="server" Text="12" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage13" runat="server" Text="13" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage14" runat="server" Text="14" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage15" runat="server" Text="15" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage16" runat="server" Text="16" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage17" runat="server" Text="17" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage18" runat="server" Text="18" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdPage19" runat="server" Text="19" OnClick="cmdPage" CssClass="btn btn-default"
                                            Width="22px" />
                                        <asp:Button ID="cmdSiguiente" runat="server" Text="Siguiente >>" OnClick="cmdSiguiente_Click"
                                            CssClass="btn btn-default" Width="100px" />
                                    </asp:Panel>
                                </PagerTemplate>
                                <EmptyDataTemplate>
                                    <asp:Panel ID="pnlNotFound" runat="server" Style="padding: 10px; ">
                                        <div class="form-inline">
                                            <div class="controls">
                                                <div class="mtop10">

                                                    <img src='<%: ResolveUrl("~/Content/img/app/NoRecords.png") %>' alt="" />
                                                    <span class="mleft10">No se encontraron datos con los par&aacute;metros de b&uacute;squeda indicados.</span>

                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </EmptyDataTemplate>
                            </asp:GridView>

                            <asp:Panel ID="pnlValidacionIngresoUbicacion" runat="server" CssClass="alert alert-danger" Visible="false">
                                
                                <asp:BulletedList ID="lstValidacionesUbicacion" runat="server">
                                </asp:BulletedList>
                            </asp:Panel>

                        </fieldset>
                        <%--Botones de Nuva Búsqueda y de Ingresar--%>

                        <asp:UpdatePanel ID="updPanelBotones" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlbotonesingreso" runat="server">
                                    
                                    <div class="form-inline text-center mtop5">

                                        <div class="form-group">
                                            <asp:Button ID="btnNuevaBusqueda" runat="server" CssClass="btn btn-default" Text="Nueva b&uacute;squeda"
                                                Width="150px" OnClick="btnNuevaBusquedar_Click" />
                                       
                                            <asp:Button ID="btnIngresarUbicacion" runat="server" CssClass="btn btn-primary" Text="Agregar Ubicaci&oacute;n"
                                                Width="160px" OnClick="btnIngresarUbicacion_Click" OnClientClick="return hidefrmConfirmarNoPH(true);" />
                                       
                                            <asp:UpdateProgress ID="UpdateProgress5" AssociatedUpdatePanelID="updPanelBotones"
                                                runat="server" DisplayAfter="200">
                                                <ProgressTemplate>
                                                    <img src="<%: ResolveUrl("~/Content/img/app/Loading24x24.gif") %>" alt="" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                            <asp:LinkButton ID="btnCerrar5" runat="server" CssClass="btn btn-danger" OnClick="btnCerrar_Click">
                                                    <i class="imoon-white imoon-close"></i>
                                                    <span class="text">Cerrar</span>
                                            </asp:LinkButton>
                                        </div>

                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>



                </ContentTemplate>
            </asp:UpdatePanel>


        </ContentTemplate>

    </asp:UpdatePanel>



    <%--Modal Confirmar Anulación--%>
    <div id="frmConfirmarNoPH" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Partidas horizontales</h4>
                </div>
                <div class="modal-body">
                    <table style="border-collapse: separate; border-spacing: 5px">
                        <tr>
                            <td style="text-align: center; vertical-align: text-top">
                                <label class="imoon imoon-remove-circle fs64 color-blue"></label>
                            </td>
                            <td style="vertical-align: middle">
                                <div>La ubicación ingresada tiene unidades funcionales. <br />
                                    ¿Está seguro que la ubicaci&oacute;n deseada no pertenece a una unidad funcional?.
                                </div>
                                <div class="mtop10">
                                    Si elige NO podrá seleccionar la unidad funcional que corresponda.<br />
                                    Si elige SI se procedrá sin la unidad funcional.
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="modal-footer">

                    <div class="form-inline">
                        <div class="form-group">
                            <button type="button" class="btn btn-default" onclick="return hidefrmConfirmarNoPH(true);">S&iacute;</button>
                            <button type="button" class="btn btn-default" onclick="return hidefrmConfirmarNoPH(false);">No</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->
    
    <%-- Modal Solicitar nueva puerta por mail --%>
    <div id="frmSolicitarNuevaPuerta" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Solicitar Nueva Puerta</h4>
                </div>
                <div class="modal-body">
                    
                    <uc1:SolicitudNuevaPuerta runat="server" id="ucSolicitudNuevaPuerta" />

                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->
    
    <script type="text/javascript">
        var vconfirm = false;

        $(document).ready(function () {

            init_JS_updBuscarUbicacion();
            init_JS_updPuertas();

        });

        function init_JS_updPuertas() {
            
            $(".grid-txtNroPuerta").autoNumeric("init", { aSep: "", mDec: 0, vMax: '99999' });
        }

        function init_JS_updBuscarUbicacion()
        {
            
            $("#<%: txtNroPartida.ClientID %>").autoNumeric("init", { aSep: "", mDec: 0, vMax: '9999999' });
            $("#<%: txtSeccion.ClientID %>").autoNumeric("init", { aSep: "", mDec: 0, vMax: '999' });
            $("#<%: txtNroPuerta.ClientID %>").autoNumeric("init", { aSep: "", mDec: 0, vMax: '99999' });

            $("#tabs").tabs({
                activate: function (event, ui) {
                    
                    var active = $("#tabs").tabs("option", "active");
                    $("#<%: hid_tabselected.ClientID %>").val(active);
                }
            });

            

            
            try
            {

                var tabselected = 0;
                tabselected = $("#<%: hid_tabselected.ClientID %>").val();

                if (tabselected == "") {
                    tabselected = 1;
                }

                $("#tabs").tabs("option", "active",tabselected)
            }
            catch(err){
                //nada
            }

        }

        function noExisteFotoParcela(objimg) {
            $(objimg).attr("src",  '<%: ResolveUrl("~/Content/img/app/ImageNotFound.png") %>');
            fotoCargada();
            return true;
        }

        function fotoCargada(objOcultar, objMostrar) {
            $("#" + objOcultar).css("display", "none");
            $(objMostrar).css("display", "inherit");

            return true;
        }


        function showfrmConfirmarNoPH() {

            vconfirm = false;
            $("#frmConfirmarNoPH").modal({
                "show": true,
                "backdrop": "static",
                "keyboard": false
            });
            
            return false;
        }

        function hidefrmConfirmarNoPH(value) {

            vconfirm = value;
            $("#frmConfirmarNoPH").modal('hide');
            
            if (value) {
                $("#<%: btnIngresarUbicacion.ClientID %>").click();
            }
            return value;
        }


        function validarIngresarUbicacion() {

            var ret = true;
           
            // Si no confirmo se valida, si ya confirmo no se valida.
            if (!vconfirm) {
                $(".BuscarUbicacion-pnlPartidasHorizontales").each(function (index, item) {
                    
                    // Si el panel de partidas horizontales está visible
                    if ($(item).css("display") == "block") {
                        //Si la cantidad de partidas horizontales seleccionadas es (0 cero).
                        if ($(item).find("input:checked").length == 0) {
                            ret = false;
                            showfrmConfirmarNoPH();
                            ret = false;
                        }
                    }
                });
            }
            return ret;
        }

        function showfrmSolicitarNuevaPuerta() {
            $("#frmSolicitarNuevaPuerta").modal("show");
            return true;
        }

        function hidefrmSolicitarNuevaPuerta() {
            $("#frmSolicitarNuevaPuerta").modal("hide");
            return false;
        }

    </script>

</div>


﻿<%@ Title="Men&uacute;" Language="C#"  MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="MenuItems.aspx.cs" Inherits="SGI.MenuItems" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row mtop20">

        <asp:DataList ID="dlItems" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%" CssClass="btn-menuitem ">
            <ItemTemplate>

                <a href='<%# ResolveUrl(Eval("pagina_menu").ToString()) %>'>
                    <ul>
                        <li class="col-sm-2" style="max-width: 65px; margin-right:5px !important">
                            <i class='<%# Eval("iconCssClass_menu") %>'></i>
                        </li>
                        <li class="col-sm-9 mtop10">
                            <strong><%# Eval("descripcion_menu") %></strong>
                            <div><%# Eval("aclaracion_menu") %></div>
                        </li>

                    </ul>

                </a>
            </ItemTemplate>
        </asp:DataList>
    </div>

</asp:Content>
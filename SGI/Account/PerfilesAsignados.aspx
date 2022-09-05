<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PerfilesAsignados.aspx.cs" Inherits="SGI.Account.PerfilesAsignados" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1>Use el formulario siguiente para consultar los perfiles asignados.</h1>
    </hgroup>


    <asp:Panel ID="pnlPerfilesUsuario" runat="server" CssClass="form-horizontal">
            <h3>Datos SADE</h3>
        
            <asp:GridView ID="grdSade" runat="server" AutoGenerateColumns="false" 
        
                CssClass="widget-box table table-bordered table-striped table-hover" GridLines="None" 
                AllowPaging="false"
                ItemType="SGI.Model.clsItemPerfilesFunciones"
                 >
             
                <HeaderStyle CssClass="GridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="usuario_sade" ItemStyle-Width="50%" HeaderText="Usuario SADE" />
                    <asp:BoundField DataField="reparticion_sade" ItemStyle-Width="50%" HeaderText="Repartición SADE"/>

                </Columns>

                <EmptyDataTemplate>
                    No hay Perfiles asignados
                </EmptyDataTemplate>

           </asp:GridView>
           
            <br />
      
            <h3>Perfiles del usuario</h3>
        
            <asp:GridView ID="grdPerfiles" runat="server" AutoGenerateColumns="false" 
        
                CssClass="widget-box table table-bordered table-striped table-hover" GridLines="None" 
                AllowPaging="false"
                ItemType="SGI.Model.clsItemPerfilesFunciones"
                 >
             
                <HeaderStyle CssClass="GridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="nombre_perfil" ItemStyle-Width="50%" HeaderText="Nombre" />
                    <asp:BoundField DataField="descripcion_perfil" ItemStyle-Width="50%" HeaderText="Descripción de Perfiles"/>

                </Columns>

                <EmptyDataTemplate>
                    No hay Perfiles asignados
                </EmptyDataTemplate>

           </asp:GridView>
           
            <br />
        
    
               
        <h3>Roles del usuario</h3>
              
        <asp:GridView ID="grdFunciones" runat="server" AutoGenerateColumns="false"
                CssClass="widget-box  table table-bordered table-striped table-hover" GridLines="None" 
                AllowPaging="false"
                ItemType="SGI.Model.clsItemPerfilesFunciones"
                 >

             <HeaderStyle CssClass="GridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="descripcion_perfil" ItemStyle-Width="20%" HeaderText="Descripción de Perfiles"/>
                    <asp:BoundField DataField="menues_perfil"  ItemStyle-Width="80%" HeaderText="Funciones" HtmlEncode="False" />
                </Columns>

                <EmptyDataTemplate>
                    No hay Perfiles asignados
                </EmptyDataTemplate>
            </asp:GridView>
       
       
            <br />
        
       
<%--            <h3>Tareas del usuario</h3>
            
            <asp:GridView ID="grdTareas" runat="server" AutoGenerateColumns="false" 
        
                CssClass=" widget-box table table-bordered table-striped table-hover" GridLines="None" 
                AllowPaging="false"
                Width="50%">
             
                <HeaderStyle CssClass="GridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="nombre_tarea" HeaderText="Nombre de Tarea"/>
                </Columns>

                <EmptyDataTemplate>
                    No hay Tareas asignadas
                </EmptyDataTemplate>

            </asp:GridView>--%>

    </asp:Panel>


    <asp:Panel ID="pnlResultado" runat="server" Visible="false" Style="margin-top:20px"  >

        <div class="alert alert-info">
            <asp:Label ID="lblMensajeResultado" runat="server" 
                class="titulo-2"
                Text="Los cambios se han realizado exitosamente."></asp:Label>
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" Style="margin-top:20px"  >

        <div class="alert alert-error" >
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <asp:Label ID="lblMensajeError" runat="server" 
                class="titulo-2"
                Text="Ha ocurrido un error al realizr los cambios."></asp:Label>
        </div>


    </asp:Panel>



</asp:Content>


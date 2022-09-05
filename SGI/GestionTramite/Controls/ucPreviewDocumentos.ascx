<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPreviewDocumentos.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucPreviewDocumentos" %>

<style type="text/css">

    .iconlocal
    {
        margin-top:-1px;
    }
    .mLeft
    {
        margin-left:25px;
    }
    
   

</style>
<div class="widget-box">
    <div class="widget-title">
        <span class="icon"><i class="icon-list-alt"></i></span>
        <h5>Previsualizacion de Documentos</h5>
    </div>
    <div class="widget-content">
        <div>
            <div class="clearfix" style="width: 100%;">
                
                <asp:Repeater ID="repeater_certificados" runat="server">
                    <ItemTemplate>

                        <asp:HyperLink ID="lnkCertificado" runat="server" Target="_blank" Style="padding-right: 10px" NavigateUrl='<%# Eval("url") %>'>
                            <i class="icon-file iconlocal"></i>
                            <span class="text"><%# Eval("nombre").ToString() %></span>
                        </asp:HyperLink>

                    </ItemTemplate>

                </asp:Repeater>            
            </div>
        </div>
    </div>
</div>

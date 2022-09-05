<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMenu.ascx.cs" Inherits="SGI.Controls.ucMenu" %>

<asp:Literal ID="lit" runat="server"></asp:Literal>
<asp:HiddenField ID="hid_id_menu_active" runat="server" />


<script type="text/javascript">

    $(document).ready(function () {
        
        var id_menu = $("#<%: hid_id_menu_active.ClientID %>").val();
        if (id_menu.length > 0) {
            $("[data-id-menu='" + id_menu + "']").addClass("active");
        }
    });

    function setmnuActive(element) {
        
        $("#sidebar li").removeClass("active");
        $(element).addClass("active");

        return true;
    }

</script>
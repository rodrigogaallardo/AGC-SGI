<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCabeceraV2.ascx.cs" Inherits="SGI.GestionTramite.Controls.ucCabeceraV2" %>

<div class="widget-box">
	<div class="widget-title">
        <span class="icon"><i class="icon-list-alt"></i></span>
        <h5>Datos del Trámite</h5>
    </div>
	<div class="widget-content">
		<div >
            <div style="width:45%; float:left">

                <ul class="cabecera">
                    <li>
                        Solicitud:<strong><asp:Label ID="lblSolicitud" runat="server"></asp:Label></strong>
                    </li>
                    <li>
                        Estado:<strong><asp:Label ID="lblEstado" runat="server"></asp:Label></strong>
                    </li>                    
                </ul>

            </div>

            <br style="clear: both" />
            <!-- limpia el float anterior -->
        </div>
		
		
	</div>
</div>


<script type="text/javascript">

    $(document).ready(function () {


    });   

   
</script>
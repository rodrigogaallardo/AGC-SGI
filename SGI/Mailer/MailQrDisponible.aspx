<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailQrDisponible.aspx.cs" Inherits="SGI.Mailer.MailQrDisponible" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>AGC - Habilitaciones</title></head>
<body style="margin: 0px; padding: 0px; font-family: 'Segoe UI', Verdana, Helvetica, Sans-Serif;">
							
	<div style="width: 1000px">
		<header>
			<img src="http://www.dghpsh.agcontrol.gob.ar/SSIT/Mailer/img/header.png" style="width: 1000px; height: 148px; max-height: 148px" />
		</header>
		<div style="padding: 20px 20px 20px 50px; min-height: 500px;">
			<h3><span id="lblTitulo" class="color:#333;">Sr. Contribuyente:</span></h3>
			<table cellspacing="0" id="ContentPlaceHolder1_FormView1">
				<tr>
					<td colspan="2">
						<div style="padding: 0px 20px 20px 20px">
							Su solicitud se encuentra ingresada y pendiente de revisi&oacute;n. 
							Se le env&iacute;a adjunto un documento con un c&oacute;digo QR para comenzar con la actividad comercial. 
							Se recuerda que el mismo no implica habilitaci&oacute;n otorgada, y que ser&aacute; notificado del estado de su solicitud. 
						</div>
					</td>
				</tr>
			</table>
		</div>
		<footer>
			<img src="http://www.dghpsh.agcontrol.gob.ar/SSIT/Mailer/img/footer.png" style="width: 1000px; height: 95px; max-height: 95px" />
		</footer>
	</div>
							
</body>
</html>
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.BusinessLogicLayer;
using System.Drawing;
using SGI.Model;
using SGI.DataLayer.Models;
using SGI.BusinessLogicLayer.Constants;
using SGI.GestionTramite.Controls;
using System.Web;
using Newtonsoft.Json;

namespace SGI
{
    public partial class NotificacionesCaducidad : BasePage
    {
        #region CargaInicial
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager sm = ScriptManager.GetCurrent(this);

            //if ( sm.IsInAsyncPostBack )
            //{
            //    ScriptManager.RegisterStartupScript(updPnlFiltroCaducar, updPnlFiltroCaducar.GetType(),"inicializar_controles", "inicializar_controles();", true);
            //}
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion

            if (!IsPostBack)
            {
                List<SSIT_Solicitudes_Notificaciones_motivos> Notificaciones_motivosList = TramitesBLL.TraerNotificaciones_motivos(out string errorMessage);
                ddlNotificaciones_motivos.DataSource = Notificaciones_motivosList;
                ddlNotificaciones_motivos.DataTextField = "NotificacionMotivo";
                ddlNotificaciones_motivos.DataValueField = "IdNotificacionMotivo";
                ddlNotificaciones_motivos.DataBind();
                string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
                txtNroSolicitud.Text = idSolicitudStr;
                btnBuscar_OnClick(sender, e);
            }
            
        }
        #endregion

        #region Entity
        DGHP_Entities db = null;
        private void IniciarEntity()
        {
            if (db == null)
            {
                this.db = new DGHP_Entities();
                this.db.Database.CommandTimeout = 300;
            }
        }
        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }
        #endregion

        #region CaducarTramite

        private void LimpiarCampos()
        {     
            txtNroSolicitud.Text = string.Empty;            
            
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            int solicitud = 0;
            LoadData(solicitud);
            LimpiarCampos();
            
        }


        protected void btnNotificar_OnClick(object sender, EventArgs e)
        {
            bool pudo = int.TryParse(txtNroSolicitud.Text, out int id_solicitud);

            if (string.IsNullOrEmpty(txtFechaNotificacion.Text.Trim()))
            {
                lblError.Text = "No se ha ingresado 'Fecha Notificación.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }


            if (ddlNotificaciones_motivos.SelectedIndex < 0)
            {
                lblError.Text = "No se ha ingresado 'Motivo de Notificación.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (pudo)
            {
                Notificar(id_solicitud, int.Parse(ddlNotificaciones_motivos.SelectedValue), Convert.ToDateTime(txtFechaNotificacion.Text.Trim()));               
                btnBuscar_OnClick(sender, e);
            }
            else
            {
                lblError.Text = "El campo solo acepta valores numéricos.";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }


        public void btnBuscar_OnClick(object sender, EventArgs e)
        {
            
            bool pudo = int.TryParse(txtNroSolicitud.Text, out int idSolicitud);

            if (pudo)
            {
                LoadData(idSolicitud);
            }           
        }

        private void Notificar(int nroSolicitud,int IdNotificacionMotivo,DateTime fechaNotificacion)
        {

            try
            {
                using (DGHP_Entities db = new DGHP_Entities())
                {
                    var solicitudesNotificadas = (from st in db.SSIT_Solicitudes
                                                  join SSN in db.SSIT_Solicitudes_Notificaciones on st.id_solicitud equals SSN.id_solicitud
                                                  where st.id_solicitud == nroSolicitud
                                                  && SSN.Id_NotificacionMotivo == IdNotificacionMotivo
                                                  && SSN.createDate.Year == DateTime.Now.Year
                                                  && SSN.createDate.Month == DateTime.Now.Month
                                                  && SSN.createDate.Day == DateTime.Now.Day
                                                  select new SSIT_Solicitudes_Model
                                                  {
                                                      id_solicitud = nroSolicitud,
                                                      id_estado = st.id_estado
                                                  }).Union(from tr in db.Transf_Solicitudes
                                                           join trn in db.Transf_Solicitudes_Notificaciones on tr.id_solicitud equals trn.id_solicitud
                                                           where tr.id_solicitud == nroSolicitud
                                                           && trn.Id_NotificacionMotivo == IdNotificacionMotivo
                                                           && trn.createDate.Year == DateTime.Now.Year
                                                           && trn.createDate.Month == DateTime.Now.Month
                                                           && trn.createDate.Day == DateTime.Now.Day
                                                           select new SSIT_Solicitudes_Model
                                                           {
                                                               id_solicitud = nroSolicitud,
                                                               id_estado = tr.id_estado
                                                           }).ToList<SSIT_Solicitudes_Model>();
                    if (IdNotificacionMotivo != 13)
                    {
                        if (solicitudesNotificadas.Count != 0)
                        {
                            throw new Exception(ErrorConstants.ERROR_SOLICITUD_NOTIFICADA);
                        }
                    }
                }
                if (IdNotificacionMotivo == 13)
                {                   
                    Response.Redirect("~/Operaciones/ViewLayer/NotificacionGenerica.aspx?id=" + IdNotificacionMotivo + "&nroSolicitud=" + nroSolicitud + "&fechaNotificacion=" + fechaNotificacion.ToShortDateString());
                }
                else if(IdNotificacionMotivo == 14)
                {
                    bool pudo = TramitesBLL.NotificarTramite(nroSolicitud, IdNotificacionMotivo, fechaNotificacion, out string errorMessage, null, null);

                    if (!pudo)
                    {
                        lblError.Text = errorMessage;
                        lblError.ForeColor = Color.Red;
                        this.EjecutarScript(updResultados, "showfrmError();");
                    }
                    else
                    {
                        lblRectificada.Text = "La Baja de su Solicitud ha sido rectificada";
                        lblRectificada.ForeColor = Color.Black;
                        this.EjecutarScript(updResultados3, "showfrmRectificada();");
                    }
                }
                else
                {
                    bool pudo = TramitesBLL.NotificarTramite(nroSolicitud, IdNotificacionMotivo, fechaNotificacion, out string errorMessage, null, null);

                    if (!pudo)
                    {
                        lblError.Text = errorMessage;
                        lblError.ForeColor = Color.Red;
                        this.EjecutarScript(updResultados, "showfrmError();");
                    }
                    else
                    {
                        lblSuccess.Text = errorMessage;
                        lblSuccess.ForeColor = Color.Black;
                        this.EjecutarScript(updResultados, "showfrmSuccess();");
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                LogError.Write(ex, "Error al intentar caducar trámite-btnCaducarNotificaciones_OnClick");
            }
        }

        public void LoadData(int id_solicitud)
        {
            if (id_solicitud == 0)
            {
                updPnlNotificaciones.Visible = false;
                updPnlNotificaciones.Update();
            }
            else
            {
                ///cambiar filtro ahora usar idsolicitud de la tabla SSIT_Solicitudes_Notificacion
                IniciarEntity();
                int id_grupotramite;
                Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);
                if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    var q = (
                                from mail in db.Emails
                                join tipo in db.Emails_Tipos on mail.id_tipo_email equals tipo.id_tipo_email
                                join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                                join ac in db.SSIT_Solicitudes_Notificaciones on mail.id_email equals ac.id_email into pleft_ac
                                from ac in pleft_ac // .DefaultIfEmpty()
                                where ac.id_solicitud == id_solicitud

                                orderby mail.id_email ascending
                                select new clsItemGrillaBuscarMails()
                                {
                                    Mail_ID = mail.id_email.ToString(),
                                    Mail_Estado = edo.descripcion,
                                    Mail_Proceso = tipo.descripcion,
                                    Mail_Asunto = mail.asunto,
                                    Mail_Email = mail.email,
                                    Mail_Fecha = (mail.fecha_envio == null) ? mail.fecha_alta : mail.fecha_envio,
                                    MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT,
                                    id_solicitud = id_solicitud
                                }
                            ).ToList();

                    var qa = (
                                from mail in db.Emails
                                join tipo in db.Emails_Tipos on mail.id_tipo_email equals tipo.id_tipo_email
                                join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                                join ac in db.SSIT_Solicitudes_AvisoCaducidad on mail.id_email equals ac.id_email into pleft_ac
                                from ac in pleft_ac.DefaultIfEmpty()
                                where mail.asunto.Contains(id_solicitud.ToString()) && ac.id_solicitud == id_solicitud ||
                                    mail.html.Contains(id_solicitud.ToString()) && ac.id_solicitud == id_solicitud

                                orderby mail.id_email ascending
                                select new clsItemGrillaBuscarMails()
                                {
                                    Mail_ID = mail.id_email.ToString(),
                                    Mail_Estado = edo.descripcion,
                                    Mail_Proceso = tipo.descripcion,
                                    Mail_Asunto = mail.asunto,
                                    Mail_Email = mail.email,
                                    Mail_Fecha = (mail.fecha_envio == null) ? mail.fecha_alta : mail.fecha_envio,
                                    MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT,
                                    id_solicitud = id_solicitud
                                }
                            ).ToList();

                    var all = q.Union(qa);
                    if (all != null)
                        all = all.OrderBy(x => x.Mail_Fecha);
                    grdBuscarNotis.DataSource = all;
                    grdBuscarNotis.DataBind();
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    var q = (
                                from mail in db.Emails
                                join tipo in db.Emails_Tipos on mail.id_tipo_email equals tipo.id_tipo_email
                                join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                                join ac in db.Transf_Solicitudes_Notificaciones on mail.id_email equals ac.id_email into pleft_ac
                                from ac in pleft_ac.DefaultIfEmpty()
                                where ac.id_solicitud == id_solicitud

                                orderby mail.id_email ascending
                                select new clsItemGrillaBuscarMails()
                                {
                                    Mail_ID = mail.id_email.ToString(),
                                    Mail_Estado = edo.descripcion,
                                    Mail_Proceso = tipo.descripcion,
                                    Mail_Asunto = mail.asunto,
                                    Mail_Email = mail.email,
                                    Mail_Fecha = (mail.fecha_envio == null) ? mail.fecha_alta : mail.fecha_envio,
                                    MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT,
                                    id_solicitud = id_solicitud
                                }
                            ).ToList();

                    var qa = (
                                from mail in db.Emails
                                join tipo in db.Emails_Tipos on mail.id_tipo_email equals tipo.id_tipo_email
                                join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                                join ac in db.Transf_Solicitudes_AvisoCaducidad on mail.id_email equals ac.id_email into pleft_ac
                                from ac in pleft_ac.DefaultIfEmpty()
                                where mail.asunto.Contains(id_solicitud.ToString()) && ac.id_solicitud == id_solicitud ||
                                    mail.html.Contains(id_solicitud.ToString()) && ac.id_solicitud == id_solicitud

                                orderby mail.id_email ascending
                                select new clsItemGrillaBuscarMails()
                                {
                                    Mail_ID = mail.id_email.ToString(),
                                    Mail_Estado = edo.descripcion,
                                    Mail_Proceso = tipo.descripcion,
                                    Mail_Asunto = mail.asunto,
                                    Mail_Email = mail.email,
                                    Mail_Fecha = (mail.fecha_envio == null) ? mail.fecha_alta : mail.fecha_envio,
                                    //MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT
                                    id_solicitud = id_solicitud
                                }
                            ).ToList();
                    var all = q.Union(qa);
                    if (all != null)
                        all = all.OrderBy(x => x.Mail_Fecha);
                    grdBuscarNotis.DataSource = all;
                    grdBuscarNotis.DataBind();
                }
                updPnlNotificaciones.Visible = true;
                updPnlNotificaciones.Update();
            }
        }

        protected void grdBuscarMails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                {
                    /*Llamar el Mensaje Modal*/
                    LinkButton lnkDetalles = (LinkButton)e.Row.FindControl("lnkDetalles");
                    Panel pnlDetalle = (Panel)e.Row.FindControl("pnlDetalle");
                    //lnkDetalles.Attributes.Add("href", "#" + pnlDetalle.ClientID);
                    int idMail = int.Parse(lnkDetalles.CommandArgument);

                    //id_Correo.Value = idMail.ToString();
                    /*Declaro Variables para llenar los campo de la tabla dentro del modal*/
                    TableCell IDCorreo = (TableCell)e.Row.FindControl("IDCorreo");
                    TableCell Email = (TableCell)e.Row.FindControl("Email");
                    TableCell Asunto = (TableCell)e.Row.FindControl("Asunto");
                    TableCell Proceso = (TableCell)e.Row.FindControl("Proceso");
                    TableCell FecAlta = (TableCell)e.Row.FindControl("FecAlta");
                    TableCell FecEnvio = (TableCell)e.Row.FindControl("FecEnvio");
                    TableCell CantInt = (TableCell)e.Row.FindControl("CantInt");
                    TableCell Prioridad = (TableCell)e.Row.FindControl("Prioridad");
                    //TableCell CuerpoHTML = (TableCell)e.Row.FindControl("CuerpoHTML");
                    //Panel CuerpoMsj = (Panel)e.Row.FindControl("CuerpoMsj");

                    System.Web.UI.HtmlControls.HtmlContainerControl Message = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("Message");

                    IniciarEntity();
                    /*Query LinQ*/
                    db = new DGHP_Entities();
                    var q = (
                                from mail in db.Emails
                                join tipo in db.Emails_Tipos on mail.id_tipo_email equals tipo.id_tipo_email
                                join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                                join ac in db.SSIT_Solicitudes_AvisoCaducidad on mail.id_email equals ac.id_email into pleft_ac
                                from ac in pleft_ac.DefaultIfEmpty()
                                where
                                    mail.id_email == idMail

                                orderby mail.id_email ascending
                                select new clsItemGrillaBuscarMails()
                                {
                                    Mail_ID = mail.id_email.ToString(),
                                    Mail_Estado = edo.descripcion,
                                    Mail_Proceso = tipo.descripcion,
                                    Mail_Asunto = mail.asunto,
                                    Mail_Email = mail.email,
                                    Mail_Fecha = (mail.fecha_envio == null) ? mail.fecha_alta : mail.fecha_envio,
                                    Mail_Html = mail.html,
                                    Mail_FechaAlta = mail.fecha_alta,
                                    Mail_FechaEnvio = mail.fecha_envio,
                                    Mail_Intentos = mail.cant_intentos,
                                    Mail_Prioridad = mail.prioridad,
                                }).ToList();
                    /*Para asignar valor a los campos de la tabla voy iterando por cada registro*/

                    foreach (var fila in q)
                    {
                        IDCorreo.Text = fila.Mail_ID;
                        Email.Text = fila.Mail_Email;
                        Asunto.Text = fila.Mail_Asunto;
                        Proceso.Text = fila.Mail_Proceso;
                        FecAlta.Text = fila.Mail_FechaAlta.ToString();
                        FecEnvio.Text = fila.Mail_FechaEnvio.ToString();
                        CantInt.Text = fila.Mail_Intentos.ToString();
                        Prioridad.Text = fila.Mail_Prioridad.ToString();
                        //CuerpoHTML.Text = fila.Mail_Html;
                        Message.Attributes["src"] = "~/Handlers/Mail_Handler.ashx?HtmlID=" + fila.Mail_ID;
                    }
                    FinalizarEntity();
                }
            }
        }



        protected void lnkDetalles_Click(object sender, EventArgs e)
        {
            hfMailID.Value = (sender as LinkButton).CommandArgument;


            string panel = (sender as LinkButton).Parent.FindControl("pnlDetalle").ClientID;
            string script = string.Format("$('#{0}').modal('show');", panel);

            ScriptManager.RegisterStartupScript(updPnlNotificaciones, updPnlNotificaciones.GetType(), "IDMailPanel", script, true);

        }

        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            LinkButton lnkEliminar = (LinkButton)sender;
            int id_solicitud = Convert.ToInt32(lnkEliminar.CommandName);

            int id_grupotramite;
            Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);

            var mailId = (sender as LinkButton).CommandArgument;
            var mailIdInt = Int32.Parse(mailId);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            using (var ftx = new DGHP_Entities())
                            {

                                SSIT_Solicitudes_Notificaciones notificacion = (from n in ftx.SSIT_Solicitudes_Notificaciones
                                                                                where n.id_solicitud == id_solicitud
                                                                                && n.id_email == mailIdInt
                                                                                select n).FirstOrDefault();


                                Emails email = (from mail in ftx.Emails
                                                where mail.id_email == mailIdInt
                                                select mail).FirstOrDefault();

                                ftx.SSIT_Solicitudes_Notificaciones.Remove(notificacion);
                                ftx.Emails.Remove(email);
                                ftx.SaveChanges();
                            }
                            LoadData(id_solicitud);
                            tran.Commit();
                            string script = "$('#frmEliminarLog').modal('show');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                            hid_id_object.Value = mailIdInt.ToString();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            using (var ftx = new DGHP_Entities())
                            {
                                Transf_Solicitudes_Notificaciones notificacion = (from n in ftx.Transf_Solicitudes_Notificaciones
                                                                                  where n.id_solicitud == id_solicitud
                                                                                  && n.id_email == mailIdInt
                                                                                  select n).FirstOrDefault();


                                Emails email = (from mail in ftx.Emails
                                                where mail.id_email == mailIdInt
                                                select mail).FirstOrDefault();

                                ftx.Transf_Solicitudes_Notificaciones.Remove(notificacion);
                                ftx.Emails.Remove(email);
                                ftx.SaveChanges();
                            }
                            LoadData(id_solicitud);
                            tran.Commit();
                            string script = "$('#frmEliminarLog').modal('show');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                            hid_id_object.Value = mailIdInt.ToString();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            DGHP_Entities db = new DGHP_Entities();
            Emails obj = db.Emails.FirstOrDefault(x => x.id_email == int.Parse(hid_id_object.Value));
            db.Dispose();
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4027);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            DGHP_Entities db = new DGHP_Entities();
            Emails obj = db.Emails.FirstOrDefault(x => x.id_email == int.Parse(hid_id_object.Value));
            db.Dispose();
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, string.Empty, "D", 4027);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
        }
        #endregion
    }
}

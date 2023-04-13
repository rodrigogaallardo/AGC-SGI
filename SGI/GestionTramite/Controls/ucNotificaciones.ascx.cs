using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucNotificaciones : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Entity
        DGHP_Entities db = null;
        private void IniciarEntity()
        {
            if (db == null)
            {
                this.db = new DGHP_Entities();
                this.db.Database.CommandTimeout = 120;
            }
        }
        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }
        #endregion

        public void LoadData(int id_solicitud)
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
                                MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT
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
                                MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT
                            }
                        ).ToList();

                var qr = (
                            from mail in db.Emails
                            join tipo in db.Emails_Tipos on mail.id_tipo_email equals tipo.id_tipo_email
                            join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                            join ac in db.SSIT_Solicitudes_AvisoRechazo on mail.id_email equals ac.id_email into pleft_ac
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
                                MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT
                            }
                        ).ToList();

                var all = q.Union(qa).Union(qr);
                if (all != null)
                    all = all.OrderBy(x => x.Mail_Fecha);
                grdBuscarMails.DataSource = all;
                grdBuscarMails.DataBind();
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
                                MailFechaNot_FechaSSIT = ac.fechaNotificacionSSIT
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
                            }
                        ).ToList();
                var all = q.Union(qa);
                if (all != null)
                    all = all.OrderBy(x => x.Mail_Fecha);
                grdBuscarMails.DataSource = all;
                grdBuscarMails.DataBind();
            }
            updPnlNotificaciones.Update();
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
    }
}
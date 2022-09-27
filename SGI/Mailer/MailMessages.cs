using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.Security;
using SGI.Model;
using SGI.WebServices;
using SGI.StaticClassNameSpace;
using System.Collections.Generic;

namespace SGI.Mailer
{
    public class MailMessages
    {
        public static string MailWelcome(Guid userid)
        {

            MembershipUser user = Membership.GetUser(userid);

            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailWelcome.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}", surl, userid));
            WebResponse response = request.GetResponse();

            Encoding enc = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(response.GetResponseStream(), enc);

            string emailHtml = reader.ReadToEnd();

            reader.Dispose();
            response.Dispose();

            return emailHtml;

        }
        public static string MailWelcomeIFCI(Guid userid)
        {

            MembershipUser user = Membership.GetUser(userid);

            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailWelcomeIFCI.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}", surl, userid));
            WebResponse response = request.GetResponse();

            Encoding enc = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(response.GetResponseStream(), enc);

            string emailHtml = reader.ReadToEnd();

            reader.Dispose();
            response.Dispose();

            return emailHtml;

        }
        public static string MailWelcomeECA(Guid userid)
        {

            MembershipUser user = Membership.GetUser(userid);

            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailWelcomeECA.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}", surl, userid));
            WebResponse response = request.GetResponse();

            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader reader = new StreamReader(response.GetResponseStream(), enc);

            string emailHtml = reader.ReadToEnd();

            reader.Dispose();
            response.Dispose();

            return emailHtml;

        }
        public static string htmlMail_RecuperoContraseña(Guid userid)
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailPassRecovery.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}", surl, userid));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMailCaratula(Guid userid, int id_solicitud)
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailCaratula.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}&solicitudId={2}", surl, userid, id_solicitud));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_PagoPendiente(Guid userid, int id_solicitud)
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailPagoPendiente.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}&id_solicitud={2}", surl, userid, id_solicitud));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_CorreccionSolicitud(Guid userid, int id_solicitud)
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailCorreccionSolicitud.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}&id_solicitud={2}", surl, userid, id_solicitud));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_ObservacionSolicitud()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailObservacionSolicitud.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_RechazoSolicitud()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailRechazoSolicitud.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_BajaSolicitud()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailBajaSolicitud.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_Caducidad()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailCaducidad.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }

        public static string htmlMail_LevantaObserva()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailCambioEstado.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_LevantaRechazo()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailLevantamientoRechazo.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_RechazoSolicitudEspar()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailRechazoSolicitudEspar.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_AprobadoSolicitud()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailAprobadoSolicitud.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_DisponibilizarQR()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailQrDisponible.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_Calificar(Guid userid, int id_solicitud)
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailAsignarCalificador.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}&id_solicitud={2}", surl, userid, id_solicitud));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_AprobacionDG(Guid userid, int id_solicitud)
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailAprobacionDG.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}&id_solicitud={2}", surl, userid, id_solicitud));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static string htmlMail_AprobadoSolicitudDG()
        {
            Control ctl = new Control();
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Mailer/MailAprobacionDG.aspx");
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(string.Format("{0}?userid={1}&id_solicitud={2}", surl));
            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            string emailHtml = reader.ReadToEnd();
            reader.Dispose();
            response.Dispose();

            return emailHtml;
        }
        public static void MailSolicitudNuevaPuerta(Guid userid, int id_ubicacion, string Calle, int NroPuerta)
        {

            MembershipUser user = Membership.GetUser(userid);

            Control ctl = new Control();
            string url = string.Format("~/Mailer/MailSolicitudNuevaPuerta.aspx?userid={0}&id_ubicacion={1}", userid, id_ubicacion);
            string surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl(url);
            surl = BasePage.IPtoDomain(surl);

            WebRequest request = WebRequest.Create(surl);
            WebResponse response = request.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(response.GetResponseStream(), enc);

            string emailHtml = reader.ReadToEnd();

            emailHtml = emailHtml.Replace(":Calle:", Calle);
            emailHtml = emailHtml.Replace(":NroPuerta:", NroPuerta.ToString());

            reader.Dispose();
            response.Dispose();
            SendMail("SGI - Solicitud de nueva calle en parcela", emailHtml, user.Email);


            return;
        }
        public static void MailPassRecovery(Guid userid)
        {
            try
            {
                MembershipUser user = Membership.GetUser(userid);
                if (user.Email != null)
                {
                    EmailServicePOST correo = new EmailServicePOST();

                    correo.IdTipoEmail = 14; //Web SGI - Recupero de contraseña
                    correo.Email = user.Email;
                    correo.Prioridad = 1;
                    correo.Asunto = "Recupero de contraseña - SGI";
                    correo.Html = Mailer.MailMessages.htmlMail_RecuperoContraseña(userid);

                    ws_MailsRest wsm = new ws_MailsRest();

                    string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                    string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                    string token = wsm.GetToken(userName, passWord);

                    wsm.SendMail(correo, token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail_EnviarCaratula(int id_solicitud)
        {
            //recupero el usuario creador de la solicitud
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var sol = (from s in db.SSIT_Solicitudes
                           where s.id_solicitud.Equals(id_solicitud)
                           select new
                           {
                               s.CreateUser
                           }).FirstOrDefault();

                if (sol == null)
                {
                    sol = (from s in db.Transf_Solicitudes
                           where s.id_solicitud.Equals(id_solicitud)
                           select new
                           {
                               s.CreateUser
                           }).FirstOrDefault();
                }

                Guid userid = sol.CreateUser;
                MembershipUser user = Membership.GetUser(userid);
                if (user.Email != null)
                {
                    EmailServicePOST correo = new EmailServicePOST();

                    correo.IdTipoEmail = 5; //Web SGI - Aviso de Carátula
                    correo.Email = user.Email;
                    correo.Prioridad = 10;
                    correo.Asunto = id_solicitud.ToString() + " - " + "Envio Caratula - SGI";
                    correo.Html = Mailer.MailMessages.htmlMailCaratula(userid, id_solicitud);

                    ws_MailsRest wsm = new ws_MailsRest();

                    string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                    string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                    string token = wsm.GetToken(userName, passWord);

                    var idmail = wsm.SendMail(correo, token);

                    #region notificacion
                    int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                    CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                    #endregion

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_EnviarCaratula_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                MembershipUser mUser = Membership.GetUser(user.UserId);

                if (mUser.Email != null)
                {
                    emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                    emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                    emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                    string asunto = idSolicitud.ToString() + " - " + "Envio Caratula - SGI";

                    var idEmails = EnviarEmails(TipoEmail.WebSGIAvisoCarátula, 10, asunto, MailMessages.htmlMailCaratula(user.UserId, idSolicitud), emails);

                    foreach (int idEmail in idEmails)
                    {
                        CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Aprobado, db);
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        private static void CrearNotificacion(int id_solicitud, int idmail, int idMotivoNotificacion, DGHP_Entities db)
        {
            if (id_solicitud > Constants.EsSolicitud)
            {

                SSIT_Solicitudes_Notificaciones notif = new SSIT_Solicitudes_Notificaciones
                {
                    id_solicitud = id_solicitud,
                    id_email = idmail,
                    Id_NotificacionMotivo = idMotivoNotificacion,
                    createDate = DateTime.Now
                };
                db.SSIT_Solicitudes_Notificaciones.Add(notif);
            }
            else
            {
                Transf_Solicitudes_Notificaciones notif = new Transf_Solicitudes_Notificaciones
                {
                    id_solicitud = id_solicitud,
                    id_email = idmail,
                    Id_NotificacionMotivo = idMotivoNotificacion,
                    createDate = DateTime.Now
                };
                db.Transf_Solicitudes_Notificaciones.Add(notif);
            }
        }

        //public static void MailCaratula(Guid userid, int id_solicitud)
        //{
        //    //MembershipUser user = Membership.GetUser(userid);
        //    //string emailHtml = htmlMailCaratula(userid, id_solicitud);
        //    //SendMail("Envio Caratula - SGI", emailHtml, user.Email);
        //    //return;
        //}
        public static void SendMail(string Subject, string bodyHtml, string EmailAdress)
        {
            try
            {
                if (EmailAdress != null)
                {
                    EmailServicePOST correo = new EmailServicePOST();

                    correo.IdTipoEmail = 11; //Genérico
                    correo.Email = EmailAdress;
                    correo.Prioridad = 10;
                    correo.Asunto = Subject;
                    correo.Html = bodyHtml;

                    ws_MailsRest wsm = new ws_MailsRest();

                    string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                    string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                    string token = wsm.GetToken(userName, passWord);

                    wsm.SendMail(correo, token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SendMail_AprobadoSolicitud(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }
                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }
                    ).FirstOrDefault();
                }

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                EmailServicePOST correo = new EmailServicePOST();



                correo.IdTipoEmail = 5; //Web SGI - Aviso de Carátula
                correo.Prioridad = 1;
                correo.Asunto = "Tra - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Aprobado) + " - " + direccion;
                correo.Html = MailMessages.htmlMail_AprobadoSolicitud();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);

                var idmail = wsm.SendMail(correo, token);

                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_AprobadoSolicitud_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string direccion = GetDireccion(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Aprobado) + " - " + direccion;

                var idEmails = EnviarEmails(TipoEmail.WebSGIAvisoCarátula, 1, asunto, htmlMail_AprobadoSolicitud(), emails);

                foreach(int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Aprobado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_DisponibilzarQR(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }
                    ).FirstOrDefault();
                }

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 8; //Web SGI - Aprobación del DG
                correo.Prioridad = 1;
                correo.Asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.QRDisponible) + " - " + direccion;

                correo.Html = MailMessages.htmlMail_DisponibilizarQR();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                int idTipoNotificacion = (int)MotivosNotificaciones.QRDisponible;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_DisponibilzarQR_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string direccion = GetDireccionTransf(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.BajaDeSolicitud) + " - " + direccion;

                var idEmails = EnviarEmails(TipoEmail.WebSGIAprobacionDG, 1, asunto, htmlMail_DisponibilizarQR(), emails);

                foreach (int idMail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idMail, (int)MotivosNotificaciones.QRDisponible, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_ObservacionSolicitud1(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var qSol = (from sol in db.SSIT_Solicitudes
                            join usr in db.Usuario on sol.CreateUser equals usr.UserId
                            where sol.id_solicitud == id_solicitud
                            select new
                            {
                                usr.Email,
                                usr.UserId
                            });

                if (qSol == null)
                {
                    qSol = (from sol in db.Transf_Solicitudes
                            join usr in db.Usuario on sol.CreateUser equals usr.UserId
                            where sol.id_solicitud == id_solicitud
                            select new
                            {
                                usr.Email,
                                usr.UserId
                            });
                }

                var querySolicitud = qSol.FirstOrDefault();
                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                var persona_fisica = (from tit in db.Encomienda_Titulares_PersonasFisicas
                                      where tit.id_encomienda == enc.id_encomienda
                                      select new { tit.Email });

                var persona_juridica = (from tit in db.Encomienda_Titulares_PersonasJuridicas
                                        where tit.id_encomienda == enc.id_encomienda
                                        select new { tit.Email });

                var persona_fisica_juridica = (from tit in db.Encomienda_Titulares_PersonasJuridicas_PersonasFisicas
                                               where tit.id_encomienda == enc.id_encomienda
                                               select new { tit.Email });

                var contribuyente = (from sol in qSol
                                     select new { sol.Email });

                var titulares = persona_fisica.Union(persona_fisica_juridica).Union(persona_fisica_juridica).Union(contribuyente).ToList();

                if (titulares != null)
                {
                    ws_MailsRest wsm = new ws_MailsRest();

                    string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                    string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                    string token = wsm.GetToken(userName, passWord);

                    foreach (var titular in titulares)
                    {
                        EmailServicePOST correo = new EmailServicePOST();

                        correo.IdTipoEmail = 8; //Web SGI - Aprobación del DG
                        correo.Prioridad = 1;
                        correo.Asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Observado) + " - " + direccion;
                        correo.Html = MailMessages.htmlMail_CorreccionSolicitud(querySolicitud.UserId, id_solicitud);
                        correo.Email = querySolicitud.Email;

                        var idmail = wsm.SendMail(correo, token);

                        int idTipoNotificacion = (int)MotivosNotificaciones.Observado;
                        CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                        db.SaveChanges();
                    }
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }

        }

        public static void SendMail_ObservacionSolicitud1_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Observado) + " - " + GetDireccion(db, idSolicitud);

                var idEmails = EnviarEmails(TipoEmail.WebSGIAprobacionDG, 1, asunto, htmlMail_CorreccionSolicitud(user.UserId, idSolicitud), emails);

                foreach(int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Observado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {   
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_Calificar(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var qSol = (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                        }
                    );

                if (qSol == null)
                {
                    qSol = (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                        }
                    );
                }

                var querySolicitud = qSol.FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 8; //Web SGI - Aprobación del DG
                correo.Prioridad = 1;
                correo.Asunto = "Solicitud de habilitación N°: " + id_solicitud;
                correo.Html = MailMessages.htmlMail_Calificar(querySolicitud.UserId, id_solicitud);
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_Calificar_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string asunto = "Solicitud de habilitación N°: " + idSolicitud + "Asignado al calificador";

                var idEmails = EnviarEmails(TipoEmail.WebSGIAprobacionDG, 1, asunto, htmlMail_Calificar(user.UserId, idSolicitud), emails);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.AsignadoAlCalificador, db);
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_CorreccionSolicitud(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var querySolicitud = (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                            usr.Apellido,
                            usr.Nombre,
                            usr.TipoPersona,
                            usr.RazonSocial
                        }
                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud = (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                            usr.Apellido,
                            usr.Nombre,
                            usr.TipoPersona,
                            usr.RazonSocial
                        }
                    ).FirstOrDefault();
                }


                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                int id_tarea_correcion_sol = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;

                var queryTramite =
                    (
                        from tt in db.SGI_Tramites_Tareas_HAB
                        where tt.id_solicitud == id_solicitud && tt.SGI_Tramites_Tareas.id_tarea == id_tarea_correcion_sol
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                        }

                    ).FirstOrDefault();

                if (queryTramite == null)
                {
                    id_tarea_correcion_sol = (int)Constants.ENG_Tareas.TRM_Correccion_Solicitud;
                    queryTramite =
                    (
                        from tt in db.SGI_Tramites_Tareas_TRANSF
                        where tt.id_solicitud == id_solicitud && tt.SGI_Tramites_Tareas.id_tarea == id_tarea_correcion_sol
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                        }

                    ).FirstOrDefault();
                }

                if (queryTramite == null)
                    throw new Exception("La solicitud no posee tarea corrección solicitud. id_solicitud: " + id_solicitud);


                if (queryTramite.FechaCierre_tramitetarea.HasValue)
                    throw new Exception("La tarea corrección solicitud ha sido finalizada. id_solicitud: " + id_solicitud + " - id_tramitetarea:  " + queryTramite.id_tramitetarea);

                //aunque no tenga mail se envian los datos igual, luego intenta 3 veces enviarlo
                //y de esa forma se tiene la certeza que ingreso a esta funcion

                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 6; //Web SGI - Corrección de la solicitud
                correo.Prioridad = 10;
                correo.Asunto = id_solicitud.ToString() + " - " + "Corrección del Trámite";
                correo.Html = MailMessages.htmlMail_CorreccionSolicitud(querySolicitud.UserId, id_solicitud);
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                int idTipoNotificacion = (int)MotivosNotificaciones.Observado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                db.SaveChanges();

                db.SaveChanges();

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_CorreccionSolicitud_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                ValidarTramiteTarea(db, idSolicitud, Constants.ENG_Tareas.TRM_Correccion_Solicitud);

                var asunto = idSolicitud.ToString() + " - " + "Corrección del Trámite";

                var idEmails = EnviarEmails(TipoEmail.WebSGICorrecciónSolicitud, 10, asunto, htmlMail_CorreccionSolicitud(user.UserId, idSolicitud), emails);

                foreach(int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Observado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        internal static void SendMail_BajaSolicitud_CPadron(int id_cpadron)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.CPadron_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_cpadron == id_cpadron
                        select new
                        {
                            usr.Email,
                            sol.id_cpadron
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_cpadron);

                string sql = "select dbo.CPadron_Solicitud_DireccionesPartidas(" + querySolicitud.id_cpadron + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 7; //Web SGI - Rechazo
                correo.Prioridad = 1;
                correo.Asunto = id_cpadron.ToString() + " - " + direccion;
                correo.Html = MailMessages.htmlMail_BajaSolicitud();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);

                var idmail = wsm.SendMail(correo, token);

                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        internal static void SendMail_BajaSolicitud_CPadron_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string direccion = GetDireccionTransf(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.BajaDeSolicitud) + " - " + direccion;
                
                var idEmails = EnviarEmails(TipoEmail.WebSGIRechazo, 1, asunto, htmlMail_BajaSolicitud(), emails);

                foreach (int idMail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idMail, (int)MotivosNotificaciones.BajaDeSolicitud, db);
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_ExpedienteGenerado(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                            usr.Apellido,
                            usr.Nombre,
                            usr.TipoPersona,
                            usr.RazonSocial
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                int id_tarea_generar_expediente = (int)Constants.ENG_Tareas.SSP_Generar_Expediente;

                var queryTramite =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud && tt.id_tarea == id_tarea_generar_expediente
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.FechaCierre_tramitetarea
                        }

                    ).FirstOrDefault();

                if (queryTramite == null)
                    throw new Exception("La solicitud no posee tarea generar expediente. id_solicitud: " + id_solicitud);


                if (!queryTramite.FechaCierre_tramitetarea.HasValue)
                    throw new Exception("La tarea generar expediente no ha finalizado. id_solicitud: " + id_solicitud + " - id_tramitetarea:  " + queryTramite.id_tramitetarea);



                //aunque no tenga mail se envian los datos igual, luego intenta 3 veces enviarlo
                //y de esa forma se tiene la certeza que ingreso a esta funcion

                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 5; //Web SGI - Aviso de Carátula
                correo.Prioridad = 10;
                correo.Asunto = id_solicitud.ToString() + " - " + "Envio Carátula - SGI";
                correo.Html = MailMessages.htmlMailCaratula(querySolicitud.UserId, id_solicitud);
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion

                db.SaveChanges();

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_ExpedienteGenerado_v1(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                ValidarTramiteTarea(db, idSolicitud, Constants.ENG_Tareas.SSP_Generar_Expediente);

                string asunto = $"{idSolicitud} - Envio Carátula - SGI";
                string html = MailMessages.htmlMailCaratula(user.UserId, idSolicitud);

                var idEmails = EnviarEmails(TipoEmail.WebSGIAvisoCarátula, 10, asunto, html, emails);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Aprobado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_BoletaGenerada(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                            usr.Apellido,
                            usr.Nombre,
                            usr.TipoPersona,
                            usr.RazonSocial
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                int id_tarea_generar_botela = (int)Constants.ENG_Tareas.SSP_Generacion_Boleta;

                var queryTramite =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud && tt.id_tarea == id_tarea_generar_botela
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.FechaCierre_tramitetarea
                        }

                    ).FirstOrDefault();

                if (queryTramite == null)
                    throw new Exception("La solicitud no posee tarea generar boleta. id_solicitud: " + id_solicitud);


                if (!queryTramite.FechaCierre_tramitetarea.HasValue)
                    throw new Exception("La tarea generar boleta no ha finalizado. id_solicitud: " + id_solicitud + " - id_tramitetarea:  " + queryTramite.id_tramitetarea);



                //aunque no tenga mail se envian los datos igual, luego intenta 3 veces enviarlo
                //y de esa forma se tiene la certeza que ingreso a esta funcion

                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 7; //Web SGI - Pendiente de Pago
                correo.Prioridad = 10;
                correo.Asunto = id_solicitud.ToString() + " - " + "Trámite Pendiente de Pago";
                correo.Html = MailMessages.htmlMail_PagoPendiente(querySolicitud.UserId, id_solicitud);
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion

                db.SaveChanges();

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_BoletaGenerada_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                ValidarTramiteTarea(db, idSolicitud, Constants.ENG_Tareas.SSP_Generacion_Boleta);

                string asunto = idSolicitud.ToString() + " - " + "Trámite Pendiente de Pago";

                var idEmails = EnviarEmails(TipoEmail.WebSGIRechazo, 10, asunto, htmlMail_PagoPendiente(user.UserId, idSolicitud), emails);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Aprobado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_ObservacionSolicitud(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }
                    ).FirstOrDefault();
                }

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 6; //Web SGI - Corrección de la solicitud
                correo.Prioridad = 1;
                correo.Asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Observado) + " - " + direccion;
                correo.Html = MailMessages.htmlMail_ObservacionSolicitud();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                int idTipoNotificacion = (int)MotivosNotificaciones.Observado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);

                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_ObservacionSolicitud_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string direccion = GetDireccion(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Observado) + " - " + direccion;

                var idEmails = EnviarEmails(TipoEmail.WebSGICorrecciónSolicitud, 1, asunto, htmlMail_ObservacionSolicitud(), emails);

                foreach(int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Observado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_RechazoSolicitud(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }
                    ).FirstOrDefault();
                }

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 7; //Web SGI - Rechazo
                correo.Prioridad = 1;
                correo.Asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Rechazado) + " - " + direccion;
                correo.Html = querySolicitud.id_subtipoexpediente != (int)Constants.SubtipoDeExpediente.HabilitacionPrevia ?
                        MailMessages.htmlMail_RechazoSolicitud() : MailMessages.htmlMail_RechazoSolicitudEspar();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);

                var idmail = wsm.SendMail(correo, token);

                int idTipoNotificacion = (int)MotivosNotificaciones.Rechazado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_RechazoSolicitud_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                var sol = db.SSIT_Solicitudes.Where(s => s.id_solicitud == idSolicitud).OrderByDescending(x => x.id_solicitud).FirstOrDefault();
                var trf = db.Transf_Solicitudes.Where(s => s.id_solicitud == idSolicitud).OrderByDescending(x => x.id_solicitud).FirstOrDefault();
                var id_ste = sol != null ? sol.id_subtipoexpediente : trf.id_subtipoexpediente;

                string direccion = GetDireccion(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.Rechazado) + " - " + direccion;
                string html = id_ste != (int)Constants.SubtipoDeExpediente.HabilitacionPrevia ? htmlMail_RechazoSolicitud() : htmlMail_RechazoSolicitudEspar();

                var idEmails = EnviarEmails(TipoEmail.WebSGIRechazo, 1, asunto, html, emails);

                foreach(int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Rechazado, db);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_BajaSolicitud(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }
                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }
                    ).FirstOrDefault();
                }

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                string asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.BajaDeSolicitud) + " - " + direccion;
                if (asunto.Length > 300)
                    asunto = asunto.Substring(1, 300);

                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 18; //Web SGI - Baja
                correo.Prioridad = 1;
                correo.Asunto = asunto;
                correo.Html = MailMessages.htmlMail_BajaSolicitud();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                int idTipoNotificacion = (int)MotivosNotificaciones.BajaDeSolicitud;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_BajaSolicitud_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string direccion = GetDireccion(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.BajaDeSolicitud) + " - " + direccion;
                
                var idEmails = EnviarEmails(TipoEmail.WebSGIBaja, 1, asunto, htmlMail_BajaSolicitud(), emails);

                foreach (int idMail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idMail, (int)MotivosNotificaciones.BajaDeSolicitud, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }


        public static void SendMail_Caducidad(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                {
                    querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }
                    ).FirstOrDefault();
                }

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                if (enc == null)
                {
                    enc = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                }

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 18; //Web SGI - Baja
                correo.Prioridad = 1;
                correo.Asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.avisoCaducidad) + " - " + direccion;
                correo.Html = MailMessages.htmlMail_Caducidad();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                int idTipoNotificacion = (int)MotivosNotificaciones.avisoCaducidad;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static List<string> SendMail_Caducidad_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            List<string> emailsNoDuplicados = new List<string>();

            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.avisoCaducidad) + " - " + GetDireccion(db, idSolicitud);
                emailsNoDuplicados = emailsNoDuplicados.ConvertAll(d => d.ToLower());
                emailsNoDuplicados = emails.Distinct().ToList();
                emailsNoDuplicados.Remove("");

                var idEmails = EnviarEmails(TipoEmail.WebSGIBaja, 1, asunto, htmlMail_Caducidad(), emailsNoDuplicados);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.avisoCaducidad, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }

            return emailsNoDuplicados;
        }
        public static void SendMail_LevantamientoRechazo(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.LevantamientoDeRechazo) + " - " + GetDireccion(db, idSolicitud);

                var html = htmlMail_LevantaRechazo();
                html = html.Replace("{id_solicitud}", idSolicitud.ToString());

                var idEmails = EnviarEmails(TipoEmail.WebSGIBaja, 1, asunto, html, emails);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.LevantamientoDeRechazo, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }
        public static void SendMail_CambioEstadoEnTramite(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_subtipoexpediente
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidas(" + enc.id_encomienda + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 11;
                correo.Prioridad = 1;
                correo.Asunto = "Solicitud de habilitación N°: " + id_solicitud + " - " + direccion;
                correo.Html = MailMessages.htmlMail_LevantaObserva();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion

                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }
        public static void SendMail_BajaSolicitud_Transf(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.Transf_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email,
                            sol.id_cpadron
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                string sql = "select dbo.CPadron_Solicitud_DireccionesPartidas(" + querySolicitud.id_cpadron + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 7; //Web SGI - Rechazo
                correo.Prioridad = 1;
                correo.Asunto = "Sol - " + id_solicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.BajaDeSolicitud) + " - " + direccion; ;
                correo.Html = MailMessages.htmlMail_BajaSolicitud();
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);

                var idmail = wsm.SendMail(correo, token);
                int idTipoNotificacion = (int)MotivosNotificaciones.Rechazado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);

                //el cliente aun no definio el tema de las notificacion para las tr, dudas? pregutarle a roxy!!!
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_BajaSolicitud_Transf_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string direccion = GetDireccionTransf(db, idSolicitud);
                string asunto = "Sol - " + idSolicitud.ToString() + " - " + Enum.GetName(typeof(MotivosNotificaciones), MotivosNotificaciones.BajaDeSolicitud) + " - " + direccion;

                var idEmails = EnviarEmails(TipoEmail.WebSGIRechazo, 1, asunto, htmlMail_BajaSolicitud(), emails);

                foreach (int idMail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idMail, (int)MotivosNotificaciones.BajaDeSolicitud, db);
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_AprobacionDG(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {

                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.UserId,
                            usr.Email,
                            usr.Apellido,
                            usr.Nombre,
                            usr.TipoPersona,
                            usr.RazonSocial
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);

                int id_tarea_aprobar_DG = (int)Constants.ENG_Tareas.SSP_Revision_DGHP;

                var queryTramite =
                    (
                        from tt in db.SGI_Tramites_Tareas_HAB
                        where tt.id_solicitud == id_solicitud && tt.SGI_Tramites_Tareas.id_tarea == id_tarea_aprobar_DG
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                        }

                    ).FirstOrDefault();

                if (queryTramite == null)
                    throw new Exception("La solicitud no posee tarea revisión DGHP. id_solicitud: " + id_solicitud);


                if (!queryTramite.FechaCierre_tramitetarea.HasValue)
                    throw new Exception("La tarea tarea revisión DGHP no ha finalizado. id_solicitud: " + id_solicitud + " - id_tramitetarea:  " + queryTramite.id_tramitetarea);

                //aunque no tenga mail se envian los datos igual, luego intenta 3 veces enviarlo
                //y de esa forma se tiene la certeza que ingreso a esta funcion



                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 8;	//Web SGI - Aprobación del DG 
                correo.Prioridad = 1;
                correo.Asunto = id_solicitud.ToString() + " - " + "Trámite Aprobado";
                correo.Html = MailMessages.htmlMail_AprobacionDG(querySolicitud.UserId, id_solicitud);
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                var idmail = wsm.SendMail(correo, token);

                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion

                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }

        }

        public static void SendMail_AprobacionDG_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                ValidarTramiteTarea(db, idSolicitud, Constants.ENG_Tareas.SSP_Revision_DGHP);

                string asunto = $"{idSolicitud} - Trámite Aprobado";
                string html = MailMessages.htmlMail_AprobacionDG(user.UserId, idSolicitud);

                var idEmails = EnviarEmails(TipoEmail.WebSGIAprobacionDG, 1, asunto, html, emails);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Aprobado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        public static void SendMail_CreacionUsuario(Guid userId, string mail)
        {
            try
            {
                //aunque no tenga mail se envian los datos igual, luego intenta 3 veces enviarlo
                //y de esa forma se tiene la certeza que ingreso a esta funcion

                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 11;	//Web SGI - IFCI - Creación usuario
                correo.Prioridad = 1;
                correo.Asunto = "Bienvenida - SGI";
                correo.Html = MailMessages.MailWelcome(userId);
                correo.Email = mail;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);
                wsm.SendMail(correo, token);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMail_CP_Generar_Expediente(int id_solicitud, string mail)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var querySolicitud =
                    (
                        from sol in db.SSIT_Solicitudes
                        join usr in db.Usuario on sol.CreateUser equals usr.UserId
                        where sol.id_solicitud == id_solicitud
                        select new
                        {
                            usr.Email
                        }

                    ).FirstOrDefault();

                if (querySolicitud == null)
                    throw new Exception("La solicitud no existe. id_solicitud: " + id_solicitud);


                EmailServicePOST correo = new EmailServicePOST();

                correo.IdTipoEmail = 8; //Web SGI - Aprobación del DG 
                correo.Prioridad = 1;
                correo.Asunto = id_solicitud.ToString() + " - " + "Actualización de estado de trámite";
                correo.Html = "Sr. contribuyente le informamos que su trámite " + id_solicitud +
                "ha sido actualizado. El mismo puede consultarse ingresando al sistema";
                correo.Email = querySolicitud.Email;

                ws_MailsRest wsm = new ws_MailsRest();

                string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
                string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
                string token = wsm.GetToken(userName, passWord);

                var idmail = wsm.SendMail(correo, token);
                
                #region notificacion
                int idTipoNotificacion = (int)MotivosNotificaciones.Aprobado;
                CrearNotificacion(id_solicitud, idmail, idTipoNotificacion, db);
                #endregion


                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }
        }

        public static void SendMail_CP_Generar_Expediente_v2(int idSolicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                var user = GetUsuario(db, idSolicitud);
                var emails = new List<string>
                {
                    user.Email
                };

                emails.AddRange(GetEmailPersonaFisica(db, idSolicitud));
                emails.AddRange(GetEmailPersonaJuridica(db, idSolicitud));
                emails.AddRange(GetEmailProfesionales(db, idSolicitud));

                string asunto = idSolicitud.ToString() + " - " + "Actualización de estado de trámite";
                string html = $"Sr. contribuyente le informamos que su trámite {idSolicitud} ha sido actualizado. El mismo puede consultarse ingresando al sistema";

                var idEmails = EnviarEmails(TipoEmail.WebSGIAprobacionDG, 1, asunto, html, emails);

                foreach (int idEmail in idEmails)
                {
                    CrearNotificacion(idSolicitud, idEmail, (int)MotivosNotificaciones.Aprobado, db);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }
        }

        private static IEnumerable<int> EnviarEmails(TipoEmail tipoEmail, int prioridad, string asunto, string html, IEnumerable<string> emails)
        {
            ws_MailsRest wsm = new ws_MailsRest();

            string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
            string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
            string token = wsm.GetToken(userName, passWord);

            var result = new List<int>();

            emails = emails.Where(e => !string.IsNullOrWhiteSpace(e)).Select(e => e.Trim()).Select(e => e.ToLower()).Distinct();
            foreach (string email in emails)
            {
                var correo = new EmailServicePOST
                {
                    IdTipoEmail = (int)tipoEmail,
                    Prioridad = prioridad,
                    Asunto = asunto.Length > 300 ? asunto.Substring(0, 300) : asunto,
                    Html = html,
                    Email = email
                };

                result.Add(wsm.SendMail(correo, token));
            }

            return result;
        }

        private static string GetDireccion(DGHP_Entities db, int idSolicitud)
        {
            var enc_ss =
                (
                  from e in db.Encomienda
                  join ess in db.Encomienda_SSIT_Solicitudes on e.id_encomienda equals ess.id_encomienda
                  where ess.id_solicitud == idSolicitud && e.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                  select new
                  {
                      e.id_encomienda
                  }
                );

            var enc_ts =
                (
                  from e in db.Encomienda
                  join ets in db.Encomienda_Transf_Solicitudes on e.id_encomienda equals ets.id_encomienda
                  where ets.id_solicitud == idSolicitud && e.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                  select new
                  {
                      e.id_encomienda
                  }
                );

            var id_enc = enc_ss.FirstOrDefault()?.id_encomienda ?? enc_ts.FirstOrDefault()?.id_encomienda;

            if (id_enc == null)
            {
                return string.Empty;
            }

            string sql = $"select dbo.Encomienda_Solicitud_DireccionesPartidas({id_enc})";
            return db.Database.SqlQuery<string>(sql).FirstOrDefault();
        }

        private static string GetDireccionTransf(DGHP_Entities db, int idSolicitud)
        {
            var sol =
                (
                  from ets in db.Transf_Solicitudes
                  where ets.id_solicitud == idSolicitud
                  orderby ets.id_solicitud descending
                  select new
                  {
                      ets.id_cpadron
                  }
                ).FirstOrDefault();

            string sql = $"select dbo.Encomienda_Solicitud_DireccionesPartidas({sol.id_cpadron})";
            string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

            return direccion;
        }

        private static Model.Usuario GetUsuario(DGHP_Entities db, int idSolicitud)
        {
            var usr =
                (
                    from u in db.Usuario
                    join ss in db.SSIT_Solicitudes on u.UserId equals ss.CreateUser
                    where ss.id_solicitud == idSolicitud
                    select u
                ).FirstOrDefault();

            if (usr != null)
            {
                return usr;
            }

            usr =
                (
                    from u in db.Usuario
                    join ts in db.Transf_Solicitudes on u.UserId equals ts.CreateUser
                    where ts.id_solicitud == idSolicitud
                    select u
                ).FirstOrDefault();

            if (usr != null)
            {
                return usr;
            }

            usr =
                (
                    from u in db.Usuario
                    join cs in db.CPadron_Solicitudes on u.UserId equals cs.CreateUser
                    where cs.id_cpadron == idSolicitud
                    select u
                ).FirstOrDefault();

            if (usr != null)
            {
                return usr;
            }

            throw new Exception($"La solicitud no existe. id_solicitud: {idSolicitud}");
        }

        private static IEnumerable<string> GetEmailPersonaFisica(DGHP_Entities db, int idSolicitud)
        {
            var pj =
                (
                    from sstpf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                    join ssfpf in db.SSIT_Solicitudes_Firmantes_PersonasFisicas on sstpf.id_solicitud equals  ssfpf.id_solicitud
                    where sstpf.id_solicitud == idSolicitud
                    select new
                    {
                        titular = sstpf.Email,
                        firmante = ssfpf.Email
                    }
                );

            var result = pj.Select(p => p.titular).ToList();
            result.AddRange(pj.Select(p => p.firmante).ToList());
            return result;
        }

        private static IEnumerable<string> GetEmailPersonaJuridica(DGHP_Entities db, int idSolicitud)
        {
            var pj =
                (
                    from sstpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                    join ssfpj in db.SSIT_Solicitudes_Firmantes_PersonasJuridicas on sstpj.id_solicitud equals ssfpj.id_solicitud
                    where sstpj.id_solicitud == idSolicitud
                    select new
                    {
                        titular = sstpj.Email,
                        firmante = ssfpj.Email
                    }
                );

            var result = pj.Select(p => p.titular).ToList();
            result.AddRange(pj.Select(p => p.firmante).ToList());
            return result;
        }

        private static IEnumerable<string> GetEmailProfesionales(DGHP_Entities db, int idSolicitud)
        {
            var p =
                (
                    from prof in db.Profesional
                    join enc in db.Encomienda on prof.Id equals enc.id_profesional
                    join ess in db.Encomienda_SSIT_Solicitudes on enc.id_encomienda equals ess.id_encomienda
                    where ess.id_solicitud == idSolicitud
                    select new
                    {
                        prof.Email
                    }
                );

            var result = p.Select(x => x.Email).ToList();
            return result;
        }

        private static void ValidarTramiteTarea(DGHP_Entities db, int idSolicitud, Constants.ENG_Tareas tarea)
        {
            var queryTramite =
                    (
                        from tt in db.SGI_Tramites_Tareas_HAB
                        where tt.id_solicitud == idSolicitud && tt.SGI_Tramites_Tareas.id_tarea == (int)tarea
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                        }

                    ).FirstOrDefault();

            if (queryTramite == null)
            {
                queryTramite =
                (
                    from tt in db.SGI_Tramites_Tareas_TRANSF
                    where tt.id_solicitud == idSolicitud && tt.SGI_Tramites_Tareas.id_tarea == (int)tarea
                    orderby tt.id_tramitetarea descending
                    select new
                    {
                        tt.id_tramitetarea,
                        tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                    }

                ).FirstOrDefault();
            }

            if (queryTramite == null)
                throw new Exception("La solicitud no posee tarea corrección solicitud. id_solicitud: " + idSolicitud);


            if (queryTramite.FechaCierre_tramitetarea.HasValue)
                throw new Exception("La tarea corrección solicitud ha sido finalizada. id_solicitud: " + idSolicitud + " - id_tramitetarea:  " + queryTramite.id_tramitetarea);
        }
    }
}
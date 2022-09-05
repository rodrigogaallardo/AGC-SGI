using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Mailer
{
    public partial class MailAsignarCalificador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public SGI.Model.MailAsignarCalificador GetData()
        {

            SGI.Model.MailAsignarCalificador pago_pendiente = null;

            if (Request.QueryString["userid"] == null || Request.QueryString["id_solicitud"] == null)
                return pago_pendiente;

            Guid userid = Guid.Parse(Request.QueryString["userid"]);
            int id_solicitud = Convert.ToInt32( Request.QueryString["id_solicitud"] );

            DGHP_Entities db = new DGHP_Entities();

            var queryUsr =
                (
                    from usr in db.Usuario
                    where usr.UserId == userid
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

            string url = System.Configuration.ConfigurationManager.AppSettings["Url.Website.SSIT"];
            
            pago_pendiente = new Model.MailAsignarCalificador();
            if (queryUsr.TipoPersona == 1)
                pago_pendiente.Nombre = HttpUtility.HtmlEncode(queryUsr.RazonSocial);
            else
                pago_pendiente.Nombre = HttpUtility.HtmlEncode(queryUsr.Apellido + " " + queryUsr.Nombre);
            pago_pendiente.UrlLogin = url;
            pago_pendiente.NumeroSolicitud = id_solicitud.ToString();

            pago_pendiente.Renglon1 = "<b>Instrucciones:</b>" +
                HttpUtility.HtmlEncode(" Haga click ") + "<a href='" + url + "'>aqu&iacute;</a> " +
                HttpUtility.HtmlEncode("e ingrese los datos requeridos. Si por algún motivo no pudiera hacer click en el botón, copie la siguiente dirección en su navegador:");


            db.Dispose();

            return pago_pendiente;
        }


    }
}
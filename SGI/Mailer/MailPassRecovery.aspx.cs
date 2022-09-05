using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;

namespace SGI.Mailer
{
    public partial class MailPassRecovery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public IQueryable<SGI.Model.MailPassRecovery> GetData()
        {

            ViewState["Subtitle"] = "Se han recuperado los datos de la cuenta de usuario. ";

            object puserid = Request.QueryString["userid"];
            //object puserid = "388C1CA4-56AA-4422-8149-8B9C23B0B3F5";


            if (puserid != null)
            {
                Guid userid = Guid.Parse(puserid.ToString());
                MembershipUser usuario = Membership.GetUser(userid);
                string pass = usuario.GetPassword();
                string email = usuario.Email;
                string url = "http://" + HttpContext.Current.Request.Url.Authority + ResolveUrl("~/Account/Login");

                string LinkController = "http://" + HttpContext.Current.Request.Url.Authority;//localhost

                string LinkAmbiente = HttpContext.Current.Request.ApplicationPath;// return /test /preprod
                if (!LinkAmbiente.EndsWith("/"))
                    LinkAmbiente += "/";

                //string urlweb = "http://" + HttpContext.Current.Request.Url.Authority;
                
                url = IPtoDomain(url);

                DGHP_Entities db = new DGHP_Entities();
                
                var query = from x in db.aspnet_Users
                            where x.UserId.Equals(userid)
                            select new SGI.Model.MailPassRecovery
                            {
                                Username = x.UserName,
                                Email = email,
                                Password = pass,
                                UrlLogin = url,
                                Renglon1 = "<b>Instrucciones:</b> Haga click en el botón Iniciar Sesion e ingrese los datos requeridos. Si por algún motivo no pudiera hacer click en el botón, copie la siguiente dirección en su navegador:",
                                ApplicationName = x.aspnet_Applications.Description,
                                UrlPage = LinkController + LinkAmbiente ,
                                Nombre = (x.SGI_Profiles.Apellido + ", " + x.SGI_Profiles.Nombres) ?? "Usuario"
                            };

                return query;
            }
            else
            {
                return null;
            }
        }
    }
}
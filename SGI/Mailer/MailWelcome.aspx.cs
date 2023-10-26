using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Web.Security;
namespace SGI.Mailer
{
    public partial class MailWelcome1 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<MailWelcome> GetData()
        {
            ViewState["Subtitle"] = "Se ha creado una cuenta de usuario. ";
            
            
            object puserid = Request.QueryString["userid"];

            if( puserid != null)
           {
                Guid userid = Guid.Parse(puserid.ToString());
                string pass = Membership.GetUser(userid).GetPassword();
                string url = "http://" + HttpContext.Current.Request.Url.Authority + ResolveUrl("~/Account/ActivateUser") + string.Format("?userid={0}", userid);
                string urlweb = "http://" + HttpContext.Current.Request.Url.Authority;


                string LinkController = "http://" + HttpContext.Current.Request.Url.Authority;//localhost

                string LinkAmbiente = HttpContext.Current.Request.ApplicationPath;// return /test /preprod
                if (!LinkAmbiente.EndsWith("/"))
                    LinkAmbiente += "/";


                url = IPtoDomain(url);
                urlweb = IPtoDomain(urlweb);

                    DGHP_Entities db = new DGHP_Entities();
                    var query = from x in  db.aspnet_Membership
                                    join usu in db.aspnet_Users on x.UserId equals usu.UserId
                                where x.UserId.Equals(userid)
                                select new MailWelcome
                              {
                                    Username = usu.UserName,
                                    Email = x.Email,
                                    Password = pass,
                                    Urlactivacion = url,
                                    Renglon1 = "Para poder utilizar dicha cuenta deberá activar el usuario presionando el botón 'Activar usuario'.",
                                    Renglon2 = "Si tiene algún inconveniente con la activación del usuario, copie la siguiente dirección en su navegador:",
                                    ApplicationName = usu.aspnet_Applications.Description,
                                    UrlPage = LinkController + LinkAmbiente,
                                    Nombre = (usu.SGI_Profiles.Apellido +", "+usu.SGI_Profiles.Nombres) ?? "Usuario"
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
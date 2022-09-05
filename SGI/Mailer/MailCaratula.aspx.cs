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
    public partial class MailCaratula : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public IQueryable<SGI.Model.MailCaratula> GetData()
        {

            ViewState["Subtitle"] = "Se han recuperado los datos de la cuenta de usuario. ";

            object puserid = Request.QueryString["userid"];
            object psolicitudId = Request.QueryString["solicitudId"];

            if (puserid != null && psolicitudId !=null)
            {
                Guid userid = Guid.Parse(puserid.ToString());
                string solicitudId= Convert.ToString(psolicitudId);
                MembershipUser usuario = Membership.GetUser(userid);
                string nombre = usuario.UserName;
                string email = usuario.Email;
                string url = "http://" + HttpContext.Current.Request.Url.Authority + ResolveUrl("~/Account/Login");
                url = IPtoDomain(url);
                DGHP_Entities db = new DGHP_Entities();
                var query = from x in db.Usuario
                            where x.UserId.Equals(userid)
                            select new SGI.Model.MailCaratula
                            {
                                Nombre = x.Apellido +" "+ x.Nombre ,
                                UrlLogin = url,
                                NumeroSolicitud= solicitudId
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
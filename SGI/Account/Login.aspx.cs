using System;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using SGI.Model;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SGI.Mailer;

namespace SGI.Account
{
    public partial class Login : Page
    {
       /* protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Functions.EsAmbienteDesa())
                {
                    LoginControl.UserName = "digsis";
                }

            }
        }*/

            protected void Page_Load(object sender, EventArgs e)
            {

            #region ASOSA BORRAR
            string aaa = MailMessages.htmlMail_RectificacionBajaSolicitud();

            Guid newGuid = Guid.Parse("50F3BB3A-8383-4377-9F4B-54579C8C1D07");
            string aaa1 = MailMessages.MailWelcome(newGuid);
            //string bbb2 = MailMessages.htmlMail_PagoPendiente(newGuid,1);
            //string ccc3 = MailMessages.htmlMail_CorreccionSolicitud(newGuid, 1);


            #endregion


            TextBox tbox = this.LoginControl.FindControl("UserName") as TextBox;

                if (tbox != null)
                {
                    ScriptManager.GetCurrent(this.Page).SetFocus(tbox);
                }


            }

  
      

        protected void LoginControl_OnLoginError(object sender, EventArgs e)
        {
            try
            {
                MembershipUser user = Membership.GetUser(LoginControl.UserName);

                if (user != null)
                {
                    if (user.IsLockedOut)
                        LoginControl.FailureText = "Su usuario se encuentra bloqueado. Póngase en contacto con el Administrador del sistema.";

                    if (!user.IsApproved)
                        LoginControl.FailureText = "Su usuario se encuentra Inactivo. Póngase en contacto con el Administrador del sistema.";

                }


            }
            catch (Exception)
            {
                LoginControl.FailureText = "Nombre de usuario o contraseña incorrecta.<br />Por favor, intente nuevamente.";
            }
        }

        

    }

}
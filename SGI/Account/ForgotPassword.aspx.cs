using System;
using System.Web.UI.WebControls;
using System.Web.Security;


namespace SGI.Account
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void PasswordRecovery_SendingMail(object sender, MailMessageEventArgs e)
        {
            try
            {
                MembershipUser usuario = Membership.GetUser(PasswordRecovery.UserName);

                if (usuario != null)
                {

                    SGI.Mailer.MailMessages.MailPassRecovery((Guid)usuario.ProviderUserKey);

                    Label lblEmail = (Label)PasswordRecovery.SuccessTemplateContainer.FindControl("lblEmail");
                    lblEmail.Text = usuario.Email;

                    
                }

                e.Cancel = true;
            }
            catch (Exception ex)
            {
                PasswordRecovery.UserNameFailureText = "No fue posible tener acceso a su información. Inténtelo nuevamente.";
                e.Cancel = true;
            }
        }

        protected void PasswordRecovery1_SendMailError(object sender, SendMailErrorEventArgs e)
        {
            
            try
            {

            }
            catch (Exception)
            {
                
            }
        }


    }
}
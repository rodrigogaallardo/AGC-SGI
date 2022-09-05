using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace SGI.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }


        protected void Page_Load()
        {
            if (!IsPostBack)
            {

                // Presentar mensaje de operación correcta
                var message = Request.QueryString["m"];
                if (message != null)
                {

                    // Seccionar la cadena de consulta desde la acción
                    Form.Action = ResolveUrl("~/Account/Manage.aspx");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Su contraseña ha sido cambiada exitosamente."
                        : message == "SetPwdSuccess" ? "Su contraseña se ha establecido."
                        : message == "RemoveLoginSuccess" ? "El inicio de sesión externo se ha quitado."
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                    changePassword.Visible = String.IsNullOrEmpty(SuccessMessage);
                }
            }

        }

        protected void setPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                TextBox CurrentPassword = (TextBox)changePassword.TemplateControl.FindControl("CurrentPassword");
                TextBox NewPassword = (TextBox)changePassword.TemplateControl.FindControl("NewPassword");

                bool result = Membership.GetUser().ChangePassword(CurrentPassword.Text, NewPassword.Text);

                if (result)
                {
                    Response.Redirect("~/Account/Manage.aspx?m=SetPwdSuccess");
                }
                else
                {
                    ModelState.AddModelError("NewPassword", "No se ha podido cambiar la contraseña.");
                }
            }
        }

    }
}
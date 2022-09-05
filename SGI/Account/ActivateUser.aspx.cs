using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Account
{
    public partial class ActivateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OnPageLoaded();
            }
        }


        //Private members

        private void OnPageLoaded()
        {
            try
            {
                ActivarUsuario();
            }
            catch (Exception ex)
            {
                pnlActivacionOk.Visible = false;
                ErrorLabel.Text = ex.Message;
            }
        }


        private void ActivarUsuario()
        {
            string id = Request["userid"];
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("userid");

            Guid userId = new Guid(id);
            MembershipUser user = Membership.GetUser(userId);

            if (user == null)
                throw new InvalidOperationException("El usuario es inexistente.");

            user.IsApproved = true;

            // Desbloquea el usuario si está lockeado
            if (user.IsLockedOut)
                user.UnlockUser();

            Membership.UpdateUser(user);
            UsuarioLabel.Text = user.UserName;

        }
    }
}
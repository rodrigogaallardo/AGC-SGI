using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //FormsAuthentication.RedirectToLoginPage();

                MembershipUser usu = Membership.GetUser();

                if (usu == null)
                    FormsAuthentication.RedirectToLoginPage();
                else
                {
                    //carga normal del master
                }
            }

        }

    }
}
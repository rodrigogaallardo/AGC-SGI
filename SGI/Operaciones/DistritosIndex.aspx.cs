using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class DistritosIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion
            if (!IsPostBack)
            {
               
            }
        }

      



     
       

        protected void btnGruposDistritos_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/GruposDistritosIndex.aspx");
        }

        protected void btnCatalogoDistritos_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritosIndex.aspx");
        }

        protected void btnCatalogoDistritos_Zonas_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasIndex.aspx");
        }

        protected void btnCatalogoDistritos_Subzonas_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritos_SubzonasIndex.aspx");
        }
    }
}
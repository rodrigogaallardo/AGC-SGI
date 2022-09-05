using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Errores
{
    public partial class error3020 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string pagina = Request.QueryString[0];
                    lblPagina.Text = pagina;
                }
                catch
                {
                    lblPagina.Text = "";
                }
            }

        }
    }
}
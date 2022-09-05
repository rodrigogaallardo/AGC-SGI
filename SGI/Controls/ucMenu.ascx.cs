using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using SGI.Entity;
using SGI.Model;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;

namespace SGI.Controls
{
    public partial class ucMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMenu();
            }
        }

        private void CargarMenu()
        {

            DGHP_Entities db = new DGHP_Entities();

            MembershipUser user = Membership.GetUser();
            StringBuilder html = new StringBuilder();


            if (user != null)
            {
                Guid userid = (Guid) user.ProviderUserKey;
                int[] arrPerfilesUsuario = db.aspnet_Users.FirstOrDefault(x=> x.UserId == userid).SGI_PerfilesUsuarios.ToList().Select(s=>s.id_perfil).ToArray();

                RoleProvider rolprovider = (RoleProvider)Roles.Provider;

                List<Shared.clsMenu> menues = (from m in db.SGI_Menues
                                        where (m.id_menu_padre == 0 || !m.id_menu_padre.HasValue)
                                        && (m.SGI_Perfiles.Any(s=>  arrPerfilesUsuario.Contains(s.id_perfil)) || user.UserName == "digsis")
                                        orderby m.nroOrden
                                        select new Shared.clsMenu
                                        { 
                                            aclaracion_menu = m.aclaracion_menu,
                                            descripcion_menu = m.descripcion_menu,
                                            CreateDate = m.CreateDate,
                                            CreateUser = m.CreateUser,
                                            iconCssClass_menu = m.iconCssClass_menu,
                                            id_menu = m.id_menu,
                                            id_menu_padre = m.id_menu_padre,
                                            nroOrden = m.nroOrden,
                                            pagina_menu = m.pagina_menu + (m.pagina_menu == "~/Menu/Items" ? "/" + SqlFunctions.StringConvert((double)m.id_menu).Trim() : ""),
                                            UpdateDate = m.UpdateDate,
                                            UpdateUser = m.UpdateUser
                                        }).ToList();

                CargarItemsLiteral(menues, html, 0);

            }

            lit.Text = html.ToString();
            db.Dispose();
        }
        private void CargarItemsLiteral(List<Shared.clsMenu> menues, StringBuilder html, int nivel)
        {
            html.AppendLine("<ul>");

            foreach (Shared.clsMenu itemMenu in menues)
            {

                string url = "#";
                if (itemMenu.pagina_menu.Length > 0)
                    url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + ResolveUrl(itemMenu.pagina_menu);


                html.AppendLine(string.Format("                <li onclick='return setmnuActive(this);' data-id-menu='{0}' >",itemMenu.id_menu));

                html.AppendLine(string.Format("                <a href='{0}'>", url));
                html.AppendLine(string.Format("                <i class='{0}'></i>", itemMenu.iconCssClass_menu));
                html.AppendLine(string.Format("                <span class='text'>{0}</span>", itemMenu.descripcion_menu));
                html.AppendLine("</a>");


                html.AppendLine("                </li>");
            }

            html.AppendLine("</ul>");

        }

        public void setearMenuActivo(int id_menu)
        {
            hid_id_menu_active.Value = id_menu.ToString();
        }
    }
    
}
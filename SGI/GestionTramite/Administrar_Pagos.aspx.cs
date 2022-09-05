using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using SGI.Controls;

namespace SGI.GestionTramite
{
    public partial class Administrar_Pagos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CargarMenuItems();
            }
        }


        private List<Menu_Items> list_menu = null;

        private void CargarMenuItems()
        {
            DGHP_Entities db = new DGHP_Entities();
            var q = (
                        from mn in db.SGI_Menues
                        where
                            mn.pagina_menu == "~/GestionTramite/Administrar_Pagos.aspx"
                        select new 
                        {
                            id_Padre = mn.id_menu,
                        }
                    ).ToList();
          

                int id_menu_padre = Convert.ToInt32(q[0].id_Padre);

                // Obtener los perfiles del usuario
                MembershipUser user = Membership.GetUser();
                Guid userid = (Guid)user.ProviderUserKey;
                int[] arrPerfilesUsuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.ToList().Select(s => s.id_perfil).ToArray();


                var menu_padre = db.SGI_Menues.Where(x => x.id_menu == id_menu_padre).FirstOrDefault();

                SiteMaster m = (SiteMaster)this.Page.Master;

                var lstMenues = (from sm in db.SGI_Menues
                                 where sm.id_menu_padre == id_menu_padre
                                 && (sm.SGI_Perfiles.Any(s => arrPerfilesUsuario.Contains(s.id_perfil)) || user.UserName == "digsis")
                                 orderby sm.nroOrden
                                 select new
                                 {
                                     id_menu = sm.id_menu,
                                     descripcion_menu = sm.descripcion_menu,
                                     aclaracion_menu = sm.aclaracion_menu,
                                     pagina_menu = sm.pagina_menu.Equals("~/Menu/Items") ? sm.pagina_menu + "/" + sm.id_menu.ToString() : sm.pagina_menu,
                                     iconCssClass_menu = sm.iconCssClass_menu,
                                     id_menu_padre = sm.id_menu_padre,
                                     nroOrden = sm.nroOrden,
                                     visible = sm.visible
                                 }
                                    ).ToList();


                dlItems.DataSource = lstMenues;
                dlItems.DataBind();

                db.Dispose();

                SiteMaster pmaster = (SiteMaster)this.Page.Master;
                ucMenu mnu = (ucMenu)pmaster.FindControl("mnu");
                mnu.setearMenuActivo(id_menu_padre);

            
        }

        protected void repeater_grupo_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id_menu_agrupacion = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_menu_agrupacion"));

                //var  list_item =
                List<Menu_Items> list_item =
                    (
                        from item in this.list_menu
                        where item.id_menu_agrupacion == id_menu_agrupacion
                        orderby item.nro_orden_menu_item
                        select item
                    ).ToList();

                Repeater menu_item = (Repeater)e.Item.FindControl("repeater_item");
                menu_item.DataSource = list_item;

                menu_item.DataBind();
            }
        }
    }
}
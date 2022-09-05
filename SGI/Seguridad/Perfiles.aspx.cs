using System;
using System.Collections.Generic;
//using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using SGI.Entity;
using SGI.Model;
using System.Data.SqlClient;
using System.Data.Entity.Core.Objects;
using System.Data;


namespace SGI.Seguridad
{
    public partial class Perfiles : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            DGHP_Entities db = new DGHP_Entities();

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updResultados, updResultados.GetType(), "init_Js_updResultados", "init_Js_updResultados();", true);
                ScriptManager.RegisterStartupScript(updDatosPerfil, updDatosPerfil.GetType(), "init_Js_updDatosPerfil", "init_Js_updDatosPerfil();", true);
                ScriptManager.RegisterStartupScript(updMenu, updMenu.GetType(), "init_Js_updMenu", "init_Js_updMenu();", true);

                List<ENG_Circuitos> lstCircuitos = (from cir in db.ENG_Circuitos
                                                    orderby cir.id_circuito
                                                    select cir).ToList();
                int indexCircuito = 0;
                foreach (ENG_Circuitos cir in lstCircuitos)
                {
                    ScriptManager.RegisterStartupScript(updPermisosTareas, updPermisosTareas.GetType(), "init_Js_updPermisosTareas", "init_Js_updPermisosTareas('#tree_tareas" + indexCircuito + "');", true);
                    indexCircuito++;
                }
             
            }

            if (!IsPostBack)
            {
                SiteMaster m = (SiteMaster)this.Page.Master;
                //m.Titulo("Administración de Perfiles");

                if (!Functions.ComprobarPermisosPagina(HttpContext.Current.Request.Url.AbsolutePath))
                    Server.Transfer("~/Errores/Error3002.aspx");
            }

        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {

                this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "finalizarCarga();showfrmError();");
            }

        }
        private void BuscarPerfiles()
        {

            lblCantidadRegistros.Text = "0";
           
            grdResultados.DataSource = GetData();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = grdResultados.Rows.Count.ToString();
            

        }

        private List<SGI_Perfiles> GetData()
        {
            DGHP_Entities db = new DGHP_Entities();
            var q = (from perf in db.SGI_Perfiles
                     select perf);

            if (txtBusNombre.Text.Trim().Length > 0)
                q = q.Where(x => x.nombre_perfil.Contains(txtBusNombre.Text.Trim()));


            if (txtBusDescripcion.Text.Trim().Length > 0)
                q = q.Where(x => x.descripcion_perfil.Contains(txtBusDescripcion.Text.Trim()));

            var lstPerfiles = q.ToList();
            db.Dispose();

            return lstPerfiles;
        }

        public string sortOrder
        {
            get
            {
                if (Session["sortOrder"] == "Descending")
                {
                    Session["sortOrder"] = "Ascending";
                }
                else
                {
                    Session["sortOrder"] = "Descending";
                }

                return Session["sortOrder"].ToString();
            }
            set
            {
                Session["sortOrder"] = value;
            }
        }  

        protected void grdResultados_Sorting(Object sender, GridViewSortEventArgs e)
        {

            var lstPerfiles = GetData();

            if (e.SortExpression == "nombre_perfil")
            {

                if (sortOrder == SortDirection.Ascending.ToString())
                    lstPerfiles = lstPerfiles.OrderByDescending(o => o.nombre_perfil).ToList();
                else
                    lstPerfiles = lstPerfiles.OrderBy(o => o.nombre_perfil).ToList();
            }

            if (e.SortExpression == "descripcion_perfil")
            {
                if (sortOrder == SortDirection.Ascending.ToString())
                    lstPerfiles = lstPerfiles.OrderByDescending(o => o.nombre_perfil).ToList();
                else
                    lstPerfiles = lstPerfiles.OrderBy(o => o.nombre_perfil).ToList();
            }


            grdResultados.DataSource = lstPerfiles;
            grdResultados.DataBind();

        }



     

        private void LimpiarDatos()
        {
            hid_id_perfil.Value = "-1";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            trMenu.Text = "";
            hid_ids_menus.Text = "";
            hid_ids_tareas.Text = "";
            CargarArbolMenu();
            CargarArbolTareas();
        }
        
        private void LimpiarDatosBusqueda()
        {
            txtBusNombre.Text = "";
            txtBusDescripcion.Text = "";
        }

        private void CargarDatos(int id_perfil)
        {
            DGHP_Entities db = new DGHP_Entities();
            
            var perfil = db.SGI_Perfiles.FirstOrDefault(x => x.id_perfil == id_perfil);

            if (perfil != null)
            {
                hid_id_perfil.Value = perfil.id_perfil.ToString();
                txtNombre.Text = perfil.nombre_perfil;
                txtDescripcion.Text = perfil.descripcion_perfil;
            }

            int[] arrayMenu = perfil.SGI_Menues.Select(x => x.id_menu).ToArray();
            hid_ids_menus.Text = string.Join(",", arrayMenu);

            int[] arrayTareas = perfil.ENG_Rel_Perfiles_Tareas.Select(x => x.id_tarea).ToArray();
            hid_ids_tareas.Text = string.Join(",", arrayTareas);

            
            db.Dispose();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                BuscarPerfiles();
                updResultados.Update();
                this.EjecutarScript(UpdatePanel2, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        protected void btnNuevoPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatosPerfil.Update();

                DGHP_Entities db = new DGHP_Entities();
                List<ENG_Circuitos> lstCircuitos = (from cir in db.ENG_Circuitos
                                                    orderby cir.id_circuito
                                                    select cir).ToList();
                this.EjecutarScript(UpdatePanel2, "showDatosPerfil(" + lstCircuitos.Count() + ");");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");

            }
        }

        protected void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditarPerfil = (LinkButton)sender;
                int id_perfil = int.Parse(btnEditarPerfil.CommandArgument);

                LimpiarDatos();
                CargarDatos(id_perfil);
                updDatosPerfil.Update();

                this.EjecutarScript(updResultados, "showDatosPerfil();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }
        }

        protected void btnEliminarPerfil_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            try
            {
                int id_perfil = int.Parse(hid_id_perfil_eliminar.Value);

                if (id_perfil == 0)
                    throw new Exception("No es posible eliminar el perfil Administrador.");

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.SGI_Perfiles_delete(id_perfil);
                        Tran.Complete();

                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw new Exception("No se ha podido eliminar el perfil debido a que el mismo ha sido utilizado.");
                    }

                }
                BuscarPerfiles();
                updResultados.Update();
                this.EjecutarScript(updConfirmarEliminar, "hidefrmConfirmarEliminar();");


            }
            catch (Exception ex)
            {

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updConfirmarEliminar, "hidefrmConfirmarEliminar();showfrmError();");
            }
            finally
            {
                db.Dispose();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                int id_perfil = 0;
                int.TryParse(hid_id_perfil.Value, out id_perfil);
                Guid userid = Functions.GetUserId();

                string Nombre = txtNombre.Text.Trim();
                string Descripcion = txtDescripcion.Text.Trim();



                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {

                        if (id_perfil > -1) // en este caso existe el perfil 0 , es el administrador del sistema por eso -1.
                        {
                            //Modificación

                            db.SGI_Perfiles_update(id_perfil, Nombre, Descripcion, userid);
                        }
                        else
                        {
                            //Alta

                            ObjectParameter param_id_perfil = new ObjectParameter("id_perfil", typeof(int));

                            db.SGI_Perfiles_insert(Nombre, Descripcion, userid, param_id_perfil);

                            id_perfil = Convert.ToInt32(param_id_perfil.Value);

                        }

                        db.SGI_Rel_Perfiles_Menues_delete(id_perfil);

                        if (hid_ids_menus.Text.Length > 0)
                        {
                            string[] ids_menu = hid_ids_menus.Text.Split(",".ToCharArray());
                            foreach (string strid_menu in ids_menu)
                            {
                                int id_menu = Convert.ToInt32(strid_menu);
                                db.SGI_Rel_Perfiles_Menues_insert(id_perfil, id_menu);
                            }
                        }


                        db.ENG_Rel_Perfiles_Tareas_delete(id_perfil);
                        
                        if (hid_ids_tareas.Text.Length > 0)
                        {
                            string[] ids_tareas = hid_ids_tareas.Text.Split(",".ToCharArray());
                            foreach (string strid_tarea in ids_tareas)
                            {
                                int id_tarea = Convert.ToInt32(strid_tarea);
                                db.ENG_Rel_Perfiles_Tareas_insert(id_perfil, id_tarea);
                            }
                        }

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();

                        SqlException sqlex = Functions.GetSqlException(ex);
                        if (sqlex != null)
                        {
                            if (sqlex.Number == (int)Constants.sqlErrNumber.UniqueKey)
                                throw new Exception("Ya existe un perfil con el mismo nombre, por favor ingrese otro");
                        }
                        throw ex;
                    }
                }

                LimpiarDatosBusqueda();
                txtBusNombre.Text = Nombre;
                BuscarPerfiles();
                updpnlBuscar.Update();
                updResultados.Update();

                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");

            }
            finally
            {
                db.Dispose();
            }
        }

        //Menu Raiz
        private void CargarArbolMenu()
        {

            MembershipUser user = Functions.GetUser();
            StringBuilder html = new StringBuilder();


            html.AppendLine("<div id='tree_menu' class='tree'>");
            html.AppendLine("<ul id='tree1' style='list-style:none'>");
            html.AppendLine("   <li class='MenuRaiz' >");
            html.AppendLine("     <span><a href='#' class='rama'><i class='link-local pright5' data-toggle='tooltip'></i></a>Men&uacute;</span>");
            if (user != null)
            {

                List<Shared.clsMenu> menues = GetMenu();

                CargarItemsLiteral(menues, html, 0);

            }

            html.AppendLine("   </li>");
            html.AppendLine("</ul>");
            html.AppendLine("</div>");
            trMenu.Text = html.ToString();
            updMenu.Update();
            updPermisosTareas.Update();
        }


        public List<SGI.Model.Shared.clsMenu> GetMenu()
        {

            DGHP_Entities db = new DGHP_Entities();
            List<Shared.clsMenu> menu = new List<Shared.clsMenu>();

            var queryMenu = from m in db.SGI_Menues
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
                                pagina_menu = m.pagina_menu,
                                UpdateDate = m.UpdateDate,
                                UpdateUser = m.UpdateUser
                            };

            //Ahora armo el menu...
            foreach (Shared.clsMenu item in queryMenu.Where(m => !m.id_menu_padre.HasValue).OrderBy(i => i.nroOrden))
            {
                Shared.clsMenu _menu = new Shared.clsMenu(); //item;

                _menu.aclaracion_menu = item.aclaracion_menu;
                _menu.descripcion_menu = item.descripcion_menu;
                _menu.CreateDate = item.CreateDate;
                _menu.CreateUser = item.CreateUser;
                _menu.iconCssClass_menu = item.iconCssClass_menu;
                _menu.id_menu = item.id_menu;
                _menu.id_menu_padre = item.id_menu_padre;
                _menu.nroOrden = item.nroOrden;
                _menu.pagina_menu = item.pagina_menu;
                _menu.UpdateDate = item.UpdateDate;
                _menu.UpdateUser = item.UpdateUser;
                _menu.Submenues = ObtenerSubMenus(item.id_menu, queryMenu);
                menu.Add(_menu);
            }

            db.Dispose();
            return menu;
        }

        private List<Shared.clsMenu> ObtenerSubMenus(decimal? parentId, IQueryable<Shared.clsMenu> items)
        {
            List<Shared.clsMenu> subMenu = new List<Shared.clsMenu>();

            foreach (Shared.clsMenu subitem in items.Where(sm => sm.id_menu_padre == parentId).OrderBy(o => o.nroOrden))
            {
                Shared.clsMenu _menu = new Shared.clsMenu(); //item;

                _menu.aclaracion_menu = subitem.aclaracion_menu;
                _menu.descripcion_menu = subitem.descripcion_menu;
                _menu.CreateDate = subitem.CreateDate;
                _menu.CreateUser = subitem.CreateUser;
                _menu.iconCssClass_menu = subitem.iconCssClass_menu;
                _menu.id_menu = subitem.id_menu;
                _menu.id_menu_padre = subitem.id_menu_padre;
                _menu.nroOrden = subitem.nroOrden;
                _menu.pagina_menu = subitem.pagina_menu;
                _menu.UpdateDate = subitem.UpdateDate;
                _menu.UpdateUser = subitem.UpdateUser;
                _menu.Submenues = ObtenerSubMenus(subitem.id_menu, items);
                subMenu.Add(_menu);
            }

            return subMenu;
        }
        //Se cargan los menues con sus submenues
        private void CargarItemsLiteral(List<Shared.clsMenu> menues, StringBuilder html, int nivel)
        {
            int chkIndex = 0;

            html.AppendLine("<ul style='list-style:none'>");

            foreach (Shared.clsMenu itemMenu in menues.OrderBy(x => x.nroOrden))
            {
                string strClassIco = "";
                
                if (!string.IsNullOrEmpty(itemMenu.iconCssClass_menu))
                    strClassIco = itemMenu.iconCssClass_menu.Replace("fs48", "").Replace("fs64", "").Replace("fs32", "").Replace("fs24", "").Replace("fs128", "");
                //Menus con submenu (Menus Padre)
                if (itemMenu.Submenues.Count > 0)
                {
                    if (nivel == 0 || nivel == 1)
                    {
                        html.AppendLine(string.Format("<li class='parent_li' data-nivel='{0}' >", nivel));
                        html.AppendLine(string.Format("   <i class='imoon-minus-sign submenu'></i>", strClassIco));
                    }
                    else
                    {
                        html.AppendLine(string.Format("<li class='parent_li' data-nivel='{0}' >", nivel));
                        html.AppendLine(string.Format("   <i class='imoon-plus-sign submenu'></i>", strClassIco));
                    }
                }
                //Menus sin submenu (Menus principales)
                else
                {
                    if (nivel == 0 || nivel == 1)
                    {
                        html.AppendLine(string.Format("<li style='' data-nivel='{1}'>", 0, nivel));
                    }
                    else
                    {
                        html.AppendLine(string.Format("<li style='display: none;' data-nivel='{1}'>", 0, nivel));
                    }
                }

                html.AppendLine("   <span>");

                //Checkbox
                html.AppendLine(string.Format("         <input type='checkbox' id='chk{0}' data-id='{1}' data-parentid='{2}' onclick='tree_chkclick(this);' class='tree-checkbox' />", chkIndex, itemMenu.id_menu, (itemMenu.id_menu_padre.HasValue ? itemMenu.id_menu_padre.Value : 0)));
                //Icono
                html.AppendLine(string.Format("         <i class='mleft5 {0}'></i>", strClassIco));
                //Descripción
                html.AppendLine(string.Format("         <label class='mleft2'>{0}</label>",itemMenu.descripcion_menu));
                    

                html.AppendLine("   </span>");

                if (itemMenu.Submenues.Count > 0)
                    CargarItemsLiteral(itemMenu.Submenues, html, nivel + 1);

                html.AppendLine("</li>");
                chkIndex++;
            }

            html.AppendLine("</ul>");


        }

        //Aqui se arma realmente el menu tareas
        private void CargarArbolTareas()
        {

            DGHP_Entities db = new DGHP_Entities();
            List<ENG_Circuitos> lstCircuitos = (from cir in db.ENG_Circuitos
                                                orderby cir.id_circuito
                                                select cir).ToList();

            StringBuilder html = new StringBuilder();
            int chkIndex = 0;
            int chkIndexCircuito = 0;
            int countCircuitos = lstCircuitos.Count();

            foreach (ENG_Circuitos cir in lstCircuitos)
            {

                var lstTareas = db.ENG_Tareas.Where(x => x.id_circuito == cir.id_circuito && x.visible_en_configuracion ).ToList();

                html.AppendLine("<div id='tree_tareas" + chkIndexCircuito + "' class='tree' >");

                html.AppendLine("<ul id='tree" + chkIndexCircuito + "' style='list-style:none'>");


                if (lstTareas.Count > 0)
                {
                    html.AppendLine("   <li class='parent_li'>");
                    html.AppendLine("   <i class='imoon-plus-sign submenu' onclick='Collapse_MenuTareas(this)'></i>");
                }
                else
                    html.AppendLine("   <li >");

                html.AppendLine("   <span>");
                
                //Icono
                html.AppendLine(string.Format("         <i class='mleft5 imoon-share'></i>"));

                //Descripción
                html.AppendLine(string.Format("         <label class='mleft2' >{0}</label>", cir.nombre_circuito));

              

                // Fin del item circuito

                if (lstTareas.Count > 0)
                {
                    html.AppendLine("<ul style='list-style:none'>");

                    foreach (var tarea in lstTareas)
                    {
                        html.AppendLine(string.Format("<li style='display: none;'>"));

                        html.AppendLine("   <span>");

                        //Checkbox
                        html.AppendLine(string.Format("         <input type='checkbox' id='chk{0}' data-id-tarea='{1}' data-tarea-parentid='{2}' onclick='tree_tareas_chkclick(this,{3});' class='tree-checkbox' />", chkIndex, tarea.id_tarea, cir.id_circuito, countCircuitos));

                        //Icono
                        html.AppendLine(string.Format("         <i class='mleft5 imoon-stack'></i>"));

                        //Descripción
                        html.AppendLine(string.Format("         <label class='mleft2'>{0}</label>", tarea.nombre_tarea));

                        html.AppendLine("   </span>");
                        html.AppendLine("</li>");
                        chkIndex++;
                    }

                    html.AppendLine("</ul>");
                }

                html.AppendLine("   </span>");
                html.AppendLine("</li>");

                html.AppendLine("   </li>");
                html.AppendLine("</ul>");
                html.AppendLine("</div>");

                chkIndexCircuito++;

            }
            
            trTareas.Text = html.ToString();
            updPermisosTareas.Update();
            
        }
    }
}
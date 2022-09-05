using System;
using System.Collections.Generic;
//using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using SGI.Entity;
using SGI.Model;
using System.Data;

namespace SGI.Seguridad
{
    public partial class ABMUsuariosWS : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updResultados, updResultados.GetType(), "init_Js_updResultados", "init_Js_updResultados();", true);
                ScriptManager.RegisterStartupScript(updDatosUsuario, updDatosUsuario.GetType(), "init_Js_updDatosUsuario", "init_Js_updDatosUsuario();", true);

            }

            if (!IsPostBack)
            {
                SiteMaster m = (SiteMaster)this.Page.Master;


                if (!Functions.ComprobarPermisosPagina(HttpContext.Current.Request.Url.AbsolutePath))
                    Server.Transfer("~/Errores/Error3002.aspx");

                CargarComboPerfiles();

            }

        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                CargarComboPerfiles();

                this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "finalizarCarga();showfrmError();");
            }

        }

        private void CargarComboPerfiles()
        {
            DGHP_Entities db = new DGHP_Entities();

            var lstPerfiles = (from rol in db.aspnet_Roles.Where(x => x.ApplicationId == Constants.WebServices.ApplicationId)
                               select new
                               {
                                   RoleId = rol.RoleId,
                                   RoleName = rol.RoleName,
                                   Description = !string.IsNullOrEmpty(rol.Description) ? (rol.RoleName + " - " + rol.Description) : rol.RoleName,
                               }).OrderBy(x => x.RoleName).ToList();


            ddlPerfiles.DataTextField = "Description";
            ddlPerfiles.DataValueField = "RoleName";
            ddlPerfiles.DataSource = lstPerfiles;
            ddlPerfiles.DataBind();

            ddlBusPerfil.DataTextField = "RoleName";
            ddlBusPerfil.DataValueField = "RoleId";
            ddlBusPerfil.DataSource = lstPerfiles;
            ddlBusPerfil.DataBind();
            ddlBusPerfil.Items.Insert(0, new ListItem());

            db.Dispose();
        }

        private void BuscarUsuarios()
        {
            try
            {

                lblCantidadRegistros.Text = "0";


                grdResultados.DataSource = GetData();
                grdResultados.DataBind();

                pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, ex.Message);
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError", "$('#frmError').modal('show');", true);
            }
        }

        private List<Shared.clsGrillaUsuarios> GetData()
        {
            Guid BusRoleId = Guid.Empty;
            Guid.TryParse(ddlBusPerfil.SelectedValue, out BusRoleId);

            string username_logueado = Functions.GetUserName();

            bool? Bloqueado = null;

            if (ddlBloqueado.SelectedItem.Text == "Sí")
                Bloqueado = true;
            if (ddlBloqueado.SelectedItem.Text == "No")
                Bloqueado = false;

            DGHP_Entities db = new DGHP_Entities();
            var q = (from usr in db.aspnet_Users.Where(x => x.ApplicationId == Constants.WebServices.ApplicationId)
                     join mem in db.aspnet_Membership on usr.UserId equals mem.UserId into memdf
                     from mem in memdf.DefaultIfEmpty()
                     select new Shared.clsGrillaUsuarios
                     {
                         UserId = usr.UserId,
                         UserName = usr.UserName,
                         Email = mem.Email,
                         Bloqueado = (mem.IsLockedOut ? true : false),
                         Roles = usr.aspnet_Roles
                     });

            if (txtBusUsername.Text.Trim().Length > 0)
                q = q.Where(x => x.UserName.Contains(txtBusUsername.Text.Trim()));

            if (BusRoleId != Guid.Empty && BusRoleId != null)
                q = q.Where(x => x.Roles.Count(r => r.RoleId == BusRoleId) > 0);

            if (Bloqueado.HasValue)
                q = q.Where(x => x.Bloqueado == Bloqueado.Value);

            var lstResultado = q.ToList();

            foreach (var item in lstResultado)
            {
                item.Perfiles_1Linea = string.Join(", ", item.Roles.Select(r => r.RoleName).ToArray());
            }

            db.Dispose();

            lblCantidadRegistros.Text = lstResultado.Count().ToString();
            return lstResultado;

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

            var lst = GetData();

            if (e.SortExpression == "username")
            {

                if (sortOrder == SortDirection.Ascending.ToString())
                    lst = lst.OrderByDescending(o => o.UserName).ToList();
                else
                    lst = lst.OrderBy(o => o.UserName).ToList();
            }

            if (e.SortExpression == "Bloqueado")
            {
                if (sortOrder == SortDirection.Ascending.ToString())
                    lst = lst.OrderByDescending(o => o.Bloqueado).ToList();
                else
                    lst = lst.OrderBy(o => o.Bloqueado).ToList();
            }

            if (e.SortExpression == "Perfiles_1Linea")
            {
                if (sortOrder == SortDirection.Ascending.ToString())
                    lst = lst.OrderByDescending(o => o.Perfiles_1Linea).ToList();
                else
                    lst = lst.OrderBy(o => o.Perfiles_1Linea).ToList();
            }


            grdResultados.DataSource = lst;
            grdResultados.DataBind();

        }

        private void LimpiarDatos()
        {
            hid_userid.Value = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtUsername.Enabled = true;
            hid_perfiles_selected.Value = "";
            //hid_bloqueado_SiNo_selected.Value = "";
            //ddlBloqueado_SiNo.ClearSelection();
            ddlEditBloqueado.ClearSelection();
            ddlPerfiles.ClearSelection();
            ddlBusPerfil.ClearSelection();
            pnlBloqueado.Style["display"] = "none";
        }

        private void LimpiarDatosBusqueda()
        {
            txtBusUsername.Text = "";
            ddlBloqueado.ClearSelection();
            ddlBusPerfil.ClearSelection();
        }

        private void CargarDatos(Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();

            var usuario = (from usu in db.aspnet_Users.Where(x => x.ApplicationId == Constants.WebServices.ApplicationId)
                           join mem in db.aspnet_Membership on usu.UserId equals mem.UserId into memdf
                           from mem in memdf.DefaultIfEmpty()
                           where usu.UserId == userid
                           select new
                           {
                               usu.UserId,
                               usu.UserName,
                               mem.Email,
                               mem.IsLockedOut,
                               mem.IsApproved,
                               usu.aspnet_Roles
                           }).FirstOrDefault();

            if (usuario != null)
            {
                MembershipProvider member = System.Web.Security.Membership.Providers["WebServices"];
                MembershipUser user = member.GetUser(usuario.UserName, true);

                txtPassword.Text = user.GetPassword().ToString();

                hid_userid.Value = usuario.UserId.ToString();
                txtUsername.Text = usuario.UserName;
                txtEmail.Text = usuario.Email;

                if (usuario.IsLockedOut)
                    ddlEditBloqueado.SelectedIndex = 1;
                else
                    ddlEditBloqueado.SelectedIndex = 0;


                hid_perfiles_selected.Value = string.Join(",", usuario.aspnet_Roles.Select(x => x.RoleName).ToArray());
            }

            db.Dispose();

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                BuscarUsuarios();
                updResultados.Update();
                EjecutarScript(UpdatePanel1, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        protected void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatosUsuario.Update();
                this.EjecutarScript(UpdatePanel1, "showDatosUsuario();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");

            }
        }

        protected void btnEditarUsuario_Click(object sender, EventArgs e)
        {

            try
            {
                LinkButton btnEditarUsuario = (LinkButton)sender;
                Guid userid = Guid.Parse(btnEditarUsuario.CommandArgument);

                LimpiarDatos();
                CargarDatos(userid);
                txtUsername.Enabled = false;
                pnlBloqueado.Style["display"] = "block";

                updDatosUsuario.Update();
                this.EjecutarScript(updResultados, "showDatosUsuario();");

            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }

        }

        protected void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            try
            {
                string username = hid_userid_eliminar.Value;

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        //db.SGI_Usuarios_delete(userid);

                        aspnet_Membership User_Editar = db.aspnet_Membership.Where(x => x.aspnet_Users.UserName == username).FirstOrDefault();

                        User_Editar.IsLockedOut = true;

                        db.SaveChanges();
                        db.Entry(User_Editar).Reload();

                        Tran.Complete();

                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw new Exception("No se ha podido bloquear el usuario debido a que el mismo ha sido utilizado.");
                    }

                }
                BuscarUsuarios();
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
                Guid? userid = null;
                if (hid_userid.Value.Length > 0)
                    userid = Guid.Parse(hid_userid.Value);

                Guid userid_logueado = Functions.GetUserId();

                string username = txtUsername.Text.Trim();

                string password = txtPassword.Text.Trim();

                string Email = txtEmail.Text.Trim();

                string Vacio = string.Empty;

                string[] Roles = !string.IsNullOrEmpty(hid_perfiles_selected.Value) ? (hid_perfiles_selected.Value.Split(',')).ToArray() : new string[] { };

                if (userid.HasValue)
                {
                    //Modificación
                    bool bloqueado = (ddlEditBloqueado.SelectedIndex == 1);
                    //if (bloqueado == "Sí")
                    //    bloqueado = (bool)true;
                    //else
                    //    bloqueado = (bool)false;
                    ModificarUsuario(userid.Value, username, password, Email, bloqueado, Roles);
                    if (bloqueado == true)
                    {
                        //para bloquear el usuario intenta un logueo y cambio de password hasta bloquear la cuenta
                        //MembershipUser UserBlock = Membership.GetUser(username);
                        //for (int i = 0; i <= Membership.MaxInvalidPasswordAttempts; i++)
                        //{
                        //    Membership.ValidateUser(UserBlock.UserName, "Not the right password");
                        //    UserBlock.ChangePassword(UserBlock.UserName, "wrong password");
                        //}
                        //Membership.UpdateUser(UserBlock);
                    }
                    LimpiarDatosBusqueda();
                    BuscarUsuarios();

                    updpnlBuscar.Update();
                    updResultados.Update();
                    updDatosUsuario.Update();

                    this.EjecutarScript(updBotonesGuardar, "GetoutDatos();");
                }
                else
                {
                    //Alta
                    CrearUsuario(username, password, Email, Roles);
                    lblAviso.Text = "Se ha creado el usuario con exito.";
                    this.EjecutarScript(updBotonesGuardar, "showfrmAviso();");
                }


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogError.Write(ex.InnerException);
                    lblError.Text = Functions.GetErrorMessage(ex.InnerException);
                }
                else
                {
                    LogError.Write(ex);
                    lblError.Text = Functions.GetErrorMessage(ex);
                }
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");

            }
            finally
            {
                db.Dispose();
            }
        }
        
        private void CrearUsuario(string username, string password, string email, string[] roles)
        {

            DGHP_Entities db = new DGHP_Entities();


            MembershipProvider member = System.Web.Security.Membership.Providers["WebServices"];
            MembershipCreateStatus status;
            //MembershipUser user = null;
            Guid userid_logueado = Functions.GetUserId();

            //--------------------------------------
            // Alta de usuario
            //--------------------------------------
            Guid userid_NewUser = Guid.NewGuid();
            string passwordold = Membership.GeneratePassword(8, 0);

            MembershipUser user = member.CreateUser(username, passwordold, email, null, null, true, userid_NewUser, out status);
            if(user != null)
                user.ChangePassword(user.ResetPassword(), password);
            try
            {

                if (status == MembershipCreateStatus.Success)
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {

                        // Actualiza los datos del profile y perfiles
                        try
                        {

                            //aspnet_Users User_New = db.aspnet_Users.Where(x => x.UserName == user.UserName).FirstOrDefault();

                            //User_New.ApplicationId = Constants.WebServices.ApplicationId;

                            //db.SaveChanges();
                            //db.Entry(User_New).Reload();

                            //roleProvider.AddUserToRoles(username, roles);

                            Tran.Complete();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();

                            SqlException sqlex = Functions.GetSqlException(ex);
                            if (sqlex != null)
                            {
                                if (sqlex.Number == (int)Constants.sqlErrNumber.UniqueKey)
                                    throw new Exception("Ya existe un profile para este usuario.");
                            }
                            throw ex;
                        }

                    }

                    RoleProvider roleProviderWS = System.Web.Security.Roles.Providers["WebServices"];

                    string[] roles_eliminar = roleProviderWS.GetRolesForUser(username);

                    if (roles_eliminar.Count() > 0)
                        roleProviderWS.RemoveUsersFromRoles(new string[] { username }, roles_eliminar);

                    if (roles.Count() > 0)
                        roleProviderWS.AddUsersToRoles(new string[] { username }, roles);

                }
                else
                {
                    string errMsg = GetErrorMessage(status);
                    throw new Exception("Error al crear el usuario: " + errMsg);
                }
            }
            catch (Exception ex)
            {
                if (status != MembershipCreateStatus.DuplicateUserName)
                {
                    MembershipUser user_delete = Membership.GetUser(username, false);
                    //if (user_delete != null)
                    //    db.SGI_Usuarios_delete((Guid)user_delete.ProviderUserKey);
                }
                throw ex;
            }
            finally
            {
                db.Dispose();
            }


        }


        private void ModificarUsuario(Guid userid_editado, string username, string password, string email, bool bloqueado, string[] roles)
        {

            DGHP_Entities db = new DGHP_Entities();

            MembershipProvider member = System.Web.Security.Membership.Providers["WebServices"];
            MembershipUser user = member.GetUser(userid_editado, true);
            Guid userid_logueado = Functions.GetUserId();

            //--------------------------------------
            // Modificar usuario
            //--------------------------------------

            try
            {
                user.Email = email;
                //user.IsApproved = !bloqueado;
                ///member.UpdateUser(user);
                //Membership.UpdateUser(user);

                if (user.IsLockedOut && !bloqueado)
                    user.UnlockUser();

                user.ChangePassword(user.ResetPassword(), password);

                using (TransactionScope Tran = new TransactionScope())
                {

                    // Actualiza los datos del profile y perfiles
                    try
                    {

                        aspnet_Membership User_Editar = db.aspnet_Membership.Where(x => x.aspnet_Users.UserName == username).FirstOrDefault();

                        User_Editar.Email = email;
                        User_Editar.IsLockedOut = bloqueado;

                        db.SaveChanges();
                        db.Entry(User_Editar).Reload();


                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();

                        SqlException sqlex = Functions.GetSqlException(ex);
                        if (sqlex != null)
                        {
                            if (sqlex.Number == (int)Constants.sqlErrNumber.UniqueKey)
                                throw new Exception("Ya existe un profile para este usuario.");
                        }
                        throw ex;
                    }

                }

                RoleProvider roleProviderWS = System.Web.Security.Roles.Providers["WebServices"];

                string[] roles_eliminar = roleProviderWS.GetRolesForUser(username);

                if (roles_eliminar.Count() > 0)
                    roleProviderWS.RemoveUsersFromRoles(new string[] { username }, roles_eliminar);

                if (roles.Count() > 0)
                    roleProviderWS.AddUsersToRoles(new string[] { username }, roles);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario: " + ex.Message);
            }
            finally
            {
                db.Dispose();
            }


        }


        private string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El nombre de usuario ya existe en la base de datos de la aplicación.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "La dirección de correo electrónico ya existe en la base de datos de la aplicación.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña no tiene el formato correcto.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de correo electrónico no tiene el formato correcto.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta de la contraseña no tiene el formato correcto.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta de la contraseña no tiene el formato correcto.";

                case MembershipCreateStatus.InvalidUserName:
                    return "No se encontró el nombre de usuario en la base de datos.";

                case MembershipCreateStatus.ProviderError:
                    return "El proveedor devolvió un error no descrito por otros valores de la enumeración MembershipCreateStatus.";

                case MembershipCreateStatus.UserRejected:
                    return "El usuario no se ha creado, por un motivo definido por el proveedor.";

                default:
                    return "Se ha producido un error desconocido. Por favor, compruebe los datos e inténtelo de nuevo. Si el problema persiste, póngase en contacto con el administrador del sistema.";
            }
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            Button obj = (Button)sender;
            grdResultados.PageIndex = Convert.ToInt16(obj.Text) - 1;
            BuscarUsuarios();
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex + 1;
            BuscarUsuarios();
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex - 1;
            BuscarUsuarios();
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            GridView grid = grdResultados;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                Button btnAnterior = (Button)fila.Cells[0].FindControl("cmdAnterior");
                Button btnSiguiente = (Button)fila.Cells[0].FindControl("cmdSiguiente");

                if (grid.PageIndex == 0)
                    btnAnterior.Visible = false;
                else
                {
                    btnAnterior.Visible = true;
                    btnAnterior.Width = Unit.Parse("100px");
                    btnAnterior.Height = Unit.Parse("40px");
                }

                if (grid.PageIndex == grid.PageCount - 1)
                    btnSiguiente.Visible = false;
                else
                {
                    btnSiguiente.Visible = true;
                    btnSiguiente.Width = Unit.Parse("100px");
                    btnSiguiente.Height = Unit.Parse("40px");
                }


                // Ocultar todos los botones con Números de Página
                for (int i = 1; i <= 19; i++)
                {
                    Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                    btn.Visible = false;
                }


                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas

                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                            btn.Text = i.ToString();
                            btn.Visible = true;
                            if (i + 1 < 100)     // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }
                    }
                }
                else
                {
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                    // o bien los que sea posible en caso de no llegar a 9

                    int CantBucles = 0;

                    Button btnPage10 = (Button)fila.Cells[0].FindControl("cmdPage10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                    // Ubica los 9 botones hacia la izquierda
                    // Linea Original "Previa al cambio": for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 5; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                            if (i + 1 < 100)             // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }

                    }

                    CantBucles = 0;
                    // Ubica los 9 botones hacia la derecha
                    // Linea Original "Previa al cambio": for (int i = grid.PageIndex - 1; i <= grid.PageIndex - 9; i--)
                    for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 5; i++)
                    {
                        CantBucles++;
                        if (i <= grid.PageCount - 1)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                            if (i + 1 < 100)     // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }
                    }



                }
                Button cmdPage;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPage" + i.ToString();
                    cmdPage = (Button)fila.Cells[0].FindControl(btnPage);
                    if (cmdPage != null && cmdPage.Visible)
                    {
                        cmdPage.Width = Unit.Parse("40px");
                        cmdPage.Height = Unit.Parse("40px");
                        cmdPage.CssClass = "btn btn-xs btn-default";
                    }
                }



                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                {
                    if (ctl is Button)
                    {
                        Button btn = (Button)ctl;
                        if (btn.Text.Equals(btnText))
                        {
                            btn.CssClass = "btn btn-info";
                        }
                    }
                }

            }
        }

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultados.PageIndex = e.NewPageIndex;
            BuscarUsuarios();
        }

        protected void btnUserUnlock_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnunlock = (LinkButton)sender;
            try
            {
                string username = btnunlock.CommandArgument;

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        //db.SGI_Usuarios_delete(userid);

                        aspnet_Membership User_Editar = db.aspnet_Membership.Where(x => x.aspnet_Users.UserName == username).FirstOrDefault();

                        User_Editar.IsLockedOut = false;

                        db.SaveChanges();
                        db.Entry(User_Editar).Reload();

                        Tran.Complete();

                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw new Exception("No se ha podido desbloquear el usuario debido a que el mismo ha sido utilizado.");
                    }

                }
                BuscarUsuarios();
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
    }
}
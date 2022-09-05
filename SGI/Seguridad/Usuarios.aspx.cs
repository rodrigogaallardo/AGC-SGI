using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using SGI.GestionTramite.Controls.ExportacionExcel;
using System.Threading;
using SGI.StaticClassNameSpace;


namespace SGI.Seguridad
{
    public partial class Usuarios : BasePage
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
                
                
                if (!Functions.ComprobarPermisosPagina( HttpContext.Current.Request.Url.AbsolutePath))
                    Server.Transfer("~/Errores/Error3002.aspx");

                CargarComboPerfiles();
                
            }

        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                CargarComboPerfiles();
                //CargarComboBloqueado();
                
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

            bool isAdmin = false;
            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                Guid userId = user != null ? (Guid)user.ProviderUserKey : new Guid();
                isAdmin = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.Where(p => p.id_perfil == 50).Any();
            }

            List<SGI_Perfiles> lstPerfiles = isAdmin ?
                lstPerfiles = db.SGI_Perfiles.OrderBy(o => o.nombre_perfil).ToList() :
                lstPerfiles = db.SGI_Perfiles.Where(p => p.id_perfil != 50).OrderBy(o => o.nombre_perfil).ToList();
            
            SGI_Perfiles perfil_todos = new SGI_Perfiles();
            perfil_todos.nombre_perfil = "Todos";
            perfil_todos.id_perfil = 0;

            ddlPerfiles.DataTextField = "nombre_perfil";
            ddlPerfiles.DataValueField = "id_perfil";
            ddlPerfiles.DataSource = lstPerfiles;
            ddlPerfiles.DataBind();

            lstPerfiles.Insert(0, perfil_todos);

            ddlBusPerfil.DataTextField = "nombre_perfil";
            ddlBusPerfil.DataValueField = "id_perfil";
            ddlBusPerfil.DataSource = lstPerfiles;
            ddlBusPerfil.DataBind();

            db.Dispose();
        }

        private void BuscarUsuarios()
        {
            lblCantidadRegistros.Text = "0";

            grdResultados.DataSource = GetData();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            btnExportar.Visible = (grdResultados.Rows.Count > 0);
        }

        private string _url_servicio_EE;
        private string url_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_url_servicio_EE))
                {
                    _url_servicio_EE = Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                }
                return _url_servicio_EE;
            }
        }

        private List<Shared.clsGrillaUsuarios> GetData()
        {
            int id_perfil = 0;
            int.TryParse(ddlBusPerfil.SelectedValue, out id_perfil);

            string username_logueado = Functions.GetUserName();

            bool? Bloqueado = null;

            if (ddlBloqueado.SelectedItem.Text == "Sí")
                Bloqueado = true;
            if (ddlBloqueado.SelectedItem.Text == "No")
                Bloqueado = false;

            DGHP_Entities db = new DGHP_Entities();
            var q = (from mem in db.aspnet_Membership
                     join usu in db.aspnet_Users on mem.UserId equals usu.UserId
                     join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                     where mem.ApplicationId == Constants.ApplicationId
                     select new Shared.clsGrillaUsuarios
                     {
                         UserId = usu.UserId,
                         UserName = usu.UserName,
                         Apellido = profile.Apellido,
                         Nombres = profile.Nombres,
                         UserName_SADE = profile.UserName_SADE,
                         Reparticion_SADE = profile.Reparticion_SADE,
                         Sector_SADE = profile.Sector_SADE,            
                         Email = mem.Email,
                         Bloqueado = (mem.IsLockedOut  ? true : false),
                         Perfiles = usu.SGI_PerfilesUsuarios
                     });

            if (txtBusApellido.Text.Trim().Length > 0)
                q = q.Where(x => x.Apellido.Contains(txtBusApellido.Text.Trim()));

            if (txtBusNombre.Text.Trim().Length > 0)
                q = q.Where(x => x.Nombres.Contains(txtBusNombre.Text.Trim()));

            if (txtBusUsername.Text.Trim().Length > 0)
                q = q.Where(x => x.UserName.Contains(txtBusUsername.Text.Trim()));

            if (id_perfil > 0)
                q = q.Where(x => x.Perfiles.Count(r => r.id_perfil == id_perfil) > 0);

            //if (ddlBloqueado.SelectedItem.Text == "Sí")
            //    q = q.Where(x => x.Bloqueado);
            //else if (ddlBloqueado.SelectedItem.Text == "No")
            //    q = q.Where(x => !x.Bloqueado);

            if (Bloqueado.HasValue)
                q = q.Where(x => x.Bloqueado == Bloqueado.Value);

            var lstResultado = q.ToList();

            foreach (var item in lstResultado)
            {
                item.Perfiles_1Linea = string.Join(", ", item.Perfiles.Select(r => r.nombre_perfil).ToArray());
            }

            // si el usuario logueado no es digsis, no incluye este usuario en la búsqueda
            //if (username_logueado != "digsis")
            //{
            //    q = q.Where(x => x.UserName != "digsis");
            //}

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

            if (e.SortExpression == "apellido")
            {
                if (sortOrder == SortDirection.Ascending.ToString())
                    lst = lst.OrderByDescending(o => o.Apellido).ToList();
                else
                    lst = lst.OrderBy(o => o.Apellido).ToList();
            }

            if (e.SortExpression == "nombres")
            {
                if (sortOrder == SortDirection.Ascending.ToString())
                    lst = lst.OrderByDescending(o => o.Nombres).ToList();
                else
                    lst = lst.OrderBy(o => o.Nombres).ToList();
            }

            if (e.SortExpression == "username_SADE")
            {
                if (sortOrder == SortDirection.Ascending.ToString())
                    lst = lst.OrderByDescending(o => o.UserName_SADE).ToList();
                else
                    lst = lst.OrderBy(o => o.UserName_SADE).ToList();
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
            txtApellido.Text = "";
            txtNombre.Text = "";
            txtEmail.Text = "";
            txtUsernameSADE.Text = "";
            txtReparticionSADE.Text = "";
            txtSectorSADE.Text =  "";
            txtUsername.Enabled = true;
            hid_perfiles_selected.Value = "";
            txtCuit.Text = "";
            //hid_bloqueado_SiNo_selected.Value = "";
            //ddlBloqueado_SiNo.ClearSelection();
            ddlEditBloqueado.ClearSelection();
            ddlPerfiles.ClearSelection();
            ddlBusPerfil.ClearSelection();
            pnlBloqueado.Style["display"] = "none";
        }
        
        private void LimpiarDatosBusqueda()
        {
            txtBusApellido.Text = "";
            txtBusNombre.Text = "";
            txtBusUsername.Text = "";
            ddlBloqueado.ClearSelection();
            ddlBusPerfil.ClearSelection();
        }

        private void CargarDatos(Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();

            var usuario = (
                          from mem in db.aspnet_Membership 
                          join usu in db.aspnet_Users on mem.UserId equals usu.UserId
                          join profile in db.SGI_Profiles on usu.UserId equals profile.userid 
                          where usu.UserId == userid
                          select new
                          {
                              usu.UserId,
                              usu.UserName,
                              profile.Apellido,
                              profile.Nombres,
                              profile.UserName_SADE,
                              profile.Reparticion_SADE,
                              profile.Sector_SADE,
                              profile.Cuit,
                              mem.Email,
                              mem.IsLockedOut,
                              mem.IsApproved,
                              usu.SGI_PerfilesUsuarios
                          }).FirstOrDefault();

            if (usuario != null)
            {
                hid_userid.Value = usuario.UserId.ToString();
                txtUsername.Text = usuario.UserName;
                txtApellido.Text = usuario.Apellido;
                txtNombre.Text = usuario.Nombres;
                txtEmail.Text = usuario.Email;
                txtUsernameSADE.Text = usuario.UserName_SADE;
                txtReparticionSADE.Text = usuario.Reparticion_SADE;
                txtSectorSADE.Text = usuario.Sector_SADE;
                txtCuit.Text = usuario.Cuit;

                txtUsernameSADE.Enabled = txtReparticionSADE.Enabled = txtSectorSADE.Enabled = false;

                if (usuario.IsLockedOut)
                    ddlEditBloqueado.SelectedIndex = 1;
                else
                    ddlEditBloqueado.SelectedIndex = 2;


                hid_perfiles_selected.Value=  string.Join(",", usuario.SGI_PerfilesUsuarios.Select(x => x.id_perfil).ToArray());
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
                Guid userid = Guid.Parse(hid_userid_eliminar.Value);

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.SGI_Usuarios_delete(userid);
                        Tran.Complete();

                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw new Exception("No se ha podido eliminar el usuario debido a que el mismo ha sido utilizado.");
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
                string Apellido = txtApellido.Text.Trim();
                string Nombre = txtNombre.Text.Trim();
                string Email = txtEmail.Text.Trim();
                string usernameSADE = txtUsernameSADE.Text.Trim();
                string reparticion_SADE = txtReparticionSADE.Text.Trim();
                string sector_SADE = txtSectorSADE.Text.Trim();
                string cuit = txtCuit.Text.Trim();

                if (userid.HasValue)
                {
                    //Modificación
                    bool bloqueado = (ddlEditBloqueado.SelectedIndex == 1);
                    //if (bloqueado == "Sí")
                    //    bloqueado = (bool)true;
                    //else
                    //    bloqueado = (bool)false;
                    ModificarUsuario(userid.Value, Apellido, Nombre, Email, usernameSADE, reparticion_SADE, bloqueado, sector_SADE, cuit);
                    if (bloqueado == true)
                    {
                        //para bloquear el usuario intenta un logueo y cambio de password hasta bloquear la cuenta
                        MembershipUser UserBlock = Membership.GetUser(username);
                        for (int i = 0; i <= Membership.MaxInvalidPasswordAttempts; i++)
                        {
                            Membership.ValidateUser(UserBlock.UserName, "Not the right password");
                            UserBlock.ChangePassword(UserBlock.UserName, "wrong password");
                        }
                        Membership.UpdateUser(UserBlock);
                    }
                    //LimpiarDatosBusqueda();
                    BuscarUsuarios();
                    
                    updpnlBuscar.Update();
                    updResultados.Update();
                    updDatosUsuario.Update();

                    this.EjecutarScript(updBotonesGuardar, "GetoutDatos();");
                }
                else
                {
                    //Alta
                    CrearUsuario(username, Apellido, Nombre, Email, usernameSADE, reparticion_SADE, sector_SADE, cuit);
                    lblAviso.Text = "Se ha enviado el mail al usuario, con los datos de su cuenta.";
                    this.EjecutarScript(updBotonesGuardar, "showfrmAviso();");
                }

               
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

        protected void btnConsultarUsuario_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            ws_ExpedienteElectronico.consultaUsuarioResponse usuario = new ws_ExpedienteElectronico.consultaUsuarioResponse();

            try
            {
                // ---------------------------------------
                serviceEE.Url = this.url_servicio_EE;
                
                // consulta los datos del usuario en SADE
                usuario = serviceEE.ConsultaCuitCuil(txtCuit.Text.Trim());

                if (usuario != null)
                {
                    txtReparticionSADE.Text = usuario.reparticion;
                    txtUsernameSADE.Text = usuario.usuario;
                    txtSectorSADE.Text = usuario.sector;

                    txtReparticionSADE.Enabled = txtUsernameSADE.Enabled = txtSectorSADE.Enabled = false;

                }
                else
                    throw new Exception("No existe el usuario");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                serviceEE.Dispose();
                db.Dispose();
            }
        }             

        private void CrearUsuario(string username, string Apellido, string Nombre, string email, string Username_SADE, string Reparticion_SADE, string Sector_SADE, string Cuit)
        {

            DGHP_Entities db = new DGHP_Entities();

            MembershipCreateStatus status;
            MembershipUser user = null;
            Guid userid_logueado = Functions.GetUserId();

            //--------------------------------------
            // Alta de usuario
            //--------------------------------------
            Guid userid_NewUser = Guid.NewGuid();
            string password = Membership.GeneratePassword(8, 0);

            user = Membership.CreateUser(username, password, email, null, null, true, userid_NewUser, out status);

            try
            {

                if (status == MembershipCreateStatus.Success)
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {

                        // Actualiza los datos del profile y perfiles
                        try
                        {

                            db.SGI_Profiles_insert(userid_NewUser, Apellido, Nombre, Username_SADE, null, Reparticion_SADE, Sector_SADE, userid_logueado, Cuit);
                            db.SGI_Rel_Usuarios_Perfiles_delete(userid_NewUser);
                            foreach (string id_perfil in hid_perfiles_selected.Value.ToString().Split(Convert.ToChar(",")))
                            {
                                if (id_perfil.Length > 0)
                                    db.SGI_Rel_Usuarios_Perfiles_insert(userid_NewUser, Convert.ToInt32(id_perfil));
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
                                    throw new Exception("Ya existe un profile para este usuario.");
                            }
                            throw ex;
                        }

                    }
                    Mailer.MailMessages.SendMail_CreacionUsuario(userid_NewUser, email);
                    //Mailer.MailMessages.MailWelcome((Guid)user.ProviderUserKey);
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
                    if (user_delete != null)
                        db.SGI_Usuarios_delete((Guid)user_delete.ProviderUserKey);
                }
                throw ex;
            }
            finally
            {
                db.Dispose();
            }

            
        }

        private void ModificarUsuario(Guid userid_editado, string Apellido, string Nombre, string email, string Username_SADE, string Reparticion_SADE, bool bloqueado, string Sector_SADE, string Cuit)
        {

            DGHP_Entities db = new DGHP_Entities();

            MembershipUser user = Membership.GetUser(userid_editado);
            Guid userid_logueado = Functions.GetUserId();

            //--------------------------------------
            // Modificar usuario
            //--------------------------------------

            try
            {
                user.Email = email;
                //user.IsApproved = !bloqueado;
                Membership.UpdateUser(user);

                if (user.IsLockedOut && !bloqueado)
                    user.UnlockUser();
                
                

                using (TransactionScope Tran = new TransactionScope())
                {

                    // Actualiza los datos del profile y perfiles
                    try
                    {

                        db.SGI_Profiles_update(userid_editado, Apellido, Nombre, Username_SADE, null, Reparticion_SADE, userid_editado, Sector_SADE, Cuit);

                        db.SGI_Rel_Usuarios_Perfiles_delete(userid_editado);
                        foreach (string id_perfil in hid_perfiles_selected.Value.ToString().Split(Convert.ToChar(",")))
                        {
                            if (id_perfil.Length > 0)
                                db.SGI_Rel_Usuarios_Perfiles_insert(userid_editado, Convert.ToInt32(id_perfil));
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
                                throw new Exception("Ya existe un profile para este usuario.");
                        }
                        throw ex;
                    }

                }

                //Mailer.MailMessages.MailWelcome((Guid)user.ProviderUserKey);
            }
            catch(Exception ex)
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

        //private void CargarComboBloqueado()
        //{
        //    string[] laItems = new string[] { "Ninguna", "Sí", "No" };

        //    for (short i = 0; i < laItems.Length; i++)
        //    {
        //        if (i == 0)
        //            ddlBloqueado.Items.Add(laItems[i]);
        //        else
        //        {
        //            ddlBloqueado.Items.Add(laItems[i]);
        //            ddlBloqueado_SiNo.Items.Add(laItems[i]);
        //        }
        //    }
        //        ddlBloqueado.DataBind();
        //        ddlBloqueado_SiNo.DataBind();
        //}

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

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                LinkButton btnEnviarMail = (LinkButton)sender;
                Guid userid = Guid.Parse(btnEnviarMail.CommandArgument);
                LimpiarDatos();
                CargarDatos(userid);
                MembershipUser emp = null;
                emp = Membership.GetUser(txtUsername.Text);

                if (emp != null)
                {
                    Guid userId = (Guid)emp.ProviderUserKey;
                    Mailer.MailMessages.SendMail_CreacionUsuario(userId, txtEmail.Text);

                    lblAviso.Text = "Se ha enviado el mail al usuario, con los datos de su cuenta.";
                    this.EjecutarScript(updResultados, "showfrmAviso();");
                }
                else
                {
                    lblError.Text = "No se han encontrado los datos del usuario.";
                    this.EjecutarScript(updResultados, "showfrmError();");
                }
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }
            finally
            {
                db.Dispose();
            }
        }

        protected void btnReenviar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                MembershipUser emp =  Membership.GetUser(txtUsername.Text);                

                if (emp != null)
                {
                    Guid userId = (Guid)emp.ProviderUserKey;
                    SGI.Mailer.MailMessages.SendMail_CreacionUsuario(userId, txtEmail.Text);

                    lblAviso.Text = "Se ha enviado el mail al usuario, con los datos de su cuenta.";
                    this.EjecutarScript(updResultados, "showfrmAviso();");
                }
                else
                {
                    lblError.Text = "No se han encontrado los datos del usuario.";
                    this.EjecutarScript(updResultados, "showfrmError();");
                }
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }
            finally
            {
                db.Dispose();
            }
        }

        #region ExportarXLS
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            mostrarTimer("Exportación-Usuarios");
            Thread thread = new Thread(new ThreadStart(ExportarUsuariosAExcel));
            thread.Start();
        }


        private void ExportarUsuariosAExcel()
        {
            DGHP_Entities db = new DGHP_Entities();
            var q = (from mem in db.aspnet_Membership
                     join usu in db.aspnet_Users on mem.UserId equals usu.UserId
                     join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                     where mem.ApplicationId == Constants.ApplicationId
                     select new Shared.UsuariosExportacion
                     {
                         Usuario = usu.UserName,
                         Apellido = profile.Apellido,
                         Nombres = profile.Nombres,
                         Usuario_SADE = profile.UserName_SADE,
                         Reparticion_SADE = profile.Reparticion_SADE,
                         Sector_SADE = profile.Sector_SADE,
                         Email = mem.Email,
                         Bloqueado = (mem.IsLockedOut ? "Si" : "No"),
                         UltimaConexion = usu.LastActivityDate,
                         ListaPerfiles = "",
                         Perfiles = usu.SGI_PerfilesUsuarios
                     });

            var lstExportar = q.ToList();

            foreach (var item in lstExportar)
            {
                item.ListaPerfiles = string.Join(", ", item.Perfiles.Select(r => r.nombre_perfil).ToArray());
                item.Perfiles = null;
            }

            //Convierte la lista en un DataSet
            DataSet ds = new DataSet();
            DataTable dt = Funciones.ToDataTable(lstExportar);

            //Remuevo la columna auxiliar que no se debe incluir en el Excel.
            dt.Columns.Remove("Perfiles");

            dt.TableName = "SGI-Usuarios-";
            ds.Tables.Add(dt);
            string savedFileName = StaticClass.Path_Temporal + Session["filename_exportacion"].ToString();
            Funciones.EliminarArchivosDirectorioTemporal();
            Funciones.ExportDataSetToExcel(ds, savedFileName);
            // quita la variable de session.
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
            }
            catch
            {
                Timer1.Enabled = false;
            }
        }

        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            ScriptManager.RegisterStartupScript(updExportaExcel, updExportaExcel.GetType(), "script", "hidefrmExportarExcel();", true);
        }

        protected void mostrarTimer(string name)
        {
            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            Random random = new Random((int)DateTime.Now.Ticks);
            int NroAleatorio = random.Next(0, 100);
            NroAleatorio = NroAleatorio * random.Next(0, 100);
            name = name + "-{0}.xls";
            string fileName = string.Format(name, NroAleatorio);

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            Timer1.Enabled = true;
        }

        #endregion


    }
}
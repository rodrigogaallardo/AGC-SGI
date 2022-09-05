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

namespace SGI.Seguridad
{
    public partial class ActualizarUsuarios : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updResultados, updResultados.GetType(), "init_Js_updResultados", "init_Js_updResultados();", true);                
            }

            if (!IsPostBack)
            {
                SiteMaster m = (SiteMaster)this.Page.Master;
                
                
                if (!Functions.ComprobarPermisosPagina( HttpContext.Current.Request.Url.AbsolutePath))
                    Server.Transfer("~/Errores/Error3002.aspx");

                CargarDatos();
            }

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


        private void CargarDatos()
        {

            DGHP_Entities db = new DGHP_Entities();

            var usuario = (
                          from mem in db.aspnet_Membership 
                          join usu in db.aspnet_Users on mem.UserId equals usu.UserId
                          join profile in db.SGI_Profiles on usu.UserId equals profile.userid 
                          where profile.Cuit != null
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
                          });

            if (usuario != null)
            {
                lblCantidadUsuarios.Text = usuario.ToList().Count().ToString();
            }

            db.Dispose();

        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                
                DGHP_Entities db = new DGHP_Entities();
                ws_ExpedienteElectronico.consultaUsuarioResponse usua = new ws_ExpedienteElectronico.consultaUsuarioResponse();
                int total = 0;
                var usuario = (
                              from mem in db.aspnet_Membership
                              join usu in db.aspnet_Users on mem.UserId equals usu.UserId
                              join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                              where profile.Cuit != null
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
                              }).ToList();

                foreach(var item in usuario)
                {
                    try
                    {
                        usua = ConsultarUsuario(item.Cuit);
                    }
                    catch(Exception ex)
                    {
                        //throw ex;
                    }
                    if (usua.apellidoNombre != null)
                    {
                        ModificarUsuario(item.UserId, item.Apellido, item.Nombres, item.Email, usua.usuario, usua.reparticion, item.IsLockedOut, usua.sector, item.Cuit);
                        total++; 

                    }
                }
                lblCantidadRegistros.Text = total.ToString(); 
                db.Dispose();

                //BuscarUsuarios();
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
        
        protected ws_ExpedienteElectronico.consultaUsuarioResponse ConsultarUsuario(string cuit)
        {
            DGHP_Entities db = new DGHP_Entities();
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            ws_ExpedienteElectronico.consultaUsuarioResponse usuario = new ws_ExpedienteElectronico.consultaUsuarioResponse();

            try
            {
                // ---------------------------------------
                serviceEE.Url = this.url_servicio_EE;
                
                // consulta los datos del usuario en SADE
                usuario = serviceEE.ConsultaCuitCuil(cuit.Trim());

                return usuario;
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
        }
        
    }
}
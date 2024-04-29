using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;
using System.Web.Services;

namespace SGI.WebServices
{
    [RoutePrefix("api")]
    public class ws_MiAGC_Controller : ApiController
    {
        public class ResponseRoles
        {
            public List<string> roles { get; set; }
            public int statusCode { get; set; }
        }

        [WebMethod]
        [HttpGet]
        [Route("ConsultaPerfilesRoles")]
        [BasicAuthFilter]
        public ResponseRoles ConsultaPerfilesRoles()
        {
            try
            {
                DGHP_Entities db = new DGHP_Entities();
                //esto es para pasarlo a async
                //var rolesSGI = await Task.Run(() => db.aspnet_Roles_GetAllRoles("SGI"));
                //esto solamente devuelve los roles
                //var rolesSGI = db.aspnet_Roles_GetAllRoles("SGI").ToList();
                var rolesSGI = db.aspnet_Roles_GetAllRoles("SGI").ToList();

                var tareasSGI = (from usr in db.aspnet_Users
                                 from usu in usr.SGI_PerfilesUsuarios
                                 select usu.nombre_perfil).ToList();
                // devuelvo la combinacion de roles y perfiles, sin repetidos
                var combinedList = rolesSGI.Union(tareasSGI).ToList();

                db.Dispose();

                return new ResponseRoles { roles = combinedList, statusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseRoles { roles = new List<string>(), statusCode = 500 };
            }
        }

        [WebMethod]
        [HttpPost]
        [Route("CrearUsuario")]
        [BasicAuthFilter]
        //public HttpResponseMessage CrearUsuario(string username, string apellido, string nombre, string mail, string username_SADE, string reparticion_SADE, string sector_SADE, string cuit, string perfiles)
        public async Task<HttpResponseMessage> CrearUsuario()
        {
            HttpResponseMessage response;
            DGHP_Entities db = new DGHP_Entities();
            MembershipCreateStatus status;
            Guid userid_logueado = Guid.Parse("B6096E86-EFE2-45F2-B3D6-D8621B4EB2AB");
            Guid userid_NewUser = Guid.NewGuid();
            string password = Membership.GeneratePassword(8, 0);
            MiAGCBodyCrearUsuario usuario = JsonConvert.DeserializeObject<MiAGCBodyCrearUsuario>(await Request.Content.ReadAsStringAsync());
            Membership.CreateUser(usuario.username, password, usuario.mail, null, null, true, userid_NewUser, out status);
            try
            {
                if (status == MembershipCreateStatus.Success)
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {
                            db.SGI_Profiles_insert(userid_NewUser, usuario.apellido, usuario.nombre, usuario.username_SADE, null, usuario.reparticion_SADE, usuario.sector_SADE, userid_logueado, usuario.cuit);
                            db.SGI_Rel_Usuarios_Perfiles_delete(userid_NewUser);
                            foreach (string id_perfil in usuario.perfiles.Split(Convert.ToChar(",")))
                            {
                                if (id_perfil.Length > 0)
                                    db.SGI_Rel_Usuarios_Perfiles_insert(userid_NewUser, Convert.ToInt32(id_perfil));
                            }
                            Tran.Complete();
                            response = Request.CreateResponse(HttpStatusCode.OK);
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
                    Mailer.MailMessages.SendMail_CreacionUsuario(userid_NewUser, usuario.mail);
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
                    MembershipUser user_delete = Membership.GetUser(usuario.username, false);
                    if (user_delete != null)
                        db.SGI_Usuarios_delete((Guid)user_delete.ProviderUserKey);
                }
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return response;
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
    }

    public class BasicAuthFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            else
            {
                // Decode the authentication token from the request header
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                // Validate the user credentials
                if (UserValidate(username, password))
                    base.OnAuthorization(actionContext);
                else
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public static bool UserValidate(string username, string password)
        {
            if (!username.ToLower().Trim().Equals(Parametros.GetParam_ValorChar("MiAGC.Usuario").ToLower().Trim()))
                return false;
            if (!password.ToLower().Trim().Equals(Parametros.GetParam_ValorChar("MiAGC.Password").ToLower().Trim()))
                return false;
            return true;
        }
    }

    public class MiAGCBodyCrearUsuario
    {
        public string username { get; set; }
        public string apellido { get; set; }
        public string nombre { get; set; }
        public string mail { get; set; }
        public string username_SADE { get; set; }
        public string reparticion_SADE { get; set; }
        public string sector_SADE { get; set; }
        public string cuit { get; set; }
        public string perfiles { get; set; }
    }
}


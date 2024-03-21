using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using SGI.Model;
using SGI.StaticClassNameSpace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using static SGI.Account.AuthenticateMIAgc;

namespace SGI.Account
    {
        public class AuthenticateMIAgc
        {

            public string ReadData()
            {
            string ret = "ok";
            string usuario = "";
            string token = "";
            List<string> roles = null;
            HttpRequest Request = HttpContext.Current.Request;
            HttpResponse Response = HttpContext.Current.Response;

            string returnUrl = (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]) ? Request.QueryString["returnUrl"] : "");
            try
            {

                try
                {
                    // Deserializar el JSON en un objeto usando Newtonsoft.Json
                    string jsonData = Request.Form["usuario_perfil"];
                    UserPerfil userPerfil = JsonConvert.DeserializeObject<UserPerfil>(jsonData);


                    usuario = userPerfil.Usuario;
                    DGHP_Entities db = new DGHP_Entities();
                    usuario = (from u in db.SGI_Profiles where u.Cuit == usuario select u.aspnet_Users2.LoweredUserName).FirstOrDefault();
                    
                    token = userPerfil.Token;
                    roles = userPerfil.Roles;

                    // Verificar si el usuario existe en la base de datos
                    MembershipUser existingUser = Membership.GetUser(usuario);
                    if (existingUser != null)
                    {
                        // El usuario existe en la base de datos
                        ret = "ok";
                    }
                    else
                    {
                        // El usuario no existe en la base de datos
                        ret = "El usuario no existe en la base de datos.";
                    }

                    // Validar el token
                    string tokenUsuario = ObtenerTokenUsuarioDesdeBaseDeDatos(usuario);

                    // Verificar si el token recibido coincide con el token almacenado en la base de datos
                    if (tokenUsuario != token)
                    {
                        // El token no coincide
                        ret = "El token no es valido.";
                    }
                }
                catch (Exception)
                {
                    ret = "El token y el usuario son campos requeridos";
                    throw new Exception("El token y el usuario son campos requeridos");
                }

                if (ret == "ok")
                {
                    AsignarRoles(roles, usuario);
                    GenerarTicketAutenticacion(usuario);
                }
                else
                {
                    //GenerarTicketAutenticacion(usuario);
                    //string url = "";

                    //if (string.IsNullOrWhiteSpace(Globals.username))
                    //    url = Functions.GetParametroChar("TAD.Url");
                    //else
                    //{
                    //    url = "~/" + RouteConfig.HOME;
                    //}

                    //Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                //if (ex != null & !(ex is System.Threading.ThreadAbortException))
                //{
                //    string id = LogError.Log(ex);
                //    ret = false;
                //    Response.Redirect(string.Format("~/Error/{0}", id));
                //}
            }

            return ret;

            }

        private string ObtenerTokenUsuarioDesdeBaseDeDatos(string usuario)
        {
            DGHP_Entities db = new DGHP_Entities();
            Guid appSGI = new Guid("5BC28D51-C240-4D79-87B4-27D554686CE3");
            Guid userId = (from o in db.aspnet_Users where o.LoweredUserName == usuario && o.ApplicationId == appSGI select o.UserId).FirstOrDefault();
            string token = (from u in db.aspnet_token_usuario where u.UsuarioId == userId select u.Token).FirstOrDefault();

            return token;
        }

        private bool AsignarRoles(List<string> roles, string usuario)
        {
            DGHP_Entities db = new DGHP_Entities();
            Guid appSGI = new Guid("5BC28D51-C240-4D79-87B4-27D554686CE3");
            aspnet_Users user = (from o in db.aspnet_Users where o.LoweredUserName == usuario && o.ApplicationId == appSGI select o).FirstOrDefault();
            
            // Limpiar todos los roles
            user.SGI_PerfilesUsuarios.Clear();
            db.SaveChanges();

            if (roles != null && roles.Count > 0)
            {
                foreach (string rol in roles)
                {
                    SGI_Perfiles rolId = (from o in db.SGI_Perfiles where o.nombre_perfil.ToUpper() == rol.ToUpper() select o).FirstOrDefault();
                    user.SGI_PerfilesUsuarios.Add(rolId);
                    // Asignar el rol
                    Debug.WriteLine("Rol asignado: " + rol);
                }
                db.SaveChanges();
                return true; // se asignaron los roles correctamente
            }
            else
            {
                return false; // No hay roles para asignar
            }
        }
        
        private void GenerarTicketAutenticacion(string username)
        {
            // Genera el ticket de autenticación
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(20), true, "",
                                   FormsAuthentication.FormsCookiePath);

            string hashCookies = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            //
            //FormsAuthentication.SetAuthCookie(username, true);
            //Globals.username = username;
        }

        public class UserPerfil
        {
            public string Usuario { get; set; }
            public List<string> Roles { get; set; }
            public string Token { get; set; }
        }


    }
}

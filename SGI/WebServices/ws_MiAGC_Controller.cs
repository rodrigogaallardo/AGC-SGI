using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
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
                string roles;
                DGHP_Entities db = new DGHP_Entities();

                //var rolesSGI = await Task.Run(() => db.aspnet_Roles_GetAllRoles("SGI"));
                var rolesSGI = db.aspnet_Roles_GetAllRoles("SGI").ToList();

                db.Dispose();

                return new ResponseRoles { roles = rolesSGI, statusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseRoles { roles = new List<string>(), statusCode = 500 };
            }
        }


    }

    public class BasicAuthFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
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
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

        public static bool UserValidate(string username, string password)
        {
            //TODO: Que saque estas credenciales de parametros
            return username == "ws-sgi" && password == "prueba123";
        }
    }


}



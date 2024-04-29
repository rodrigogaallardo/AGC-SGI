using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using SGI;
using SGI.App_Start;
using System.Web.Http;

namespace SGI
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciarse la aplicación
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Código que se ejecuta al cerrarse la aplicación

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se produce un error sin procesar

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se inicia una nueva sesión
        }

        public void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Debug.WriteLine(Request.InputStream.CanRead, "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
            // Esto sirve para que cuando redirigen con el token autologuea
            if (Request.Form["usuario_perfil"] != null)
            {
                string returnUrl = Request.Url.AbsoluteUri;

                Account.AuthenticateMIAgc auth = new Account.AuthenticateMIAgc();
                auth.ReadData();
                if(auth.ReadData() != "ok")
                {
                    Response.StatusCode = 400; // Bad Request
                    Response.Write(auth.ReadData());
                    Response.End(); // Terminar la respuesta
                }
                else
                {
                    Response.Redirect(returnUrl);
                }

            }

        }

    }
}

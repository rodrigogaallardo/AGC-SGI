﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using SGI;

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
    }
}

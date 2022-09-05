using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using SGI.Model;
using System.Linq;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Deployment.Application;
using System.Reflection;

namespace SGI
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    FormsAuthentication.SignOut();
                    //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        public static Version ApplicationVersion
        {
            get
            {
                if ((ApplicationDeployment.IsNetworkDeployed))
                    return ApplicationDeployment.CurrentDeployment.CurrentVersion;
                else
                    return Assembly.GetExecutingAssembly().GetName().Version;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "[Version " + ApplicationVersion.Major.ToString() + " Revision " + ApplicationVersion.Revision.ToString() + "]";

            if (string.IsNullOrEmpty(Page.Title))
                SiteTitle.Text = "SGI";
            else
                SiteTitle.Text = "SGI - " + Page.Title;

            if (!Page.IsPostBack)
            {

                CargarMenu();
            }
        }

        protected void LogOff_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }


        private void CargarMenu()
        {
            MembershipUser usuario = Membership.GetUser();

            bool tiene_bandeja_entrada = false;
            bool tiene_buscar_tramite = false;

            if (usuario != null)
            {
                Guid userId = (Guid)usuario.ProviderUserKey;


                List<SGI_Menues> menu_usuarios = Usuario.GetMenuUsuario(userId);

                DGHP_Entities db = new DGHP_Entities();
                var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

                foreach (var perfil in perfiles_usuario)
                {
                    var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                    if (menu_usuario.Contains("Bandeja de Entrada"))
                        tiene_bandeja_entrada = true;
                    if (menu_usuario.Contains("Buscar Trámites"))
                        tiene_buscar_tramite = true;                
                }

                repeater_menu.DataSource = menu_usuarios;
                repeater_menu.DataBind();

            }
            else
            {
                //ver que mostrar para indicar que no tiene 
                repeater_menu.DataSource = null;
                repeater_menu.DataBind();
            }


            lnkBuscarTramite.Visible = tiene_buscar_tramite;
            lnkBandejaEntrada.Visible = tiene_bandeja_entrada;
        }
    }
}
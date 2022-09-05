using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Dashboard
{
    public partial class SolicitudesPendientesCalificar : System.Web.UI.Page
    {

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion

        protected void ucSolicitudesPendientes_Error(object sender, SGI.Dashboard.Controls.ucSolicitudesPendientesCalificar.ucSolicitudesPendientes_EventArgs e)
        {
            if ( e.ex != null ) 
                Enviar_Mensaje(e.ex.Message, "");
        }

        protected void ucSolicitudesAsignadas_Error(object sender, SGI.Dashboard.Controls.ucSolicitudesAsignadasCalificar.ucSolicitudesAsignadas_EventArgs e)
        {
            if (e.ex != null)
                Enviar_Mensaje(e.ex.Message, "");
        }
    }
}
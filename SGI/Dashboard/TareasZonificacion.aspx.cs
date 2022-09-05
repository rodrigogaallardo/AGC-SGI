using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
namespace SGI.Dashboard
{
    public partial class TareasZonificacion : System.Web.UI.Page
    {

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargaInicial();
            }
        }

        private void cargaInicial()
        {
            ucGraficoTorta.LoadData((int)Constants.ENG_Tareas.SSP_Validar_Zonificacion, true, "Solicitudes sin asignar");
            ucGraficoTorta2.LoadData((int)Constants.ENG_Tareas.SSP_Validar_Zonificacion);
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

        protected void ucGraficoTorta_Error(object sender, SGI.Dashboard.Controls.ucGraficoTorta.ucGraficoTorta_EventArgs e)
        {
            if (e.ex != null)
                Enviar_Mensaje(e.ex.Message, "");
        }

        protected void ucGraficoTorta2_Error(object sender, SGI.Dashboard.Controls.ucGraficoTorta2.ucGraficoTorta2_EventArgs e)
        {
            if (e.ex != null)
                Enviar_Mensaje(e.ex.Message, "");
        }
    }
}
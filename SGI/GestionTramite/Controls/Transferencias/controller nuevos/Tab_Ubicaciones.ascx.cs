using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.Transferencias
{
    public partial class Tab_Ubicaciones : System.Web.UI.UserControl
    {
        public delegate void EventHandlerUbicacionActualizada(object sender, EventArgs e);
        public event EventHandlerUbicacionActualizada UbicacionActualizada;

       /* private int validar_estado
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_validar_estado.Value, out ret);
                return ret;
            }
            set
            {
                hid_validar_estado.Value = value.ToString();
            }

        }
        private int id_solicitud
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_solicitud.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
            }

        }


        public void CargarDatos(int id_solicitud, int Validar)
        {

            this.id_solicitud = id_solicitud;
            this.validar_estado = Validar;
            visUbicaciones.Editable = true;
            visUbicaciones.CargarDatos(id_solicitud);

            CargarTablaPlantasHabilitar(id_solicitud);

        }

        protected void visUbicaciones_EliminarClick(object sender, ucEliminarEventsArgs args)
        {

            btnEliminar_Si.CommandArgument = args.id_cpadronubicacion.ToString();

            this.EjecutarScript(updUbicaciones, "showfrmConfirmarEliminar();");

        }
        protected void visUbicaciones_EditarClick(object sender, ucEditarEventsArgs args)
        {

            this.BuscarUbicacion.id_cpadronUbicacion = args.id_cpadronubicacion;
            this.BuscarUbicacion.validar_estado = this.validar_estado;
            this.BuscarUbicacion.id_cpadron_mod = this.id_cpadron;
            ScriptManager.RegisterStartupScript(updAgregarUbicacion, updAgregarUbicacion.GetType(), "showfrmAgregarUbicacion()", "showfrmAgregarUbicacion();", true);
            this.BuscarUbicacion.editar();


        }
        public void EjecutarScript(UpdatePanel objupd, string script)
        {
            ScriptManager.RegisterClientScriptBlock(objupd, objupd.GetType(), "script", script, true);
        }

        protected void btnEliminar_Si_Click(object sender, EventArgs e)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {
                Button btnEliminar_Si = (Button)sender;
                int id_cpadronubicacion = int.Parse(btnEliminar_Si.CommandArgument);

                // Eliminar la ubicación.
                db.CPadron_EliminarUbicacion(this.id_cpadron, id_cpadronubicacion, validar_estado);

                // vuelve a cargar los datos.
                visUbicaciones.CargarDatos(this.id_cpadron);
                Functions.EjecutarScript(updUbicaciones, "hidefrmConfirmarEliminar();");


                // Hace Fire al eneto de Actualización de la úbicación si el mismo está definido.
                if (UbicacionActualizada != null)
                    UbicacionActualizada(sender, e);

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updConfirmarEliminar, "hidefrmConfirmarEliminar();showfrmError();");
            }
            finally
            {
                db.Dispose();
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }*/
    }
}
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Dashboard.Controls
{
    public partial class ucSolicitudesPendientesCalificarTR : System.Web.UI.UserControl
    {
        public class ucSolicitudesPendientes_EventArgs : EventArgs
        {
            public string mensaje { get; set; }
            public Exception ex { get; set; }
        }

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                cargar_datos();
            }
            catch (Exception ex)
            {
                enviar_error(sender, ex);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            Dispose();
            base.OnUnload(e);
        }

        public void Dispose()
        {
            FinalizarEntity();
        }

        private void cargar_datos()
        {
            IniciarEntity();
            CargarDetalle();
            FinalizarEntity();
        }

        private void enviar_error(object sender, Exception ex)
        {
            if (ex != null)
            {
                if (Error != null)
                {
                    ucSolicitudesPendientes_EventArgs args = new ucSolicitudesPendientes_EventArgs();
                    args.ex = ex;
                    Error(sender, args);
                }
            }
        }

        #endregion


        #region Eventos

        public delegate void EventHandler_error_sol_pend(object sender, ucSolicitudesPendientes_EventArgs e);
        public event EventHandler_error_sol_pend Error;

        #endregion


        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion


        #region grilla detalle

        protected void btnCargarDetalle_OnClick(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();
                CargarDetalle();
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                enviar_error(sender, ex);

            }

        }

        public void CargarDetalle()
        {

            DateTime fecha_hoy = DateTime.Today;
            
            int[] tareas_asignar = Dashboard.getTareasAsignarPorUsuarioTR();

            var tramites =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    where tareas_asignar.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null 
                    orderby tt.FechaInicio_tramitetarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        tt.id_tramitetarea,
                        tt_hab.id_solicitud,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaInicio_tramitetarea, fecha_hoy)
                    }
                ).ToList();

            grdSolPend.DataSource = tramites;
            grdSolPend.DataBind();

            updPnlSolicitudesPendienetes_detalle.Update();
        }

        #endregion
    }
}
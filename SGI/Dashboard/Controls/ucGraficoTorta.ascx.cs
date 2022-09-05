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
    public partial class ucGraficoTorta : System.Web.UI.UserControl
    {

        public class ucGraficoTorta_EventArgs : EventArgs
        {
            public string mensaje { get; set; }
            public Exception ex { get; set; }
        }

        #region cargar inicial

        private bool detalle= true;

        public void LoadData(int tareaId, bool mostarListado, string titulo_detalle)
        {
            tarea_id.Value = tareaId.ToString();
            detalle = mostarListado;
            SolicitudesPendienetes_detalle.Visible = mostarListado;
            tituloDetalle.Text = titulo_detalle;
        }

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
            if(detalle)
                CargarDetalle();
            FinalizarEntity();
        }

        private void enviar_error(object sender, Exception ex)
        {
            if (ex != null)
            {
                if (Error != null)
                {
                    ucGraficoTorta_EventArgs args = new ucGraficoTorta_EventArgs();
                    args.ex = ex;
                    Error(sender, args);
                }
            }
        }

        #endregion


        #region Eventos

        public delegate void EventHandler_error_sol_pend(object sender, ucGraficoTorta_EventArgs e);
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
                if (detalle)
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
            int[] tareas_asignar = new int[1] { Int32.Parse(tarea_id.Value) };

            var tramites =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
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
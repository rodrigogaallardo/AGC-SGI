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
    public partial class ucSolicitudesAsignadasCalificarTR : System.Web.UI.UserControl
    {
        public class ucSolicitudesAsignadas_EventArgs : EventArgs
        {
            public string mensaje { get; set; }
            public Exception ex { get; set; }
        }


        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
           IniciarEntity();
            //Cargo el listado completo en la GridView
            DateTime fecha_hoy = DateTime.Today;
            

            int[] tareas = Dashboard.getTareasCalificarPorUsuarioTR();

            var tramites =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    where tareas.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null
                    orderby tt.FechaAsignacion_tramtietarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        calificador = cir.cod_circuito + " - " + per.Apellido + ", " + per.Nombres,
                        tt.id_tramitetarea,
                        tt_hab.id_solicitud,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                    }
                ).ToList();

            grdSolAsig.DataSource = tramites;
            grdSolAsig.DataBind();

            updPnlSolicitudesAsignadas_detalle.Update();

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

        private void enviar_error(object sender, Exception ex)
        {
            if (ex != null)
            {
                if (Error != null)
                {
                    ucSolicitudesAsignadas_EventArgs args = new ucSolicitudesAsignadas_EventArgs();
                    args.ex = ex;
                    Error(sender, args);
                }
            }
        }

        #endregion

        #region Eventos

        public delegate void EventHandler_error_sol_asig(object sender, ucSolicitudesAsignadas_EventArgs e);
        public event EventHandler_error_sol_asig Error;

        #endregion

        #region Propiedades

        private Guid _userid;
        public Guid userid
        {
            get
            {
                Guid aux;
                if (Guid.TryParse(hid_userid_detalle.Value, out aux))
                {
                    _userid = aux;
                }
                else
                {
                    aux = Guid.Empty;
                }

                return _userid;
            }
            set
            {
                _userid = value;
                hid_userid_detalle.Value = _userid.ToString();
            }
        }

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
                CargarDetalle(this.userid);
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                enviar_error(sender, ex);
            }

        }

        public void CargarDetalle(Guid userid)
        {

            DateTime fecha_hoy = DateTime.Today;
            int[] tareas = Dashboard.getTareasCalificarPorUsuarioTR();            

            var tramites =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    where tt.UsuarioAsignado_tramitetarea == userid &&
                            tareas.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null 
                    orderby tt.FechaAsignacion_tramtietarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        calificador = cir.cod_circuito + " - " + per.Apellido + ", " + per.Nombres,
                        tt.id_tramitetarea,
                        tt_hab.id_solicitud,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                    }
                ).ToList();

            grdSolAsig.DataSource = tramites;
            grdSolAsig.DataBind();

            updPnlSolicitudesAsignadas_detalle.Update();
        }

        #endregion
    }
}
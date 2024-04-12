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
    public partial class ucSolicitudesAsignadasCalificar : System.Web.UI.UserControl
    {


        public class ucSolicitudesAsignadas_EventArgs : EventArgs
        {
            public string mensaje{ get; set; }
            public Exception ex { get; set; }
        }


        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            IniciarEntity();
            //Cargo el listado completo en la GridView
            DateTime fecha_hoy = DateTime.Today;
            int[] tareas = Dashboard.getTareasCalificarPorUsuario();
            Guid userid = Functions.GetUserId();
            var qEquipo =
                   (

                       from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                       join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                       join eq in db.ENG_EquipoDeTrabajo on tramite_tareas.UsuarioAsignado_tramitetarea equals eq.Userid
                       join usr in db.SGI_Profiles on eq.Userid equals usr.userid
                       join mem in db.aspnet_Membership on usr.userid equals mem.UserId
                       where 
                       eq.Userid_Responsable == userid
                       && mem.IsApproved == true
                       && tareas.Contains(tramite_tareas.id_tarea)
                       && tramite_tareas.FechaCierre_tramitetarea == null

                       select
                        usr.userid

                   ).Distinct().ToList();


            var tramites_hab =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join sol in db.SSIT_Solicitudes on tt_hab.id_solicitud equals sol.id_solicitud
                    where
                    tt.FechaCierre_tramitetarea == null
                    && qEquipo.Contains(per.userid)
                    && tt.FechaAsignacion_tramtietarea != null
                    && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado

                    orderby tt.FechaAsignacion_tramtietarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        calificador = cir.cod_circuito+" - "+per.Apellido + ", " + per.Nombres,
                        tt.id_tramitetarea,
                        tt_hab.id_solicitud,
                        id_tarea = tt.id_tarea,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                    }
                ).ToList();

            var tramites_transf =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                    join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join sol in db.Transf_Solicitudes on tt_transf.id_solicitud equals sol.id_solicitud
                    where
                    tt.FechaCierre_tramitetarea == null
                    && qEquipo.Contains(per.userid)
                    && tt.FechaAsignacion_tramtietarea != null
                    && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado

                    orderby tt.FechaAsignacion_tramtietarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        calificador = cir.cod_circuito + " - " + per.Apellido + ", " + per.Nombres,
                        tt.id_tramitetarea,
                        tt_transf.id_solicitud,
                        id_tarea = tt.id_tarea,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                    }
                ).ToList();

            var tramites_cpadron =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_cpadron in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cpadron.id_tramitetarea
                    join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join sol in db.CPadron_Solicitudes on tt_cpadron.id_cpadron equals sol.id_cpadron
                    where
                    tt.FechaCierre_tramitetarea == null
                    && qEquipo.Contains(per.userid)
                    && tt.FechaAsignacion_tramtietarea != null
                    && sol.id_estado != (int)Constants.CPadron_EstadoSolicitud.Anulado

                    orderby tt.FechaAsignacion_tramtietarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        calificador = cir.cod_circuito + " - " + per.Apellido + ", " + per.Nombres,
                        tt.id_tramitetarea,
                        id_solicitud = tt_cpadron.id_cpadron,
                        id_tarea = tt.id_tarea,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                    }
                ).ToList();

            var tramites_totales = tramites_hab
                            .Concat(tramites_transf)
                            .Concat(tramites_cpadron)
                            .ToList();

            grdSolAsig.DataSource = tramites_totales;
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
                Guid aux ;
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
            int[] tareas = Dashboard.getTareasCalificarPorUsuario();

            var tramites_Hab = (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                join sol in db.SSIT_Solicitudes on tt_hab.id_solicitud equals sol.id_solicitud
                where
                tt.UsuarioAsignado_tramitetarea == userid
                && tt.FechaCierre_tramitetarea == null
                && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
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

            var tramites_transf = (
                from tt in db.SGI_Tramites_Tareas
                join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                join sol in db.SSIT_Solicitudes on tt_transf.id_solicitud equals sol.id_solicitud
                where
                tt.UsuarioAsignado_tramitetarea == userid
                && tt.FechaCierre_tramitetarea == null
                && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                orderby tt.FechaAsignacion_tramtietarea
                select new
                {
                    userid = tt.UsuarioAsignado_tramitetarea,
                    calificador = cir.cod_circuito + " - " + per.Apellido + ", " + per.Nombres,
                    tt.id_tramitetarea,
                    tt_transf.id_solicitud,
                    tt.FechaInicio_tramitetarea,
                    dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                }
            ).ToList();

                var tramites_cpadron = (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_cpadron in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cpadron.id_tramitetarea
                    join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join sol in db.CPadron_Solicitudes on tt_cpadron.id_cpadron equals sol.id_cpadron
                    where
                    tt.UsuarioAsignado_tramitetarea == userid
                    && tt.FechaCierre_tramitetarea == null
                    && sol.id_estado != (int)Constants.CPadron_EstadoSolicitud.Anulado
                    orderby tt.FechaAsignacion_tramtietarea
                    select new
                    {
                        userid = tt.UsuarioAsignado_tramitetarea,
                        calificador = cir.cod_circuito + " - " + per.Apellido + ", " + per.Nombres,
                        tt.id_tramitetarea,
                        id_solicitud = tt_cpadron.id_cpadron,
                        tt.FechaInicio_tramitetarea,
                        dias = EntityFunctions.DiffDays(tt.FechaAsignacion_tramtietarea, fecha_hoy)
                    }
                ).ToList();

            var tramites_totales = tramites_Hab
                                    .Concat(tramites_transf)
                                    .Concat(tramites_cpadron)
                                    .ToList();

            // Asigna la lista combinada como el DataSource del GridView
            grdSolAsig.DataSource = tramites_totales;
            grdSolAsig.DataBind();

            updPnlSolicitudesAsignadas_detalle.Update();
        }

        #endregion

    }

}
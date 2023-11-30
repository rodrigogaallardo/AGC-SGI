using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Asignar_Calificador : BasePage
    {

        #region cargar inicial

        protected async Task Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    await CargarDatosTramite(id_tramitetarea);
            }

        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        #region entity

        private DGHP_Entities db = null;

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

        #endregion

        private async Task CargarDatosTramite(int id_tramitetarea)
        {
            IniciarEntity();

            Guid userid = Functions.GetUserId();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);

            bool IsEditable = false;

            pnlUsuarioAsignado.Visible = false;
            pnlAsignarUsuario.Visible = false;

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);


            if (IsEditable)
            {
                pnlAsignarUsuario.Visible = true;
            }
            else
            {
                pnlUsuarioAsignado.Visible = true;
            }


            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucObservacionPlancheta.Enabled = IsEditable;
            ucObservacionProvidencia.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Asignar_Calificador asignar_calificar = Buscar_Tarea(id_tramitetarea);

            Guid? usuario_asignado = (asignar_calificar == null) ? null : asignar_calificar.usuario_asignado;

            cargarUsuariosAsignar(userid, tramite_tarea.id_tarea, usuario_asignado);

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (asignar_calificar != null) ? asignar_calificar.Observaciones : "";
            ddlUsuarioAsignar.Enabled = IsEditable;

            if (this.id_solicitud > nroTrReferencia)
                ucListaObservacionesAnteriores.Visible = false;
            else
                ucListaObservacionesAnterioresv1.Visible = false;
        }

        private List<SGI.Servicios.clsEquipoTrabajo> lstEquipoTrabajo = null;

        private void cargarUsuariosAsignar(Guid userid, int id_tarea, Guid? usuario_asignado)
        {
            hid_ddlEmpleado.Value = "";

            var qUsuario =
                (
                    from perf in db.SGI_Profiles
                    where perf.userid == userid
                    select new
                    {
                        perf.Apellido,
                        perf.Nombres
                    }
                ).FirstOrDefault();

            txtUsuarioasignado.Text = qUsuario.Apellido + " " + qUsuario.Nombres;

            // buscar roles de usuario
            List<int> lst_perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.id_perfil).ToList();

            // buscar roles de usuarios que pueden ser asignados a la tarea
            var q_rol_asignador =
                (
                    from conf in db.ENG_Config_BandejaAsignacion
                    where lst_perfiles_usuario.Contains(conf.id_perfil_asignador.Value)
                    && conf.id_tarea == id_tarea
                    select new
                    {
                        conf.id_perfil_asignado,
                        conf.id_perfil_asignador
                    }
                ).ToList();


            lstEquipoTrabajo = new List<Servicios.clsEquipoTrabajo>();

            Servicios srv = new Servicios();

            // buscar usuarios del equipo de trabajo 
            foreach (var item in q_rol_asignador)
            {

                List<SGI.Servicios.clsEquipoTrabajo> lstEquipo = srv.GetEquipoDeTrabajo(userid.ToString(), item.id_perfil_asignado.ToString());

                foreach (SGI.Servicios.clsEquipoTrabajo equipo in lstEquipo)
                {
                    // no agregar al usuario que esta asignando
                    // no agregar al usuario que ya tiene la tarea 

                    if (!lstEquipoTrabajo.Exists(x => x.userid.ToLower() == equipo.userid.ToLower()))
                    {
                        equipo.nombres_apellido = equipo.nombres_apellido + "|" + equipo.tramites;
                        lstEquipoTrabajo.Add(equipo);

                    }

                }

            }

            int index = 0;
            if (lstEquipoTrabajo.Count > 0)
            {

                // si puedo asignarlo a otros tambien me agrego a mi
                if (!lstEquipoTrabajo.Exists(x => x.userid.ToLower() == userid.ToString().ToLower()))
                {
                    SGI.Servicios.clsEquipoTrabajo equipo = new Servicios.clsEquipoTrabajo();
                    equipo.userid = userid.ToString().ToLower();
                    equipo.nombres_apellido = qUsuario.Apellido + " " + qUsuario.Nombres;
                    equipo.tramites = "Sin tr&aacute;mites pendientes";
                    lstEquipoTrabajo.Add(equipo);
                }

                if (usuario_asignado.HasValue)
                {
                    index = lstEquipoTrabajo.FindIndex(x => x.userid.ToLower() == usuario_asignado.ToString().ToLower());

                    if (index > -1)
                        hid_ddlEmpleado.Value = lstEquipoTrabajo[index].userid.ToString();

                }


            }

            srv.Dispose();

            //usuario_asignado

            // carga de combo con usuarios
            ddlUsuarioAsignar.DataValueField = "userid";
            ddlUsuarioAsignar.DataTextField = "nombres_apellido";
            ddlUsuarioAsignar.DataSource = lstEquipoTrabajo;
            ddlUsuarioAsignar.DataBind();

            if (index > 0)
                ddlUsuarioAsignar.SelectedIndex = index;

            if (lstEquipoTrabajo.Count > 0)
                ddlUsuarioAsignar.Items.Insert(0, new ListItem("", "0"));

            // cuando el usuario tiene algun rol de asignacion para tarea actual, se pone visible el control del usuario


        }

        private int _tramiteTarea = 0;
        public int TramiteTarea
        {
            get
            {
                if (_tramiteTarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _tramiteTarea);
                }
                return _tramiteTarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _tramiteTarea = value;
            }
        }

        private int _id_solicitud = 0;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        private SGI_Tarea_Asignar_Calificador Buscar_Tarea(int id_tramitetarea)
        {

            SGI_Tarea_Asignar_Calificador asignar_calificar =
                (
                    from asig_calif in db.SGI_Tarea_Asignar_Calificador
                    where asig_calif.id_tramitetarea == id_tramitetarea
                    orderby asig_calif.id_asignar_calificador descending
                    select asig_calif
                ).ToList().FirstOrDefault();

            return asignar_calificar;
        }

        #endregion


        #region acciones


        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }


        private void Validar_Tarea()
        {
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId, Guid usuario_asignado)
        {

            SGI_Tarea_Asignar_Calificador asignar_calificar = Buscar_Tarea(id_tramite_tarea);

            int id_asignar_calificador = 0;
            if (asignar_calificar != null)
                id_asignar_calificador = asignar_calificar.id_asignar_calificador;

            db.SGI_Tarea_Asignar_Calificador_Actualizar(id_asignar_calificador, id_tramite_tarea, observacion, userId, usuario_asignado);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid_asignado = Guid.Parse(hid_ddlEmpleado.Value);

                Guid userid = Functions.GetUserId();

                IniciarEntity();

                Validar_Tarea();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid, userid_asignado);
                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. Asignar_Calificador-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }
                }

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();

            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }

        }



        private void Validar_Finalizar()
        {
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid_asignado = Guid.Parse(hid_ddlEmpleado.Value);

                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid, userid_asignado);
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                        // SGI_AsignarTarea
                        if(db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == id_tramitetarea_nuevo).FirstOrDefault().UsuarioAsignado_tramitetarea == null)
                            Engine.TomarTarea(id_tramitetarea_nuevo, userid_asignado);

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. Asignar_Calificador-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }

                }


                Enviar_Mensaje("Se ha finalizado la tarea.", "");

                Redireccionar_VisorTramite();

            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }

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
    }

}
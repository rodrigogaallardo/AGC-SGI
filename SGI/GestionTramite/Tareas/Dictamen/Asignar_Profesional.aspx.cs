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

namespace SGI.GestionTramite.Tareas.Dictamen
{
    public partial class Asignar_Profesional : System.Web.UI.Page
    {
        #region cargar inicial

        protected async void Page_Load(object sender, EventArgs e)
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

        private async Task CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            IniciarEntity();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            pnlUsuarioAsignado.Visible = false;
            pnlAsignarUsuario.Visible = false;
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

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

            SGI_Tarea_Dictamen_Asignar_Profesional pvh = Buscar_Tarea(id_tramitetarea);

            Guid? usuario_asignado = (pvh == null) ? null : pvh.usuario_asignado;

            cargarUsuariosAsignar(userid, tramite_tarea.id_tarea, usuario_asignado);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";
            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Enviar_al_Gerente)
            {
                ddlUsuarioAsignar.SelectedIndex = 0;
                updpnlAsignarUsuario.Update();
            }
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

        public int id_circuito
        {
            get
            {
                return (ViewState["_id_circuito"] != null ? Convert.ToInt32(ViewState["_id_circuito"]) : 0);
            }
            set
            {
                ViewState["_id_circuito"] = value.ToString();
            }
        }
        private SGI_Tarea_Dictamen_Asignar_Profesional Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Dictamen_Asignar_Profesional pvh =
                (
                    from env_phv in db.SGI_Tarea_Dictamen_Asignar_Profesional
                    where env_phv.id_tramitetarea == id_tramitetarea
                    orderby env_phv.id_Dictamen_Asignar_Profesional descending
                    select env_phv
                ).FirstOrDefault();

            return pvh;
        }

        #endregion


        #region acciones

        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0); string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }


        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId, Guid? usuario_asignado)
        {

            SGI_Tarea_Dictamen_Asignar_Profesional pvh = Buscar_Tarea(id_tramite_tarea);

            int id_Dictamen_Asignar_Profesional = 0;
            if (pvh != null)
                id_Dictamen_Asignar_Profesional = pvh.id_Dictamen_Asignar_Profesional;

            db.SGI_Tarea_Dictamen_Asignar_Profesional_Actualizar(id_Dictamen_Asignar_Profesional, id_tramite_tarea, observacion, userId, usuario_asignado);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid? userid_asignado = null;
                if (!string.IsNullOrEmpty(hid_ddlEmpleado.Value) && hid_ddlEmpleado.Value != "0")
                    userid_asignado = Guid.Parse(hid_ddlEmpleado.Value);

                Guid userid = Functions.GetUserId();

                IniciarEntity();

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
                        LogError.Write(ex, "error en transaccion. pvh-ucResultadoTarea_GuardarClick");
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
            int cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Primer_Gestion_Documental);
            var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
            var id_tarea_ger = t != null ? t.id_tarea : 0;
            if ((string.IsNullOrEmpty(hid_ddlEmpleado.Value) || hid_ddlEmpleado.Value == "0")
                && ucResultadoTarea.getIdProximaTarea() != id_tarea_ger)
                throw new Exception("Seleccione calificador.");
        }

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Enviar_al_Gerente)
            {
                ddlUsuarioAsignar.SelectedIndex = 0;
                updpnlAsignarUsuario.Update();
            }
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                Guid? userid_asignado = null;
                if (!string.IsNullOrEmpty(hid_ddlEmpleado.Value) && hid_ddlEmpleado.Value != "0")
                    userid_asignado = Guid.Parse(hid_ddlEmpleado.Value);

                TransactionScope Tran = new TransactionScope();

                try
                {

                    Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid, userid_asignado);
                    db.SaveChanges();

                    id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                    int cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Primer_Gestion_Documental);
                    var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
                    var id_tarea_ger = t != null ? t.id_tarea : 0;
                    if (e.id_proxima_tarea != id_tarea_ger)
                        Engine.TomarTarea(id_tramitetarea_nuevo, userid_asignado.Value);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    if (Tran != null)
                        Tran.Dispose();
                    LogError.Write(ex, "error en transaccion. pvh-ucResultadoTarea_FinalizarTareaClick");
                    throw ex;
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
    }
}
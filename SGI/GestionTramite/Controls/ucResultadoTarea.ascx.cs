using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Data.Entity.Core.Objects;
using System.Collections;
using System.Data.Entity.Migrations;

namespace SGI.GestionTramite.Controls
{

    public class ucResultadoTareaEventsArgs : EventArgs
    {
        public int id_tramitetarea_actual { get; set; }
        public int id_tarea_actual { get; set; }
        public Nullable<int> id_proxima_tarea { get; set; }
        public Nullable<int> id_resultado { get; set; }
    }

    public partial class ucResultadoTarea : System.Web.UI.UserControl
    {

        #region "Propiedades"

        private bool _Enabled;

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Devuelve/Establece el estado de los controles contenidos en este control.")]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                ddlResultado.Enabled = value;
                ddlProximaTarea.Enabled = value;
                ddlTipoPlanoIncendio.Enabled = value;
                btnFinalizarTarea.Visible = _Enabled;
                btnGuardar.Visible = _Enabled;
                ddlProximaTarea.Visible = _Enabled;
                lblProximaTarea.Visible = _Enabled;
                lblTipoPlanoIncendio.Visible = _Enabled;
                ddlTipoPlanoIncendio.Visible = _Enabled;
            }
        }

        private string _ValidationGroupFinalizar = "";
        public string ValidationGroupFinalizar
        {
            get
            {
                return _ValidationGroupFinalizar;
            }
            set
            {
                _ValidationGroupFinalizar = value;
                btnFinalizarTarea.ValidationGroup = _ValidationGroupFinalizar;
            }
        }

        private string _ValidationGroupGuardar = "";
        public string ValidationGroupGuardar
        {
            get
            {
                return _ValidationGroupGuardar;
            }
            set
            {
                _ValidationGroupGuardar = value;
                btnGuardar.ValidationGroup = _ValidationGroupGuardar;
            }
        }

        public bool ddlResultado_Enabled
        {
            get
            {
                return ddlResultado.Enabled;
            }
            set
            {
                ddlResultado.Enabled = value;
            }
        }
        public bool ddlProximaTarea_Enabled
        {
            get
            {
                return ddlProximaTarea.Enabled;
            }
            set
            {
                ddlProximaTarea.Enabled = value;
            }
        }

        public bool btnFinalizar_Enabled
        {
            get
            {
                return btnFinalizarTarea.Enabled;
            }
            set
            {
                btnFinalizarTarea.Enabled = value;
            }
        }

        public bool btnFinalizar_Visible
        {
            get
            {
                return btnFinalizarTarea.Visible; ;
            }
            set
            {
                btnFinalizarTarea.Visible = value;
            }
        }

        public bool btnGuardar_Enabled
        {
            get
            {
                return btnGuardar.Enabled; ;
            }
            set
            {
                btnGuardar.Enabled = value;
            }
        }

        public bool btnGuardar_Visible
        {
            get
            {
                return btnGuardar.Visible; ;
            }
            set
            {
                btnGuardar.Visible = value;
            }
        }

        private int _id_proximatarea_default = 0;
        public int id_proximatarea_default
        {
            get
            {
                return _id_proximatarea_default;
            }
            set
            {
                _id_proximatarea_default = value;
            }
        }
        #endregion

        #region "Eventos"

        public delegate void EventHandlerGuardar(object sender, ucResultadoTareaEventsArgs e);
        public event EventHandlerGuardar GuardarClick;

        public delegate void EventHandlerFinalizarTarea(object sender, ucResultadoTareaEventsArgs e);
        public event EventHandlerFinalizarTarea FinalizarTareaClick;

        public event EventHandler CerrarClick;

        public delegate void EventHandlerResultadoIndexChange(object sender, ucResultadoTareaEventsArgs e);
        public event EventHandlerResultadoIndexChange ResultadoSelectedIndexChanged;

        protected virtual void OnGuardarClick(EventArgs e)
        {
            if (GuardarClick != null)
            {

                int id_tarea_actual = 0;
                int id_proxima_tarea = 0;
                int id_resultado = 0;
                int id_tramitetarea = 0;
                int id_planoincendio = 0;
                int.TryParse(hid_id_tramite_tarea.Value, out id_tramitetarea);
                int.TryParse(hid_id_tarea.Value, out id_tarea_actual);
                int.TryParse(ddlProximaTarea.SelectedValue, out id_proxima_tarea);
                int.TryParse(ddlResultado.SelectedValue, out id_resultado);
                int.TryParse(ddlTipoPlanoIncendio.SelectedValue, out id_planoincendio);

                ucResultadoTareaEventsArgs args = new ucResultadoTareaEventsArgs();
                args.id_tramitetarea_actual = id_tramitetarea;
                args.id_tarea_actual = id_tarea_actual;

                if (id_proxima_tarea > 0)
                    args.id_proxima_tarea = id_proxima_tarea;

                if (id_resultado > 0)
                    args.id_resultado = id_resultado;

                actualizarTabla(id_resultado, id_tramitetarea, id_planoincendio);
                GuardarClick(this, args);
            }
        }
        protected virtual void OnFinalizarTareaClick(EventArgs e)
        {
            try
            {
                if (FinalizarTareaClick != null)
                {

                    int id_tarea_actual = 0;
                    int id_proxima_tarea = 0;
                    int id_resultado = 0;
                    int id_tramitetarea = 0;
                    int id_planoincendio = 0;
                    int.TryParse(hid_id_tramite_tarea.Value, out id_tramitetarea);
                    int.TryParse(hid_id_tarea.Value, out id_tarea_actual);
                    int.TryParse(ddlProximaTarea.SelectedValue, out id_proxima_tarea);
                    int.TryParse(ddlResultado.SelectedValue, out id_resultado);
                    int.TryParse(ddlTipoPlanoIncendio.SelectedValue, out id_planoincendio);

                    ucResultadoTareaEventsArgs args = new ucResultadoTareaEventsArgs();
                    args.id_tramitetarea_actual = id_tramitetarea;
                    args.id_tarea_actual = id_tarea_actual;
                    args.id_proxima_tarea = id_proxima_tarea;
                    if (id_proxima_tarea > 0)
                        args.id_proxima_tarea = id_proxima_tarea;

                    if (id_resultado > 0)
                        args.id_resultado = id_resultado;

                    actualizarTabla(id_resultado, id_tramitetarea, id_planoincendio);
                    FinalizarTareaClick(this, args);
                }
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }

        protected virtual void OnCerrarClick(EventArgs e)
        {
            if (CerrarClick != null)
            {
                CerrarClick(this, e);
            }
        }

        #endregion

        public void LoadData(int id_tramitetarea, bool confirmacionFinalizar)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_tramitetarea, confirmacionFinalizar);
        }

        public void LoadData(int id_grupotramite, int id_tramitetarea, bool confirmacionFinalizar)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            int id_tarea = 0, id_resultado = 0, id_proxima_tarea;
            DateTime? FechaCierre_tramitetarea = null;
            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            SGI_Tarea_Transicion_Enviar_DGFYCO transicion = db.SGI_Tarea_Transicion_Enviar_DGFYCO.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            id_tarea = tramite_tarea.id_tarea;
            FechaCierre_tramitetarea = tramite_tarea.FechaCierre_tramitetarea;
            id_resultado = tramite_tarea.id_resultado;
            id_proxima_tarea = tramite_tarea.id_proxima_tarea != null ? tramite_tarea.id_proxima_tarea.Value : 0;
            if (id_proxima_tarea == 0)
                id_proxima_tarea = id_proximatarea_default;

            var tiposPlanos = db.SGI_Tipos_Planos_Incendio.ToList();

            ddlTipoPlanoIncendio.DataSource = tiposPlanos;
            ddlTipoPlanoIncendio.DataTextField = "nombre_tipoplanoincendio";
            ddlTipoPlanoIncendio.DataValueField = "id_tipoplanoincendio";
            ddlTipoPlanoIncendio.DataBind();

            if (transicion != null)
                ddlTipoPlanoIncendio.SelectedValue = transicion.id_tipoplanoincendio.ToString();

            hid_id_grupotramite.Value = id_grupotramite.ToString();
            hid_id_tramite_tarea.Value = id_tramitetarea.ToString();
            hid_id_tarea.Value = id_tarea.ToString();
            Engine.Tarea tarea = Engine.Tarea.Get(id_tarea, id_tramitetarea);

            hid_alert_conf.Value = confirmacionFinalizar.ToString();

            ddlResultado.DataSource = tarea.Resultados;
            ddlResultado.DataTextField = "nombre_resultado";
            ddlResultado.DataValueField = "id_resultado";
            ddlResultado.DataBind();
            if (ddlResultado.Items.Count > 0)
            {
                //setea el valor leido desde la tarea ya almacenada
                if (id_resultado > 0)
                    ddlResultado.SelectedValue = id_resultado.ToString();
                else
                {
                    // Si la tarea no está editable y no está cerrada se oculta el resultado
                    if (!_Enabled && !FechaCierre_tramitetarea.HasValue)
                    {
                        lblResultado.Visible = false;
                        ddlResultado.Visible = false;
                    }
                }
                int id_resultadoSelect = int.Parse(ddlResultado.SelectedValue);
                
                CargarProximasTareas(id_resultadoSelect, id_tarea, id_tramitetarea, id_proxima_tarea);
            }

            db.Dispose();
        }

        protected void ddlResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tarea = Convert.ToInt32(hid_id_tarea.Value);
            int id_resultado = int.Parse(ddlResultado.SelectedValue);
            int id_tramitetarea = int.Parse(hid_id_tramite_tarea.Value);

            CargarProximasTareas(id_resultado, id_tarea, id_tramitetarea, 0);

            if (ResultadoSelectedIndexChanged != null)
            {
                ucResultadoTareaEventsArgs args = new ucResultadoTareaEventsArgs();
                args.id_tramitetarea_actual = id_tramitetarea;
                args.id_tarea_actual = id_tarea;
                args.id_resultado = id_resultado;
                args.id_proxima_tarea = null;
                ResultadoSelectedIndexChanged(sender, args);
            }
        }

        private void CargarProximasTareas(int id_resultado, int id_tarea, int id_tramitetarea, int id_proxima_tarea)
        {
            ddlProximaTarea.DataSource = Engine.GetTareasSiguientes(id_resultado, id_tarea, id_tramitetarea);
            ddlProximaTarea.DataTextField = "nombre_tarea";
            ddlProximaTarea.DataValueField = "id_tarea";
            ddlProximaTarea.DataBind();
            //setea el valor leido desde la tarea ya almacenada
            if (id_proxima_tarea > 0)
                ddlProximaTarea.SelectedValue = id_proxima_tarea.ToString();
            if (id_resultado == 94)
            {
                ddlTipoPlanoIncendio.Visible = true;
                lblTipoPlanoIncendio.Visible = true;
            }
            else
            {
                ddlTipoPlanoIncendio.Visible = false;
                lblTipoPlanoIncendio.Visible = false;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            OnGuardarClick(e);
        }

        protected void btnFinalizarTarea_Click(object sender, EventArgs e)
        {
            Page.Validate("finalizar");
            if (Page.IsValid)
                OnFinalizarTareaClick(e);
        }

        public int FinalizarTarea()
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            int id_tramitetarea = int.Parse(hid_id_tramite_tarea.Value);
            int id_resultado = int.Parse(ddlResultado.SelectedValue);
            int id_grupotramite = int.Parse(hid_id_grupotramite.Value);
            int id_proximatarea = 0;
            int id_tramitetarea_nuevo = 0;
            string cod_tar = null;

            ObjectParameter param_id_tramitetarea = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));

            int.TryParse(ddlProximaTarea.SelectedValue, out id_proximatarea);
            Guid userid = new Guid();
            DateTime? FechaCierre_tramitetarea = null;
            int id_solicitud = 0;
            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (tramite_tarea != null)
            {
                userid = (Guid)tramite_tarea.UsuarioAsignado_tramitetarea; // el usuario asignado a esta tarea es que el finaliza y crea la proxima
                FechaCierre_tramitetarea = tramite_tarea.FechaCierre_tramitetarea;
            }

            var es_TR = db.SGI_Tramites_Tareas_TRANSF.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();
            if (id_grupotramite == (int)Constants.GruposDeTramite.CP && es_TR != null)
                id_grupotramite = (int)Constants.GruposDeTramite.TR;

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                SGI_Tramites_Tareas_HAB tramite_tarea_hab = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                if (tramite_tarea_hab != null)
                {
                    id_solicitud = tramite_tarea_hab.id_solicitud;
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {
                SGI_Tramites_Tareas_CPADRON cpadron_tarea = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                if (cpadron_tarea != null)
                {
                    id_solicitud = cpadron_tarea.id_cpadron;

                    param_id_tramitetarea = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));

                    db.ENG_Finalizar_Tarea(id_tramitetarea, id_resultado, id_proximatarea, userid, param_id_tramitetarea);

                    if (param_id_tramitetarea.Value != DBNull.Value)
                        id_tramitetarea_nuevo = Convert.ToInt32(param_id_tramitetarea.Value);


                    if (id_proximatarea == (int)Constants.ENG_Tareas.CP_Carga_Informacion)
                    {
                        Guid CalificadorAnterior = Engine.GetUltimoUsuarioAsignadoCP(id_solicitud, id_proximatarea);
                        id_tramitetarea_nuevo = db.SGI_Tramites_Tareas_CPADRON.OrderByDescending(x => x.id_rel_tt_CPADRON).FirstOrDefault(x => x.id_cpadron == id_solicitud).id_tramitetarea;
                        if (CalificadorAnterior != Guid.Empty)
                            Engine.TomarTarea(id_tramitetarea_nuevo, CalificadorAnterior);
                    }
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                SGI_Tramites_Tareas_TRANSF tramite_tarea_hab = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                if (tramite_tarea_hab != null)
                {
                    id_solicitud = tramite_tarea_hab.id_solicitud;
                }
            }

            if (id_solicitud != 0)
            {
                if (!FechaCierre_tramitetarea.HasValue)
                {
                    if (param_id_tramitetarea.Value == null)
                    {
                        param_id_tramitetarea = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));
                        db.ENG_Finalizar_Tarea(id_tramitetarea, id_resultado, id_proximatarea, userid, param_id_tramitetarea);
                    }

                    if (param_id_tramitetarea.Value != DBNull.Value)
                        id_tramitetarea_nuevo = Convert.ToInt32(param_id_tramitetarea.Value);

                    var Tarea = db.ENG_Tareas.Where(x => x.id_tarea == id_proximatarea).FirstOrDefault();

                    // Si la tarea generada es la de calificar trámite se debe asignar el mismo calificador que en la oportunidad anterior
                    // siempre y cuando se haya calificado al menos una vez y solo si es calificador,  gerente nop
                    cod_tar = Tarea.cod_tarea.ToString().Substring(Tarea.cod_tarea.ToString().Length - 2, 2);
                    if (cod_tar == Constants.ENG_Tipos_Tareas.Calificar ||
                        cod_tar == Constants.ENG_Tipos_Tareas.Calificar2 ||
                        cod_tar == Constants.ENG_Tipos_Tareas.Calificar3)
                    {
                        int id_tareaCal = id_proximatarea;
                        if (id_proximatarea == (int)Constants.ENG_Tareas.ESP_Calificar_2)
                            id_tareaCal = (int)Constants.ENG_Tareas.ESP_Calificar_1;
                        else if (id_proximatarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2)
                            id_tareaCal = (int)Constants.ENG_Tareas.ESPAR_Calificar_1;
                        else if (id_proximatarea == (int)Constants.ENG_Tareas.ESP_Calificar_2_Nuevo)
                            id_tareaCal = (int)Constants.ENG_Tareas.ESP_Calificar_1_Nuevo;
                        else if (id_proximatarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo)
                            id_tareaCal = (int)Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo;

                        List<string> codTar = new List<string> { Constants.ENG_Tipos_Tareas.Calificar, Constants.ENG_Tipos_Tareas.Calificar2,
                            Constants.ENG_Tipos_Tareas.Calificar3};

                        Guid CalificadorAnterior = Engine.GetUltimoUsuarioAsignado(id_solicitud, id_tareaCal, id_grupotramite, codTar);
                        if (CalificadorAnterior != Guid.Empty && Engine.EsCalificador(CalificadorAnterior))
                            Engine.TomarTarea(id_tramitetarea_nuevo, CalificadorAnterior);
                    }
                }
            }
            else
                throw new Exception(string.Format("No se encontro el registro en SGI_tramites_tareas con el id {0}", id_tramitetarea));

            db.Dispose();

            return id_tramitetarea_nuevo;

        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            OnCerrarClick(e);
        }

        protected void actualizarTabla(int id_resultado, int id_tramitetarea, int id_tipoplanoincendio)
        {
            DGHP_Entities db = new DGHP_Entities();
            SGI_Tarea_Transicion_Enviar_DGFYCO actual = (from u in db.SGI_Tarea_Transicion_Enviar_DGFYCO
                                                        where u.id_tramitetarea == id_tramitetarea
                                                        select u).FirstOrDefault();

            if (id_resultado == 94)
            {
                if (actual != null)
                {
                    actual.id_tipoplanoincendio = id_tipoplanoincendio;
                    db.SGI_Tarea_Transicion_Enviar_DGFYCO.AddOrUpdate(actual);
                }
                else
                {
                    SGI_Tarea_Transicion_Enviar_DGFYCO nuevo = new SGI_Tarea_Transicion_Enviar_DGFYCO();
                    nuevo.id_tramitetarea = id_tramitetarea;
                    nuevo.id_tipoplanoincendio = id_tipoplanoincendio;
                    db.SGI_Tarea_Transicion_Enviar_DGFYCO.Add(nuevo);
                }
            }
            else
            {
                if (actual != null)
                {
                    db.SGI_Tarea_Transicion_Enviar_DGFYCO.Remove(actual);
                }
            }
            db.SaveChanges();
            db.Dispose();
        }

        public int getIdProximaTarea()
        {
            int id_proximatarea = 0;

            int.TryParse(ddlProximaTarea.SelectedValue, out id_proximatarea);
            return id_proximatarea;
        }

        public int getIdResultadoTarea()
        {
            int id_resultado = 0;

            int.TryParse(ddlResultado.SelectedValue, out id_resultado);
            return id_resultado;
        }
    }
}
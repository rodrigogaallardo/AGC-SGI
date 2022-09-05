using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Generar_Expediente : BasePage
    {
        #region cargar inicial
        private int id_solicitud
        {
            get
            {
                
                int ret = (ViewState["_id_solicitud"] != null ? Convert.ToInt32(ViewState["_id_solicitud"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_solicitud"] = value.ToString();
            }
        }

        private int id_grupotramite
        {
            get
            {

                int ret = (ViewState["_id_grupotramite"] != null ? Convert.ToInt32(ViewState["_id_grupotramite"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_grupotramite"] = value.ToString();
            }
        }

        private int id_tramitetarea
        {
            get
            {

                int ret = (ViewState["_id_tramitetarea"] != null ? Convert.ToInt32(ViewState["_id_tramitetarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_tramitetarea"] = value.ToString();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {

                
            }

            if (!IsPostBack)
            {
                this.id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);

            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            try
            {
                CargarDatosTramite(this.id_tramitetarea);
                this.EjecutarScript(updCargaInicial, "finalizarCarga();" );

            }
            catch (Exception ex)
            {

                lblErrorCargaInicial.Text = ex.Message;
                pnlErrorCargaInicial.Visible = true;
            }

            
        }

        private void CargarDatosTramite(int id_tramitetarea)
        {
            Guid userid = Functions.GetUserId();

            IniciarEntity();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro un registro con el id_tramitetarea proporcionado. id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----

            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;

            int id_grupotramite, id_solicitud;
            Engine.getIdSolicitud_IdGrupoTrabajo(id_tramitetarea, out id_solicitud, out id_grupotramite);
            this.id_solicitud = id_solicitud;
            this.id_grupotramite = id_grupotramite;
            ucCabecera.LoadData(id_grupotramite, id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);

            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            if (IsEditable && !hayProcesosGenerados)
            {
                try
                {
                    if(tramite_tarea.ENG_Tareas.id_circuito == (int)Constants.ENG_Circuitos.TRANSF_NUEVO)
                        db.SGI_Transmisiones_GenerarProcesos_SADE(this.id_tramitetarea, userid);
                    else
                        db.SGI_Transf_GenerarProcesos_SADE(this.id_tramitetarea, userid);
                }
                catch (Exception ex)
                {

                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updCargarProcesos, "showfrmError();");
                    IsEditable = false;
                }
            }
            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
            ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, false);
            ucResultadoTarea.btnFinalizar_Enabled = IsEditable && !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

            FinalizarEntity();

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

        protected void btnCargarProcesos_Click(object sender, EventArgs e)
        {
            try
            {

                IniciarEntity();
                Guid userid = Functions.GetUserId();
                bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);

                ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, IsEditable);

                FinalizarEntity();

        }
            catch (Exception ex) 
        {

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updCargarProcesos, "showfrmError();");
        }

        }


        #endregion


        private void Enviar_Mensaje(UpdatePanel upd, string mensaje, string titulo)
                {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            this.EjecutarScript(upd,"mostrarMensaje('" + mensaje + "','" + titulo + "')");

                }

        private void Enviar_Mensaje(Page pag, string mensaje, string titulo)
            {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            this.EjecutarScript(pag, "mostrarMensaje('" + mensaje + "','" + titulo + "')");

        }

        private bool ActualizarNroExpediente()
        {
            var sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
            if (string.IsNullOrEmpty(sol.NroExpedienteSade))
            {
                int id_paquete = (from p in db.SGI_SADE_Procesos
                                  join tt_hab in db.SGI_Tramites_Tareas_TRANSF on p.id_tramitetarea equals tt_hab.id_tramitetarea
                                  where tt_hab.id_solicitud == this.id_solicitud
                                  && p.id_proceso == (int)Constants.EE_Procesos.GeneracionCaratula
                                  && p.realizado_en_SADE == true
                                  select p.id_paquete).FirstOrDefault();
                EE_Entities dbEE = new EE_Entities();
                var car = dbEE.wsEE_Caratulas.FirstOrDefault(x => x.id_paquete == id_paquete);
                dbEE.Dispose();

                if (car == null || string.IsNullOrEmpty(car.resultado_SADE))
                    return false;

                this.db.SGI_Transf_Nro_Expediente_Sade_Actualizar(id_solicitud, car.resultado_SADE);
            }
            return true;
        }
        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            try
            {

                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;

                if (!ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {
                    if (ActualizarNroExpediente())
                    {

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
                        Enviar_Mensaje(updFinalizarTarea, "Se ha finalizado la tarea.", "");
                        Redireccionar_VisorTramite();
                    }
                    else
                        Enviar_Mensaje(updFinalizarTarea, "No es posible avanzar la tarea, no se puedo obtener el numero de expediente.", "");
                }
                else
                {
                    Enviar_Mensaje(updFinalizarTarea,"No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updFinalizarTarea, "showfrmError();");
            }
            finally
            {
                FinalizarEntity();
        }

        }

        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
            {
            Redireccionar_VisorTramite();
        }

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            ucResultadoTarea.btnFinalizar_Enabled = true;
            ucListaDocumentos.LoadData(this.id_grupotramite, this.id_solicitud);
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }
 
    }
}
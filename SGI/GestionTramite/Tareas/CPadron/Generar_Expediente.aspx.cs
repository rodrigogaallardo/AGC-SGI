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

namespace SGI.GestionTramite.Tareas.CPadron
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

        protected async Task btnCargarDatos_Click(object sender, EventArgs e)
        {

            try
        {
                await CargarDatosTramite(this.id_tramitetarea);
                this.EjecutarScript(updCargaInicial, "finalizarCarga();" );

            }
            catch (Exception ex)
            {

                lblErrorCargaInicial.Text = ex.Message;
                if (ex.InnerException.Message.Trim().Length != 0)
                    lblErrorCargaInicial.Text = lblErrorCargaInicial.Text + ' ' + ex.InnerException.Message;
                pnlErrorCargaInicial.Visible = true;
            }

            
        }

        private async Task CargarDatosTramite(int id_tramitetarea)
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

            int id_grupotramite = (int)Constants.GruposDeTramite.CP;
            SGI_Tramites_Tareas_CPADRON ttCP = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttCP.id_cpadron;
            ucCabecera.LoadData(id_grupotramite, id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;

            if (IsEditable && !hayProcesosGenerados)
                db.SGI_CPadron_GenerarProcesos_SADE(this.id_tramitetarea, userid);

            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.CP;
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

        private bool ActualizarNroExpediente()
        {
            var sol = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == id_solicitud);
            if (string.IsNullOrEmpty(sol.NroExpedienteSade))
            {
                int id_paquete = (from p in db.SGI_SADE_Procesos
                                  join tt_hab in db.SGI_Tramites_Tareas_CPADRON on p.id_tramitetarea equals tt_hab.id_tramitetarea
                                  where tt_hab.id_cpadron == this.id_solicitud
                                  && p.id_proceso == (int)Constants.EE_Procesos.GeneracionCaratula
                                  && p.realizado_en_SADE == true
                                  select p.id_paquete).FirstOrDefault();
                EE_Entities dbEE = new EE_Entities();
                var car = dbEE.wsEE_Caratulas.FirstOrDefault(x => x.id_paquete == id_paquete);
                dbEE.Dispose();

                if (car == null || string.IsNullOrEmpty(car.resultado_SADE))
                    return false;

                this.db.SGI_CPadron_Nro_Expediente_Sade_Actualizar(id_solicitud, car.resultado_SADE);
            }
            return true;
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
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
            }

        }
    }

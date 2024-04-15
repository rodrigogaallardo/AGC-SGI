using SGI.GestionTramite.Controls;
using SGI.Mailer;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Revision_Firma_Disposicion : System.Web.UI.Page
    {

        #region cargar inicial

        protected async void Page_Load(object sender, EventArgs e)
        {

            IniciarEntity();

            if (!IsPostBack)
            {
                try
                {
                    int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                    if (id_tramitetarea > 0)
                        await CargarDatosTramite(id_tramitetarea);
                }
                catch (Exception ex)
                {
                    Enviar_Mensaje(ex.Message, "");
                }
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


            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;

            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.id_tramitetarea = id_tramitetarea;
            this.id_paquete = (from p in db.SGI_SADE_Procesos
                               join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                               join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                               where tt_hab.id_solicitud == this.id_solicitud
                               select p.id_paquete).FirstOrDefault();

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucTitulares.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);

            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud, (int)Constants.TipoDeTramite.Transferencia);
            ucResultadoTarea.btnGuardar_Visible = false;

            SGI_Tarea_Entregar_Tramite rev = Buscar_Tarea(id_tramitetarea);
            if (rev != null)
                ucObservacionesTarea.Text = rev.Observaciones;
            else
                ucObservacionesTarea.Text = "";

            ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out int nroTrReferencia);
            if (this.id_solicitud > nroTrReferencia)
                ucListaObservacionesAnteriores.Visible = false;
            else
                ucListaObservacionesAnterioresv1.Visible = false;
        }

        private int _tramiteTarea = 0;
        public int id_tramitetarea
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

        private int id_paquete
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_paquete"] != null ? Convert.ToInt32(ViewState["_id_paquete"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_paquete"] = value;
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

        private SGI_Tarea_Entregar_Tramite Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Entregar_Tramite entregar_tramite =
                (
                    from ent_tra in db.SGI_Tarea_Entregar_Tramite
                    where ent_tra.id_tramitetarea == id_tramitetarea
                    orderby ent_tra.id_entregar_tramite descending
                    select ent_tra
                ).ToList().FirstOrDefault();

            return entregar_tramite;
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

        private void Validar_Guardar()
        {
            // agregar aca validaciones
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Entregar_Tramite entregar_tramite = Buscar_Tarea(id_tramite_tarea);

            int id_entregar_tramite = 0;
            if (entregar_tramite != null)
                id_entregar_tramite = entregar_tramite.id_entregar_tramite;
            db.SGI_Tarea_Entregar_Tramite_Actualizar(id_entregar_tramite, id_tramite_tarea, observacion, null, null, null, null, userId);
        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Validar_Guardar();

                using (TransactionScope Tran = new TransactionScope())
                {
                    Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid);

                    db.SaveChanges();

                    Tran.Complete();
                    Tran.Dispose();
                }
                db.Dispose();

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }

        }



        private void Validar_Finalizar()
        {
            // agregar aca validaciones
        }

        private int id_tipotramite = 0;
        private int id_tipoexpediente = 0;
        private int id_subtipoexpediente = 0;

        private void buscar_datos_solicitud()
        {
            // buscar datos del tipo de tramite 
            var datos_sol =
                (
                    from sol in this.db.SSIT_Solicitudes
                    where sol.id_solicitud == this.id_solicitud
                    select new
                    {
                        sol.id_tipotramite,
                        sol.id_tipoexpediente,
                        sol.id_subtipoexpediente,
                        sol.id_solicitud
                    }
                ).FirstOrDefault();

            this.id_tipotramite = datos_sol.id_tipotramite;
            this.id_tipoexpediente = datos_sol.id_tipoexpediente;
            this.id_subtipoexpediente = datos_sol.id_subtipoexpediente;
            this.id_solicitud = datos_sol.id_solicitud;
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea = this.id_tramitetarea;
                int id_tramitetarea_nuevo = 0;
                int id_resultado = (int)e.id_resultado;
                int id_proxima_tarea = (int)e.id_proxima_tarea;

                IniciarEntity();

                Validar_Finalizar();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;

                if (!hayProcesosGenerados)
                {
                    //Valido si el ultimo "calificar tramite" esta aprobado o rechazado
                    var ttTR = (from tt in db.SGI_Tramites_Tareas
                                join tt_TR in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_TR.id_tramitetarea
                                join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                                where
                                  tt_TR.id_solicitud == id_solicitud
                                  && tt.id_tramitetarea < id_tramitetarea
                                  //&& (t.cod_tarea == 510  || t.cod_tarea == 710)
                                  && t.cod_tarea.ToString().Substring(t.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Calificar
                                orderby tt_TR.id_tramitetarea descending
                                select tt
                    ).FirstOrDefault();
                    //Busco la tarea destino
                    var tDest = (from et in db.ENG_Transiciones where et.id_tarea_origen == id_proxima_tarea select et).FirstOrDefault();
                    var idProximaTarea = (from t in db.ENG_Tareas where t.cod_tarea == 729 select t).FirstOrDefault().id_tarea;
                    bool es_valida = false;

                    //Valido cual es la tarea destino y el id del resultado
                    if (tDest != null)
                    {
                        if (tDest.id_tarea_destino == idProximaTarea && ttTR.id_resultado == 19)
                        {
                            //le paso true
                            es_valida = true;
                        }
                        else
                        {
                            es_valida = false;
                        }
                    }
                    else
                        es_valida = false;

                    db.SGI_Tarea_Revision_Firma_Disposicion_GenerarProcesos(this.id_tramitetarea, this.id_paquete, userid, @es_valida);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, true);
                }
                else if (!ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {

                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text.Trim(), userid);
                            db.SaveChanges();

                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. revision_firma_dispo-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }
                    db.Dispose();

                    Enviar_Mensaje("Se ha finalizado la tarea.", "");

                    if (Functions.IsAprobadoTransf(id_solicitud))
                    {
                        MailMessages.SendMail_AprobadoSolicitud_v2(id_solicitud);
                    }
                    else
                    {
                        MailMessages.SendMail_RechazoSolicitud_v2(id_solicitud);
                    }

                    Redireccionar_VisorTramite();
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }

            }
            catch (Exception ex)
            {
                FinalizarEntity();
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

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }

    }
}
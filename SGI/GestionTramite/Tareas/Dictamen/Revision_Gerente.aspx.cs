﻿using SGI.GestionTramite.Controls;
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
    public partial class Revision_Gerente : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Enviar_PVH;

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
            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable && !hayProcesosGenerados;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;
            this.id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

            SGI_Tarea_Dictamen_Revision_Gerente pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, this.TramiteTarea);

            int cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Dictamen_RevisionGerente2);
            var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
            var id_tarea_ger = t != null ? t.id_tarea : 0;

            if (id_tarea == id_tarea_ger)
            {
                ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                ucProcesosSADE.cargarDatosProcesos(id_tramitetarea, IsEditable);
                ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
                if (IsEditable)
                    ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);
            }
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
        public int id_tarea
        {
            get
            {
                return (ViewState["_id_tarea"] != null ? Convert.ToInt32(ViewState["_id_tarea"]) : 0);
            }
            set
            {
                ViewState["_id_tarea"] = value.ToString();
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

        private SGI_Tarea_Dictamen_Revision_Gerente Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Dictamen_Revision_Gerente pvh =
                (
                    from env_phv in db.SGI_Tarea_Dictamen_Revision_Gerente
                    where env_phv.id_tramitetarea == id_tramitetarea
                    orderby env_phv.id_Dictamen_Revision_Gerente descending
                    select env_phv
                ).FirstOrDefault();

            return pvh;
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

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Dictamen_Revision_Gerente pvh = Buscar_Tarea(id_tramite_tarea);

            int id_enviar_avh = 0;
            if (pvh != null)
                id_enviar_avh = pvh.id_Dictamen_Revision_Gerente;

            db.SGI_Tarea_Dictamen_Revision_Gerente_Actualizar(id_enviar_avh, id_tramite_tarea, observacion, userId);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                IniciarEntity();

                Validar_Tarea();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
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
        }


        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Validar_Finalizar();

                Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                db.SaveChanges();
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                int cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Gestion_Documental);
                var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
                var id_tarea_ger = t != null ? t.id_tarea : 0;

                bool sinProceso = false;
                if (ucResultadoTarea.getIdProximaTarea() != id_tarea_ger)
                {
                    sinProceso = true;
                }
                if (!sinProceso && !hayProcesosGenerados)
                {
                    db.SGI_Tarea_Dictamen_Revision_Gerente_GenerarProcesos(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || sinProceso || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {
                            ucResultadoTarea.FinalizarTarea();
                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. revision_gerente-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }
                    db.Dispose();

                    Enviar_Mensaje("Se ha finalizado la tarea.", "");
                    Redireccionar_VisorTramite();
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                string message = ex.Message;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;
                Enviar_Mensaje(message, "");
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
            ucResultadoTarea.btnFinalizar_Enabled = true;
        }
    }

}
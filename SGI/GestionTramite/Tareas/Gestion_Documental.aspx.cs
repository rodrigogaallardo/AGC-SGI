using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.UI;
using SGI.GestionTramite.Controls;
using SGI.Model;

namespace SGI.GestionTramite.Tareas
{
    public partial class Gestion_Documental : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Revision_Gerente;

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

            this.db = new DGHP_Entities();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            this.Title = "Tarea: " + tramite_tarea.ENG_Tareas.nombre_tarea;

            if (tramite_tarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }



            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservaciones.Enabled = IsEditable && !hayProcesosGenerados;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable && !hayProcesosGenerados;
            ucConsiderandoDispo.Enabled = IsEditable && !hayProcesosGenerados;
            ucObservacionPlancheta.Enabled = IsEditable && !hayProcesosGenerados;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;
            this.id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

            SGI_Tarea_Gestion_Documental gerente = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);

            ucListaRubros.Visible = true;
            ucTramitesRelacionados.Visible = true;

            if (ttHAB.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(ttHAB.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Primer_Gestion_Documental)
                ucConsiderandoDispo.Visible = false;

            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);

            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);

            if (gerente != null)
            {
                ucObservaciones.Text = gerente.Observaciones;
                ucConsiderandoDispo.Text = db.SGI_Tramites_Tareas_Dispo_Considerando.Where(x => x.id_tramitetarea == id_tramitetarea).Select(y => y.considerando_dispo).FirstOrDefault();
                ucObservacionPlancheta.Text = gerente.observacion_plancheta;
            }
            else
            {
                ucObservaciones.Text = "";
                if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
                {
                    ucConsiderandoDispo.Text = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.Label1");
                    ucConsiderandoDispo.Update();
                }
                ucObservacionPlancheta.Text = ObservacionAnteriores.Buscar_ObservacionPlancheta(
                    (int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
            }

            var tar = db.ENG_Tareas.Where(x => x.id_tarea == this.id_tarea).Select(y => y.cod_tarea).FirstOrDefault();

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                this.id_paquete = (from p in db.SGI_Tarea_Generar_Expediente_Procesos
                                   join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                   join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                   where tt_hab.id_solicitud == this.id_solicitud
                                   select p.id_paquete).FirstOrDefault();
                if (this.id_paquete == 0)
                    this.id_paquete = (from p in db.SGI_SADE_Procesos
                                       join tt_hab in db.SGI_Tramites_Tareas_HAB on p.id_tramitetarea equals tt_hab.id_tramitetarea
                                       where tt_hab.id_solicitud == this.id_solicitud
                                       select p.id_paquete).FirstOrDefault();

                SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);

                ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;

                ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            }

            this.db.Dispose();

        }

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            ucConsiderandoDispo.Enabled = true;

            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                ucConsiderandoDispo.Text = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.Label1");
                ucConsiderandoDispo.Update();
            }
            else if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Observacion)
            {
                ucConsiderandoDispo.Text = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Observa.Label1");
                ucConsiderandoDispo.Update();
            }
            else if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Desestima)
            {
                ucConsiderandoDispo.Text = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Rechaza.Label1");
                ucConsiderandoDispo.Update();
            }
            else
            {
                ucConsiderandoDispo.Text = "";
                ucConsiderandoDispo.Update();
            }
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
        private SGI_Tarea_Gestion_Documental Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Gestion_Documental gerente =
                (
                    from gere in db.SGI_Tarea_Gestion_Documental
                    where gere.id_tramitetarea == id_tramitetarea
                    orderby gere.id_gestion_documental descending
                    select gere
                ).ToList().FirstOrDefault();

            return gerente;
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


        private void Validar_Tarea()
        {
        }

        private void Guardar_tarea(bool finalizar, int id_solicitud, int id_tramite_tarea, string observacion, string observacion_plancheta, Guid userId, string considerando)
        {
            SGI_Tarea_Gestion_Documental gerente = Buscar_Tarea(id_tramite_tarea);
            SGI_Tramites_Tareas_Dispo_Considerando text_dispo = db.SGI_Tramites_Tareas_Dispo_Considerando.Where(x => x.id_tramitetarea == id_tramite_tarea).FirstOrDefault();

            int id_gestion_documental = 0;
            if (gerente != null)
                id_gestion_documental = gerente.id_gestion_documental;

            int id_sgi_tt_dispo = 0;
            if (text_dispo != null)
                id_sgi_tt_dispo = text_dispo.id_tt_Dispo_Considerando;

            db.SGI_Tarea_Gestion_Documental_Actualizar( id_gestion_documental, id_tramite_tarea, observacion, observacion_plancheta, userId);
            db.SGI_Tramites_Tareas_Dispo_Considerando_Actualizar(id_sgi_tt_dispo, id_tramite_tarea, considerando);
        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Validar_Tarea();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(false, this.id_solicitud, this.TramiteTarea, ucObservaciones.Text.Trim(),
                            ucObservacionPlancheta.Text.Trim(), userid, ucConsiderandoDispo.Text.Trim());

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. revision_gerente-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

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
            int cod_tarea = Convert.ToInt32(this.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Primer_Gestion_Documental);
            var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea);
            int id_tarea_GD = t != null ? t.id_tarea : 0;

            if (string.IsNullOrEmpty(ucObservaciones.Text.Trim()))
                throw new Exception("Debe agregar observaciones.");

            if (this.id_tarea == id_tarea_GD)
            {
                var list_doc_adj =
                    (
                        from adj in db.SGI_Tarea_Documentos_Adjuntos
                        where adj.id_tramitetarea == this.TramiteTarea
                        && adj.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Providencia_GestionDocumental
                        select new
                        {
                            adj
                        }
                    );
                if (list_doc_adj.Count() == 0)
                    throw new Exception("Debe subir la Providencia de Gestión Documental.");
            }
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                this.db = new DGHP_Entities();

                Validar_Finalizar();

                Guardar_tarea(true, this.id_solicitud, this.TramiteTarea, ucObservaciones.Text.Trim(),
                    ucObservacionPlancheta.Text.Trim(), userid, ucConsiderandoDispo.Text.Trim());

                // Actualiza los valores del resultado y proxima tarea
                var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == this.TramiteTarea);
                if (e.id_resultado.HasValue)
                    tt.id_resultado = e.id_resultado.Value;
                if (e.id_proxima_tarea.HasValue)
                    tt.id_proxima_tarea = e.id_proxima_tarea.Value;
                db.SGI_Tramites_Tareas.Attach(tt);
                db.Entry(tt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                if (!hayProcesosGenerados)
                {
                    db.SGI_Tarea_Gestion_Documental_GenerarProcesos(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();
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

                string message = Functions.GetErrorMessage(ex);
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

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
            ucResultadoTarea.btnFinalizar_Enabled = true;
        }
        #endregion
    }
}
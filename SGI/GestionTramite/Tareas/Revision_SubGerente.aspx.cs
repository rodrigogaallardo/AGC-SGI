using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using SGI.GestionTramite.Controls;
using SGI.Model;

namespace SGI.GestionTramite.Tareas
{
    public partial class Revision_SubGerente : System.Web.UI.Page
    {

        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Revision_SubGerente;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    CargarDatosTramite(id_tramitetarea);

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

        private void CargarDatosTramite(int id_tramitetarea)
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
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucObservacionPlancheta.Enabled = IsEditable;
            UcObservacionesContribuyente.Enabled = IsEditable;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;
            ucObservaciones.Enabled = IsEditable;
            ucSGI_ListaPlanoVisado.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;
            int id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

            SGI_Tarea_Revision_SubGerente subGenrente = Buscar_Tarea(id_tramitetarea);

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            bool mostarRespaldo = id_circuito == (int)Constants.ENG_Circuitos.AMP_ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.AMP_SCP5 ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_SCP5 ||
                id_circuito == (int)Constants.ENG_Circuitos.SCP5
                ? false : true;
            ucObservaciones.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea, mostarRespaldo);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                ucListaObservacionesAnteriores.Visible = false;
                UcObservacionesContribuyente.Visible = false;
            }
            else
            {
                ucObservaciones.Visible = false;
                ucListaObservacionesAnterioresv1.Visible = false;
            }

            ucListaRubros.Visible = true;
            ucTramitesRelacionados.Visible = true;
            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucListaResultadoTareasAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucSGI_ListaPlanoVisado.LoadData(this.id_solicitud, this.TramiteTarea);
            ucSGI_ListaPlanoVisado.Visible = (tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_SubGerente) ||
                                              tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_SubGerente2)) &&
                                              tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSP &&
                                                 tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSPA;

            var ssit_Sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this._id_solicitud);

            bool isConPlanos = false;
            isConPlanos = ssit_Sol.id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.ConPlanos;

            bool isSimple = false;
            isSimple = ssit_Sol.id_tipoexpediente == (int)Constants.TipoDeExpediente.Simple;

            bool isHP = false;
            isHP = ssit_Sol.id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.HabilitacionPrevia;

            if (isConPlanos)
                ucObservacionProvidencia.Height = 141;
            else
                ucObservacionProvidencia.Height = 205;

            if (subGenrente != null)
            {
                ucObservacionesTarea.Text = subGenrente.Observaciones;
                UcObservacionesContribuyente.Text = subGenrente.observaciones_contribuyente;
                ucObservacionPlancheta.Text = subGenrente.observacion_plancheta;
                ucObservacionProvidencia.Text = subGenrente.observacion_providencia;
            }
            else
            {
                ucObservacionesTarea.Text = "";
                UcObservacionesContribuyente.Text = "";
                ucObservacionPlancheta.Text = ObservacionAnteriores.Buscar_ObservacionPlancheta((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
                if (Functions.isAprobado(this.id_solicitud))
                    ucObservacionProvidencia.Text = string.Format(Parametros.GetParam_ValorChar("PROVIDENCIA.SUBGERENTE"), "\n\n\n", "\n\n", "se");
                else
                    ucObservacionProvidencia.Text = string.Format(Parametros.GetParam_ValorChar("PROVIDENCIA.SUBGERENTE"), "\n\n\n", "\n\n", "no se");

            }
            if (tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Revision_SubGerente_1 ||
                tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Revision_SubGerente_2 ||
                tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Revision_SubGerente_1 ||
                tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Revision_SubGerente_2)
            {
                ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                ucProcesosSADE.cargarDatosProcesos(id_tramitetarea, IsEditable);
                ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
                if (IsEditable)
                    ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);
            }
            this.db.Dispose();

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

        private SGI_Tarea_Revision_SubGerente Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Revision_SubGerente subGenrente =
                (
                    from sub_gere in db.SGI_Tarea_Revision_SubGerente
                    where sub_gere.id_tramitetarea == id_tramitetarea
                    orderby sub_gere.id_revision_subGerente descending
                    select sub_gere
                ).ToList().FirstOrDefault();

            return subGenrente;
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

        private void Guardar_tarea(bool finalizar, int id_solicitud, int id_tramite_tarea, string observacion, string observacion_plancheta, string observacion_providencia, string observacion_contribuyente, Guid userId)
        {

            SGI_Tarea_Revision_SubGerente subGenrente = Buscar_Tarea(id_tramite_tarea);

            int id_revision_subGerente = 0;
            if (subGenrente != null)
                id_revision_subGerente = subGenrente.id_revision_subGerente;

            db.SGI_Tarea_Revision_SubGerente_Actualizar(id_revision_subGerente, id_tramite_tarea, observacion, observacion_plancheta, observacion_providencia, observacion_contribuyente, userId);
            if (finalizar && !string.IsNullOrEmpty(observacion_contribuyente))
                db.SSIT_Solicitudes_AgregarObservaciones(id_solicitud, observacion_contribuyente, userId);

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
                        Guardar_tarea(false, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text.Trim(), ucObservacionPlancheta.Text.Trim(), ucObservacionProvidencia.Text.Trim(), UcObservacionesContribuyente.Text.Trim(), userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. revision_subgerente-ucResultadoTarea_GuardarClick");
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
            var tarea = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();

            int cod_tarea_rev = Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP);
            var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_rev);
            int id_tarea_rev = t != null ? t.id_tarea : 0;

            int cod_tarea_rev_ger = Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_Gerente2);
            t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_rev_ger);
            int id_tarea_rev_ger = t != null ? t.id_tarea : 0;

            int cod_tarea_dic_asig = Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Dictamen_Asignacon);
            t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_dic_asig);
            int id_tarea_dic_asig = t != null ? t.id_tarea : 0;

            if (ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.SCP_Correccion_Solicitud_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESP_Correccion_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESPAR_Correccion_Nuevo)
            {
                if (ucObservaciones.countObservaciones == 0)
                    throw new Exception("Debe especificar la Documentacion a Presentar.");
            }
            else if (ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.SSP_Revision_DGHP_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.SCP_Revision_DGHP_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESP_Generar_Ticket_Lisa_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESPAR_Generar_Ticket_Lisa_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESP_Dictamen_Asignar_Profesional_Nuevo ||
                ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESPAR_Dictamen_Asignar_Profesional_Nuevo)
            {
                if (ucObservaciones.countObservaciones != 0)
                    throw new Exception("No se debe especificar la Documentacion a Presentar.");
            }

            int archivo = (from tth in db.SGI_Tramites_Tareas_HAB
                           join doc in db.SGI_Tarea_Documentos_Adjuntos on tth.id_tramitetarea equals doc.id_tramitetarea
                           where tth.id_solicitud == id_solicitud && doc.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Plano_Visado
                           select doc).ToList().Count;

            //143479: JADHE YYYYY - SGI - Plano visado en Escuelas
            List<int> listCircuitosEscuelas = new List<int>();
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.SCP5);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.ESCU_HP);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.AMP_SCP5);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.AMP_ESCU_HP);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.RU_SCP5);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.RU_ESCU_HP);

            if (!listCircuitosEscuelas.Contains(tarea.id_circuito))
            {
                if ((tarea.cod_tarea == Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_SubGerente) ||
                tarea.cod_tarea == Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_SubGerente2)) &&
                tarea.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSP &&
                tarea.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSPA &&
                Functions.isAprobado(this.id_solicitud) &&
                (ucResultadoTarea.getIdProximaTarea() == id_tarea_rev_ger ||
                 ucResultadoTarea.getIdProximaTarea() == id_tarea_dic_asig) &&
                (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Ratifica_calificacion ||
                 ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Subgerente_Estoy_de_Acuerdo_con_el_Calificador ||
                 ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Retifica ||
                 ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Gerente_Estoy_de_Acuerdo_con_el_Calificador ||
                 ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Enviar_al_Gerente) &&
                (ucSGI_ListaPlanoVisado.getSeleccionPlanos() <= 0 ||
                (ucSGI_ListaPlanoVisado.getSeleccionPlanos() > 0 && archivo <= 0)))
                {
                    throw new Exception("Debe subir el archivo correspondiente a Plano Visado.");
                }
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

                Guardar_tarea(true, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text.Trim(), ucObservacionPlancheta.Text.Trim(), ucObservacionProvidencia.Text.Trim(), UcObservacionesContribuyente.Text.Trim(), userid);
                db.SaveChanges();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                bool sinProceso = false;
                //Esto con los nuevos circuitos no ira 
                if (this.id_tarea != (int)Constants.ENG_Tareas.ESCU_HP_Revision_SubGerente_1 &&
                    this.id_tarea != (int)Constants.ENG_Tareas.ESCU_HP_Revision_SubGerente_2 &&
                    this.id_tarea != (int)Constants.ENG_Tareas.ESCU_IP_Revision_SubGerente_1 &&
                    this.id_tarea != (int)Constants.ENG_Tareas.ESCU_IP_Revision_SubGerente_2)
                {
                    sinProceso = true;
                }
                if (!sinProceso && !hayProcesosGenerados)
                {
                    db.SGI_HAB_GenerarProcesos_SADE_v4(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || sinProceso || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
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
                            LogError.Write(ex, "Error en transaccion. revision_subgerente-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }

                    var plano = db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea == TramiteTarea).ToList();
                    if (plano != null &&
                        Functions.isAprobado(this.id_solicitud))
                    {
                        foreach (var item in plano)
                            db.Solicitud_planoVisado_Agregar(this.id_solicitud, id_tramitetarea_nuevo, userid, item.id_docAdjunto);
                    }

                    db.Dispose();

                    string mensaje_envio_mail = "";
                    try
                    {
                        if ((e.id_proxima_tarea == (int)Constants.ENG_Tareas.SSP_Revision_Gerente ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.SCP_Revision_Gerente ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Revision_Gerente_1 ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Revision_Gerente_2 ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_Gerente_1 ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_Gerente_2) && ucObservacionesTarea.Text.Trim() != "")
                            Mailer.MailMessages.SendMail_ObservacionSolicitud_v2(id_solicitud);
                        else if (e.id_resultado == (int)Constants.ENG_ResultadoTarea.Retifica
                            && e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Dictamen_Asignar_Profesional)
                            Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(id_solicitud);
                        else if (e.id_proxima_tarea == (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud_Nuevo ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.SCP_Correccion_Solicitud_Nuevo ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Correccion_Nuevo ||
                            e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Correccion_Nuevo ||
                             e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR2_Correccion_Solicitud)    
                            Mailer.MailMessages.SendMail_ObservacionSolicitud1_v2(id_solicitud);
                        else if (e.id_resultado == (int)Constants.ENG_ResultadoTarea.Retifica
                            && e.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Dictamen_Asignar_Profesional_Nuevo)
                            Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(id_solicitud);
                    }
                    catch (Exception ex)
                    {
                        mensaje_envio_mail = ex.Message;
                    }
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
                titulo = System.Web.HttpUtility.HtmlEncode("Revisíón SubGerente");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }
        #endregion
    }
}
using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas
{
    public partial class Enviar_AVH : BasePage
    {
        #region "Propiedades"


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
        

        private SGI_Tarea_Enviar_AVH Buscar_Tarea(int id_tramitetarea)
        {
            var pvh = (from env_phv in db.SGI_Tarea_Enviar_AVH
                       where env_phv.id_tramitetarea == id_tramitetarea
                       orderby env_phv.id_enviar_avh descending
                       select env_phv
                      ).FirstOrDefault();

            return pvh;
        }
        private string _url_servicio_EE;
        private string url_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_url_servicio_EE))
                {
                    _url_servicio_EE = Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                }
                return _url_servicio_EE;
            }
        }
        private string _username_servicio_EE;
        private string username_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_username_servicio_EE))
                {
                    _username_servicio_EE = Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
                }
                return _username_servicio_EE;
            }
        }
        private string _pass_servicio_EE;
        private string pass_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_pass_servicio_EE))
                {
                    _pass_servicio_EE = Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
                }
                return _pass_servicio_EE;
            }
        }

        #endregion

        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Enviar_PVH;

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

        private void CargarDatosTramite(int id_tramitetarea)
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
            this.id_tramitetarea = id_tramitetarea;
            this.id_paquete = (from p in db.SGI_Tarea_Generar_Expediente_Procesos
                               join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                               join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                               where tt_hab.id_solicitud == this.id_solicitud
                               select p.id_paquete)
                               .Union(
                                   from p in db.SGI_SADE_Procesos
                                   join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                   join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                   where tt_hab.id_solicitud == this.id_solicitud
                                   select p.id_paquete
                               ).FirstOrDefault();


            SGI_Tarea_Enviar_AVH pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";
            
            // Cargar controles correspondientes al informe gráfico.
            // ---
            txt_numeroGEDO_Tipo.Text = "IF";
            txt_numeroGEDO_Anio.Text = DateTime.Now.Year.ToString();
            txt_numeroGEDO_Reparticion.Text = "AGC";
            txt_numeroGEDO_Tipo.Enabled = IsEditable;
            txt_numeroGEDO_Anio.Enabled = IsEditable;
            txt_numeroGEDO_Nro.Enabled = IsEditable;
            txt_numeroGEDO_Reparticion.Enabled = IsEditable;
            btnObtener.Visible = IsEditable;
            cargarDocumentoGuardado(this.id_tramitetarea);

            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

        }

        private void cargarDocumentoGuardado( int id_tramitetarea)
        {
            
            IniciarEntity();
            pnlInformeGuardado.Visible = false;

            var tarea = db.SGI_Tarea_Enviar_AVH.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            
            if (tarea != null && tarea.id_docadjunto.HasValue)
            {
                lnkInformeGraficoGuardado.Text = tarea.numeroGEDO;
                lnkInformeGraficoGuardado.NavigateUrl = string.Format("~/ImprimirDocumentoAdjunto/{0}", tarea.id_docadjunto);
                pnlInformeGuardado.Visible = true;
            }
            
            FinalizarEntity();
        }

        #endregion


        #region acciones

        private void Redireccionar_VisorTramite()
        {
            //string url = string.Format("~/GestionTramite/VisorTramite.aspx?id={0}", this.id_solicitud);
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
            int id_enviar_avh = 0;
            SGI_Tarea_Enviar_AVH pvh = Buscar_Tarea(id_tramitetarea);
            
            if (pvh != null)
                id_enviar_avh = pvh.id_enviar_avh;

            db.SGI_Tarea_Enviar_AVH_Actualizar(id_enviar_avh, id_tramite_tarea, observacion, userId);

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
                        Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid);
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
            IniciarEntity();
            var tarea = db.SGI_Tarea_Enviar_AVH.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (tarea == null || !tarea.id_docadjunto.HasValue)
            {
                throw new Exception("Para poder finalizar la tarea debe haber obtnenido el IF correspondiente al informe de AVH.");
            }
            FinalizarEntity();
        }

    
        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {

                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;
                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;

                if (!hayProcesosGenerados)
                {
                    Validar_Finalizar();
                    db.SGI_Tarea_Enviar_AVH_GenerarProcesos(this.id_tramitetarea, this.id_paquete, userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, true);
                }
                else if (!ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {
                        
                    id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                    Enviar_Mensaje("Se ha finalizado la tarea.", "");

                    Redireccionar_VisorTramite();
                }
                else
                {
                    throw new Exception("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.");
                }

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updFinalizarTarea, "showfrmError();");
            }
            finally {
                db.Dispose();
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

        protected void btnObtener_Click(object sender, EventArgs e)
        {

            IniciarEntity();
            try
            {
                Guid userid = Functions.GetUserId();
                int id_tdocreq = (int)Constants.TiposDeDocumentosRequeridos.Informe_AVH;

                // Se llama a guardar porque si no esta creada la tarea esta función la crea.
                // --
                Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid);

                // Recupera la tarea en donde va a guardar el documento.
                // --
                SGI_Tarea_Enviar_AVH tarea = db.SGI_Tarea_Enviar_AVH.FirstOrDefault(x => x.id_tramitetarea == this.id_tramitetarea);

                if (tarea != null )
                {
                    string username_SADE = Functions.GetUsernameSADE(userid);
                    string numeroIF =  txt_numeroGEDO_Nro.Text.Trim().PadLeft(8,Convert.ToChar("0"));
                    string numeroGEDO = string.Format("{0}-{1}-{2}-   -{3}", txt_numeroGEDO_Tipo.Text.Trim(), txt_numeroGEDO_Anio.Text.Trim(),numeroIF,txt_numeroGEDO_Reparticion.Text.Trim());

                    if (username_SADE.Length == 0)
                        throw new Exception("Para poder utilizar esta funcionalidad debe tener asignado el nombre de usuario SADE dentro de la configuración de usuario. Comuníquese con sistemas.");
                    
                    // Recuperar el Informe de AVH.
                    // --
                    ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                    serviceEE.Url = this.url_servicio_EE;
                    SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);
                    string origen = Constants.ApplicationName;
                    byte[] documento = serviceEE.GetDocumentoPDF_SADE(this.username_servicio_EE, this.pass_servicio_EE, numeroGEDO, sol.NroExpedienteSade, origen, username_SADE);

                    if (documento.Length > 10)
                    {
                        
                        Webservices.SADE.documentos_oficiales.IAdministracionDeDocumentosOficialesService service_SADE_Docs = new Webservices.SADE.documentos_oficiales.IAdministracionDeDocumentosOficialesService();
                        service_SADE_Docs.Url = Functions.GetParametroCharEE("SADE.Host") + "/EEServices/documentos-oficiales";
                        Webservices.SADE.bloqueo_expediente.IBloqueoExpedienteService sevice_SADE_bloq = new Webservices.SADE.bloqueo_expediente.IBloqueoExpedienteService();
                        sevice_SADE_bloq.Url = Functions.GetParametroCharEE("SADE.Host") + "/EEServices/bloqueo-expediente";

                        try
                        {
                            if (!sevice_SADE_bloq.isBloqueado(sol.NroExpedienteSade))
                                sevice_SADE_bloq.bloquearExpediente(Constants.ApplicationName, sol.NroExpedienteSade);

                            // Esto se hizo de esta manera y esta mal, pero se dejo asi porque se debe hacer con la nueva pasarela
                            // en donde existan métodos que vayan directo contra SADE.
                            try
                            {
                                service_SADE_Docs.vincularDocumentosOficiales(Constants.ApplicationName, username_SADE, sol.NroExpedienteSade, new string[] { numeroGEDO });
                            }
                            catch (Exception ex)
                            {
                                // si el error es el del mensaje se deja continuar porque ya estaba vinculado
                                if(!ex.Message.ToLower().Contains("el documento ya se encuentra vinculado al expediente"))
                                    throw ex;
                            }


                            // --
                            ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                            db.SGI_Tarea_Enviar_AVH_ActualizarDocumento(tarea.id_enviar_avh, id_tdocreq, numeroGEDO, documento, userid, param_id_docadjunto);
                            db.SaveChanges();

                            
                        }
                        catch (Exception ex)
                        {
                            LogError.Write(ex);
                            throw new Exception(string.Format("SADE devolvió error al intentar vincular el documento {0} con el expediente {1}. Intente más tarde y se vuelve a repetir comuníquelo a sistemas. <br/> <b>Error SADE:</b> {2}", numeroGEDO, sol.NroExpedienteSade, ex.Message));
                        }
                        finally
                        {
                            service_SADE_Docs.Dispose();
                        }

                    }
                    else
                    {
                        throw new Exception("No se pudo recuperar el pdf del documento " + numeroGEDO);
                    }

                    Response.Redirect("~/GestionTramite/Tareas/Enviar_AVH.aspx?id=" + this.id_tramitetarea);
                }
                else
                {
                    throw new Exception("No se pudo obtener el documento en SADE, nro de documento: " + lbl_numeroGEDO);
                }
                
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updObtenerInforme, "showfrmError();");
            }
            finally
            {
                FinalizarEntity();
            }
            

        }

    }
}
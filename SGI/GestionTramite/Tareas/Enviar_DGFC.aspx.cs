using RestSharp;
using SGI.GestionTramite.Controls;
using SGI.Model;
using SGI.WebServices;
using SGI.WebServices.LIZA;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas
{
    public partial class Enviar_DGFC : BasePage
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

        private SGI_Tarea_Enviar_DGFC Buscar_Tarea(int id_tramitetarea)
        {
            var pvh = (from env_phv in db.SGI_Tarea_Enviar_DGFC
                       where env_phv.id_tramitetarea == id_tramitetarea
                       orderby env_phv.id_enviar_dgfc descending
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
            this.id_tramitetarea = id_tramitetarea;
            this.id_paquete = (from p in db.SGI_SADE_Procesos
                               join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                               join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                               where tt_hab.id_solicitud == this.id_solicitud
                               select p.id_paquete).FirstOrDefault();

            SGI_Tarea_Enviar_DGFC pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";

            SGI_LIZA_Ticket ticket= db.SGI_LIZA_Ticket.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            lbl_liza_alias.Text = "";
            if (ticket == null)
            {
                if (IsEditable)
                {
                    try
                    {
                       /* WS_Ticket wsTicket = generarTicket(this.id_solicitud);
                        ObjectParameter id_ticket = new ObjectParameter("id_ticket", typeof(int));
                        db.SGI_LIZA_Ticket_Agregar(id_tramitetarea, wsTicket.Alias, wsTicket.UrlSeguimiento, userid, id_ticket);
                        lbl_liza_alias.Text = wsTicket.Alias;*/
                    }
                    catch
                    {
                        lbl_liza_alias.Text = "No se pudo generar el ticket, intente mas tarde.";
                    }
                }
            } else
            {
                lbl_liza_alias.Text = ticket.Alias;
            }

            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);

        }

        private WS_Ticket generarTicket(int id_solicitud)
        {
            List<Item_doc> archivos = new List<Item_doc>();
            //Dispocision
            var doc = db.SSIT_DocumentosAdjuntos.Where(x => x.id_solicitud == id_solicitud && x.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.DISPOSICION_HABILITACION).FirstOrDefault();
            if (doc != null) {
                var a= new Item_doc
                {
                    nombre = "Disposicion.pdf",
                    documento = ws_FilesRest.DownloadFile(doc.id_file),
                    tipo = "application/pdf"
                };
                archivos.Add(a);
            }
            //Encomienda de la solicitud
            int id_tipodocsis = Functions.GetTipoDocSistema("ENCOMIENDA_DIGITAL");
            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

            var lstFilesEncomiendas = (from encdoc in db.Encomienda_DocumentosAdjuntos
                                       join file in db.Files on encdoc.id_file equals file.id_file
                                       where encdoc.id_encomienda == enc.id_encomienda && encdoc.id_tipodocsis == id_tipodocsis
                                       select new Item_doc
                                       {
                                           nombre = "Encomienda Digital.pdf",
                                           documento = file.content_file,
                                           tipo = "application/pdf"
                                       }
                                       ).ToList();
            if(lstFilesEncomiendas.Count>0)
                archivos.AddRange(lstFilesEncomiendas);

            //Planos
            var listPlanos = (from encPla in db.Encomienda_Planos
                              join file in db.Files on encPla.id_file equals file.id_file
                              where encPla.id_encomienda == enc.id_encomienda
                              select new Item_doc
                              {
                                  nombre = encPla.nombre_archivo,
                                  documento = file.content_file,
                                  tipo = encPla.nombre_archivo.Substring(encPla.nombre_archivo.Length - 3).ToLower().Equals("jpg") ? "image/jpeg" : "application/dwf"
                              }
                            ).ToList();
            if (listPlanos.Count > 0)
                archivos.AddRange(listPlanos);

            WS_Ticket wsTicket = WS_LizaRest.generarTicket(this.id_solicitud, archivos);
            return wsTicket;
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
            int id_enviar_dgfc = 0;
            SGI_Tarea_Enviar_DGFC tarea = Buscar_Tarea(id_tramitetarea);

            if (tarea != null)
                id_enviar_dgfc = tarea.id_enviar_dgfc;

            db.SGI_Tarea_Enviar_DGFC_Actualizar(id_enviar_dgfc, id_tramite_tarea, observacion, userId);
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
            /*SGI_LIZA_Ticket ticket = db.SGI_LIZA_Ticket.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if(ticket== null)
                throw new Exception("No se puede finalizar la tarea si no se genero el ticket.");*/
        }


        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                this.db = new DGHP_Entities();

                Validar_Finalizar();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;

                if (!hayProcesosGenerados)
                {
                    db.SGI_Tarea_Enviar_DGFC_GenerarProcesos(this.id_tramitetarea, this.id_paquete, userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, true);
                }
                else if (!ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))
                {


                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            Guardar_tarea(this.id_tramitetarea, ucObservacionesTarea.Text, userid);
                            db.SaveChanges();

                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. rechazo_en_sadeucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }
                    db.Dispose();

                    string mensaje_envio_mail = "";
                    try
                    {
                        Mailer.MailMessages.SendMail_RechazoSolicitud_v2(id_solicitud);
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

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
            ucResultadoTarea.btnFinalizar_Enabled = true;
        }
    }
}
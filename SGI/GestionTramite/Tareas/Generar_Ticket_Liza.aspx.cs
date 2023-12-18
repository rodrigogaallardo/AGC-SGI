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
    public partial class Generar_Ticket_Liza : System.Web.UI.Page
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
            IniciarEntityFiles();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Generar_Ticket_Liza pvh = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesTarea.Text = (pvh != null) ? pvh.Observaciones : "";

            SGI_LIZA_Ticket ticket = db.SGI_LIZA_Ticket.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            lbl_liza_alias.Text = "";
            if (ticket == null)
            {
                if (IsEditable)
                {
                    try
                    {
                        WS_Ticket wsTicket = generarTicket(this.id_solicitud);
                        ObjectParameter id_ticket = new ObjectParameter("id_ticket", typeof(int));
                        db.SGI_LIZA_Ticket_Agregar(id_tramitetarea, wsTicket.Alias, wsTicket.UrlSeguimiento, userid, id_ticket);
                        lbl_liza_alias.Text = wsTicket.Alias;
                    }
                    catch(Exception ex)
                    {
                        lbl_liza_alias.Text = "No se pudo generar el ticket, intente mas tarde.";
                    }
                }
            }
            else
            {
                lbl_liza_alias.Text = ticket.Alias;
            }
        }

        private WS_Ticket generarTicket(int id_solicitud)
        {
            List<TiposDeDocumentosRequeridos> list_tipo_doc = (
                from tdoc in db.TiposDeDocumentosRequeridos
                select tdoc
            ).ToList();

            List<Item_doc> archivos = new List<Item_doc>();
            //Recupero los archivos adjunto del calificador
            var list_doc_adj =
                (
                    from adj in db.SGI_Tarea_Documentos_Adjuntos
                    join th in db.SGI_Tramites_Tareas_HAB on adj.id_tramitetarea equals th.id_tramitetarea
                    where th.id_solicitud == id_solicitud
                    select new
                    {
                        adj.id_tdocreq,
                        adj.id_file
                    }
                ).ToList();
            Item_doc doc_adj = null;
            foreach (var item in list_doc_adj)
            {
                doc_adj = new Item_doc();
                doc_adj.documento = ws_FilesRest.DownloadFile(item.id_file);
                var tipo_doc = list_tipo_doc.FirstOrDefault(x => x.id_tdocreq == item.id_tdocreq);
                if (tipo_doc != null)
                    doc_adj.nombre = tipo_doc.nombre_tdocreq + ".pdf";
                doc_adj.tipo = "application/pdf";
                archivos.Add(doc_adj);
            }

            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

            //Encomienda de la solicitud
            int id_tipodocsis = Functions.GetTipoDocSistema("ENCOMIENDA_DIGITAL");
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

        private SGI_Tarea_Generar_Ticket_Liza Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Generar_Ticket_Liza pvh =
                (
                    from env_phv in db.SGI_Tarea_Generar_Ticket_Liza
                    where env_phv.id_tramitetarea == id_tramitetarea
                    orderby env_phv.id_generar_ticket_liza descending
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

            SGI_Tarea_Generar_Ticket_Liza pvh = Buscar_Tarea(id_tramite_tarea);

            int id_enviar_avh = 0;
            if (pvh != null)
                id_enviar_avh = pvh.id_generar_ticket_liza;

            db.SGI_Tarea_Generar_Ticket_Liza_Actualizar(id_enviar_avh, id_tramite_tarea, observacion, userId);

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
            SGI_LIZA_Ticket ticket = db.SGI_LIZA_Ticket.FirstOrDefault(x => x.id_tramitetarea == TramiteTarea);
            if (ticket == null)
                throw new Exception("No se puede finalizar la tarea si no se genero el ticket.");
        }


        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                TransactionScope Tran = new TransactionScope();

                try
                {
                    Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                    db.SaveChanges();

                    id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

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

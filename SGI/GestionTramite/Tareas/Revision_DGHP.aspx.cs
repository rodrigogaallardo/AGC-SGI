using System;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using SGI.GestionTramite.Controls;
using SGI.Model;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace SGI.GestionTramite.Tareas
{
    public partial class Revision_DGHP : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Revision_DGHP;

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
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid) ;
            
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable && !hayProcesosGenerados;
            ucObservacionPlancheta.Enabled = IsEditable && !hayProcesosGenerados;
            ucConsiderandoDispo.Enabled = IsEditable && !hayProcesosGenerados;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable && !hayProcesosGenerados;
            ucSGI_ListaPlanoVisado.Enabled = IsEditable && !hayProcesosGenerados;
            ucSGI_ListaDocumentoAdjuntoAnt.Enabled = IsEditable && !hayProcesosGenerados;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            this.id_tarea = tramite_tarea.id_tarea;
            this.id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

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
                                   select p.id_paquete).FirstOrDefault();

            SGI_Tarea_Revision_DGHP rev_dghp = Buscar_Tarea(id_tramitetarea);
            var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);


            if (sol.id_estado != (int)Constants.Solicitud_Estados.RevCaducidad &&
                sol.id_estado != (int)Constants.Solicitud_Estados.RevRechazo &&
                Functions.isAprobado(this.id_solicitud) &&
                !db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea == this.TramiteTarea).Any() &&
               (id_circuito == (int)Constants.ENG_Circuitos.ESPAR2 ||
                id_circuito == (int)Constants.ENG_Circuitos.AMP_ESPAR2 ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_ESPAR2))
            {
                int idTT = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == id_solicitud &&
                            (x.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == 14 ||
                             x.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == 15)).
                            Select(y => y.id_tramitetarea).Max();

                var plano = db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea == idTT).ToList();
                if (plano != null)
                {
                    foreach (var item in plano)
                        db.Solicitud_planoVisado_Agregar(this.id_solicitud, this.TramiteTarea, userid, item.id_docAdjunto);
                }
            }

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucSGI_ListaDocumentoAdjuntoAnt.LoadData(id_grupotramite, this.id_solicitud, this.TramiteTarea);
            ucListaResultadoTareasAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucSGI_ListaPlanoVisado.LoadData(this.id_solicitud, this.TramiteTarea);
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            //ucSGI_DocumentoAdjunto.Visible = tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP2) ||
            //(tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP) &&
            // tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSP &&
            // tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSPA);

            ucSGI_ListaPlanoVisado.Visible = (tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP) &&
                                              tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSP &&
                                              tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSPA);

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                ucListaObservacionesAnteriores.Visible = false;
            }
            else
            {
                ucListaObservacionesAnterioresv1.Visible = false;
            }

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucListaRubros.Visible = true;
            ucTramitesRelacionados.Visible = true;

            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);

            ucObservacionesTarea.Text = (rev_dghp != null) ? rev_dghp.Observaciones : "";
            ucObservacionPlancheta.Text = (rev_dghp != null) ? rev_dghp.observacion_plancheta 
                : ObservacionAnteriores.Buscar_ObservacionPlancheta((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea); ;
            ucConsiderandoDispo.Text = Buscar_Considerando(this.id_solicitud, id_tramitetarea);
            //Esto con los nuevos circuitos no ira 

            int cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP2);
            var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
            int id_tarea_ger1 = t != null ? t.id_tarea : 0;

            cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP3);
            t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
            var id_tarea_ger = t != null ? t.id_tarea : 0;

            if (id_circuito == (int)Constants.ENG_Circuitos.ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.AMP_ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.SCP5 ||
                id_circuito == (int)Constants.ENG_Circuitos.AMP_SCP5 ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_SCP5 ||
                id_tarea == id_tarea_ger ||
                id_tarea == id_tarea_ger1)
            {
                ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
                ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);
            }

            if (id_tarea != id_tarea_ger &&
                id_tarea != id_tarea_ger1)
                ucConsiderandoDispo.Visible = false;

            FinalizarEntity();
        }

        private string Buscar_Considerando(int id_solicitud, int id_tramitetarea)
        {
            string considerando_dispo = "";

            SGI_Tarea_Revision_DGHP rev_dghp = Buscar_Tarea(id_tramitetarea);

            if (rev_dghp != null)
                considerando_dispo = db.SGI_Tramites_Tareas_Dispo_Considerando.Where(x => x.id_tramitetarea == id_tramitetarea).Select(y => y.considerando_dispo).FirstOrDefault();
            else
            {
                var tarea = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();
                string tipo_tarea = tarea.cod_tarea.ToString();
                tipo_tarea = tipo_tarea.Substring(tipo_tarea.Length - 2);
                List<int> tareas = new List<int>();

                var cod_tarea = int.Parse(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Gestion_Documental);
                var t = db.ENG_Tareas.Where(x => x.cod_tarea == cod_tarea).FirstOrDefault();
                if (t != null)
                    tareas.Add(t.id_tarea);

                List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea, tareas.ToArray());

                if (list_tramite_tarea.Count > 0)
                {
                    int id_tramitetarea_gestion = 0;
                    id_tramitetarea_gestion = list_tramite_tarea[0].id_tramitetarea;

                    var q = (from sub in db.SGI_Tramites_Tareas_Dispo_Considerando
                             where sub.id_tramitetarea == id_tramitetarea_gestion
                             select new
                             {
                                 considerando = sub.considerando_dispo
                             }
                             ).FirstOrDefault();
                    if (q != null)
                        considerando_dispo = q.considerando; ;

                }

                //Grabo el considerando para la tarea Rev_dghp
                SGI_Tramites_Tareas_Dispo_Considerando text_dispo = db.SGI_Tramites_Tareas_Dispo_Considerando.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();
                int id_sgi_tt_dispo = 0;
                if (text_dispo != null)
                    id_sgi_tt_dispo = text_dispo.id_tt_Dispo_Considerando;

                db.SGI_Tramites_Tareas_Dispo_Considerando_Actualizar(id_sgi_tt_dispo, id_tramitetarea, considerando_dispo);
            }
            return considerando_dispo;
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

        private int id_tarea
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_tarea"] != null ? Convert.ToInt32(ViewState["_id_tarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_tarea"] = value;
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

        private SGI_Tarea_Revision_DGHP Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Revision_DGHP rev_dghp =
                (
                    from dghp in db.SGI_Tarea_Revision_DGHP
                    where dghp.id_tramitetarea == id_tramitetarea
                    orderby dghp.id_revision_dghp descending
                    select dghp
                ).ToList().FirstOrDefault();

            return rev_dghp;
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
            var sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).FirstOrDefault();
            int archivo = (from tth in db.SGI_Tramites_Tareas_HAB
                           join doc in db.SGI_Tarea_Documentos_Adjuntos on tth.id_tramitetarea equals doc.id_tramitetarea
                           where tth.id_solicitud == id_solicitud && doc.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Plano_Visado
                           select doc).ToList().Count;

            var tarea = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();
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
                if (sol.id_estado != (int)Constants.Solicitud_Estados.RevCaducidad &&
                    sol.id_estado != (int)Constants.Solicitud_Estados.RevRechazo &&
                tarea.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP) &&
                Functions.isAprobado(this.id_solicitud) &&
                (ucSGI_ListaPlanoVisado.getSeleccionPlanos() <= 0 ||
                (ucSGI_ListaPlanoVisado.getSeleccionPlanos() > 0 && archivo <= 0)))
                {
                    throw new Exception("Debe seleccionar el archivo correspondiente a Plano Visado.");
                }
            }
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, string observacion_plancheta, Guid userId)
        {

            SGI_Tarea_Revision_DGHP rev_dghp = Buscar_Tarea(id_tramite_tarea);

            int id_revision_dghp = 0;
            if (rev_dghp != null)
                id_revision_dghp = rev_dghp.id_revision_dghp;

            db.SGI_Tarea_Revision_DGHP_Actualizar(id_revision_dghp, id_tramite_tarea, observacion, observacion_plancheta, userId);

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
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, ucObservacionPlancheta.Text, userid);
                        var tar = db.SGI_Tramites_Tareas.First(x => x.id_tramitetarea == this.TramiteTarea);
                        tar.id_resultado = ucResultadoTarea.getIdResultadoTarea();
                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. revision_dghp-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

                }
                FinalizarEntity();

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, ucObservacionPlancheta.Text, userid);

                var tar = db.SGI_Tramites_Tareas.First(x => x.id_tramitetarea == this.TramiteTarea);
                tar.id_resultado = ucResultadoTarea.getIdResultadoTarea();
                tar.id_proxima_tarea = ucResultadoTarea.getIdProximaTarea();
                db.SGI_Tramites_Tareas.Attach(tar);
                db.Entry(tar).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                int cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP2);
                var t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
                int id_tarea_ger1 = t != null ? t.id_tarea : 0;

                cod_tarea_ger = Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP3);
                t = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == cod_tarea_ger);
                int id_tarea_ger = t != null ? t.id_tarea : 0;

                bool sinProceso = false;
                //Esto con los nuevos circuitos no ira 
                if (this.id_circuito != (int)Constants.ENG_Circuitos.ESCU_HP &&
                    this.id_circuito != (int)Constants.ENG_Circuitos.AMP_ESCU_HP &&
                    this.id_circuito != (int)Constants.ENG_Circuitos.RU_ESCU_HP &&
                    this.id_circuito != (int)Constants.ENG_Circuitos.SCP5 &&
                    this.id_circuito != (int)Constants.ENG_Circuitos.AMP_SCP5 &&
                    this.id_circuito != (int)Constants.ENG_Circuitos.RU_SCP5 &&
                    this.id_tarea != id_tarea_ger &&
                    this.id_tarea != id_tarea_ger1)
                {
                    sinProceso = true;
                }

                if (!sinProceso && !hayProcesosGenerados)
                {
                    if (this.id_tarea == id_tarea_ger)
                        db.SGI_Tarea_Revision_DGHP_GenerarProcesos_Reconsideracion(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    else if (this.id_tarea == id_tarea_ger1)
                        db.SGI_Tarea_Revision_DGHP_GenerarProcesos_Reconsideracion_1(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    else
                        db.SGI_Tarea_Revision_DGHP_GenerarProcesos_v4(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || sinProceso || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {

                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            db.SaveChanges();

                            id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();

                            Enviar_Mensaje("Se ha finalizado la tarea.", "");
                            Redireccionar_VisorTramite();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. revision_dghp-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
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
                this.dbFiles.Dispose();
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
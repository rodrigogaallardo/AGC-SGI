using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using SGI.GestionTramite.Controls;
using SGI.Model;

namespace SGI.GestionTramite.Tareas
{
    public partial class Ejecutiva : System.Web.UI.Page
    {
        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Revision_Gerente;

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
            ucObservaciones.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.id_tramitetarea = id_tramitetarea;
            this.id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;

            SGI_Tarea_Ejecutiva gerente = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);

            ucListaRubros.Visible = true;
            ucTramitesRelacionados.Visible = true;
            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);

            if (gerente != null)
                ucObservaciones.Text = gerente.Observaciones;
            else
                ucObservaciones.Text = "";
            var list = db.SGI_Tarea_Ejecutiva_NumeroGedo.Where(x => x.id_tramitetarea == id_tramitetarea);
            bool primero = true;
            foreach(var n in list)
            {
                if(primero)
                    txtNumeroGedo1.Text = n.numero_gedo;
                else
                    txtNumeroGedo2.Text = n.numero_gedo;
                primero = false;
            }

            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            if (IsEditable && !hayProcesosGenerados)
            {
                try
                {
                    db.SGI_Tarea_Ejecutiva_GenerarProcesos_enviar(this.id_tramitetarea, 1, userid);
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Error procesos");
                    //this.EjecutarScript(updCargarProcesos, "showfrmError();");
                    IsEditable = false;
                }
            }

            ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
            ucProcesosSADE.cargarDatosProcesos(tramite_tarea.id_tramitetarea, IsEditable);
            ucResultadoTarea.btnFinalizar_Enabled = IsEditable && (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea));

            this.db.Dispose();
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
        private SGI_Tarea_Ejecutiva Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Ejecutiva gerente =
                (
                    from gere in db.SGI_Tarea_Ejecutiva
                    where gere.id_tramitetarea == id_tramitetarea
                    orderby gere.id_ejecutiva descending
                    select gere
                ).ToList().FirstOrDefault();

            return gerente;
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
            SGI_Tarea_Ejecutiva gerente = Buscar_Tarea(id_tramite_tarea);

            int id_ejecutiva = 0;
            if (gerente != null)
                id_ejecutiva = gerente.id_ejecutiva;

            db.SGI_Tarea_Ejecutiva_Actualizar(id_ejecutiva, id_tramite_tarea, observacion, userId);
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
                        Guardar_tarea(this.id_tramitetarea, ucObservaciones.Text.Trim(), userid);
                        Guardar_tarea_NumerosGedos(this.id_tramitetarea, txtNumeroGedo1.Text.Trim(), txtNumeroGedo2.Text.Trim(), userid);

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

        private void Guardar_tarea_NumerosGedos(int id_tramitetarea, string numerogedo1, string numerogedo2, Guid userid)
        {
            var list = db.SGI_Tarea_Ejecutiva_NumeroGedo.Where(x => x.id_tramitetarea == id_tramitetarea);

            //borro los q no estan
            foreach (var i in list)
            {
                if (i.numero_gedo != numerogedo1 && i.numero_gedo != numerogedo2)
                    db.SGI_Tarea_Ejecutiva_NumeroGedo_Delete(i.id_ejecutiva_numeroGedo);
            }
            var numero = db.SGI_Tarea_Ejecutiva_NumeroGedo.Where(x => x.numero_gedo == numerogedo1).FirstOrDefault();
            if (numero == null)
                db.SGI_Tarea_Ejecutiva_NumeroGedo_Insert(id_tramitetarea, numerogedo1, userid);
            numero = db.SGI_Tarea_Ejecutiva_NumeroGedo.Where(x => x.numero_gedo == numerogedo2).FirstOrDefault();
            if (numero == null)
                db.SGI_Tarea_Ejecutiva_NumeroGedo_Insert(id_tramitetarea, numerogedo2, userid);
        }

        private void Validar_Finalizar()
        {
            if(string.IsNullOrEmpty(txtNumeroGedo1.Text.Trim()) && string.IsNullOrEmpty(txtNumeroGedo2.Text.Trim()))
                throw new Exception("Debe ingresar al menos un número de documento.");
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Validar_Finalizar();

                Guardar_tarea(this.id_tramitetarea, ucObservaciones.Text.Trim(), userid);
                Guardar_tarea_NumerosGedos(this.id_tramitetarea, txtNumeroGedo1.Text.Trim(), txtNumeroGedo2.Text.Trim(), userid);
                db.SaveChanges();

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == this.id_tramitetarea) > 1;

                if (!hayProcesosGenerados)
                {
                    this.db.SGI_Tarea_Ejecutiva_GenerarProcesos(this.id_tramitetarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.id_tramitetarea, true);
                }
                else if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(this.id_tramitetarea))

                {
                    ucResultadoTarea.FinalizarTarea();

                    FinalizarEntity();

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
                titulo = System.Web.HttpUtility.HtmlEncode( this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }
        #endregion
        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }
        protected void ucProcesosSADE_FinalizadoEnSADE_inic(object sender, EventArgs e)
        {
        }
    }
}
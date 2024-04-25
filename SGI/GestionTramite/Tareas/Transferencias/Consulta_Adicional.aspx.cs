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

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Consulta_Adicional : Page
    {
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
            this.Title = tramite_tarea.ENG_Tareas.nombre_tarea;
            if (tramite_tarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }
            //Se debe establecer siempre el estado de controles antes del load de los controles
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucObservacionPlancheta.Enabled = IsEditable;
            ucDocumentoAdjunto.Enabled = IsEditable;
            ucObservaciones.Enabled = IsEditable;
            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttTR.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            SGI_Tarea_Consulta_Adicional cons_adic = Buscar_Tarea(id_tramitetarea);
            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucTitulares.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucPreviewDocumentos.LoadData(this.id_solicitud, (int)Constants.TipoDeTramite.Transferencia);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucListaResultadoTareasAnteriores.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucDocumentoAdjunto.LoadData(id_grupotramite, this._id_solicitud, id_tramitetarea);
            ucObservaciones.Visible = false;
            ucObservaciones.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea, false);
            ucObservaciones.Visible = true;
            UcObservacionesContribuyente.Visible = false;
            ucListaObservacionesAnteriores.Visible = false;
            if (cons_adic != null)
            {
                ucObservacionesTarea.Text = ucObservacionesTarea.LoadObservacionesInternas(id_tramitetarea, id_solicitud);
                ucObservacionPlancheta.Text = ucObservacionesTarea.LoadObservacionesPlancheta(id_tramitetarea, id_solicitud);
                UcObservacionesContribuyente.Text = cons_adic.observaciones_contribuyente;
                ucObservacionProvidencia.Text = cons_adic.observacion_providencia;
            }
            else
            {
                ucObservacionesTarea.Text = ucObservacionesTarea.LoadObservacionesInternas(id_tramitetarea, id_solicitud);
                UcObservacionesContribuyente.Text = "";
                ucObservacionPlancheta.Text = ObservacionAnteriores.Buscar_ObservacionPlancheta((int)Constants.GruposDeTramite.TR, id_solicitud, id_tramitetarea);
                ucObservacionProvidencia.Height = 180;
                if (Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Aprobado)
                    ucObservacionProvidencia.Text = string.Format(Parametros.GetParam_ValorChar("PROVIDENCIA.GERENTE.TRANSMISIONES"), "\n\n\n", "\n\n", "se", "\n\n");
                else
                    ucObservacionProvidencia.Text = string.Format(Parametros.GetParam_ValorChar("PROVIDENCIA.GERENTE.TRANSMISIONES"), "\n\n\n", "\n\n", "no se", "\n\n");
            }
            this.db.Dispose();
        }

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

        private int _tramiteTarea = 0;

        public int TramiteTarea
        {
            get
            {
                if (_tramiteTarea == 0)
                    int.TryParse(hid_id_tramitetarea.Value, out _tramiteTarea);
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
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        private SGI_Tarea_Consulta_Adicional Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Consulta_Adicional cons_adic = (from ca in db.SGI_Tarea_Consulta_Adicional
                                                      where ca.id_tramitetarea == id_tramitetarea
                                                      orderby ca.id_consulta_adicional descending
                                                      select ca).ToList().FirstOrDefault();
            return cons_adic;
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

        private void Guardar_tarea(bool finalizar, int id_solicitud, int id_tramite_tarea, string observaciones, string observacion_plancheta, string observacion_providencia, string observacion_contribuyente, Guid userId)
        {
            SGI_Tarea_Consulta_Adicional cons_adic = Buscar_Tarea(id_tramite_tarea);
            int id_consulta_adicional = 0;
            if (cons_adic != null)
                id_consulta_adicional = cons_adic.id_consulta_adicional;
            db.SGI_Tarea_Consulta_Adicional_Actualizar(id_consulta_adicional, id_tramite_tarea, observaciones, observacion_plancheta, observacion_providencia, observacion_contribuyente, userId);
        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                this.db = new DGHP_Entities();
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

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;
                this.db = new DGHP_Entities();
                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        Guardar_tarea(true, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text.Trim(), ucObservacionPlancheta.Text.Trim(), ucObservacionProvidencia.Text.Trim(), UcObservacionesContribuyente.Text.Trim(), userid);
                        db.SaveChanges();
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
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            int nroTrReferencia;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
            if (this.id_solicitud > nroTrReferencia)
            {
                if (Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Aprobado)
                    ucObservacionProvidencia.Text = string.Format(Parametros.GetParam_ValorChar("PROVIDENCIA.GERENTE.TRANSMISIONES"), "\n\n\n", "\n\n", "se", "\n\n");
                else
                    ucObservacionProvidencia.Text = string.Format(Parametros.GetParam_ValorChar("PROVIDENCIA.GERENTE.TRANSMISIONES"), "\n\n\n", "\n\n", "no se", "\n\n");
                 ucObservacionProvidencia.Update();
            }
            FinalizarEntity();
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }
    }
}
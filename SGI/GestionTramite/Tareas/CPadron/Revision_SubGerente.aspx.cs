using SGI.GestionTramite.Controls;
using SGI.GestionTramite.Controls.CPadron;
using SGI.Mailer;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Tareas.CPadron
{
    public partial class Revision_SubGerente : BasePage
    {
        #region cargar inicial
        private int id_solicitud
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_solicitud.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
            }
        }

        private int id_tramitetarea
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_tramitetarea.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                string scripts = Tabs_Tramite.scriptCarga(false, false);
                this.EjecutarScript(updCargaTramite, "finalizarCarga();" + scripts);
            } 
            
            if (!IsPostBack)
            {
                this.id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            CargarDatosTramite(this.id_tramitetarea);
        }

        private void CargarDatosTramite(int id_tramitetarea)
        {
            Guid userid = Functions.GetUserId();

            this.db = new DGHP_Entities();

            SGI_Tramites_Tareas cpadrontarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            
            if (cpadrontarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro en la tabla SGI_CPadron_Tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }
            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.CP;
            SGI_Tramites_Tareas_CPADRON ttCP = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttCP.id_cpadron;

            ucCabecera.LoadData(id_grupotramite, id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            Tabs_Tramite.editar = false;
            Tabs_Tramite.LoadData(this.id_solicitud,0, 1);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);

            string scripts = Tabs_Tramite.scriptCarga(false, false);
            this.EjecutarScript(updCargaTramite, "finalizarCarga();" + scripts);

            ucObservacionesAnteriores.LoadData(id_grupotramite, id_solicitud, id_tramitetarea, ttCP.SGI_Tramites_Tareas.id_tarea);
            
            
            SGI_Tarea_Revision_SubGerente carga = Buscar_Tarea(id_tramitetarea);
            if (carga != null)
            {
                ucObservacionesTarea.Text = carga.Observaciones;
            }
            else
            {
                ucObservacionesTarea.Text = ObservacionAnteriores.Buscar_ObservacionPlancheta((int)Constants.GruposDeTramite.CP, id_solicitud, id_tramitetarea);
            }

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

        private SGI_Tarea_Revision_SubGerente Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Revision_SubGerente tarea =
                (
                    from gere in db.SGI_Tarea_Revision_SubGerente
                    where gere.id_tramitetarea == id_tramitetarea
                    orderby gere.id_revision_subGerente descending
                    select gere
                ).ToList().FirstOrDefault();

            return tarea;
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

        private void Guardar_tarea(int id_cpadron, int id_tramitetarea, string observacion, Guid userId)
        {

            SGI_Tarea_Revision_SubGerente tarea = Buscar_Tarea(id_tramitetarea);

            int id_tarea = 0;
            if (tarea != null)
                id_tarea = tarea.id_revision_subGerente;

            db.SGI_Tarea_Revision_SubGerente_Actualizar(id_tarea, id_tramitetarea, observacion, null, null, null, null, userId, null);


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
                        Guardar_tarea(this.id_solicitud, this.id_tramitetarea, ucObservacionesTarea.Text.Trim(), userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. carga_tramite-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

                }
                db.Dispose();

                //Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updCargaTramite, "showfrmError_Carga();");

            }

        }

        private bool Validar_Finalizar()
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {
                Guid userid = Functions.GetUserId();


                ObjectParameter param_msgError = new ObjectParameter("MsgError", typeof(string));

                db.SGI_CPadron_Solicitudes_VerificarCarga(this.id_solicitud, param_msgError);
                string ms = param_msgError.Value.ToString();

                if (ms.Length > 0)
                {
                    lblError.Text = ms;
                    Functions.EjecutarScript(updCargaTramite, "showfrmError_Carga();");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updCargaTramite, "showfrmError_Carga();");
                LogError.Write(ex, "Error en transaccion. carga_tramite-ucResultadoTarea_Validar_Finalizar");
                throw ex;
            }
            finally
            {
                db.Dispose();
            }
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_cpadrontarea_nuevo = 0;

                if (Validar_Finalizar())
                {

                    this.db = new DGHP_Entities();
                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            Guardar_tarea(this.id_solicitud, this.id_tramitetarea, ucObservacionesTarea.Text.Trim(), userid);

                            db.SaveChanges();

                            id_cpadrontarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. carga_tramite-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }
                    string mensaje_envio_mail = "";
                    try
                    {
                        if (e.id_proxima_tarea == (int)Constants.ENG_Tareas.CP_Generar_Expediente)
                        {
                            MailMessages.SendMail_CP_Generar_Expediente_v2(id_solicitud);
                        }
                    }
                    catch (Exception ex)
                    {
                        mensaje_envio_mail = ex.Message;
                    }
                    db.Dispose();
                    Redireccionar_VisorTramite();
                }

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updCargaTramite, "showfrmError_Carga();");
            }

        }

        #endregion
    }
}
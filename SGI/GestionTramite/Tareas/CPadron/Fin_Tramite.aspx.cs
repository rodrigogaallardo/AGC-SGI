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

namespace SGI.GestionTramite.Tareas.CPadron
{
    public partial class Fin_Tramite : BasePage
    {
        #region cargar inicial

        private int id_solicitud
        {
            get
            {
                int ret = (ViewState["_id_solicitud"] != null ? Convert.ToInt32(ViewState["_id_solicitud"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_solicitud"] = value.ToString();
            }
        }

        private int id_cpadrontarea
        {
            get
            {
                int ret = (ViewState["_id_cpadrontarea"] != null ? Convert.ToInt32(ViewState["_id_cpadrontarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_cpadrontarea"] = value.ToString();
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
                this.id_cpadrontarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        protected async Task btnCargarDatos_Click(object sender, EventArgs e)
        {
            await CargarDatosTramite(this.id_cpadrontarea);
        }

        private async Task CargarDatosTramite(int id_tramitetarea)
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
            bool IsEditable = Engine.CheckEditTarea(id_cpadrontarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucObservacionContribuyente.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.CP;
            SGI_Tramites_Tareas_CPADRON ttCP = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttCP.id_cpadron;

            SGI_Tarea_Fin_Tramite fin = Buscar_Tarea(id_cpadrontarea);

            ucCabecera.LoadData(id_grupotramite, id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            Tabs_Tramite.editar = false;
            Tabs_Tramite.LoadData(this.id_solicitud,0, 1);
            ucResultadoTarea.LoadData(id_grupotramite, id_cpadrontarea, true);
            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea, cpadrontarea.id_tarea); //tramite_tarea.id_solicitud, id_tramitetarea, tarea_pagina);

            string scripts = Tabs_Tramite.scriptCarga(false, false);
            this.EjecutarScript(updCargaTramite, "finalizarCarga();" + scripts);

            if (fin != null)
            {
                ucObservacionesTarea.Text = fin.Observaciones;
                //ucObservacionContribuyente.Text = fin.Observaciones_contribuyente;
            }
            else
            {
                ucObservacionesTarea.Text = "";
                ucObservacionContribuyente.Text = "";
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

        private SGI_Tarea_Fin_Tramite Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Fin_Tramite tarea =
                (
                    from gere in db.SGI_Tarea_Fin_Tramite
                    where gere.id_tramitetarea == id_tramitetarea
                    orderby gere.id_Fin_Tramite descending
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

        private void Guardar_tarea(bool finalizar, int id_cpadron, int id_cpadrontarea, string observacion, string observContribuyente, Guid userId)
        {
            SGI_Tarea_Fin_Tramite tarea = Buscar_Tarea(id_cpadrontarea);
            int id_tarea = 0;
            if (tarea != null)
                id_tarea = tarea.id_Fin_Tramite;
            db.SGI_Tarea_Fin_Tramite_Actualizar(id_tarea, id_cpadrontarea, observacion, userId);
            if (finalizar && !string.IsNullOrEmpty(observContribuyente))
                db.CPadron_Solicitudes_AgregarObservaciones(id_cpadron, observContribuyente, userId);
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
                        Guardar_tarea(false, this.id_solicitud, this.id_cpadrontarea, ucObservacionesTarea.Text.Trim(), ucObservacionContribuyente.Text.Trim(), userid);

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
            return true;
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
                            Guardar_tarea(true, this.id_solicitud, this.id_cpadrontarea, ucObservacionesTarea.Text.Trim(), ucObservacionContribuyente.Text.Trim(), userid);
                            
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
using SGI.GestionTramite.Controls;
using SGI.GestionTramite.Controls.CPadron;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.WebServices;

namespace SGI.GestionTramite.Tareas.Transferencias
{
    public partial class Control_e_Informe : BasePage
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
                //string scripts = Tabs_Tramite.scriptCarga(true, false);
                //this.EjecutarScript(updCargaTramite, "finalizarCarga();" + scripts);
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
            //CargarTiposDeInformes();

            SGI_Tramites_Tareas Transftarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (Transftarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro en la tabla SGI_Transf_Tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }
            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);

            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucObservacionContribuyente.Enabled = IsEditable;
            ucObservacionInforme.Enabled = IsEditable;
            ucDocumentoAdjunto.Enabled = IsEditable;
            ddlTiposInforme.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.TR;
            SGI_Tramites_Tareas_TRANSF tttransf = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = tttransf.id_solicitud;
            ucCabecera.LoadData((int)Constants.GruposDeTramite.TR, this.id_solicitud);
            ucListaDocumentos.LoadData((int)Constants.GruposDeTramite.TR, this.id_solicitud);
            ucDocumentoAdjunto.LoadData((int)Constants.GruposDeTramite.TR, this.id_solicitud, id_tramitetarea);

            Tabs_Tramite.LoadData(this.id_solicitud, 1);

            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucObservacionesAnteriores.LoadData(id_grupotramite, id_solicitud, id_tramitetarea, tttransf.SGI_Tramites_Tareas.id_tarea);

            string scripts = Tabs_Tramite.scriptCarga(IsEditable, false);
            this.EjecutarScript(updCargaTramite, "finalizarCarga();" + scripts);

            SGI_Tarea_Carga_Tramite carga = Buscar_tarea_anterior(this.id_solicitud);
            if (carga != null)
            {
                ucObservacionesTarea.Text = carga.Observaciones;
                ucObservacionContribuyente.Text = carga.observaciones_contribuyente;
                ucObservacionInforme.Text = carga.Observaciones_informe;
                ddlTiposInforme.SelectedValue = carga.id_tipo_informe.ToString();
            }
            else
            {
                ucObservacionesTarea.Text = "";
                ucObservacionContribuyente.Text = "";
                ucObservacionInforme.Text = "";
            }

            this.db.Dispose();
        }

        //private void CargarTiposDeInformes()
        //{
        //    ddlTiposInforme.DataValueField = "id_tipo_informe";
        //    ddlTiposInforme.DataTextField = "nombre";
        //    List<TiposDeInformes> list = this.db.TiposDeInformes.ToList();
        //    TiposDeInformes tipo = new TiposDeInformes();
        //    tipo.id_tipo_informe = 0;
        //    tipo.nombre = "Seleccioné una opción";

        //    list.Insert(0, tipo);
        //    ddlTiposInforme.DataSource = list;
        //    ddlTiposInforme.DataBind();
        //    ddlTiposInforme.Focus();
        //}

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
        private SGI_Tarea_Carga_Tramite Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Carga_Tramite tarea =
                (
                    from gere in db.SGI_Tarea_Carga_Tramite
                    where gere.id_tramitetarea == id_tramitetarea
                    orderby gere.id_carga_tramite descending
                    select gere
                ).ToList().FirstOrDefault();

            return tarea;
        }

        private SGI_Tarea_Carga_Tramite Buscar_tarea_anterior(int id_transf)
        {
            SGI_Tarea_Carga_Tramite tarea =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                    join gere in db.SGI_Tarea_Carga_Tramite on tt_tr.id_tramitetarea equals gere.id_tramitetarea
                    where tt_tr.id_solicitud == id_transf
                    && tt.id_tarea == (int)Constants.ENG_Tareas.TR_Control_e_Informe
                    orderby gere.id_carga_tramite descending
                    select gere
                ).ToList().FirstOrDefault();

            return tarea;
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

        private void Guardar_tarea(bool finalizar, int id_cpadron, int id_tramitetarea, int? id_tipo_informe, string observacion, string observContribuyente, string observInforme, Guid userId)
        {

            SGI_Tarea_Carga_Tramite tarea = Buscar_Tarea(id_tramitetarea);

            int id_tarea = 0;
            if (tarea != null)
                id_tarea = tarea.id_carga_tramite;

            db.SGI_Tarea_Carga_Tramite_Actualizar(id_tarea, id_tramitetarea, observacion, observContribuyente, id_tipo_informe, observInforme, userId);

            //if (finalizar && !string.IsNullOrEmpty(observContribuyente))
               // db.CPadron_Solicitudes_AgregarObservaciones(id_cpadron, observContribuyente, userId);

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
                        int? id_tipo_informe = Convert.ToInt32(ddlTiposInforme.SelectedValue);
                        if (id_tipo_informe == 0)
                            id_tipo_informe = null;
                        Guardar_tarea(false, this.id_solicitud, this.id_tramitetarea, id_tipo_informe, ucObservacionesTarea.Text.Trim(), ucObservacionContribuyente.Text.Trim(), ucObservacionInforme.Text.Trim(), userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.LogError.Write(ex, "Error en transaccion. carga_tramite-ucResultadoTarea_GuardarClick");
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

                //db.SGI_CPadron_Solicitudes_VerificarCarga(this.id_solicitud, param_msgError);
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
                LogError.LogError.Write(ex, "Error en transaccion. carga_tramite-ucResultadoTarea_Validar_Finalizar");
                throw ex;
            }
            finally
            {
                db.Dispose();
            }
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            lblError.Text = "";

            this.db = new DGHP_Entities();

            try
            {
                Guid userid = Functions.GetUserId();
                int id_TransfTarea_nuevo = 0;

                if (Validar_Finalizar())
                {

                    using (TransactionScope Tran = new TransactionScope())
                    {

                        try
                        {
                            int? id_tipo_informe = Convert.ToInt32(ddlTiposInforme.SelectedValue);
                            if (id_tipo_informe == 0)
                                id_tipo_informe = null;
                            Guardar_tarea(true, this.id_solicitud, this.id_tramitetarea, id_tipo_informe, ucObservacionesTarea.Text.Trim(), ucObservacionContribuyente.Text.Trim(), ucObservacionInforme.Text.Trim(), userid);

                            db.SaveChanges();

                            id_TransfTarea_nuevo = ucResultadoTarea.FinalizarTarea();

                            Tran.Complete();
                            Tran.Dispose();
                            //if (id_tipo_informe != null)

                            db.Dispose();

                            int? id_tipoinforme = Convert.ToInt32(ddlTiposInforme.SelectedValue);
                            if (id_tipoinforme == 0)
                                id_tipoinforme = null;

                            //if (id_tipoinforme != null)
                                //SubirPDFInforme(this.id_solicitud, this.id_tramitetarea, ucObservacionInforme.Text.Trim(), id_tipoinforme.Value);

                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.LogError.Write(ex, "Error en transaccion. carga_tramite-ucResultadoTarea_FinalizarTareaClick");
                            throw ex;
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updCargaTramite, "showfrmError_Carga();");
            }
            finally
            {
                this.db.Dispose();
            }

            if (lblError.Text.Length == 0)
                Redireccionar_VisorTramite();

        }

        #endregion

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Guid userid = Functions.GetUserId();
            int? id_tipo_informe = Convert.ToInt32(ddlTiposInforme.SelectedValue);
            if (id_tipo_informe == 0)
                id_tipo_informe = null;

            this.db = new DGHP_Entities();
            Guardar_tarea(true, this.id_solicitud, this.id_tramitetarea, id_tipo_informe, ucObservacionesTarea.Text.Trim(), ucObservacionContribuyente.Text.Trim(), ucObservacionInforme.Text.Trim(), userid);
            this.db.Dispose();
            //Response.Redirect("~/Reportes/CPadron/ImprimirInformeTipo.aspx?id=" + this.id_tramitetarea + "&tt=" + id_tipo_informe);
        }

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            int id_resultado = e.id_resultado.Value;
            var resultado = db.ENG_Resultados.FirstOrDefault(x => x.id_resultado == id_resultado);
            if (resultado.nombre_resultado == "Pedir Rectificación y/o Documentación Adicional")
            {
                ucObservacionInforme.ValidarRequerido = false;
                Req_ddlTiposInforme.Enabled = false;
            }
            else
            {
                ucObservacionInforme.ValidarRequerido = true;
                Req_ddlTiposInforme.Enabled = true;
            }
            FinalizarEntity();
        }

        protected void ucDocumentoAdjunto_GuardarDocumentoAdjuntoClick(object sender, ucDocumentoAdjuntoEventsArgs e)
        {
            ucListaDocumentos.LoadData((int)Constants.GruposDeTramite.TR, this.id_solicitud);
        }

        protected void ucDocumentoAdjunto_EliminarDocumentoAdjuntoClick(object sender, ucDocumentoAdjuntoEventsArgs e)
        {
            ucListaDocumentos.LoadData((int)Constants.GruposDeTramite.TR, this.id_solicitud);
        }

       
        protected void lnkNroExpSave(object sender, Tabs_TramiteEventsArgs e)
        {
            ucCabecera.LoadData((int)Constants.GruposDeTramite.TR, this.id_solicitud);
        }
    }
}
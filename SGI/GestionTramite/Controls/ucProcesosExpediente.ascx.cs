using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{


    public partial class ucProcesosExpediente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(int id_tramitetarea)
        {
            IniciarEntity();

            hid_cancelado_usuario.Value = "0";

            this.id_tramite_tarea = id_tramitetarea;

            Guid userid = Functions.GetUserId();

            Expediente procesoExpediente = new Expediente(userid, Constants.ApplicationName);

            this.lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramitetarea);

            procesoExpediente.Dispose();

            grdProcesosExpediente.DataSource = lstProcesos;
            grdProcesosExpediente.DataBind();

            grdResultadoExpediente.DataSource = lstProcesos;
            grdResultadoExpediente.DataBind();

            this.index_proceso_pendiente = BuscarProximoProcesoPendiente(id_tramitetarea);

            updPnlGrillaProcesos.Update();
            updPnlGrillaResultadoExpediente.Update();

            FinalizarEntity();

        }


        #region "Propiedades"

        private int _index_proceso_pendiente;
        public int index_proceso_pendiente
        {
            get
            {
                if (!string.IsNullOrEmpty(hid_index_proceso_pendiente.Value))
                    _index_proceso_pendiente = Convert.ToInt32(hid_index_proceso_pendiente.Value);
                else
                    _index_proceso_pendiente = -1;

                return _index_proceso_pendiente;
            }
            set
            {
                _index_proceso_pendiente = value;
                hid_index_proceso_pendiente.Value = _index_proceso_pendiente.ToString();
            }
        }

        private bool _editable;
        public bool editable
        {
            get
            {
                if (!string.IsNullOrEmpty(hid_editable.Value))
                    _editable = Convert.ToBoolean(hid_editable.Value);
                else
                    _editable = false;

                return _editable;
            }
            set
            {
                _editable = value;
                hid_editable.Value = _editable.ToString();
            }
        }

        private int _id_tramite_tarea;
        public int id_tramite_tarea
        {
            get
            {

                if (!string.IsNullOrEmpty(hid_id_tramite_tarea.Value))
                    _id_tramite_tarea = Convert.ToInt32(hid_id_tramite_tarea.Value);
                else
                    _id_tramite_tarea = -1;

                return _id_tramite_tarea;
            }
            set
            {
                _id_tramite_tarea = value;
                hid_id_tramite_tarea.Value = _id_tramite_tarea.ToString();
            }
        }


        private bool _existen_procesos_pendientes;
        public bool Existen_procesos_pendientes
        {
            get {
                if (this.lstProcesos == null || this.lstProcesos.Count == 0)
                    _existen_procesos_pendientes = true;
                else
                    _existen_procesos_pendientes = this.lstProcesos.Exists(x => x.realizado == false || !string.IsNullOrEmpty(x.resultado_sade_error)); 

                return _existen_procesos_pendientes;
            }
        }



        #endregion

        #region "Eventos"

        public class ResultadoProcesoExpediente : EventArgs
        {
            public int id_tramite_tarea { get; set; }
            public int id_proceso_ejecutado { get; set; }

            public Nullable<int> index_actual { get; set; }
            public Nullable<int> index_proximo { get; set; }
            public bool hay_procesos_pendientes { get; set; }

        }
        public class ErrorProcesoExpediente_EventArgs : EventArgs
        {
            public string mensaje{ get; set; }
            public Exception ex { get; set; }
        }

        public delegate void EventHandler_Proceso(object sender, ResultadoProcesoExpediente e);
        public event EventHandler_Proceso Proceso_Finalizado;

        public delegate void EventHandler_ProcesoItem(object sender, ResultadoProcesoExpediente e);
        public event EventHandler_ProcesoItem ProcesoItem_Finalizado;

        public delegate void EventHandler_errorProceso(object sender, ErrorProcesoExpediente_EventArgs e);
        public event EventHandler_errorProceso ProcesoError;

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


        public List<ProcesoExpediente> lstProcesos { get; set; }

        protected async virtual void grdProcesosExpediente_Click(object sender, CommandEventArgs e)
        {

            Expediente procesoExpediente = null;

            Label lblMensajeError = null;
            
            try
            {
                ResultadoProcesoExpediente args = new ResultadoProcesoExpediente();

                IniciarEntity();

                Button btnProcesarItemExpediente = (Button)sender;
                GridViewRow row = (GridViewRow)btnProcesarItemExpediente.Parent.Parent.Parent.Parent;
                int index = row.RowIndex;

                lblMensajeError = (Label)row.FindControl("lblMensajeError");
                lblMensajeError.Text = "";

                Guid userid = Functions.GetUserId();
                int id_generar_expediente_proc = Convert.ToInt32(grdProcesosExpediente.DataKeys[index].Values[0]);
                int id_tramitetarea = Convert.ToInt32(grdProcesosExpediente.DataKeys[index].Values[1]);
                this.id_tramite_tarea = id_tramitetarea;

                procesoExpediente = new Expediente(userid, Constants.ApplicationName);

                await procesoExpediente.Procesar(id_generar_expediente_proc, id_tramitetarea);

                int id_proceso_ejecutado = procesoExpediente.proceso.id_proceso;
                int id_devolucion_ee_ejecutado = procesoExpediente.proceso.id_devolucion_ee;
                int nro_tramite_ejecutado = procesoExpediente.proceso.nro_tramite.HasValue ? (int)procesoExpediente.proceso.nro_tramite:0;
                string resultado_ee_ejecutado = procesoExpediente.proceso.resultado_ee;

                this.lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramitetarea);

                procesoExpediente.Dispose();

                if (!string.IsNullOrEmpty(resultado_ee_ejecutado))
                {

                    ProcesoExpediente item = this.lstProcesos.Where(x => x.id_generar_expediente_proc == id_generar_expediente_proc).FirstOrDefault();
                    if (item != null)
                    {
                        item.resultado_sade_error = resultado_ee_ejecutado;
                    }

                }

                grdProcesosExpediente.DataSource = lstProcesos;
                grdProcesosExpediente.DataBind();
                updPnlGrillaProcesos.Update();

                grdResultadoExpediente.DataSource = lstProcesos;
                grdResultadoExpediente.DataBind();
                updPnlGrillaResultadoExpediente.Update();

                args.id_tramite_tarea = id_tramitetarea;
                args.hay_procesos_pendientes= Existen_procesos_pendientes;
                args.id_proceso_ejecutado = id_proceso_ejecutado;
                args.index_actual = index;
                args.index_proximo = -1;

                this.index_proceso_pendiente = index;

                if (args.hay_procesos_pendientes)
                    args.index_proximo = btnProcesarItemExpediente_LanzarClick();

                FinalizarEntity();


                if (Proceso_Finalizado != null && !hayProcesosPendientes())
                {
                    Proceso_Finalizado(this, args);
                }

                if (ProcesoItem_Finalizado != null )
                {
                    ProcesoItem_Finalizado(this, args);
                }

            }
            catch (Exception ex)
            {
                if ( procesoExpediente != null )
                    procesoExpediente.Dispose();
                if (lblMensajeError != null && string.IsNullOrEmpty(lblMensajeError.Text))
                    lblMensajeError.Text = "Tarea incompleta-" + ex.Message;
                FinalizarEntity();
                if (ProcesoError != null)
                {
                    ErrorProcesoExpediente_EventArgs args = new ErrorProcesoExpediente_EventArgs();
                    args.ex = ex;
                    args.mensaje = "Tarea incompleta-" + ex.Message;
                    ProcesoError(sender, args);
                }
            }
            
        }

        private bool hayProcesosPendientes()
        {
            DGHP_Entities db = new DGHP_Entities();
            bool procesosPendientes = (from tp in db.SGI_Tarea_Generar_Expediente_Procesos
                                       where tp.id_tramitetarea == this.id_tramite_tarea && !tp.realizado
                                       select tp).Count() > 0;


            db.Dispose();
            return procesosPendientes;

        }
        private int btnProcesarItemExpediente_LanzarClick()
        {
   
            int indexActual = this.index_proceso_pendiente;
            int id_tramitetarea = this.id_tramite_tarea ;

            //avisar el browser que ejecute el proximo item de la grilla
            //enviando la ejecucion del js 
            int rowIndexPendiente = BuscarProximoProcesoPendiente(id_tramitetarea);

            this.index_proceso_pendiente = rowIndexPendiente;

            //btnEjecutarProcesos.Visible = !ucResultadoTarea.btnFinalizar_Enabled;

            //if (rowIndexPendiente == -1)
            //    btnEjecutarProcesos.Visible = false;


            if (rowIndexPendiente < 0 || rowIndexPendiente > grdProcesosExpediente.Rows.Count) // evitar errores por estar fuera del rango, o no hay pendientes
                return -1;

            if (rowIndexPendiente == indexActual) //lo ultimo procesado es lo mismo que el siguiente, evitar un bucle infinito
                return -1;

            //if (!btnEjecutarProcesos.Visible)
            //{
            //     cuando el boton procesar todo no esta visible es porque no hay nada pendiente 
            //    return;
            //}


            UpdatePanel updPnlItemGrillaProcesos = (UpdatePanel)grdProcesosExpediente.Rows[rowIndexPendiente].FindControl("updPnlItemGrillaProcesos");

            ScriptManager sm = ScriptManager.GetCurrent(Page);

            if (sm.IsInAsyncPostBack)
            {
               // string proximo_proceso = "ejecutarProcesos(" + rowIndexPendiente + ")";
                string proximo_proceso = "ejecutarTodosProcesosEE()";
                ScriptManager.RegisterStartupScript(updPnlItemGrillaProcesos, updPnlItemGrillaProcesos.GetType(), "ejecutarTodosProcesosEE", proximo_proceso, true);
            }



            return rowIndexPendiente;

        }

        private int BuscarProximoProcesoPendiente(int id_tramitetarea)
        {

            //Buscar el primer id_generar_expediente_proc que no esta procesado
            int index = -1;

            int id_generar_expediente_proc = this.lstProcesos.Where(x => x.id_devolucion_ee == -1 || x.realizado == false).OrderBy(x => x.id_generar_expediente_proc).Select(x => x.id_generar_expediente_proc).FirstOrDefault();

            //recorrer grilla para averiguar la fila de este id_generar_expediente_proc. 
            //es para porder enviar el click del boton de la fila pendiente de ejecucion. -1 no hay pendientes
            int iii = 0;
            for (iii = 0; iii < grdProcesosExpediente.Rows.Count; iii++)
            {

                int id_reg = Convert.ToInt32(grdProcesosExpediente.DataKeys[iii].Values["id_generar_expediente_proc"]);

                if (id_reg == id_generar_expediente_proc )
                {
                    index = iii;
                    break;
                }

            }

            return index;
        }

        #region generar procesos pendientes

        public void GenerarProcesos_Expediente(int id_tramite_tarea)
        {

            IniciarEntity();

            //using (TransactionScope Tran = new TransactionScope()){
                try
                {
                    Guid userid = Functions.GetUserId();
                     this.db.SGI_Cargar_Procesos_Expedientes(id_tramite_tarea, userid);

                    //Tran.Complete();
                    //Tran.Dispose();

                    FinalizarEntity();
                }
                catch (Exception ex)
                {
                    //Tran.Dispose();
                    FinalizarEntity();
                    LogError.Write(ex, "error en transaccion. ucProcesosExpediente-GenerarProcesos_Expediente");
                    throw ex;
                }
            //}
        }


        public void GenerarProcesos_Revision_Firma_Disposicion(int id_tramite_tarea)
        {

            IniciarEntity();

            try
            {
                Guid userid = Functions.GetUserId();
                TransactionScope Tran = new TransactionScope();

                try
                {
                    this.db.SGI_Cargar_Procesos_Revision_Firma_Disposicion(id_tramite_tarea, userid);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    if (Tran != null)
                        Tran.Dispose();
                    LogError.Write(ex, "error en transaccion. GenerarProcesos_Revision_Firma_Disposicion");
                    throw ex;
                }

                FinalizarEntity();

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }
        }


        #endregion

    }

}
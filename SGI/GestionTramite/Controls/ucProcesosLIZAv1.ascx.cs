using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucProcesosLIZAv1 : System.Web.UI.UserControl
    {


        public delegate void EventHandlerFinalizadoEnLIZA(object sender, EventArgs e);
        public event EventHandlerFinalizadoEnLIZA FinalizadoEnLIZA;

        #region "Propiedades"

        public int id_tipo_tramite
        {
            get
            {
                int ret = (ViewState["_tipo_tramite"] != null ? Convert.ToInt32(ViewState["_tipo_tramite"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_tipo_tramite"] = value;
            }

        }

        public int id_tarea
        {
            get
            {
                int ret = (ViewState["_tarea"] != null ? Convert.ToInt32(ViewState["_tarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_tarea"] = value;
            }

        }

        private int id_tramitetarea
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_tramitetarea"] != null ? Convert.ToInt32(ViewState["_id_tramitetarea"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_tramitetarea"] = value;
            }

        }
        private int id_solicitud
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_solicitud"] != null ? Convert.ToInt32(ViewState["_id_solicitud"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_solicitud"] = value;
            }

        }
        private bool isEditable
        {
            get
            {
                bool ret = false;
                ret = (ViewState["_isEditable"] != null ? Convert.ToBoolean(ViewState["_isEditable"]) : false);
                return ret;
            }
            set
            {
                ViewState["_isEditable"] = value;
            }

        }

        private string _sistema_SADE;

        private string sistema_SADE
        {
            get
            {
                if (string.IsNullOrEmpty(_sistema_SADE))
                {
                    _sistema_SADE = "SGI";
                }
                return _sistema_SADE;
            }
        }
        #endregion

        public void cargarDatosProcesos(int id_tramitetarea, bool ejecutarProcesos)
        {
            this.isEditable = isEditable;
            this.id_tramitetarea = id_tramitetarea;
            Guid userid = Functions.GetUserId();

            DGHP_Entities db = new DGHP_Entities();

            // carga los datos de los procesos
            // -----------------------------------------------------------------------------------
            var lstProcesos = (from pe in db.SGI_LIZA_Procesos
                               where pe.id_tramitetarea == id_tramitetarea
                               orderby pe.id_liza_proceso
                               select new dtoSGI_LIZA_Procesos
                               {
                                   id_liza_proceso = pe.id_liza_proceso,
                                   id_tramite_tarea = pe.id_tramitetarea,
                                   realizado = pe.realizado,
                                   descripcion = pe.descripcion,
                                   class_resultado = (pe.realizado ? "text-success" : "text-error"),
                                   resultado = pe.resultado
                               }).ToList();


            grdResultadoExpediente.DataSource = lstProcesos;
            grdResultadoExpediente.DataBind();
            updPnlGrillaResultadoExpediente.Update();

            db.Dispose();


            bool ejecutarGeneracion = false;

            if (ejecutarProcesos)
            {
                ejecutarGeneracion = hayProcesosPendientes(id_tramitetarea);
                if (ejecutarGeneracion)
                    cargarGrillaParaProcesar(id_tramitetarea);


                if (ejecutarGeneracion)
                    ScriptManager.RegisterStartupScript(updPnlGrillaResultadoExpediente, updPnlGrillaResultadoExpediente.GetType(), "script", "ejecutargeneracionEnPasarela();", true);


            }
        }

        public bool hayProcesosPendientes(int id_tramitetarea)
        {
            bool ret = false;
            DGHP_Entities db = new DGHP_Entities();

            ret = (db.SGI_LIZA_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea && !x.realizado) > 0);

            db.Dispose();
            return ret;
        }

        private void cargarGrillaParaProcesar(int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();

            // carga los datos de los procesos
            // -----------------------------------------------------------------------------------
            var lstProcesos = (from pe in db.SGI_LIZA_Procesos
                               where pe.id_tramitetarea == id_tramitetarea
                               orderby pe.id_liza_proceso
                               select new dtoSGI_LIZA_Procesos
                               {
                                   id_liza_proceso = pe.id_liza_proceso,
                                   id_tramite_tarea = pe.id_tramitetarea,
                                   realizado = pe.realizado,
                                   descripcion = pe.descripcion,
                                   class_resultado = (pe.realizado ? "text-success" : "text-error"),
                                   resultado = pe.resultado
                               }).ToList();

            grdProcesosExpediente.DataSource = lstProcesos;
            grdProcesosExpediente.DataBind();
            updPnlGrillaProcesos.Update();

            db.Dispose();

        }

        protected void btnProcesarItemExpediente_Click(object sender, EventArgs e)
        {
            Button btnProcesarItemExpediente = (Button)sender;
            int id_liza_proceso = int.Parse(btnProcesarItemExpediente.CommandArgument);
            bool ejecutarSiguienteProceso = true;

            Guid userid = Functions.GetUserId();
            GridViewRow row = (GridViewRow)btnProcesarItemExpediente.Parent.Parent.Parent.Parent;
            UpdatePanel updPnlItemGrillaProcesos = (UpdatePanel)row.FindControl("updPnlItemGrillaProcesos");

            try
            {

                DGHP_Entities db = new DGHP_Entities();

                var tp = db.SGI_LIZA_Procesos.FirstOrDefault(x => x.id_liza_proceso == id_liza_proceso);

                if (tp != null)
                {
                    if (!tp.realizado)
                    {
                        try {
                            byte[] archivo = null;// System.IO.File.ReadAllBytes(tp.parametros);

                            System.Net.WebClient client = new System.Net.WebClient();
                            //client.DownloadFile(new Uri(tp.parametros), tp.descripcion);
                            archivo= client.DownloadData(new Uri(tp.parametros));

                            int id_file= ws_FilesRest.subirArchivo(tp.descripcion, archivo);
                            db.SGI_LIZA_Procesos_update(id_liza_proceso, true, id_file, "", userid);
                        }
                        catch (Exception ex){
                            db.SGI_LIZA_Procesos_update(id_liza_proceso, false, -1, ex.Message, userid);
                        }



                        // Actualizar los datos de la grilla
                        // --------------------------------
                        db.Entry(tp).Reload();  // esto es para refrescar los datos del objeto entity ya que fueron modificados por stores
                        if (tp != null)
                        {
                            CheckBox chkRealizadoEnPasarela = (CheckBox)row.FindControl("chkRealizado");
                            Label lblResultado = (Label)row.FindControl("lblResultado");

                            chkRealizadoEnPasarela.Checked = tp.realizado;
                            lblResultado.CssClass = (tp.realizado ? "text-success" : "text-error");
                            lblResultado.Text = tp.resultado;
                            updPnlGrillaProcesos.Update();
                        }
                    }

                }

                db.Dispose();

                if (ejecutarSiguienteProceso)
                    ScriptManager.RegisterStartupScript(updPnlGrillaProcesos, updPnlGrillaProcesos.GetType(), "script", "ejecutarProcesoSiguiente();", true);
                else
                {
                    ScriptManager.RegisterStartupScript(updPnlGrillaProcesos, updPnlGrillaProcesos.GetType(), "script", "finalizarEjecucionProcesos();", true);
                }

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(updPnlItemGrillaProcesos, updPnlItemGrillaProcesos.GetType(), "script", "showfrmErrorProcesosSADE();", true);
            }
        }

        protected void btnCerrarModalProcesos_Click(object sender, EventArgs e)
        {
            // Solo carga la lista sin ejecutar los procesos
            cargarDatosProcesos(this.id_tramitetarea, false);

            if (!hayProcesosPendientes(this.id_tramitetarea))
            {
                if (FinalizadoEnLIZA != null)
                    FinalizadoEnLIZA(sender, new EventArgs());
            }
            ScriptManager.RegisterStartupScript(updCerrarProcesos, updCerrarProcesos.GetType(), "script", "ocultarfrmProcesarEE();", true);
        }
    }

    public class dtoSGI_LIZA_Procesos
    {
        public int id_liza_proceso { get; set; }
        public int id_tramite_tarea { get; set; }
        public bool realizado { get; set; }
        public string descripcion { get; set; }
        public string class_resultado { get; set; }
        public string resultado { get; set; }
        public DateTime? fecha_sade { get; set; }
        public int id_devolucion_ee { get; set; }
        public bool realizado_en_SADE { get; set; }
        public bool ejecutado_anteriormente { get; set; }
    }

}

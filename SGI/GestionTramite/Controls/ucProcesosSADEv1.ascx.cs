using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Transactions;
using System.Web.Services.Protocols;
using System.Data.Entity.Core.Objects;
using System.Text;
using System.IO;
using SGI.WebServices;
using System.Data;
using SGI.StaticClassNameSpace;

namespace SGI.GestionTramite.Controls
{
    public partial class ucProcesosSADEv1 : System.Web.UI.UserControl
    {
        

        public delegate void EventHandlerFinalizadoEnSADE(object sender, EventArgs e);
        public event EventHandlerFinalizadoEnSADE FinalizadoEnSADE;

        private class Datos_Caratula_Persona_Fisica
        {
            public string apellido { get; set; }
            public string nombres { get; set; }
            public string cuit { get; set; }
            public string email { get; set; }
            public string telefono { get; set; }
            public int id_tipodoc_personal { get; set; }
            public string numero_doc { get; set; }
            public string descrip_expediente { get; set; }
            public string motivo_expediente { get; set; }
            public string codigo_trata { get; set; }
            public string domicilio { get; set; }
            public string piso { get; set; }
            public string depto { get; set; }
            public string codigo_postal { get; set; }
            public string motivo_externo { get; set; }
        }
        private class Datos_Caratula_Persona_Juridica
        {
            public string razon_social { get; set; }
            public string cuit { get; set; }
            public string email { get; set; }
            public string telefono { get; set; }
            public string descrip_expediente { get; set; }
            public string motivo_expediente { get; set; }
            public string codigo_trata { get; set; }
            public string domicilio { get; set; }
            public string piso { get; set; }
            public string depto { get; set; }
            public string codigo_postal { get; set; }
            public string motivo_externo { get; set; }
        }

        #region "Propiedades"

        public int id_grupo_tramite
        {
            get
            {
                int ret = (ViewState["_grupo_tramite"] != null ? Convert.ToInt32(ViewState["_grupo_tramite"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_grupo_tramite"] = value;
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
        public int id_solicitud
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
            set { 
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

        public bool isEmptyGrid() 
        {
            return (grdResultadoExpediente.Rows.Count == 0);
        }

        public void cargarDatosProcesos(int id_tramitetarea, bool ejecutarProcesos)
        {
            this.isEditable = isEditable;
            this.id_tramitetarea = id_tramitetarea;
            Guid userid = Functions.GetUserId();

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
            {
                var tt = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                this.id_solicitud = tt.id_cpadron;
            }
            else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
            {
                var tt = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                this.id_solicitud = tt.id_solicitud;
            }
            else
            {
                var tt = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                this.id_solicitud = tt.id_solicitud;
            }

            
            // carga los datos de los procesos
            // -----------------------------------------------------------------------------------
            var lstProcesos = (from pe in db.SGI_SADE_Procesos
                               join proc in db.SGI_Procesos_EE on pe.id_proceso equals proc.id_proceso
                               where pe.id_tramitetarea == id_tramitetarea
                               orderby pe.id_tarea_proc
                               select new dtoSGI_SADE_Procesos
                               {
                                   id_tarea_proc = pe.id_tarea_proc,
                                   id_tramite_tarea = pe.id_tramitetarea,
                                   id_proceso = pe.id_proceso,
                                   nombre_proceso = proc.desc_proceso,
                                   id_origen_reg = pe.id_origen_reg,
                                   realizado_en_pasarela = pe.realizado_en_pasarela,
                                   descripcion_tramite = pe.descripcion_tramite,
                                   class_resultado_SADE = (pe.realizado_en_pasarela && (pe.realizado_en_SADE ||  pe.id_proceso == (int) Constants.SGI_Procesos_EE.GEN_PAQUETE) ? "text-success" : "text-error"),
                                   resultado_sade = pe.resultado_ee,
                                   realizado_en_SADE = pe.realizado_en_SADE,
                                   fecha_sade = pe.fecha_en_SADE
                               }).ToList();


            grdResultadoExpediente.DataSource = lstProcesos;
            grdResultadoExpediente.DataBind();
            updPnlGrillaResultadoExpediente.Update();

            db.Dispose();


            bool ejecutarGeneracionEnPasarela = false;

            if (ejecutarProcesos)
            {
                actualizarResultadosSADE(id_tramitetarea, userid);

                ejecutarGeneracionEnPasarela = hayProcesosPendientesPasarella(id_tramitetarea);
                if (ejecutarGeneracionEnPasarela)
                    cargarGrillaParaProcesar(id_tramitetarea);


                if (ejecutarGeneracionEnPasarela)
                    ScriptManager.RegisterStartupScript(updPnlGrillaResultadoExpediente, updPnlGrillaResultadoExpediente.GetType(), "script", "ejecutargeneracionEnPasarela();", true);


            }
        }
                
        public bool hayProcesosPendientesPasarella(int id_tramitetarea)
        {
            // Evalúa si hayProcesosPendientes procesos pendientes de realizar en la pasarela.

            bool ret = false;
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            ret = (db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea && !x.realizado_en_pasarela) > 0);

            db.Dispose();

            return ret;
        }

        public bool hayProcesosPendientesSADE(int id_tramitetarea)
        {
            // Evalúa si hay Procesos Pendientes de realizar en SADE

            bool ret = false;
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            ret = (db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea && !x.realizado_en_SADE && x.id_proceso != (int) Constants.SGI_Procesos_EE.GEN_PAQUETE) > 0);

            db.Dispose();

            return ret;
        }

        private void cargarGrillaParaProcesar(int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            // carga los datos de los procesos
            // -----------------------------------------------------------------------------------
            var lstProcesos = (from pe in db.SGI_SADE_Procesos
                               join proc in db.SGI_Procesos_EE on pe.id_proceso equals proc.id_proceso
                               where pe.id_tramitetarea == id_tramitetarea
                               orderby pe.id_tarea_proc
                               select new dtoSGI_SADE_Procesos
                               {
                                   id_tarea_proc = pe.id_tarea_proc,
                                   id_tramite_tarea = pe.id_tramitetarea,
                                   id_proceso = pe.id_proceso,
                                   nombre_proceso = proc.desc_proceso,
                                   id_origen_reg = pe.id_origen_reg,
                                   realizado_en_pasarela = pe.realizado_en_pasarela,
                                   descripcion_tramite = pe.descripcion_tramite,
                                   class_resultado_SADE = (pe.realizado_en_pasarela && (pe.realizado_en_SADE || pe.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_PAQUETE) ? "text-success" : "text-error"),
                                   resultado_sade = pe.resultado_ee,
                                   realizado_en_SADE = pe.realizado_en_SADE,
                                   fecha_sade = pe.fecha_en_SADE,
                                   ejecutado_anteriormente = pe.realizado_en_pasarela       // se iniciliza de esta manera para no ejecutar nuevamente los procesos
                               }).ToList();

            grdProcesosExpediente.DataSource = lstProcesos;
            grdProcesosExpediente.DataBind();
            updPnlGrillaProcesos.Update();

            db.Dispose();

        }

        private void actualizarResultadosSADE(int id_tramitetarea, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            //obtiene los resultados SADE de todos los procesos que fueron realizados en la pasarela pero no en SADE,
            //excluyendo la generación del paquete, ya que el mismo no se realiza en SADE.

            var lstProcesos = (from tp in db.SGI_SADE_Procesos
                               where tp.id_tramitetarea == id_tramitetarea &&
                               tp.id_proceso != (int)Constants.SGI_Procesos_EE.GEN_PAQUETE &&
                               tp.realizado_en_pasarela && !tp.realizado_en_SADE
                               select new
                               {
                                   tp.id_tarea_proc,
                                   tp.id_paquete
                               }).ToList();

            foreach (var item in lstProcesos)
            {
                obtenerResultadoSADE(item.id_tarea_proc, item.id_paquete, userid);
            }

            db.Dispose();
        }

        protected async void btnProcesarItemExpediente_Click(object sender, EventArgs e)
        {


            Button btnProcesarItemExpediente = (Button)sender;
            int id_tarea_proc = int.Parse(btnProcesarItemExpediente.CommandArgument);
            bool ejecutarSiguienteProceso = true;
            int id_file = 0;

            Guid userid = Functions.GetUserId();
            GridViewRow row = (GridViewRow)btnProcesarItemExpediente.Parent.Parent.Parent.Parent;
            UpdatePanel updPnlItemGrillaProcesos = (UpdatePanel)row.FindControl("updPnlItemGrillaProcesos");

            try
            {
                DGHP_Entities db = new DGHP_Entities();
                db.Database.CommandTimeout = 300;
                int id_caratula = 0;
                var aux = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                SGI_SADE_Procesos tp = new SGI_SADE_Procesos();
                if (aux.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_PAQUETE)
                    tp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                else
                    tp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc && x.id_paquete != 0);

                if (tp != null)
                {
                    if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP ||
                        this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                    {
                        var texp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete && x.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_CARATULA);
                        if (texp != null && texp.id_devolucion_ee.HasValue)
                            id_caratula = texp.id_devolucion_ee.Value;
                    }
                    else
                    {
                        // Habilitaciones SGI 2.0
                        var texp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete && x.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_CARATULA);
                        if (texp != null && texp.id_devolucion_ee.HasValue)
                            id_caratula = texp.id_devolucion_ee.Value;
                        else
                        {
                            // Habilitaciones SGI 1.0
                            var texp1 = db.SGI_Tarea_Generar_Expediente_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete);
                            if (texp1 != null)
                                id_caratula = texp1.id_caratula;
                        }
                    }
                    switch (tp.id_proceso)
                    {
                        case (int)Constants.SGI_Procesos_EE.GEN_PAQUETE:
                            if (!tp.realizado_en_pasarela)
                            {
                                generarPaquete(id_tarea_proc, userid);
                            }
                            break;
                                

                        case (int) Constants.SGI_Procesos_EE.GEN_CARATULA:
                            if (!tp.realizado_en_pasarela)
                            {
                                generarCaratula(id_tarea_proc, tp.id_paquete, userid);
                            }
                            obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            break;

                        case (int) Constants.SGI_Procesos_EE.SUBIR_DOCUMENTO:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    subirDocumento(tp.id_tarea_proc, tp.id_paquete, tp.id_file.Value, tp.descripcion_tramite, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;
                        case (int)Constants.SGI_Procesos_EE.SUBIR_OBSERVACIONES:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    subirObservaciones(tp.id_tarea_proc, tp.id_paquete, tp.id_file.Value, tp.descripcion_tramite, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;
                        case (int)Constants.SGI_Procesos_EE.SUBIR_PLANO:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    subirPlano(tp.id_tarea_proc, tp.id_paquete, tp.id_file.Value, tp.descripcion_tramite, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;

                        case (int)Constants.SGI_Procesos_EE.SUBIR_PROVIDENCIA:

                            if (validarProcesoCaratulaEnSADE() && (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea)))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    subirProvidencia(tp.id_tarea_proc, tp.id_paquete, tp.descripcion_tramite, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;

                        case (int)Constants.SGI_Procesos_EE.GEN_PASE:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    generarPaseExpediente(tp.id_tarea_proc, tp.id_paquete, id_caratula, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;

                        case (int)Constants.SGI_Procesos_EE.BLOQUEO_EXPEDIENTE:

                            if (bloquearExpediente(tp.id_tarea_proc, tp.id_paquete, id_caratula, userid))
                            {
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;

                            break;

                        case (int)Constants.SGI_Procesos_EE.DESBLOQUEO_EXPEDIENTE:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                desbloquearExpediente(tp.id_tarea_proc, tp.id_paquete, id_caratula, userid);
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;

                        case (int)Constants.SGI_Procesos_EE.GET_CARATULA:
                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                ObtenerCaratula(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;

                            break;

                        case (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA:
                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                string NroExpediente = obtenerNroExpediente(tp.id_paquete);
                                if (NroExpediente.Length > 0 )
                                {
                                    if (!tp.realizado_en_pasarela)
                                    {
                                        string html_dispo = "";
                                        if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                                        {
                                            
                                            if ( !tp.id_file.HasValue )
                                            {

                                                //No existe, genero html_dispo
                                                //Genero id_file
                                                PdfDisposicion dispo = new PdfDisposicion();
                                                html_dispo = dispo.GenerarHtml_Disposicion(this.id_solicitud, tp.id_tramitetarea, NroExpediente);
                                                string nombreArchivo = "HtmlDispo_IdSol:" + id_solicitud.ToString() + "_IdTramiteTarea:" + id_tramitetarea.ToString() + "_NroExped:" + NroExpediente + ".html";

                                                
                                                byte[] utfBytes  = Encoding.GetEncoding(Functions.GetParametroChar("Server.Encoding")).GetBytes(html_dispo);
                                                id_file = ws_FilesRest.subirArchivo(nombreArchivo, utfBytes);
                                            }
                                            else
                                            {
                                                //Ya existe el id_file solo recupero el html_dispo generado con anterioridad
                                                byte[] bytes = ws_FilesRest.DownloadFile(Convert.ToInt32(tp.id_file.Value));
                                                html_dispo = Encoding.ASCII.GetString(bytes);
                                            }
                                            
                                        }
                                        else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                                        {
                                            string str_archivo = "";
                                            int nroTrReferencia = 0;
                                            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
                                            if (!tp.id_file.HasValue)
                                            {
                                                if (this.id_solicitud <= nroTrReferencia)
                                                {
                                                    str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\Disposicion.html"));
                                                    html_dispo = PdfDisposicion.Transf_GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, NroExpediente, str_archivo);
                                                }
                                                else
                                                {
                                                    str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmision.html"));
                                                    html_dispo = PdfDisposicion.Transmision_GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, NroExpediente, str_archivo);
                                                }

                                                string nombreArchivo = "HtmlDispo_IdSol:" + id_solicitud.ToString() + "_IdTramiteTarea:" + id_tramitetarea.ToString() + "_NroExped:" + NroExpediente + ".html";
                                                byte[] bytes = Encoding.ASCII.GetBytes(html_dispo);
                                                id_file = ws_FilesRest.subirArchivo(nombreArchivo, bytes);
                                            }
                                            else
                                            {
                                                //Ya existe el id_file solo recupero el html_dispo generado con anterioridad
                                                byte[] bytes = ws_FilesRest.DownloadFile(Convert.ToInt32(tp.id_file.Value));
                                                html_dispo = Encoding.ASCII.GetString(bytes);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(html_dispo))
                                        {
                                            //Copio id_file generado en SGI_SADE_Procesos
                                            ActualizarIdFile(tp.id_tarea_proc, id_file);
                                            generarTareaAlaFirma(tp.id_tarea_proc, tp.id_paquete, html_dispo, userid);
                                            
                                        }
                                        else
                                        {
                                            throw new Exception("No es posible obtener el html de la disposición.");
                                        }
                                    }
                                    obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                                }
                                else
                                    ejecutarSiguienteProceso = false;
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;


                        case (int)Constants.SGI_Procesos_EE.REVISION_DE_FIRMA:
                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                ejecutarSiguienteProceso = revisionFirma(tp.id_tarea_proc, tp.id_paquete, userid);
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;

                            break;

                        case (int)Constants.SGI_Procesos_EE.RELACIONAR_DOCUMENTO:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {

                                if (tp.id_origen_reg == null)
                                {
                                    if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                                    {
                                        relacionarTareaALaFirma(tp.id_tarea_proc, tp.id_paquete, userid);
                                    }
                                }
                                else
                                    relacionarDocumento(tp.id_tarea_proc, tp.id_paquete, userid);

                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;

                            break;

                        case (int)Constants.SGI_Procesos_EE.GET_DISPOSICION:

                            
                            if (validarProcesoFirmaDispoEnSADE())
                            {
                                ObtenerDisposicion(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;

                            break;

                        case (int)Constants.SGI_Procesos_EE.SUBIR_CERTIFICADO:

                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    subirCertificado(tp.id_tarea_proc, tp.id_paquete, tp.descripcion_tramite, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;

                        case (int)Constants.SGI_Procesos_EE.RELACIONAR_EXPEDIENTE:
                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                if (!tp.realizado_en_pasarela)
                                {
                                    relacionarExpediente(tp.id_tarea_proc, tp.id_paquete, tp.descripcion_tramite, userid);
                                }
                                obtenerResultadoSADE(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;
                            break;
                        case (int)Constants.SGI_Procesos_EE.GET_DOCUMENTO:
                            if (validarProcesosRealizadosEnSADE(tp.id_tarea_proc, tp.id_tramitetarea))
                            {
                                ObtenerDocumento(tp.id_tarea_proc, tp.id_paquete, userid);
                            }
                            else
                                ejecutarSiguienteProceso = false;

                            break;
                    }


                    // Actualizar los datos de la grilla
                    // --------------------------------

                    db.Entry(tp).Reload();  // esto es para refrescar los datos del objeto entity ya que fueron modificados por stores
                    if (tp != null)
                    {
                        CheckBox chkRealizadoEnPasarela = (CheckBox)row.FindControl("chkRealizado_en_pasarela");
                        CheckBox chkRealizadoEnSADE = (CheckBox)row.FindControl("chkRealizado_en_SADE");
                        Label lblResultadoSADE = (Label)row.FindControl("lblResultadoSADE");
                        Label lblDescripcionTramite = (Label)row.FindControl("lblDescripcionTramite");

                        chkRealizadoEnPasarela.Checked = tp.realizado_en_pasarela;
                        chkRealizadoEnSADE.Checked = tp.realizado_en_SADE;
                        lblResultadoSADE.CssClass = (tp.realizado_en_pasarela && tp.realizado_en_SADE ? "text-success" : "text-error");
                        lblResultadoSADE.Text = tp.resultado_ee;
                        lblDescripcionTramite.Text = tp.descripcion_tramite;
                        updPnlGrillaProcesos.Update();

                    }
                   

                }
                else
                {
                    ejecutarSiguienteProceso = false;
                    CheckBox chkRealizadoEnPasarela = (CheckBox)row.FindControl("chkRealizado_en_pasarela");
                    CheckBox chkRealizadoEnSADE = (CheckBox)row.FindControl("chkRealizado_en_SADE");
                    Label lblResultadoSADE = (Label)row.FindControl("lblResultadoSADE");

                    chkRealizadoEnPasarela.Checked = false;
                    chkRealizadoEnSADE.Checked = false;
                    lblResultadoSADE.CssClass = "text-error";
                    lblResultadoSADE.Text = "El trámite no está caratulado";
                    updPnlGrillaProcesos.Update();
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
                LogError.Write(ex, "Error en btnProcesarItemExpediente_Click");
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(updPnlItemGrillaProcesos, updPnlItemGrillaProcesos.GetType(), "script", "showfrmErrorProcesosSADE();", true);
            }


        }

        private void ActualizarIdFile(int id_tarea_proc, int id_file)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            using (TransactionScope Tran = new TransactionScope())
            {
                try
                {
                    db.SGI_SADE_Procesos_updateFile(id_tarea_proc, id_file);
                    Tran.Complete();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    throw ex;
                }
            }
        }

        private SGI_SADE_Procesos RecuperarRegistroSgiSadeProceso(int id_tramitetarea, int id_paquete, int id_solicitud)
        {
            int idProceso = (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA;
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            var sgi_reg = (from stt in db.SGI_Tramites_Tareas
                          join spp in db.SGI_SADE_Procesos on stt.id_tramitetarea equals spp.id_tramitetarea
                          where spp.id_tramitetarea == id_tramitetarea && spp.id_proceso == idProceso && spp.id_paquete == id_paquete
                          select spp).FirstOrDefault();
            return sgi_reg;
        }

        #region "Funciones para Expediente Electrónico"

        //private string getRubrosParaExpediente(int id_solicitud)
        //{
        //    string rubros = "";
        //    DGHP_Entities db = new DGHP_Entities();

        //    var query_Rubros =
        //        (
        //            from sol in db.CAA_Solicitudes
        //            join rub in db.CAA_Rubros on sol.id_caa equals rub.id_caa
        //            join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
        //            join docreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals docreq.Id
        //            where sol.id_caa == id_solicitud
        //            select new
        //            {
        //                rub.id_caarubro,
        //                sol.id_caa,
        //                rub.cod_rubro,
        //                desc_rubro = rub.desc_rubro,
        //                rub.EsAnterior,
        //                TipoActividad = tact.Nombre,
        //                DocRequerida = docreq.Nomenclatura,
        //                rub.SuperficieHabilitar
        //            }
        //        ).ToList();


        //    foreach (var item in query_Rubros)
        //    {
        //        if (string.IsNullOrEmpty(rubros))
        //            rubros = item.cod_rubro;
        //        else
        //        {
        //            if (rubros.Length > 70)
        //                rubros = rubros + ",\n" + item.cod_rubro;
        //            else
        //                rubros = rubros + ", " + item.cod_rubro;
        //        }

        //    }

        //    if (rubros.Length > 0)
        //    {
        //        if (query_Rubros.Count > 1)
        //            rubros = "Rubros: " + rubros;
        //        else
        //            rubros = "Rubro: " + rubros;

        //    }

        //    db.Dispose();

        //    return rubros;
        //}
        private ws_ExpedienteElectronico.EETipodocumento ConvertToTipoDocumento_EE(int id_tipo_doc_solicitud)
        {
            ws_ExpedienteElectronico.EETipodocumento tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.OT;

            switch (id_tipo_doc_solicitud)
            {
                case 1:
                    tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.DU;
                    break;

                case 3:
                    tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.LC;
                    break;

                case 4:
                    tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.CI;
                    break;

                case 5:
                    tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.PA;
                    break;

                default:
                    tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.OT;
                    break;
            }

            return tipo_doc_ee;
        }

        private decimal ConvertCuitToDecimal(string p_cuit)
        {
            decimal cuit = 0;
            string str_cuit = "";

            foreach (char item in p_cuit.ToCharArray())
            {
                if (Char.IsDigit(item))
                {
                    str_cuit = str_cuit + item;
                }
            }

            decimal.TryParse(str_cuit, out cuit);

            return cuit;
        }

        #endregion

        #region "Procesos de Expediente Electrónico"

        private void generarPaquete(int id_tarea_proc, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;

            try
            {

                // Subir y relacionar documento en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;

                try
                {
                    id_devolucion_ee = serviceEE.CrearPaquete(this.username_servicio_EE, this.pass_servicio_EE);
                    resultado_ee = string.Format("Paquete Nº {0}", id_devolucion_ee);
                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw ex;
                }
                finally
                {
                    serviceEE.Dispose();
                }


            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                {
                    db.SGI_SADE_Procesos_updatePaquete(this.id_tramitetarea, id_devolucion_ee);
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                }

                db.Dispose();
            }


        }

        private void generarCaratula(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;
            Datos_Caratula_Persona_Fisica pf =  null;
            Datos_Caratula_Persona_Juridica pj = null;
            
            try
            {
                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros= null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                // Obtiene las datos de la carátula para enviar al servicio
                // ---------
                obtenerDatosCaratula(this.id_grupo_tramite, this.id_tramitetarea, ref pf, ref pj);

                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                
                if(username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                // Genera carátula en servicio
                // ---------------------------------
                try
                {
                    

                    if (pf != null)
                    {
                        //generar caratula persona física
                        decimal dCuit = ConvertCuitToDecimal(pf.cuit);

                        ws_ExpedienteElectronico.EETipodocumento tipo_doc = ConvertToTipoDocumento_EE(pf.id_tipodoc_personal);

                        decimal dNroDoc = 0;
                        string Nro_Documento = pf.numero_doc;
                        bool convert = decimal.TryParse(pf.numero_doc, out dNroDoc);
                        if (tipo_doc == ws_ExpedienteElectronico.EETipodocumento.PA && !convert)
                        {
                            tipo_doc = ws_ExpedienteElectronico.EETipodocumento.DU;
                            Nro_Documento = pf.cuit.Substring(2,8);
                            decimal.TryParse(Nro_Documento, out dNroDoc);
                        }

                        id_devolucion_ee = serviceEE.CaratularPersonaFisica_v2(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, pf.apellido,
                            pf.nombres, dCuit, tipo_doc, dNroDoc, pf.email, string.Empty, this.sistema_SADE, pf.descrip_expediente,
                            pf.motivo_expediente, username_SADE, pf.codigo_trata, pf.domicilio, pf.piso, pf.depto, pf.codigo_postal,pf.motivo_externo);


                    }
                    else if (pj != null)
                    {
                        //generar caratula persona jurídica

                        decimal dCuit = ConvertCuitToDecimal(pj.cuit);

                        id_devolucion_ee = serviceEE.CaratularPersonaJuridica_v2(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, pj.razon_social,
                            dCuit, pj.email, string.Empty, this.sistema_SADE, pj.descrip_expediente, pj.motivo_expediente, username_SADE, pj.codigo_trata, pj.domicilio,
                            pj.piso, pj.depto, pj.codigo_postal, pj.motivo_externo);
                    }
                    else
                        throw new Exception("No se pudieron obtener los datos de los titulares de la carátula.");

                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                {
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                }

                db.Dispose();
            }
            
        }

        private void obtenerDatosCaratula(int id_grupo_tramite, int id_tramitetarea, ref Datos_Caratula_Persona_Fisica pf, ref Datos_Caratula_Persona_Juridica pj)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string cod_trata = "";
            string motivo_expediente = "";

            if (id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
            {
                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Consulta.Padron");
                
                pj = (from proc in db.SGI_SADE_Procesos
                      join tt in db.SGI_Tramites_Tareas on proc.id_tramitetarea equals tt.id_tramitetarea
                      join cpadron in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals cpadron.id_tramitetarea
                      join tit_pj in db.CPadron_Titulares_PersonasJuridicas on cpadron.id_cpadron equals tit_pj.id_cpadron
                      where proc.id_tramitetarea == id_tramitetarea
                      select new Datos_Caratula_Persona_Juridica
                      {
                          razon_social = tit_pj.Razon_Social,
                          domicilio = tit_pj.Calle + " " + tit_pj.NroPuerta.ToString(),
                          piso = tit_pj.Piso,
                          depto = tit_pj.Depto,
                          codigo_postal = "",
                          cuit = tit_pj.CUIT,
                          motivo_expediente = "Consulta al padrón",
                          email = tit_pj.Email,
                          motivo_externo = "",
                          telefono = "",
                          descrip_expediente = "Solicitud Nº " + cpadron.id_cpadron.ToString()
                      }).FirstOrDefault();

                if (pj != null)
                    pj.codigo_trata = cod_trata;



                if (pj == null)
                {
                    pf = (from proc in db.SGI_SADE_Procesos
                          join tt in db.SGI_Tramites_Tareas on proc.id_tramitetarea equals tt.id_tramitetarea
                          join cpadron in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals cpadron.id_tramitetarea
                          join tit_pf in db.CPadron_Titulares_PersonasFisicas on cpadron.id_cpadron equals tit_pf.id_cpadron
                          where proc.id_tramitetarea == id_tramitetarea
                          select new Datos_Caratula_Persona_Fisica
                          {
                              apellido = tit_pf.Apellido,
                              nombres = tit_pf.Nombres,
                              id_tipodoc_personal = tit_pf.id_tipodoc_personal,
                              numero_doc = tit_pf.Nro_Documento,
                              domicilio = tit_pf.Calle + " " + tit_pf.Nro_Puerta.ToString(),
                              piso = tit_pf.Piso,
                              depto = tit_pf.Depto,
                              codigo_postal = "",
                              cuit = tit_pf.Cuit,
                              motivo_expediente = "Consulta al padrón",
                              email = tit_pf.Email,
                              motivo_externo = "",
                              telefono = "",
                              descrip_expediente = "Solicitud Nº " + cpadron.id_cpadron.ToString()
                          }).FirstOrDefault();
                    
                    if (pf != null)
                        pf.codigo_trata = cod_trata;

                }
            }
            else if (id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
            {

                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Transferencias");

                pj = (from proc in db.SGI_SADE_Procesos
                      join tt in db.SGI_Tramites_Tareas on proc.id_tramitetarea equals tt.id_tramitetarea
                      join transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals transf.id_tramitetarea
                      join tit_pj in db.Transf_Titulares_PersonasJuridicas on transf.id_solicitud equals tit_pj.id_solicitud
                      where proc.id_tramitetarea == id_tramitetarea
                      select new Datos_Caratula_Persona_Juridica
                      {
                          razon_social = tit_pj.Razon_Social,
                          domicilio = tit_pj.Calle + " " + tit_pj.NroPuerta.ToString(),
                          piso = tit_pj.Piso,
                          depto = tit_pj.Depto,
                          codigo_postal = "",
                          cuit = tit_pj.CUIT,
                          motivo_expediente = "Transferencia",
                          email = tit_pj.Email,
                          motivo_externo = "",
                          telefono = "",
                          descrip_expediente = "Solicitud Nº " + transf.id_solicitud.ToString()
                      }).FirstOrDefault();

                if (pj != null)
                    pj.codigo_trata = cod_trata;



                if (pj == null)
                {
                    pf = (from proc in db.SGI_SADE_Procesos
                          join tt in db.SGI_Tramites_Tareas on proc.id_tramitetarea equals tt.id_tramitetarea
                          join transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals transf.id_tramitetarea
                          join tit_pf in db.Transf_Titulares_PersonasFisicas on transf.id_solicitud equals tit_pf.id_solicitud
                          where proc.id_tramitetarea == id_tramitetarea
                          select new Datos_Caratula_Persona_Fisica
                          {
                              apellido = tit_pf.Apellido,
                              nombres = tit_pf.Nombres,
                              id_tipodoc_personal = tit_pf.id_tipodoc_personal,
                              numero_doc = tit_pf.Nro_Documento,
                              domicilio = tit_pf.Calle + " " + tit_pf.Nro_Puerta.ToString(),
                              piso = tit_pf.Piso,
                              depto = tit_pf.Depto,
                              codigo_postal = "",
                              cuit = tit_pf.Cuit,
                              motivo_expediente = "Transferencia",
                              email = tit_pf.Email,
                              motivo_externo = "",
                              telefono = "",
                              descrip_expediente = "Solicitud Nº " + transf.id_solicitud.ToString()
                          }).FirstOrDefault();

                    if (pf != null)
                        pf.codigo_trata = cod_trata;

                }
            }
            else if (id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
            {
                var datos = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                      join sol in db.SSIT_Solicitudes on tt_hab.id_solicitud equals sol.id_solicitud
                      where tt_hab.id_tramitetarea == id_tramitetarea
                      select new 
                      {
                          sol.id_tipotramite,
                          sol.id_subtipoexpediente,
                          sol.id_solicitud,
                          tt_hab.SGI_Tramites_Tareas.id_tarea
                      }).FirstOrDefault();

                bool esEscuela= false;
                if ( Shared.GetGrupoCircuito(id_solicitud) == (int) Constants.ENG_Grupos_Circuitos.SCPESCU ||
                    Shared.GetGrupoCircuito(id_solicitud) == (int)Constants.ENG_Grupos_Circuitos.HPESCU)
                    esEscuela = true;
                if (esEscuela)
                {
                    motivo_expediente = "Habilitación";
                    cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.Escuela");
                }
                else if (datos.id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion)
                {
                    motivo_expediente = "Ampliación";
                    cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Ampliacion");
                }
                else if (datos.id_tipotramite == (int)Constants.TipoDeTramite.RedistribucionDeUso)
                {
                    motivo_expediente = "Redistribución de Uso";
                    cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.RedistribucionDeUso");
                }
                else
                {
                    switch (datos.id_subtipoexpediente)
                    {
                        case 1: // SIN_PLANOS
                            cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleSinplanos");
                            break;
                        case 2: // CON_PLANOS
                            decimal sup = 0;
                            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == datos.id_solicitud
                                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                            sup = enc.Encomienda_DatosLocal.First().superficie_cubierta_dl.Value + enc.Encomienda_DatosLocal.First().superficie_descubierta_dl.Value;

                            if (sup < 100)
                                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.A");
                            else if (sup < 500)
                                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.B");
                            else
                                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.C");
                            break;
                        case 3: //INSPECCION_PREVIA
                            cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.InspeccionPrevia") ;
                            break;
                        case 4: //Habilitacion_PREVIA
                            cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.InspeccionPrevia");
                            break;
                        default:
                            break;
                    }

                    motivo_expediente = "Habilitación";
                }

                pj = (from proc in db.SGI_SADE_Procesos
                      join tt in db.SGI_Tramites_Tareas on proc.id_tramitetarea equals tt.id_tramitetarea
                      join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                      join sol in db.SSIT_Solicitudes on tt_hab.id_solicitud equals sol.id_solicitud
                      join tit_pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on sol.id_solicitud equals tit_pj.id_solicitud
                      where proc.id_tramitetarea == id_tramitetarea
                      select new Datos_Caratula_Persona_Juridica
                      {
                          razon_social = tit_pj.Razon_Social,
                          domicilio = tit_pj.Calle + " " + tit_pj.NroPuerta.ToString(),
                          piso = tit_pj.Piso,
                          depto = tit_pj.Depto,
                          codigo_postal = "",
                          cuit = tit_pj.CUIT,
                          motivo_expediente = motivo_expediente,
                          email = tit_pj.Email,
                          motivo_externo = "",
                          telefono = "",
                          descrip_expediente = "Solicitud Nº " + tt_hab.id_solicitud.ToString()
                      }).FirstOrDefault();

                if (pj != null)
                    pj.codigo_trata = cod_trata;



                if (pj == null)
                {
                    pf = (from proc in db.SGI_SADE_Procesos
                          join tt in db.SGI_Tramites_Tareas on proc.id_tramitetarea equals tt.id_tramitetarea
                          join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                          join sol in db.SSIT_Solicitudes on tt_hab.id_solicitud equals sol.id_solicitud
                          join tit_pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on sol.id_solicitud equals tit_pf.id_solicitud
                          where proc.id_tramitetarea == id_tramitetarea
                          select new Datos_Caratula_Persona_Fisica
                          {
                              apellido = tit_pf.Apellido,
                              nombres = tit_pf.Nombres,
                              id_tipodoc_personal = tit_pf.id_tipodoc_personal,
                              numero_doc = tit_pf.Nro_Documento,
                              domicilio = tit_pf.Calle + " " + tit_pf.Nro_Puerta.ToString(),
                              piso = tit_pf.Piso,
                              depto = tit_pf.Depto,
                              codigo_postal = "",
                              cuit = tit_pf.Cuit,
                              motivo_expediente = motivo_expediente,
                              email = tit_pf.Email,
                              motivo_externo = "",
                              telefono = "",
                              descrip_expediente = "Solicitud Nº " + tt_hab.id_solicitud.ToString()
                          }).FirstOrDefault();

                    if (pf != null)
                        pf.codigo_trata = cod_trata;

                }
            }
            db.Dispose();
        }

        private async void subirDocumento(int id_tarea_proc, int id_paquete, int id_file, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();
            dbFiles.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;

            try
            {
                byte[] documento = null;

                // Recupero los parámetros de la tarea de proceso
                // ----------------------------------------------
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                string Tabla_Origen = "";
                string Acronimo_SADE = "";
                string formato_archivo = "";
                string Ffcc_SADE = null;
                string nombre_archivo = "";

                try { username_SADE = parametros.Usuario_SADE.Value; }
                catch (Exception) { username_SADE = Functions.GetUsernameSADE(userid); }

                try { Tabla_Origen = parametros.Tabla_Origen.Value; }
                catch (Exception) { Tabla_Origen = ""; }

                try { Acronimo_SADE = parametros.Acronimo_SADE.Value; }
                catch (Exception) { Acronimo_SADE = Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma"); }

                try { formato_archivo = parametros.formato_archivo.Value; }
                catch (Exception) { formato_archivo = "pdf"; }

                try { Ffcc_SADE = parametros.Ffcc_SADE.Value; }
                catch (Exception) { Ffcc_SADE = null; }

                //Si id_file=-1 es la plancheta
                if (id_file == -1)
                {
                    if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                    {
                        string nro_expediente = obtenerNroExpediente(id_paquete);
                        var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == this.id_solicitud
                            && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                        
                        documento = await Plancheta.GenerarPdfPlanchetahabilitacion(this.id_solicitud, id_tramitetarea, enc.id_encomienda, nro_expediente, false);

                        id_file = ws_FilesRest.subirArchivo("Plancheta.pdf", documento);

                        int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.PLANCHETA_HABILITACION;
                        ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                        using (TransactionScope Tran = new TransactionScope())
                        {
                            try
                            {
                                db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, true, id_file, "Plancheta.pdf", userid, param_id_docadjunto);
                                //actualizo el file del proceso asi no lo tiene que volver a generar
                                db.SGI_SADE_Procesos_updateFileOrigen(id_tarea_proc, 0, id_file);
                                proc.id_origen_reg = id_file;
                                Tran.Complete();
                            }
                            catch (Exception ex)
                            {
                                Tran.Dispose();
                                throw ex;
                            }
                        }
                    }
                    else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                    {
                        //TODO Falta q la planceha se actualize no se se cree una nueva
                        string nro_expediente = obtenerNroExpediente(id_paquete);

                        documento = Plancheta.Transf_GenerarPdf(this.id_solicitud, nro_expediente, false);
                        int id_tdocreq = (int)Constants.TiposDeDocumentosRequeridos.Plancheta;
                        string tdocreq_detalle = "";
                        int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.PLANCHETA_TRANSFERENCIA;
                        nombre_archivo = "";
                        ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                        id_file = ws_FilesRest.subirArchivo("Plancheta.pdf", documento);
                        using (TransactionScope Tran = new TransactionScope())
                        {
                            try
                            {
                                db.Transf_DocumentosAdjuntos_Agregar(this.id_solicitud, id_tdocreq, tdocreq_detalle, id_tipodocsis, true, id_file, nombre_archivo, userid, (int)Constants.NivelesDeAgrupamiento.General, param_id_docadjunto);
                                //actualizo el file del proceso asi no lo tiene que volver a generar
                                db.SGI_SADE_Procesos_updateFile(id_tarea_proc, id_file);
                                Tran.Complete();
                            }
                            catch (Exception ex)
                            {
                                Tran.Dispose();
                                throw ex;
                            }
                        }
                    }
                }
                if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB ||
                    this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR || 
                    this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                {
                    if (Tabla_Origen == "Certificados")
                    {
                        var file = dbFiles.Certificados.FirstOrDefault(x => x.id_certificado == proc.id_origen_reg);
                        if (file == null)
                            throw new Exception("No se encontró el contenido del archivo en la base de Files.");

                        documento = file.Certificado;

                        if (documento.Length <= 1)
                            throw new Exception("El documento se encuentra vacío.");
                    }
                    else
                    {

                        documento = ws_FilesRest.DownloadFile(id_file);
                        if (documento == null)
                            throw new Exception("No se encontró el contenido del archivo en la base de Files.");

                        if (documento.Length <= 1)
                            throw new Exception("El documento se encuentra vacío.");
                    }
                }
                
                // Subir y relacionar documento en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                //serviceEE.Url = this.url_servicio_EE;
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {
                    string identificacion_documento = string.Format("Nro. de trámite: {0}, Nro. de documento: {1}", this.id_solicitud, id_file);

                    bool EnviarEmbebido = false;

                    // identifica si es un pdf y si tiene los permisos correctos y si está firmado.
                    try
                    {
                        using (var pdf = new iTextSharp.text.pdf.PdfReader(documento))
                        {
                            if (!pdf.IsOpenedWithFullPermissions || (serviceEE.isPdfFirmado(ref documento) && Acronimo_SADE == Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma")))
                                EnviarEmbebido = true;
                        }

                    }
                    catch (Exception)
                    {
                        // si no es pdf va embebido siempre
                        EnviarEmbebido = true;
                    }

                    if (Ffcc_SADE == null)
                    {
                        if (EnviarEmbebido)
                        {
                            var nom = db.SSIT_DocumentosAdjuntos.Where(x => x.id_file == id_file).FirstOrDefault();

                            if (nom == null)
                                nombre_archivo = identificacion_documento + ".txt";
                            else
                                nombre_archivo = nom.nombre_archivo;

                            id_devolucion_ee = serviceEE.Subir_Documentos_Embebidos_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, "txt", nombre_archivo);
                        }
                        else
                            id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, formato_archivo,false);
                    }
                    else
                    {
                        string formulario_json = await FormulariosControlados.getFormulario(Ffcc_SADE, this.id_solicitud);
                        if (EnviarEmbebido)
                            id_devolucion_ee = serviceEE.Subir_Documentos_Embebidos_ConAcroAndTipo_ffcc(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, "txt", identificacion_documento, formulario_json);
                        else
                            id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo_ffcc(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, formato_archivo, formulario_json);
                    }
                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
                dbFiles.Dispose();
            }

        }

        private async void subirObservaciones(int id_tarea_proc, int id_paquete, int id_file, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();
            dbFiles.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;
            string Ffcc_SADE = null;
            try
            {
                byte[] documento = null;

                // Recupero los parámetros de la tarea de proceso
                // ----------------------------------------------
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                string Tabla_Origen = "";
                string Acronimo_SADE = "";
                string formato_archivo = "";

                try { username_SADE = parametros.Usuario_SADE.Value; }
                catch (Exception) { username_SADE = Functions.GetUsernameSADE(userid); }

                try { Tabla_Origen = parametros.Tabla_Origen.Value; }
                catch (Exception) { Tabla_Origen = ""; }

                try { Acronimo_SADE = parametros.Acronimo_SADE.Value; }
                catch (Exception) { Acronimo_SADE = Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma"); }

                try { formato_archivo = parametros.formato_archivo.Value; }
                catch (Exception) { formato_archivo = "pdf"; }

                try { Ffcc_SADE = parametros.Ffcc_SADE.Value; }
                catch (Exception) { Ffcc_SADE = null; }

                //Si id_file=-1 es la plancheta
                if (id_file == -1)
                {
                    documento = Observaciones_SADE.GenerarPdf(id_solicitud);
                    id_file = ws_FilesRest.subirArchivo("Observaciones.pdf", documento);

                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {
                            //Actualuzo el file de las observaciones
                            var lstObservaciones = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                                    join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                                    join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                                    where tt_h.id_solicitud == id_solicitud && obs.id_file_sade == null
                                                    select new
                                                    {
                                                        obs.id_ObsDocs,
                                                    }).ToList();
                            foreach (var ob in lstObservaciones)
                            {
                                var obDoc = db.SGI_Tarea_Calificar_ObsDocs.Where(x => x.id_ObsDocs == ob.id_ObsDocs).First();
                                obDoc.id_file_sade = id_file;
                                db.SaveChanges();
                            }
                            //actualizo el file del proceso asi no lo tiene que volver a generar
                            db.SGI_SADE_Procesos_updateFileOrigen(id_tarea_proc, 0, id_file);
                            proc.id_origen_reg = id_file;
                            Tran.Complete();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            throw ex;
                        }
                    }
                }
                else
                    documento = ws_FilesRest.DownloadFile(id_file);

                if (documento == null)
                    throw new Exception("No se encontró el contenido del archivo en la base de Files.");

                if (documento.Length <= 1)
                    throw new Exception("El documento se encuentra vacío.");


                // Subir y relacionar documento en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {
                    string identificacion_documento = string.Format("Nro. de trámite: {0}, Nro. de documento: {1}", this.id_solicitud, id_file);


                    if (Ffcc_SADE == null)
                    {
                        id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                identificacion_documento, descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, formato_archivo,false);
                    }
                    else
                    {
                        string formulario_json = await FormulariosControlados.getFormulario(Ffcc_SADE, this.id_solicitud);
                        id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo_ffcc(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                identificacion_documento, descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, formato_archivo, formulario_json);
                    }

                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
                dbFiles.Dispose();
            }

        }


        private void subirProvidencia(int id_tarea_proc, int id_paquete, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;
            string tipoArchivo = "html";
            string acronimoSADE = "PV";
            string identificacion_documento = descripcion_tramite;
            string motivo = obtenerNroExpediente(id_paquete);//descripcion_tramite

            try
            {
                byte[] documento = Get_Providencia_HTML(id_tarea_proc);


                if (documento == null || documento.Length <= 1)
                    throw new Exception("El documento se encuentra vacío.");


                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                // Subir y relacionar la providencia en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {
                    
                    id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento, identificacion_documento, motivo, this.sistema_SADE, username_SADE, acronimoSADE,tipoArchivo,false);
                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
            }

        }

        private void subirCertificado(int id_tarea_proc, int id_paquete, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;
            string tipoArchivo = "pdf";
            string acronimoSADE = "IFGRA";
            string identificacion_documento = descripcion_tramite;
            string motivo = obtenerNroExpediente(id_paquete);//descripcion_tramite

            try
            {
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);

                string nro_expediente = obtenerNroExpediente(id_paquete);

                byte[] documento = Certificado.GenerarPdf(this.id_solicitud, nro_expediente, false);
                if (documento == null || documento.Length <= 1)
                    throw new Exception("El documento se encuentra vacío.");
                int id_file = ws_FilesRest.subirArchivo("Certificado.pdf", documento);

                int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.CERTIFICADO_HABILITACION;
                ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, true, id_file, "Certificado.pdf", userid, param_id_docadjunto);
                        //actualizo el file del proceso asi no lo tiene que volver a generar
                        db.SGI_SADE_Procesos_updateFileOrigen(id_tarea_proc, 0, id_file);
                        proc.id_origen_reg = id_file;
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                //Recupero el usuario_Sade
             
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                // Subir y relacionar en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {

                    id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento, identificacion_documento, motivo, this.sistema_SADE, username_SADE, acronimoSADE, tipoArchivo,false);
                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
            }

        }

        private void relacionarExpediente(int id_tarea_proc, int id_paquete, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;

            try
            {
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);

                string nro_expediente = obtenerNroExpediente(id_paquete);

                var sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).First();
                string nro_expediente_asociado = sol.NroExpedienteSadeRelacionado;

                //Recupero el usuario_Sade
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                // Subir y relacionar en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {
                    id_devolucion_ee = serviceEE.relacionarExpedientes(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, nro_expediente, nro_expediente_asociado, username_SADE, this.sistema_SADE);
                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
            }

        }

        private bool bloquearExpediente(int id_tarea_proc, int id_paquete, int id_caratula, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {
                // ---------------------------------------
                serviceEE.Url = this.url_servicio_EE;
                string username_SADE = Functions.GetUsernameSADE(userid);
                if (username_SADE == null || username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");


                id_devolucion_ee = serviceEE.Bloquear_Expediente(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, id_caratula, this.sistema_SADE, username_SADE);
                realizado_en_pasarela = true;
                resultado_ee = "";

                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }

            return realizado_en_pasarela;
        }


        private void desbloquearExpediente(int id_tarea_proc, int id_paquete, int id_caratula, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {
                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                // ---------------------------------------
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");


                id_devolucion_ee = serviceEE.Desbloquear_Expediente(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, id_caratula, this.sistema_SADE, username_SADE);
                realizado_en_pasarela = true;
                resultado_ee = "";

                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }
        }

        private void ObtenerCaratula(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;

            byte[] documento = new byte[0];

            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {
                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }
                serviceEE.Url = this.url_servicio_EE;

                documento = serviceEE.GetPdfCaratula(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);

                int id_tipodocsis = 0;

                if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB )
                    id_tipodocsis = Functions.GetTipoDocSistema("CARATULA_HABILITACION");
                else if ( this.id_grupo_tramite == (int) Constants.GruposDeTramite.CP )
                    id_tipodocsis = Functions.GetTipoDocSistema("CARATULA_CPADRON");
                else if ( this.id_grupo_tramite == (int) Constants.GruposDeTramite.TR )
                    id_tipodocsis = Functions.GetTipoDocSistema("CARATULA_TRANSFERENCIA");


                ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        int id_file = ws_FilesRest.subirArchivo("Caratula.pdf", documento);
                        if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                            db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, string.Empty, id_tipodocsis, false, id_file, "Caratula.pdf", userid, param_id_docadjunto);
                        else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                            db.CPadron_DocumentosAdjuntos_Agregar(this.id_solicitud, 0, string.Empty, id_tipodocsis, false, id_file, "Caratula.pdf", userid, param_id_docadjunto);
                        else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                            db.Transf_DocumentosAdjuntos_Agregar(this.id_solicitud, 0, string.Empty, id_tipodocsis, false, id_file, "Caratula.pdf", userid, (int)Constants.NivelesDeAgrupamiento.General, param_id_docadjunto);

                        id_devolucion_ee = 1;
                        resultado_ee = "Carátula obtenida.";
                        realizado_en_pasarela = true;

                        db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                        db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, true, DateTime.Now, resultado_ee, userid);

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }

        }

        private void ObtenerDisposicion(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;

            byte[] documento = new byte[0];

            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {
                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                var procDispo = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tramitetarea == proc.id_tramitetarea && x.id_proceso == (int) Constants.SGI_Procesos_EE.REVISION_DE_FIRMA);

                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }
                serviceEE.Url = this.url_servicio_EE;

                if (Functions.EsAmbienteDesa())
                {
                    id_devolucion_ee = 1;
                    resultado_ee = "Disposición obtenida. Forzado por Entorno de Desarrollo";
                    realizado_en_pasarela = true;

                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                    db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, true, DateTime.Now, resultado_ee, userid);
                }
                else
                {
                    documento = serviceEE.GetPdfDisposicionNroGedo(this.username_servicio_EE, this.pass_servicio_EE, username_SADE, procDispo.resultado_ee);

                    int id_tipodocsis = 0;

                    if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                        id_tipodocsis = Functions.GetTipoDocSistema("DISPOSICION_CPADRON");
                    else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                        id_tipodocsis = Functions.GetTipoDocSistema("DISPOSICION_TRANSFERENCIA");
                    else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                        id_tipodocsis = Functions.GetTipoDocSistema("DISPOSICION_HABILITACION");


                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));

                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {
                            int id_file = ws_FilesRest.subirArchivo("Disposicion.pdf", documento);
                            if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                                db.CPadron_DocumentosAdjuntos_Agregar(this.id_solicitud, 0, string.Empty, id_tipodocsis, true, id_file, "Disposicion.pdf", userid, param_id_docadjunto);
                            else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                                db.Transf_DocumentosAdjuntos_Agregar(this.id_solicitud, 0, string.Empty, id_tipodocsis, true, id_file, "Disposicion.pdf", userid, (int)Constants.NivelesDeAgrupamiento.General, param_id_docadjunto);
                            else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                                db.SSIT_DocumentosAdjuntos_Add(this.id_solicitud, 0, string.Empty, id_tipodocsis, true, id_file, "Disposicion.pdf", userid, param_id_docadjunto);

                            id_devolucion_ee = 1;
                            resultado_ee = "Disposición obtenida.";
                            realizado_en_pasarela = true;

                            db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, true, DateTime.Now, resultado_ee, userid);

                            Tran.Complete();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            throw ex;
                        }
                    }
                }                

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }

        }
        private void ObtenerDocumento(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;

            byte[] documento = new byte[0];

            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {
                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                string Numero_gedo = "";
                try
                {
                    Numero_gedo = parametros.Numero_gedo.Value;
                }
                catch (Exception)
                {
                    Numero_gedo = "";
                }
                serviceEE.Url = this.url_servicio_EE;

                documento = serviceEE.GetDocumentoPDF_SADE(this.username_servicio_EE, this.pass_servicio_EE, Numero_gedo, null, sistema_SADE, username_SADE);

                int id_tipodocsis = 0;

                if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                    id_tipodocsis = Functions.GetTipoDocSistema("DOC_ADJUNTO_CPADRON");
                else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                    id_tipodocsis = Functions.GetTipoDocSistema("DOC_ADJUNTO_TRANSFERENCIA");
                else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                    id_tipodocsis = Functions.GetTipoDocSistema("DOC_ADJUNTO_SSIT");


                ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        int id_file = ws_FilesRest.subirArchivo(Numero_gedo + ".pdf", documento);
                        if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                            db.CPadron_DocumentosAdjuntos_Agregar(this.id_solicitud, 0, Numero_gedo, id_tipodocsis, true, id_file, "Documento.pdf", userid, param_id_docadjunto);
                        else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                            db.Transf_DocumentosAdjuntos_Agregar(this.id_solicitud, 0, Numero_gedo, id_tipodocsis, true, id_file, "Documento.pdf", userid, (int)Constants.NivelesDeAgrupamiento.General, param_id_docadjunto);
                        else if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                        {

                            var docadj = new SSIT_DocumentosAdjuntos();
                            docadj.id_solicitud = this.id_solicitud;
                            docadj.id_tdocreq = 0;
                            docadj.id_tipodocsis = id_tipodocsis;
                            docadj.id_file = id_file;
                            docadj.generadoxSistema = true;
                            docadj.CreateDate = DateTime.Now;
                            docadj.CreateUser = userid;
                            docadj.nombre_archivo = Numero_gedo + ".pdf";
                            docadj.fechaPresentado = DateTime.Now;
                            docadj.ExcluirSubidaSADE = true;
                            db.SSIT_DocumentosAdjuntos.Add(docadj);
                            db.SaveChanges();
                            
                        }

                        id_devolucion_ee = 1;
                        resultado_ee = Numero_gedo;
                        realizado_en_pasarela = true;

                        db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                        db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, true, DateTime.Now, resultado_ee, userid);

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }

        }

        private void generarPaseExpediente(int id_tarea_proc, int id_paquete, int id_caratula, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {
                // ---------------------------------------
                serviceEE.Url = this.url_servicio_EE;

                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);
                string username_destino_SADE = "";
                string sector_destino_SADE = "";
                string reparticion_destino_SADE = "";
                string estado_SADE = "";
                bool conDesbloqueo = false;
                try
                {	        
		            username_destino_SADE = parametros.Usuario.Value;
	            }
	            catch (Exception)
	            {
		            username_destino_SADE = "";
	            }
                try 
	            {	        
		            sector_destino_SADE = parametros.Sector.Value;
	            }
	            catch (Exception)
	            {
		            sector_destino_SADE = "";
	            }    
                try 
	            {	        
		            reparticion_destino_SADE = parametros.Reparticion.Value;
	            }
	            catch (Exception)
	            {
		            reparticion_destino_SADE = "";
	            }
                try
                {
                    estado_SADE = parametros.Estado.Value;
                }
                catch (Exception)
                {
                    estado_SADE = "";
                }
                try
                {
                    conDesbloqueo = parametros.ConDesbloqueo.Value;
                }
                catch (Exception)
                {
                    conDesbloqueo = false;
                }
                // Averigua quien tiene el expediente en SADE
                string username_Origen_SADE = serviceEE.GetUsuarioAsignado(this.username_servicio_EE, this.pass_servicio_EE, id_caratula);
                
                // Si no lo tiene ningun usuario es porque lo tiene un buzon grupal, en este caso el 102.2 por ej
                // entonces se toma el usuario SADE del perfil y se intenta realizar el pase con el, que deberia ser un usuario del buzon.
                if (username_Origen_SADE.Length == 0)
                {
                    //Busco si lo tiene asignado un sector
                    string sector_Origen_SADE = serviceEE.GetSectorAsignado(this.username_servicio_EE, this.pass_servicio_EE, id_caratula);

                    if (sector_Origen_SADE == null ||sector_Origen_SADE.Trim().Length == 0)
                        throw new Exception("El expediente no esta asignado a un usuario o sector");  //username_Origen_SADE = Functions.GetUsernameSADE(userid);
                    else if(Functions.GetSectorSADE(userid) != sector_Origen_SADE)
                        throw new Exception("No es posible realizar el pase ya que el expediente se encuentra asignado al sector " + sector_Origen_SADE);
                    else
                        username_Origen_SADE = Functions.GetUsernameSADE(userid);
                }
                    

                if (username_Origen_SADE.Length > 0 && username_destino_SADE.Length > 0)
                {
                    id_devolucion_ee = serviceEE.PasarExpediente_aUsuario(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, "Pase al usuario " + username_destino_SADE, username_Origen_SADE, username_destino_SADE);

                    if (id_devolucion_ee > -1)
                        realizado_en_pasarela = true;

                    resultado_ee = "";

                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                }
                else if (username_Origen_SADE.Length > 0 && sector_destino_SADE.Length > 0)
                {
                    if (estado_SADE.Length == 0)
                        id_devolucion_ee = serviceEE.PasarExpediente_aGrupo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, "Pase al sector " + sector_destino_SADE, username_Origen_SADE, sector_destino_SADE, reparticion_destino_SADE);
                    else
                        id_devolucion_ee = serviceEE.PasarExpediente_aGrupo_v2(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, "Pase al sector " + sector_destino_SADE, username_Origen_SADE, sector_destino_SADE, reparticion_destino_SADE, estado_SADE, conDesbloqueo);

                    if (id_devolucion_ee > -1)
                        realizado_en_pasarela = true;

                    resultado_ee = "";

                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                }
                else
                {
                    id_devolucion_ee = serviceEE.PasarExpediente_aGuardaTemporal(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, "Pase a Guarda Temporal", username_Origen_SADE, estado_SADE);
                    if (id_devolucion_ee > -1)
                        realizado_en_pasarela = true;

                    resultado_ee = "";
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);
                }
                

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {

                serviceEE.Dispose();
                db.Dispose();
            }
        }
        
        private void generarTareaAlaFirma(int id_tarea_proc, int id_paquete, string html_dispo, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            string descripcion_tramite = "";
            
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();

            try
            {

                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc != null && proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                ws_ExpedienteElectronico.WS_Item[] ws_item = null;
                string NroExpediente = obtenerNroExpediente(id_paquete);
                string userName_SADE_Director = SGI.Parametros.GetParam_ValorChar("SGI.Username.Director.Habilitaciones");
                string userName_SADE_receptor = SGI.Parametros.GetParam_ValorChar("SGI.Username.Receptor.Habilitaciones");
                byte[] documento = Encoding.UTF8.GetBytes(html_dispo);
                descripcion_tramite = this.id_solicitud.ToString() + " (" + NroExpediente + ")";
                

                if ( userName_SADE_Director.Length == 0)
                    throw new Exception("No se ha configurado el usuario que va a firmar la disposición");

                if (userName_SADE_receptor.Length == 0)
                    userName_SADE_receptor = userName_SADE_Director;

                if (username_SADE == null || username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                // ---------------------------------------
                serviceEE.Url = this.url_servicio_EE;
                string[] usuarios_firmantes = new string[] { userName_SADE_Director };

                id_devolucion_ee = serviceEE.generarTareaAlaFirma(this.username_servicio_EE, this.pass_servicio_EE, id_paquete,
                    documento, ws_item, descripcion_tramite, this.sistema_SADE, username_SADE, usuarios_firmantes, userName_SADE_receptor);
                
                if(id_devolucion_ee > 0)
                    realizado_en_pasarela = true;

                resultado_ee = "";

                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------

               int re = db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, resultado_ee, userid);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }


        }

        private bool revisionFirma(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "Disposición No Firmada";
            int id_devolucion_ee = -1;
            string descripcion_tramite = "";
            
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            serviceEE.Url = this.url_servicio_EE;
            try
            {
                ws_ExpedienteElectronico.wsResultado result = serviceEE.EjecutarPaquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);
          
                ws_ExpedienteElectronico.dsInfoPaquete dsInfo = serviceEE.Get_Info_Paquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);

                //orderno la lista en forma descendiente y trae mas de un registro
                 if (dsInfo.Tables["TareasAlaFirma"].Rows.Count > 1)
                  {
                    ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row = (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow)dsInfo.Tables["TareasAlaFirma"].Rows[dsInfo.Tables["TareasAlaFirma"].Rows.Count - 1];
                    id_devolucion_ee = row.id_tarea_documento;
                    if (row.firmado)
                    {
                        realizado_en_pasarela = true;
                       resultado_ee = row.firmado_numeroGEDO;
                    }

                 }
                 else
                 {
                    if (dsInfo.Tables["TareasAlaFirma"].Rows.Count > 0)
                        {
                            ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row = (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow)dsInfo.Tables["TareasAlaFirma"].Rows[0];
                            id_devolucion_ee = row.id_tarea_documento;

                            if (row.firmado)
                            {
                                realizado_en_pasarela = true;
                                resultado_ee = row.firmado_numeroGEDO;
                            }
                        }
                }               
                

                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------

                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, resultado_ee, userid);
                return realizado_en_pasarela;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, error, userid);
                return false;
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }
        }

        private void relacionarTareaALaFirma(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            string descripcion_tramite = "";
            string NombreSistema = Constants.ApplicationName;
            
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            serviceEE.Url = this.url_servicio_EE;
            try
            {

                //Recupero el usuario_Sade
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);


                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }


                int id_caratula = 0;
                var tp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                

                var texp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete && x.id_proceso == (int) Constants.SGI_Procesos_EE.GEN_CARATULA);
                if(texp != null && texp.id_devolucion_ee.HasValue)
                    id_caratula = texp.id_devolucion_ee.Value;
                    
                var texp_ant = db.SGI_Tarea_Generar_Expediente_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete);
                if( texp_ant != null)
                    id_caratula = texp_ant.id_caratula;

                ws_ExpedienteElectronico.dsInfoPaquete dsInfo = serviceEE.Get_Info_Paquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);

                ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row = (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow)dsInfo.Tables["TareasAlaFirma"].Rows[0];

                if (row.firmado)
                {
                    id_devolucion_ee = serviceEE.Relacionar_TareasDocumento(this.username_servicio_EE, this.pass_servicio_EE,
                        id_caratula, row.id_tarea_documento, NombreSistema, username_SADE);
                }

                if(id_devolucion_ee > 0)
                    realizado_en_pasarela = true;

                resultado_ee = "";

                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------

                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, resultado_ee, userid);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }

        }

        private void relacionarDocumento(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            string descripcion_tramite = "";
            string NombreSistema = Constants.ApplicationName;

            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            serviceEE.Url = this.url_servicio_EE;
            try
            {

                int id_caratula = 0;
                var tp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);

                if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP ||
                    this.id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                {
                    var texp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete && x.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_CARATULA);
                    if (texp != null && texp.id_devolucion_ee.HasValue)
                        id_caratula = texp.id_devolucion_ee.Value;
                }
                else
                {
                    // Habilitaciones SGI 2.0
                    var texp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete && x.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_CARATULA);
                    if (texp != null && texp.id_devolucion_ee.HasValue)
                        id_caratula = texp.id_devolucion_ee.Value;
                    else
                    {
                        // Habilitaciones SGI 1.0
                        var texp1 = db.SGI_Tarea_Generar_Expediente_Procesos.FirstOrDefault(x => x.id_paquete == tp.id_paquete);
                        if (texp1 != null)
                            id_caratula = texp1.id_caratula;
                    }
                }

                dynamic parametros = null;
                if (tp.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tp.parametros_SADE);

                string username_SADE = "";
                try
                {
                    username_SADE = parametros.Usuario_SADE.Value;
                }
                catch (Exception)
                {
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                int id_documento = tp.id_origen_reg.Value;

                id_devolucion_ee = serviceEE.Relacionar_Documento(this.username_servicio_EE, this.pass_servicio_EE,
                                id_caratula, id_documento, NombreSistema, username_SADE);

                if (id_devolucion_ee > 0)
                    realizado_en_pasarela = true;

                resultado_ee = "";

                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------

                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, resultado_ee, userid);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, error, userid);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }

        }

        public string obtenerNroExpediente(int id_paquete)
        {
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            serviceEE.Url = this.url_servicio_EE;
            string NroExpediente = "";

            ws_ExpedienteElectronico.dsInfoPaquete dsInfo = serviceEE.Get_Info_Paquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);

            foreach (ws_ExpedienteElectronico.dsInfoPaquete.CaratulaRow row in dsInfo.Tables["Caratula"].Rows)
            {
                NroExpediente = row.resultado;
            }

            return NroExpediente;
        }

        public void obtenerResultadoSADE(int id_tarea_proc, int id_paquete, Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            bool NOTAD = false;
            bool NOGP = false;
            bool.TryParse(Functions.GetParametroChar("SGI.NO.TAD"), out NOTAD);
            bool.TryParse(Functions.GetParametroChar("SGI.NO.GP"), out NOGP);

            try
            {
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                string username_SADE = Functions.GetUsernameSADE(userid);
                string resultado_ee = "";

                ws_ExpedienteElectronico.dsInfoPaquete dsInfo = serviceEE.Get_Info_Paquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);

                var proceso = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                
                if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_CARATULA)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.CaratulaRow row in dsInfo.Tables["Caratula"].Rows)
                    {
                        resultado_ee = row.resultado;

                        db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.generada, row.fecha_resultado, row.resultado, userid);
                        //Guardo el numero de expediente
                        string cod_trata = null;
                        string direccion = null;
                        int idTAD = 0;

                        if (id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                        {
                            if(proceso.realizado_en_SADE)
                                db.SGI_Nro_Expediente_Sade_Actualizar(id_solicitud, row.resultado);

                            var datos = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                         join sol in db.SSIT_Solicitudes on tt_hab.id_solicitud equals sol.id_solicitud
                                         where tt_hab.id_tramitetarea == id_tramitetarea
                                         select new
                                         {
                                             sol.id_tipotramite,
                                             sol.id_subtipoexpediente,
                                             sol.id_solicitud,
                                             sol.idTAD,
                                             tt_hab.SGI_Tramites_Tareas.id_tarea
                                         }).FirstOrDefault();
                            if (datos.idTAD != null) {
                                idTAD = datos.idTAD.Value;

                                #region recupero el codigo trata
                                //bool esEscuela = false;
                                //if (datos.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Generar_Expediente
                                //    || datos.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Generar_Expediente)
                                //    esEscuela = true;


                                //if (datos.id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion)
                                //{
                                //    cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Ampliacion");
                                //}
                                //else if (datos.id_tipotramite == (int)Constants.TipoDeTramite.RedistribucionDeUso)
                                //{
                                //    cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.RedistribucionDeUso");
                                //}
                                //else if (esEscuela)
                                //{
                                //    cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.Escuela");
                                //}
                                //else
                                //{
                                //    switch (datos.id_subtipoexpediente)
                                //    {
                                //        case 1: // SIN_PLANOS
                                //            cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleSinplanos");
                                //            break;
                                //        case 2: // CON_PLANOS
                                //            decimal sup = 0;
                                //            var enc = db.Encomienda.Where(x => x.id_solicitud == datos.id_solicitud
                                //                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                                //            sup = enc.Encomienda_DatosLocal.First().superficie_cubierta_dl.Value + enc.Encomienda_DatosLocal.First().superficie_descubierta_dl.Value;

                                //            if (sup < 100)
                                //                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.A");
                                //            else if (sup < 500)
                                //                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.B");
                                //            else
                                //                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.C");
                                //            break;
                                //        case 3: //INSPECCION_PREVIA
                                //            cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.InspeccionPrevia");
                                //            break;
                                //        case 4: //Habilitacion_PREVIA
                                //            cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Habilitacion.InspeccionPrevia");
                                //            break;
                                //        default:
                                //            break;
                                //    }
                                //}
                                #endregion
                                string sql = "select dbo.SGI_DireccionesPartidasPlancheta(" + id_solicitud + ")";
                                direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                            }
                        }
                        else if (id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
                        {
                            db.SGI_CPadron_Nro_Expediente_Sade_Actualizar(id_solicitud, row.resultado);
                            var cp = db.CPadron_Solicitudes.Where(x => x.id_cpadron == id_solicitud).First();
                            if (cp.idTAD != null)
                            {
                                idTAD = cp.idTAD.Value;
                                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Consulta.Padron");
                                string sql = "select dbo.CPadron_Solicitud_DireccionesPartidas(" + id_solicitud + ")";
                                direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                            }
                        }
                        else if (id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
                        {
                            //Parece que aca estaba el bug que borraba los EE de las transferencias
                            //db.SGI_Transf_Nro_Expediente_Sade_Actualizar(id_solicitud, row.resultado);
                            var tra = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_solicitud).First();
                            if (tra.idTAD != null)
                            {
                                idTAD = tra.idTAD.Value;
                                cod_trata = Parametros.GetParamEE_ValorChar("EE.Trata.Transferencias");
                                string sql = "select dbo.CPadron_Solicitud_DireccionesPartidas(" + tra.id_cpadron + ")";
                                direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                            }
                        }

                        //Informe al TAD
                        if (idTAD != 0 && row.generada)
                        {
                            string _urlESB = Functions.GetParametroChar("Url.Service.ESB");
                            if (!NOGP)
                                wsGP.asociarExpediente(_urlESB, idTAD, row.resultado);
                            if (!NOTAD)
                                wsTAD.actualizarTramite(_urlESB, idTAD, id_solicitud, row.resultado, cod_trata, direccion);
                        }
                        else
                            throw new Exception(row.resultado);
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_DOCUMENTO ||
                         proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_PLANO ||
                         proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_PROVIDENCIA ||
                         proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_CERTIFICADO ||
                         proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_OBSERVACIONES)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.DocumentosRow row in dsInfo.Tables["Documentos"].Rows)
                    {
                        if (row.id_documento == proceso.id_devolucion_ee)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.subido, row.fecha_resultado, row.resultado, userid);

                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.RelacionesDocumentosRow rowRel in dsInfo.Tables["RelacionesDocumentos"].Rows)
                            {
                                if (rowRel.id_documento == row.id_documento && row.subido)
                                {
                                    if (!rowRel.realizado)
                                        db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, rowRel.realizado, rowRel.fecha_resultado, row.resultado + " (Relación:" +  rowRel.resultado + ")", userid);
                                }
                            }

                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.RELACIONAR_DOCUMENTO)
                {
                    //Relación de documentos
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.RelacionesDocumentosRow row in dsInfo.Tables["RelacionesDocumentos"].Rows)
                    {
                        if (row.id_relacion == proceso.id_devolucion_ee && proceso.id_origen_reg.HasValue)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.realizado, row.fecha_resultado, resultado_ee, userid);
                        }

                    }

                    //Relacionar las disposiciones.
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row in dsInfo.Tables["TareasAlaFirma"].Rows)
                    {
                        if (proceso.id_devolucion_ee == -1 && row.id_tarea_documento > 0)
                        {
                            db.SGI_SADE_Procesos_update(id_tarea_proc, true, proceso.descripcion_tramite, row.id_tarea_documento, proceso.resultado_ee, userid);
                        }

                        if (row.id_tarea_documento > 0)
                        {
                            resultado_ee = "";
                            if (row.relacion_subido)
                                resultado_ee = row.relacion_resultado;

                            if (Functions.EsAmbienteDesa())
                                db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, true, row.relacion_fecha_resultado, resultado_ee, userid);
                            else
                                db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.relacion_subido, row.relacion_fecha_resultado, resultado_ee, userid);
                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_PASE)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.PasesRow row in dsInfo.Tables["Pases"].Rows)
                    {
                        if (row.id_pase == proceso.id_devolucion_ee)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.generado, row.fecha_resultado, row.resultado, userid);
                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.DESBLOQUEO_EXPEDIENTE)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.DesbloqueosRow row in dsInfo.Tables["Desbloqueos"].Rows)
                    {
                        if (row.id_desbloqueo == proceso.id_devolucion_ee)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.desbloqueado, row.fecha_resultado, row.resultado, userid);
                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.BLOQUEO_EXPEDIENTE)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.BloqueosRow row in dsInfo.Tables["Bloqueos"].Rows)
                    {
                        if (row.id_bloqueo == proceso.id_devolucion_ee)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.bloqueado, row.fecha_resultado, row.resultado, userid);
                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row in dsInfo.Tables["TareasAlaFirma"].Rows)
                    {
                        if (row.id_tarea_documento == proceso.id_devolucion_ee)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.subido, row.fecha_subida, row.resultado, userid);
                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.REVISION_DE_FIRMA)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row in dsInfo.Tables["TareasAlaFirma"].Rows)
                    {

                        if (proceso.id_devolucion_ee == -1 && row.id_tarea_documento > 0)
                        {
                            db.SGI_SADE_Procesos_update(id_tarea_proc, true, proceso.descripcion_tramite, row.id_tarea_documento, proceso.resultado_ee, userid);
                        }
                        if (row.id_tarea_documento > 0)
                        {
                            if (row.firmado)
                                resultado_ee = row.firmado_numeroGEDO;
                            else
                                resultado_ee = "Disposición pendiente de firma.";

                            if (Functions.EsAmbienteDesa())
                            {
                                db.SGI_SADE_Procesos_update(id_tarea_proc, true, proceso.descripcion_tramite, row.id_tarea_documento, proceso.resultado_ee, userid);
                                db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, true, row.fecha_firmado, "Disposicion Firmada. Forzado por Entorno de Desarrollo", userid);
                            }
                            else
                                db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.firmado, row.fecha_firmado, resultado_ee, userid);
                        }
                    }
                }
                else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.RELACIONAR_EXPEDIENTE)
                {
                    foreach (ws_ExpedienteElectronico.dsInfoPaquete.RelacionesExpedientesRow row in dsInfo.Tables["RelacionesExpedientes"].Rows)
                    {
                        if (row.id_relacion == proceso.id_devolucion_ee)
                        {
                            resultado_ee = row.resultado;
                            db.SGI_SADE_Procesos_updateDatosSADE(id_tarea_proc, row.realizado, row.fecha_resultado, row.resultado, userid);
                        }
                    }
                }
                serviceEE.Dispose();


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los datos -> GetInfoPaquete: " + ex.InnerException.Message);
                else
                    throw new Exception("Error al obtener los datos -> GetInfoPaquete: " + ex.Message);
            }
        }

        private bool validarProcesoCaratulaEnSADE()
        {
            bool ret = true;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            // valida que la caratula haya sido subida a SADE
            int[] arrProcesos_a_validar = new int[] { (int)Constants.SGI_Procesos_EE.GEN_CARATULA };
            int id_paquete = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tramitetarea == this.id_tramitetarea).id_paquete;
            int cantidad_No_subida_SADE = 0;

            if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.CP)
            {

                cantidad_No_subida_SADE = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == this.id_tramitetarea &&
                                                                                            arrProcesos_a_validar.Contains(x.id_proceso) && !x.realizado_en_SADE);

            }
            else
            {
                cantidad_No_subida_SADE = db.SGI_SADE_Procesos.Count(x => x.id_paquete == id_paquete &&
                                                                                            arrProcesos_a_validar.Contains(x.id_proceso) && !x.realizado_en_SADE);

            }
            if (cantidad_No_subida_SADE > 0)
                ret = false;

            db.Dispose();

            return ret;
        }

        private bool validarProcesoFirmaDispoEnSADE()
        {
            bool ret = true;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            // valida que la dispo haya sido firmada en SADE
            int[] arrProcesos_a_validar = new int[] { (int)Constants.SGI_Procesos_EE.REVISION_DE_FIRMA };
            int id_paquete = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tramitetarea == this.id_tramitetarea).id_paquete;
            int cantidad_No_subida_SADE = 0;

            cantidad_No_subida_SADE = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == this.id_tramitetarea &&
                                                                                            arrProcesos_a_validar.Contains(x.id_proceso) && !x.realizado_en_SADE);
            if (cantidad_No_subida_SADE > 0)
                ret = false;

            db.Dispose();

            return ret;
        }

        private bool validarProcesosRealizadosEnSADE(int id_tarea_proc, int id_tramitetarea)
        {
            bool procesado_en_SADE = true;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;


            try
            {

                int cantidad_No_subida_SADE = db.SGI_SADE_Procesos.Count(x => (x.id_tramitetarea == id_tramitetarea &&
                                                                       x.id_tarea_proc < id_tarea_proc
                                                                       && x.id_proceso != (int)Constants.SGI_Procesos_EE.GEN_PAQUETE) && !x.realizado_en_SADE);
                if (cantidad_No_subida_SADE > 0)
                {
                    procesado_en_SADE = false;
                }
                else
                {


                    ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                    serviceEE.Url = this.url_servicio_EE;
                    string resultado_ee = "";


                    var lstProcesos = db.SGI_SADE_Procesos.Where(x => x.id_tramitetarea == id_tramitetarea &&
                                                                 x.id_tarea_proc <= id_tarea_proc &&
                                                                 x.id_proceso != (int)Constants.SGI_Procesos_EE.GEN_PAQUETE &&
                                                                 x.realizado_en_pasarela  );

                    ws_ExpedienteElectronico.dsInfoPaquete dsInfo = null;

                    foreach (var proceso in lstProcesos)
                    {

                        if (dsInfo == null)
                            dsInfo = serviceEE.Get_Info_Paquete(this.username_servicio_EE, this.pass_servicio_EE, proceso.id_paquete);


                        if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_CARATULA)
                        {
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.CaratulaRow row in dsInfo.Tables["Caratula"].Rows)
                            {
                                resultado_ee = row.resultado;
                                if (!row.generada)
                                    procesado_en_SADE = false;
                            }
                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_DOCUMENTO ||
                                    proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_PLANO ||
                                    proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_PROVIDENCIA ||
                                    proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_CERTIFICADO ||
                                    proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_OBSERVACIONES)
                        {
                            // Chequea que el documento este subido en SADE y su relación tambien.
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.DocumentosRow row in dsInfo.Tables["Documentos"].Rows)
                            {

                                if (row.id_documento == proceso.id_devolucion_ee)
                                {
                                    if (row.subido)
                                    {
                                        foreach (ws_ExpedienteElectronico.dsInfoPaquete.RelacionesDocumentosRow rowRel in dsInfo.Tables["RelacionesDocumentos"].Rows)
                                        {
                                            if (rowRel.id_documento == row.id_documento)
                                            {
                                                if (!rowRel.realizado)
                                                    procesado_en_SADE = false;
                                            }
                                        }
                                    }
                                    else
                                        procesado_en_SADE = false;

                                }
                            }
                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.RELACIONAR_DOCUMENTO)
                        {
                            //Relación de documentos
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.RelacionesDocumentosRow row in dsInfo.Tables["RelacionesDocumentos"].Rows)
                            {
                                if (row.id_relacion == proceso.id_devolucion_ee && proceso.id_origen_reg.HasValue)
                                {
                                    if (!row.realizado)
                                        procesado_en_SADE = false;
                                }

                            }

                            //Relación de disposiciones.
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row in dsInfo.Tables["TareasAlaFirma"].Rows)
                            {

                                if (row.id_relacion == proceso.id_devolucion_ee && !proceso.id_origen_reg.HasValue)
                                {
                                    if (!row.relacion_subido)
                                        procesado_en_SADE = false;

                                }
                            }
                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_PASE)
                        {
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.PasesRow row in dsInfo.Tables["Pases"].Rows)
                            {
                                if (row.id_pase == proceso.id_devolucion_ee)
                                {
                                    if (!row.generado)
                                        procesado_en_SADE = false;
                                }
                            }

                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.DESBLOQUEO_EXPEDIENTE)
                        {
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.DesbloqueosRow row in dsInfo.Tables["Desbloqueos"].Rows)
                            {
                                if (row.id_desbloqueo == proceso.id_devolucion_ee)
                                {
                                    if (!row.desbloqueado)
                                        procesado_en_SADE = false;
                                }
                            }

                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.BLOQUEO_EXPEDIENTE)
                        {
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.BloqueosRow row in dsInfo.Tables["Bloqueos"].Rows)
                            {
                                if (row.id_bloqueo == proceso.id_devolucion_ee)
                                {
                                    if (!row.bloqueado)
                                        procesado_en_SADE = false;
                                }
                            }

                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA)
                        {
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row in dsInfo.Tables["TareasAlaFirma"].Rows)
                            {
                                if (row.id_tarea_documento == proceso.id_devolucion_ee)
                                {
                                    if (!row.subido)
                                        procesado_en_SADE = false;
                                }
                            }

                        }
                        else if (proceso.id_proceso == (int)Constants.SGI_Procesos_EE.REVISION_DE_FIRMA)
                        {
                            foreach (ws_ExpedienteElectronico.dsInfoPaquete.TareasAlaFirmaRow row in dsInfo.Tables["TareasAlaFirma"].Rows)
                            {
                                if (proceso.id_devolucion_ee == row.id_tarea_documento)
                                {
                                    if (!row.firmado)
                                        if (!Functions.EsAmbienteDesa())
                                            procesado_en_SADE = false;
                                }
                            }
                        }

                    }

                    serviceEE.Dispose();

                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error en la función validarProcesosRealizadosEnSADE() : " + ex.InnerException.Message);
                else
                    throw new Exception("Error en la función validarProcesosRealizadosEnSADE() : " + ex.Message);
            }

            return procesado_en_SADE;
        }

        #endregion

        protected void btnCerrarModalProcesos_Click(object sender, EventArgs e)
        {
            // Solo carga la lista sin ejecutar los procesos
            cargarDatosProcesos(this.id_tramitetarea, false);


            if (!hayProcesosPendientesSADE(this.id_tramitetarea))
            {
                if (FinalizadoEnSADE != null)
                    FinalizadoEnSADE(sender, new EventArgs());
            }


            ScriptManager.RegisterStartupScript(updCerrarProcesos, updCerrarProcesos.GetType(), "script", "ocultarfrmProcesarEE();", true);
                
            
        }

        private byte[] Get_Providencia_HTML(int id_tarea_proc)
        {
            
            byte[] ret = null;
            
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string texto_providencia = "";
            string ProvidenciaHTML = "";

            try
            {
                
                var tp = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                if (tp != null)
                {
                    int id_solicitud = Functions.Get_id_solicitud(tp.id_tramitetarea);
                    int id_grupotramite = Functions.Get_id_grupotramite(tp.id_tramitetarea);

                    // -------------------------------------
                    // Providencia del Gerente a la privada
                    // -------------------------------------
                    if ( tp.id_origen_reg == (int) Constants.Tipo_Providencia.Gerente_a_DGHyP)
                    {
                        // -------------------------------------
                        // Providencia de Habilitaciones
                        // -------------------------------------
                        if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Revision_Gerente
                                                 join tt_hab in db.SGI_Tramites_Tareas_HAB on tarea.id_tramitetarea equals tt_hab.id_tramitetarea
                                                 where tt_hab.id_solicitud == id_solicitud
                                                 select new
                                                 {
                                                     tarea.id_tramitetarea,
                                                     tarea.observacion_providencia
                                                 }).Union(
                                                from tarea in db.SGI_Tarea_Revision_SubGerente
                                                join tt_hab in db.SGI_Tramites_Tareas_HAB on tarea.id_tramitetarea equals tt_hab.id_tramitetarea
                                                where tt_hab.id_solicitud == id_solicitud
                                                select new
                                                {
                                                    tarea.id_tramitetarea,
                                                    tarea.observacion_providencia
                                                }).OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.observacion_providencia;
                            else
                                texto_providencia = Parametros.GetParam_ValorChar("Texto.Providencia.al.Director");
                        }

                        // -------------------------------------
                        // Providencia de Consulta al Padron
                        // -------------------------------------
                        else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Revision_Gerente
                                                 join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tarea.id_tramitetarea equals tt_cp.id_tramitetarea
                                                 where tt_cp.id_cpadron == id_solicitud
                                                 orderby tarea.id_revision_gerente descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.observacion_providencia;
                        }

                        // -------------------------------------
                        // Providencia de Transferencias
                        // -------------------------------------
                        else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Revision_Gerente
                                                 join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tarea.id_tramitetarea equals tt_transf.id_tramitetarea
                                                 where tt_transf.id_solicitud == id_solicitud
                                                 orderby tarea.id_revision_gerente descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.observacion_providencia;


                        }   
                    }
                    // -------------------------------------
                    // Providencia del Calificador a SubGerente
                    // -------------------------------------
                    else if (tp.id_origen_reg == (int)Constants.Tipo_Providencia.Calificador_a_SubGerente)
                    {
                        // -------------------------------------
                        // Providencia de Habilitaciones
                        // -------------------------------------
                        if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Calificar
                                                 join tt_hab in db.SGI_Tramites_Tareas_HAB on tarea.id_tramitetarea equals tt_hab.id_tramitetarea
                                                 where tt_hab.id_solicitud == id_solicitud
                                                 orderby tarea.id_calificar descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.Observaciones_Providencia;
                        }

                        // -------------------------------------
                        // Providencia de Consulta al Padron
                        // -------------------------------------
                        else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Calificar
                                                 join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tarea.id_tramitetarea equals tt_cp.id_tramitetarea
                                                 where tt_cp.id_cpadron == id_solicitud
                                                 orderby tarea.id_calificar descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.Observaciones_Providencia;
                        }

                        // -------------------------------------
                        // Providencia de Transferencias
                        // -------------------------------------
                        else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Calificar
                                                 join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tarea.id_tramitetarea equals tt_transf.id_tramitetarea
                                                 where tt_transf.id_solicitud == id_solicitud
                                                 orderby tarea.id_calificar descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.Observaciones_Providencia;
                        }
                    }

                    // -------------------------------------
                    // Providencia del SubGerente a Gerente
                    // -------------------------------------
                    else if (tp.id_origen_reg == (int)Constants.Tipo_Providencia.SubGerente_a_Gerente)
                    {
                        // -------------------------------------
                        // Providencia de Habilitaciones
                        // -------------------------------------
                        if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Revision_SubGerente
                                                 join tt_hab in db.SGI_Tramites_Tareas_HAB on tarea.id_tramitetarea equals tt_hab.id_tramitetarea
                                                 where tt_hab.id_solicitud == id_solicitud
                                                 orderby tarea.id_revision_subGerente descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.observacion_providencia;
                        }

                        // -------------------------------------
                        // Providencia de Consulta al Padron
                        // -------------------------------------
                        else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Revision_SubGerente
                                                 join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tarea.id_tramitetarea equals tt_cp.id_tramitetarea
                                                 where tt_cp.id_cpadron == id_solicitud
                                                 orderby tarea.id_revision_subGerente descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.observacion_providencia;
                        }

                        // -------------------------------------
                        // Providencia de Transferencias
                        // -------------------------------------
                        else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                        {
                            var tarea_gerente = (from tarea in db.SGI_Tarea_Revision_SubGerente
                                                 join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tarea.id_tramitetarea equals tt_transf.id_tramitetarea
                                                 where tt_transf.id_solicitud == id_solicitud
                                                 orderby tarea.id_revision_subGerente descending
                                                 select tarea).FirstOrDefault();

                            if (tarea_gerente != null)
                                texto_providencia = tarea_gerente.observacion_providencia;


                        }
                    }
                }

                if (tp.id_origen_reg == (int)Constants.Tipo_Providencia.Gerente_a_DGHyP
                    || tp.id_origen_reg == (int)Constants.Tipo_Providencia.Calificador_a_SubGerente
                    || tp.id_origen_reg == (int)Constants.Tipo_Providencia.SubGerente_a_Gerente)
                {

                    ProvidenciaHTML = File.ReadAllText(Server.MapPath(@"~\Reportes\Providencia.html"));
                    if (string.IsNullOrEmpty(texto_providencia))
                        texto_providencia = "";

                    texto_providencia = HttpUtility.HtmlEncode(texto_providencia);
                    texto_providencia = texto_providencia.Replace("\n", "\n <br/>");

                    ProvidenciaHTML = ProvidenciaHTML.Replace("@Texto", texto_providencia);

                    ret = Encoding.GetEncoding("ISO-8859-1").GetBytes(ProvidenciaHTML);
                }
                else if (tp.id_origen_reg == (int)Constants.Tipo_Providencia.Gerente_a_AVH)
                {
                    ProvidenciaHTML = File.ReadAllText(Server.MapPath(@"~\Reportes\Providencia_AVH.html"));
                    ret = Encoding.GetEncoding("ISO-8859-1").GetBytes(ProvidenciaHTML);
                }
                else if (tp.id_origen_reg == (int)Constants.Tipo_Providencia.Gerente_a_DGFYC)
                {
                    ProvidenciaHTML = File.ReadAllText(Server.MapPath(@"~\Reportes\Providencia_DGFYC.html"));
                    ret = Encoding.GetEncoding("ISO-8859-1").GetBytes(ProvidenciaHTML);
                }
             }
            catch (Exception ex)
            {
                ret = null;
                throw ex;
            }
            finally
            {
                db.Dispose();
            }

            return ret;

        }

      
        private void subirPlano(int id_tarea_proc, int id_paquete, int id_file, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();
            dbFiles.Database.CommandTimeout = 300;
            bool realizado_en_pasarela = false;
            string resultado_ee = "";
            int id_devolucion_ee = -1;
            bool huboError = false;
            string acronimo_SADE = "";
            string nombreArchivo = "";

            try
            {
                byte[] documento = null;

                // Recupero los parámetros de la tarea de proceso
                // ----------------------------------------------
                var proc = db.SGI_SADE_Procesos.FirstOrDefault(x => x.id_tarea_proc == id_tarea_proc);
                dynamic parametros = null;
                if (proc.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(proc.parametros_SADE);

                string username_SADE = "";
                try { username_SADE = parametros.Usuario_SADE.Value; }
                catch (Exception) { username_SADE = Functions.GetUsernameSADE(userid); }


                int id_encomienda_plano = (proc.id_origen_reg.HasValue ? proc.id_origen_reg.Value : 0);
                var datosPlano = (from encplan in db.Encomienda_Planos
                                  join tplan in db.TiposDePlanos on encplan.id_tipo_plano equals tplan.id_tipo_plano
                                  where encplan.id_encomienda_plano == id_encomienda_plano
                                  select new
                                  {
                                      encplan.nombre_archivo,
                                      tplan.acronimo_SADE,
                                      tplan.requiere_detalle,
                                      encplan.detalle,
                                      tplan.nombre
                                  }).FirstOrDefault();

                if (datosPlano != null)
                {
                    descripcion_tramite = datosPlano.requiere_detalle.Value && !string.IsNullOrEmpty(datosPlano.detalle) ? datosPlano.detalle : datosPlano.nombre;
                    nombreArchivo = datosPlano.nombre_archivo;
                    acronimo_SADE = datosPlano.acronimo_SADE;
                }
                else
                    throw new Exception(string.Format("No se ha encontrado la información del plano, id_encomiendaplano = {0}.", id_encomienda_plano));

                documento = ws_FilesRest.DownloadFile(id_file);

                if (documento.Length <= 1)
                {
                    throw new Exception("No se encontró el contenido del archivo en la base de Files.");
                }


                // Subir y relacionar documento en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {
                    string tipoArchivo =  "txt";
                    string identificacion_documento = nombreArchivo;    // En el caso de los planos se usa la identificacion del documento como nombre del archivo embebido en SADE
                    
                    id_devolucion_ee = serviceEE.Subir_Documentos_Embebidos_ConAcroAndTipo( 
                        this.username_servicio_EE, this.pass_servicio_EE, id_paquete,
                        documento, identificacion_documento, descripcion_tramite,
                        this.sistema_SADE, username_SADE, acronimo_SADE, tipoArchivo, nombreArchivo);
                    realizado_en_pasarela = true;
                }
                catch (Exception ex)
                {
                    realizado_en_pasarela = false;
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (!huboError)
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
                dbFiles.Dispose();
            }

        }
    }
   
}
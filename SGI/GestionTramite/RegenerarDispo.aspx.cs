using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Core.Objects;
using SGI.Model;
using SGI.GestionTramite.Controls;
using System.IO;
using System.Text;
using System.Data;

namespace SGI.GestionTramite
{
    public partial class RegenerarDispo : BasePage
    {
        DGHP_Entities db = null;
        ucProcesosSADEv1 ucPS = new ucProcesosSADEv1();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);

            }
        }

        #region Entity
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

        private int _id_grupotramite = 0;
        public int id_grupotramite
        {
            get
            {
                if (_id_grupotramite == 0)
                {
                    int.TryParse(hid_id_grupotramite.Value, out _id_grupotramite);
                }
                return _id_grupotramite;
            }
            set
            {
                hid_id_grupotramite.Value = value.ToString();
                _id_grupotramite = value;
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
        private void generarTareaAlaFirma(int id_tarea_proc, int id_paquete, string html_dispo, Guid userid, int idProc)
        {

            DGHP_Entities db = new DGHP_Entities();
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
                    userid = (Guid)db.SGI_Tarea_Generar_Expediente_Procesos.Where(x => x.id_generar_expediente_proc == id_tarea_proc).Select(x => x.UpdateUser).FirstOrDefault();
                    username_SADE = Functions.GetUsernameSADE(userid);
                }

                ws_ExpedienteElectronico.WS_Item[] ws_item = null;
                string NroExpediente = ucPS.obtenerNroExpediente(id_paquete);
                string userName_SADE_Director = SGI.Parametros.GetParam_ValorChar("SGI.Username.Director.Habilitaciones");
                string userName_SADE_receptor = SGI.Parametros.GetParam_ValorChar("SGI.Username.Receptor.Habilitaciones");
                byte[] documento = Encoding.UTF8.GetBytes(html_dispo);
                descripcion_tramite = this.id_solicitud.ToString() + " (" + NroExpediente + ")";


                if (userName_SADE_Director.Length == 0)
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

                if (id_devolucion_ee > 0)
                {
                    realizado_en_pasarela = true;

                    ws_ExpedienteElectronico.dsResultadoFirma consultarResultadoFirma = serviceEE.consultarResultadoFirma(this.username_servicio_EE, this.pass_servicio_EE, id_devolucion_ee);

                    ws_ExpedienteElectronico.dsResultadoFirma.ResultadoRow row = (ws_ExpedienteElectronico.dsResultadoFirma.ResultadoRow)consultarResultadoFirma.Tables["Resultado"].Rows[0];

                    if (!row.ejecutado_sade)
                    {
                         serviceEE.eliminarTareaDocumento(id_devolucion_ee);
                         throw new Exception(row.resultado_sade);
                    }
                }

                resultado_ee = "";

                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (idProc == 1)
                {
                    db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, resultado_ee, userid);
                }
                else
                {
                    var gep = db.SGI_Tarea_Generar_Expediente_Procesos.Where(x => x.id_generar_expediente_proc == id_tarea_proc).FirstOrDefault();
                    db.SGI_Tarea_Generar_Expediente_Procesos_update(id_tarea_proc, (int)Constants.SGI_Procesos.Generacion_disposicion_firma, id_devolucion_ee, gep.resultado_ee, gep.realizado, gep.UpdateUser, gep.nro_tramite, gep.descripcion_tramite);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, descripcion_tramite, id_devolucion_ee, error, userid);
                throw new Exception("", ex);
            }
            finally
            {
                serviceEE.Dispose();
                db.Dispose();
            }


        }
        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Regenerar Disposición");


            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }
        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        private int buscarTarea(int idSolicitud, int grupoTramite)
        {
            int idTramiteTarea = 0;
            if (grupoTramite == (int)Constants.GruposDeTramite.HAB)
            {                
                var Ultima_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                    join tt in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt.id_tramitetarea
                                    where tt_hab.id_solicitud == idSolicitud && tt.ENG_Tareas.cod_tarea.ToString().Substring(tt.ENG_Tareas.cod_tarea.ToString().Length -2, 2) == Constants.ENG_Tipos_Tareas.Generar_Expediente
                                    orderby tt_hab.id_tramitetarea descending
                                    select new
                                    {
                                       idTramiteTarea = tt_hab.id_tramitetarea
                                    }).FirstOrDefault();
                idTramiteTarea = Ultima_tarea.idTramiteTarea;
            }
            else if (grupoTramite == (int)Constants.GruposDeTramite.TR)
            {
                var Ultima_tarea = (from tt_t in db.SGI_Tramites_Tareas_TRANSF
                                    join tt in db.SGI_Tramites_Tareas on tt_t.id_tramitetarea equals tt.id_tramitetarea
                                    where tt_t.id_solicitud == idSolicitud && tt.ENG_Tareas.cod_tarea.ToString().Substring(tt.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Generar_Expediente
                                    orderby tt_t.id_tramitetarea descending
                                    select new
                                    {
                                        idTramiteTarea = tt_t.id_tramitetarea
                                    }).FirstOrDefault();
                idTramiteTarea = Ultima_tarea.idTramiteTarea;
            }
            return idTramiteTarea;
        }

        private int validarTarea(int idSolicitud, int grupoTramite)
        {
            string codTarea = null;
            string fechaCierre = null;
            int idTramiteTarea = 0;
            int result = -1;

            if (grupoTramite == (int)Constants.GruposDeTramite.HAB)
            {
                 var Ultima_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                    join tt in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt.id_tramitetarea
                                    where tt_hab.id_solicitud == idSolicitud
                                    orderby tt_hab.id_tramitetarea descending
                                    select new
                                    {
                                        codTar = tt.ENG_Tareas.cod_tarea,
                                        fechaCierre = tt.FechaCierre_tramitetarea,
                                        idTramiteTarea = tt_hab.id_tramitetarea
                                    }).FirstOrDefault();
                codTarea = Ultima_tarea.codTar.ToString().Substring(Ultima_tarea.codTar.ToString().Length - 2, 2);
                fechaCierre = Ultima_tarea.fechaCierre.ToString();
                idTramiteTarea = Ultima_tarea.idTramiteTarea;
            }
            else if (grupoTramite == (int)Constants.GruposDeTramite.TR)
            {
                var Ultima_tarea = (from tt_t in db.SGI_Tramites_Tareas_TRANSF
                                    join tt in db.SGI_Tramites_Tareas on tt_t.id_tramitetarea equals tt.id_tramitetarea
                                    where tt_t.id_solicitud == idSolicitud
                                    orderby tt_t.id_tramitetarea descending
                                    select new
                                    {
                                        codTar = tt.ENG_Tareas.cod_tarea,
                                        fechaCierre = tt.FechaCierre_tramitetarea,
                                        idTramiteTarea = tt_t.id_tramitetarea
                                    }).FirstOrDefault();
                codTarea = Ultima_tarea.codTar.ToString().Substring(Ultima_tarea.codTar.ToString().Length - 2, 2);
                fechaCierre = Ultima_tarea.fechaCierre.ToString();
                idTramiteTarea = Ultima_tarea.idTramiteTarea;
            }

            if ((fechaCierre == null || fechaCierre.Length == 0) && (codTarea == Constants.ENG_Tipos_Tareas.Revision_Firma_Disposicion || codTarea == Constants.ENG_Tipos_Tareas.Revision_Firma_Disposicion2))
                result = idTramiteTarea;

            return result;

        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {            
            SGI_SADE_Procesos tareaSade = new SGI_SADE_Procesos();

            try
            {
                IniciarEntity();

                if (txtSolicitud.Text.Length == 0)
                    throw new Exception("Debe ingresar un dato.");

                //int id_solicitud = 0;
                int.TryParse(txtSolicitud.Text.Trim(), out _id_solicitud);

                if (id_solicitud != 0)
                {
                    var solicitud = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);                    
                    var tr = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

                    if (solicitud == null && tr == null)
                        throw new Exception("No se puede encontrar la solicitud.");
                   
                    if (solicitud != null)
                        _id_grupotramite = (int)Constants.GruposDeTramite.HAB;
                    else if (tr != null)
                        _id_grupotramite = (int)Constants.GruposDeTramite.TR;
                }

                int idTramiteTarea = validarTarea(id_solicitud, _id_grupotramite);
                if (idTramiteTarea > 0)
                {
                    Guid userid = Functions.GetUserId();
                    tareaSade = db.SGI_SADE_Procesos.OrderByDescending(x => x.id_tramitetarea).FirstOrDefault(x => x.id_tramitetarea == idTramiteTarea && x.id_proceso == (int)Constants.EE_Procesos.FirmarDocumento_RevisarFirma);
                    
                    if (tareaSade != null)
                    {
                        string NroExpediente = ucPS.obtenerNroExpediente(tareaSade.id_paquete);
                        if (NroExpediente.Length > 0)
                        {
                            if (!tareaSade.realizado_en_pasarela)
                            {
                                string html_dispo = "";

                                idTramiteTarea = buscarTarea(id_solicitud, _id_grupotramite);

                                var tareaGenDisp = (from sgiSP in db.SGI_SADE_Procesos
                                                    where sgiSP.id_tramitetarea == idTramiteTarea && sgiSP.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA
                                                    select new {
                                                        idTT  = sgiSP.id_tramitetarea,
                                                        idPaq = sgiSP.id_paquete,
                                                        idTareaProc = sgiSP.id_tarea_proc,
                                                        idProc = 1
                                                    }).Union(
                                                    from stgep in db.SGI_Tarea_Generar_Expediente_Procesos
                                                    where stgep.id_tramitetarea == idTramiteTarea && stgep.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA
                                                    select new
                                                    {
                                                        idTT = (int)stgep.id_tramitetarea,
                                                        idPaq = stgep.id_paquete,
                                                        idTareaProc = stgep.id_generar_expediente_proc,
                                                        idProc = 2
                                                    }).FirstOrDefault();

                             
                                if (tareaGenDisp != null)
                                {
                                    if (this._id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                                    {
                                        PdfDisposicion dispo = new PdfDisposicion();
                                        html_dispo = dispo.GenerarHtml_Disposicion(id_solicitud, tareaGenDisp.idTT, NroExpediente);
                                    }
                                    else if (this._id_grupotramite == (int)Constants.GruposDeTramite.TR)
                                    {
                                        string str_archivo = "";                                        
                                        int nroTrReferencia = 0;
                                        int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
                                        if (this.id_solicitud <= nroTrReferencia)
                                        {
                                            str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\Disposicion.html"));
                                            html_dispo = PdfDisposicion.Transf_GenerarHtml_Disposicion(id_solicitud, tareaGenDisp.idTT, NroExpediente, str_archivo);
                                        }
                                        else
                                        {
                                            str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\DisposicionTransmision.html"));
                                            html_dispo = PdfDisposicion.Transmision_GenerarHtml_Disposicion(id_solicitud, tareaGenDisp.idTT, NroExpediente, str_archivo);
                                        }
                                    }
                                   
                                    if (!string.IsNullOrEmpty(html_dispo))
                                    {
                                        generarTareaAlaFirma(tareaGenDisp.idTareaProc, tareaGenDisp.idPaq, html_dispo, userid, tareaGenDisp.idProc);
                                        var idDevolucionEe = (from sadeproc in db.SGI_SADE_Procesos
                                                              where sadeproc.id_tramitetarea == idTramiteTarea && sadeproc.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA
                                                              select  (int)sadeproc.id_devolucion_ee ).Union(
                                                              from stgep in db.SGI_Tarea_Generar_Expediente_Procesos
                                                              where stgep.id_tramitetarea == idTramiteTarea && stgep.id_proceso == (int)Constants.SGI_Procesos_EE.GEN_TAREA_A_LA_FIRMA
                                                              select stgep.id_devolucion_ee ).FirstOrDefault();                                      
                                        db.SGI_SADE_Procesos_update(tareaSade.id_tarea_proc, tareaSade.realizado_en_pasarela, tareaSade.descripcion_tramite, idDevolucionEe, tareaSade.resultado_ee, userid);
                                    }                                    
                                    else
                                    {
                                        throw new Exception("No es posible obtener el html de la disposición.");
                                    }                                 
                                }
                                else
                                    throw new Exception("Esta solicitud no esta en condiciones de regenerar disposicion.");
                            }
                            Enviar_Mensaje("Se regenero la disposición.", "");
                        }
                    }
                    else
                        throw new Exception("Esta solicitud no esta en condiciones de regenerar disposicion.");
                }
                else
                    throw new Exception("Esta solicitud no esta en condiciones de regenerar disposicion.");

            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
            finally
            {
                FinalizarEntity();
            }
        }

    }
}
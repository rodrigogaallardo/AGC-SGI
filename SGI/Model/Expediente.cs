using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using System.Web.UI;
using System.Net;
using System.Data.Entity.Core.Objects;
using SGI.WebServices;
using System.Threading.Tasks;

namespace SGI.Model
{
    public class GetDocumentoReturn
    {
        public byte[] documento { get; set; }
        public string mensajeError { get; set; }
        public bool forzar_procesado { get; set; }
        public bool guardar_documento { get; set; }
    }

    public class GenerarPdf_Plancheta_HabilitacionReturn
    {
        public int retorno { get; set; }
        public byte[] documento { get; set; }
    }

    public class dtoSGI_SADE_Procesos
    {
        public int id_tarea_proc { get; set; }
        public int id_tramite_tarea { get; set; }
        public int id_proceso { get; set; }
        public string nombre_proceso { get; set; }
        public Nullable<int> id_origen_reg { get; set; }
        public bool realizado_en_pasarela { get; set; }
        public string descripcion_tramite { get; set; }
        public string class_resultado_SADE { get; set; }
        public string resultado_sade { get; set; }
        public DateTime? fecha_sade { get; set; }
        public int id_devolucion_ee { get; set; }
        public bool realizado_en_SADE { get; set; }
        public bool ejecutado_anteriormente { get; set; }
    }

    public class ProcesoExpediente
    {
        public int id_generar_expediente_proc { get; set; }
        public int id_tramite_tarea { get; set; }
        public int id_proceso { get; set; }
        public string nombre_proceso { get; set; }
        public bool realizado { get; set; }
        public bool realizado_en_sade { get; set; }
        public Nullable<int> nro_tramite { get; set; }
        public string nombre_tramite { get; set; }
        public string resultado_sade_error { get; set; }
        public string resultado_sade_ok { get; set; }
        public string resultado_alerta { get; set; }
        public DateTime? fecha_sade { get; set; }
        public int id_devolucion_ee{ get; set; }
        
        public ProcesoExpediente()
        {
            this.resultado_sade_ok = "";
            this.resultado_alerta = "";
        }

    }

    public class Expediente
    {

        #region atributos con datos de info paquete

        public int datos_caratula_id_caratula
        {
            get
            {
                int id_caratula = 0;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                {
                    int.TryParse(this.ds_getInfoPaquete.Tables["Caratula"].Rows[0]["id_caratula"].ToString(), out id_caratula);
                }

                return id_caratula;
            }
        }

        public string datos_caratula_nro_expediente
        {
            get
            {
                string expe_actuacion = "";

                string nro_actuacion  = this.datos_caratula_nro_actuacion;
                string anio_actuacion = this.datos_caratula_anio_actuacion;

                if (!string.IsNullOrEmpty(nro_actuacion) && !string.IsNullOrEmpty(nro_actuacion))
                    expe_actuacion = nro_actuacion + "/" + anio_actuacion;

                return expe_actuacion;
            }
        }
        public string datos_caratula_nro_expediente_completo
        {
            get
            {
                string resultado = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                {
                    resultado = Convert.ToString(ds_getInfoPaquete.Tables["Caratula"].Rows[0]["resultado"]);
                }

                return resultado;
            }
        }
        public string datos_caratula_tipo_actuacion
        {
            get
            {
                string tipo_actuacion = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                {
                    tipo_actuacion = Convert.ToString(this.ds_getInfoPaquete.Tables["Caratula"].Rows[0]["tipo_actuacion"]);
                }

                return tipo_actuacion;
            }
        }
        public string datos_caratula_anio_actuacion
        {
            get
            {
                string anio_actuacion = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                {
                    anio_actuacion = Convert.ToString(this.ds_getInfoPaquete.Tables["Caratula"].Rows[0]["anio_actuacion"]);
                }

                return anio_actuacion;
            }
        }
        public string datos_caratula_nro_actuacion
        {
            get
            {
                string nro_actuacion = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                {
                    nro_actuacion = Convert.ToString(this.ds_getInfoPaquete.Tables["Caratula"].Rows[0]["nro_actuacion"]);
                }

                return nro_actuacion;
            }
        }
        public bool datos_caratula_subida_sade   // Caratula_subida_sade
        {  
            get
            {
                bool genedara_sade = false;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                {
                    genedara_sade = (ds_getInfoPaquete.Tables["Caratula"].Rows[0]["generada"] == DBNull.Value) ? false : Convert.ToBoolean(ds_getInfoPaquete.Tables["Caratula"].Rows[0]["generada"]);
                }


                return genedara_sade;
            }
        }



        public bool datos_dispo_esta_firmada  
        {
            get
            {
                bool firmado = false;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    firmado = (ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["firmado"] == DBNull.Value) ? false : Convert.ToBoolean(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["firmado"]);
                }

                return firmado;
            }
        }
        public string datos_dispo_resultado
        {
            get
            {
                string resultado_firma = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    resultado_firma = Convert.ToString(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["resultado"]);
                }

                return resultado_firma;
            }
        }

        public bool datos_dispo_documento_subido
        {
            get
            {
                bool dispo_subido = false; ;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    dispo_subido = Convert.ToBoolean(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["subido"]);
                }

                return dispo_subido;
            }
        }

        public string datos_dispo_numeroGEDO
        {
            get
            {
                string numeroGEDO = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    numeroGEDO = Convert.ToString(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["firmado_numeroGEDO"]);
                }

                return numeroGEDO;
            }
        }

        public string datos_dispo_numeroGEDO_resultado
        {
            get
            {
                string numeroGEDO = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    numeroGEDO = Convert.ToString(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["firmado_resultado"]);
                }

                return numeroGEDO;
            }
        }

        public DateTime datos_dispo_fecha_resultado
        {
            get
            {
                DateTime fecha = DateTime.MinValue;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    fecha = Convert.ToDateTime(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["fecha_resultado"]);
                }

                return fecha; ;
            }
        }

        public bool datos_dispo_relacion_subido
        {
            get
            {
                bool relacion_subido = false;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    relacion_subido = (ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["relacion_subido"] == DBNull.Value) ? false : Convert.ToBoolean(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["relacion_subido"]);
                }

                return relacion_subido;
            }
        }

        public DateTime datos_dispo_relacion_fecha_resultado
        {
            get
            {
                DateTime fecha = DateTime.MinValue;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    fecha = Convert.ToDateTime(ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["relacion_fecha_resultado"]);
                }

                return fecha;
            }
        }

        public string datos_dispo_relacion_resultado
        {
            get
            {
                string resultado = "";

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    resultado = Convert.ToString(this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["relacion_resultado"]);
                }

                return resultado;
            }
        }

        public int datos_dispo_relacion_id_relacion
        {
            get
            {
                int id_relacion = 0;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    id_relacion = Convert.ToInt32(this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["id_relacion"]);
                }

                return id_relacion;
            }
        }

        public int datos_dispo_relacion_id_tarea_documento
        {
            get
            {
                int id_tarea_documento = 0;

                if (this.ds_getInfoPaquete != null && this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                {
                    id_tarea_documento = Convert.ToInt32(this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows[0]["id_tarea_documento"]);
                }

                return id_tarea_documento;
            }
        }

        #endregion

        public Expediente()
        {
            this.userid = Functions.GetUserId();
            this.NombreSistema = Constants.ApplicationName;
            inicializar();
        }

        public Expediente(Guid userid, string nombreSistema)
        {
            this.userid = userid;
            this.NombreSistema = nombreSistema;
            inicializar();
        }

        private void inicializar()
        {
            this.Reprocesar = false;
            IniciarEntity();
            IniciarEntityFiles();
        }

        public void Dispose()
        {
            FinalizarEntity();
            FinalizarEntityFiles();
        }

        #region Atributos publicos


        public string NombreSistema { get; set; }
        public Guid userid { get; set; }
        public bool Reprocesar{ get; set; }
        public SGI_Tarea_Generar_Expediente_Procesos proceso = null;
        public ws_ExpedienteElectronico.dsInfoPaquete ds_getInfoPaquete { get; set; }

        #endregion

        #region Atributos privados

        private string _ws_userName;
        private string WS_userName
        {
            get { 
                if ( string.IsNullOrEmpty(_ws_userName) )
                {
                    _ws_userName = SGI.Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
                }
                return _ws_userName; 
            }
        }

        private string _ws_pwd;
        private string WS_passWord
        {
            get {
                if (string.IsNullOrEmpty(_ws_pwd))
                {
                    _ws_pwd = SGI.Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
                }                
                return _ws_pwd; 
            }
        }

        private string _ws_url;
        private string WS_url
        {
            get
            {
                if (string.IsNullOrEmpty(_ws_url))
                {
                    _ws_url = SGI.Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                }
                return _ws_url;
            }
        }

        private string _userName_SADE;
        private string UserName_SADE
        {
            get
            {
                if (string.IsNullOrEmpty(_userName_SADE))
                {
                    _userName_SADE = this.db.SGI_Profiles.Where(x => x.userid == this.userid).Select(x => x.UserName_SADE).FirstOrDefault();
                }

                if (string.IsNullOrEmpty(_userName_SADE))
                    throw new ExpedienteException("No posee usuario SADE.");

                return _userName_SADE;
            }
        }

        private string _userName_SADE_Director;
        private string UserName_SADE_Director
        {

            get
            {
                if (string.IsNullOrEmpty(_userName_SADE_Director))
                {
                    _userName_SADE_Director = SGI.Parametros.GetParam_ValorChar("SGI.Username.Director.Habilitaciones");
                }

                return _userName_SADE_Director;
            }

        }

        private string _userName_SADE_receptor;
        private string UserName_SADE_receptor
        {
            get
            {
                if (string.IsNullOrEmpty(_userName_SADE_receptor))
                {
                    _userName_SADE_receptor = SGI.Parametros.GetParam_ValorChar("SGI.Username.Receptor.Habilitaciones");
                }

                return _userName_SADE_receptor;
            }

        }

        private string _sector_SADE_receptor_RevFirmaDispo;
        private string Sector_SADE_receptor_RevFirmaDispo
        {
            get
            {
                if (string.IsNullOrEmpty(_sector_SADE_receptor_RevFirmaDispo))
                {
                    _sector_SADE_receptor_RevFirmaDispo = SGI.Parametros.GetParam_ValorChar("SGI.RevFirmaDispo.SectorDestino");
                }

                return _sector_SADE_receptor_RevFirmaDispo;
            }

        }
        private string _reparticion_SADE_receptor_RevFirmaDispo;
        private string Reparticion_SADE_receptor_RevFirmaDispo
        {
            get
            {
                if (string.IsNullOrEmpty(_reparticion_SADE_receptor_RevFirmaDispo))
                {
                    _reparticion_SADE_receptor_RevFirmaDispo = SGI.Parametros.GetParam_ValorChar("SGI.RevFirmaDispo.SectorDestino.Reparticion");
                }

                return _reparticion_SADE_receptor_RevFirmaDispo;
            }

        }

        private string _sector_SADE_receptor_RevFirmaDispo_Rechazo;
        private string Sector_SADE_receptor_RevFirmaDispo_Rechazo
        {
            get
            {
                if (string.IsNullOrEmpty(_sector_SADE_receptor_RevFirmaDispo_Rechazo))
                {
                    _sector_SADE_receptor_RevFirmaDispo_Rechazo = SGI.Parametros.GetParam_ValorChar("SGI.Username.Pase_GenerarExpediente_EnviarAVH");
                }

                return _sector_SADE_receptor_RevFirmaDispo_Rechazo;
            }

        }
        private string _reparticion_SADE_receptor_RevFirmaDispo_Rechazo;
        private string Reparticion_SADE_receptor_RevFirmaDispo_Rechazo
        {
            get
            {
                if (string.IsNullOrEmpty(_reparticion_SADE_receptor_RevFirmaDispo_Rechazo))
                {
                    _reparticion_SADE_receptor_RevFirmaDispo_Rechazo = SGI.Parametros.GetParam_ValorChar("SGI.Reparticion.Pase_GenerarExpediente_EnviarAVH");
                }

                return _reparticion_SADE_receptor_RevFirmaDispo_Rechazo;
            }

        }

        private string _acronicoPV;
        private string AcronicoPV
        {
            get
            {
                if (string.IsNullOrEmpty(_acronicoPV))
                {
                    _acronicoPV = SGI.Parametros.GetParam_ValorChar("EE.acronimo.pv");
                }

                return _acronicoPV;
            }

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
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion

        #region metodos publicos

        #region  buscar procesos 

        public List<ProcesoExpediente> GetProcesos_porTramiteTarea(int id_tramitetarea)
        {
            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            var q =
                    (
                        from pe in this.db.SGI_Tarea_Generar_Expediente_Procesos
                        join p in this.db.SGI_Procesos_EE on pe.id_proceso equals p.id_proceso
                        where pe.id_tramitetarea == id_tramitetarea
                        orderby pe.id_generar_expediente_proc
                        select new
                        {
                            id_generar_expediente_proc = pe.id_generar_expediente_proc,
                            id_proceso = pe.id_proceso,
                            id_paquete = pe.id_paquete,
                            id_devolucion = pe.id_devolucion_ee,
                            nombre_proceso = p.desc_proceso,
                            realizado = pe.realizado,
                            nro_tramite = pe.nro_tramite,
                            nombre_tramite = pe.descripcion_tramite,
                            resultado_sade = pe.resultado_ee,
                            resultado_sade_ok = "",
                            id_tramitetarea = pe.id_tramitetarea
                        }
                    ).ToList();

            this.proceso = new SGI_Tarea_Generar_Expediente_Procesos();
            this.proceso.id_paquete = (q.Count > 0) ? q[0].id_paquete : -1;

            this.ds_getInfoPaquete = WS_GetInfoPaquete();

            List<ProcesoExpediente> listProcesos = new List<ProcesoExpediente>();
            ProcesoExpediente proceso = null;

            foreach (var item in q)
            {
                proceso = new ProcesoExpediente();

                proceso.id_generar_expediente_proc = item.id_generar_expediente_proc;
                proceso.id_proceso = item.id_proceso;
                proceso.nombre_proceso = item.nombre_proceso;
                proceso.realizado = item.realizado;
                proceso.nro_tramite = item.nro_tramite;
                proceso.nombre_tramite = item.nombre_tramite;
                proceso.resultado_sade_error = item.resultado_sade;
                proceso.id_tramite_tarea = (int)item.id_tramitetarea;
                proceso.id_devolucion_ee = item.id_devolucion;

                proceso = buscarProcesos(proceso); 

                listProcesos.Add(proceso);
            }

            return listProcesos;

        }



        private ProcesoExpediente buscarProcesos(ProcesoExpediente proceso) //, int id_documento)
        {

            int id_documento_ee = 0;
 
                switch (proceso.id_proceso)
                {
                    case 1: // paquete 
                        proceso.realizado_en_sade = true;
                        proceso.resultado_sade_error = "";
                        proceso.resultado_sade_ok = "";
                        proceso.fecha_sade = null;
                        break;

                    case 2: // caratula
                        if (ds_getInfoPaquete != null && ds_getInfoPaquete.Tables["Caratula"] != null && ds_getInfoPaquete.Tables["Caratula"].Rows.Count > 0)
                        {

                            bool subido = this.datos_caratula_subida_sade; // Caratula_subida_sade();
                            string resultado = this.datos_caratula_nro_expediente_completo; // Caratula_resultado_sade(); 
                            proceso.realizado_en_sade= this.datos_caratula_subida_sade;
                            proceso.id_devolucion_ee = this.datos_caratula_id_caratula;
                            if (subido)
                            {
                                
                                proceso.resultado_sade_error = "";
                                proceso.resultado_sade_ok = resultado;
                            }
                            else
                            {
                                proceso.resultado_sade_error = resultado;
                                proceso.resultado_sade_ok = "";
                            }

                            proceso.fecha_sade = Convert.ToDateTime(ds_getInfoPaquete.Tables["Caratula"].Rows[0]["fecha_resultado"]);
                        }
                        break;

                    case 3: // subir documento

                        proceso.fecha_sade = null;

                        foreach (DataRow row in ds_getInfoPaquete.Documentos.Rows)
                        {
                            id_documento_ee = Convert.ToInt32(row["id_documento"]);

                            if (id_documento_ee == proceso.id_devolucion_ee)
                            {
                                bool subido = (row["subido"] == DBNull.Value) ? false : Convert.ToBoolean(row["subido"]);
                                string resultado = Convert.ToString(row["resultado"]);
                                proceso.realizado_en_sade= subido;
                                if (subido)
                                {
                                    proceso.resultado_sade_error = "";
                                    proceso.resultado_sade_ok = resultado;
                                }
                                else
                                {
                                    proceso.resultado_sade_error = resultado;
                                    proceso.resultado_sade_ok = "";
                                }


                                proceso.fecha_sade = Convert.ToDateTime(row["fecha_resultado"]);  // Convert.ToString(row["fecha_resultado"]);
                                break;
                            }

                        }

                        break;

                    case 4: // desbloquear
                        if (ds_getInfoPaquete != null && ds_getInfoPaquete.Tables["Desbloqueos"] != null && ds_getInfoPaquete.Tables["Desbloqueos"].Rows.Count > 0)
                        {
        
                            foreach (DataRow row in ds_getInfoPaquete.Desbloqueos.Rows)
                            {

                                id_documento_ee = Convert.ToInt32(row["id_desbloqueo"]);

                                if (id_documento_ee == proceso.id_devolucion_ee)
                                {

                                    proceso.fecha_sade = Convert.ToDateTime(ds_getInfoPaquete.Tables["Desbloqueos"].Rows[0]["fecha_resultado"]);
                                    if (Convert.ToBoolean(ds_getInfoPaquete.Tables["Desbloqueos"].Rows[0]["desbloqueado"].ToString())){
                                        proceso.resultado_sade_ok = ds_getInfoPaquete.Tables["Desbloqueos"].Rows[0]["resultado"].ToString();
                                        proceso.realizado_en_sade= true;
                                    }else{
                                        proceso.resultado_sade_error = ds_getInfoPaquete.Tables["Desbloqueos"].Rows[0]["resultado"].ToString();
                                        proceso.realizado_en_sade= false;
                                    }
                                }

                            }

                        }
                        break;

                    case 5: // bloquear
                        if (ds_getInfoPaquete != null && ds_getInfoPaquete.Tables["Bloqueos"] != null && ds_getInfoPaquete.Tables["Bloqueos"].Rows.Count > 0)
                        {

                            foreach (DataRow row in ds_getInfoPaquete.Bloqueos.Rows)
                            {
                                id_documento_ee = Convert.ToInt32(row["id_bloqueo"]);

                                if (id_documento_ee == proceso.id_devolucion_ee)
                                {
                                    proceso.fecha_sade = Convert.ToDateTime(ds_getInfoPaquete.Tables["Bloqueos"].Rows[0]["fecha_resultado"]);
                                    proceso.realizado_en_sade= Convert.ToBoolean(ds_getInfoPaquete.Tables["Bloqueos"].Rows[0]["bloqueado"].ToString());
                                    if (proceso.realizado_en_sade)
                                        proceso.resultado_sade_ok = ds_getInfoPaquete.Tables["Bloqueos"].Rows[0]["resultado"].ToString();
                                    else
                                        proceso.resultado_sade_error = ds_getInfoPaquete.Tables["Bloqueos"].Rows[0]["resultado"].ToString();
                                }

                            }

                        }
                        break;

                    case 6: // disposicion
                        if (ds_getInfoPaquete != null && ds_getInfoPaquete.Tables["TareasAlaFirma"] != null && ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                        {
                            var tarea_proceso  = db.SGI_Tarea_Generar_Expediente_Procesos.FirstOrDefault(x => x.id_tramitetarea == proceso.id_tramite_tarea && x.id_proceso == 6);

                            bool firmado = this.datos_dispo_esta_firmada; // Disposicion_esta_firmada();
                            string resultado = this.datos_dispo_resultado; // Disposicion_resultado();
                            string firmado_numeroGEDO = this.datos_dispo_numeroGEDO; // Disposicion_numeroGEDO();
                            bool subido = this.datos_dispo_documento_subido; // Disposicion_documento_subido();
                            string firmado_resultado = this.datos_dispo_numeroGEDO_resultado; // Disposicion_numeroGEDO_resultado();

                            if(tarea_proceso != null)
                                proceso.fecha_sade = tarea_proceso.CreateDate;

                            //proceso.realizado = subido;
                            proceso.realizado_en_sade = subido;

                            if (subido)
                            {
                                proceso.resultado_sade_error = "";
                                proceso.resultado_sade_ok = firmado_numeroGEDO;
                                if ( ! string.IsNullOrEmpty(proceso.resultado_sade_ok) )
                                    proceso.resultado_alerta = "";
                                else
                                    proceso.resultado_alerta = firmado_resultado;
                            }
                            else
                            {
                                proceso.resultado_sade_error = resultado;
                                proceso.resultado_sade_ok = "";
                                proceso.resultado_alerta = "";
                            }
                        }
                        break;

                    case 7: // relacion disposicion con caratula

                        if (ds_getInfoPaquete != null && ds_getInfoPaquete.Tables["TareasAlaFirma"] != null && ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                        {
                            string resultado_relacion = this.datos_dispo_resultado; // Disposicion_resultado();
                            bool relacion_subido = this.datos_dispo_relacion_subido; // Disposicion_relacion_subido();
                            proceso.fecha_sade = this.datos_dispo_relacion_fecha_resultado; // Disposicion_relacion_fechaResultado();
                            proceso.realizado_en_sade = relacion_subido;
                            if (relacion_subido)
                            {
                                proceso.resultado_sade_error = "";
                                proceso.resultado_sade_ok = resultado_relacion;
                            }
                            else
                            {
                                proceso.resultado_sade_error = resultado_relacion;
                                proceso.resultado_sade_ok = "";
                            }

                        }
                        break;


                    case 8: // controlar firma disposicion
                        if (ds_getInfoPaquete != null && ds_getInfoPaquete.Tables["TareasAlaFirma"] != null && ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
                        {


                            bool subido = this.datos_dispo_documento_subido; // Disposicion_documento_subido();
                            string firmado_numeroGEDO = this.datos_dispo_numeroGEDO; // Disposicion_numeroGEDO();
                            string firmado_resultado = this.datos_dispo_numeroGEDO_resultado; // Disposicion_numeroGEDO_resultado();

                            bool firmado = this.datos_dispo_esta_firmada; // Disposicion_esta_firmada();
                            string resultado = this.datos_dispo_resultado; // Disposicion_resultado();
                            proceso.fecha_sade = this.datos_dispo_fecha_resultado; // Disposicion_fechaResultado();
                            proceso.realizado_en_sade = firmado;
                            if (subido)
                            {
                                if (firmado)
                                {
                                    proceso.resultado_sade_error = "";
                                    proceso.resultado_sade_ok = firmado_numeroGEDO;
                                }
                                else
                                {
                                    proceso.resultado_sade_error = resultado;
                                    proceso.resultado_sade_ok = "";
                                }

                                if (!string.IsNullOrEmpty(proceso.resultado_sade_ok))
                                    proceso.resultado_alerta = "";
                                else
                                    proceso.resultado_alerta = firmado_resultado;
                            }
                            else
                            {
                                proceso.resultado_sade_error = resultado;
                                proceso.resultado_sade_ok = "";
                                proceso.resultado_alerta = "";
                            }

                        }

                        break;
                    case 9: // planos

                        proceso.fecha_sade = null;
                        foreach (DataRow row in ds_getInfoPaquete.Documentos.Rows)
                        {
                            id_documento_ee = Convert.ToInt32(row["id_documento"]);

                            if (id_documento_ee == proceso.id_devolucion_ee)
                            {
                                bool subido = (row["subido"] == DBNull.Value) ? false : Convert.ToBoolean(row["subido"]);
                                string resultado = Convert.ToString(row["resultado"]);
                                proceso.realizado_en_sade = subido;
                                if (subido)
                                {
                                    proceso.resultado_sade_error = "";
                                    proceso.resultado_sade_ok = resultado;
                                }
                                else
                                {
                                    proceso.resultado_sade_error = resultado;
                                    proceso.resultado_sade_ok = "";
                                }
                                proceso.fecha_sade = Convert.ToDateTime(row["fecha_resultado"]);  // Convert.ToString(row["fecha_resultado"]);
                                break;
                            }

                        }

                        break;
                    case 11: //Obtener carátula de SADE
                        proceso.realizado_en_sade = true;
                        proceso.resultado_sade_error = "";
                        proceso.resultado_sade_ok = "";
                        proceso.fecha_sade = null;
                        break;
                    case 12: //Obtener disposicion
                        proceso.realizado_en_sade = true;
                        proceso.resultado_sade_error = "";
                        proceso.resultado_sade_ok = "";
                        proceso.fecha_sade = null;
                        break;
                    case 13: // subir providencia

                        proceso.fecha_sade = null;

                        foreach (DataRow row in ds_getInfoPaquete.Documentos.Rows)
                        {
                            id_documento_ee = Convert.ToInt32(row["id_documento"]);

                            if (id_documento_ee == proceso.id_devolucion_ee)
                            {
                                bool subido = (row["subido"] == DBNull.Value) ? false : Convert.ToBoolean(row["subido"]);
                                string resultado = Convert.ToString(row["resultado"]);
                                proceso.realizado_en_sade = subido;
                                if (subido)
                                {
                                    proceso.resultado_sade_error = "";
                                    proceso.resultado_sade_ok = resultado;
                                }
                                else
                                {
                                    proceso.resultado_sade_error = resultado;
                                    proceso.resultado_sade_ok = "";
                                }


                                proceso.fecha_sade = Convert.ToDateTime(row["fecha_resultado"]);  // Convert.ToString(row["fecha_resultado"]);
                                break;
                            }

                        }

                        break;
                    default:
                        break;

                }


            return proceso;

        }

        #endregion 

        public async Task<int> Procesar(int id_generar_expediente_proc, int id_tramite_tarea)
        {

            this.proceso =this.db.SGI_Tarea_Generar_Expediente_Procesos.Where(
                                x => x.id_generar_expediente_proc == id_generar_expediente_proc).FirstOrDefault();

            if (this.proceso == null ||  this.proceso.realizado )
                return 0;

            if (this.proceso.id_tramitetarea == id_tramite_tarea && !this.proceso.realizado)
            {

                switch (this.proceso.id_proceso)
                {
                    case (int)Constants.EE_Procesos.GeneracionPaquete:
                        ProcesarPaquete();
                        break;

                    case (int)Constants.EE_Procesos.GeneracionCaratula:
                        ProcesarCaratula();
                        break;
                    case (int)Constants.EE_Procesos.ObtenerCaratula:
                        ObtenerCaratula();
                        break;
                    case (int)Constants.EE_Procesos.ObtenerDisposicion:
                        ObtenerDisposicion();
                        break;
                    case (int)Constants.EE_Procesos.SubirDocumento:
                        ProcesarDocumento();
                        break;

                    case (int)Constants.EE_Procesos.PdfPlanosAdjuntos:
                        ProcesarPlanos();
                        break;

                    case (int)Constants.EE_Procesos.DesbloqueoExpediente:
                        ProcesarDesbloqueo();
                        break;

                    case (int)Constants.EE_Procesos.BloqueoExpediente:
                        ProcesarBloqueo();
                        break;

                    case (int)Constants.EE_Procesos.FirmarDocumento:
                        ProcesarTareaFirmar();
                        break;
                    case (int)Constants.EE_Procesos.FirmarDocumento_RevisarFirma:
                        ProcesarTareaFirmar_verificarFirmado();
                        break;

                    case (int)Constants.EE_Procesos.RelacionarDocumento:
                        ProcesarRelacionar();
                        break;

                    case (int)Constants.EE_Procesos.PasarExpediente:
                        ProcesarPaseExpediente();
                        break;

                    case (int)Constants.EE_Procesos.SubirProvidencia:
                        ProcesarProvidencia();
                        break;

                    default:
                        throw new ExpedienteException("Proceso no definido, id_proceso " + this.proceso.id_proceso);
                }

            }

            return 0;
        }

        public DataSet GetInfoPaquete(int id_paquete)
        {

            if (this.proceso == null)
                this.proceso = new SGI_Tarea_Generar_Expediente_Procesos();

            this.proceso.id_paquete = id_paquete;

            this.ds_getInfoPaquete = WS_GetInfoPaquete();

            return this.ds_getInfoPaquete;
        }

        public DataSet GetDatosExpediente(int id_solicitud)
        {

            if (this.proceso == null)
                this.proceso = new SGI_Tarea_Generar_Expediente_Procesos();

            //buscar el ultimo tramite tarea con expediente generado
            var ultimoTT = (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join expe in db.SGI_SADE_Procesos on tt.id_tramitetarea equals expe.id_tramitetarea 
                    where tt_hab.id_solicitud == id_solicitud
                    orderby tt.id_tramitetarea descending
                    select new
                    {
                        tt.id_tramitetarea,
                        expe.id_paquete
                    }
                ).FirstOrDefault();

            int id_tramitetarea = 0;
            int id_paquete = 0;
            if (ultimoTT != null)
            {
                id_tramitetarea = ultimoTT.id_tramitetarea;
                id_paquete = ultimoTT.id_paquete;

                this.proceso.id_paquete = id_paquete;
                this.ds_getInfoPaquete = WS_GetInfoPaquete();

            }

            return this.ds_getInfoPaquete;  // lo vuelvo por si se quieren analizar
        }

        #endregion 

        private void ProcesarPaquete()
        {
            if (this.proceso.id_paquete != 0)
                return;

            WS_GenerarPaquete();

            this.proceso.id_paquete = this.proceso.id_devolucion_ee;

            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    if (this.proceso.realizado)
                    {
                        //vincular todos los registros al mismo paquete
                        this.db.SGI_Tarea_Generar_Expediente_Procesos_Actualizar_Paquete(
                                    this.proceso.id_tramitetarea, this.proceso.id_paquete, this.userid);
                    }

                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                this.proceso.id_devolucion_ee,
                                this.proceso.resultado_ee, this.proceso.realizado,
                                this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarPaquete-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }

        }

        #region procesos caratula

        private string getRubrosParaPlancheta(int id_encomienda)
        {
            string rubros = "";

            var query_Rubros =
                (
                    from enc in db.Encomienda
                    join rub in db.Encomienda_Rubros on enc.id_encomienda equals rub.id_encomienda
                    join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
                    join docreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals docreq.Id
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        rub.id_encomiendarubro,
                        enc.id_encomienda,
                        rub.cod_rubro,
                        desc_rubro = rub.desc_rubro,
                        rub.EsAnterior,
                        TipoActividad = tact.Nombre,
                        DocRequerida = docreq.Nomenclatura,
                        rub.SuperficieHabilitar
                    }
                ).ToList();

            
            foreach (var item in query_Rubros)
            {
                if (string.IsNullOrEmpty(rubros))
                    rubros = item.cod_rubro;
                else
                {
                    if (rubros.Length > 70)
                        rubros = rubros + ",\n" + item.cod_rubro;
                    else
                        rubros = rubros + ", " + item.cod_rubro;
                }

            }

            if ( rubros.Length > 0 )
            {
                if (query_Rubros.Count > 1)
                    rubros = "Rubros: " + rubros;
                else
                    rubros = "Rubro: " + rubros;

            }



            return rubros;
        }

        private string getDomicilioParaPlancheta(int id_solicitud)
        {
            string domicilio = "";
            string sql = "select dbo.SGI_DireccionesPartidasPlancheta(" + id_solicitud + ")";
            string dom = db.Database.SqlQuery<string>(sql).FirstOrDefault();

            int inicio_corte = 0;
            int cant = 70;

            while (inicio_corte < dom.Length)
            {
                if (string.IsNullOrEmpty(domicilio))
                {
                    if (dom.Length - inicio_corte < cant)
                        domicilio = dom.Substring(inicio_corte);  // copia hasta el final
                    else
                        domicilio = dom.Substring(inicio_corte, cant);
                }
                else
                {
                    if (dom.Length - inicio_corte < cant)
                        domicilio = domicilio + "\n" + dom.Substring(inicio_corte); // copia hasta el final
                    else
                        domicilio = domicilio + "\n" + dom.Substring(inicio_corte, cant);

                }

                inicio_corte = inicio_corte + cant;

            }

            return domicilio;
        }

        private void ProcesarCaratula()
        {
            if (this.proceso.id_paquete == 0)
                return;

            if (this.proceso.id_caratula != 0)
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            //Buscar datos titular encomienda
            int id_encomienda = 0;
            int id_tipotramite = 0;
            int id_tipoexpediente = 0;
            int id_subtipoexpediente = 0;
            int id_solicitud = 0;

            Encomienda_Titulares_PersonasFisicas titularFisica = null;
            Encomienda_Titulares_PersonasJuridicas titularJuridica = null;

            var datos_sol =
                (
                    from tt in this.db.SGI_Tramites_Tareas_HAB
                    join sol in this.db.SSIT_Solicitudes on tt.id_solicitud equals sol.id_solicitud
                    where tt.id_tramitetarea == this.proceso.id_tramitetarea
                    select new
                    {
                        sol.id_tipotramite,
                        sol.id_tipoexpediente,
                        sol.id_subtipoexpediente,
                        sol.id_solicitud
                    }
                ).FirstOrDefault();

            id_tipotramite = datos_sol.id_tipotramite;
            id_tipoexpediente = datos_sol.id_tipoexpediente;
            id_subtipoexpediente = datos_sol.id_subtipoexpediente;
            id_solicitud = datos_sol.id_solicitud;

            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
            id_encomienda = enc.id_encomienda;


            titularFisica = this.db.Encomienda_Titulares_PersonasFisicas.Where(x => x.id_encomienda == id_encomienda).OrderBy(x => x.id_personafisica).FirstOrDefault();
            if (titularFisica == null)
            {
               titularJuridica = this.db.Encomienda_Titulares_PersonasJuridicas.Where(x => x.id_encomienda == id_encomienda).OrderBy(x => x.id_personajuridica).FirstOrDefault();
            }

            if (titularFisica == null && titularJuridica == null)
                throw new ExpedienteException("No se encontro titular del expediente. Encomienda: " + id_encomienda);

            string telefono = "";
            string piso = "", codigo_postal = "", departamento = "", motivo_externo = "";
            string domicilio = getDomicilioParaPlancheta(id_solicitud);
            string descrip_expediente = domicilio;

            if (descrip_expediente.Length > 255)
                descrip_expediente = descrip_expediente.Substring(0, 255);

            domicilio = domicilio.Length < 240 ? domicilio : domicilio.Substring(0, 240);

            // Functions.GetTipoDeTramiteDesc(id_tipotramite) + "  " + Functions.GetTipoExpedienteDesc(id_tipoexpediente, id_subtipoexpediente);
            string motivo_expediente = motivo_expediente = getRubrosParaPlancheta(id_encomienda);
            if (motivo_expediente.Length > 255)
                motivo_expediente = motivo_expediente.Substring(0, 255);

            string codigo_trata = "";

            if (id_tipotramite == (int)Constants.TipoDeTramite.Habilitacion || 
                id_tipotramite == (int)Constants.TipoDeTramite.RectificatoriaHabilitacion )
                switch (id_subtipoexpediente)
                {
                    case 1: // SIN_PLANOS
                        codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Habilitacion.SimpleSinplanos");
                        break;
                    case 2: // CON_PLANOS
                        decimal sup = 0;
                        var datos_enc =
                            (
                                from ed in this.db.Encomienda_DatosLocal
                                where ed.id_encomienda == id_encomienda
                                select new
                                {
                                    ed.superficie_cubierta_dl,
                                    ed.superficie_descubierta_dl
                                }
                            ).FirstOrDefault();
                        sup = datos_enc.superficie_cubierta_dl.Value + datos_enc.superficie_descubierta_dl.Value;

                        if (sup < 100)
                            codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.A");
                        else if (sup < 500)
                            codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.B");
                        else
                            codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Habilitacion.SimpleConPlanos.C");
                        break;
                    case 3: //INSPECCION_PREVIA
                        codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Habilitacion.InspeccionPrevia");
                        break;
                    case 4: //HABILITACION_PREVIA
                        codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Habilitacion.InspeccionPrevia");
                        break;
                    default:
                        break;
                }
            else if (id_tipotramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Consulta.Padron");
            else if(id_tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                codigo_trata = ParametrosEE.GetParam_ValorChar("EE.Trata.Transferencia");

            if (string.IsNullOrEmpty(codigo_trata))
                throw new Exception("No se pudo recuperar el código de trata.");

            //Recupero el usuario_Sade
            dynamic parametros = null;
            if (this.proceso.parametros_SADE != null)
                parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.proceso.parametros_SADE);

            string username_SADE = "";
            try{username_SADE = parametros.Usuario_SADE.Value;}
            catch (Exception){ username_SADE = this.UserName_SADE;}

            //generar caratula
            if (titularFisica != null)
            {
                ws_ExpedienteElectronico.EETipodocumento tipo_doc = ConvertToTipoDocumento_EE(titularFisica.id_tipodoc_personal);

                telefono = titularFisica.Telefono;
                decimal nro_cuit = GetCuitPersonaJuridica(titularFisica.Cuit);

                Decimal nroDoc;
                string Nro_Documento = titularFisica.Nro_Documento;
                bool convert = decimal.TryParse(titularFisica.Nro_Documento, out nroDoc);
                if (tipo_doc == ws_ExpedienteElectronico.EETipodocumento.PA && !convert)
                {
                    tipo_doc = ws_ExpedienteElectronico.EETipodocumento.DU;
                    Nro_Documento = titularFisica.Cuit.Substring(titularFisica.Cuit.IndexOf("-")+1);
                    Nro_Documento = Nro_Documento.Substring(0, Nro_Documento.LastIndexOf("-"));
                }


                WS_GenerarCaratulaPersonaFisica(titularFisica.Apellido, titularFisica.Nombres, nro_cuit , tipo_doc,
                    Nro_Documento, titularFisica.Email, telefono, 
                    descrip_expediente, motivo_expediente, username_SADE, codigo_trata, domicilio, piso, departamento,
                            codigo_postal, motivo_externo);
            }
            else
            {
                telefono = titularJuridica.Telefono.Length == 0 ? "" : titularJuridica.Telefono.Trim().Replace("-", "").Replace("(", "").Replace(")", "")
                        .Replace(" ", "").Replace("/","").Replace(".","");
                int tel=0;
                if(!int.TryParse(telefono,out tel))
                    telefono = "";

                decimal nro_cuit = GetCuitPersonaJuridica(titularJuridica.CUIT);

                WS_GenerarCaratulaPersonaJuridica(titularJuridica.Razon_Social, nro_cuit, titularJuridica.Email, telefono,
                            descrip_expediente, motivo_expediente, username_SADE, codigo_trata, domicilio, piso, departamento,
                            codigo_postal, motivo_externo);
            }

            this.proceso.id_caratula = this.proceso.id_devolucion_ee;
             
            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    if (this.proceso.realizado)
                    {
                        //vincular todos los registros a la misma caratula
                        this.db.SGI_Tarea_Generar_Expediente_Procesos_Actualizar_Caratula(
                                        this.proceso.id_tramitetarea, this.proceso.id_caratula, this.userid);
                    }

                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                                this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarCaratula-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }

        }

        private void ObtenerCaratula()
        {
            if (this.proceso.id_paquete == 0)
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            //Buscar datos encomienda
            int id_solicitud = 0;
            int id_tramitetarea = 0;

            var datos_sol =
                (
                    from tt in this.db.SGI_Tramites_Tareas_HAB
                    join sol in this.db.SSIT_Solicitudes on tt.id_solicitud equals sol.id_solicitud
                    where tt.id_tramitetarea == this.proceso.id_tramitetarea
                    select new
                    {
                        sol.id_solicitud,
                        tt.id_tramitetarea,
                    }
                ).FirstOrDefault();

            id_solicitud = datos_sol.id_solicitud;
            id_tramitetarea = datos_sol.id_tramitetarea;

            try
            {
                // que se grabe la caratula
                bool pdf_guardado = GuardarCaratula(id_tramitetarea, id_solicitud);

                if (!pdf_guardado)
                    throw new Exception("No se ha guardado la caratula en SGI.");

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "No se pudo recuperar la carátula.");
                throw new Exception("No se pudo recuperar la carátula.");
            }
        }

        private bool GuardarCaratula(int id_tramitetarea, int id_solicitud)
        {
            //IniciarEntity();
            //IniciarEntityFiles();
            bool pdfGenerado = false;

            byte[] documento = null;
            int id_proceso = (int)Constants.EE_Procesos.ObtenerCaratula;
            int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.CARATULA_HABILITACION;
            int id_file = 0;
            string appName = this.NombreSistema;

            var proceso =
                (
                    from tt in db.SGI_Tramites_Tareas_HAB
                    join p in db.SGI_Tarea_Generar_Expediente_Procesos on tt.id_tramitetarea equals p.id_tramitetarea
                    join ssit in db.SSIT_Solicitudes on tt.id_solicitud equals ssit.id_solicitud
                    where tt.id_solicitud == id_solicitud && p.id_proceso == id_proceso
                    select new
                    {
                        p.id_paquete,
                        p.id_caratula,
                        p.realizado
                    }

                ).FirstOrDefault();

            var doc_adj =
                (
                    from adj in db.SSIT_DocumentosAdjuntos
                    where adj.id_solicitud == id_solicitud && adj.id_tipodocsis == id_tipodocsis
                    select new
                    {
                        adj.id_file
                    }

                ).FirstOrDefault();

            if (doc_adj != null)
            {
                id_file = doc_adj.id_file;
                pdfGenerado = true;
            }

            if (!proceso.realizado && id_file == 0) //solo se vuelve a buscar si no lo grabo en sistema sgi
            {
                WS_GetPdfCaratula(proceso.id_paquete, ref documento);

                if (documento != null)
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {
                        id_file = ws_FilesRest.subirArchivo("Caratula.pdf", documento);
                        ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                        db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, false, id_file, "Caratula.pdf", userid, param_id_docadjunto);

                        try
                        {
                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. GuardarCaratula-id_tramitetarea:" + id_tramitetarea + "id_solicitud:" + id_solicitud);
                            throw ex;
                        }
                    }
                }
                pdfGenerado = true;
            }
            if (!proceso.realizado && pdfGenerado)
            {
                this.proceso.realizado = true;
                this.proceso.resultado_ee = "";
                this.proceso.descripcion_tramite = "";
                this.proceso.id_devolucion_ee = id_file;
                using (TransactionScope Tran = new TransactionScope())
                {
                    //Guarda el numero de expediente
                    this.db.SGI_Nro_Expediente_Sade_Actualizar(id_solicitud, this.datos_caratula_nro_expediente_completo);

                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                                this.userid, null, this.proceso.descripcion_tramite);

                    try
                    {
                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. GuardarCaratula-id_tramitetarea:" + id_tramitetarea + "id_solicitud:" + id_solicitud);
                        throw ex;
                    }
                }
            }
            return pdfGenerado;
        }

        private void ObtenerDisposicion()
        {
            if (this.proceso.id_paquete == 0)
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            //Buscar datos encomienda
            int id_solicitud = 0;
            int id_tramitetarea = 0;

            var datos_sol =
                (
                    from tt in this.db.SGI_Tramites_Tareas_HAB
                    join sol in this.db.SSIT_Solicitudes on tt.id_solicitud equals sol.id_solicitud
                    where tt.id_tramitetarea == this.proceso.id_tramitetarea
                    select new
                    {
                        sol.id_solicitud,
                        tt.id_tramitetarea,
                    }
                ).FirstOrDefault();

            id_solicitud = datos_sol.id_solicitud;
            id_tramitetarea = datos_sol.id_tramitetarea;

            // buscar si exienten procesos pendientes anteriores a este proceso
            // pendientes reales, es decir, sin el ok de sade aunque tenga el ok del ws expediente electronico
            bool hay_procesos_pendientes = true;
            Expediente procesoExpediente = new Expediente(userid, Constants.ApplicationName);
            List<ProcesoExpediente> lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramitetarea);
            if (lstProcesos == null || lstProcesos.Count == 0)
                hay_procesos_pendientes = true;
            else
                hay_procesos_pendientes = lstProcesos.Exists(x => x.id_generar_expediente_proc < this.proceso.id_generar_expediente_proc && (x.realizado_en_sade == false));

            if (hay_procesos_pendientes)
                throw new Exception("Para Obtener la Disposición, todas los procesos deben haber finalizado.");
            try
            {
                // que se grabe la disposicion
                bool pdf_guardado = GuardarDisposicion(id_tramitetarea, id_solicitud);

                if (!pdf_guardado)
                    throw new Exception("No se ha podido recuperar la disposición.");

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "No se pudo recuperar la disposición.");
                throw new Exception("No se pudo recuperar la disposición.");
            }
        }

        private bool GuardarDisposicion(int id_tramitetarea, int id_solicitud)
        {
            bool pdfGenerado = false;

            byte[] documento = null;
            int id_proceso = (int)Constants.EE_Procesos.ObtenerDisposicion;
            int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.DISPOSICION_HABILITACION;
            int id_cert = 0;
            string appName = this.NombreSistema;

            var proceso =
                (
                    from tt in db.SGI_Tramites_Tareas_HAB
                    join p in db.SGI_Tarea_Generar_Expediente_Procesos on tt.id_tramitetarea equals p.id_tramitetarea
                    join ssit in db.SSIT_Solicitudes on tt.id_solicitud equals ssit.id_solicitud
                    where tt.id_solicitud == id_solicitud && p.id_proceso == id_proceso
                    select new
                    {
                        p.id_paquete,
                        p.id_caratula,
                        p.realizado
                    }

                ).FirstOrDefault();

            var cert =
                (
                    from adj in db.SSIT_DocumentosAdjuntos
                    where adj.id_solicitud == id_solicitud && adj.id_tipodocsis == id_tipodocsis
                    select new
                    {
                        adj.id_file
                    }

                ).FirstOrDefault();

            if (cert != null)
            {
                id_cert = cert.id_file;
                pdfGenerado = true;
            }

            if (!proceso.realizado && id_cert == 0) //solo se vuelve a buscar si no lo grabo en sistema sgi
            {
                WS_GetPdfDisposicion(proceso.id_paquete, ref documento);
                if (documento != null)
                {
                    using (TransactionScope Tran = new TransactionScope())
                    {
                        id_cert = ws_FilesRest.subirArchivo("Disposicion.pdf", documento);

                        ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                        db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, true, id_cert, "Disposicion.pdf", this.userid, param_id_docadjunto);

                        try
                        {
                            Tran.Complete();
                            Tran.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            LogError.Write(ex, "Error en transaccion. GuardarDisposicion-id_tramitetarea:" + id_tramitetarea + "id_solicitud:" + id_solicitud);
                            throw ex;
                        }
                    }
                }
                pdfGenerado = true;
            }
            
            if (!proceso.realizado && pdfGenerado)
            {
                this.proceso.realizado = true;
                this.proceso.resultado_ee = "";
                this.proceso.descripcion_tramite = "";
                this.proceso.id_devolucion_ee = id_cert;
                using (TransactionScope Tran = new TransactionScope())
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                                this.userid, null, this.proceso.descripcion_tramite);

                    try
                    {
                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. GuardarDisposicion-id_tramitetarea:" + id_tramitetarea + "id_solicitud:" + id_solicitud);
                        throw ex;
                    }
                }
            }
            return pdfGenerado;
        }

        private ws_ExpedienteElectronico.EETipodocumento ConvertToTipoDocumento_EE(int id_tipo_doc_encomienda)
        {
            ws_ExpedienteElectronico.EETipodocumento tipo_doc_ee = ws_ExpedienteElectronico.EETipodocumento.OT;

            switch (id_tipo_doc_encomienda)
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

        private decimal GetCuitPersonaJuridica(string p_cuit)
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

        #region procesos documentos

        private byte[] documento { get; set; }
        private string motivoDocumento { get; set; }

        private async void ProcesarDocumento()
        {
            if (this.proceso.id_paquete == 0 ) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            string username_SADE = "";
            string acronimo_SADE = "";
            string formato_archivo = "";

            //Recupero el usuario_Sade
            dynamic parametros = null;
            if (this.proceso.parametros_SADE != null)
                parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.proceso.parametros_SADE);

            
            try { username_SADE = parametros.Usuario_SADE.Value; }
            catch (Exception) { username_SADE = this.UserName_SADE; }


            try { acronimo_SADE = parametros.Acronimo_SADE.Value; }
            catch (Exception) { acronimo_SADE = Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma"); }

            try { formato_archivo = parametros.formato_archivo.Value; }
            catch (Exception) { formato_archivo = "pdf"; }


            string mensajeError_GetDocumento = "";
            GetDocumentoReturn getdocu = await GetDocumento();

            if (getdocu.documento == null || getdocu.documento.Length < 500 || ! string.IsNullOrEmpty(mensajeError_GetDocumento) )
            {
                if ( ! string.IsNullOrEmpty(mensajeError_GetDocumento) ) 
                    this.proceso.resultado_ee = mensajeError_GetDocumento;
                else
                    this.proceso.resultado_ee = (getdocu.documento == null ? "":System.Text.ASCIIEncoding.ASCII.GetString(getdocu.documento) );
                this.proceso.realizado = false;
                getdocu.documento = null;
            }


            if (getdocu.documento != null)
            {

                if (formato_archivo != "pdf")
                {
                    getdocu.documento = await WS_SubirDocumentoEmbebido(getdocu.documento, string.Format("file-", this.proceso.nro_tramite), acronimo_SADE, string.Format("archivo - ", this.proceso.nro_tramite));
                }
                else
                {
                    getdocu.documento = await WS_SubirDocumento(getdocu.documento, username_SADE, acronimo_SADE, formato_archivo);
                }
                try
                {
                    // copiar a dispo para verificar el archivo
                    if (Functions.CopiarDocumentoDisco())
                    {
                        string arch = Constants.PathTemporal + "documento-proceso-" + this.proceso.id_generar_expediente_proc + "-id_tramitetarea-" + this.proceso.id_tramitetarea + ".pdf"; ;

                        File.WriteAllBytes(arch, getdocu.documento);
                    }

                }
                catch { }

            }


            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                                this.userid, this.proceso.nro_tramite, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarDocumento-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }


        }

        private bool isPdfFirmado(byte[] archivo)
        {
            bool ret = false;
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(archivo);
            iTextSharp.text.pdf.AcroFields af = reader.AcroFields;
            ret = af.GetSignatureNames().Count > 0;
            reader.Dispose();

            return ret;

        }

        private async Task<GetDocumentoReturn> GetDocumento()
        {
            GetDocumentoReturn ret = new GetDocumentoReturn();
            ret.guardar_documento = true;
            ret.mensajeError = "";
            int id_solicitud = this.db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x=>x.id_tramitetarea == this.proceso.id_tramitetarea).id_solicitud;
            int id_file = (int)this.proceso.nro_tramite;
            string descripcion = this.proceso.descripcion_tramite;
            
            int id_tramitetarea = (int)this.proceso.id_tramitetarea;
            int id_certificado = 0;
            ret.forzar_procesado = false; // flag para que se comporte de otra forma desde fuera de esta funcion

            ret.documento = null;

            //IniciarEntityFiles();

            descripcion = descripcion.ToLower();

            
            // this.proceso.SGI_Tarea_Generar_Expediente.SGI_Tramites_Tareas.id_solicitud
            if (descripcion.StartsWith("solicitud de habilitación".ToLower()))
            {
                string createUser = this.db.SSIT_Solicitudes.Where(x => x.id_solicitud == id_file).Select(x => x.CreateUser).FirstOrDefault().ToString(); ;
                string url = Functions.GetUrlSSIT();
                url = url + "Reportes/ImprimirSolicitud.aspx?id=" + id_file + "&user=" + createUser;
                ret.documento = Functions.GetBytesFromUrl(url);
            }
            else if (this.proceso.id_proceso == (int)Constants.SGI_Procesos_EE.SUBIR_DOCUMENTO)
            {
                ret.documento = ws_FilesRest.DownloadFile(id_file);
            }

            #region buscar apra

            if (descripcion.StartsWith("certificado de aptitud ambiental".ToLower()))
            {
                ret.documento = ws_FilesRest.DownloadFile(id_file);
            }

            #endregion


            #region buscar disposicion

            if (descripcion.StartsWith("disposición a la firma".ToLower()))
            {
                ret.documento = this.dbFiles.Certificados.Where(x => x.id_certificado == id_file).Select(x => x.Certificado).FirstOrDefault();
            }

            #endregion

            #region buscar plancheta habilitacion

            if (descripcion.StartsWith(  "certificado de habilitación".ToLower()))
            {
                var q = 
                    (
                        from cert in this.db.SSIT_DocumentosAdjuntos
                        where cert.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.PLANCHETA_HABILITACION
                        && cert.id_solicitud == id_solicitud
                        select new
                        {
                            cert.id_solicitud,
                            cert.id_file
                        }
                    ).FirstOrDefault();

                if (q == null)
                {
                    try
                    {
                        GenerarPdf_Plancheta_HabilitacionReturn pdfplanhab = await GenerarPdf_Plancheta_Habilitacion(this.userid, id_tramitetarea, this.proceso.id_paquete);
                        id_certificado = pdfplanhab.retorno;
                        this.proceso.nro_tramite = id_certificado;
                        this.proceso.descripcion_tramite = "Certificado de habilitación Nro. " + id_certificado;
                    }
                    catch (ExpedienteException ex)  // no existe caratula
                    {
                        documento = null;
                        ret.mensajeError = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        documento = null;
                        throw ex;
                    }

                }
                else
                {
                    ret.documento = ws_FilesRest.DownloadFile(q.id_file);
                    id_certificado = q.id_solicitud;
                    this.proceso.nro_tramite = id_certificado;
                    this.proceso.descripcion_tramite = "Certificado de habilitación Nro. " + id_certificado;
                }

            }

            #endregion

            #region buscar planos

            if (this.proceso.id_proceso == (int) Constants.SGI_Procesos_EE.SUBIR_PLANO ) 
            {
                
                try
                {
                    ret.documento = ws_FilesRest.DownloadFile(id_file);
                    
                    if (documento.Length == 0)
                        throw new Exception("No se pudo obtener el archivo del plano.");

                }
                catch (Exception ex)
                {
                  
                    LogError.Write(ex);
                    this.proceso.resultado_ee = ex.Message;
                    this.proceso.realizado = false;
                    ret.documento = null;
                }
            }
            #endregion
            return ret;
        }
        #endregion 

        #region bloquear y desbloquear
        
        private void ProcesarBloqueo()
        {
            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            WS_BloquearExpediente();

            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                    this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                    this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                                    this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarBloqueo-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }

        }

        private void ProcesarDesbloqueo()
        {

            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            WS_DesbloquearExpediente();

            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                            this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                            this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                            this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarDesbloqueo-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }


        }

        #endregion

        #region disposicion (tarea firmar)

        private void ProcesarTareaFirmar()
        {
            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            byte[] documento = null;

            int id_cert_disposicion = 0;

            string html_dispo = "";
            try
            {
                html_dispo = GenerarDisposicionHtml(this.proceso.id_paquete, userid, ref documento, this.proceso.id_generar_expediente_proc);
                if (documento == null || documento.Length < 500)
                {
                    this.proceso.resultado_ee = (documento == null )? "":System.Text.ASCIIEncoding.ASCII.GetString(documento);
                    this.proceso.realizado = false;
                    documento = null;
                }

            }
            catch (Exception ex)
            {
                this.proceso.resultado_ee = ex.Message;
                this.proceso.realizado = false;
                documento = null;
            }

            ws_ExpedienteElectronico.WS_Item[] ws_item = null;

            if (documento != null)
            {
                string expediente = this.datos_caratula_nro_expediente_completo; 

                if (!string.IsNullOrEmpty(expediente))
                {
                    SGI_Tramites_Tareas_HAB tramite_tarea = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == this.proceso.id_tramitetarea);
                    int id_solicitud = tramite_tarea.id_solicitud;
                    
                    this.proceso.descripcion_tramite =  tramite_tarea.id_solicitud.ToString() + " (" + expediente + ")";
                }
                
                string usuario_firmate = UserName_SADE_Director;
                string usuario_recepcion = UserName_SADE_receptor;

                //Recupero el usuario_Sade
                dynamic parametros = null;
                if (this.proceso.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.proceso.parametros_SADE);

                string username_SADE = "";
                try { username_SADE = parametros.Usuario_SADE.Value; }
                catch (Exception) { username_SADE = this.UserName_SADE; }

                WS_TareaFirmarDisposicion(ref documento, ws_item, username_SADE, new string[] { usuario_firmate }, usuario_recepcion);

                try
                {
                    // copiar a dispo para verificar el archivo
                    if (Functions.CopiarDocumentoDisco())
                     {
                         string arch = Constants.PathTemporal + "dispo-proceso-" + this.proceso.id_generar_expediente_proc + "-id_tramitetarea-" + this.proceso.id_tramitetarea + ".html";

                        File.WriteAllBytes(arch, documento);
                    }

                }
                catch {}

            }

            using (TransactionScope Tran = new TransactionScope())
            {
                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                    this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                    this.proceso.id_devolucion_ee, this.proceso.resultado_ee,
                                    this.proceso.realizado,
                                    this.userid, id_cert_disposicion, this.proceso.descripcion_tramite);
                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion.ProcesarTareaFirmar-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }
            }
        }

        private void ProcesarTareaFirmar_verificarFirmado()
        {
            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee == -1)  // ya fue procesado
                return;

            this.ds_getInfoPaquete = WS_GetInfoPaquete();

            if (this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
            {

                if (this.datos_dispo_esta_firmada)  //Disposicion_esta_firmada())
                {

                    this.proceso.realizado = true;
                    this.proceso.resultado_ee = this.datos_dispo_numeroGEDO; // Disposicion_numeroGEDO();
                }
                else
                {
                    this.proceso.realizado = false;
                    this.proceso.resultado_ee = this.datos_dispo_resultado; // Disposicion_resultado();
                }
            }

            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                    this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                    this.proceso.id_devolucion_ee, this.proceso.resultado_ee,
                                    this.proceso.realizado,
                                    this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion.ProcesarTareaFirmar_verificarFirmado-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }


        }

        private void ProcesarRelacionar()
        {
            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.nro_tramite == null)
                ProcesarRelacionar_TareaFirmar_Caratula();
            else
                ProcesarRelacionar_Documento();

        }

        private void ProcesarRelacionar_TareaFirmar_Caratula()
        {
            this.ds_getInfoPaquete = WS_GetInfoPaquete();

            if (this.ds_getInfoPaquete.Tables.Count == 0 || this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count == 0)
                return;

            // Caratula   id_caratula, generada, tipo_actuacion, anio_actuacion, nro_actuacion
            // reparticion_actuacion, reparticion_usuario, resultado
            DataRow row = this.ds_getInfoPaquete.Tables["Caratula"].Rows[0];

            bool firmado = false;
            string resultado_firma = "";
            string numeroGEDO = "";

            int id_caratula = 0;
            int.TryParse(row["id_caratula"].ToString(), out id_caratula);
            int id_tarea_documento = -1;
            if (this.ds_getInfoPaquete.Tables["TareasAlaFirma"].Rows.Count > 0)
            {
                firmado = this.datos_dispo_esta_firmada; // Disposicion_esta_firmada();
                resultado_firma = this.datos_dispo_resultado; // Disposicion_resultado();
                numeroGEDO = this.datos_dispo_numeroGEDO; // Disposicion_numeroGEDO();

                id_tarea_documento = this.datos_dispo_relacion_id_tarea_documento;
                this.proceso.realizado = this.datos_dispo_relacion_subido; // Disposicion_relacion_subido();
                this.proceso.resultado_ee = this.datos_dispo_relacion_resultado; // Disposicion_relacion_resultado();

                if (!firmado)
                {
                    this.proceso.resultado_ee = resultado_firma;
                }

            }

            if (firmado && !this.datos_dispo_relacion_subido)
            {
                WS_Relacionar_TareasDocumento(id_tarea_documento);
            }
            else 
            {
                this.proceso.id_devolucion_ee = this.datos_dispo_relacion_id_relacion;
                this.proceso.resultado_ee = this.datos_dispo_relacion_resultado;
                this.proceso.realizado = this.datos_dispo_relacion_subido;
            }

            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                    this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                    this.proceso.id_devolucion_ee, this.proceso.resultado_ee,
                                    this.proceso.realizado,
                                    this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarRelacionar_TareaFirmar_Caratula-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }

        }

        private void ProcesarRelacionar_Documento()
        {
            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            int id_documento = this.proceso.nro_tramite.Value;

            WS_RelacionarDocumento(id_documento);
          
            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                    this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                    this.proceso.id_devolucion_ee, this.proceso.resultado_ee,
                                    this.proceso.realizado,
                                    this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarRelacionar_Documento-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }

        }

        #endregion

        private void ProcesarPaseExpediente()
        {

            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == proceso.id_tramitetarea);
            int id_tarea = tramite_tarea.id_tarea;

            if (id_tarea == (int)Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion ||
                id_tarea == (int)Constants.ENG_Tareas.SCP_Revision_Firma_Disposicion)
            {
                //Busco el resultado del Calificador para saber si es aprobado o rechazado
                int[] tareas = new int[6] { (int)SGI.Constants.ENG_Tareas.SSP_Calificar, (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                                                          (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1, (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                                                          (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1, (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2};

                SGI_Tramites_Tareas_HAB tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_tramitetarea == this.proceso.id_tramitetarea).FirstOrDefault();

                int id_solicitud = tarea.id_solicitud;

                var q = (from stt in db.SGI_Tramites_Tareas
                         join tt_hab in db.SGI_Tramites_Tareas_HAB on stt.id_tramitetarea equals tt_hab.id_tramitetarea
                         join lista_tareas in tareas.ToList() on stt.id_tarea equals lista_tareas
                         where tt_hab.id_solicitud == id_solicitud
                         orderby stt.id_tramitetarea descending
                         select new
                         {
                             stt.id_resultado
                         }
                    ).FirstOrDefault();

                if (q.id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)//Aprobado
                {
                    WS_PasarExpediente_aGrupo(this.proceso.id_paquete, this.proceso.id_caratula, "Pase al grupo " + this.Sector_SADE_receptor_RevFirmaDispo,
                    this.Sector_SADE_receptor_RevFirmaDispo, this.Reparticion_SADE_receptor_RevFirmaDispo);
                }
                else
                {
                    WS_PasarExpediente_aGrupo(this.proceso.id_paquete, this.proceso.id_caratula, "Pase al grupo " + Sector_SADE_receptor_RevFirmaDispo_Rechazo,
                    this.Sector_SADE_receptor_RevFirmaDispo_Rechazo, this.Reparticion_SADE_receptor_RevFirmaDispo_Rechazo);
                }
            }
            else
            {
                string username_destino_SADE = "";
                dynamic parametros = null;
                if (this.proceso.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.proceso.parametros_SADE);
                try { username_destino_SADE = parametros.Usuario.Value; }
                catch (Exception) { username_destino_SADE = this.UserName_SADE_Director; }

                // Averigua quien tiene el expediente en SADE
                string username_Origen_SADE = GetUsuarioAsignado(this.proceso.id_caratula);

                // Si no lo tiene ningun usuario es porque lo tiene un buzon grupal, en este caso el 102.2 por ej
                // entonces se toma el usuario SADE del perfil y se intenta realizar el pase con el, que deberia ser un usuario del buzon.
                if (username_Origen_SADE.Length == 0)
                    username_Origen_SADE = this.UserName_SADE;

                WS_PasarExpediente(this.proceso.id_paquete, "Pase al usuario " + username_destino_SADE, username_Origen_SADE, username_destino_SADE);
            }
            
                


            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                    this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                    this.proceso.id_devolucion_ee, this.proceso.resultado_ee,
                                    this.proceso.realizado,
                                    this.userid, 0, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarPaseExpediente-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }

        }

        private void ProcesarProvidencia()
        {
            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            byte[] documento = null;


            SGI_Tramites_Tareas_HAB tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_tramitetarea== this.proceso.id_tramitetarea).FirstOrDefault();

            int id_solicitud = tarea.id_solicitud;
            string mensajeError_GetDocumento = "";
            string acronimo_SADE = AcronicoPV;
            string tipoArchivo = "html";

            //Buscar datos encomienda
            int id_encomienda = 0;
            var datos_sol =
                (
                    from tt in this.db.SGI_Tramites_Tareas_HAB
                    join sol in this.db.SSIT_Solicitudes on tt.id_solicitud equals sol.id_solicitud
                    where tt.id_tramitetarea == this.proceso.id_tramitetarea
                    select new
                    {
                        sol.id_solicitud,
                    }
                ).FirstOrDefault();
            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == datos_sol.id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();


            id_encomienda = enc.id_encomienda;


            if (this.proceso.nro_tramite == 1) 
            {
                //---------------------------
                // Providencia Subgerente
                //---------------------------
                tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == id_solicitud
                                && (x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SCP_Revision_SubGerente
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SSP_Revision_SubGerente
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESP_Revision_SubGerente
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_SubGerente
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo
                                || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo
                                )).OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();
                SGI_Tarea_Revision_SubGerente tareaSubGerente = this.db.SGI_Tarea_Revision_SubGerente.Where(x => x.id_tramitetarea == tarea.id_tramitetarea).FirstOrDefault();

                //subgerente
                if (string.IsNullOrEmpty(tareaSubGerente.observacion_providencia))
                    throw new ExpedienteException("No tiene definida la providenvia del Subgerente");

                
                GetProvidencia(ref documento, tareaSubGerente.id_tramitetarea, tareaSubGerente.observacion_providencia, id_encomienda);

            }
            else
            {

                //---------------------------
                // Providencia Gerente
                //---------------------------

                tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == id_solicitud
                                            && (x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SCP_Revision_Gerente
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SSP_Revision_Gerente
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESP_Revision_Gerente_1
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESP_Revision_Gerente_2
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_Gerente_1
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_Gerente_2
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo
                                            || x.SGI_Tramites_Tareas.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo
                                            )).OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();
                SGI_Tarea_Revision_Gerente tareaGerente = this.db.SGI_Tarea_Revision_Gerente.Where(x => x.id_tramitetarea == tarea.id_tramitetarea).FirstOrDefault();

                if (string.IsNullOrEmpty(tareaGerente.observacion_providencia))
                    throw new ExpedienteException("No tiene definida la providenvia del Gerente");

                GetProvidencia(ref documento, tareaGerente.id_tramitetarea, tareaGerente.observacion_providencia, id_encomienda);

            }


            if (documento == null || documento.Length < 500 || !string.IsNullOrEmpty(mensajeError_GetDocumento))
            {
                if (!string.IsNullOrEmpty(mensajeError_GetDocumento))
                    this.proceso.resultado_ee = mensajeError_GetDocumento;
                else
                    this.proceso.resultado_ee = (documento == null ? "" : System.Text.ASCIIEncoding.ASCII.GetString(documento));
                this.proceso.realizado = false;
                documento = null;
            }
            
            this.ds_getInfoPaquete = WS_GetInfoPaquete();
            
            if (documento != null)
            {
                this.proceso.descripcion_tramite= this.datos_caratula_nro_expediente_completo;
                //Recupero el usuario_Sade
                dynamic parametros = null;
                if (this.proceso.parametros_SADE != null)
                    parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.proceso.parametros_SADE);

                string username_SADE = "";
                try { username_SADE = parametros.Usuario_SADE.Value; }
                catch (Exception) { username_SADE = this.UserName_SADE; }

                WS_SubirDocumento_ConAcro(ref documento, username_SADE, acronimo_SADE, tipoArchivo);
                try
                {
                    // copiar a dispo para verificar el archivo
                    if (Functions.CopiarDocumentoDisco())
                    {
                        string arch = Constants.PathTemporal + "documento-proceso-" + this.proceso.id_generar_expediente_proc + "-id_tramitetarea-" + this.proceso.id_tramitetarea + ".pdf"; ;
                        File.WriteAllBytes(arch, documento);
                    }
                }
                catch { }
            }


            try
            {
                //actualizar registros especifico del proceso
                this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                            this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                            this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                            this.userid, this.proceso.nro_tramite, this.proceso.descripcion_tramite);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error en transaccion. ProcesarDocumento-id_paquete:" + this.proceso.id_paquete);
                throw ex;
            }


        }

        private void GetProvidencia(ref byte[] documento, int id_tramitetarea, string observaciones, int id_encomienda)
        {
            int id_solicitud = this.db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == this.proceso.id_tramitetarea).id_solicitud;
            int id_documento = (int)this.proceso.nro_tramite;

            string appName = this.NombreSistema;

            documento = null;
            
            App_Data.dsImpresionProvidencia ds = new App_Data.dsImpresionProvidencia();
            
            DataRow row;
            DataTable dtDatos = ds.Tables["datos_providencia"];
            row = dtDatos.NewRow();
            row["observaciones"] = observaciones;
            dtDatos.Rows.Add(row);

            try
            {
                //Genero el html a pasar
                string html = GetHtmlProvidencia(id_tramitetarea);
                documento = Encoding.UTF8.GetBytes(html);
            }
            catch (Exception ex)
            {
                documento = null;
                LogError.Write(ex, "Error en en la generación de la providencia: id_solicitud:" + id_solicitud);
                throw ex;
            }

        }

        public string GetHtmlProvidencia(int id_tramitetarea)
        {
            string emailHtml = "";
            string surl = "";
            WebRequest request = null;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                Control ctl = new Control();
                surl = "http://" + HttpContext.Current.Request.Url.Authority + ctl.ResolveUrl("~/Reportes/ImprimirProvidencia.aspx");
                surl = BasePage.IPtoDomain(surl);
                ctl.Dispose();
                string url = string.Format("{0}?id_tramitetarea={1}", surl, id_tramitetarea);
                request = WebRequest.Create(url);
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
                emailHtml = reader.ReadToEnd();
                reader.Dispose();
                response.Dispose();
            }
            catch (Exception ex)
            {
                if (reader != null) reader.Dispose();
                if (response != null) response.Dispose();
                string mensaje = "";
                try
                {
                    if (HttpContext.Current != null)
                    {
                        if (HttpContext.Current.Request != null)
                        {
                            if (HttpContext.Current.Request.Url != null)
                            {
                                mensaje = " HttpContext.Current.Request.Url.Authority  = " + HttpContext.Current.Request.Url.Authority;
                                mensaje = mensaje + " HttpContext.Current.Request.Url.AbsoluteUri  = " + HttpContext.Current.Request.Url.AbsoluteUri;
                                mensaje = mensaje + " HttpContext.Current.Request.Url.AbsolutePath  = " + HttpContext.Current.Request.Url.AbsolutePath;
                            }
                            else
                            {
                                mensaje = "HttpContext.Current.Request.Url  = null";
                            }
                        }
                        else
                        {
                            mensaje = "HttpContext.Current.Request  = null";
                        }
                    }
                    else
                    {
                        mensaje = "HttpContext.Current  = null";
                    }
                }
                catch (Exception ex2)
                {
                    mensaje = "error en cadena de if " + ex2.Message;
                }
                LogError.Write(ex, mensaje);
                throw ex;
            }
            return emailHtml;
        }

        #region procesar contra WS Expediente Electronico

        private void WS_GenerarPaquete()
        {
            this.proceso.resultado_ee = "";

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;
            
            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.CrearPaquete(this.WS_userName, this.WS_passWord);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if ( service != null ) 
                    service.Dispose();
                this.proceso.resultado_ee = "Error en CrearPaquete de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }

        }

        private void WS_GenerarCaratulaPersonaFisica(
                    string apellido, string nombre, decimal cuit,
                    ws_ExpedienteElectronico.EETipodocumento tipo_doc,
                    string nro_documento, string email, string telefono, 
                    string descripcion_expediente, string motivo_expediente, string username_SADE,
                    string codigo_trata, string domicilio, string piso, string departamento, 
                    string codigo_postal, string motivo_externo)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.CaratularPersonaFisica_v2(
                            this.WS_userName, this.WS_passWord, this.proceso.id_paquete,
                            apellido, nombre, cuit, tipo_doc,Convert.ToDecimal(nro_documento), email, telefono, 
                            this.NombreSistema, descripcion_expediente, motivo_expediente,
                            username_SADE, codigo_trata, domicilio, piso, departamento,
                            codigo_postal, motivo_externo);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en CaratularPersonaFisica de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
        }

        private void WS_GenerarCaratulaPersonaJuridica(
                    string razon_social, decimal cuit, string email, string telefono,
                    string descripcion_expediente, string motivo_expediente, string username_SADE,
                    string codigo_trata, string domicilio, string piso, string departamento,
                    string codigo_postal, string motivo_externo)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.CaratularPersonaJuridica_v2(
                            this.WS_userName, this.WS_passWord, this.proceso.id_paquete,
                            razon_social, cuit, email, telefono,
                            this.NombreSistema, descripcion_expediente, motivo_expediente,
                            username_SADE, codigo_trata, domicilio, piso, departamento,
                            codigo_postal, motivo_externo);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = this.proceso.resultado_ee = "Error en CaratularPersonaJuridica de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
        }

        private async Task<byte[]> WS_SubirDocumento(byte[] documento, string username_SADE, string acronimoSADE, string formato_archivo)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;
                bool EnviarEmbebido = false;

                // identifica si es un pdf y si tiene los permisos correctos y si está firmado.
                try
                {
                    using (var pdf = new iTextSharp.text.pdf.PdfReader(documento))
                    {
                        if (!pdf.IsOpenedWithFullPermissions || ( service.isPdfFirmado(ref documento)  && acronimoSADE == "IFGRA" )) 
                        {
                            EnviarEmbebido = true;
                        }
                    }

                }
                catch (Exception)
                {
                    EnviarEmbebido = false;
                }


                if (EnviarEmbebido)
                {
                    this.proceso.id_devolucion_ee = service.Subir_Documentos_Embebidos_ConAcroAndTipo(
                                this.WS_userName, this.WS_passWord, this.proceso.id_paquete,
                                documento, this.proceso.nro_tramite.ToString(), this.proceso.descripcion_tramite,
                                this.NombreSistema, username_SADE, "IF", "txt", "Documento adjunto - " + this.proceso.nro_tramite.ToString());

                }
                else
                {
                    this.proceso.id_devolucion_ee = service.Subir_Documento_ConAcroAndTipo(
                                  this.WS_userName, this.WS_passWord, this.proceso.id_paquete,
                                  documento, this.proceso.nro_tramite.ToString(), this.proceso.descripcion_tramite,
                                  this.NombreSistema, username_SADE, acronimoSADE, formato_archivo,false);
                }

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Subir_Documento de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
            return documento;
        }

        private void WS_SubirDocumento_ConAcro(ref byte[] documento, string username_SADE, string acronimoSADE, string tipoArchivo)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.Subir_Documento_ConAcroAndTipo(
                                    this.WS_userName, this.WS_passWord, this.proceso.id_paquete,
                                    documento, this.proceso.nro_tramite.ToString(), this.proceso.descripcion_tramite,
                                    this.NombreSistema, username_SADE, acronimoSADE, tipoArchivo,false);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();

                this.proceso.resultado_ee = "Error en Subir_Documento de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
        }

        private async Task<byte[]> WS_SubirDocumentoEmbebido (byte[] documento, string nombreArchivo, string acronimoSADE, string detalle)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.Subir_Documentos_Embebidos_ConAcroAndTipo(
                                    this.WS_userName, this.WS_passWord, this.proceso.id_paquete,
                                    documento, detalle, this.proceso.descripcion_tramite,
                                    this.NombreSistema, this.UserName_SADE, acronimoSADE,"txt", nombreArchivo);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Subir_Documentos_Embebidos de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
            return documento;
        }

        private void WS_BloquearExpediente()
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.Bloquear_Expediente(
                                this.WS_userName, this.WS_passWord, 
                                this.proceso.id_paquete, this.proceso.id_caratula, 
                                this.NombreSistema, this.UserName_SADE);
                                    
                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Bloquear_Expediente de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
        }

        private void WS_DesbloquearExpediente()
        {
            
            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;
                
                this.proceso.id_devolucion_ee = service.Desbloquear_Expediente(
                            this.WS_userName, this.WS_passWord, this.proceso.id_paquete, 
                            this.proceso.id_caratula, this.NombreSistema, this.UserName_SADE);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Desbloquear_Expediente de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }
        }

        private void WS_TareaFirmarDisposicion(ref byte[] documento, ws_ExpedienteElectronico.WS_Item[] datosDocumento, string username_SADE, string[] firmantes_SADE, string username_SADE_receptor)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.generarTareaAlaFirma(
                                this.WS_userName, this.WS_passWord,
                                this.proceso.id_paquete, documento, datosDocumento,
                                this.proceso.descripcion_tramite, this.NombreSistema,
                                username_SADE, firmantes_SADE, username_SADE_receptor);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en generarTareaAlaFirma de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }

        }

        private ws_ExpedienteElectronico.dsInfoPaquete WS_GetInfoPaquete()
        {
            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;
            ws_ExpedienteElectronico.dsInfoPaquete ds = null;
            try
            {
                ds = new ws_ExpedienteElectronico.dsInfoPaquete();
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;
                ds = service.Get_Info_Paquete(this.WS_userName, this.WS_passWord, this.proceso.id_paquete);
                service.Dispose();
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Get_Info_Paquete de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }

            return  ds;
        }

        private void WS_RelacionarDocumento(int id_documento)
        {
            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                string usernameSADE = service.GetUsuarioAsignado(this.WS_userName, this.WS_passWord, this.proceso.id_caratula);

                this.proceso.id_devolucion_ee = service.Relacionar_Documento(
                                this.WS_userName, this.WS_passWord, 
                                this.proceso.id_caratula, id_documento, 
                                this.NombreSistema, usernameSADE);

                service.Dispose();
                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Relacionar_Documento de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
            }

        }

        private void WS_Relacionar_TareasDocumento(int id_tarea_documento)
        {
            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;
                
                string usernameSADE = service.GetUsuarioAsignado(this.WS_userName, this.WS_passWord, this.datos_caratula_id_caratula);

                if (usernameSADE.Length == 0)
                    throw new Exception("El expediente no se encuentra asignado a ningún usuario, es probable que esté asignado a un grupo, no es posible relacionar la disposición a la carátula, primero debe realizar un pase en SADE a un usuario y no un grupo.");

                this.proceso.id_devolucion_ee = service.Relacionar_TareasDocumento(
                                this.WS_userName, this.WS_passWord,
                                this.datos_caratula_id_caratula, id_tarea_documento,
                                this.NombreSistema, usernameSADE);

                service.Dispose();
                this.proceso.resultado_ee = "Relacionado OK";
                this.proceso.realizado = true;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en Relacionar_TareasDocumento de ws_ExpedienteElectronico. " + ex.Message;
                this.proceso.realizado = false;
                LogError.Write(ex, string.Format("Error en Relacionar_TareasDocumento de ws_ExpedienteElectronico. Parametros: id_tarea_documento = {0}, id_caratula = {1}", id_tarea_documento, this.datos_caratula_id_caratula));
            }

        }

        private void WS_GetPdfCaratula(int id_paquete, ref byte[] documento)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                documento = service.GetPdfCaratula(this.WS_userName, this.WS_passWord, id_paquete);

                service.Dispose();

            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                throw ex;
            }

            if (service != null)
                service.Dispose();

        }

        private void WS_GetPdfDisposicion(int id_paquete, ref byte[] documento)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                documento = service.GetPdfDisposicion(this.WS_userName, this.WS_passWord, id_paquete);
                //var q =
                //    (
                //        from c in dbFiles.Certificados
                //        where c.id_certificado == 10000
                //        select new

                //        {
                //            c.Certificado
                //        }
                //    ).FirstOrDefault();

                //documento = q.Certificado;

                service.Dispose();

            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                throw ex;
            }

            if (service != null)
                service.Dispose();

        }

        private string GetUsuarioAsignado(int id_caratula)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            service.Url = this.WS_url;
            string usuario = service.GetUsuarioAsignado(this.WS_userName, this.WS_passWord, id_caratula);
            return usuario;
        }

        private void WS_PasarExpediente(int id_paquete, string motivo, string usr_origen, string usr_destino)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;

                this.proceso.id_devolucion_ee = service.PasarExpediente_aUsuario(this.WS_userName, this.WS_passWord,
                                    id_paquete, motivo, usr_origen, usr_destino);

                service.Dispose();

                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
                this.proceso.nro_tramite = null;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en pasar expediente de WS_PasarExpediente. " + ex.Message;
                this.proceso.realizado = false;
            }


        }

        private void WS_PasarExpediente_aGrupo(int id_paquete,int id_caratula, string motivo,  string sector_destino, string reparticion_destino)
        {

            ws_ExpedienteElectronico.ws_ExpedienteElectronico service = null;

            try
            {
                service = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                service.Url = this.WS_url;


                string usr_origen = service.GetUsuarioAsignado(this.WS_userName, this.WS_passWord, id_caratula);
                if (string.IsNullOrEmpty(usr_origen))
                    usr_origen = this.UserName_SADE;

                this.proceso.id_devolucion_ee = service.PasarExpediente_aGrupo(this.WS_userName, this.WS_passWord,
                                    id_paquete, motivo, usr_origen, sector_destino, reparticion_destino);

                service.Dispose();

                this.proceso.resultado_ee = "";
                this.proceso.realizado = true;
                this.proceso.nro_tramite = null;
            }
            catch (Exception ex)
            {
                if (service != null)
                    service.Dispose();
                this.proceso.resultado_ee = "Error en pasar expediente de WS_PasarExpediente_aGrupo. " + ex.Message;
                this.proceso.realizado = false;
            }


        }


        #endregion


        public class ExpedienteException : Exception
        {
            public ExpedienteException(string mensaje)
                : base(mensaje, new Exception()) {}
        }

        
        private async void ProcesarPlanos()
        {
            if (this.proceso.id_paquete == 0) // no hay paquete que englobe al registro
                return;

            if (this.proceso.id_caratula == 0) // no hay caratula  que englobe al registro
                return;

            if (this.proceso.id_devolucion_ee != -1)  // ya fue procesado
                return;

            byte[] documento = null;

            bool forzar_procesado = false;
            bool guardar_documento = true;
            string mensajeError_GetDocumento = "";

            string username_SADE = "";
            string acronimo_SADE = "";
            string formato_archivo = "";

            //Recupero los datos de los parámetros de SADE
            dynamic parametros = null;
            if (this.proceso.parametros_SADE != null)
                parametros = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.proceso.parametros_SADE);


            try { username_SADE = parametros.Usuario_SADE.Value; }
            catch (Exception) { username_SADE = this.UserName_SADE; }


            try { acronimo_SADE = parametros.Acronimo_SADE.Value; }
            catch (Exception) { acronimo_SADE = Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma"); }

            try { formato_archivo = parametros.formato_archivo.Value; }
            catch (Exception) { formato_archivo = "pdf"; }


            GetDocumentoReturn getdocu = await GetDocumento();

            if (documento == null || documento.Length < 500 || !string.IsNullOrEmpty(mensajeError_GetDocumento))
            {
                if (!string.IsNullOrEmpty(mensajeError_GetDocumento))
                    this.proceso.resultado_ee = mensajeError_GetDocumento;
                else
                    this.proceso.resultado_ee = (documento == null ? "" : System.Text.ASCIIEncoding.ASCII.GetString(documento));
                this.proceso.realizado = false;
                documento = null;
            }


            if (documento != null)
            {
                int id_file = (int)this.proceso.nro_tramite;
                var datosPlano = (from encplan in db.Encomienda_Planos
                                 join tplan in db.TiposDePlanos on encplan.id_tipo_plano equals tplan.id_tipo_plano
                                 where  encplan.id_file == id_file
                                 select new
                                 {
                                     encplan.nombre_archivo,
                                     tplan.acronimo_SADE,
                                     tplan.requiere_detalle,
                                     encplan.detalle,
                                     tplan.nombre
                                 }).FirstOrDefault();

                string detalle= datosPlano.requiere_detalle.Value? datosPlano.detalle:datosPlano.nombre;
                documento = await WS_SubirDocumentoEmbebido(documento, datosPlano.nombre_archivo, datosPlano.acronimo_SADE, detalle);

                try
                {
                    // copiar a dispo para verificar el archivo
                    if (Functions.CopiarDocumentoDisco())
                    {
                        string arch = Constants.PathTemporal + "documento-proceso-" + this.proceso.id_generar_expediente_proc + "-id_tramitetarea-" + this.proceso.id_tramitetarea + ".pdf"; ;

                        File.WriteAllBytes(arch, documento);
                    }

                }
                catch { }

            }


            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    //actualizar registros especifico del proceso
                    this.db.SGI_Tarea_Generar_Expediente_Procesos_update(
                                this.proceso.id_generar_expediente_proc, this.proceso.id_proceso,
                                this.proceso.id_devolucion_ee, this.proceso.resultado_ee, this.proceso.realizado,
                                this.userid, this.proceso.nro_tramite, this.proceso.descripcion_tramite);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. ProcesarDocumento-id_paquete:" + this.proceso.id_paquete);
                    throw ex;
                }

            }


        }


        #region generar documento disposicion

        public void GenerarDisposicionPdf(int id_paquete, Guid userid, ref byte[] documento, int id_generar_expediente_proc)
        {

            string html_dispo = "";
            bool existe_reg_dispo = false;
            bool existe_reg_caratula = false;
            bool dispo_realizada = false;
            int id_solicitud = 0;
            int id_tramitetarea = 0;
            int id_caratula = 0;
            int id_certificado_disposicion = -1;

            //  disposicion solo se puede generar cuando la caratula esta terminada
            var q =
                (
                    from proceso in db.SGI_Tarea_Generar_Expediente_Procesos
                    join tt in db.SGI_Tramites_Tareas on proceso.id_tramitetarea equals tt.id_tramitetarea
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    where proceso.id_paquete == id_paquete && new[] { 2, 6 }.Contains(proceso.id_proceso)
                    select new
                    {
                        proceso.id_caratula,
                        proceso.id_proceso,
                        proceso.realizado,
                        proceso.nro_tramite,
                        tt.id_tramitetarea,
                        tt_hab.id_solicitud
                    }
                ).ToList();

            foreach (var item in q)
            {
                if (item.id_proceso == 2)
                    existe_reg_caratula = item.realizado;

                if (item.id_proceso == 6)
                {
                    dispo_realizada = item.realizado;
                    existe_reg_dispo = true;
                    id_certificado_disposicion = (int)item.nro_tramite;
                }

                id_solicitud = item.id_solicitud;
                id_tramitetarea = item.id_tramitetarea;
                id_caratula = item.id_caratula;
            }

            // la caratula no esta hecha y por ende el expediente no esta o la disposicion ya esta hecha
            if (!existe_reg_caratula || !existe_reg_dispo) // || id_certificado_disposicion > 0 )
                return;

            // buscar si exienten procesos pendientes anteriores a este proceso
            // pendientes reales, es decir, sin el ok de sade aunque tenga el ok del ws expediente electronico
            bool hay_procesos_pendientes = true;
            Expediente procesoExpediente = new Expediente(userid, Constants.ApplicationName);
            List<ProcesoExpediente> lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramitetarea);
            if (lstProcesos == null || lstProcesos.Count == 0)
                hay_procesos_pendientes = true;
            else
                hay_procesos_pendientes = lstProcesos.Exists(x => x.id_generar_expediente_proc < id_generar_expediente_proc && (x.realizado == false || !string.IsNullOrEmpty(x.resultado_sade_error)));

            if (hay_procesos_pendientes)
                throw new Exception("Para generar la Disposición, todas los procesos deben haber finalizado.");


            this.ds_getInfoPaquete = new ws_ExpedienteElectronico.dsInfoPaquete();
            this.ds_getInfoPaquete.Merge(procesoExpediente.ds_getInfoPaquete, false, MissingSchemaAction.Ignore);

            if (this.ds_getInfoPaquete.Tables.Count == 0 || this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count == 0)
                return ;

            DataRow row = this.ds_getInfoPaquete.Tables["Caratula"].Rows[0];

            id_caratula = 0;
            int.TryParse(row["id_caratula"].ToString(), out id_caratula);

            bool generada = false;
            bool.TryParse(row["generada"].ToString(), out generada);

            string tipo_actuacion = Convert.ToString(row["tipo_actuacion"]);

            int anio_actuacion = 0;
            int.TryParse(row["anio_actuacion"].ToString(), out anio_actuacion);

            int nro_actuacion = 0;
            int.TryParse(row["nro_actuacion"].ToString(), out nro_actuacion);

            string reparticion_actuacion = Convert.ToString(row["reparticion_actuacion"]);
            string reparticion_usuario = Convert.ToString(row["reparticion_usuario"]);
            string expediente = Convert.ToString(row["resultado"]);

            if (generada && !string.IsNullOrEmpty(expediente))
            {

                PdfDisposicion dispo_html = null;
                try
                {
                    dispo_html = new PdfDisposicion();
                    dispo_html.GenerarPDF_Disposicion(id_solicitud, id_tramitetarea, userid, expediente, id_paquete, id_caratula, ref documento);

                    dispo_html.Dispose();

                }
                catch (Exception ex)
                {
                    if (dispo_html != null) dispo_html.Dispose();
                    throw ex;
                }

            }

        }

        public string GenerarDisposicionHtml(int id_paquete, Guid userid, ref byte[] documento, int id_generar_expediente_proc)
        {
            string html_dispo = "";
            bool existe_reg_dispo = false;
            bool existe_reg_caratula = false;
            bool dispo_realizada = false;
            int id_solicitud = 0;
            int id_tramitetarea = 0;
            int id_caratula = 0;
            int id_certificado_disposicion = -1;

            //  disposicion solo se puede generar cuando la caratula esta terminada
            var q =
                (
                    from proceso in db.SGI_Tarea_Generar_Expediente_Procesos
                    join tt in db.SGI_Tramites_Tareas on proceso.id_tramitetarea equals tt.id_tramitetarea
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    where proceso.id_paquete == id_paquete && new[] { 2, 6 }.Contains(proceso.id_proceso)
                    select new
                    {
                        proceso.id_caratula,
                        proceso.id_proceso,
                        proceso.realizado,
                        proceso.nro_tramite,
                        tt.id_tramitetarea,
                        tt_hab.id_solicitud
                    }
                ).ToList();

            foreach (var item in q)
            {
                if (item.id_proceso == 2)
                    existe_reg_caratula = item.realizado;

                if (item.id_proceso == 6)
                {
                    dispo_realizada = item.realizado;
                    existe_reg_dispo = true;
                    id_certificado_disposicion = (int)item.nro_tramite;
                }

                id_solicitud = item.id_solicitud;
                id_tramitetarea = item.id_tramitetarea;
                id_caratula = item.id_caratula;
            }

            // la caratula no esta hecha y por ende el expediente no esta o la disposicion ya esta hecha
            if (!existe_reg_caratula || !existe_reg_dispo ) // || id_certificado_disposicion > 0 )
                return html_dispo;

            // buscar si exienten procesos pendientes anteriores a este proceso
            // pendientes reales, es decir, sin el ok de sade aunque tenga el ok del ws expediente electronico
            bool hay_procesos_pendientes = true;
            Expediente procesoExpediente = new Expediente(userid, Constants.ApplicationName);
            List<ProcesoExpediente> lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramitetarea);
            if (lstProcesos == null || lstProcesos.Count == 0)
                hay_procesos_pendientes = true;
            else
                hay_procesos_pendientes = lstProcesos.Exists(x => x.id_generar_expediente_proc < id_generar_expediente_proc && ( x.realizado == false || !string.IsNullOrEmpty(x.resultado_sade_error) ) );

            if (hay_procesos_pendientes)
                throw new Exception("Para generar la Disposición, todas los procesos deben haber finalizado.");

            this.ds_getInfoPaquete = WS_GetInfoPaquete();

            if (this.ds_getInfoPaquete.Tables.Count == 0 || this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count == 0)
                return html_dispo;

            // Caratula   id_caratula, generada, tipo_actuacion, anio_actuacion, nro_actuacion
            // reparticion_actuacion, reparticion_usuario, resultado
            DataRow row = this.ds_getInfoPaquete.Tables["Caratula"].Rows[0];

            id_caratula = 0;
            int.TryParse(row["id_caratula"].ToString(), out id_caratula);

            bool generada = false;
            bool.TryParse(row["generada"].ToString(), out generada);

            string tipo_actuacion = Convert.ToString(row["tipo_actuacion"]);

            int anio_actuacion = 0;
            int.TryParse(row["anio_actuacion"].ToString(), out anio_actuacion);

            int nro_actuacion = 0;
            int.TryParse(row["nro_actuacion"].ToString(), out nro_actuacion);

            string reparticion_actuacion = Convert.ToString(row["reparticion_actuacion"]);
            string reparticion_usuario = Convert.ToString(row["reparticion_usuario"]);
            string resultado = Convert.ToString(row["resultado"]);

            if (generada && ! string.IsNullOrEmpty(resultado) )
            {
                string expediente = resultado;
                //id_certificado_disposicion = GenerarPDF_Disposicion(id_solicitud, id_tramitetarea, userid, expediente, this.proceso.id_paquete, id_caratula, ref documento);

                PdfDisposicion dispo_html = null;
                try
                {
                    dispo_html = new PdfDisposicion();
                    string html = dispo_html.GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, expediente);
                    dispo_html.Dispose();

                    byte[] utfBytes = Encoding.GetEncoding(Functions.GetParametroChar("Server.Encoding")).GetBytes(html_dispo);
                    documento = utfBytes;

                    //documento = Convert.ToBase64CharArray(documento, );
                    //File.WriteAllBytes("C:\\Users\\cnieto.AR.MOST\\Desktop\\borrar\\dispo.html", documento);
                }
                catch (Exception ex)
                {
                    if (dispo_html != null) dispo_html.Dispose();
                    throw ex;
                }
                

            }

            return html_dispo;

        }

        //public string GenerarDocumentoPDF_Disposicion(Guid userid, ref byte[] documento, int id_tramitetarea)
        //{
            
        //    bool existe_reg_dispo = false;
        //    bool existe_reg_caratula = false;
        //    bool dispo_realizada = false;
        //    int id_solicitud = 0;
        //    int id_generar_expediente_proc = 0;
        //    int id_caratula = 0;
        //    int id_certificado_disposicion = -1;
        //    int id_paquete = 0;

        //    //  disposicion solo se puede generar cuando la caratula esta terminada
        //    var q =
        //        (
        //            from proceso in db.SGI_Tarea_Generar_Expediente_Procesos
        //            join tt in db.SGI_Tramites_Tareas on proceso.id_tramitetarea equals tt.id_tramitetarea
        //            where proceso.id_tramitetarea == id_tramitetarea && new[] { 2, 6 }.Contains(proceso.id_proceso)
        //            select new
        //            {
        //                proceso.id_generar_expediente_proc,
        //                proceso.id_caratula,
        //                proceso.id_paquete,
        //                proceso.id_proceso,
        //                proceso.realizado,
        //                proceso.nro_tramite,
        //                tt.id_tramitetarea,
        //                tt.id_solicitud
        //            }
        //        ).ToList();

        //    foreach (var item in q)
        //    {
        //        if (item.id_proceso == 2)
        //            existe_reg_caratula = item.realizado;

        //        if (item.id_proceso == 6)
        //        {
        //            dispo_realizada = item.realizado;
        //            existe_reg_dispo = true;
        //            id_certificado_disposicion = (int)item.nro_tramite;
        //            id_generar_expediente_proc = item.id_generar_expediente_proc;
        //        }

        //        id_solicitud = item.id_solicitud;
        //        //id_tramitetarea = item.id_tramitetarea;
        //        id_caratula = item.id_caratula;
        //        id_paquete = item.id_paquete;

        //    }

        //    // la caratula no esta hecha y por ende el expediente no esta o la disposicion ya esta hecha
        //    if (!existe_reg_caratula || !existe_reg_dispo) // || id_certificado_disposicion > 0 )
        //        return id_certificado_disposicion;

        //    // buscar si exienten procesos pendientes anteriores a este proceso
        //    // pendientes reales, es decir, sin el ok de sade aunque tenga el ok del ws expediente electronico
        //    bool hay_procesos_pendientes = true;
        //    Expediente procesoExpediente = new Expediente(userid, Constants.ApplicationName);
        //    List<ProcesoExpediente> lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramitetarea);
        //    if (lstProcesos == null || lstProcesos.Count == 0)
        //        hay_procesos_pendientes = true;
        //    else
        //        hay_procesos_pendientes = lstProcesos.Exists(x => x.id_generar_expediente_proc < id_generar_expediente_proc && (x.realizado == false || !string.IsNullOrEmpty(x.resultado_sade_error)));

        //    if (hay_procesos_pendientes)
        //        throw new Exception("Para generar la Disposición, todas los procesos deben haber finalizado.");


        //    this.ds_getInfoPaquete = new ws_ExpedienteElectronico.dsInfoPaquete();
        //    this.ds_getInfoPaquete.Merge(procesoExpediente.ds_getInfoPaquete, false, MissingSchemaAction.Ignore);

        //    if (this.ds_getInfoPaquete.Tables.Count == 0 || this.ds_getInfoPaquete.Tables["Caratula"].Rows.Count == 0)
        //        return id_certificado_disposicion;

        //    // Caratula   id_caratula, generada, tipo_actuacion, anio_actuacion, nro_actuacion
        //    // reparticion_actuacion, reparticion_usuario, resultado
        //    DataRow row = this.ds_getInfoPaquete.Tables["Caratula"].Rows[0];

        //    id_caratula = 0;
        //    int.TryParse(row["id_caratula"].ToString(), out id_caratula);

        //    bool generada = false;
        //    bool.TryParse(row["generada"].ToString(), out generada);

        //    string tipo_actuacion = Convert.ToString(row["tipo_actuacion"]);

        //    int anio_actuacion = 0;
        //    int.TryParse(row["anio_actuacion"].ToString(), out anio_actuacion);

        //    int nro_actuacion = 0;
        //    int.TryParse(row["nro_actuacion"].ToString(), out nro_actuacion);

        //    string reparticion_actuacion = Convert.ToString(row["reparticion_actuacion"]);
        //    string reparticion_usuario = Convert.ToString(row["reparticion_usuario"]);
        //    string resultado = Convert.ToString(row["resultado"]);

        //    string html = "";
        //    if (generada && !string.IsNullOrEmpty(resultado))
        //    {
        //        string expediente = resultado;
        //        PdfDisposicion dispo_html = null;
        //        try
        //        {
        //            dispo_html = new PdfDisposicion();
        //            html = dispo_html.GenerarHtml_Disposicion(userid, id_solicitud, id_tramitetarea, expediente);
        //            dispo_html.Dispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            if (dispo_html != null) dispo_html.Dispose();
        //            throw ex;
        //        }
                
                
        //        //id_certificado_disposicion = GenerarPDF_Disposicion(id_solicitud, id_tramitetarea, userid, expediente, id_paquete, id_caratula, ref documento);
        //    }

        //    return html;


        //}

        #endregion

        #region parametros EE

        public class ParametrosEE
        {

            public static string GetParam_ValorChar(string codigo_param)
            {
                EE_Entities db = null;
                string param = ""; 
                try
                {
                    db = new EE_Entities();
                    param = db.Parametros.Where(x => x.cod_param.Equals(codigo_param)).Select(x => x.valorchar_param).FirstOrDefault();
                    db.Dispose();
                    if (param == null) param = "";
                }
                catch (Exception ex)
                {
                    if (db != null)
                        db.Dispose();
                    throw ex;
                }

                return param;
            }

            public static string GetParam_ValorNum(string codigo_param)
            {
                DGHP_Entities db = null;
                decimal? param = null;

                try
                {
                    db = new DGHP_Entities();

                    param = db.Parametros.Where(x => x.cod_param.Equals(codigo_param)).Select(x => x.valornum_param).FirstOrDefault();

                    db.Dispose();
                }
                catch (Exception ex)
                {
                    if (db != null)
                        db.Dispose();
                    throw ex;
                }

                if (param.HasValue)
                    return param.ToString();
                else
                    return "";

            }

        }

        #endregion


        #region generar documento plancheta

        private string Buscar_ObservacionPlancheta_Gerente(int id_solicitud, int id_tramitetarea)
        {
            string observ = "";
            int[] tareas = new int[2] { (int)Constants.ENG_Tareas.SSP_Revision_Gerente, (int)Constants.ENG_Tareas.SCP_Revision_Gerente };

            List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea, tareas).OrderByDescending(x => x.id_tramitetarea).ToList();

            if (list_tramite_tarea.Count > 0)
            {
                int id_tramitetarea_gerente = list_tramite_tarea[0].id_tramitetarea;
                var q =
                    (
                        from sub in db.SGI_Tarea_Revision_Gerente
                        where sub.id_tramitetarea == id_tramitetarea_gerente
                        select new
                        {
                            sub.observacion_plancheta
                        }

                    ).FirstOrDefault();

                if (q != null)
                    observ = q.observacion_plancheta;
            }
            else
            {
                int[] tareasSubgerente = new int[2] { (int)Constants.ENG_Tareas.SSP_Revision_SubGerente, (int)Constants.ENG_Tareas.SCP_Revision_SubGerente };

                List<TramiteTareaAnteriores> list_tramite_tarea_subgerente = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea((int)Constants.GruposDeTramite.HAB,
                    id_solicitud, id_tramitetarea, tareasSubgerente).OrderByDescending(x=>x.id_tramitetarea).ToList();
                if (list_tramite_tarea_subgerente.Count > 0)
                {
                    int id_tramitetarea_subgerente = list_tramite_tarea_subgerente[0].id_tramitetarea;
                    var qs =
                        (
                            from sub in db.SGI_Tarea_Revision_SubGerente
                            where sub.id_tramitetarea == id_tramitetarea_subgerente
                            select new
                            {
                                sub.observacion_plancheta
                            }

                        ).FirstOrDefault();

                    if (qs != null)
                        observ = qs.observacion_plancheta;
                }
            }

            return observ;
        }

         public App_Data.dsImpresionProfesional GenerarDataSetProfesional(int id_profesional) 
        {
            App_Data.dsImpresionProfesional ds = new App_Data.dsImpresionProfesional();

            var datos = (
                    from profesionales in db.Profesional
                    where profesionales.Id == id_profesional
                    select new
                    {
                        id_profesional = profesionales.Id,
                        ConsejoProfesional = profesionales.ConsejoProfesional.Nombre,
                        Matricula = profesionales.Matricula,
                        Apellido = profesionales.Apellido,
                        Nombre = profesionales.Nombre,
                        NroDocumento = profesionales.NroDocumento,
                        Calle = profesionales.Calle,
                        NroPuerta = profesionales.NroPuerta,
                        Piso = profesionales.Piso,
                        Depto = profesionales.Depto,
                        Localidad = profesionales.Localidad,
                        Provincia = profesionales.Provincia,
                        Email = profesionales.Email,
                        Sms = profesionales.Sms,
                        Telefono = profesionales.Telefono,
                        Cuit = profesionales.Cuit,
                        IngresosBrutos = profesionales.IngresosBrutos,
                        MatriculaMetrogas = profesionales.MatriculaMetrogas,
                        CategoriaMetrogas = profesionales.CategoriaMetrogas
                    }

                ).ToList();

            DataTable dtProf = ds.Tables["Profesional"];
            DataRow row;
            foreach (var item in datos)
            {
                row = dtProf.NewRow();

                row["id_profesional"] = item.id_profesional ;
                row["ConsejoProfesional"] = item.ConsejoProfesional;
                row["Matricula"] = item.Matricula;
                row["Apellido"] = item.Apellido;
                row["Nombre"] = item.Nombre;
                row["NroDocumento"] = item.NroDocumento;
                row["Calle"] = item.Calle;
                row["NroPuerta"] = item.NroPuerta;
                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;
                row["Localidad"] = item.Localidad;
                row["Provincia"] = item.Provincia;
                row["Email"] = item.Email;
                row["Sms"] = item.Sms;
                row["Telefono"] = item.Telefono;
                row["Cuit"] = item.Cuit;
                row["IngresosBrutos"] = item.IngresosBrutos;
                row["MatriculaMetrogas"] = item.MatriculaMetrogas;
                row["CategoriaMetrogas"] = item.CategoriaMetrogas;

                dtProf.Rows.Add(row);
            }

            return ds;
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        
        public async Task<GenerarPdf_Plancheta_HabilitacionReturn> GenerarPdf_Plancheta_Habilitacion(Guid userId, int id_tramitetarea, int id_paquete)
        {
            GenerarPdf_Plancheta_HabilitacionReturn ret = new GenerarPdf_Plancheta_HabilitacionReturn();

            ret.retorno = 0;
            ret.documento = null;

            if (id_paquete == 0)
                return ret;

            Expediente expeNuevo = new Expediente(userId, Constants.ApplicationName);
            this.ds_getInfoPaquete = (ws_ExpedienteElectronico.dsInfoPaquete)expeNuevo.GetInfoPaquete(id_paquete);
            expeNuevo.Dispose();


            if (!this.datos_caratula_subida_sade) //if (!Caratula_subida_sade())
            {
                throw new ExpedienteException("Falta generar carátula en SADE.");
            }

            var q_datos =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join ssit in db.SSIT_Solicitudes on tt_hab.id_solicitud equals ssit.id_solicitud
                    where tt.id_tramitetarea == id_tramitetarea 
                    select new
                    {
                        tt_hab.id_solicitud

                    }
                ).FirstOrDefault();

            int id_solicitud = q_datos.id_solicitud;
            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

            string expediente_actuacion = this.datos_caratula_nro_expediente; // GetExpediente();

            ret.documento = await Plancheta.GenerarPdfPlanchetahabilitacion(id_solicitud, id_tramitetarea, enc.id_encomienda, expediente_actuacion, false);

            using (TransactionScope Tran = new TransactionScope())
            {

                try
                {
                    ret.retorno = ws_FilesRest.subirArchivo("Plancheta.pdf", ret.documento);

                    int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.PLANCHETA_HABILITACION;
                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                    db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, true, ret.retorno, "Plancheta.pdf", userId, param_id_docadjunto);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. GenerarPdf_Plancheta_Habilitacion-id_tramitetarea:" + id_tramitetarea + "id_solicitud:" + id_solicitud);
                    throw ex;
                }
            }

            return ret;
        }


        #endregion

    }



}
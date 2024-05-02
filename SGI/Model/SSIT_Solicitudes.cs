

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class SSIT_Solicitudes
    {
        public SSIT_Solicitudes()
        {
            this.Encomienda_SSIT_Solicitudes = new HashSet<Encomienda_SSIT_Solicitudes>();
            this.Eximicion_CAA = new HashSet<Eximicion_CAA>();
            this.SGI_Tramites_Tareas_HAB = new HashSet<SGI_Tramites_Tareas_HAB>();
            this.Solicitud_planoVisado = new HashSet<Solicitud_planoVisado>();
            this.SSIT_Solicitudes_AvisoCaducidad = new HashSet<SSIT_Solicitudes_AvisoCaducidad>();
            this.SSIT_Solicitudes_Baja = new HashSet<SSIT_Solicitudes_Baja>();
            this.SSIT_Solicitudes_Encomienda = new HashSet<SSIT_Solicitudes_Encomienda>();
            this.SSIT_Solicitudes_Firmantes_PersonasFisicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasFisicas>();
            this.SSIT_Solicitudes_Firmantes_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasJuridicas>();
            this.SSIT_Solicitudes_HistorialEstados = new HashSet<SSIT_Solicitudes_HistorialEstados>();
            this.SSIT_Solicitudes_Notificaciones = new HashSet<SSIT_Solicitudes_Notificaciones>();
            this.SSIT_Solicitudes_Observaciones = new HashSet<SSIT_Solicitudes_Observaciones>();
            this.SSIT_Solicitudes_Pagos = new HashSet<SSIT_Solicitudes_Pagos>();
            this.SSIT_Solicitudes_Titulares_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasFisicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas>();
            this.SSIT_Solicitudes_Ubicaciones = new HashSet<SSIT_Solicitudes_Ubicaciones>();
            this.SSIT_Solicitudes_RubrosCN = new HashSet<SSIT_Solicitudes_RubrosCN>();
            this.SSIT_Solicitudes_Origen1 = new HashSet<SSIT_Solicitudes_Origen>();
            this.SSIT_DocumentosAdjuntos = new HashSet<SSIT_DocumentosAdjuntos>();
            this.SSIT_Solicitudes_AvisoRechazo = new HashSet<SSIT_Solicitudes_AvisoRechazo>();
        }
    
        public int id_solicitud { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public Nullable<int> MatriculaEscribano { get; set; }
        public string NroExpediente { get; set; }
        public int id_estado { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string NroExpedienteSade { get; set; }
        public string telefono { get; set; }
        public Nullable<System.DateTime> FechaLibrado { get; set; }
        public string CodigoSeguridad { get; set; }
        public bool Servidumbre_paso { get; set; }
        public string CodArea { get; set; }
        public string Prefijo { get; set; }
        public string Sufijo { get; set; }
        public string NroDisposicionSADE { get; set; }
        public Nullable<System.DateTime> FechaDisposicion { get; set; }
        public Nullable<int> NroCertificado { get; set; }
        public string NroExpedienteCAA { get; set; }
        public bool ExencionPago { get; set; }
        public Nullable<bool> documentacionPA { get; set; }
        public Nullable<System.DateTime> Fecha_Habilitacion { get; set; }
        public string NroExpedienteSadeRelacionado { get; set; }
        public string circuito_origen { get; set; }
        public string NroEspecialSADE { get; set; }
        public Nullable<int> idTAD { get; set; }
        public Nullable<bool> EximirCAA { get; set; }
        public Nullable<bool> EsECI { get; set; }

        [JsonIgnore]
        public virtual ICollection<Encomienda_SSIT_Solicitudes> Encomienda_SSIT_Solicitudes { get; set; }
        [JsonIgnore]
        public virtual ICollection<Eximicion_CAA> Eximicion_CAA { get; set; }
        [JsonIgnore]
        public virtual ICollection<SGI_Tramites_Tareas_HAB> SGI_Tramites_Tareas_HAB { get; set; }
        [JsonIgnore]
        public virtual ICollection<Solicitud_planoVisado> Solicitud_planoVisado { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_AvisoCaducidad> SSIT_Solicitudes_AvisoCaducidad { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Baja> SSIT_Solicitudes_Baja { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Encomienda> SSIT_Solicitudes_Encomienda { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasFisicas> SSIT_Solicitudes_Firmantes_PersonasFisicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasJuridicas> SSIT_Solicitudes_Firmantes_PersonasJuridicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_HistorialEstados> SSIT_Solicitudes_HistorialEstados { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Notificaciones> SSIT_Solicitudes_Notificaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Observaciones> SSIT_Solicitudes_Observaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Pagos> SSIT_Solicitudes_Pagos { get; set; }
        [JsonIgnore]
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        [JsonIgnore]
        public virtual TipoEstadoSolicitud TipoEstadoSolicitud { get; set; }
        [JsonIgnore]
        public virtual TipoExpediente TipoExpediente { get; set; }
        [JsonIgnore]
        public virtual TipoTramite TipoTramite { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasFisicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas> SSIT_Solicitudes_Titulares_PersonasJuridicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones> SSIT_Solicitudes_Ubicaciones { get; set; }
        [JsonIgnore]
        public virtual SSIT_Permisos_DatosAdicionales SSIT_Permisos_DatosAdicionales { get; set; }
        [JsonIgnore]
        public virtual SSIT_Solicitudes_DatosLocal SSIT_Solicitudes_DatosLocal { get; set; }
        [JsonIgnore]
        public virtual SSIT_Solicitudes_Normativas SSIT_Solicitudes_Normativas { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_RubrosCN> SSIT_Solicitudes_RubrosCN { get; set; }
        [JsonIgnore]
        public virtual SSIT_Solicitudes_Origen SSIT_Solicitudes_Origen { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Origen> SSIT_Solicitudes_Origen1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_DocumentosAdjuntos> SSIT_DocumentosAdjuntos { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_AvisoRechazo> SSIT_Solicitudes_AvisoRechazo { get; set; }
    }
}

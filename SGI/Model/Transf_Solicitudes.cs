

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Solicitudes
    {
        public Transf_Solicitudes()
        {
            this.SGI_Tramites_Tareas_TRANSF = new HashSet<SGI_Tramites_Tareas_TRANSF>();
            this.Transf_DocumentosAdjuntos = new HashSet<Transf_DocumentosAdjuntos>();
            this.Transf_Firmantes_PersonasFisicas = new HashSet<Transf_Firmantes_PersonasFisicas>();
            this.Transf_Firmantes_PersonasJuridicas = new HashSet<Transf_Firmantes_PersonasJuridicas>();
            this.Transf_Solicitudes_Baja = new HashSet<Transf_Solicitudes_Baja>();
            this.Transf_Solicitudes_HistorialEstados = new HashSet<Transf_Solicitudes_HistorialEstados>();
            this.Transf_Solicitudes_Observaciones = new HashSet<Transf_Solicitudes_Observaciones>();
            this.Transf_Titulares_PersonasFisicas = new HashSet<Transf_Titulares_PersonasFisicas>();
            this.Transf_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<Transf_Titulares_PersonasJuridicas_PersonasFisicas>();
            this.Transf_Titulares_PersonasJuridicas = new HashSet<Transf_Titulares_PersonasJuridicas>();
            this.Transf_DatosLocal = new HashSet<Transf_DatosLocal>();
            this.Transf_Firmantes_Solicitud_PersonasFisicas = new HashSet<Transf_Firmantes_Solicitud_PersonasFisicas>();
            this.Transf_Firmantes_Solicitud_PersonasJuridicas = new HashSet<Transf_Firmantes_Solicitud_PersonasJuridicas>();
            this.Transf_Normativas = new HashSet<Transf_Normativas>();
            this.Transf_Plantas = new HashSet<Transf_Plantas>();
            this.Transf_Rubros = new HashSet<Transf_Rubros>();
            this.Transf_Titulares_Solicitud_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasFisicas>();
            this.Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas>();
            this.Transf_Titulares_Solicitud_PersonasJuridicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas>();
            this.Transf_Ubicaciones = new HashSet<Transf_Ubicaciones>();
            this.Transf_ConformacionLocal = new HashSet<Transf_ConformacionLocal>();
            this.Encomienda_Transf_Solicitudes = new HashSet<Encomienda_Transf_Solicitudes>();
            this.Transf_Solicitudes_AvisoCaducidad = new HashSet<Transf_Solicitudes_AvisoCaducidad>();
            this.Transf_Solicitudes_Pagos = new HashSet<Transf_Solicitudes_Pagos>();
            this.SSIT_Solicitudes_Origen = new HashSet<SSIT_Solicitudes_Origen>();
        }
    
        public int id_solicitud { get; set; }
        public int id_cpadron { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public int id_estado { get; set; }
        public string NroExpedienteSade { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string CodigoSeguridad { get; set; }
        public Nullable<System.DateTime> Fecha_Habilitacion { get; set; }
        public Nullable<int> idTAD { get; set; }
        public string ZonaDeclarada { get; set; }
        public Nullable<int> idTipoTransmision { get; set; }
        public Nullable<int> idSolicitudRef { get; set; }
    
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas_TRANSF> SGI_Tramites_Tareas_TRANSF { get; set; }
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        public virtual TipoEstadoSolicitud TipoEstadoSolicitud { get; set; }
        public virtual TipoExpediente TipoExpediente { get; set; }
        public virtual TipoTramite TipoTramite { get; set; }
        public virtual ICollection<Transf_DocumentosAdjuntos> Transf_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Transf_Firmantes_PersonasFisicas> Transf_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Firmantes_PersonasJuridicas> Transf_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Solicitudes_Baja> Transf_Solicitudes_Baja { get; set; }
        public virtual ICollection<Transf_Solicitudes_HistorialEstados> Transf_Solicitudes_HistorialEstados { get; set; }
        public virtual ICollection<Transf_Solicitudes_Observaciones> Transf_Solicitudes_Observaciones { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasFisicas> Transf_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas_PersonasFisicas> Transf_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas> Transf_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_DatosLocal> Transf_DatosLocal { get; set; }
        public virtual ICollection<Transf_Firmantes_Solicitud_PersonasFisicas> Transf_Firmantes_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Firmantes_Solicitud_PersonasJuridicas> Transf_Firmantes_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Normativas> Transf_Normativas { get; set; }
        public virtual ICollection<Transf_Plantas> Transf_Plantas { get; set; }
        public virtual ICollection<Transf_Rubros> Transf_Rubros { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasFisicas> Transf_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas> Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas> Transf_Titulares_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Ubicaciones> Transf_Ubicaciones { get; set; }
        public virtual ICollection<Transf_ConformacionLocal> Transf_ConformacionLocal { get; set; }
        public virtual ICollection<Encomienda_Transf_Solicitudes> Encomienda_Transf_Solicitudes { get; set; }
        public virtual TiposdeTransmision TiposdeTransmision { get; set; }
        public virtual ICollection<Transf_Solicitudes_AvisoCaducidad> Transf_Solicitudes_AvisoCaducidad { get; set; }
        public virtual ICollection<Transf_Solicitudes_Pagos> Transf_Solicitudes_Pagos { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Origen> SSIT_Solicitudes_Origen { get; set; }
    }
}

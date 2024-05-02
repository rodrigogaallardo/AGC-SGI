

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class CPadron_Solicitudes
    {
        public CPadron_Solicitudes()
        {
            this.CPadron_ConformacionLocal = new HashSet<CPadron_ConformacionLocal>();
            this.CPadron_DocumentosAdjuntos = new HashSet<CPadron_DocumentosAdjuntos>();
            this.CPadron_HistorialEstados = new HashSet<CPadron_HistorialEstados>();
            this.CPadron_Normativas = new HashSet<CPadron_Normativas>();
            this.CPadron_Plantas = new HashSet<CPadron_Plantas>();
            this.CPadron_Rubros = new HashSet<CPadron_Rubros>();
            this.CPadron_Solicitudes_Observaciones = new HashSet<CPadron_Solicitudes_Observaciones>();
            this.CPadron_Titulares_PersonasFisicas = new HashSet<CPadron_Titulares_PersonasFisicas>();
            this.CPadron_Titulares_PersonasJuridicas = new HashSet<CPadron_Titulares_PersonasJuridicas>();
            this.CPadron_Titulares_Solicitud_PersonasFisicas = new HashSet<CPadron_Titulares_Solicitud_PersonasFisicas>();
            this.CPadron_Titulares_Solicitud_PersonasJuridicas = new HashSet<CPadron_Titulares_Solicitud_PersonasJuridicas>();
            this.CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas = new HashSet<CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas>();
            this.CPadron_Ubicaciones = new HashSet<CPadron_Ubicaciones>();
            this.SGI_Tramites_Tareas_CPADRON = new HashSet<SGI_Tramites_Tareas_CPADRON>();
            this.CPadron_DatosLocal = new HashSet<CPadron_DatosLocal>();
            this.Cpadron_Solicitudes_Baja = new HashSet<Cpadron_Solicitudes_Baja>();
            this.Transf_Solicitudes = new HashSet<Transf_Solicitudes>();
            this.CPadron_RubrosCN = new HashSet<CPadron_RubrosCN>();
        }
    
        public int id_cpadron { get; set; }
        public string CodigoSeguridad { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public int id_estado { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string observaciones_internas { get; set; }
        public string observaciones_contribuyente { get; set; }
        public string ZonaDeclarada { get; set; }
        public string nro_expediente_anterior { get; set; }
        public string observaciones { get; set; }
        public string nombre_apellido_escribano { get; set; }
        public Nullable<int> nro_matricula_escribano { get; set; }
        public string NroExpedienteSade { get; set; }
        public Nullable<int> idTAD { get; set; }

        [JsonIgnore]
        public virtual aspnet_Users aspnet_Users { get; set; }
        [JsonIgnore]
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_ConformacionLocal> CPadron_ConformacionLocal { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_DocumentosAdjuntos> CPadron_DocumentosAdjuntos { get; set; }
        [JsonIgnore]
        public virtual CPadron_Estados CPadron_Estados { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_HistorialEstados> CPadron_HistorialEstados { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Normativas> CPadron_Normativas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Plantas> CPadron_Plantas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Rubros> CPadron_Rubros { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Solicitudes_Observaciones> CPadron_Solicitudes_Observaciones { get; set; }
        [JsonIgnore]
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        [JsonIgnore]
        public virtual TipoExpediente TipoExpediente { get; set; }
        [JsonIgnore]
        public virtual TipoTramite TipoTramite { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Titulares_PersonasFisicas> CPadron_Titulares_PersonasFisicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Titulares_PersonasJuridicas> CPadron_Titulares_PersonasJuridicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasFisicas> CPadron_Titulares_Solicitud_PersonasFisicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasJuridicas> CPadron_Titulares_Solicitud_PersonasJuridicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas> CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Ubicaciones> CPadron_Ubicaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<SGI_Tramites_Tareas_CPADRON> SGI_Tramites_Tareas_CPADRON { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_DatosLocal> CPadron_DatosLocal { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cpadron_Solicitudes_Baja> Cpadron_Solicitudes_Baja { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transf_Solicitudes> Transf_Solicitudes { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_RubrosCN> CPadron_RubrosCN { get; set; }
    }
}

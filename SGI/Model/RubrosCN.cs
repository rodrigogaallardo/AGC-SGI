

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class RubrosCN
    {
        public RubrosCN()
        {
            this.Encomienda_RubrosCN = new HashSet<Encomienda_RubrosCN>();
            this.Encomienda_RubrosCN_AT_Anterior = new HashSet<Encomienda_RubrosCN_AT_Anterior>();
            this.Encomienda_RubrosCN_Deposito = new HashSet<Encomienda_RubrosCN_Deposito>();
            this.RubrosCN_Config_Incendio = new HashSet<RubrosCN_Config_Incendio>();
            this.RubrosCN_InformacionRelevante = new HashSet<RubrosCN_InformacionRelevante>();
            this.RubrosCN_Subrubros = new HashSet<RubrosCN_Subrubros>();
            this.RubrosCN_TiposDeDocumentosRequeridos = new HashSet<RubrosCN_TiposDeDocumentosRequeridos>();
            this.SSIT_Solicitudes_RubrosCN = new HashSet<SSIT_Solicitudes_RubrosCN>();
            this.CPadron_RubrosCN = new HashSet<CPadron_RubrosCN>();
        }
    
        public int IdRubro { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Keywords { get; set; }
        public Nullable<System.DateTime> VigenciaDesde_rubro { get; set; }
        public Nullable<System.DateTime> VigenciaHasta_rubro { get; set; }
        public int IdTipoActividad { get; set; }
        public int IdTipoExpediente { get; set; }
        public Nullable<int> IdGrupoCircuito { get; set; }
        public bool LibrarUso { get; set; }
        public string ZonaMixtura1 { get; set; }
        public string ZonaMixtura2 { get; set; }
        public string ZonaMixtura3 { get; set; }
        public string ZonaMixtura4 { get; set; }
        public Nullable<int> IdEstacionamiento { get; set; }
        public Nullable<int> IdBicicleta { get; set; }
        public Nullable<int> IdCyD { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<bool> Asistentes350 { get; set; }
        public Nullable<bool> TieneRubroDeposito { get; set; }
        public Nullable<int> idCondicionIncendio { get; set; }
        public bool SoloAPRA { get; set; }
        public Nullable<bool> SinBanioPCD { get; set; }
        public bool CondicionExpress { get; set; }

        [JsonIgnore]
        public virtual CondicionesIncendio CondicionesIncendio { get; set; }
        [JsonIgnore]
        public virtual ICollection<Encomienda_RubrosCN> Encomienda_RubrosCN { get; set; }
        [JsonIgnore]
        public virtual ICollection<Encomienda_RubrosCN_AT_Anterior> Encomienda_RubrosCN_AT_Anterior { get; set; }
        [JsonIgnore]
        public virtual ICollection<Encomienda_RubrosCN_Deposito> Encomienda_RubrosCN_Deposito { get; set; }
        [JsonIgnore]
        public virtual ENG_Grupos_Circuitos ENG_Grupos_Circuitos { get; set; }
        [JsonIgnore]
        public virtual RubrosBicicletas RubrosBicicletas { get; set; }
        [JsonIgnore]
        public virtual RubrosCargasyDescargas RubrosCargasyDescargas { get; set; }
        [JsonIgnore]
        public virtual ICollection<RubrosCN_Config_Incendio> RubrosCN_Config_Incendio { get; set; }
        [JsonIgnore]
        public virtual ICollection<RubrosCN_InformacionRelevante> RubrosCN_InformacionRelevante { get; set; }
        [JsonIgnore]
        public virtual RubrosEstacionamientos RubrosEstacionamientos { get; set; }
        [JsonIgnore]
        public virtual ICollection<RubrosCN_Subrubros> RubrosCN_Subrubros { get; set; }
        [JsonIgnore]
        public virtual TipoActividad TipoActividad { get; set; }
        [JsonIgnore]
        public virtual TipoExpediente TipoExpediente { get; set; }
        [JsonIgnore]
        public virtual ICollection<RubrosCN_TiposDeDocumentosRequeridos> RubrosCN_TiposDeDocumentosRequeridos { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_RubrosCN> SSIT_Solicitudes_RubrosCN { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_RubrosCN> CPadron_RubrosCN { get; set; }
    }
}

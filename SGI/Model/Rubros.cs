

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros
    {
        public Rubros()
        {
            this.Parametros_Bandeja_Rubro = new HashSet<Parametros_Bandeja_Rubro>();
            this.Rubros_CircuitoAtomatico_Zonas = new HashSet<Rubros_CircuitoAtomatico_Zonas>();
            this.Rubros_InformacionRelevante = new HashSet<Rubros_InformacionRelevante>();
            this.Rubros_TiposDeDocumentosRequeridos = new HashSet<Rubros_TiposDeDocumentosRequeridos>();
            this.Rubros_TiposDeDocumentosRequeridos_Zonas = new HashSet<Rubros_TiposDeDocumentosRequeridos_Zonas>();
            this.Rubros_Config_Incendio = new HashSet<Rubros_Config_Incendio>();
            this.RubrosCN_InformacionRelevante = new HashSet<RubrosCN_InformacionRelevante>();
        }
    
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public string bus_rubro { get; set; }
        public int id_tipoactividad { get; set; }
        public int id_tipodocreq { get; set; }
        public bool EsAnterior_Rubro { get; set; }
        public Nullable<System.DateTime> VigenciaDesde_rubro { get; set; }
        public Nullable<System.DateTime> VigenciaHasta_rubro { get; set; }
        public bool PregAntenaEmisora { get; set; }
        public bool SoloAPRA { get; set; }
        public string tooltip_rubro { get; set; }
        public Nullable<double> local_venta { get; set; }
        public Nullable<bool> ley105 { get; set; }
        public bool Circuito_Automatico { get; set; }
        public bool Uso_Condicionado { get; set; }
        public bool TieneDeposito { get; set; }
        public Nullable<decimal> SupMinCargaDescarga { get; set; }
        public Nullable<decimal> SupMinCargaDescargaRefII { get; set; }
        public Nullable<decimal> SupMinCargaDescargaRefV { get; set; }
        public bool OficinaComercial { get; set; }
        public bool EsRubroIndividual { get; set; }
        public bool EsProTeatro { get; set; }
        public bool EsEstadio { get; set; }
        public bool EsCentroCultural { get; set; }
        public bool ValidaCargaDescarga { get; set; }
        public bool RequiereVisado { get; set; }
        public Nullable<int> id_licencia_alcohol { get; set; }
        public Nullable<int> id_grupo_circuito { get; set; }
        public bool Librar_Uso { get; set; }
        public Nullable<int> id_clanae { get; set; }
    
        public virtual ENG_Grupos_Circuitos ENG_Grupos_Circuitos { get; set; }
        public virtual ICollection<Parametros_Bandeja_Rubro> Parametros_Bandeja_Rubro { get; set; }
        public virtual ICollection<Rubros_CircuitoAtomatico_Zonas> Rubros_CircuitoAtomatico_Zonas { get; set; }
        public virtual ICollection<Rubros_InformacionRelevante> Rubros_InformacionRelevante { get; set; }
        public virtual RAL_Licencias RAL_Licencias { get; set; }
        public virtual ICollection<Rubros_TiposDeDocumentosRequeridos> Rubros_TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<Rubros_TiposDeDocumentosRequeridos_Zonas> Rubros_TiposDeDocumentosRequeridos_Zonas { get; set; }
        public virtual ICollection<Rubros_Config_Incendio> Rubros_Config_Incendio { get; set; }
        public virtual Clanae Clanae { get; set; }
        public virtual ICollection<RubrosCN_InformacionRelevante> RubrosCN_InformacionRelevante { get; set; }
    }
}

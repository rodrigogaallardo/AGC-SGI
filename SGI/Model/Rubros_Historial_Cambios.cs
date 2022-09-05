

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Historial_Cambios
    {
        public Rubros_Historial_Cambios()
        {
            this.Rubros_CircuitoAtomatico_Zonas_Historial_Cambios = new HashSet<Rubros_CircuitoAtomatico_Zonas_Historial_Cambios>();
            this.Rubros_Config_Incendio_Historial_Cambios = new HashSet<Rubros_Config_Incendio_Historial_Cambios>();
            this.Rubros_Historial_Cambios_Estados = new HashSet<Rubros_Historial_Cambios_Estados>();
            this.Rubros_InformacionRelevante_Historial_Cambios = new HashSet<Rubros_InformacionRelevante_Historial_Cambios>();
            this.Rubros_TiposDeDocumentosRequeridos_Historial_Cambios = new HashSet<Rubros_TiposDeDocumentosRequeridos_Historial_Cambios>();
            this.Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios = new HashSet<Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios>();
        }
    
        public int id_rubhistcam { get; set; }
        public int tipo_solicitud_rubhistcam { get; set; }
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
        public int id_estado_modif { get; set; }
        public string observaciones_rubhistcam { get; set; }
        public bool Circuito_Automatico { get; set; }
        public bool Uso_Condicionado { get; set; }
        public bool TieneDeposito { get; set; }
        public Nullable<decimal> SupMinCargaDescarga { get; set; }
        public Nullable<decimal> SupMinCargaDescargaRefII { get; set; }
        public Nullable<decimal> SupMinCargaDescargaRefV { get; set; }
        public bool OficinaComercial { get; set; }
        public bool ValidaCargaDescarga { get; set; }
        public Nullable<int> id_licencia_alcohol { get; set; }
        public Nullable<int> id_grupo_circuito { get; set; }
        public bool Librar_Uso { get; set; }
        public Nullable<int> id_clanae { get; set; }
        public Nullable<double> local_venta { get; set; }
    
        public virtual ICollection<Rubros_CircuitoAtomatico_Zonas_Historial_Cambios> Rubros_CircuitoAtomatico_Zonas_Historial_Cambios { get; set; }
        public virtual ICollection<Rubros_Config_Incendio_Historial_Cambios> Rubros_Config_Incendio_Historial_Cambios { get; set; }
        public virtual ICollection<Rubros_Historial_Cambios_Estados> Rubros_Historial_Cambios_Estados { get; set; }
        public virtual Tipo_Documentacion_Req Tipo_Documentacion_Req { get; set; }
        public virtual TipoActividad TipoActividad { get; set; }
        public virtual ICollection<Rubros_InformacionRelevante_Historial_Cambios> Rubros_InformacionRelevante_Historial_Cambios { get; set; }
        public virtual ICollection<Rubros_TiposDeDocumentosRequeridos_Historial_Cambios> Rubros_TiposDeDocumentosRequeridos_Historial_Cambios { get; set; }
        public virtual ICollection<Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios> Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios { get; set; }
    }
}

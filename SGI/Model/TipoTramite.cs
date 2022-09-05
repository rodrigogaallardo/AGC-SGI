

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoTramite
    {
        public TipoTramite()
        {
            this.ENG_Rel_Circuitos_TiposDeTramite = new HashSet<ENG_Rel_Circuitos_TiposDeTramite>();
            this.Rel_TipoTramite_TipoExpediente = new HashSet<Rel_TipoTramite_TipoExpediente>();
            this.Rel_TipoTramite_TiposDeDocumentosRequeridos = new HashSet<Rel_TipoTramite_TiposDeDocumentosRequeridos>();
            this.SSIT_Solicitudes_HistorialUsuarios = new HashSet<SSIT_Solicitudes_HistorialUsuarios>();
            this.Solicitud = new HashSet<Solicitud>();
            this.CPadron_Solicitudes = new HashSet<CPadron_Solicitudes>();
            this.SSIT_Solicitudes_Nuevas = new HashSet<SSIT_Solicitudes_Nuevas>();
            this.Transf_Solicitudes = new HashSet<Transf_Solicitudes>();
            this.Encomienda = new HashSet<Encomienda>();
            this.SSIT_Solicitudes = new HashSet<SSIT_Solicitudes>();
        }
    
        public int id_tipotramite { get; set; }
        public string cod_tipotramite { get; set; }
        public string descripcion_tipotramite { get; set; }
        public string cod_tipotramite_ws { get; set; }
        public Nullable<bool> habilitado_ssit { get; set; }
        public Nullable<int> orden { get; set; }
    
        public virtual ICollection<ENG_Rel_Circuitos_TiposDeTramite> ENG_Rel_Circuitos_TiposDeTramite { get; set; }
        public virtual ICollection<Rel_TipoTramite_TipoExpediente> Rel_TipoTramite_TipoExpediente { get; set; }
        public virtual ICollection<Rel_TipoTramite_TiposDeDocumentosRequeridos> Rel_TipoTramite_TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<SSIT_Solicitudes_HistorialUsuarios> SSIT_Solicitudes_HistorialUsuarios { get; set; }
        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<CPadron_Solicitudes> CPadron_Solicitudes { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Nuevas> SSIT_Solicitudes_Nuevas { get; set; }
        public virtual ICollection<Transf_Solicitudes> Transf_Solicitudes { get; set; }
        public virtual ICollection<Encomienda> Encomienda { get; set; }
        public virtual ICollection<SSIT_Solicitudes> SSIT_Solicitudes { get; set; }
    }
}

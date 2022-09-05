

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoExpediente
    {
        public TipoExpediente()
        {
            this.ENG_Rel_Circuitos_TiposDeTramite = new HashSet<ENG_Rel_Circuitos_TiposDeTramite>();
            this.Rel_TipoExpediente_SubtipoExpediente = new HashSet<Rel_TipoExpediente_SubtipoExpediente>();
            this.Rel_TipoTramite_TipoExpediente = new HashSet<Rel_TipoTramite_TipoExpediente>();
            this.Solicitud = new HashSet<Solicitud>();
            this.CPadron_Solicitudes = new HashSet<CPadron_Solicitudes>();
            this.Encomienda_RubrosCN = new HashSet<Encomienda_RubrosCN>();
            this.Encomienda_RubrosCN_AT_Anterior = new HashSet<Encomienda_RubrosCN_AT_Anterior>();
            this.Transf_Solicitudes = new HashSet<Transf_Solicitudes>();
            this.Encomienda = new HashSet<Encomienda>();
            this.ENG_Grupos_Circuitos = new HashSet<ENG_Grupos_Circuitos>();
            this.ENG_Grupos_Circuitos_Tipo_Tramite = new HashSet<ENG_Grupos_Circuitos_Tipo_Tramite>();
            this.SSIT_Solicitudes = new HashSet<SSIT_Solicitudes>();
            this.RubrosCN = new HashSet<RubrosCN>();
        }
    
        public int id_tipoexpediente { get; set; }
        public string cod_tipoexpediente { get; set; }
        public string descripcion_tipoexpediente { get; set; }
        public string cod_tipoexpediente_ws { get; set; }
    
        public virtual ICollection<ENG_Rel_Circuitos_TiposDeTramite> ENG_Rel_Circuitos_TiposDeTramite { get; set; }
        public virtual ICollection<Rel_TipoExpediente_SubtipoExpediente> Rel_TipoExpediente_SubtipoExpediente { get; set; }
        public virtual ICollection<Rel_TipoTramite_TipoExpediente> Rel_TipoTramite_TipoExpediente { get; set; }
        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<CPadron_Solicitudes> CPadron_Solicitudes { get; set; }
        public virtual ICollection<Encomienda_RubrosCN> Encomienda_RubrosCN { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_AT_Anterior> Encomienda_RubrosCN_AT_Anterior { get; set; }
        public virtual ICollection<Transf_Solicitudes> Transf_Solicitudes { get; set; }
        public virtual ICollection<Encomienda> Encomienda { get; set; }
        public virtual ICollection<ENG_Grupos_Circuitos> ENG_Grupos_Circuitos { get; set; }
        public virtual ICollection<ENG_Grupos_Circuitos_Tipo_Tramite> ENG_Grupos_Circuitos_Tipo_Tramite { get; set; }
        public virtual ICollection<SSIT_Solicitudes> SSIT_Solicitudes { get; set; }
        public virtual ICollection<RubrosCN> RubrosCN { get; set; }
    }
}

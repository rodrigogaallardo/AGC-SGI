

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubtipoExpediente
    {
        public SubtipoExpediente()
        {
            this.ENG_Rel_Circuitos_TiposDeTramite = new HashSet<ENG_Rel_Circuitos_TiposDeTramite>();
            this.Rel_TipoExpediente_SubtipoExpediente = new HashSet<Rel_TipoExpediente_SubtipoExpediente>();
            this.Solicitud = new HashSet<Solicitud>();
            this.CPadron_Solicitudes = new HashSet<CPadron_Solicitudes>();
            this.Transf_Solicitudes = new HashSet<Transf_Solicitudes>();
            this.Encomienda = new HashSet<Encomienda>();
            this.ENG_Grupos_Circuitos = new HashSet<ENG_Grupos_Circuitos>();
            this.ENG_Grupos_Circuitos_Tipo_Tramite = new HashSet<ENG_Grupos_Circuitos_Tipo_Tramite>();
            this.SSIT_Solicitudes = new HashSet<SSIT_Solicitudes>();
        }
    
        public int id_subtipoexpediente { get; set; }
        public string cod_subtipoexpediente { get; set; }
        public string descripcion_subtipoexpediente { get; set; }
        public string cod_subtipoexpediente_ws { get; set; }
    
        public virtual ICollection<ENG_Rel_Circuitos_TiposDeTramite> ENG_Rel_Circuitos_TiposDeTramite { get; set; }
        public virtual ICollection<Rel_TipoExpediente_SubtipoExpediente> Rel_TipoExpediente_SubtipoExpediente { get; set; }
        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<CPadron_Solicitudes> CPadron_Solicitudes { get; set; }
        public virtual ICollection<Transf_Solicitudes> Transf_Solicitudes { get; set; }
        public virtual ICollection<Encomienda> Encomienda { get; set; }
        public virtual ICollection<ENG_Grupos_Circuitos> ENG_Grupos_Circuitos { get; set; }
        public virtual ICollection<ENG_Grupos_Circuitos_Tipo_Tramite> ENG_Grupos_Circuitos_Tipo_Tramite { get; set; }
        public virtual ICollection<SSIT_Solicitudes> SSIT_Solicitudes { get; set; }
    }
}

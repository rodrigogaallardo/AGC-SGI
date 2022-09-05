

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoNormativa
    {
        public TipoNormativa()
        {
            this.CPadron_Normativas = new HashSet<CPadron_Normativas>();
            this.Encomienda_Normativas = new HashSet<Encomienda_Normativas>();
            this.Solicitud = new HashSet<Solicitud>();
            this.Transf_Normativas = new HashSet<Transf_Normativas>();
            this.SSIT_Solicitudes_Normativas = new HashSet<SSIT_Solicitudes_Normativas>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<CPadron_Normativas> CPadron_Normativas { get; set; }
        public virtual ICollection<Encomienda_Normativas> Encomienda_Normativas { get; set; }
        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<Transf_Normativas> Transf_Normativas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Normativas> SSIT_Solicitudes_Normativas { get; set; }
    }
}

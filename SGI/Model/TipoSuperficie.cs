

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoSuperficie
    {
        public TipoSuperficie()
        {
            this.CPadron_ConformacionLocal = new HashSet<CPadron_ConformacionLocal>();
            this.Encomienda_ConformacionLocal = new HashSet<Encomienda_ConformacionLocal>();
            this.Transf_ConformacionLocal = new HashSet<Transf_ConformacionLocal>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<CPadron_ConformacionLocal> CPadron_ConformacionLocal { get; set; }
        public virtual ICollection<Encomienda_ConformacionLocal> Encomienda_ConformacionLocal { get; set; }
        public virtual ICollection<Transf_ConformacionLocal> Transf_ConformacionLocal { get; set; }
    }
}

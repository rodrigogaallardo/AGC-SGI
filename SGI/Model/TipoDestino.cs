

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoDestino
    {
        public TipoDestino()
        {
            this.CPadron_ConformacionLocal = new HashSet<CPadron_ConformacionLocal>();
            this.Encomienda_ConformacionLocal = new HashSet<Encomienda_ConformacionLocal>();
            this.Transf_ConformacionLocal = new HashSet<Transf_ConformacionLocal>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> RequiereObservaciones { get; set; }
        public bool RequiereDetalle { get; set; }
    
        public virtual ICollection<CPadron_ConformacionLocal> CPadron_ConformacionLocal { get; set; }
        public virtual ICollection<Encomienda_ConformacionLocal> Encomienda_ConformacionLocal { get; set; }
        public virtual ICollection<Transf_ConformacionLocal> Transf_ConformacionLocal { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoPersona
    {
        public TipoPersona()
        {
            this.PersonasInhibidas = new HashSet<PersonasInhibidas>();
        }
    
        public int Id_TipoPersona { get; set; }
        public string Nombre { get; set; }
    
        public virtual ICollection<PersonasInhibidas> PersonasInhibidas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Provincia
    {
        public Provincia()
        {
            this.Localidad = new HashSet<Localidad>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
    
        public virtual ICollection<Localidad> Localidad { get; set; }
    }
}

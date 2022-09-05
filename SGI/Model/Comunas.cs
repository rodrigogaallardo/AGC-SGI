

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comunas
    {
        public Comunas()
        {
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public int id_comuna { get; set; }
        public string nom_comuna { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}

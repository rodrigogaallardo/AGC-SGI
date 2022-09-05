

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Estados
    {
        public Encomienda_Estados()
        {
            this.Encomienda = new HashSet<Encomienda>();
        }
    
        public int id_estado { get; set; }
        public string cod_estado { get; set; }
        public string nom_estado { get; set; }
        public string nom_estado_consejo { get; set; }
    
        public virtual ICollection<Encomienda> Encomienda { get; set; }
    }
}

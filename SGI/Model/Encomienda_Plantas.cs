

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Plantas
    {
        public Encomienda_Plantas()
        {
            this.Encomienda_ConformacionLocal = new HashSet<Encomienda_ConformacionLocal>();
        }
    
        public int id_encomiendatiposector { get; set; }
        public int id_encomienda { get; set; }
        public int id_tiposector { get; set; }
        public string detalle_encomiendatiposector { get; set; }
    
        public virtual ICollection<Encomienda_ConformacionLocal> Encomienda_ConformacionLocal { get; set; }
        public virtual TipoSector TipoSector { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}

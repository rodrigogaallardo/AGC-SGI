

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposdeTransmision
    {
        public TiposdeTransmision()
        {
            this.Transf_Solicitudes = new HashSet<Transf_Solicitudes>();
        }
    
        public int id_tipoTransmision { get; set; }
        public string nom_tipotransmision { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<Transf_Solicitudes> Transf_Solicitudes { get; set; }
    }
}

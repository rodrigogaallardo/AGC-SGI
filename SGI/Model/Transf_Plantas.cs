

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Plantas
    {
        public Transf_Plantas()
        {
            this.Transf_ConformacionLocal = new HashSet<Transf_ConformacionLocal>();
        }
    
        public int id_transftiposector { get; set; }
        public int id_solicitud { get; set; }
        public int id_tiposector { get; set; }
        public string detalle_transftiposector { get; set; }
    
        public virtual TipoSector TipoSector { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
        public virtual ICollection<Transf_ConformacionLocal> Transf_ConformacionLocal { get; set; }
    }
}

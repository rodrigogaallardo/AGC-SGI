

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Plantas
    {
        public CPadron_Plantas()
        {
            this.CPadron_ConformacionLocal = new HashSet<CPadron_ConformacionLocal>();
        }
    
        public int id_cpadrontiposector { get; set; }
        public int id_cpadron { get; set; }
        public int id_tiposector { get; set; }
        public string detalle_cpadrontiposector { get; set; }
    
        public virtual ICollection<CPadron_ConformacionLocal> CPadron_ConformacionLocal { get; set; }
        public virtual TipoSector TipoSector { get; set; }
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
    }
}

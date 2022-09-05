

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Solicitudes_Observaciones
    {
        public int id_cpadron_observacion { get; set; }
        public int id_cpadron { get; set; }
        public string observaciones { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<bool> leido { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
    }
}

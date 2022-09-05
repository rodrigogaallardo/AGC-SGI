

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cpadron_Solicitudes_Baja
    {
        public int id_baja { get; set; }
        public int id_cpadron { get; set; }
        public int id_tipo_motivo_baja { get; set; }
        public System.DateTime fecha_baja { get; set; }
        public string observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
        public virtual TiposMotivoBaja TiposMotivoBaja { get; set; }
    }
}

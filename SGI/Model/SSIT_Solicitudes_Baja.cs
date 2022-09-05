

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Baja
    {
        public int id_baja { get; set; }
        public int id_solicitud { get; set; }
        public int id_tipo_motivo_baja { get; set; }
        public System.DateTime fecha_baja { get; set; }
        public string observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual TiposMotivoBaja TiposMotivoBaja { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

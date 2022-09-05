

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_HistorialEstados
    {
        public int id_cpadron_his { get; set; }
        public int id_cpadron { get; set; }
        public string cod_estado_ant { get; set; }
        public string cod_estado_nuevo { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public System.Guid usuario_modificacion { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
    }
}

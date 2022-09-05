

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_HistorialEstados
    {
        public int id_enchistest { get; set; }
        public int id_encomienda { get; set; }
        public string cod_estado_ant { get; set; }
        public string cod_estado_nuevo { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public System.Guid usuario_modificacion { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
    }
}

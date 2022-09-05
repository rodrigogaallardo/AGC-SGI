

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Historial_Cambios_Estados
    {
        public int id_rubhistcamest { get; set; }
        public int id_rubhistcam { get; set; }
        public int id_estado_ant { get; set; }
        public int id_estado_nuevo { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public System.Guid usuario_modificacion { get; set; }
    
        public virtual Rubros_Historial_Cambios Rubros_Historial_Cambios { get; set; }
    }
}

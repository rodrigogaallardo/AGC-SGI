

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Historial_Cambios_Estados
    {
        public int id_ubihistcamest { get; set; }
        public int id_ubihistcam { get; set; }
        public int id_estado_ant { get; set; }
        public int id_estado_nuevo { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public System.Guid usuario_modificacion { get; set; }
    
        public virtual Ubicaciones_Historial_Cambios Ubicaciones_Historial_Cambios { get; set; }
    }
}

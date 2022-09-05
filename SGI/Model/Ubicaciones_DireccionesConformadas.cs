

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_DireccionesConformadas
    {
        public int id_direccion { get; set; }
        public int id_ubicacion { get; set; }
        public string direccion { get; set; }
        public System.DateTime fecha_actualizacion { get; set; }
    
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

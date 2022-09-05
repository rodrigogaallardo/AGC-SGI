

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Operaciones_Detalle
    {
        public int id_operacion_det { get; set; }
        public int id_operacion { get; set; }
        public Nullable<int> id_ubicacion { get; set; }
        public Nullable<int> id_ubicacion_temp { get; set; }
        public string Detalle { get; set; }
    
        public virtual Ubicaciones_Operaciones Ubicaciones_Operaciones { get; set; }
    }
}

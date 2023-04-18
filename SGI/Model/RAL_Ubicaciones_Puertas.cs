

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RAL_Ubicaciones_Puertas
    {
        public int id_ral_puerta { get; set; }
        public int id_ral_ubicacion { get; set; }
        public Nullable<int> codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public Nullable<int> nro_puerta { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}

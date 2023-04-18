

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DGFYCO_Ubicaciones_Puertas
    {
        public int id_dgpuerta { get; set; }
        public int id_dgubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string Nombre_calle { get; set; }
        public int NroPuerta { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}

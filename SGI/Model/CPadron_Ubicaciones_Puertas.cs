

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Ubicaciones_Puertas
    {
        public int id_cpadronpuerta { get; set; }
        public int id_cpadronubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public int NroPuerta { get; set; }
    
        public virtual CPadron_Ubicaciones CPadron_Ubicaciones { get; set; }
    }
}

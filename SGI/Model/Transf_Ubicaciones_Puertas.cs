

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Ubicaciones_Puertas
    {
        public int id_transfpuerta { get; set; }
        public int id_transfubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public int NroPuerta { get; set; }
    
        public virtual Transf_Ubicaciones Transf_Ubicaciones { get; set; }
    }
}

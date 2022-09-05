

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Puertas
    {
        public int id_ubic_puerta { get; set; }
        public int id_ubicacion { get; set; }
        public string tipo_puerta { get; set; }
        public int codigo_calle { get; set; }
        public int NroPuerta_ubic { get; set; }
    
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

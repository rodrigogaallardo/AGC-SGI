

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Puertas_temp
    {
        public int id_ubic_puerta_temp { get; set; }
        public int id_ubicacion_temp { get; set; }
        public string tipo_puerta { get; set; }
        public int codigo_calle { get; set; }
        public int NroPuerta_ubic { get; set; }
    
        public virtual Ubicaciones_temp Ubicaciones_temp { get; set; }
    }
}

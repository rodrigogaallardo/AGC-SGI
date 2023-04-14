

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AVH_Ubicaciones_Puertas
    {
        public int id_avh_puerta { get; set; }
        public int id_avh_ubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public int NroPuerta { get; set; }
    }
}

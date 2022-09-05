

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Ubicaciones_Puertas
    {
        public int id_encomiendapuerta { get; set; }
        public int id_encomiendaubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public int NroPuerta { get; set; }
    
        public virtual Encomienda_Ubicaciones Encomienda_Ubicaciones { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Ubicaciones_Mixturas
    {
        public int id_encomiendaubicacionmixtura { get; set; }
        public int id_encomiendaubicacion { get; set; }
        public int IdZonaMixtura { get; set; }
    
        public virtual Encomienda_Ubicaciones Encomienda_Ubicaciones { get; set; }
        public virtual Ubicaciones_ZonasMixtura Ubicaciones_ZonasMixtura { get; set; }
    }
}

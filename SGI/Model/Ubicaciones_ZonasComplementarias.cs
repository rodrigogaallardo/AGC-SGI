

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_ZonasComplementarias
    {
        public int id_ubic_zonas { get; set; }
        public int id_ubicacion { get; set; }
        public string tipo_ubicacion { get; set; }
        public int id_zonaplaneamiento { get; set; }
    
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_ZonasComplementarias_Historial_Cambios
    {
        public int id_ubic_zonas_histcam { get; set; }
        public int id_ubihistcam { get; set; }
        public int id_ubicacion { get; set; }
        public string tipo_ubicacion { get; set; }
        public int id_zonaplaneamiento { get; set; }
    
        public virtual Ubicaciones_Historial_Cambios Ubicaciones_Historial_Cambios { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
    }
}

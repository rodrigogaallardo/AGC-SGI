

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Puertas_Historial_Cambios
    {
        public int id_ubipuehistcam { get; set; }
        public int id_ubihistcam { get; set; }
        public int id_ubicacion { get; set; }
        public string tipo_puerta { get; set; }
        public int codigo_calle { get; set; }
        public int NroPuerta_ubic { get; set; }
    
        public virtual Ubicaciones_Historial_Cambios Ubicaciones_Historial_Cambios { get; set; }
    }
}

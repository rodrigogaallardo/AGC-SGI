

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Ubicaciones_Mixturas
    {
        public int id_solicitudubicacionmixtura { get; set; }
        public int id_solicitudubicacion { get; set; }
        public int IdZonaMixtura { get; set; }
    
        public virtual SSIT_Solicitudes_Ubicaciones SSIT_Solicitudes_Ubicaciones { get; set; }
        public virtual Ubicaciones_ZonasMixtura Ubicaciones_ZonasMixtura { get; set; }
    }
}

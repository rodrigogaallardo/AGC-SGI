

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Ubicaciones_Puertas
    {
        public int id_solicitudpuerta { get; set; }
        public int id_solicitudubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public int NroPuerta { get; set; }
    
        public virtual SSIT_Solicitudes_Ubicaciones SSIT_Solicitudes_Ubicaciones { get; set; }
    }
}

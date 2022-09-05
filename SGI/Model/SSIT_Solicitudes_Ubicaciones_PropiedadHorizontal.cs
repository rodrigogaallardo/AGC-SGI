

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal
    {
        public int id_solicitudprophorizontal { get; set; }
        public Nullable<int> id_solicitudubicacion { get; set; }
        public Nullable<int> id_propiedadhorizontal { get; set; }
    
        public virtual SSIT_Solicitudes_Ubicaciones SSIT_Solicitudes_Ubicaciones { get; set; }
        public virtual Ubicaciones_PropiedadHorizontal Ubicaciones_PropiedadHorizontal { get; set; }
    }
}

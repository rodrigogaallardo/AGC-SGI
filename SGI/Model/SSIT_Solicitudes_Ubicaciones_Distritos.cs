

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Ubicaciones_Distritos
    {
        public int id_solicitudubicaciondistrito { get; set; }
        public int id_solicitudubicacion { get; set; }
        public int IdDistrito { get; set; }
        public Nullable<int> IdZona { get; set; }
        public Nullable<int> IdSubZona { get; set; }
    
        public virtual SSIT_Solicitudes_Ubicaciones SSIT_Solicitudes_Ubicaciones { get; set; }
        public virtual Ubicaciones_CatalogoDistritos Ubicaciones_CatalogoDistritos { get; set; }
    }
}

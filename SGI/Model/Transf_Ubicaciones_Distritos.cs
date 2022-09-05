

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Ubicaciones_Distritos
    {
        public int id_transfubicaciondistrito { get; set; }
        public int id_transfubicacion { get; set; }
        public int IdDistrito { get; set; }
        public Nullable<int> IdZona { get; set; }
        public Nullable<int> IdSubZona { get; set; }
    
        public virtual Transf_Ubicaciones Transf_Ubicaciones { get; set; }
        public virtual Ubicaciones_CatalogoDistritos Ubicaciones_CatalogoDistritos { get; set; }
    }
}

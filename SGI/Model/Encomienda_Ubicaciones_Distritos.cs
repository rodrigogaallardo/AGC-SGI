

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Ubicaciones_Distritos
    {
        public int id_encomiendaubicaciondistrito { get; set; }
        public int id_encomiendaubicacion { get; set; }
        public int IdDistrito { get; set; }
        public Nullable<int> IdZona { get; set; }
        public Nullable<int> IdSubZona { get; set; }
    
        public virtual Encomienda_Ubicaciones Encomienda_Ubicaciones { get; set; }
        public virtual Ubicaciones_CatalogoDistritos Ubicaciones_CatalogoDistritos { get; set; }
    }
}

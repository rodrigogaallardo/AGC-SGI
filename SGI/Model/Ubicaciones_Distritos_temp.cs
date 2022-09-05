

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Distritos_temp
    {
        public int id_ubicacion_temp { get; set; }
        public int IdDistrito { get; set; }
        public Nullable<int> IdZona { get; set; }
        public Nullable<int> IdSubZona { get; set; }
        public int Id { get; set; }
    
        public virtual Ubicaciones_CatalogoDistritos Ubicaciones_CatalogoDistritos { get; set; }
        public virtual Ubicaciones_temp Ubicaciones_temp { get; set; }
    }
}

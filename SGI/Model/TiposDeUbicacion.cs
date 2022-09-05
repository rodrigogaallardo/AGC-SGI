

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDeUbicacion
    {
        public TiposDeUbicacion()
        {
            this.SubTiposDeUbicacion = new HashSet<SubTiposDeUbicacion>();
        }
    
        public int id_tipoubicacion { get; set; }
        public string descripcion_tipoubicacion { get; set; }
        public Nullable<bool> RequiereSMP { get; set; }
    
        public virtual ICollection<SubTiposDeUbicacion> SubTiposDeUbicacion { get; set; }
    }
}

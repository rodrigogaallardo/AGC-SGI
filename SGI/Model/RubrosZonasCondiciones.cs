

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosZonasCondiciones
    {
        public string cod_rubro { get; set; }
        public string cod_ZonaHab { get; set; }
        public string cod_condicion { get; set; }
    
        public virtual RubrosCondiciones RubrosCondiciones { get; set; }
        public virtual Zonas_Habilitaciones Zonas_Habilitaciones { get; set; }
    }
}

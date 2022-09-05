

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parametros_Bandeja_Rubro
    {
        public int id_param { get; set; }
        public int id_rubro { get; set; }
        public string Revisa { get; set; }
    
        public virtual Rubros Rubros { get; set; }
    }
}

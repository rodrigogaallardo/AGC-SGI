

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_InformacionRelevante
    {
        public int id_rubinf { get; set; }
        public int id_rubro { get; set; }
        public string descripcion_rubinf { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual Rubros Rubros { get; set; }
    }
}

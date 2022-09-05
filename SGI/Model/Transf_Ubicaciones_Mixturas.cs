

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Ubicaciones_Mixturas
    {
        public int id_transfubicacionmixtura { get; set; }
        public int id_transfubicacion { get; set; }
        public int IdZonaMixtura { get; set; }
    
        public virtual Transf_Ubicaciones Transf_Ubicaciones { get; set; }
        public virtual Ubicaciones_ZonasMixtura Ubicaciones_ZonasMixtura { get; set; }
    }
}

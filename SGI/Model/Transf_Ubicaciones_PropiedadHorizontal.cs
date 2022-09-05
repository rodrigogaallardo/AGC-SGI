

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Ubicaciones_PropiedadHorizontal
    {
        public int id_transfprophorizontal { get; set; }
        public Nullable<int> id_transfubicacion { get; set; }
        public Nullable<int> id_propiedadhorizontal { get; set; }
    
        public virtual Transf_Ubicaciones Transf_Ubicaciones { get; set; }
        public virtual Ubicaciones_PropiedadHorizontal Ubicaciones_PropiedadHorizontal { get; set; }
    }
}

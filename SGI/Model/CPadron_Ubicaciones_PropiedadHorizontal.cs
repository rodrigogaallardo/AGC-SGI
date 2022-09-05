

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Ubicaciones_PropiedadHorizontal
    {
        public int id_cpadronprophorizontal { get; set; }
        public Nullable<int> id_cpadronubicacion { get; set; }
        public Nullable<int> id_propiedadhorizontal { get; set; }
    
        public virtual CPadron_Ubicaciones CPadron_Ubicaciones { get; set; }
        public virtual Ubicaciones_PropiedadHorizontal Ubicaciones_PropiedadHorizontal { get; set; }
    }
}

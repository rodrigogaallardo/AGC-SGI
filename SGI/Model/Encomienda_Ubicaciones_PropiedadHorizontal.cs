

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Ubicaciones_PropiedadHorizontal
    {
        public int id_encomiendaprophorizontal { get; set; }
        public Nullable<int> id_encomiendaubicacion { get; set; }
        public Nullable<int> id_propiedadhorizontal { get; set; }
    
        public virtual Ubicaciones_PropiedadHorizontal Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual Encomienda_Ubicaciones Encomienda_Ubicaciones { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_InformacionRelevante_Historial_Cambios
    {
        public int id_rubInfRel_histcam { get; set; }
        public int id_rubhistcam { get; set; }
        public int id_rubinf { get; set; }
        public string descripcion_rubinf { get; set; }
    
        public virtual Rubros_Historial_Cambios Rubros_Historial_Cambios { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_CircuitoAtomatico_Zonas_Historial_Cambios
    {
        public int id_rubcircauto_histcam { get; set; }
        public int id_rubhistcam { get; set; }
        public int id_rubcircauto { get; set; }
        public string codZonaHab { get; set; }
    
        public virtual Rubros_Historial_Cambios Rubros_Historial_Cambios { get; set; }
    }
}

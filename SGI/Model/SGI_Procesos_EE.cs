

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Procesos_EE
    {
        public SGI_Procesos_EE()
        {
            this.SGI_Tarea_Generar_Expediente_Procesos = new HashSet<SGI_Tarea_Generar_Expediente_Procesos>();
        }
    
        public int id_proceso { get; set; }
        public string desc_proceso { get; set; }
        public string cod_proceso { get; set; }
    
        public virtual ICollection<SGI_Tarea_Generar_Expediente_Procesos> SGI_Tarea_Generar_Expediente_Procesos { get; set; }
    }
}

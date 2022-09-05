

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Rel_Resultados_Tareas
    {
        public ENG_Rel_Resultados_Tareas()
        {
            this.ENG_Rel_Resultados_Tareas_Transiciones = new HashSet<ENG_Rel_Resultados_Tareas_Transiciones>();
        }
    
        public int id_resultadotarea { get; set; }
        public int id_tarea { get; set; }
        public int id_resultado { get; set; }
    
        public virtual ICollection<ENG_Rel_Resultados_Tareas_Transiciones> ENG_Rel_Resultados_Tareas_Transiciones { get; set; }
        public virtual ENG_Tareas ENG_Tareas { get; set; }
        public virtual ENG_Resultados ENG_Resultados { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Rel_Resultados_Tareas_Transiciones
    {
        public int id_resultadotareatransicion { get; set; }
        public int id_resultadotarea { get; set; }
        public int id_transicion { get; set; }
    
        public virtual ENG_Rel_Resultados_Tareas ENG_Rel_Resultados_Tareas { get; set; }
        public virtual ENG_Transiciones ENG_Transiciones { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Transiciones
    {
        public ENG_Transiciones()
        {
            this.ENG_Rel_Resultados_Tareas_Transiciones = new HashSet<ENG_Rel_Resultados_Tareas_Transiciones>();
        }
    
        public int id_transicion { get; set; }
        public int id_tarea_origen { get; set; }
        public int id_tarea_destino { get; set; }
        public string condiciones_transicion { get; set; }
        public string acciones_al_entrar { get; set; }
        public string acciones_al_salir { get; set; }
    
        public virtual ICollection<ENG_Rel_Resultados_Tareas_Transiciones> ENG_Rel_Resultados_Tareas_Transiciones { get; set; }
        public virtual ENG_Tareas ENG_Tareas { get; set; }
        public virtual ENG_Tareas ENG_Tareas1 { get; set; }
    }
}

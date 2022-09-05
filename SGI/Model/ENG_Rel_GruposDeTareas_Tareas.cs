

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Rel_GruposDeTareas_Tareas
    {
        public int id_grupotarea { get; set; }
        public int id_grupo { get; set; }
        public int id_tarea { get; set; }
    
        public virtual ENG_GruposDeTareas ENG_GruposDeTareas { get; set; }
        public virtual ENG_Tareas ENG_Tareas { get; set; }
    }
}

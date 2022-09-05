

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Tipos_Tareas
    {
        public ENG_Tipos_Tareas()
        {
            this.ENG_Tareas = new HashSet<ENG_Tareas>();
        }
    
        public int id_tipo_tarea { get; set; }
        public string nombre { get; set; }
    
        public virtual ICollection<ENG_Tareas> ENG_Tareas { get; set; }
    }
}

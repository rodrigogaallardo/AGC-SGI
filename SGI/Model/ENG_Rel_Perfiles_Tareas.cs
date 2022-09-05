

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Rel_Perfiles_Tareas
    {
        public int id_perfiltarea { get; set; }
        public int id_perfil { get; set; }
        public int id_tarea { get; set; }
    
        public virtual ENG_Tareas ENG_Tareas { get; set; }
        public virtual SGI_Perfiles SGI_Perfiles { get; set; }
    }
}

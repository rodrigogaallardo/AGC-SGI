

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_GruposDeTareas
    {
        public ENG_GruposDeTareas()
        {
            this.ENG_Rel_GruposDeTareas_Tareas = new HashSet<ENG_Rel_GruposDeTareas_Tareas>();
        }
    
        public int id_grupo { get; set; }
        public int cod_grupo { get; set; }
        public string nombre_grupo { get; set; }
    
        public virtual ICollection<ENG_Rel_GruposDeTareas_Tareas> ENG_Rel_GruposDeTareas_Tareas { get; set; }
    }
}

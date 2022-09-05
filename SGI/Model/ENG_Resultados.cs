

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Resultados
    {
        public ENG_Resultados()
        {
            this.ENG_Rel_Resultados_Tareas = new HashSet<ENG_Rel_Resultados_Tareas>();
            this.SGI_Tramites_Tareas = new HashSet<SGI_Tramites_Tareas>();
        }
    
        public int id_resultado { get; set; }
        public string nombre_resultado { get; set; }
        public int nro_orden_resultado { get; set; }
    
        public virtual ICollection<ENG_Rel_Resultados_Tareas> ENG_Rel_Resultados_Tareas { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas> SGI_Tramites_Tareas { get; set; }
    }
}

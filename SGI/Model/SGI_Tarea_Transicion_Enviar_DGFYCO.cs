

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Transicion_Enviar_DGFYCO
    {
        public int id_tareatransdgfyco { get; set; }
        public int id_tramitetarea { get; set; }
        public int id_tipoplanoincendio { get; set; }
    
        public virtual SGI_Tipos_Planos_Incendio SGI_Tipos_Planos_Incendio { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

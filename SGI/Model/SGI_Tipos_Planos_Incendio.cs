

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tipos_Planos_Incendio
    {
        public SGI_Tipos_Planos_Incendio()
        {
            this.SGI_Tarea_Transicion_Enviar_DGFYCO = new HashSet<SGI_Tarea_Transicion_Enviar_DGFYCO>();
        }
    
        public int id_tipoplanoincendio { get; set; }
        public string nombre_tipoplanoincendio { get; set; }
        public string descripcion_tipoplanoincendio { get; set; }
        public bool baja_logica { get; set; }
    
        public virtual ICollection<SGI_Tarea_Transicion_Enviar_DGFYCO> SGI_Tarea_Transicion_Enviar_DGFYCO { get; set; }
    }
}

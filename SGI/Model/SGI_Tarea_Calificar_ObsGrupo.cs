

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Calificar_ObsGrupo
    {
        public SGI_Tarea_Calificar_ObsGrupo()
        {
            this.SGI_Tarea_Calificar_ObsDocs = new HashSet<SGI_Tarea_Calificar_ObsDocs>();
        }
    
        public int id_ObsGrupo { get; set; }
        public int id_tramitetarea { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual ICollection<SGI_Tarea_Calificar_ObsDocs> SGI_Tarea_Calificar_ObsDocs { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

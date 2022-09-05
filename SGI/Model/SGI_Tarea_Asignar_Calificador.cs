

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Asignar_Calificador
    {
        public int id_asignar_calificador { get; set; }
        public int id_tramitetarea { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.Guid> usuario_asignado { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Calificar
    {
        public int id_calificar { get; set; }
        public int id_tramitetarea { get; set; }
        public string Observaciones { get; set; }
        public string Observaciones_contribuyente { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string Observaciones_Internas { get; set; }
        public string Observaciones_Providencia { get; set; }
        public bool Librar_Uso { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

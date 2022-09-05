

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Pagos_log
    {
        public int id { get; set; }
        public int id_tramitetarea { get; set; }
        public Nullable<int> id_pago { get; set; }
        public Nullable<int> id_estado { get; set; }
        public string mensaje { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
        public System.Guid CreateUser { get; set; }
        public string sistema { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Entregar_Tramite
    {
        public int id_entregar_tramite { get; set; }
        public int id_tramitetarea { get; set; }
        public string Observaciones { get; set; }
        public Nullable<int> nro_expediente { get; set; }
        public Nullable<int> anio_expediente { get; set; }
        public Nullable<System.DateTime> fecha_entrega_tramite { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string enviar_a { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Carga_Tramite
    {
        public int id_carga_tramite { get; set; }
        public int id_tramitetarea { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string Observaciones_informe { get; set; }
        public string observaciones_contribuyente { get; set; }
        public Nullable<int> id_tipo_informe { get; set; }
    
        public virtual TiposDeInformes TiposDeInformes { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_SADE_Procesos
    {
        public int id_tarea_proc { get; set; }
        public int id_paquete { get; set; }
        public int id_tramitetarea { get; set; }
        public int id_proceso { get; set; }
        public Nullable<int> id_origen_reg { get; set; }
        public Nullable<int> id_file { get; set; }
        public bool realizado_en_pasarela { get; set; }
        public string descripcion_tramite { get; set; }
        public Nullable<int> id_devolucion_ee { get; set; }
        public string resultado_ee { get; set; }
        public Nullable<System.DateTime> fecha_en_SADE { get; set; }
        public bool realizado_en_SADE { get; set; }
        public string parametros_SADE { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

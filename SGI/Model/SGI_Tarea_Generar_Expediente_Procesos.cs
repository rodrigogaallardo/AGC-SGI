

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Generar_Expediente_Procesos
    {
        public int id_generar_expediente_proc { get; set; }
        public int id_paquete { get; set; }
        public int id_caratula { get; set; }
        public int id_proceso { get; set; }
        public int id_devolucion_ee { get; set; }
        public string resultado_ee { get; set; }
        public bool realizado { get; set; }
        public Nullable<int> nro_tramite { get; set; }
        public string descripcion_tramite { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> id_tramitetarea { get; set; }
        public string parametros_SADE { get; set; }
    
        public virtual SGI_Procesos_EE SGI_Procesos_EE { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

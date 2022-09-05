

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tareas_Pases_Sectores
    {
        public int id_tarea_sector { get; set; }
        public int id_tarea_origen { get; set; }
        public Nullable<int> id_tarea_destino { get; set; }
        public int id_sector { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<int> id_estado { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual ENG_Tareas ENG_Tareas { get; set; }
        public virtual ENG_Tareas ENG_Tareas1 { get; set; }
        public virtual SADE_Estados_Expedientes SADE_Estados_Expedientes { get; set; }
        public virtual Sectores_SADE Sectores_SADE { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_LIZA_Procesos
    {
        public int id_liza_proceso { get; set; }
        public int id_ticket { get; set; }
        public int id_tramitetarea { get; set; }
        public Nullable<int> id_file { get; set; }
        public bool realizado { get; set; }
        public string descripcion { get; set; }
        public string resultado { get; set; }
        public string parametros { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual SGI_LIZA_Ticket SGI_LIZA_Ticket { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

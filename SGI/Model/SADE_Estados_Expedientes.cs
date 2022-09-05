

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SADE_Estados_Expedientes
    {
        public SADE_Estados_Expedientes()
        {
            this.SGI_Tareas_Pases_Sectores = new HashSet<SGI_Tareas_Pases_Sectores>();
        }
    
        public int id_estado { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual ICollection<SGI_Tareas_Pases_Sectores> SGI_Tareas_Pases_Sectores { get; set; }
    }
}

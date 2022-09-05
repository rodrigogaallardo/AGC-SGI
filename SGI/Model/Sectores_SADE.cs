

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sectores_SADE
    {
        public Sectores_SADE()
        {
            this.SGI_Tareas_Pases_Sectores = new HashSet<SGI_Tareas_Pases_Sectores>();
        }
    
        public int id_sector { get; set; }
        public string nombre_sector { get; set; }
        public string codigo_sector { get; set; }
        public string reparticion { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual ICollection<SGI_Tareas_Pases_Sectores> SGI_Tareas_Pases_Sectores { get; set; }
    }
}

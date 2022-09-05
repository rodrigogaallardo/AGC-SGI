

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Operaciones
    {
        public Ubicaciones_Operaciones()
        {
            this.Ubicaciones_Operaciones_Detalle = new HashSet<Ubicaciones_Operaciones_Detalle>();
        }
    
        public int id_operacion { get; set; }
        public int id_accion { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> lastUpdateDate { get; set; }
        public Nullable<System.Guid> lastUpdateUser { get; set; }
        public int id_estado { get; set; }
    
        public virtual Ubicaciones_Acciones Ubicaciones_Acciones { get; set; }
        public virtual ICollection<Ubicaciones_Operaciones_Detalle> Ubicaciones_Operaciones_Detalle { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Solicitudes_Observaciones
    {
        public int id_solobs { get; set; }
        public int id_solicitud { get; set; }
        public string observaciones { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<bool> leido { get; set; }
    
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}

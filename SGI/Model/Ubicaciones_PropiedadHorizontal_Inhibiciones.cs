

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_PropiedadHorizontal_Inhibiciones
    {
        public int id_ubicphorinhibi { get; set; }
        public int id_propiedadhorizontal { get; set; }
        public string motivo { get; set; }
        public System.DateTime fecha_inhibicion { get; set; }
        public Nullable<System.DateTime> fecha_vencimiento { get; set; }
        public string resultado { get; set; }
        public string observaciones { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string MotivoLevantamiento { get; set; }
    
        public virtual Ubicaciones_PropiedadHorizontal Ubicaciones_PropiedadHorizontal { get; set; }
    }
}

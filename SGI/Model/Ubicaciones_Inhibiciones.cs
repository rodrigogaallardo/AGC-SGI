

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class Ubicaciones_Inhibiciones
    {
        public int id_ubicinhibi { get; set; }
        public int id_ubicacion { get; set; }
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

        [JsonIgnore]
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

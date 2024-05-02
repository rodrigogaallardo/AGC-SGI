

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class Ubicaciones_Clausuras
    {
        public int id_ubicclausura { get; set; }
        public int id_ubicacion { get; set; }
        public string motivo { get; set; }
        public System.DateTime fecha_alta_clausura { get; set; }
        public Nullable<System.DateTime> fecha_baja_clausura { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }

        [JsonIgnore]
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

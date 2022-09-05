

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_PropiedadHorizontal_Clausuras
    {
        public int id_ubicphorclausura { get; set; }
        public int id_propiedadhorizontal { get; set; }
        public string motivo { get; set; }
        public System.DateTime fecha_alta_clausura { get; set; }
        public Nullable<System.DateTime> fecha_baja_clausura { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    
        public virtual Ubicaciones_PropiedadHorizontal Ubicaciones_PropiedadHorizontal { get; set; }
    }
}

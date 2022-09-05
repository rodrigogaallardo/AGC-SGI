

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Normativas
    {
        public int id_transftiponormativa { get; set; }
        public int id_solicitud { get; set; }
        public int id_tiponormativa { get; set; }
        public int id_entidadnormativa { get; set; }
        public string nro_normativa { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual EntidadNormativa EntidadNormativa { get; set; }
        public virtual TipoNormativa TipoNormativa { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}

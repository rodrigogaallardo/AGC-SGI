

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Normativas
    {
        public int id_encomiendatiponormativa { get; set; }
        public int id_encomienda { get; set; }
        public int id_tiponormativa { get; set; }
        public int id_entidadnormativa { get; set; }
        public string nro_normativa { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    
        public virtual EntidadNormativa EntidadNormativa { get; set; }
        public virtual TipoNormativa TipoNormativa { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}

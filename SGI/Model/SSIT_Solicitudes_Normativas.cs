

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Normativas
    {
        public int IdSolicitud { get; set; }
        public int id_tiponormativa { get; set; }
        public int id_entidadnormativa { get; set; }
        public string nro_normativa { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual EntidadNormativa EntidadNormativa { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
        public virtual TipoNormativa TipoNormativa { get; set; }
    }
}

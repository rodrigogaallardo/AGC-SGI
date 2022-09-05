

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Eximicion_CAA
    {
        public int id_eximicion_caa { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int id_solicitud { get; set; }
        public int id_tipo_tramite { get; set; }
        public bool eximido { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

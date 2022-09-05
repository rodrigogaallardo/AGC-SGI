

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Solicitud_planoVisado
    {
        public int id_solPlanoVisado { get; set; }
        public int id_solicitud { get; set; }
        public int id_docAdjunto { get; set; }
        public int id_tramiteTarea { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

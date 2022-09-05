

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Permisos_DatosAdicionales
    {
        public int IdSolicitud { get; set; }
        public int id_solicitud_caa { get; set; }
        public int id_caa { get; set; }
        public int id_rac { get; set; }
        public int id_form_rac { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

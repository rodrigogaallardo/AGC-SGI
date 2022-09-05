

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Solicitudes_AvisoCaducidad
    {
        public int id_aviso { get; set; }
        public int id_solicitud { get; set; }
        public int id_email { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Emails Emails { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}

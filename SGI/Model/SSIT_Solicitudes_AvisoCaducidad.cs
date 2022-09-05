

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_AvisoCaducidad
    {
        public int id_aviso { get; set; }
        public int id_solicitud { get; set; }
        public int id_email { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> fechaNotificacionSSIT { get; set; }
    
        public virtual Emails Emails { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Notificaciones
    {
        public int id_notificacion { get; set; }
        public int id_solicitud { get; set; }
        public int id_email { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> fechaNotificacionSSIT { get; set; }
        public Nullable<int> Id_NotificacionMotivo { get; set; }
    
        public virtual Emails Emails { get; set; }
        public virtual SSIT_Solicitudes_Notificaciones_motivos SSIT_Solicitudes_Notificaciones_motivos { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

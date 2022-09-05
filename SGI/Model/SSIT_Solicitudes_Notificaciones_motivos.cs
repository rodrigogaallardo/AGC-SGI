

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Notificaciones_motivos
    {
        public SSIT_Solicitudes_Notificaciones_motivos()
        {
            this.SSIT_Solicitudes_Notificaciones = new HashSet<SSIT_Solicitudes_Notificaciones>();
            this.Transf_Solicitudes_Notificaciones = new HashSet<Transf_Solicitudes_Notificaciones>();
        }
    
        public int IdNotificacionMotivo { get; set; }
        public string NotificacionMotivo { get; set; }
    
        public virtual ICollection<SSIT_Solicitudes_Notificaciones> SSIT_Solicitudes_Notificaciones { get; set; }
        public virtual ICollection<Transf_Solicitudes_Notificaciones> Transf_Solicitudes_Notificaciones { get; set; }
    }
}

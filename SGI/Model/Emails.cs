

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emails
    {
        public Emails()
        {
            this.SSIT_Solicitudes_Notificaciones = new HashSet<SSIT_Solicitudes_Notificaciones>();
            this.SSIT_Solicitudes_AvisoCaducidad = new HashSet<SSIT_Solicitudes_AvisoCaducidad>();
            this.Transf_Solicitudes_AvisoCaducidad = new HashSet<Transf_Solicitudes_AvisoCaducidad>();
            this.SSIT_Solicitudes_AvisoRechazo = new HashSet<SSIT_Solicitudes_AvisoRechazo>();
        }
    
        public int id_email { get; set; }
        public Nullable<System.Guid> guid { get; set; }
        public int id_origen { get; set; }
        public int id_tipo_email { get; set; }
        public int id_estado { get; set; }
        public Nullable<int> cant_intentos { get; set; }
        public Nullable<int> cant_max_intentos { get; set; }
        public Nullable<int> prioridad { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public Nullable<System.DateTime> fecha_envio { get; set; }
        public string email { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string asunto { get; set; }
        public string html { get; set; }
        public Nullable<System.DateTime> fecha_lectura { get; set; }
        public string Usuario { get; set; }
    
        public virtual Email_Estados Email_Estados { get; set; }
        public virtual Emails_Tipos Emails_Tipos { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Notificaciones> SSIT_Solicitudes_Notificaciones { get; set; }
        public virtual ICollection<SSIT_Solicitudes_AvisoCaducidad> SSIT_Solicitudes_AvisoCaducidad { get; set; }
        public virtual ICollection<Transf_Solicitudes_AvisoCaducidad> Transf_Solicitudes_AvisoCaducidad { get; set; }
        public virtual ICollection<SSIT_Solicitudes_AvisoRechazo> SSIT_Solicitudes_AvisoRechazo { get; set; }
    }
}

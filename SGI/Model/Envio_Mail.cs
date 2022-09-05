

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Envio_Mail
    {
        public int id_envio_mail { get; set; }
        public Nullable<int> id_proceso { get; set; }
        public Nullable<int> id_origen { get; set; }
        public Nullable<int> id_estado { get; set; }
        public Nullable<int> cant_intentos { get; set; }
        public Nullable<int> cant_max_intentos { get; set; }
        public Nullable<int> prioridad { get; set; }
        public Nullable<System.DateTime> fecha_alta { get; set; }
        public Nullable<System.DateTime> fecha_envio { get; set; }
        public string email { get; set; }
        public string asunto { get; set; }
        public string html { get; set; }
    
        public virtual Envio_Mail_Estados Envio_Mail_Estados { get; set; }
        public virtual Envio_Mail_Proceso Envio_Mail_Proceso { get; set; }
    }
}

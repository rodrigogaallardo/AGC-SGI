

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Envio_Mail_Prioridades
    {
        public int ID_Prioridad { get; set; }
        public System.TimeSpan Hora_Desde { get; set; }
        public System.TimeSpan Hora_Hasta { get; set; }
        public int Tiempo_Reenvio { get; set; }
        public string Obervacion { get; set; }
        public Nullable<System.DateTime> Ultima_Ejecucion { get; set; }
    }
}

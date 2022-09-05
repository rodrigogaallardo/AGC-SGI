

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_SSIT_Solicitudes
    {
        public int id_encomiendaSolicitud { get; set; }
        public int id_encomienda { get; set; }
        public int id_solicitud { get; set; }
    
        public virtual Encomienda Encomienda { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

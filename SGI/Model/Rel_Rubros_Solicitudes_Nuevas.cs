

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_Rubros_Solicitudes_Nuevas
    {
        public int id_relrubSol { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int id_Solicitud { get; set; }
        public decimal Superficie { get; set; }
    
        public virtual SSIT_Solicitudes_Nuevas SSIT_Solicitudes_Nuevas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_ZonasPlaneamiento_ZonasHabilitaciones
    {
        public int id_rel_zonapla_zonahab { get; set; }
        public string CodZonaLey449 { get; set; }
        public string CodZonaHab { get; set; }
        public string Observaciones { get; set; }
    
        public virtual Zonas_Habilitaciones Zonas_Habilitaciones { get; set; }
    }
}

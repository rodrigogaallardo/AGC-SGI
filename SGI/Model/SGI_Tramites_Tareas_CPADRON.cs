

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tramites_Tareas_CPADRON
    {
        public int id_rel_tt_CPADRON { get; set; }
        public int id_tramitetarea { get; set; }
        public int id_cpadron { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
    }
}

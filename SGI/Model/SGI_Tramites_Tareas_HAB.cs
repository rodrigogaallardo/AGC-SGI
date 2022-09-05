

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tramites_Tareas_HAB
    {
        public int id_rel_tt_HAB { get; set; }
        public int id_tramitetarea { get; set; }
        public int id_solicitud { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}

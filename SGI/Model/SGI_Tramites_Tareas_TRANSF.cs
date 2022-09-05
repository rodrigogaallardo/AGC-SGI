

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tramites_Tareas_TRANSF
    {
        public int id_rel_tt_TRANSF { get; set; }
        public int id_tramitetarea { get; set; }
        public int id_solicitud { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}

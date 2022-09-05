

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tramites_Tareas_Dispo_Considerando
    {
        public int id_tt_Dispo_Considerando { get; set; }
        public int id_tramitetarea { get; set; }
        public string considerando_dispo { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}

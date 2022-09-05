

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_TiposDeDocumentosRequeridos_ENG_Tareas
    {
        public int id_tdoc_adj_tarea { get; set; }
        public int id_tdocreq { get; set; }
        public int id_tarea { get; set; }
    
        public virtual ENG_Tareas ENG_Tareas { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
    }
}

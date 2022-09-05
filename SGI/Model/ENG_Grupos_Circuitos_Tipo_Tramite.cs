

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Grupos_Circuitos_Tipo_Tramite
    {
        public int id_eng_gc_tt { get; set; }
        public int id_grupo_circuito { get; set; }
        public int id_tipo_expediente { get; set; }
        public int id_sub_tipo_expediente { get; set; }
    
        public virtual ENG_Grupos_Circuitos ENG_Grupos_Circuitos { get; set; }
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        public virtual TipoExpediente TipoExpediente { get; set; }
    }
}

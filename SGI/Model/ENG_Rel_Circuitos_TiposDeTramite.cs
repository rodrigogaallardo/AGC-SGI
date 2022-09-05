

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Rel_Circuitos_TiposDeTramite
    {
        public int id_circuitotipotramite { get; set; }
        public int id_circuito { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public Nullable<int> id_grupo_circuito { get; set; }
    
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        public virtual TipoExpediente TipoExpediente { get; set; }
        public virtual TipoTramite TipoTramite { get; set; }
        public virtual ENG_Grupos_Circuitos ENG_Grupos_Circuitos { get; set; }
        public virtual ENG_Circuitos ENG_Circuitos { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_TipoTramite_TipoExpediente
    {
        public int id_rel_TipoTramite_TipoExpediente { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
    
        public virtual TipoExpediente TipoExpediente { get; set; }
        public virtual TipoTramite TipoTramite { get; set; }
    }
}

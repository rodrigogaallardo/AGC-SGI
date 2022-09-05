

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_TipoExpediente_SubtipoExpediente
    {
        public int id_tipo_subtipo { get; set; }
        public Nullable<int> id_tipoexpediente { get; set; }
        public Nullable<int> id_subtipoexpediente { get; set; }
    
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        public virtual TipoExpediente TipoExpediente { get; set; }
    }
}

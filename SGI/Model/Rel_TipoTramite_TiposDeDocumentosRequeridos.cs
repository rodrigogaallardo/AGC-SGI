

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_TipoTramite_TiposDeDocumentosRequeridos
    {
        public int id_rel_tiptra_tdocreq { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tdocreq { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual TipoTramite TipoTramite { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
    }
}

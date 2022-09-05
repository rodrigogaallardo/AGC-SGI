

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosCN_TiposDeDocumentosRequeridos
    {
        public int id_rubtdocreq { get; set; }
        public int id_rubro { get; set; }
        public int id_tdocreq { get; set; }
        public bool obligatorio_rubtdocreq { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
        public virtual RubrosCN RubrosCN { get; set; }
    }
}

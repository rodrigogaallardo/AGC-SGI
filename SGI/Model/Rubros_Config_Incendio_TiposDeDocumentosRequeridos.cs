

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Config_Incendio_TiposDeDocumentosRequeridos
    {
        public int id_rubro_incendio_tdocreq { get; set; }
        public int id_rubro_incendio { get; set; }
        public int id_tdocreq { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Rubros_Config_Incendio Rubros_Config_Incendio { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
    }
}

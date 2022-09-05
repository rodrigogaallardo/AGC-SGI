

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_Encomienda_Rectificatoria
    {
        public int id_relencrec { get; set; }
        public int id_encomienda_anterior { get; set; }
        public int id_solicitud_anterior { get; set; }
        public int id_encomienda_nueva { get; set; }
    
        public virtual Encomienda Encomienda { get; set; }
        public virtual Encomienda Encomienda1 { get; set; }
    }
}

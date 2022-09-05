

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_TiposDeDocumentosRequeridos_Historial_Cambios
    {
        public int id_rubDocReq_histcam { get; set; }
        public int id_rubhistcam { get; set; }
        public int id_rubtdocreq { get; set; }
        public int id_tdocreq { get; set; }
        public bool obligatorio_rubtdocreq { get; set; }
    
        public virtual Rubros_Historial_Cambios Rubros_Historial_Cambios { get; set; }
    }
}

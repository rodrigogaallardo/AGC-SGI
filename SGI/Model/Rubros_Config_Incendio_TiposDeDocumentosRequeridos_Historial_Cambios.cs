

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios
    {
        public int id_rubro_incendio_tdocreq_histcam { get; set; }
        public int id_rubro_incendio_histcam { get; set; }
        public int id_rubro_incendio_tdocreq { get; set; }
        public int id_tdocreq { get; set; }
    
        public virtual Rubros_Config_Incendio_Historial_Cambios Rubros_Config_Incendio_Historial_Cambios { get; set; }
    }
}

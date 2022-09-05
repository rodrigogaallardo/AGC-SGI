

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Config_Incendio_Historial_Cambios
    {
        public Rubros_Config_Incendio_Historial_Cambios()
        {
            this.Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios = new HashSet<Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios>();
        }
    
        public int id_rubro_incendio_histcam { get; set; }
        public int id_rubhistcam { get; set; }
        public int id_rubro_incendio { get; set; }
        public int riesgo { get; set; }
        public decimal DesdeM2 { get; set; }
        public decimal HastaM2 { get; set; }
    
        public virtual ICollection<Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios> Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios { get; set; }
        public virtual Rubros_Historial_Cambios Rubros_Historial_Cambios { get; set; }
    }
}

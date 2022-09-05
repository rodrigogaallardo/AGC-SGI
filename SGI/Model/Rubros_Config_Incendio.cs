

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Config_Incendio
    {
        public Rubros_Config_Incendio()
        {
            this.Rubros_Config_Incendio_TiposDeDocumentosRequeridos = new HashSet<Rubros_Config_Incendio_TiposDeDocumentosRequeridos>();
        }
    
        public int id_rubro_incendio { get; set; }
        public int id_rubro { get; set; }
        public int riesgo { get; set; }
        public decimal DesdeM2 { get; set; }
        public decimal HastaM2 { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual Rubros Rubros { get; set; }
        public virtual ICollection<Rubros_Config_Incendio_TiposDeDocumentosRequeridos> Rubros_Config_Incendio_TiposDeDocumentosRequeridos { get; set; }
    }
}

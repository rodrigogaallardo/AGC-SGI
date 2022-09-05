

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosCN_Config_Incendio
    {
        public int id_rubro_incendio { get; set; }
        public int id_rubro { get; set; }
        public int riesgo { get; set; }
        public decimal DesdeM2 { get; set; }
        public decimal HastaM2 { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual RubrosCN RubrosCN { get; set; }
    }
}

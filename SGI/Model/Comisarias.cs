

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comisarias
    {
        public Comisarias()
        {
            this.Ubicaciones_Historial_Cambios = new HashSet<Ubicaciones_Historial_Cambios>();
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public int id_comisaria { get; set; }
        public string nom_comisaria { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<Ubicaciones_Historial_Cambios> Ubicaciones_Historial_Cambios { get; set; }
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Barrios
    {
        public Barrios()
        {
            this.Ubicaciones_Historial_Cambios = new HashSet<Ubicaciones_Historial_Cambios>();
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public int id_barrio { get; set; }
        public string nom_barrio { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<Ubicaciones_Historial_Cambios> Ubicaciones_Historial_Cambios { get; set; }
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}

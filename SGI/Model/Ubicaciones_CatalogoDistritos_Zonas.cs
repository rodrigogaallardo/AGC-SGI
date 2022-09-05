

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_CatalogoDistritos_Zonas
    {
        public Ubicaciones_CatalogoDistritos_Zonas()
        {
            this.Ubicaciones_CatalogoDistritos_Subzonas = new HashSet<Ubicaciones_CatalogoDistritos_Subzonas>();
        }
    
        public int IdZona { get; set; }
        public int IdDistrito { get; set; }
        public string CodigoZona { get; set; }
    
        public virtual ICollection<Ubicaciones_CatalogoDistritos_Subzonas> Ubicaciones_CatalogoDistritos_Subzonas { get; set; }
        public virtual Ubicaciones_CatalogoDistritos Ubicaciones_CatalogoDistritos { get; set; }
    }
}

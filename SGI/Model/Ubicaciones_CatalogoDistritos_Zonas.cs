

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class Ubicaciones_CatalogoDistritos_Zonas
    {
        public Ubicaciones_CatalogoDistritos_Zonas()
        {
            this.Ubicaciones_CatalogoDistritos_Subzonas = new HashSet<Ubicaciones_CatalogoDistritos_Subzonas>();
        }
    
        public int IdZona { get; set; }
        public int IdDistrito { get; set; }
        public string CodigoZona { get; set; }

        [JsonIgnore]
        public virtual ICollection<Ubicaciones_CatalogoDistritos_Subzonas> Ubicaciones_CatalogoDistritos_Subzonas { get; set; }
        [JsonIgnore]
        public virtual Ubicaciones_CatalogoDistritos Ubicaciones_CatalogoDistritos { get; set; }
    }
}

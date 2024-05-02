

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class Ubicaciones_CatalogoDistritos_Subzonas
    {
        public int IdSubZona { get; set; }
        public int IdZona { get; set; }
        public string CodigoSubZona { get; set; }

        [JsonIgnore]
        public virtual Ubicaciones_CatalogoDistritos_Zonas Ubicaciones_CatalogoDistritos_Zonas { get; set; }
    }
}

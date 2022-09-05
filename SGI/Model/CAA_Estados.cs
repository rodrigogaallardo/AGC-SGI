

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CAA_Estados
    {
        public int id_estado { get; set; }
        public string cod_estado { get; set; }
        public string nom_estado_usuario { get; set; }
        public string nom_estado_interno { get; set; }
        public int orden { get; set; }
        public bool visible_Aprob_prof { get; set; }
    }
}

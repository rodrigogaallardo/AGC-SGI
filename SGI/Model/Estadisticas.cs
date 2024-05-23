

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas
    {
        public int id_estadistica { get; set; }
        public string cod_estadistica { get; set; }
        public Nullable<decimal> valornum_estadistica { get; set; }
        public string valorchar_estadistica { get; set; }
    }
}

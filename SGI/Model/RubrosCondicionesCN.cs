

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosCondicionesCN
    {
        public int IdCondicion { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdFormula { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}

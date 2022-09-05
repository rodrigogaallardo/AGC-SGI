

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_Rubros_ImpactoAmbiental
    {
        public int id_relrubImp { get; set; }
        public int id_rubro { get; set; }
        public int id_ImpactoAmbiental { get; set; }
        public decimal DesdeM2 { get; set; }
        public decimal HastaM2 { get; set; }
        public bool AntenaEmisora { get; set; }
        public string LetraAnexo { get; set; }
    
        public virtual ImpactoAmbiental ImpactoAmbiental { get; set; }
    }
}

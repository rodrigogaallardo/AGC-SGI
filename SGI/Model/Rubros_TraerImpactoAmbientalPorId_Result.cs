

namespace SGI.Model
{
    using System;
    
    public partial class Rubros_TraerImpactoAmbientalPorId_Result
    {
        public int id_relrubimp { get; set; }
        public int id_rubro { get; set; }
        public int id_impactoambiental { get; set; }
        public string cod_impactoambiental { get; set; }
        public string nom_impactoambiental { get; set; }
        public decimal desdem2 { get; set; }
        public decimal hastam2 { get; set; }
        public bool antenaemisora { get; set; }
        public string LetraAnexo { get; set; }
    }
}

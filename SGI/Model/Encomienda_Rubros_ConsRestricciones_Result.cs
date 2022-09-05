

namespace SGI.Model
{
    using System;
    
    public partial class Encomienda_Rubros_ConsRestricciones_Result
    {
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public bool EsAnterior_rubro { get; set; }
        public Nullable<int> Superficie_ok { get; set; }
        public Nullable<int> Zona_ok { get; set; }
    }
}

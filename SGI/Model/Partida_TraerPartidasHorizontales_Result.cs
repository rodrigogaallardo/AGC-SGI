

namespace SGI.Model
{
    using System;
    
    public partial class Partida_TraerPartidasHorizontales_Result
    {
        public int id_partidamatriz { get; set; }
        public int id_partidahorizontal { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
    }
}

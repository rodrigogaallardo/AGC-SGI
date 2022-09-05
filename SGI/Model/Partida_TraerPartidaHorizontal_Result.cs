

namespace SGI.Model
{
    using System;
    
    public partial class Partida_TraerPartidaHorizontal_Result
    {
        public int id_PartidaHorizontal { get; set; }
        public int id_partidamatriz { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public Nullable<int> CodCalle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartidaHorizontal
    {
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> CodCalle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string Observaciones { get; set; }
        public Nullable<bool> Inhibida { get; set; }
        public int id_partidahorizontal { get; set; }
        public int id_partidamatriz { get; set; }
        public System.DateTime VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public bool baja_logica { get; set; }
    }
}

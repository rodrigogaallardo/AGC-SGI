

namespace SGI.Model
{
    using System;
    
    public partial class Partida_TraerPartidaMatriz_Result
    {
        public int id_PartidaMatriz { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public Nullable<decimal> Superficie { get; set; }
        public int NroLocales { get; set; }
        public int NroUnidades { get; set; }
        public Nullable<decimal> Fondo { get; set; }
        public Nullable<decimal> Frente { get; set; }
        public Nullable<int> Estado { get; set; }
    }
}

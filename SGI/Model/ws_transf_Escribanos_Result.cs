

namespace SGI.Model
{
    using System;
    
    public partial class ws_transf_Escribanos_Result
    {
        public int id_cpadron { get; set; }
        public string Direccion { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
    }
}

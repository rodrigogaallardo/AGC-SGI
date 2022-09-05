

namespace SGI.Model
{
    using System;
    
    public partial class SSIS_BuscarUbicacionesAProcesar_Result
    {
        public int id_ubicacion { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string puerta { get; set; }
    }
}

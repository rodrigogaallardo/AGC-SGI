

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerPuertas_Result
    {
        public int Id { get; set; }
        public int IdSolicitudPartida { get; set; }
        public Nullable<int> id_partidahorizontal { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public Nullable<int> id_partidamatriz { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public string NomCalle { get; set; }
        public int CodCalle { get; set; }
        public string NroPuerta { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string Observaciones { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public int id_subtipoubicacion { get; set; }
    }
}

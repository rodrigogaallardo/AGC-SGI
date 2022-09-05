

namespace SGI.Model
{
    using System;
    
    public partial class ASR_Datos_elevador_Result
    {
        public string RazonSocial_empasc { get; set; }
        public string nom_estado_aceptacion { get; set; }
        public Nullable<int> id_estado_pago { get; set; }
        public Nullable<int> id_empasc { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string Nombre_calle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> NroRegistro_empasc { get; set; }
        public Nullable<int> cantidad { get; set; }
    }
}

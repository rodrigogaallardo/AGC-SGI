

namespace SGI.Model
{
    using System;
    
    public partial class ASR_Elevadores_PorEmpresa_PorUbicacion_Result
    {
        public string RazonSocial_empasc { get; set; }
        public Nullable<int> id_empasc { get; set; }
        public Nullable<int> id_dgubicacion { get; set; }
        public Nullable<int> anio_vigencia { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> NroRegistro_empasc { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<System.DateTime> fecha_inspeccion { get; set; }
    }
}

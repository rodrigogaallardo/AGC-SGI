

namespace SGI.Model
{
    using System;
    
    public partial class ASRExportarElevadoresxEmpresaxEstados_Result
    {
        public int id_dgubicacion { get; set; }
        public string RazonSocial_empasc { get; set; }
        public string Email_empasc { get; set; }
        public int Anio { get; set; }
        public string Estado_Pago { get; set; }
        public string Estado_Aceptacion { get; set; }
        public Nullable<int> Cant_elevadores { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public string Email_administrador { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    
    public partial class ASRExportarElevadoresGeneral_Result
    {
        public int id_dgubicacion { get; set; }
        public Nullable<int> seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public int NroRegistro_empasc { get; set; }
        public string RazonSocial_empasc { get; set; }
        public string Email_empasc { get; set; }
        public int Anio { get; set; }
        public string Estado_Pago { get; set; }
        public Nullable<System.DateTime> fecha_estado_aceptacion { get; set; }
        public string Estado_Aceptacion { get; set; }
        public Nullable<int> patente_elevador { get; set; }
        public string nom_tipoelevador { get; set; }
        public string Email_administrador { get; set; }
    }
}

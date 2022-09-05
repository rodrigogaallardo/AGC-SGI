

namespace SGI.Model
{
    using System;
    
    public partial class IFCIExportarConsultaGeneralExtendida_Result
    {
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string Direccion { get; set; }
        public string Dado_baja { get; set; }
        public Nullable<int> patente_ifci { get; set; }
        public string nom_tipoinstalacion { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Nullable<int> NroRegistro_empici { get; set; }
        public string RazonSocial_empici { get; set; }
        public string email_empici { get; set; }
        public Nullable<int> cant_pisos { get; set; }
        public Nullable<int> cant_subsuelos { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string nom_estado_aceptacion { get; set; }
        public string rubro { get; set; }
        public string confirmado { get; set; }
        public decimal superficie { get; set; }
    }
}

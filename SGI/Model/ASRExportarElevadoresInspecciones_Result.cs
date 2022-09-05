

namespace SGI.Model
{
    using System;
    
    public partial class ASRExportarElevadoresInspecciones_Result
    {
        public int id_dgubicacion { get; set; }
        public Nullable<int> seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public Nullable<int> id_empasc { get; set; }
        public string RazonSocial_empasc { get; set; }
        public int Anio { get; set; }
        public int id_elevador { get; set; }
        public Nullable<int> patente_elevador { get; set; }
        public string tecnico_ult_informe { get; set; }
        public Nullable<System.DateTime> fecha_ult_inspeccion { get; set; }
        public Nullable<System.TimeSpan> hora_ult_inspeccion { get; set; }
        public string res_ult_informe { get; set; }
        public string obs_ult_informe { get; set; }
        public string mail_administrador { get; set; }
        public string mail_empasc { get; set; }
        public string mail_tecnico { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    
    public partial class SolicitudesListaTareaResultados_Result
    {
        public Nullable<int> id_solicitud { get; set; }
        public string nombre_tarea { get; set; }
        public string nombre_resultado { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio { get; set; }
        public Nullable<System.DateTime> Fecha_Asignacion { get; set; }
        public Nullable<System.TimeSpan> Hora_Asignacion { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre { get; set; }
        public Nullable<int> Dif_ini_cierre { get; set; }
        public Nullable<int> Dif_asig_cierre { get; set; }
        public string UserName { get; set; }
        public Nullable<decimal> superficie { get; set; }
        public string numero_dispo_GEDO { get; set; }
    }
}

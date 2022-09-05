

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja3
    {
        public Nullable<int> id_circuito_actual { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public string nombre_tarea { get; set; }
        public string Circuito_Tarea { get; set; }
        public string nombre_resultado { get; set; }
        public Nullable<System.DateTime> fecha_inicio { get; set; }
        public Nullable<System.TimeSpan> hora_inicio { get; set; }
        public Nullable<System.DateTime> fecha_asignacion { get; set; }
        public Nullable<System.TimeSpan> hora_asignacion { get; set; }
        public Nullable<System.DateTime> fecha_cierre { get; set; }
        public Nullable<System.TimeSpan> hora_cierre { get; set; }
        public Nullable<int> Dif_ini_cierre { get; set; }
        public Nullable<int> Dif_asig_cierre { get; set; }
        public string username { get; set; }
        public Nullable<decimal> superficie { get; set; }
        public string NroDisposicionSADE { get; set; }
        public string nombre_proxima_tarea { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Circuito_Origen { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public int id { get; set; }
    }
}

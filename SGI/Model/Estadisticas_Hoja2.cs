

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja2
    {
        public Nullable<int> id_circuito_actual { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public string Observaciones_contribuyente { get; set; }
        public Nullable<System.DateTime> FechaInicio_tramitetarea { get; set; }
        public Nullable<System.DateTime> FechaAsignacion_tramitetarea { get; set; }
        public Nullable<System.DateTime> FechaCierre_tramitetarea { get; set; }
        public string observador { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public int id { get; set; }
        public string nombre_tdocreq { get; set; }
        public string Observacion_ObsDocs { get; set; }
    }
}

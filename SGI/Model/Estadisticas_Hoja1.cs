

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja1
    {
        public Nullable<int> id_circuito_actual { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public string Observaciones { get; set; }
        public string observador { get; set; }
        public string cod_circuito_origen { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public int id { get; set; }
    }
}

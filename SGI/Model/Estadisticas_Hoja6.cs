

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja6
    {
        public Nullable<int> id_circuito_actual { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public Nullable<System.DateTime> asigCalif_ini { get; set; }
        public Nullable<System.DateTime> rhyp_ini { get; set; }
        public Nullable<System.DateTime> rfd_cierre { get; set; }
        public Nullable<System.DateTime> dict_ini { get; set; }
        public Nullable<System.DateTime> dict_cierre { get; set; }
        public Nullable<System.DateTime> avh_ini { get; set; }
        public Nullable<System.DateTime> avh_cierre { get; set; }
        public string Observado { get; set; }
        public Nullable<int> Cantidad_Observado { get; set; }
        public Nullable<int> Dif_EE_asig_cierre { get; set; }
        public Nullable<int> Dif_RFD_asig_cierre { get; set; }
        public string Circuito_Origen { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public int id { get; set; }
        public Nullable<int> dias_dictamen { get; set; }
        public string es_caducidad { get; set; }
    }
}

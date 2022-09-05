

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja9
    {
        public int id { get; set; }
        public int id_circuito_actual { get; set; }
        public string cod_circuito { get; set; }
        public int id_solicitud { get; set; }
        public Nullable<System.DateTime> FechaInicio_Asig_Calificador { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Notif_Caducidad { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Rev_DGHP_Caducidad { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Rev_DGHP_Caducidad { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Gen_Expediente { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Rev_Firma { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}

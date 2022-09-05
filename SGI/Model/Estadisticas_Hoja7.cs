

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja7
    {
        public int id { get; set; }
        public int id_circuito_actual { get; set; }
        public string cod_circuito { get; set; }
        public int id_solicitud { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Control_Informe { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_Control_Informe { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_GenerarExpediente { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_GenerarExpediente { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Generar_Expediente { get; set; }
        public Nullable<System.TimeSpan> Hora_Fin_Generar_Expediente { get; set; }
        public Nullable<bool> Observado_alguna_vez { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}

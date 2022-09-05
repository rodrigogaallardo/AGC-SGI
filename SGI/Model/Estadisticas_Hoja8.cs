

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja8
    {
        public int Id { get; set; }
        public int id_circuito_actual { get; set; }
        public string cod_circuito { get; set; }
        public int id_solicitud { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Asignacion_Calificador { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_Asignacion_Calificador { get; set; }
        public Nullable<System.DateTime> Fecha_cierre_Revision_gerente_2 { get; set; }
        public Nullable<System.TimeSpan> Hora_cierre_Revision_gerente_2 { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_Revision_firma_dispo { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre_Revision_firma_dispo { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Dictamenes { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_Dictamenes { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_Dictamenes { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre_Dictamenes { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Revision_Pagos { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_Revision_Pagos { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Revision_Pagos { get; set; }
        public Nullable<System.TimeSpan> Hora_Fin_Revision_Pagos { get; set; }
        public string Observado { get; set; }
        public Nullable<int> Cantidad_Veces_Observado { get; set; }
        public string Circuito_Origen { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> dias_dictamen { get; set; }
    }
}

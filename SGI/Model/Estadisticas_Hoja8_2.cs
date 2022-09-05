

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadisticas_Hoja8_2
    {
        public int Id { get; set; }
        public int id_circuito_actual { get; set; }
        public string cod_circuito { get; set; }
        public int id_solicitud { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Asignacion_Calificador { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_Asignacion_Calificador { get; set; }
        public Nullable<System.DateTime> Fecha_inicio_Revision_DGHP { get; set; }
        public Nullable<System.TimeSpan> Hora_inicio_Revision_DGHP { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_Revision_firma_dispo { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre_Revision_firma_dispo { get; set; }
        public Nullable<System.DateTime> Fecha_inicio_Consulta_Adicional { get; set; }
        public Nullable<System.TimeSpan> Hora_inicio_Consulta_Adicional { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_Consulta_Adicional { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre_Consulta_Adicional { get; set; }
        public Nullable<System.DateTime> Fecha_inicio_AVH { get; set; }
        public Nullable<System.TimeSpan> Hora_inicio_AVH { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_AVH { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre_AVH { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio_Dictamenes { get; set; }
        public Nullable<System.TimeSpan> Hora_Inicio_Dictamenes { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_Dictamenes { get; set; }
        public Nullable<System.TimeSpan> Hora_Cierre_Dictamenes { get; set; }
        public string Observado { get; set; }
        public Nullable<int> Cantidad_Veces_Observado { get; set; }
        public string Circuito_Origen { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> dias_dictamen { get; set; }
        public Nullable<int> dias_consulta_adicional { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Revision_Gerente { get; set; }
        public Nullable<System.TimeSpan> hora_Fin_Revision_Gerente { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Revision_Subgerente { get; set; }
        public Nullable<System.TimeSpan> Hora_Fin_Revision_Subgerente { get; set; }
    }
}

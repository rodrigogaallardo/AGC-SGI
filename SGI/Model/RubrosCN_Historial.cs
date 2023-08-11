

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosCN_Historial
    {
        public int IdRubrosCN_historial { get; set; }
        public string tipo_operacion { get; set; }
        public Nullable<System.DateTime> fecha_operacion { get; set; }
        public int IdRubro { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Keywords { get; set; }
        public Nullable<System.DateTime> VigenciaDesde_rubro { get; set; }
        public Nullable<System.DateTime> VigenciaHasta_rubro { get; set; }
        public int IdTipoActividad { get; set; }
        public int IdTipoExpediente { get; set; }
        public Nullable<int> IdGrupoCircuito { get; set; }
        public bool LibrarUso { get; set; }
        public string ZonaMixtura1 { get; set; }
        public string ZonaMixtura2 { get; set; }
        public string ZonaMixtura3 { get; set; }
        public string ZonaMixtura4 { get; set; }
        public Nullable<int> IdEstacionamiento { get; set; }
        public Nullable<int> IdBicicleta { get; set; }
        public Nullable<int> IdCyD { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<bool> Asistentes350 { get; set; }
        public Nullable<bool> SinBanioPCD { get; set; }
        public bool CondicionExpress { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Log_MovimientosUsuario
    {
        public int id { get; set; }
        public Nullable<System.Guid> usuario { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }
        public Nullable<int> id_file { get; set; }
        public string datosAdicionales { get; set; }
        public string URL { get; set; }
        public string Observacion_Solicitante { get; set; }
        public string TipoMovimiento { get; set; }
        public Nullable<int> TipoURL { get; set; }
    }
}

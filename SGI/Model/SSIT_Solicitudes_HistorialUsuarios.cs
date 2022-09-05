

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_HistorialUsuarios
    {
        public int id_solicituduser { get; set; }
        public int id_solicitud { get; set; }
        public System.Guid Usuario_Origen { get; set; }
        public System.Guid Usuario_Destino { get; set; }
        public System.Guid Usuario_Editor { get; set; }
        public System.DateTime FechaHora_Modificación { get; set; }
        public Nullable<int> id_file { get; set; }
        public Nullable<int> id_tipotramite { get; set; }
        public Nullable<int> id_encomienda { get; set; }
    
        public virtual TipoTramite TipoTramite { get; set; }
    }
}

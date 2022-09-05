

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Historial_Cambios_UsuariosIntervinientes
    {
        public int id_ubihistcamusu { get; set; }
        public int id_ubihistcam { get; set; }
        public Nullable<System.Guid> Userid { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rubros_Historial_Cambios_UsuariosIntervinientes
    {
        public int id_rubhistcamusu { get; set; }
        public int id_rubhistcam { get; set; }
        public System.Guid Userid { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
    }
}

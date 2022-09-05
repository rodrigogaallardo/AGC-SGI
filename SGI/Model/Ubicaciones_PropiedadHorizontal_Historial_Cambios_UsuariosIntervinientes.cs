

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes
    {
        public int id_phhistcamusu { get; set; }
        public int id_phhistcam { get; set; }
        public System.Guid Userid { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
    }
}

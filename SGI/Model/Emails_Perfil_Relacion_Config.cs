

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emails_Perfil_Relacion_Config
    {
        public int id_rel { get; set; }
        public int id_profile { get; set; }
        public string ws_username { get; set; }
    
        public virtual Emails_Perfil_Config Emails_Perfil_Config { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Config_BandejaAsignacion
    {
        public int id_bandejaAsig { get; set; }
        public int id_tarea { get; set; }
        public Nullable<int> id_perfil_asignador { get; set; }
        public Nullable<int> id_perfil_asignado { get; set; }
    
        public virtual SGI_Perfiles SGI_Perfiles { get; set; }
        public virtual SGI_Perfiles SGI_Perfiles1 { get; set; }
        public virtual ENG_Tareas ENG_Tareas { get; set; }
    }
}

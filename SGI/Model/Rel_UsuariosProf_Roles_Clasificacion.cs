

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rel_UsuariosProf_Roles_Clasificacion
    {
        public int id_rel_prof_clasificacion { get; set; }
        public System.Guid UserID { get; set; }
        public System.Guid RoleID { get; set; }
        public int id_clasificacion { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual aspnet_Roles aspnet_Roles { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual GrupoConsejos_Roles_Clasificacion GrupoConsejos_Roles_Clasificacion { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
    }
}

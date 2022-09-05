

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConsejoProfesional_RolesPermitidos
    {
        public int id_grupoconsejo_rol { get; set; }
        public int id_grupoconsejo { get; set; }
        public System.Guid RoleID { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual aspnet_Roles aspnet_Roles { get; set; }
        public virtual GrupoConsejos GrupoConsejos { get; set; }
    }
}

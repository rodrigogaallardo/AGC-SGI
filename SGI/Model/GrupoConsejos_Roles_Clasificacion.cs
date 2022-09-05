

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GrupoConsejos_Roles_Clasificacion
    {
        public GrupoConsejos_Roles_Clasificacion()
        {
            this.Rel_UsuariosProf_Roles_Clasificacion = new HashSet<Rel_UsuariosProf_Roles_Clasificacion>();
        }
    
        public int id_clasificacion { get; set; }
        public System.Guid RoleID { get; set; }
        public string descripcion_clasificacion { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual aspnet_Roles aspnet_Roles { get; set; }
        public virtual ICollection<Rel_UsuariosProf_Roles_Clasificacion> Rel_UsuariosProf_Roles_Clasificacion { get; set; }
    }
}

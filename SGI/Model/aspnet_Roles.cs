

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class aspnet_Roles
    {
        public aspnet_Roles()
        {
            this.aspnet_Users = new HashSet<aspnet_Users>();
            this.GrupoConsejos_Roles_Clasificacion = new HashSet<GrupoConsejos_Roles_Clasificacion>();
            this.Rel_UsuariosProf_Roles_Clasificacion = new HashSet<Rel_UsuariosProf_Roles_Clasificacion>();
            this.ConsejoProfesional_RolesPermitidos = new HashSet<ConsejoProfesional_RolesPermitidos>();
        }
    
        public System.Guid ApplicationId { get; set; }
        public System.Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
    
        public virtual aspnet_Applications aspnet_Applications { get; set; }
        public virtual ICollection<aspnet_Users> aspnet_Users { get; set; }
        public virtual ICollection<GrupoConsejos_Roles_Clasificacion> GrupoConsejos_Roles_Clasificacion { get; set; }
        public virtual ICollection<Rel_UsuariosProf_Roles_Clasificacion> Rel_UsuariosProf_Roles_Clasificacion { get; set; }
        public virtual ICollection<ConsejoProfesional_RolesPermitidos> ConsejoProfesional_RolesPermitidos { get; set; }
    }
}

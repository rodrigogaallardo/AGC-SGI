

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Perfiles
    {
        public SGI_Perfiles()
        {
            this.ENG_Rel_Perfiles_Tareas = new HashSet<ENG_Rel_Perfiles_Tareas>();
            this.aspnet_Users2 = new HashSet<aspnet_Users>();
            this.SGI_Menues = new HashSet<SGI_Menues>();
            this.ENG_Config_BandejaAsignacion = new HashSet<ENG_Config_BandejaAsignacion>();
            this.ENG_Config_BandejaAsignacion1 = new HashSet<ENG_Config_BandejaAsignacion>();
        }
    
        public int id_perfil { get; set; }
        public string nombre_perfil { get; set; }
        public string descripcion_perfil { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual ICollection<ENG_Rel_Perfiles_Tareas> ENG_Rel_Perfiles_Tareas { get; set; }
        public virtual ICollection<aspnet_Users> aspnet_Users2 { get; set; }
        public virtual ICollection<SGI_Menues> SGI_Menues { get; set; }
        public virtual ICollection<ENG_Config_BandejaAsignacion> ENG_Config_BandejaAsignacion { get; set; }
        public virtual ICollection<ENG_Config_BandejaAsignacion> ENG_Config_BandejaAsignacion1 { get; set; }
    }
}

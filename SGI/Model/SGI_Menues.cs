

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Menues
    {
        public SGI_Menues()
        {
            this.SGI_Perfiles = new HashSet<SGI_Perfiles>();
        }
    
        public int id_menu { get; set; }
        public string descripcion_menu { get; set; }
        public string aclaracion_menu { get; set; }
        public string pagina_menu { get; set; }
        public string iconCssClass_menu { get; set; }
        public Nullable<int> id_menu_padre { get; set; }
        public int nroOrden { get; set; }
        public bool visible { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual ICollection<SGI_Perfiles> SGI_Perfiles { get; set; }
    }
}

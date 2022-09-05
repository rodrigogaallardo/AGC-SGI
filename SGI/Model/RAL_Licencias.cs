

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RAL_Licencias
    {
        public RAL_Licencias()
        {
            this.Rubros = new HashSet<Rubros>();
        }
    
        public int id_licencia_alcohol { get; set; }
        public string cod_licencia_alcohol { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<Rubros> Rubros { get; set; }
    }
}

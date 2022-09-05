

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_EquipoDeTrabajo
    {
        public int id_equipotrabajo { get; set; }
        public System.Guid Userid { get; set; }
        public System.Guid Userid_Responsable { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Instructivos
    {
        public int id_instructivo { get; set; }
        public string cod_instructivo { get; set; }
        public int id_file { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
    }
}

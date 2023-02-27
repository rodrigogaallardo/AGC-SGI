

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Feriados
    {
        public int IdFeriado { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
    }
}

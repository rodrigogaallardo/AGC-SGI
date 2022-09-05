

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Sobrecargas
    {
        public int id_sobrecarga { get; set; }
        public int id_encomienda { get; set; }
        public string estructura_sobrecarga { get; set; }
        public int peso_sobrecarga { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual Encomienda Encomienda { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Email_Estados
    {
        public Email_Estados()
        {
            this.Emails = new HashSet<Emails>();
        }
    
        public int id_estado { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<Emails> Emails { get; set; }
    }
}

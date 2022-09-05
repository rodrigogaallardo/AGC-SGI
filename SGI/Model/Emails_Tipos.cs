

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emails_Tipos
    {
        public Emails_Tipos()
        {
            this.Emails = new HashSet<Emails>();
        }
    
        public int id_tipo_email { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<Emails> Emails { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Envio_Mail_Estados
    {
        public Envio_Mail_Estados()
        {
            this.Envio_Mail = new HashSet<Envio_Mail>();
        }
    
        public int id_estado { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<Envio_Mail> Envio_Mail { get; set; }
    }
}

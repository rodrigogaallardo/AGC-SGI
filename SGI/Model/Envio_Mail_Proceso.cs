

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Envio_Mail_Proceso
    {
        public Envio_Mail_Proceso()
        {
            this.Envio_Mail = new HashSet<Envio_Mail>();
        }
    
        public int id_proceso { get; set; }
        public string descripcion { get; set; }
        public string cfg_mail_from { get; set; }
        public string cfg_smtp { get; set; }
        public Nullable<int> cfg_smpt_puerto { get; set; }
        public string cfg_html { get; set; }
    
        public virtual ICollection<Envio_Mail> Envio_Mail { get; set; }
    }
}

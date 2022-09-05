

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Estados
    {
        public CPadron_Estados()
        {
            this.CPadron_Solicitudes = new HashSet<CPadron_Solicitudes>();
        }
    
        public int id_estado { get; set; }
        public string cod_estado { get; set; }
        public string nom_estado_usuario { get; set; }
        public string nom_estado_interno { get; set; }
    
        public virtual ICollection<CPadron_Solicitudes> CPadron_Solicitudes { get; set; }
    }
}

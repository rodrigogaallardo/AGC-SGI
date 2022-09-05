

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class vis_Certificados
    {
        public int id_certificado { get; set; }
        public int TipoTramite { get; set; }
        public int NroTramite { get; set; }
        public byte[] Certificado { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<int> item { get; set; }
    }
}

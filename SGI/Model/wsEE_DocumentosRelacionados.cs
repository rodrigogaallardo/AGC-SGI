

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsEE_DocumentosRelacionados
    {
        public int id_relacion { get; set; }
        public int id_caratula { get; set; }
        public int id_documento { get; set; }
        public string sistema { get; set; }
        public string username_SADE { get; set; }
        public bool relacionado_SADE { get; set; }
        public string resultado_SADE { get; set; }
        public int cantidad_intentos { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
    
        public virtual wsEE_Caratulas wsEE_Caratulas { get; set; }
        public virtual wsEE_Documentos wsEE_Documentos { get; set; }
    }
}

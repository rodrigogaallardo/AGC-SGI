

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerNormativa_Result
    {
        public Nullable<int> IdTipoNormativa { get; set; }
        public string TipoNormativa { get; set; }
        public string NroNormativa { get; set; }
        public Nullable<int> IdEntidadNormativa { get; set; }
        public string EntidadNormativa { get; set; }
        public Nullable<int> AnioNormativa { get; set; }
    }
}

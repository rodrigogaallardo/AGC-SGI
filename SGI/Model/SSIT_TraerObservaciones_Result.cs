

namespace SGI.Model
{
    using System;
    
    public partial class SSIT_TraerObservaciones_Result
    {
        public int id_solobs { get; set; }
        public int id_solicitud { get; set; }
        public string observaciones { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string fecha_usuario { get; set; }
    }
}

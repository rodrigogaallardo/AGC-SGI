

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Origen
    {
        public int id_solicitud { get; set; }
        public Nullable<int> id_solicitud_origen { get; set; }
        public Nullable<int> id_transf_origen { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes1 { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}

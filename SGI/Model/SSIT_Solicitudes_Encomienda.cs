

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Encomienda
    {
        public int id_sol_enc { get; set; }
        public int id_solicitud { get; set; }
        public int id_encomienda { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual Encomienda Encomienda { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Solicitudes_Pagos
    {
        public int id_sol_pago { get; set; }
        public int id_solicitud { get; set; }
        public int id_pago { get; set; }
        public decimal monto_pago { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}

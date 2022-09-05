

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Solicitudes_Pagos
    {
        public int id_sol_pago { get; set; }
        public int id_tramitetarea { get; set; }
        public int id_pago { get; set; }
        public int id_medio_pago { get; set; }
        public decimal monto_pago { get; set; }
        public string codigo_barras { get; set; }
        public Nullable<long> nro_boleta_unica { get; set; }
        public Nullable<int> nro_dependencia { get; set; }
        public string codigo_verificador { get; set; }
        public string url_pago { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
        public virtual wsPagos wsPagos { get; set; }
    }
}

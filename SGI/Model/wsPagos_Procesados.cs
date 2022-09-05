

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsPagos_Procesados
    {
        public int id_pago_BU { get; set; }
        public int EstadoPago_BU { get; set; }
        public int EstadoPago_Nuevo { get; set; }
        public string Job { get; set; }
        public System.DateTime Fecha { get; set; }
    }
}

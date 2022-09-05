

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsPagos_BoletaUnica_HistorialEstados2
    {
        public int id_histest { get; set; }
        public int id_pago_bu { get; set; }
        public int id_estadopago_ant { get; set; }
        public int id_estadopago_nuevo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
}

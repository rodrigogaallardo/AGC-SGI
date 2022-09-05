

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsPagos_BoletaUnica_Estados
    {
        public wsPagos_BoletaUnica_Estados()
        {
            this.wsPagos_BoletaUnica = new HashSet<wsPagos_BoletaUnica>();
            this.wsPagos_BoletaUnica_HistorialEstados = new HashSet<wsPagos_BoletaUnica_HistorialEstados>();
        }
    
        public int id_estadopago { get; set; }
        public string codigo_estadopago_BUI { get; set; }
        public string nom_estadopago { get; set; }
        public int secuencia_estado_pago { get; set; }
    
        public virtual ICollection<wsPagos_BoletaUnica> wsPagos_BoletaUnica { get; set; }
        public virtual ICollection<wsPagos_BoletaUnica_HistorialEstados> wsPagos_BoletaUnica_HistorialEstados { get; set; }
    }
}

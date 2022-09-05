

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposMotivoBaja
    {
        public TiposMotivoBaja()
        {
            this.SSIT_Solicitudes_Baja = new HashSet<SSIT_Solicitudes_Baja>();
            this.Transf_Solicitudes_Baja = new HashSet<Transf_Solicitudes_Baja>();
            this.Cpadron_Solicitudes_Baja = new HashSet<Cpadron_Solicitudes_Baja>();
        }
    
        public int id_tipo_motivo_baja { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
    
        public virtual ICollection<SSIT_Solicitudes_Baja> SSIT_Solicitudes_Baja { get; set; }
        public virtual ICollection<Transf_Solicitudes_Baja> Transf_Solicitudes_Baja { get; set; }
        public virtual ICollection<Cpadron_Solicitudes_Baja> Cpadron_Solicitudes_Baja { get; set; }
    }
}

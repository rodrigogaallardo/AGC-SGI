

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoEstadoSolicitud
    {
        public TipoEstadoSolicitud()
        {
            this.Solicitud = new HashSet<Solicitud>();
            this.SSIT_Solicitudes_Nuevas = new HashSet<SSIT_Solicitudes_Nuevas>();
            this.Transf_Solicitudes = new HashSet<Transf_Solicitudes>();
            this.SSIT_Solicitudes = new HashSet<SSIT_Solicitudes>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool InformaRULHSolicitud { get; set; }
        public bool InformaRULHHistorial { get; set; }
    
        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Nuevas> SSIT_Solicitudes_Nuevas { get; set; }
        public virtual ICollection<Transf_Solicitudes> Transf_Solicitudes { get; set; }
        public virtual ICollection<SSIT_Solicitudes> SSIT_Solicitudes { get; set; }
    }
}

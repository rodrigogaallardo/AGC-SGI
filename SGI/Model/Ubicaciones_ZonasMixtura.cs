

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_ZonasMixtura
    {
        public Ubicaciones_ZonasMixtura()
        {
            this.Encomienda_Ubicaciones_Mixturas = new HashSet<Encomienda_Ubicaciones_Mixturas>();
            this.Transf_Ubicaciones_Mixturas = new HashSet<Transf_Ubicaciones_Mixturas>();
            this.SSIT_Solicitudes_Ubicaciones_Mixturas = new HashSet<SSIT_Solicitudes_Ubicaciones_Mixturas>();
            this.Ubicaciones_temp = new HashSet<Ubicaciones_temp>();
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public int IdZonaMixtura { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<Encomienda_Ubicaciones_Mixturas> Encomienda_Ubicaciones_Mixturas { get; set; }
        public virtual ICollection<Transf_Ubicaciones_Mixturas> Transf_Ubicaciones_Mixturas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_Mixturas> SSIT_Solicitudes_Ubicaciones_Mixturas { get; set; }
        public virtual ICollection<Ubicaciones_temp> Ubicaciones_temp { get; set; }
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}

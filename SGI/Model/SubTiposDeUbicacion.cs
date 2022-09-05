

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubTiposDeUbicacion
    {
        public SubTiposDeUbicacion()
        {
            this.CPadron_Ubicaciones = new HashSet<CPadron_Ubicaciones>();
            this.Ubicaciones_Historial_Cambios = new HashSet<Ubicaciones_Historial_Cambios>();
            this.SSIT_Solicitudes_Ubicaciones = new HashSet<SSIT_Solicitudes_Ubicaciones>();
            this.Encomienda_Ubicaciones = new HashSet<Encomienda_Ubicaciones>();
            this.Transf_Ubicaciones = new HashSet<Transf_Ubicaciones>();
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public int id_subtipoubicacion { get; set; }
        public string descripcion_subtipoubicacion { get; set; }
        public int id_tipoubicacion { get; set; }
        public bool habilitado { get; set; }
    
        public virtual TiposDeUbicacion TiposDeUbicacion { get; set; }
        public virtual ICollection<CPadron_Ubicaciones> CPadron_Ubicaciones { get; set; }
        public virtual ICollection<Ubicaciones_Historial_Cambios> Ubicaciones_Historial_Cambios { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones> SSIT_Solicitudes_Ubicaciones { get; set; }
        public virtual ICollection<Encomienda_Ubicaciones> Encomienda_Ubicaciones { get; set; }
        public virtual ICollection<Transf_Ubicaciones> Transf_Ubicaciones { get; set; }
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}

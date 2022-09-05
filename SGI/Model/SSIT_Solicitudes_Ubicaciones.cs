

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Ubicaciones
    {
        public SSIT_Solicitudes_Ubicaciones()
        {
            this.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal = new HashSet<SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal>();
            this.SSIT_Solicitudes_Ubicaciones_Puertas = new HashSet<SSIT_Solicitudes_Ubicaciones_Puertas>();
            this.SSIT_Solicitudes_Ubicaciones_Distritos = new HashSet<SSIT_Solicitudes_Ubicaciones_Distritos>();
            this.SSIT_Solicitudes_Ubicaciones_Mixturas = new HashSet<SSIT_Solicitudes_Ubicaciones_Mixturas>();
        }
    
        public int id_solicitudubicacion { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public Nullable<int> id_ubicacion { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public string local_subtipoubicacion { get; set; }
        public string deptoLocal_ubicacion { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public int id_zonaplaneamiento { get; set; }
        public string Torre { get; set; }
        public string Local { get; set; }
        public string Depto { get; set; }
    
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal> SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_Puertas> SSIT_Solicitudes_Ubicaciones_Puertas { get; set; }
        public virtual SubTiposDeUbicacion SubTiposDeUbicacion { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_Distritos> SSIT_Solicitudes_Ubicaciones_Distritos { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_Mixturas> SSIT_Solicitudes_Ubicaciones_Mixturas { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

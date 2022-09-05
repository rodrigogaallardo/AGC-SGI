

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Ubicaciones
    {
        public Transf_Ubicaciones()
        {
            this.Transf_Ubicaciones_Distritos = new HashSet<Transf_Ubicaciones_Distritos>();
            this.Transf_Ubicaciones_Mixturas = new HashSet<Transf_Ubicaciones_Mixturas>();
            this.Transf_Ubicaciones_PropiedadHorizontal = new HashSet<Transf_Ubicaciones_PropiedadHorizontal>();
            this.Transf_Ubicaciones_Puertas = new HashSet<Transf_Ubicaciones_Puertas>();
        }
    
        public int id_transfubicacion { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public Nullable<int> id_ubicacion { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public string local_subtipoubicacion { get; set; }
        public string deptoLocal_transfubicacion { get; set; }
        public int id_zonaplaneamiento { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public string Torre { get; set; }
        public string Local { get; set; }
        public string Depto { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual SubTiposDeUbicacion SubTiposDeUbicacion { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
        public virtual ICollection<Transf_Ubicaciones_Distritos> Transf_Ubicaciones_Distritos { get; set; }
        public virtual ICollection<Transf_Ubicaciones_Mixturas> Transf_Ubicaciones_Mixturas { get; set; }
        public virtual ICollection<Transf_Ubicaciones_PropiedadHorizontal> Transf_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<Transf_Ubicaciones_Puertas> Transf_Ubicaciones_Puertas { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

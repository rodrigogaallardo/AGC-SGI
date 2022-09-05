

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Ubicaciones
    {
        public CPadron_Ubicaciones()
        {
            this.CPadron_Ubicaciones_PropiedadHorizontal = new HashSet<CPadron_Ubicaciones_PropiedadHorizontal>();
            this.CPadron_Ubicaciones_Puertas = new HashSet<CPadron_Ubicaciones_Puertas>();
        }
    
        public int id_cpadronubicacion { get; set; }
        public Nullable<int> id_cpadron { get; set; }
        public Nullable<int> id_ubicacion { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public string local_subtipoubicacion { get; set; }
        public string deptoLocal_cpadronubicacion { get; set; }
        public int id_zonaplaneamiento { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public string Torre { get; set; }
        public string Local { get; set; }
        public string Depto { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<CPadron_Ubicaciones_PropiedadHorizontal> CPadron_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<CPadron_Ubicaciones_Puertas> CPadron_Ubicaciones_Puertas { get; set; }
        public virtual SubTiposDeUbicacion SubTiposDeUbicacion { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Ubicaciones
    {
        public Encomienda_Ubicaciones()
        {
            this.Encomienda_Ubicaciones_PropiedadHorizontal = new HashSet<Encomienda_Ubicaciones_PropiedadHorizontal>();
            this.Encomienda_Ubicaciones_Puertas = new HashSet<Encomienda_Ubicaciones_Puertas>();
            this.Encomienda_Ubicaciones_Distritos = new HashSet<Encomienda_Ubicaciones_Distritos>();
            this.Encomienda_Ubicaciones_Mixturas = new HashSet<Encomienda_Ubicaciones_Mixturas>();
        }
    
        public int id_encomiendaubicacion { get; set; }
        public Nullable<int> id_encomienda { get; set; }
        public Nullable<int> id_ubicacion { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public string local_subtipoubicacion { get; set; }
        public string deptoLocal_encomiendaubicacion { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public int id_zonaplaneamiento { get; set; }
        public string Depto { get; set; }
        public string Local { get; set; }
        public string Torre { get; set; }
        public Nullable<int> AnchoCalle { get; set; }
        public Nullable<bool> InmuebleCatalogado { get; set; }
    
        public virtual ICollection<Encomienda_Ubicaciones_PropiedadHorizontal> Encomienda_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<Encomienda_Ubicaciones_Puertas> Encomienda_Ubicaciones_Puertas { get; set; }
        public virtual SubTiposDeUbicacion SubTiposDeUbicacion { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual ICollection<Encomienda_Ubicaciones_Distritos> Encomienda_Ubicaciones_Distritos { get; set; }
        public virtual ICollection<Encomienda_Ubicaciones_Mixturas> Encomienda_Ubicaciones_Mixturas { get; set; }
        public virtual Encomienda Encomienda { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

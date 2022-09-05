

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_PropiedadHorizontal
    {
        public Ubicaciones_PropiedadHorizontal()
        {
            this.Encomienda_Ubicaciones_PropiedadHorizontal = new HashSet<Encomienda_Ubicaciones_PropiedadHorizontal>();
            this.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal = new HashSet<SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal>();
            this.Ubicaciones_PropiedadHorizontal_Clausuras = new HashSet<Ubicaciones_PropiedadHorizontal_Clausuras>();
            this.Ubicaciones_PropiedadHorizontal_Inhibiciones = new HashSet<Ubicaciones_PropiedadHorizontal_Inhibiciones>();
            this.Transf_Ubicaciones_PropiedadHorizontal = new HashSet<Transf_Ubicaciones_PropiedadHorizontal>();
            this.CPadron_Ubicaciones_PropiedadHorizontal = new HashSet<CPadron_Ubicaciones_PropiedadHorizontal>();
        }
    
        public int id_propiedadhorizontal { get; set; }
        public int id_ubicacion { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string UnidadFuncional { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public bool EsEntidadGubernamental { get; set; }
        public bool baja_logica { get; set; }
    
        public virtual ICollection<Encomienda_Ubicaciones_PropiedadHorizontal> Encomienda_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal> SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<Ubicaciones_PropiedadHorizontal_Clausuras> Ubicaciones_PropiedadHorizontal_Clausuras { get; set; }
        public virtual ICollection<Ubicaciones_PropiedadHorizontal_Inhibiciones> Ubicaciones_PropiedadHorizontal_Inhibiciones { get; set; }
        public virtual ICollection<Transf_Ubicaciones_PropiedadHorizontal> Transf_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<CPadron_Ubicaciones_PropiedadHorizontal> CPadron_Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}

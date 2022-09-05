

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones
    {
        public Ubicaciones()
        {
            this.CPadron_Ubicaciones = new HashSet<CPadron_Ubicaciones>();
            this.Encomienda_Ubicaciones = new HashSet<Encomienda_Ubicaciones>();
            this.SSIT_Solicitudes_Ubicaciones = new HashSet<SSIT_Solicitudes_Ubicaciones>();
            this.Transf_Ubicaciones = new HashSet<Transf_Ubicaciones>();
            this.Ubicaciones_Clausuras = new HashSet<Ubicaciones_Clausuras>();
            this.Ubicaciones_DireccionesConformadas = new HashSet<Ubicaciones_DireccionesConformadas>();
            this.Ubicaciones_Distritos = new HashSet<Ubicaciones_Distritos>();
            this.Ubicaciones_Inhibiciones = new HashSet<Ubicaciones_Inhibiciones>();
            this.Ubicaciones_PropiedadHorizontal = new HashSet<Ubicaciones_PropiedadHorizontal>();
            this.Ubicaciones_Puertas = new HashSet<Ubicaciones_Puertas>();
            this.Ubicaciones_ZonasComplementarias = new HashSet<Ubicaciones_ZonasComplementarias>();
            this.Ubicaciones_ZonasMixtura = new HashSet<Ubicaciones_ZonasMixtura>();
        }
    
        public int id_ubicacion { get; set; }
        public Nullable<int> id_subtipoubicacion { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> Circunscripcion { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public Nullable<decimal> SuperficieTotal { get; set; }
        public Nullable<decimal> Fondo { get; set; }
        public Nullable<decimal> Frente { get; set; }
        public int id_zonaplaneamiento { get; set; }
        public string Observaciones { get; set; }
        public Nullable<decimal> Coordenada_X { get; set; }
        public Nullable<decimal> Coordenada_Y { get; set; }
        public System.DateTime VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public string Inhibida_Observacion { get; set; }
        public bool baja_logica { get; set; }
        public Nullable<decimal> SuperficieCubierta { get; set; }
        public Nullable<int> pisos_bajo_rasante { get; set; }
        public Nullable<int> pisos_sobre_rasante { get; set; }
        public Nullable<int> unidades { get; set; }
        public Nullable<int> locales { get; set; }
        public Nullable<int> cant_ph { get; set; }
        public Nullable<decimal> vuc { get; set; }
        public Nullable<int> id_comuna { get; set; }
        public Nullable<int> id_barrio { get; set; }
        public Nullable<int> id_areahospitalaria { get; set; }
        public Nullable<int> id_comisaria { get; set; }
        public Nullable<int> id_regionsanitaria { get; set; }
        public Nullable<int> id_distritoescolar { get; set; }
        public Nullable<System.DateTime> FechaUltimaActualizacionUSIG { get; set; }
        public Nullable<int> CantiActualizacionesUSIG { get; set; }
        public string resultadoActualizacionUSIG { get; set; }
        public string TipoPersonaTitularAGIP { get; set; }
        public string TitularAGIP { get; set; }
        public Nullable<System.DateTime> FechaAltaAGIP { get; set; }
        public bool EsEntidadGubernamental { get; set; }
        public bool EsUbicacionProtegida { get; set; }
    
        public virtual Barrios Barrios { get; set; }
        public virtual Comisarias Comisarias { get; set; }
        public virtual Comunas Comunas { get; set; }
        public virtual ICollection<CPadron_Ubicaciones> CPadron_Ubicaciones { get; set; }
        public virtual ICollection<Encomienda_Ubicaciones> Encomienda_Ubicaciones { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones> SSIT_Solicitudes_Ubicaciones { get; set; }
        public virtual SubTiposDeUbicacion SubTiposDeUbicacion { get; set; }
        public virtual ICollection<Transf_Ubicaciones> Transf_Ubicaciones { get; set; }
        public virtual ICollection<Ubicaciones_Clausuras> Ubicaciones_Clausuras { get; set; }
        public virtual ICollection<Ubicaciones_DireccionesConformadas> Ubicaciones_DireccionesConformadas { get; set; }
        public virtual ICollection<Ubicaciones_Distritos> Ubicaciones_Distritos { get; set; }
        public virtual ICollection<Ubicaciones_Inhibiciones> Ubicaciones_Inhibiciones { get; set; }
        public virtual ICollection<Ubicaciones_PropiedadHorizontal> Ubicaciones_PropiedadHorizontal { get; set; }
        public virtual ICollection<Ubicaciones_Puertas> Ubicaciones_Puertas { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual ICollection<Ubicaciones_ZonasComplementarias> Ubicaciones_ZonasComplementarias { get; set; }
        public virtual ICollection<Ubicaciones_ZonasMixtura> Ubicaciones_ZonasMixtura { get; set; }
    }
}

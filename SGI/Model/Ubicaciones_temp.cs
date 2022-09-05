

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_temp
    {
        public Ubicaciones_temp()
        {
            this.Ubicaciones_Distritos_temp = new HashSet<Ubicaciones_Distritos_temp>();
            this.Ubicaciones_Puertas_temp = new HashSet<Ubicaciones_Puertas_temp>();
            this.Ubicaciones_ZonasMixtura = new HashSet<Ubicaciones_ZonasMixtura>();
        }
    
        public int id_ubicacion_temp { get; set; }
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
    
        public virtual ICollection<Ubicaciones_Distritos_temp> Ubicaciones_Distritos_temp { get; set; }
        public virtual ICollection<Ubicaciones_Puertas_temp> Ubicaciones_Puertas_temp { get; set; }
        public virtual ICollection<Ubicaciones_ZonasMixtura> Ubicaciones_ZonasMixtura { get; set; }
    }
}

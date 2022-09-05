

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_Historial_Cambios
    {
        public Ubicaciones_Historial_Cambios()
        {
            this.Ubicaciones_Puertas_Historial_Cambios = new HashSet<Ubicaciones_Puertas_Historial_Cambios>();
            this.Ubicaciones_ZonasComplementarias_Historial_Cambios = new HashSet<Ubicaciones_ZonasComplementarias_Historial_Cambios>();
            this.Ubicaciones_Historial_Cambios_Estados = new HashSet<Ubicaciones_Historial_Cambios_Estados>();
        }
    
        public int id_ubihistcam { get; set; }
        public int tipo_solicitud { get; set; }
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
        public string Coordenada_X { get; set; }
        public string Coordenada_Y { get; set; }
        public System.DateTime VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public int id_estado_modif { get; set; }
        public string observaciones_ubihistcam { get; set; }
        public bool EsEntidadGubernamental { get; set; }
        public bool EsUbicacionProtegida { get; set; }
        public Nullable<bool> baja_logica { get; set; }
        public Nullable<int> id_barrio { get; set; }
        public Nullable<int> id_comisaria { get; set; }
    
        public virtual SubTiposDeUbicacion SubTiposDeUbicacion { get; set; }
        public virtual Zonas_Planeamiento Zonas_Planeamiento { get; set; }
        public virtual ICollection<Ubicaciones_Puertas_Historial_Cambios> Ubicaciones_Puertas_Historial_Cambios { get; set; }
        public virtual ICollection<Ubicaciones_ZonasComplementarias_Historial_Cambios> Ubicaciones_ZonasComplementarias_Historial_Cambios { get; set; }
        public virtual Barrios Barrios { get; set; }
        public virtual Comisarias Comisarias { get; set; }
        public virtual ICollection<Ubicaciones_Historial_Cambios_Estados> Ubicaciones_Historial_Cambios_Estados { get; set; }
    }
}

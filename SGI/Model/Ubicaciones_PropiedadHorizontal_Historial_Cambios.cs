

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_PropiedadHorizontal_Historial_Cambios
    {
        public int id_phhistcam { get; set; }
        public int tipo_solicitud { get; set; }
        public int id_propiedadhorizontal { get; set; }
        public int id_ubicacion { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string UnidadFuncional { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public int id_estado_modif { get; set; }
        public string observaciones_phhistcam { get; set; }
        public bool EsEntidadGubernamental { get; set; }
    }
}

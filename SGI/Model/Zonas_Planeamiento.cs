

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class Zonas_Planeamiento
    {
        public Zonas_Planeamiento()
        {
            this.Ubicaciones_ZonasComplementarias = new HashSet<Ubicaciones_ZonasComplementarias>();
            this.CPadron_Ubicaciones = new HashSet<CPadron_Ubicaciones>();
            this.Ubicaciones_Historial_Cambios = new HashSet<Ubicaciones_Historial_Cambios>();
            this.Ubicaciones_ZonasComplementarias_Historial_Cambios = new HashSet<Ubicaciones_ZonasComplementarias_Historial_Cambios>();
            this.SSIT_Solicitudes_Ubicaciones = new HashSet<SSIT_Solicitudes_Ubicaciones>();
            this.Encomienda_Ubicaciones = new HashSet<Encomienda_Ubicaciones>();
            this.Transf_Ubicaciones = new HashSet<Transf_Ubicaciones>();
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public int id_zonaplaneamiento { get; set; }
        public string CodZonaPla { get; set; }
        public string DescripcionZonaPla { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<Ubicaciones_ZonasComplementarias> Ubicaciones_ZonasComplementarias { get; set; }
        [JsonIgnore]
        public virtual ICollection<CPadron_Ubicaciones> CPadron_Ubicaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ubicaciones_Historial_Cambios> Ubicaciones_Historial_Cambios { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ubicaciones_ZonasComplementarias_Historial_Cambios> Ubicaciones_ZonasComplementarias_Historial_Cambios { get; set; }
        [JsonIgnore]
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones> SSIT_Solicitudes_Ubicaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<Encomienda_Ubicaciones> Encomienda_Ubicaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transf_Ubicaciones> Transf_Ubicaciones { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}

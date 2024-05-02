

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class RubrosDepositosCN
    {
        public RubrosDepositosCN()
        {
            this.Encomienda_RubrosCN_Deposito = new HashSet<Encomienda_RubrosCN_Deposito>();
            this.RubrosDepositosCN_RangosSuperficie = new HashSet<RubrosDepositosCN_RangosSuperficie>();
        }
    
        public int IdDeposito { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoriaDeposito { get; set; }
        public string GradoMolestia { get; set; }
        public string ZonaMixtura1 { get; set; }
        public string ZonaMixtura2 { get; set; }
        public string ZonaMixtura3 { get; set; }
        public string ZonaMixtura4 { get; set; }
        public string ObservacionesCategorizacion { get; set; }
        public Nullable<int> idCondicionIncendio { get; set; }
        public Nullable<System.DateTime> VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }

        [JsonIgnore]
        public virtual ICollection<Encomienda_RubrosCN_Deposito> Encomienda_RubrosCN_Deposito { get; set; }
        [JsonIgnore]
        public virtual RubrosDepositosCategoriasCN RubrosDepositosCategoriasCN { get; set; }
        [JsonIgnore]
        public virtual ICollection<RubrosDepositosCN_RangosSuperficie> RubrosDepositosCN_RangosSuperficie { get; set; }
        [JsonIgnore]
        public virtual CondicionesIncendio CondicionesIncendio { get; set; }
    }
}

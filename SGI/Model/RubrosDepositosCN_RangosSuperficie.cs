

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosDepositosCN_RangosSuperficie
    {
        public int idRepositoRangoSup { get; set; }
        public int IdDeposito { get; set; }
        public int id_tipocircuito { get; set; }
        public string LetraAnexo { get; set; }
        public decimal DesdeM2 { get; set; }
        public decimal HastaM2 { get; set; }
    
        public virtual RubrosDepositosCN RubrosDepositosCN { get; set; }
    }
}

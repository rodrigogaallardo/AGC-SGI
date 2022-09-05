

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_RubrosCN_Deposito
    {
        public int id_encomienda { get; set; }
        public int IdRubro { get; set; }
        public int IdDeposito { get; set; }
    
        public virtual Encomienda Encomienda { get; set; }
        public virtual RubrosDepositosCN RubrosDepositosCN { get; set; }
        public virtual RubrosCN RubrosCN { get; set; }
    }
}

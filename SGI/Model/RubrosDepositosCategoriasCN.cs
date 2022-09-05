

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosDepositosCategoriasCN
    {
        public RubrosDepositosCategoriasCN()
        {
            this.RubrosDepositosCN = new HashSet<RubrosDepositosCN>();
        }
    
        public int IdCategoriaDeposito { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<RubrosDepositosCN> RubrosDepositosCN { get; set; }
    }
}

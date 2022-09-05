

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CondicionesIncendio
    {
        public CondicionesIncendio()
        {
            this.RubrosDepositosCN = new HashSet<RubrosDepositosCN>();
            this.RubrosCN = new HashSet<RubrosCN>();
        }
    
        public int idCondicionIncendio { get; set; }
        public string codigo { get; set; }
        public Nullable<decimal> superficie { get; set; }
        public Nullable<decimal> superficieSubsuelo { get; set; }
    
        public virtual ICollection<RubrosDepositosCN> RubrosDepositosCN { get; set; }
        public virtual ICollection<RubrosCN> RubrosCN { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosBicicletas
    {
        public RubrosBicicletas()
        {
            this.RubrosCN = new HashSet<RubrosCN>();
        }
    
        public int IdBicicleta { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<RubrosCN> RubrosCN { get; set; }
    }
}

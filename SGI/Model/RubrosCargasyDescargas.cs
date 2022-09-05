

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosCargasyDescargas
    {
        public RubrosCargasyDescargas()
        {
            this.RubrosCN = new HashSet<RubrosCN>();
        }
    
        public int IdCyD { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<RubrosCN> RubrosCN { get; set; }
    }
}

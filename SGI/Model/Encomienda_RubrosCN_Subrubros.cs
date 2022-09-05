

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_RubrosCN_Subrubros
    {
        public int Id_EncRubCNSubrubros { get; set; }
        public int Id_EncRubro { get; set; }
        public int Id_rubrosubrubro { get; set; }
    
        public virtual Encomienda_RubrosCN Encomienda_RubrosCN { get; set; }
        public virtual RubrosCN_Subrubros RubrosCN_Subrubros { get; set; }
    }
}

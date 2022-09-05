

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Gerencias
    {
        public ENG_Gerencias()
        {
            this.ENG_Circuitos = new HashSet<ENG_Circuitos>();
        }
    
        public int id_gerencia { get; set; }
        public string cod_gerencia { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Orden { get; set; }
    
        public virtual ICollection<ENG_Circuitos> ENG_Circuitos { get; set; }
    }
}

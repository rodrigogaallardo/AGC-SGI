

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RubrosCN_Subrubros
    {
        public RubrosCN_Subrubros()
        {
            this.Encomienda_RubrosCN_Subrubros = new HashSet<Encomienda_RubrosCN_Subrubros>();
        }
    
        public int Id_rubroCNsubrubro { get; set; }
        public int Id_rubroCN { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdGrupoCircuito { get; set; }
        public Nullable<System.DateTime> VigenciaDesde_subrubro { get; set; }
        public Nullable<System.DateTime> VigenciaHasta_subrubro { get; set; }
    
        public virtual ENG_Grupos_Circuitos ENG_Grupos_Circuitos { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_Subrubros> Encomienda_RubrosCN_Subrubros { get; set; }
        public virtual RubrosCN RubrosCN { get; set; }
    }
}

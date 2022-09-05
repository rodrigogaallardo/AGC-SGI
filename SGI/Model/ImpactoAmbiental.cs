

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ImpactoAmbiental
    {
        public ImpactoAmbiental()
        {
            this.CPadron_Rubros = new HashSet<CPadron_Rubros>();
            this.Encomienda_Rubros = new HashSet<Encomienda_Rubros>();
            this.Encomienda_Rubros_AT_Anterior = new HashSet<Encomienda_Rubros_AT_Anterior>();
            this.Rel_Rubros_ImpactoAmbiental = new HashSet<Rel_Rubros_ImpactoAmbiental>();
            this.Encomienda_RubrosCN = new HashSet<Encomienda_RubrosCN>();
            this.Encomienda_RubrosCN_AT_Anterior = new HashSet<Encomienda_RubrosCN_AT_Anterior>();
            this.Transf_Rubros = new HashSet<Transf_Rubros>();
            this.CPadron_RubrosCN = new HashSet<CPadron_RubrosCN>();
        }
    
        public int id_ImpactoAmbiental { get; set; }
        public string cod_ImpactoAmbiental { get; set; }
        public string nom_ImpactoAmbiental { get; set; }
        public string cod_Impacto { get; set; }
    
        public virtual ICollection<CPadron_Rubros> CPadron_Rubros { get; set; }
        public virtual ICollection<Encomienda_Rubros> Encomienda_Rubros { get; set; }
        public virtual ICollection<Encomienda_Rubros_AT_Anterior> Encomienda_Rubros_AT_Anterior { get; set; }
        public virtual ICollection<Rel_Rubros_ImpactoAmbiental> Rel_Rubros_ImpactoAmbiental { get; set; }
        public virtual ICollection<Encomienda_RubrosCN> Encomienda_RubrosCN { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_AT_Anterior> Encomienda_RubrosCN_AT_Anterior { get; set; }
        public virtual ICollection<Transf_Rubros> Transf_Rubros { get; set; }
        public virtual ICollection<CPadron_RubrosCN> CPadron_RubrosCN { get; set; }
    }
}

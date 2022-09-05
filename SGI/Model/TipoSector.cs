

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoSector
    {
        public TipoSector()
        {
            this.CPadron_Plantas = new HashSet<CPadron_Plantas>();
            this.Encomienda_Plantas = new HashSet<Encomienda_Plantas>();
            this.Transf_Plantas = new HashSet<Transf_Plantas>();
        }
    
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Ocultar { get; set; }
        public Nullable<bool> MuestraCampoAdicional { get; set; }
        public Nullable<int> TamanoCampoAdicional { get; set; }
    
        public virtual ICollection<CPadron_Plantas> CPadron_Plantas { get; set; }
        public virtual ICollection<Encomienda_Plantas> Encomienda_Plantas { get; set; }
        public virtual ICollection<Transf_Plantas> Transf_Plantas { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tipo_Documentacion_Req
    {
        public Tipo_Documentacion_Req()
        {
            this.CPadron_Rubros = new HashSet<CPadron_Rubros>();
            this.Encomienda_Rubros = new HashSet<Encomienda_Rubros>();
            this.Encomienda_Rubros_AT_Anterior = new HashSet<Encomienda_Rubros_AT_Anterior>();
            this.Rubros_Historial_Cambios = new HashSet<Rubros_Historial_Cambios>();
            this.Transf_Rubros = new HashSet<Transf_Rubros>();
            this.CPadron_RubrosCN = new HashSet<CPadron_RubrosCN>();
        }
    
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Documentos { get; set; }
        public string Otros { get; set; }
        public string TipoTramite { get; set; }
        public string Nomenclatura { get; set; }
    
        public virtual ICollection<CPadron_Rubros> CPadron_Rubros { get; set; }
        public virtual ICollection<Encomienda_Rubros> Encomienda_Rubros { get; set; }
        public virtual ICollection<Encomienda_Rubros_AT_Anterior> Encomienda_Rubros_AT_Anterior { get; set; }
        public virtual ICollection<Rubros_Historial_Cambios> Rubros_Historial_Cambios { get; set; }
        public virtual ICollection<Transf_Rubros> Transf_Rubros { get; set; }
        public virtual ICollection<CPadron_RubrosCN> CPadron_RubrosCN { get; set; }
    }
}



namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoActividad
    {
        public TipoActividad()
        {
            this.CPadron_Rubros = new HashSet<CPadron_Rubros>();
            this.Encomienda_Rubros = new HashSet<Encomienda_Rubros>();
            this.Encomienda_Rubros_AT_Anterior = new HashSet<Encomienda_Rubros_AT_Anterior>();
            this.Rubros_Historial_Cambios = new HashSet<Rubros_Historial_Cambios>();
            this.Encomienda_RubrosCN = new HashSet<Encomienda_RubrosCN>();
            this.Encomienda_RubrosCN_AT_Anterior = new HashSet<Encomienda_RubrosCN_AT_Anterior>();
            this.Transf_Rubros = new HashSet<Transf_Rubros>();
            this.SSIT_Solicitudes_RubrosCN = new HashSet<SSIT_Solicitudes_RubrosCN>();
            this.RubrosCN = new HashSet<RubrosCN>();
            this.CPadron_RubrosCN = new HashSet<CPadron_RubrosCN>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool CodigoNuevo { get; set; }
    
        public virtual ICollection<CPadron_Rubros> CPadron_Rubros { get; set; }
        public virtual ICollection<Encomienda_Rubros> Encomienda_Rubros { get; set; }
        public virtual ICollection<Encomienda_Rubros_AT_Anterior> Encomienda_Rubros_AT_Anterior { get; set; }
        public virtual ICollection<Rubros_Historial_Cambios> Rubros_Historial_Cambios { get; set; }
        public virtual ICollection<Encomienda_RubrosCN> Encomienda_RubrosCN { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_AT_Anterior> Encomienda_RubrosCN_AT_Anterior { get; set; }
        public virtual ICollection<Transf_Rubros> Transf_Rubros { get; set; }
        public virtual ICollection<SSIT_Solicitudes_RubrosCN> SSIT_Solicitudes_RubrosCN { get; set; }
        public virtual ICollection<RubrosCN> RubrosCN { get; set; }
        public virtual ICollection<CPadron_RubrosCN> CPadron_RubrosCN { get; set; }
    }
}

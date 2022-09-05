

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDePlanos
    {
        public TiposDePlanos()
        {
            this.Encomienda_Planos = new HashSet<Encomienda_Planos>();
        }
    
        public int id_tipo_plano { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public Nullable<bool> requiere_detalle { get; set; }
        public string extension { get; set; }
        public string acronimo_SADE { get; set; }
        public Nullable<int> tamanio_max_mb { get; set; }
    
        public virtual ICollection<Encomienda_Planos> Encomienda_Planos { get; set; }
    }
}

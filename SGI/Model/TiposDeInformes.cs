

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDeInformes
    {
        public TiposDeInformes()
        {
            this.SGI_Tarea_Carga_Tramite = new HashSet<SGI_Tarea_Carga_Tramite>();
        }
    
        public int id_tipo_informe { get; set; }
        public string Descripcion { get; set; }
        public string nombre { get; set; }
    
        public virtual ICollection<SGI_Tarea_Carga_Tramite> SGI_Tarea_Carga_Tramite { get; set; }
    }
}

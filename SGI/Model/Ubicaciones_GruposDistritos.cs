

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_GruposDistritos
    {
        public Ubicaciones_GruposDistritos()
        {
            this.Ubicaciones_CatalogoDistritos = new HashSet<Ubicaciones_CatalogoDistritos>();
        }
    
        public int IdGrupoDistrito { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string definicion { get; set; }
        public string referencia { get; set; }
    
        public virtual ICollection<Ubicaciones_CatalogoDistritos> Ubicaciones_CatalogoDistritos { get; set; }
    }
}

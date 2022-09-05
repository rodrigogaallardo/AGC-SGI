

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicaciones_CatalogoDistritos
    {
        public Ubicaciones_CatalogoDistritos()
        {
            this.Encomienda_Ubicaciones_Distritos = new HashSet<Encomienda_Ubicaciones_Distritos>();
            this.SSIT_Solicitudes_Ubicaciones_Distritos = new HashSet<SSIT_Solicitudes_Ubicaciones_Distritos>();
            this.Transf_Ubicaciones_Distritos = new HashSet<Transf_Ubicaciones_Distritos>();
            this.Ubicaciones_Distritos_temp = new HashSet<Ubicaciones_Distritos_temp>();
            this.Ubicaciones_Distritos = new HashSet<Ubicaciones_Distritos>();
            this.Ubicaciones_CatalogoDistritos_Zonas = new HashSet<Ubicaciones_CatalogoDistritos_Zonas>();
        }
    
        public int IdDistrito { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int IdGrupoDistrito { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> orden { get; set; }
    
        public virtual ICollection<Encomienda_Ubicaciones_Distritos> Encomienda_Ubicaciones_Distritos { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Ubicaciones_Distritos> SSIT_Solicitudes_Ubicaciones_Distritos { get; set; }
        public virtual ICollection<Transf_Ubicaciones_Distritos> Transf_Ubicaciones_Distritos { get; set; }
        public virtual ICollection<Ubicaciones_Distritos_temp> Ubicaciones_Distritos_temp { get; set; }
        public virtual ICollection<Ubicaciones_Distritos> Ubicaciones_Distritos { get; set; }
        public virtual Ubicaciones_GruposDistritos Ubicaciones_GruposDistritos { get; set; }
        public virtual ICollection<Ubicaciones_CatalogoDistritos_Zonas> Ubicaciones_CatalogoDistritos_Zonas { get; set; }
    }
}

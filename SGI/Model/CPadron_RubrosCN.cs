

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_RubrosCN
    {
        public int id_cpadronrubrocn { get; set; }
        public int id_cpadron { get; set; }
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string desc_rubro { get; set; }
        public bool EsAnterior { get; set; }
        public int id_tipoactividad { get; set; }
        public int id_tipodocreq { get; set; }
        public decimal SuperficieHabilitar { get; set; }
        public Nullable<int> id_ImpactoAmbiental { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
        public virtual ImpactoAmbiental ImpactoAmbiental { get; set; }
        public virtual RubrosCN RubrosCN { get; set; }
        public virtual Tipo_Documentacion_Req Tipo_Documentacion_Req { get; set; }
        public virtual TipoActividad TipoActividad { get; set; }
    }
}

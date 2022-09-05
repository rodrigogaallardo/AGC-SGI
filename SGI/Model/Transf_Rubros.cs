

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Rubros
    {
        public int id_transfrubro { get; set; }
        public int id_solicitud { get; set; }
        public string cod_rubro { get; set; }
        public string desc_rubro { get; set; }
        public bool EsAnterior { get; set; }
        public int id_tipoactividad { get; set; }
        public int id_tipodocreq { get; set; }
        public decimal SuperficieHabilitar { get; set; }
        public Nullable<int> id_ImpactoAmbiental { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ImpactoAmbiental ImpactoAmbiental { get; set; }
        public virtual Tipo_Documentacion_Req Tipo_Documentacion_Req { get; set; }
        public virtual TipoActividad TipoActividad { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}
